using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Webao.Attributes;

namespace Webao
{
    public abstract class AbstractAccessObject
    {
        private readonly IRequest req;
        public IDictionary<MethodInfo, IEnumerable<Attribute>> dic;

        protected AbstractAccessObject(IRequest req)
        {
            this.req = req;
        }

        public object Request(params object[] args)
        {
            StackTrace stackTrace = new StackTrace();
            MethodInfo callSite = (MethodInfo) stackTrace.GetFrame(1).GetMethod();

            /*
             * The callsite is the caller method.
             */

            //GetAttribute get = (GetAttribute)callSite.GetCustomAttribute(typeof(GetAttribute));
            IEnumerable<Attribute> attributes;
            GetAttribute get = null;
            MappingAttribute mapping = null;
            if (dic.TryGetValue(callSite, out attributes))
            {
                foreach (Attribute attr in attributes)
                {
                    if (attr.GetType().Equals(typeof(GetAttribute))) get = (GetAttribute)attr;
                    else mapping = (MappingAttribute)attr;
                }
            }
            else throw new ArgumentNullException("Method attributes missing");
           
            if(get != null)
            {
                String path = get.path;
                if(path != null)
                {
                    //MappingAttribute mapping = (MappingAttribute)callSite.GetCustomAttribute(typeof(MappingAttribute));
                    Type dest = mapping.dest;
                    if (mapping != null && dest != null)
                    {
                        foreach(object arg in args)
                        {
                            path = findAndReplace(path, arg+"");
                        }
                        Object dto = req.Get(path, dest);
                            
                        return GetDeepPropertyValue(dto, dest, mapping.path);
                    }
                     
                }
            }
            throw new ArgumentNullException();
        }

        public object GetDeepPropertyValue(object instance, Type t, string path)
        {
            var pp = path.Split('.');
            foreach (var prop in pp.Skip(1)) 
            {
                PropertyInfo propInfo = t.GetProperty(prop);
                if (propInfo != null)
                {
                    instance = propInfo.GetValue(instance, null);
                    t = propInfo.PropertyType;
                }
                else throw new ArgumentException("Properties path is not correct");
            }
            return instance;
        }

        public string findAndReplace(string str, string toReplace)
        {
            int start = str.IndexOf("{");
            int end = str.IndexOf("}", start);
            string aux = str.Substring(start, end - start+1);
            return str.Replace(aux, toReplace);
        }

    }
}
