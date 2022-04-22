using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebaoBench
{
    class test
    {



        public static void testdel()
        {
            Delegate del;

            Func<String, String> func1 = msg =>
            {
                Console.WriteLine(msg);
                return msg;
            };

            func1.Invoke("delegate");

            del = func1;

            object[] param = { "Delegate" };
            del.Method.Invoke(null, param);
        }
    }
}
