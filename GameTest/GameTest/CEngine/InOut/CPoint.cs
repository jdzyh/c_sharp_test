using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGraphics
{
    /// <summary>  
    /// 位置结构  
    /// </summary>  
    public struct CPoint {
        private Int32 m_x;
        private Int32 m_y;

        public CPoint(Int32 x, Int32 y) {
            this.m_x = x;
            this.m_y = y;
        }

        public Int32 getX() {
            return this.m_x;
        }

        public Int32 getY() {
            return this.m_y;
        }

        public void setX(Int32 x) {
            this.m_x = x;
        }

        public void setY(Int32 y) {
            this.m_y = y;
        }

        public static Boolean operator ==(CPoint p1, CPoint p2) {
            return (p1.m_x == p2.m_x) && (p1.m_y == p2.m_y);
        }

        public static Boolean operator !=(CPoint p1, CPoint p2) {
            return (p1.m_x != p2.m_x) || (p1.m_y != p2.m_y);
        }

        public override bool Equals(object obj) {
            return this == (CPoint)obj;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public static CPoint operator -(CPoint p1, CPoint p2) {
            return new CPoint(p1.getX() - p2.getX(), p1.getY() - p2.getY());
        }

        public static CPoint operator +(CPoint p1, CPoint p2) {
            return new CPoint(p1.getX() + p2.getX(), p1.getY() + p2.getY());
        }

        public override string ToString() {
            return string.Format("[{0},{1}]", m_x, m_y);
        }
    }  
}
