using System;
using System.Collections.Generic;
using System.Reflection;
using Webao.Attributes;

namespace Webao
{
    public class WebaoBuilder
    {
        public static AbstractAccessObject Build(Type webao, IRequest req)
        {   
            BaseUrlAttribute baseUrl = (BaseUrlAttribute)Attribute.GetCustomAttribute(webao, typeof(BaseUrlAttribute));
            if (baseUrl != null && baseUrl.host != null) req.BaseUrl(baseUrl.host);

            AddParameterAttribute[] addParameter = (AddParameterAttribute[])Attribute.GetCustomAttributes(webao, typeof(AddParameterAttribute));
            foreach (AddParameterAttribute parameterAttribute in addParameter)
            {
                if (parameterAttribute != null)
                {
                    String paramName = parameterAttribute.name, paramVal = parameterAttribute.val;
                    if (paramName != null && paramVal != null) req.AddParameter(paramName, paramVal);
                }
            }

            MethodInfo[] methods = webao.GetMethods();
            Dictionary<MethodInfo, IEnumerable<Attribute>> methodAttributes = new Dictionary<MethodInfo, IEnumerable<Attribute>>();
                    
            foreach(MethodInfo method in methods)
            {
                methodAttributes.Add(method, method.GetCustomAttributes());
            }

            AbstractAccessObject toReturn = (AbstractAccessObject)Activator.CreateInstance(webao, req);
            toReturn.dic = methodAttributes;
                    return toReturn;
 
        }
    }
}
 