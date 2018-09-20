using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.Container
{
    /// <summary>
    /// 允许调整尺寸的容器
    /// </summary>
    public class SizableContainer : ContainerBase
    {
        protected static readonly ContentAlignment AnyRightAlign = (ContentAlignment)1092;
        protected static readonly ContentAlignment AnyLeftAlign = (ContentAlignment)273;
        protected static readonly ContentAlignment AnyTopAlign = (ContentAlignment)7;
        protected static readonly ContentAlignment AnyBottomAlign = (ContentAlignment)1792;
        
        /// <summary>
        /// 允许拖拽
        /// </summary>
        [DefaultValue(false)]
        public bool Dragable { get; set; } = false;

        /// <summary>
        /// 正在拖拽
        /// </summary>
        protected bool Dragging = false;

        /// <summary>
        /// 允许调整大小
        /// </summary>
        [DefaultValue(false)]
        public bool Sizable { get; set; } = false;

        /// <summary>
        /// 正在调整大小
        /// </summary>
        protected bool Sizing = false;

        /// <summary>
        /// 拖拽或调整大小开始时鼠标的位置
        /// </summary>
        protected Point MousePoint;

        /// <summary>
        /// 调整边框时调整的边界值
        /// </summary>
        protected ContentAlignment SizableBound = ContentAlignment.MiddleCenter;

        public override void InitializeLayout() { }

        public override void ResetSize(int width, int height) { }

        /// <summary>
        /// 覆写触发鼠标按压方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (PreMouseDown(e)) return;

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 预处理鼠标按压事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        //TODO: [提醒] 自定义拖拽开始和调整大小开始的区域和条件
        protected virtual bool PreMouseDown(MouseEventArgs e)
        {
            if (Dragable && e.Button == MouseButtons.Left &&
                5 < e.Location.X && e.Location.X < this.Width - 5 && 5 < e.Location.Y && e.Location.Y <= 10)
            {
                Dragging = true;
                MousePoint = MousePosition;
                return true;
            }

            if (Sizable && e.Button == MouseButtons.Left &&
                ((e.Location.X <= 5 || e.Location.X >= this.Width - 5) ||
                (e.Location.Y <= 5 || e.Location.Y >= this.Height - 5)))
            {
                Sizing = true;
                MousePoint = MousePosition;

                //记录调整大小的边界
                if (e.X >= this.Width - 5 && e.Y >= this.Height - 5)
                    SizableBound = ContentAlignment.BottomRight;
                else if (e.X <= 5 && e.Y <= 5)
                    SizableBound = ContentAlignment.TopLeft;
                else if (e.X <= 5 && e.Y >= this.Height - 5)
                    SizableBound = ContentAlignment.BottomLeft;
                else if (e.X >= this.Width - 5 && e.Y <= 5)
                    SizableBound = ContentAlignment.TopRight;
                else if (e.X >= this.Width - 5)
                    SizableBound = ContentAlignment.MiddleRight;
                else if (e.Y >= this.Height - 5)
                    SizableBound = ContentAlignment.BottomCenter;
                else if (e.X <= 5)
                    SizableBound = ContentAlignment.MiddleLeft;
                else if (e.Y <= 5)
                    SizableBound = ContentAlignment.TopCenter;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 覆写触发鼠标移动方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Dragable)
            {
                if (Dragging)
                {
                    Point cpoint = MousePosition;
                    int x = cpoint.X - MousePoint.X;
                    int y = cpoint.Y - MousePoint.Y;
                    this.Left += x;
                    this.Top += y;
                    MousePoint = cpoint;
                    return;
                }
                else
                {
                    Point ClientPoint = this.PointToClient(MousePosition);
                    if (5 < ClientPoint.X && ClientPoint.X < this.Width - 5 && 5 < ClientPoint.Y && ClientPoint.Y <= 10)
                    {
                        this.Cursor = Cursors.SizeAll;
                        return;
                    }
                }
            }

            if (Sizable)
            {
                if (Sizing)
                {
                    if (SizableBound == ContentAlignment.MiddleCenter) return;

                    Point cpoint = MousePosition;
                    int x = cpoint.X - MousePoint.X;
                    int y = cpoint.Y - MousePoint.Y;
                    MousePoint = cpoint;

                    this.SuspendPaint();
                    if ((SizableBound & AnyLeftAlign) != (ContentAlignment)0)
                    {
                        this.Left += x;
                        this.Width -= x;
                    }
                    else if ((SizableBound & AnyRightAlign) != (ContentAlignment)0)
                        this.Width += x;

                    if ((SizableBound & AnyTopAlign) != (ContentAlignment)0)
                    {
                        this.Top += y;
                        this.Height -= y;
                    }
                    else if ((SizableBound & AnyBottomAlign) != (ContentAlignment)0)
                        this.Height += y;

                    this.ResumePaint();
                    return;
                }
                else
                {
                    Point ClientPoint = this.PointToClient(MousePosition);
                    if (ClientPoint.X >= this.Width - 5 && ClientPoint.Y >= this.Height - 5)
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        return;
                    }
                    else if (ClientPoint.X <= 5 && ClientPoint.Y <= 5)
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        return;
                    }
                    else if (ClientPoint.X <= 5 && ClientPoint.Y >= this.Height - 5)
                    {
                        this.Cursor = Cursors.SizeNESW;
                        return;
                    }
                    else if (ClientPoint.X >= this.Width - 5 && ClientPoint.Y <= 5)
                    {
                        this.Cursor = Cursors.SizeNESW;
                        return;
                    }
                    else if (ClientPoint.X >= this.Width - 5)
                    {
                        this.Cursor = Cursors.SizeWE;
                        return;
                    }
                    else if (ClientPoint.Y >= this.Height - 5)
                    {
                        this.Cursor = Cursors.SizeNS;
                        return;
                    }
                    else if (ClientPoint.X <= 5)
                    {
                        this.Cursor = Cursors.SizeWE;
                        return;
                    }
                    else if (ClientPoint.Y <= 5)
                    {
                        this.Cursor = Cursors.SizeNS;
                        return;
                    }
                }
            }

            if(this.Cursor != Cursors.Default)
                this.Cursor = Cursors.Default;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// 覆写触发鼠标抬起方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Dragging || Sizing)
            {
                this.Dragging = false;
                this.Sizing = false;
                this.SizableBound = ContentAlignment.MiddleCenter;
                this.Cursor = Cursors.Default;
            }
            else
            {
                base.OnMouseUp(e);
            }
        }

    }
}
