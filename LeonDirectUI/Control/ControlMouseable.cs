using LeonDirectUI.Interface;
using LeonDirectUI.VisualStyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.Control
{
    public class ControlMouseable : ControlBase, IMouseable
    {
        //TODO: 鼠标状态改变调用绘制方法

        private MouseStates mouseState = MouseStates.Normal;
        /// <summary>
        /// 鼠标状态
        /// </summary>
        public MouseStates MouseState
        {
            get => mouseState;
            set
            {
                if (mouseState != value)
                {
                    mouseState = value;
                    //重新绘制
                    Painter?.Paint(this);
                }
            }
        }

        #region 容器订阅事件

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        /// <param name="e"></param>
        public event EventHandler MouseEnter;

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// 鼠标悬停事件
        /// </summary>
        /// <param name="e"></param>
        public event EventHandler MouseHover;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        public event MouseEventHandler MouseDown;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="e"></param>
        public event MouseEventHandler MouseUp;

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="e"></param>
        public event EventHandler MouseLeave;

        /// <summary>
        /// 点击事件
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// 双击事件
        /// </summary>
        public event EventHandler DoubleClick;

        #endregion

        #region 容器访问方法

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        public void OnClick(EventArgs e)
        {
            if (Visible && Enabled)
            {
                //MouseState = MouseStates.Press;
                Click?.Invoke(this, e);
                //MouseState = MouseStates.Normal;
            }
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="e"></param>
        public void OnDoubleClick(EventArgs e)
        {
            if (Visible && Enabled)
            {
                //MouseState = MouseStates.Press;
                DoubleClick?.Invoke(this, e);
                //MouseState = MouseStates.Normal;
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseEnter(EventArgs e)
        {
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Hover;
                MouseEnter?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseMove(MouseEventArgs e)
        {
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Hover;
                MouseMove?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标悬停
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseHover(EventArgs e)
        {
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Hover;
                MouseHover?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseDown(MouseEventArgs e)
        {
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Press;
                MouseDown?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseUp(MouseEventArgs e)
        {
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Hover;
                MouseUp?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseLeave(EventArgs e)
        {
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Normal;
                MouseLeave?.Invoke(this, e);
            }
        }

        #endregion

    }
}
