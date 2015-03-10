using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CEngine
{
    /// <summary>
    /// 通用游戏类
    /// </summary>
    public abstract class CGame : ICGame
    {
        #region Api函数

        [DllImport("User32.dll")]
        private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        #endregion  
 
        #region 字段  
        /// <summary>  
        /// 控制台句柄  
        /// </summary>  
        private IntPtr m_hwnd = IntPtr.Zero;
        /// <summary>  
        /// 画面更新速率  
        /// </summary>  
        private Int32 m_updateRate;  
        /// <summary>  
        /// 当前帧数  
        /// </summary>  
        private Int32 m_fps;  
        /// <summary>  
        /// 记录帧数  
        /// </summary>  
        private Int32 m_tickcount;  
        /// <summary>  
        /// 记录上次运行时间  
        /// </summary>  
        private Int32 m_lastTime;  
        /// <summary>  
        /// 游戏是否结束  
        /// </summary>  
        private Boolean m_bGameOver;
        /// <summary>  
        /// 鼠标输入设备  
        /// </summary>  
        private CMouse m_dc_mouse;
        /// <summary>  
        /// 键盘输入设备  
        /// </summary>  
        private CKeyboard m_dc_keyboard;
        #endregion  
 
        #region 构造函数  
  
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        public CGame()  
        {  
            m_bGameOver = false;

            m_hwnd = FindWindow(null, getTitle());
            m_dc_mouse = new CMouse(m_hwnd);
            m_dc_keyboard = new CKeyboard();

            //订阅鼠标事件  
            m_dc_mouse.addMouseMoveEvent(gameMouseMove);
            m_dc_mouse.addMouseAwayEvent(gameMouseAway);
            m_dc_mouse.addMouseDownEvent(gameMouseDown);

            //订阅键盘事件  
            m_dc_keyboard.addKeyDownEvent(gameKeyDown);
            m_dc_keyboard.addKeyUpEvent(gameKeyUp);
        }  
 
        #endregion  
 
        #region 游戏运行函数  
        /// <summary>  
        /// 游戏输入  
        /// </summary>  
        private void gameInput()
        {
            //处理鼠标事件  
            this.getMouseDevice().mouseEventsHandler();
            //处理键盘事件  
            this.getKeyboardDevice().keyboardEventsHandler();
        }
        /// <summary>  
        /// 游戏初始化  
        /// </summary>  
        protected abstract void gameInit();  
        /// <summary>  
        /// 游戏逻辑  
        /// </summary>  
        protected abstract void gameLoop();  // 在游戏主循环刷新间隔内进行业务操作的函数
        /// <summary>  
        /// 游戏结束  
        /// </summary>  
        protected abstract void gameExit();  
 
        #endregion  
 
        #region 游戏设置函数  
  
        /// <summary>  
        /// 设置画面更新速率  
        /// </summary>  
        /// <param name="rate"></param>  
        protected void setUpdateRate(Int32 rate)  
        {  
            this.m_updateRate = rate;  
        }  
  
        /// <summary>  
        /// 获取画面更新率  
        /// </summary>  
        /// <returns></returns>  
        protected Int32 getUpdateRate()  
        {  
            return 1000 / this.m_updateRate;  
        }  
  
        /// <summary>  
        /// 获取FPS  
        /// </summary>  
        /// <returns></returns>  
        protected Int32 getFPS()  
        {  
            return this.m_fps;  
        }  
  
        /// <summary>  
        /// 计算FPS  
        /// </summary>  
        private void setFPS()  
        {  
            Int32 ticks = Environment.TickCount;  
            m_tickcount += 1;  
            if (ticks - m_lastTime >= 1000)  
            {  
                m_fps = m_tickcount;  
                m_tickcount = 0;  
                m_lastTime = ticks;  
            }  
        }  
  
        /// <summary>  
        /// 延迟  
        /// </summary>  
        private void delay()  
        {  
            this.delay(1);  
        }  
  
        protected void delay(Int32 time)  
        {  
            Thread.Sleep(time);  
        }  
  
        /// <summary>  
        /// 游戏结束  
        /// </summary>  
        /// <param name="gameOver"></param>  
        protected void setGameOver(Boolean gameOver)  
        {  
            this.m_bGameOver = gameOver;  
        }  
  
        /// <summary>  
        /// 游戏是否结束  
        /// </summary>  
        /// <returns></returns>  
        protected Boolean isGameOver()  
        {  
            return this.m_bGameOver;  
        }  
  
        /// <summary>  
        /// 设置光标是否可见  
        /// </summary>  
        /// <param name="visible"></param>  
        protected void setCursorVisible(Boolean visible)  
        {  
            Console.CursorVisible = visible;  
        }  
  
        /// <summary>  
        /// 设置控制台标题  
        /// </summary>  
        /// <param name="title"></param>  
        protected void setTitle(String title)  
        {  
            Console.Title = title;  
        }  
  
        /// <summary>  
        /// 获取控制台标题  
        /// </summary>  
        /// <returns></returns>  
        protected String getTitle()  
        {  
            return Console.Title;  
        }  
  
  
        /// <summary>  
        /// 关闭游戏并释放资源  
        /// </summary>  
        private void close()  
        {  
  
        }  
 
        #endregion  
        #region 游戏输入设备

        /// <summary>  
        /// 获取鼠标设备  
        /// </summary>  
        /// <returns></returns>  
        internal CMouse getMouseDevice()
        {
            return m_dc_mouse;
        }

        /// <summary>  
        /// 获取键盘设备  
        /// </summary>  
        /// <returns></returns>  
        internal CKeyboard getKeyboardDevice()
        {
            return m_dc_keyboard;
        }

        #endregion

        #region 游戏输入事件
        /// <summary>  
        /// 键盘按下虚拟函数  
        /// </summary>  
        /// <param name="e"></param>  
        protected virtual void gameKeyDown(CKeyboardEventArgs e)
        {
            //此处处理键盘按下事件  
        }

        /// <summary>  
        /// 键盘释放虚拟函数  
        /// </summary>  
        /// <param name="e"></param>  
        protected virtual void gameKeyUp(CKeyboardEventArgs e)
        {
            //此处处理键盘释放事件  
        }
        /// <summary>  
        /// 鼠标移动虚拟函数  
        /// </summary>  
        /// <param name="e"></param>  
        protected virtual void gameMouseMove(CMouseEventArgs e)
        {
            //此处处理鼠标移动事件  
        }

        /// <summary>  
        /// 鼠标离开虚拟函数  
        /// </summary>  
        /// <param name="e"></param>  
        protected virtual void gameMouseAway(CMouseEventArgs e)
        {
            //此处处理鼠标离开事件  
        }

        /// <summary>  
        /// 鼠标按下虚拟函数  
        /// </summary>  
        /// <param name="e"></param>  
        protected virtual void gameMouseDown(CMouseEventArgs e)
        {
            //此处处理鼠标按下事件  
        }



        #endregion

        #region 游戏启动接口

        /// <summary>  
        /// 游戏运行  
        /// </summary>  
        public void run()
        {
            //游戏初始化  
            this.gameInit();

            Int32 startTime = 0;
            while (!this.isGameOver())
            {
                //启动计时  
                startTime = Environment.TickCount;
                //计算fps  
                this.setFPS();
                //游戏输入  
                this.gameInput();
                //游戏逻辑  
                this.gameLoop();
                //保持一定的FPS  
                while (Environment.TickCount - startTime < this.m_updateRate)
                {
                    this.delay();
                }
            }

            //游戏退出  
            this.gameExit();
            //释放游戏资源  
            this.close();
        }

        #endregion  
    }  
}
