using System;
using System.Collections.Generic;

namespace Webao
{
    public class InterfaceAttributeInfo<T>
    {
        private string host;
        private Dictionary<String, String> parameters;
        private MethodAttributeInfo<T> method_attrs_info;

        public InterfaceAttributeInfo(string host)
        { 
            this.host = host;
            this.parameters = new Dictionary<string, string>();
            this.method_attrs_info = new MethodAttributeInfo<T>(this);
        }

        public InterfaceAttributeInfo<T> AddParameter(String name, String val)
        {
            parameters.Add(name, val);
            return this;    
        }

        public MethodAttributeInfo<T> On(String method_name)
        {
            AttrInfoContainer attr_info = new AttrInfoContainer();
            method_attrs_info.attribute_info.Add(method_name, attr_info);
            method_attrs_info.Curr_attr_info = attr_info;
            method_attrs_info.Curr_method_name = method_name;
            return method_attrs_info;
        }

        public T Build(IRequest req)
        {
            return (T)WebaoDynBuilder.Build(typeof(T), req, host, parameters,
                method_attrs_info.attribute_info, method_attrs_info.mappers);
        }

    }
}