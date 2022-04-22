using System;
using System.Security.Cryptography;

namespace Webao
{
    public class AttrInfoContainer
    {
        public String Get_from { set; get; }
        public Type Dest { set; get; }
        public String Path { set; get; }
        public String With { set; get; }

        public AttrInfoContainer()
        {

        }

        public AttrInfoContainer(String get_from, Type dest, String path, String with)
        {         
            this.Get_from = get_from;
            this.Dest = dest;
            this.Path = path;
            this.With = with;
        }
    }
}