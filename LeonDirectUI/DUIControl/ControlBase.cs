using LeonDirectUI.Interface;
using LeonDirectUI.VisualStyle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.DUIControl
{
    /// <summary>
    /// 控件基类
    /// </summary>
    public class ControlBase : IMouseable
    {

        #region 基本属性
        //TODO: 可见性和可用性影响显示效果和交互效果

        /// <summary>
        /// 控件的名称
        /// </summary>
        public virtual string Name { get; set; } = "虚拟控件";

        /// <summary>
        /// 可用性
        /// </summary>
        public virtual bool Enabled { get; set; } = true;

        /// <summary>
        /// 可见性
        /// </summary>
        public virtual bool Visible { get; set; } = true;

        #endregion

        #region 显示属性
        //TODO: 每次显示属性发生变动需要调用绘制方法

        /// <summary>
        /// 显示文本
        /// </summary>
        public virtual string Text { get; set; } = "虚拟控件";

        /// <summary>
        /// 文本的显示位置
        /// </summary>
        public virtual ContentAlignment TextAlign { get; set; } = ContentAlignment.MiddleLeft;

        /// <summary>
        /// 字体
        /// </summary>
        public virtual Font Font { get; set; } = SystemFonts.DefaultFont;

        /// <summary>
        /// 背景颜色
        /// </summary>
        public virtual Color BackColor { get; set; } = Color.Transparent;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public virtual Color ForeColor { get; set; } = Color.Black;

        /// <summary>
        /// 控件显示图像
        /// </summary>
        public virtual Image Image { get; set; }

        /// <summary>
        /// 图像的显示位置
        /// </summary>
        public virtual ContentAlignment ImageAlign { get; set; } = ContentAlignment.MiddleCenter;

        /// <summary>
        /// 背景图像
        /// </summary>
        public virtual Image BackgroundImage { get; set; }

        /// <summary>
        /// 背景图显示方式
        /// </summary>
        public virtual ImageLayout BackgroundImageLayout { get; set; } = ImageLayout.None;

        /// <summary>
        /// 内边距
        /// </summary>
        public virtual Padding Padding { get; set; } = new Padding(3, 3, 3, 3);

        #endregion

        #region 区域

        /// <summary>
        /// 区域 (核心)
        /// </summary>
        private Rectangle Papa = Rectangle.Empty;

        /// <summary>
        /// 左坐标
        /// </summary>
        public virtual int Left
        {
            get => Papa.X;
            set
            {
                Papa.X = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 上坐标
        /// </summary>
        public virtual int Top
        {
            get => Papa.Y;
            set
            {
                Papa.Y = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public virtual int Width
        {
            get => Papa.Width;
            set
            {
                Papa.Width = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public virtual int Height
        {
            get => Papa.Height;
            set
            {
                Papa.Height = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 右边界
        /// </summary>
        public virtual int Right { get => Papa.Right; }

        /// <summary>
        /// 下边界
        /// </summary>
        public virtual int Bottom { get => Papa.Bottom; }

        /// <summary>
        /// 显示区域
        /// </summary>
        public virtual Rectangle Rectangle
        {
            get => Papa;
            set
            {
                Papa = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 控件尺寸
        /// </summary>
        public virtual Size Size
        {
            get => Papa.Size;
            set
            {
                Papa.Size = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 控件坐标
        /// </summary>
        public virtual Point Location
        {
            get => Papa.Location;
            set
            {
                Papa.Location = value;
                //TODO: 通知容器刷新虚拟控件
            }
        }

        /// <summary>
        /// 设置边界
        /// </summary>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public virtual void SetBounds(int left, int top, int width, int height)
        {
            Papa.X = left;
            Papa.Y = top;
            Papa.Width = width;
            Papa.Height = height;
            
            //TODO: 通知容器刷新虚拟控件
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public virtual void SetSize(int width, int height)
        {
            Papa.Width = width;
            Papa.Height = height;
            
            //TODO: 通知容器刷新虚拟控件
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        public virtual void SetLocation(int left, int top)
        {
            Papa.X = left;
            Papa.Y = top;
            
            //TODO: 通知容器刷新虚拟控件
        }

        #endregion

        #region 区域关系计算

        /// <summary>
        /// 目标点是否包含在区域内
        /// </summary>
        /// <param name="x">水平坐标</param>
        /// <param name="y">垂直坐标</param>
        /// <returns></returns>
        public virtual bool Contains(int x, int y) => Papa.Contains(x, y);

        /// <summary>
        /// 将区域放大指定量
        /// </summary>
        /// <param name="width">放大宽度</param>
        /// <param name="height">放大高度</param>
        public virtual void Inflate(int width, int height)
        {
            Papa.Inflate(width, height);
            //TODO: 通知容器刷新虚拟控件
        }

        /// <summary>
        /// 将区域替换为与目标区域的交集
        /// </summary>
        /// <param name="rect">目标区域</param>
        public virtual void Intersect(Rectangle rect)
        {
            Papa.Intersect(rect);
            //TODO: 通知容器刷新虚拟控件
        }

        /// <summary>
        /// 是否与目标区域相交
        /// </summary>
        /// <param name="rect">目标区域</param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rect) => Papa.IntersectsWith(rect);

        /// <summary>
        /// 区域是否为空
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty() => Papa.IsEmpty;

        /// <summary>
        /// 将区域调整指定的量
        /// </summary>
        /// <param name="point"></param>
        public virtual void Offset(Point point)
        {
            Papa.Offset(point);
            //TODO: 通知容器刷新虚拟控件
        }

        #endregion

        #region 鼠标响应支持

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
                    //TODO: 通知容器刷新虚拟控件
                }
            }
        }

        #endregion

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
