using CEngine;
using System;

namespace Game
{
    public class TestGame : CGame
    {
        #region 框架测试 v0.2
        /*
        private Int32 m_ticks;
        private Int32 m_lasttime;

        /// <summary>  
        /// 游戏初始化  
        /// </summary>  
        protected override void gameInit() {
            //设置游戏标题  
            setTitle("游戏框架测试");
            //设置游戏画面刷新率 每毫秒一次  
            setUpdateRate(1000);
            //设置光标隐藏  
            setCursorVisible(false);

            Console.WriteLine("游戏初始化成功!");

            m_lasttime = Environment.TickCount;
        }

        /// <summary>  
        /// 游戏逻辑  
        /// </summary>  
        protected override void gameLoop() {
            if (m_ticks++ < 15) {
                Console.WriteLine(string.Format("  游戏运行中,第{0}帧,耗时{1}ms", m_ticks, Environment.TickCount - m_lasttime));
                m_lasttime = Environment.TickCount;
            } else {
                setGameOver(true);
            }
        }

        /// <summary>  
        /// 游戏结束  
        /// </summary>  
        protected override void gameExit() {
            Console.WriteLine("游戏结束!");
            Console.Read();
        }
        */
        #endregion

        //#region 框架测试 v0.3
        private Boolean m_bkeydown = false;

        /// <summary>  
        /// 游戏初始化  
        /// </summary>  
        protected override void gameInit()
        {
            //设置游戏标题  
            setTitle("游戏框架测试");
            //设置游戏画面刷新率 每毫秒一次  
            setUpdateRate(30);
            //设置光标隐藏  
            setCursorVisible(false);

            Console.WriteLine("游戏初始化成功!");
        }

        /// <summary>  
        /// 游戏逻辑  
        /// </summary>  
        protected override void gameLoop()
        {

        }

        /// <summary>  
        /// 游戏结束  
        /// </summary>  
        protected override void gameExit()
        {
            Console.WriteLine("游戏结束!");
            Console.ReadLine();
        }

        protected override void gameKeyDown(CKeyboardEventArgs e)
        {
            if (!m_bkeydown)
            {
                Console.WriteLine("按下键：" + e.getKey());

                m_bkeydown = true;
            }

            if (e.getKey() == CKeys.Escape)
            {
                setGameOver(true);
            }
        }

        protected override void gameKeyUp(CKeyboardEventArgs e)
        {
            Console.WriteLine("释放键：" + e.getKey());
            m_bkeydown = false;
        }

        protected override void gameMouseAway(CMouseEventArgs e)
        {
            setTitle("鼠标离开了工作区!");
        }

        protected override void gameMouseDown(CMouseEventArgs e)
        {
            if (e.getKey() == CMouseButtons.Left)
            {
                Console.SetCursorPosition(15, 2);
                Console.WriteLine("鼠标工作区坐标：" + e.ToString() + "  " + e.getKey().ToString());
            }
            else if (e.getKey() == CMouseButtons.Right)
            {
                Console.SetCursorPosition(15, 3);
                Console.WriteLine("鼠标工作区坐标：" + e.ToString() + "  " + e.getKey().ToString());
            }
            else if (e.getKey() == CMouseButtons.Middle)
            {
                Console.SetCursorPosition(15, 4);
                Console.WriteLine("鼠标工作区坐标：" + e.ToString() + "  " + e.getKey().ToString());
            }
        }

        protected override void gameMouseMove(CMouseEventArgs e)
        {
            setTitle("鼠标回到了工作区!");
        }
        //#endregion 
    }
}
