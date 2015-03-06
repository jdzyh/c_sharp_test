using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngine
{
    /// <summary>
    /// 游戏启动类
    /// </summary>
    public sealed class CGameEngine
    {
        public static void Run(ICGame game)
        {
            if (game == null) {
                Console.WriteLine("Not yet init !");
                Console.ReadLine();
            } else {
                game.run();
            }
        }
    }
}
