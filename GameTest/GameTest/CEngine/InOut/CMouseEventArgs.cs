using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEngine
{
    /// <summary>  
    /// 鼠标事件参数  
    /// </summary>  
    public sealed class CMouseEventArgs : EventArgs {
        private Int32 m_x;
        private Int32 m_y;
        private Boolean m_leave;
        private CMouseButtons m_vKey;

        public CMouseEventArgs(Int32 x, Int32 y, Boolean leave) {
            this.m_x = x;
            this.m_y = y;
            this.m_leave = leave;
        }

        public CMouseEventArgs(Int32 x, Int32 y, CMouseButtons key) {
            this.m_x = x;
            this.m_y = y;
            this.m_vKey = key;
        }

        public Int32 getX() {
            return m_x;
        }

        public Int32 getY() {
            return m_y;
        }

        public Boolean isLeave() {
            return m_leave;
        }

        public CMouseButtons getKey() {
            return m_vKey;
        }

        public bool containKey(CMouseButtons key) {
            return (getKey() & key) == key;
        }

        public override string ToString() {
            return string.Format("{0},{1}", getX(), getY());
        }
    } 
}
