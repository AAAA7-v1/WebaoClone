using System;

namespace Webao.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MappingAttribute : Attribute
    {
        public readonly string path;
        public readonly Type dest;

        public MappingAttribute(Type dest, string path)
        {
            this.dest = dest;
            this.path = path;
        }
 
       //added for TP3

        public MappingAttribute(Type dest)
        {
            this.dest = dest;
        }

        public string With {
            set; get;
        }

    }
}
