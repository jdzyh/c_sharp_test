using CGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngine
{
    /// <summary>  
    /// 鼠标键值  
    /// </summary>  
    [Flags] // 支持按位进行运算(同时满足多个状态)
    public enum CMouseButtons {
        Left = 0x01,
        Middle = 0x04,
        None = 0,
        Right = 0x02
    }

    /// <summary>  
    /// 鼠标类  
    /// </summary>  
    internal sealed class CMouse : CInput {
        /// <summary>  
        /// 鼠标事件委托  
        /// </summary>  
        /// <typeparam name="TEventArgs"></typeparam>  
        /// <param name="e"></param>  
        internal delegate void CMouseHandler<TEventArgs>(TEventArgs e); // 申明 鼠标事件处理 委托类型
        /// <summary>  
        /// 鼠标移动事件  
        /// </summary>  
        private event CMouseHandler<CMouseEventArgs> m_mouseMove; // 事件 : move
        /// <summary>  
        /// 鼠标离开事件  
        /// </summary>  
        private event CMouseHandler<CMouseEventArgs> m_mouseAway; // 事件 : away (?)
        /// <summary>  
        /// 鼠标按下事件  
        /// </summary>  
        private event CMouseHandler<CMouseEventArgs> m_mouseDwon; // 事件 : down

        /// <summary>  
        /// 最大X值  
        /// </summary>  
        private readonly Int32 MAX_X = 639;
        /// <summary>  
        /// 最大Y值  
        /// </summary>  
        private readonly Int32 MAX_Y = 400;

        /// <summary>  
        /// 控制台句柄  
        /// </summary>  
        private IntPtr m_hwnd = IntPtr.Zero; // ?
        /// <summary>  
        /// 鼠标离开工作区范围前的位置  
        /// </summary>  
        private CPoint m_oldPoint;
        /// <summary>  
        /// 鼠标是否离开工作区范围  
        /// </summary>  
        private Boolean m_leave;

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        public CMouse(IntPtr hwnd) {
            this.m_hwnd = hwnd;
            this.m_oldPoint = new CPoint(0, 0);
            this.m_leave = false;

            this.MAX_X = (Console.WindowWidth << 3) - 1;
            this.MAX_Y = Console.WindowHeight << 4;
        }

        #region 鼠标函数

        /// <summary>  
        /// 是否按下鼠标  
        /// </summary>  
        /// <param name="vKey"></param>  
        /// <returns></returns>  
        private Boolean isMouseDown(CMouseButtons vKey) {
            return 0 != (GetAsyncKeyState((Int32)vKey) & KEY_STATE); // 判断鼠标按键状态
        }

        /// <summary>  
        /// 获取鼠标当前按下的键值  
        /// </summary>  
        /// <returns></returns>  
        private CMouseButtons getCurMouseDownKeys() {
            CMouseButtons vKey = CMouseButtons.None;
            foreach (Int32 key in Enum.GetValues(typeof(CMouseButtons))) { // 遍历，确定按下的键
                if (isMouseDown((CMouseButtons)key)) {
                    //可以同时按下多个键  
                    vKey |= (CMouseButtons)key;
                }
            }
            return vKey;
        }

        /// <summary>  
        /// 获取鼠标列坐标  
        /// </summary>  
        /// <returns></returns>  
        private Int32 getMouseX() {
            return getMousePoint().getX();
        }

        /// <summary>  
        /// 获取鼠标行坐标  
        /// </summary>  
        /// <returns></returns>  
        private Int32 getMouseY() {
            return getMousePoint().getY();
        }

        /// <summary>  
        /// 是否离开工作区  
        /// </summary>  
        /// <returns></returns>  
        private Boolean isLeave() {
            return m_leave;
        }

        /// <summary>  
        /// 获取鼠标坐标  
        /// </summary>  
        /// <returns></returns>  
        private CPoint getMousePoint() {
            CPoint point;
            //获取鼠标在屏幕的位置  
            /**
             * GetCursorPos : https://msdn.microsoft.com/en-us/library/windows/desktop/ms648390(v=vs.85).aspx
             * Returns nonzero if successful or zero otherwise.
             */
            if (GetCursorPos(out point)) { 
                if (m_hwnd != IntPtr.Zero) {
                    //把屏幕位置转换成控制台工作区位置   
                    ScreenToClient(m_hwnd, out point);

                    if ((point.getX() >= 0 && point.getX() <= MAX_X)
                        && point.getY() >= 0 && point.getY() <= MAX_Y) {
                        this.m_oldPoint = point;
                        this.m_leave = false;
                    } else {
                        m_leave = true;
                    }
                }
            }
            return m_oldPoint;
        }

        #endregion

        #region 鼠标事件

        /// <summary>  
        /// 响应鼠标移动事件  
        /// </summary>  
        /// <param name="e"></param>  
        private void onMouseMove(CMouseEventArgs e) {
            CMouseHandler<CMouseEventArgs> temp = m_mouseMove;
            if (temp != null) {
                temp.Invoke(e);
            }
        }

        /// <summary>  
        /// 响应鼠标离开事件  
        /// </summary>  
        /// <param name="e"></param>  
        private void onMouseAway(CMouseEventArgs e) {
            CMouseHandler<CMouseEventArgs> temp = m_mouseAway;
            if (temp != null) {
                temp.Invoke(e);
            }
        }

        /// <summary>  
        /// 响应鼠标按下事件  
        /// </summary>  
        /// <param name="e"></param>  
        private void onMouseDown(CMouseEventArgs e) {
            CMouseHandler<CMouseEventArgs> temp = m_mouseDwon;
            if (temp != null) {
                temp.Invoke(e);
            }
        }

        /// <summary>  
        /// 添加鼠标移动事件  
        /// </summary>  
        /// <param name="func"></param>  
        public void addMouseMoveEvent(CMouseHandler<CMouseEventArgs> func) {
            m_mouseMove += func;
        }

        /// <summary>  
        /// 添加鼠标离开事件  
        /// </summary>  
        /// <param name="func"></param>  
        public void addMouseAwayEvent(CMouseHandler<CMouseEventArgs> func) {
            m_mouseAway += func;
        }

        /// <summary>  
        /// 添加鼠标按下事件  
        /// </summary>  
        /// <param name="func"></param>  
        public void addMouseDownEvent(CMouseHandler<CMouseEventArgs> func) {
            m_mouseDwon += func;
        }

        /// <summary>  
        ///鼠标事件处理  
        /// </summary>  
        public void mouseEventsHandler() {
            CMouseEventArgs e;

            CPoint point = getMousePoint();

            CMouseButtons vKey = getCurMouseDownKeys();
            if (!isLeave()) {
                if (vKey != CMouseButtons.None) {
                    e = new CMouseEventArgs(point.getX(), point.getY(), vKey);
                    this.onMouseDown(e);
                }

                e = new CMouseEventArgs(point.getX(), point.getY(), false);
                this.onMouseMove(e);
            } else {
                e = new CMouseEventArgs(-1, -1, true);
                this.onMouseAway(e);
            }
        }

        #endregion
    }  
}
