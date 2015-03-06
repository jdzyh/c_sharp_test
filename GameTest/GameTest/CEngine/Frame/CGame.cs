using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CEngine
{
    /// <summary>
    /// 通用游戏类
    /// </summary>
    public abstract class CGame : ICGame
    {
        #region 字段  
  
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
 
        #endregion  
 
        #region 构造函数  
  
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        public CGame()  
        {  
            m_bGameOver = false;  
        }  
 
        #endregion  
 
        #region 游戏运行函数  
  
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
                //游戏逻辑  
                this.gameLoop();  
                //保持一定的FPS 
                //--保持, 直到：该帧的运行时间与该帧的开始时间相差 设定的刷新间隔
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
