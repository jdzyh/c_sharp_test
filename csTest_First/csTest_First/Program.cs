using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*快捷键：
** CTRL+F5 调试运行
** CTRL+j  补全代码
**
*/

namespace csTest_First
{
    class Program
    {   enum Test {AAAA=1, BBBB, CCCC};

        static void Main(string[] args)
        {
            Test t = Test.BBBB;
            int x = (int)Test.BBBB;

            Console.WriteLine("{0}, {1}", t, x);
        }
    }
}
