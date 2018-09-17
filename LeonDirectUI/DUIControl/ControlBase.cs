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
    //TODO: 增加 MaxSize 和 MinSize 属性

    /// <summary>
    /// 控件基类
    /// </summary>
    public class ControlBase : IMouseable
    {

        #region 基本属性
        //TODO: [提醒] 可见性和可用性影响显示效果和交互效果

        /// <summary>
        /// 控件的名称
        /// </summary>
        public virtual string Name { get; set; } = "虚拟控件";

        private bool _enabled = true;
        /// <summary>
        /// 可用性
        /// </summary>
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private bool _visible = true;
        /// <summary>
        /// 可见性
        /// </summary>
        public virtual bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (value) PaintRequired?.Invoke(this, Rectangle);
            }
        }

        #endregion

        #region 显示属性
        //TODO: [提醒] 每次显示属性发生变动需要调用绘制方法

        private string _text = "虚拟控件";
        /// <summary>
        /// 显示文本
        /// </summary>
        public virtual string Text
        {
            get => _text;
            set
            {
                _text = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private bool _showEllipsis = false;
        /// <summary>
        /// 显示省略号
        /// </summary>
        public bool ShowEllipsis
        {
            get => _showEllipsis;
            set
            {
                _showEllipsis = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 文本的显示位置
        /// </summary>
        public virtual ContentAlignment TextAlign
        {
            get => _textAlign;
            set
            {
                _textAlign = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private Font _font = SystemFonts.DefaultFont;
        /// <summary>
        /// 字体
        /// </summary>
        public virtual Font Font
        {
            get => _font;
            set
            {
                _font = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private Color _backColor = Color.Transparent;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public virtual Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private Color _foreColor = Color.Black;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public virtual Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private Image _image;
        /// <summary>
        /// 控件显示图像
        /// </summary>
        public virtual Image Image
        {
            get => _image;
            set
            {
                _image = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 图像的显示位置
        /// </summary>
        public virtual ContentAlignment ImageAlign
        {
            get => _imageAlign;
            set
            {
                _imageAlign = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private Image _backgroundImage;
        /// <summary>
        /// 背景图像
        /// </summary>
        public virtual Image BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                _backgroundImage = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private ImageLayout _backgroundImageLayout = ImageLayout.None;
        /// <summary>
        /// 背景图显示方式
        /// </summary>
        public virtual ImageLayout BackgroundImageLayout
        {
            get => _backgroundImageLayout;
            set
            {
                _backgroundImageLayout = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        private Padding _padding = new Padding(3, 3, 3, 3);
        /// <summary>
        /// 内边距
        /// </summary>
        public virtual Padding Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                PaintRequired?.Invoke(this, Rectangle);
            }
        }

        #endregion

        #region 区域属性

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
                Rectangle lastRectangle = Rectangle;
                Papa.X = value;
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.Y = value;
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.Width = Math.Max(value, 0);
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.Height = Math.Max(value, 0);
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.X = value.Left;
                Papa.Y = value.Top;
                Papa.Width = Math.Max(value.Width, 0);
                Papa.Height = Math.Max(value.Height, 0);
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.Width = Math.Max(value.Width, 0);
                Papa.Height = Math.Max(value.Height, 0);
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.Location = value;
                PaintRequired?.Invoke(this, lastRectangle);
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
            Rectangle lastRectangle = Rectangle;
            Papa.X = left;
            Papa.Y = top;
            Papa.Width = Math.Max(width, 0);
            Papa.Height = Math.Max(height, 0);
            PaintRequired?.Invoke(this, lastRectangle);
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public virtual void SetSize(int width, int height)
        {
            Rectangle lastRectangle = Rectangle;
            Papa.Width = Math.Max(width, 0);
            Papa.Height = Math.Max(height, 0);
            PaintRequired?.Invoke(this, lastRectangle);
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        public virtual void SetLocation(int left, int top)
        {
            Rectangle lastRectangle = Rectangle;
            Papa.X = left;
            Papa.Y = top;
            PaintRequired?.Invoke(this, lastRectangle);
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
        /// 目标点是否包含在区域内
        /// </summary>
        /// <param name="point">目标坐标</param>
        /// <returns></returns>
        public virtual bool Contains(Point point) => Papa.Contains(point);

        /// <summary>
        /// 将区域放大指定量
        /// </summary>
        /// <param name="width">放大宽度</param>
        /// <param name="height">放大高度</param>
        public virtual void Inflate(int width, int height)
        {
            Rectangle lastRectangle = Rectangle;
            Papa.Inflate(width, height);
            Papa.Width = Math.Max(Papa.Width, 0);
            Papa.Height = Math.Max(Papa.Height, 0);
            PaintRequired?.Invoke(this, lastRectangle);
        }

        /// <summary>
        /// 将区域替换为与目标区域的交集
        /// </summary>
        /// <param name="rect">目标区域</param>
        public virtual void Intersect(Rectangle rect)
        {
            Rectangle lastRectangle = Rectangle;
            Papa.Intersect(rect);
            PaintRequired?.Invoke(this, lastRectangle);
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
            Rectangle lastRectangle = Rectangle;
            Papa.Offset(point);
            PaintRequired?.Invoke(this, lastRectangle);
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
                    PaintRequired?.Invoke(this, Rectangle);
                }
            }
        }

        #endregion

        #region 容器订阅事件

        /// <summary>
        /// 绘制请求事件委托
        /// </summary>
        /// <param name="rectangle">绘制区域</param>
        public delegate void PaintRequiredHandler(ControlBase sender, Rectangle rectangle);

        /// <summary>
        /// 请求绘制事件
        /// </summary>
        public event PaintRequiredHandler PaintRequired;

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

        //触发绘制请求
        public void OnPaintRequired()
        {
            PaintRequired?.Invoke(this, Rectangle);
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        public void OnClick(EventArgs e)
        {
            //Console.WriteLine($"{Name} : 点击");
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
            //Console.WriteLine($"{Name} : 双击");
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
            //Console.WriteLine($"{Name} : 鼠标进入");
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
            //Console.WriteLine($"{Name} : 鼠标移动");
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
            //Console.WriteLine($"{Name} : 鼠标悬停");
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
            //Console.WriteLine($"{Name} : 鼠标按压");
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
            //Console.WriteLine($"{Name} : 鼠标抬起");
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
            //Console.WriteLine($"{Name} : 鼠标离开");
            if (Visible && Enabled)
            {
                MouseState = MouseStates.Normal;
                MouseLeave?.Invoke(this, e);
            }
        }

        #endregion

    }
}
