using System;
using System.Collections.Generic;

namespace Webao
{
    public class MethodAttributeInfo<T>
    {
        private InterfaceAttributeInfo<T> parent_interface;
        public Dictionary<String, AttrInfoContainer> attribute_info;
        public Dictionary<String, Delegate> mappers;

        public AttrInfoContainer Curr_attr_info { set; get; }
        public String Curr_method_name { set; get; }

        public MethodAttributeInfo(InterfaceAttributeInfo<T> parent_interface)
        {
            this.parent_interface = parent_interface;
            attribute_info = new Dictionary<string, AttrInfoContainer>();
            mappers = new Dictionary<string, Delegate>();
        }

        public MethodAttributeInfo<T> GetFrom(string path)
        {   
            if (Curr_attr_info != null)
            {
                Curr_attr_info.Get_from = path;
            }
            return this;
        }

        public InterfaceAttributeInfo<T> Mapping<R>(Func<R, object> mapper)
        {
            if (Curr_attr_info != null && Curr_attr_info.Get_from != null)
            {
                mappers.Add(Curr_method_name, mapper);
                Curr_attr_info.Dest = typeof(R);
                Curr_attr_info = null;
                Curr_method_name = null;
                return parent_interface;
            }
            return null;
        }
    }
}