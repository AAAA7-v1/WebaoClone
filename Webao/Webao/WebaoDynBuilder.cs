using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Webao.Attributes;
using System.Reflection.Emit;
using System.IO.MemoryMappedFiles;

namespace Webao
{
    public class WebaoDynBuilder
    {

        public static Object Build(Type webaoInterface, IRequest req)
        {
            // Aux memory structures
            Dictionary<String, AttrInfoContainer> methods_attr_infos = new Dictionary<string, AttrInfoContainer>();
            Dictionary<String, String> parameters = new Dictionary<string, string>();

            // Read custom attributes
            BaseUrlAttribute baseUrl = (BaseUrlAttribute)Attribute.GetCustomAttribute(webaoInterface, typeof(BaseUrlAttribute));
            String host = baseUrl.host;

            AddParameterAttribute[] addParameters = (AddParameterAttribute[])Attribute.GetCustomAttributes(webaoInterface, typeof(AddParameterAttribute));
            foreach (AddParameterAttribute parameter in addParameters)
            {
                parameters.Add(parameter.name, parameter.val);
            }

            MethodInfo[] methods = webaoInterface.GetMethods();
            foreach (MethodInfo method in methods)
            {
                GetAttribute getAttr = (GetAttribute)Attribute.GetCustomAttribute(method, typeof(GetAttribute));
                MappingAttribute mapAttr = (MappingAttribute)Attribute.GetCustomAttribute(method, typeof(MappingAttribute));
                methods_attr_infos.Add(method.Name,
                    new AttrInfoContainer(getAttr.path, mapAttr.dest, mapAttr.path, mapAttr.With));
            }

            return Build(webaoInterface, req, host, parameters, methods_attr_infos, null);
        }

        public static Object Build(Type webaoInterface, IRequest req,
            String host, Dictionary<String, String> parameters,
            Dictionary<String, AttrInfoContainer> methods_attr_infos,
            Dictionary<String, Delegate> mappers)
        {
            // Build type
            Type dynWebao = BuildType(webaoInterface, methods_attr_infos, mappers);

            // Setup req
            req.BaseUrl(host);
            foreach (KeyValuePair<String, String> parameter in parameters)
            {
                req.AddParameter(parameter.Key, parameter.Value);
            }

            // Return the instance of the dynamically generated class.
            return Activator.CreateInstance(dynWebao, req, mappers);
        }

        public static Type BuildType(Type webaoInterface,
            Dictionary<String, AttrInfoContainer> methods_attr_infos,
            Dictionary<String, Delegate> mappers)
        {
            AssemblyName aName = new AssemblyName(webaoInterface.Name.Remove(0,1));
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder mb =
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            //ab.DefineDynamicModule(aName.Name);

            TypeBuilder tb = mb.DefineType(
                aName.Name,
                TypeAttributes.Public);
            tb.AddInterfaceImplementation(webaoInterface);

            // Add a private field of type IRequest to store req.
            FieldBuilder fbReq = tb.DefineField(
                "req",
                typeof(IRequest),
                FieldAttributes.Private);

            // Add a private field of type IRequest to store req.
            FieldBuilder fbMappers = tb.DefineField(
                "mappers",
                typeof(Dictionary<String, Delegate>),
                FieldAttributes.Private);

            // Build constructor.
            BuildCtor(tb, fbReq, fbMappers);

            // Lets build the interface methods!
            MethodInfo[] methods = webaoInterface.GetMethods();
            foreach (MethodInfo method in methods)
            {
                Delegate mapper = mappers == null ? null : mappers[method.Name];
                BuildMethod(tb, webaoInterface, fbReq, fbMappers,
                    method, methods_attr_infos[method.Name], mapper);
            }

            Type t = tb.CreateType();

            // The following line saves the single-module assembly. This
            // requires AssemblyBuilderAccess to include Save. You can now
            // type "ildasm MyDynamicAsm.dll" at the command prompt, and 
            // examine the assembly. You can also write a program that has
            // a reference to the assembly, and use the MyDynamicType type.
            // 
            ab.Save(aName.Name + ".dll");
            return t;
        }


        static void BuildCtor(TypeBuilder tb, FieldInfo fldReq, FieldInfo fldMappers)
        {
            // Define a constructor that takes an IRequest argument and 
            // a Dictionary<String, Func<object, object>> stores it in the private field. 
            Type[] parameterTypes = { typeof(IRequest), typeof(Dictionary<String, Delegate>) };
            ConstructorBuilder ctor = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                parameterTypes);

            ILGenerator ctorIL = ctor.GetILGenerator();

            //IL EMIT the constructor

            // For a constructor, argument zero is a reference to the new
            // instance. Push it on the stack before calling the base
            // class constructor. Specify the default constructor of the 
            // base class (System.Object) by passing an empty array of 
            // types (Type.EmptyTypes) to GetConstructor.
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call,
                typeof(object).GetConstructor(Type.EmptyTypes));
            // Push the instance on the stack before pushing the argument
            // that is to be assigned to the private field req.

            // Assign fldReq to private field of type IRequest
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, fldReq);

            // Assign fldMappers to private field of type Dictionary<String, Delegate>
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, fldMappers);
                
            ctorIL.Emit(OpCodes.Ret);
        }

        static void BuildMethod(TypeBuilder tb, Type webaoInterface, FieldInfo fldReq, FieldInfo fldMappers,
            MethodInfo method, AttrInfoContainer method_attr_info, Delegate mapper)

        {
            ParameterInfo[] parameters = method.GetParameters();
            MethodBuilder meth = tb.DefineMethod(
               method.Name,
               MethodAttributes.Public | MethodAttributes.Virtual,
               method.ReturnType,
               parameters.Select(p => p.ParameterType).ToArray());
            tb.DefineMethodOverride(meth, webaoInterface.GetMethod(method.Name));

            ILGenerator methIL = meth.GetILGenerator();
            String method_name = method.Name;

            // Processing request path
            PathProcessing(methIL, method_attr_info.Get_from, parameters);

            // Getting the respective Dto
            Type dto = method_attr_info.Dest;
            GetDto(methIL, dto, fldReq);

            // If configured via custom attribute and with.
            if (method_attr_info.With != null)
            {
                String with = method_attr_info.With;
                GetDeepPropertyValWith(methIL, dto, with);

            }
            // Else if configures via custom attribute and path.
            else if (method_attr_info.Path != null)
            {
                String propertyPath = method_attr_info.Path;
                GetDeepPropertyValue(methIL, dto, propertyPath);
            }
            // Else was configured through fluent api methods.
            else
            {
                // Call mapper function
                GetMapped(methIL, dto, method.ReturnType, fldMappers, method_name, mapper);
            }
            

            // After we're done
            methIL.Emit(OpCodes.Ret);
        }

        static readonly MethodInfo OBJ_TOSTRING = typeof(Object).GetMethod("ToString");
        static readonly MethodInfo REPLACE = typeof(String).GetMethod("Replace", new Type[] { typeof(String), typeof(String) });
        static void PathProcessing(ILGenerator methIL, String path, ParameterInfo[] parameters)
        {
            List<String> toSubstitute = FindAndGet(path); // Get all instances of what we should replace.

            if (toSubstitute.Count != 0)
            {              
                methIL.Emit(OpCodes.Ldstr, path);
                int idx = 0;
                foreach (String placeholder in toSubstitute)
                {
                    ParameterInfo parameter = parameters[idx];

                    methIL.Emit(OpCodes.Ldstr, placeholder);

                    if (idx++ == 0) methIL.Emit(OpCodes.Ldarg_1);
                    else methIL.Emit(OpCodes.Ldarga_S, idx);

  
                    if (parameter.ParameterType.IsValueType) methIL.Emit(OpCodes.Call, parameter.ParameterType.GetMethod("ToString", Type.EmptyTypes));
                    else methIL.Emit(OpCodes.Callvirt, OBJ_TOSTRING);

                    methIL.Emit(OpCodes.Callvirt, REPLACE);
                }
            }
        }

        static readonly MethodInfo GET_TYPE_FROM_HANDLE = typeof(Type).GetMethod("GetTypeFromHandle");
        static readonly MethodInfo REQUEST_GET = typeof(IRequest).GetMethod("Get");
        static void GetDto(ILGenerator methIL, Type dto, FieldInfo fldReq)
        {   
            methIL.DeclareLocal(typeof(String)); // Declare local variable to hold path.
            methIL.Emit(OpCodes.Stloc_0); // Pop path to local var
            methIL.Emit(OpCodes.Ldarg_0);
            methIL.Emit(OpCodes.Ldfld, fldReq);
            methIL.Emit(OpCodes.Ldloc_0); // Push path to stack
            methIL.Emit(OpCodes.Ldtoken, dto); // Push dtoType to stack
            methIL.Emit(OpCodes.Call, GET_TYPE_FROM_HANDLE);
            methIL.Emit(OpCodes.Callvirt, REQUEST_GET);
            if (dto.IsValueType) methIL.Emit(OpCodes.Unbox_Any, dto); // If the dto we are working with is a Value Type
            else methIL.Emit(OpCodes.Castclass, dto); // Else if its a Reference Type
        }

        static void GetDeepPropertyValue(ILGenerator methIL, Type currType, String path)
        {
            var properties = path.Split('.');
          
            int loc = 1;
            for (int i = 1; i < properties.Length; ++i)
            {
                String property = properties[i];
                MethodInfo GET = currType.GetMethod("get_" + property);

                if (currType.IsValueType)
                {
                    methIL.DeclareLocal(currType);
                    methIL.Emit(OpCodes.Stloc, loc);
                    methIL.Emit(OpCodes.Ldloca_S, loc);
                    methIL.Emit(OpCodes.Call, GET);
                    ++loc;
                }
                else methIL.Emit(OpCodes.Callvirt, GET);
                currType = currType.GetProperty(property).PropertyType;
            }
        }

        static void GetDeepPropertyValWith(ILGenerator methIL, Type dto, String with)
        {
           
            String withMethod = with.Substring(with.LastIndexOf(".") + 1, with.Length - with.LastIndexOf(".") - 1);
            String withClass = with.Substring(0, with.LastIndexOf("."));

            Type classType = dto.Assembly.GetType(withClass);

            MethodInfo GET = classType.GetMethod(withMethod);

            if (classType.IsValueType)
            {
                methIL.DeclareLocal(classType);
                methIL.Emit(OpCodes.Stloc, 1);
                methIL.Emit(OpCodes.Ldloca_S, 1);
                methIL.Emit(OpCodes.Call, GET);
            }
            else
            {
                methIL.Emit(OpCodes.Callvirt, GET);
            }

        }

        static readonly MethodInfo GET_TARGET = typeof(Delegate).GetMethod("get_Target");
        static readonly MethodInfo GET_ITEM = typeof(Dictionary<String, Delegate>).GetMethod("get_Item");
        static void GetMapped(ILGenerator methIL, Type dto, Type method_return_type,
            FieldInfo fldMappers, String method_name, Delegate mapper)
        {
            MethodInfo mapper_info = mapper.Method;

            // If method from delegate is not accessible, we need to invoke through delegate.
            if (!mapper_info.IsPublic)
            {
                // Pop dto into local var temporarily.
                methIL.DeclareLocal(dto);
                methIL.Emit(OpCodes.Stloc, 1);

                // Load dictionary and get delegate from it
                methIL.Emit(OpCodes.Ldarg_0);
                methIL.Emit(OpCodes.Ldfld, fldMappers);
                methIL.Emit(OpCodes.Ldstr, method_name);
                methIL.Emit(OpCodes.Call, GET_ITEM);

                Type del_type = mapper.GetType();
                MethodInfo invoke = del_type.GetMethod("Invoke");

                methIL.Emit(OpCodes.Castclass, del_type);
                methIL.Emit(OpCodes.Ldloc, 1);
                methIL.Emit(OpCodes.Callvirt, invoke);
            }
            else // If we can access it, then we can call through Delegate.Method.
            {   
                // If instance method, we need the target to call it on.
                if (!mapper_info.IsStatic)
                {
                    // Pop dto into local var temporarily.
                    methIL.DeclareLocal(dto);
                    methIL.Emit(OpCodes.Stloc, 1);

                    // Load dictionary and get delegate from it
                    methIL.Emit(OpCodes.Ldarg_0);
                    methIL.Emit(OpCodes.Ldfld, fldMappers);
                    methIL.Emit(OpCodes.Ldstr, method_name);
                    methIL.Emit(OpCodes.Call, GET_ITEM);

                    // Get target
                    methIL.Emit(OpCodes.Call, GET_TARGET);

                    // Push dto back into the evaluation stack before calling the mapper method.
                    methIL.Emit(OpCodes.Ldloc, 1);
                }

                methIL.Emit(OpCodes.Call, mapper_info);
            }

            // Cast / Unbox
            if (method_return_type.IsValueType) methIL.Emit(OpCodes.Unbox_Any, method_return_type);
            else methIL.Emit(OpCodes.Castclass, method_return_type);
        }

        static List<String> FindAndGet(String toSearch)
        {
            List<String> toReturn = new List<string>();

            for(int i = 0; i < toSearch.Length; ++i)
            {
                if(toSearch[i] == '{')
                {
                    int start = i;
                    while (i < toSearch.Length && toSearch[i] != '}') ++i;
                    if (i != toSearch.Length)
                    {
                        int end = i;
                        toReturn.Add(toSearch.Substring(start, end-start+1));
                    }
                }
            }
            return toReturn;
        }

        public static InterfaceAttributeInfo<T> For<T>(string host)
        {
            return new InterfaceAttributeInfo<T>(host);
        }
    }
}
