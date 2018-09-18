using LeonDirectUI.Interface;
using LeonDirectUI.VisualStyle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.DUIControl
{
    //TODO: SuspendLayout 和 ResumeLayout 方法密集调整省略计算
    //TODO: SuspendPaint 和 ResumePaint 方法密集刷新时省略绘制

    /// <summary>
    /// 控件基类
    /// </summary>
    public class ControlBase : IMouseable
    {

        #region 基本属性
        //TODO: [提醒] [虚拟控件扩展] 可见性和可用性影响显示效果和交互效果

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

        private Size _maxSize = Size.Empty;
        /// <summary>
        /// 最大尺寸
        /// </summary>
        [AmbientValue(typeof(Size), "0, 0")]
        public virtual Size MaxSize
        {
            get => _maxSize;
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                if (value.Width > 0 && value.Width < _minSize.Width) _minSize.Width = value.Width;
                if (value.Height > 0 && value.Height < _minSize.Height) _minSize.Height = value.Height;

                _maxSize = value;

                if ((_maxSize.Width != 0 && _maxSize.Height != 0) &&
                    (_maxSize.Width < Width || _maxSize.Height < Height))
                    SetSize(Math.Min(Width, value.Width), Math.Min(Height, value.Height));
            }
        }

        private Size _minSize = Size.Empty;
        /// <summary>
        /// 最小尺寸
        /// </summary>
        [AmbientValue(typeof(Size), "0, 0")]
        public virtual Size MinSize
        {
            get => _minSize;
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                if (value.Width > 0 && value.Width > _maxSize.Width) _maxSize.Width = value.Width;
                if (value.Height > 0 && value.Height > _maxSize.Height) _maxSize.Height = value.Height;

                _minSize = value;

                if ((_minSize.Width != 0 && _minSize.Height != 0) &&
                    (_minSize.Width > Width || _minSize.Height > Height))
                    SetSize(Math.Max(Width, value.Width), Math.Max(Height, value.Height));
            }
        }

        #endregion

        #region 显示属性
        //TODO: [提醒] [虚拟控件扩展] 每次显示属性发生变动需要调用绘制方法

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
        /// 右边界
        /// </summary>
        public virtual int Right { get => Papa.Right; }

        /// <summary>
        /// 下边界
        /// </summary>
        public virtual int Bottom { get => Papa.Bottom; }

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
                if (MinSize.Width > 0) Papa.Width = Math.Max(MinSize.Width, Papa.Width);
                if (MaxSize.Width > 0) Papa.Width = Math.Min(MaxSize.Width, Papa.Width);
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
                if (MinSize.Height > 0) Papa.Height = Math.Max(MinSize.Height, Papa.Height);
                if (MaxSize.Height > 0) Papa.Height = Math.Min(MaxSize.Height, Papa.Height);
                PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 显示区域
        /// </summary>
        public virtual Rectangle Rectangle
        {
            get => Papa;
            set
            {
                SetBounds(value.Left, value.Top, value.Width, value.Height);
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
                SetSize(value.Width, value.Height);
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
                SetLocation(value.X, value.Y);
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
            if (MinSize.Width > 0) Papa.Width = Math.Max(MinSize.Width, Papa.Width);
            if (MaxSize.Width > 0) Papa.Width = Math.Min(MaxSize.Width, Papa.Width);
            if (MinSize.Height > 0) Papa.Height = Math.Max(MinSize.Height, Papa.Height);
            if (MaxSize.Height > 0) Papa.Height = Math.Min(MaxSize.Height, Papa.Height);
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
            if (MinSize.Width > 0) Papa.Width = Math.Max(MinSize.Width, Papa.Width);
            if (MaxSize.Width > 0) Papa.Width = Math.Min(MaxSize.Width, Papa.Width);
            if (MinSize.Height > 0) Papa.Height = Math.Max(MinSize.Height, Papa.Height);
            if (MaxSize.Height > 0) Papa.Height = Math.Min(MaxSize.Height, Papa.Height);
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
            if (MinSize.Width > 0) Papa.Width = Math.Max(MinSize.Width, Papa.Width);
            if (MaxSize.Width > 0) Papa.Width = Math.Min(MaxSize.Width, Papa.Width);
            if (MinSize.Height > 0) Papa.Height = Math.Max(MinSize.Height, Papa.Height);
            if (MaxSize.Height > 0) Papa.Height = Math.Min(MaxSize.Height, Papa.Height);
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
            if (MinSize.Width > 0) Papa.Width = Math.Max(MinSize.Width, Papa.Width);
            if (MaxSize.Width > 0) Papa.Width = Math.Min(MaxSize.Width, Papa.Width);
            if (MinSize.Height > 0) Papa.Height = Math.Max(MinSize.Height, Papa.Height);
            if (MaxSize.Height > 0) Papa.Height = Math.Min(MaxSize.Height, Papa.Height);
            PaintRequired?.Invoke(this, lastRectangle);
        }

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
        /// 目标区域是否完全包含在区域内
        /// </summary>
        /// <param name="rectangle">目标区域</param>
        /// <returns></returns>
        public virtual bool Contains(Rectangle rectangle) => Papa.Contains(rectangle);

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

        #endregion

        #region 鼠标响应支持

        private MouseStates mouseState = MouseStates.Normal;

        /// <summary>
        /// 鼠标状态
        /// </summary>
        public MouseStates MouseState
        {
            get => mouseState;
            protected set
            {
                if (mouseState != value)
                {
                    mouseState = value;
                    MouseStateChanged?.Invoke(this, value);
                    //PaintRequired?.Invoke(this, Rectangle);
                }
            }
        }

        private bool _mouseable = false;
        /// <summary>
        /// 是否响应鼠标事件
        /// </summary>
        //TODO: [提醒] [虚拟控件使用] 虚拟控件需要开启此属性才可响应鼠标事件
        public virtual bool Mouseable
        {
            get => _mouseable;
            set
            {
                if (_mouseable != value)
                {
                    _mouseable = value;
                    //禁用鼠标支持时若鼠标状态不是正常状态则恢复鼠标状态并请求重绘
                    if (!value && MouseState != MouseStates.Normal)
                    {
                        MouseState = MouseStates.Normal;
                        PaintRequired?.Invoke(this, Rectangle);
                    }
                    //触发鼠标支持状态改变事件
                    MouseableChanged?.Invoke(this, value);
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
        /// 鼠标支持状态变化
        /// </summary>
        public event EventHandler<bool> MouseableChanged;

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

        /// <summary>
        /// 鼠标状态改变
        /// </summary>
        public event EventHandler<MouseStates> MouseStateChanged;

        #endregion

        #region 容器访问方法

        /// <summary>
        /// 触发绘制请求
        /// </summary>
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
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
            if (Visible && Enabled && Mouseable)
            {
                MouseState = MouseStates.Normal;
                MouseLeave?.Invoke(this, e);
            }
        }

        #endregion

    }
}
