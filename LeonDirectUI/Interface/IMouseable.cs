using System;
using System.Windows.Forms;

using LeonDirectUI.VisualStyle;

namespace LeonDirectUI.Interface
{
    /// <summary>
    /// 可响应鼠标
    /// </summary>
    public interface IMouseable
    {
        #region 容器访问方法

        /// <summary>
        /// 鼠标状态
        /// </summary>
        MouseStates MouseState { get; }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        void OnClick(EventArgs e);

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="e"></param>
        void OnDoubleClick(EventArgs e);

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        void OnMouseEnter(EventArgs e);

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        void OnMouseMove(MouseEventArgs e);

        /// <summary>
        /// 鼠标悬停
        /// </summary>
        /// <param name="e"></param>
        void OnMouseHover(EventArgs e);

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        void OnMouseDown(MouseEventArgs e);

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        void OnMouseUp(MouseEventArgs e);

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        void OnMouseLeave(EventArgs e);

        #endregion

        #region 容器订阅事件
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="e"></param>
        event EventHandler Click;
        

        /// <summary>
        /// 鼠标状态改变
        /// </summary>
        event EventHandler<MouseStates> MouseStateChanged;
        
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="e"></param>
        event EventHandler DoubleClick;

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        /// <param name="e"></param>
        event EventHandler MouseEnter;

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        event MouseEventHandler MouseMove;

        /// <summary>
        /// 鼠标悬停事件
        /// </summary>
        /// <param name="e"></param>
        event EventHandler MouseHover;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        event MouseEventHandler MouseDown;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="e"></param>
        event MouseEventHandler MouseUp;

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="e"></param>
        event EventHandler MouseLeave;

        #endregion

    }
}
