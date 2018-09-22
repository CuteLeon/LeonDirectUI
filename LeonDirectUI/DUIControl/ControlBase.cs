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

    /// <summary>
    /// 控件基类
    /// </summary>
    public class ControlBase : IMouseable, IDisposable
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
            get => this._enabled;
            set
            {
                if (this._enabled != value)
                {
                    this._enabled = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private bool _visible = true;
        /// <summary>
        /// 可见性
        /// </summary>
        public virtual bool Visible
        {
            get => this._visible;
            set
            {
                if (this._visible != value)
                {
                    this._visible = value;
                    if (value) this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Size _maxSize = Size.Empty;
        /// <summary>
        /// 最大尺寸
        /// </summary>
        [AmbientValue(typeof(Size), "0, 0")]
        public virtual Size MaxSize
        {
            get => this._maxSize;
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                if (value.Width > 0 && value.Width < this._minSize.Width) this._minSize.Width = value.Width;
                if (value.Height > 0 && value.Height < this._minSize.Height) this._minSize.Height = value.Height;

                this._maxSize = value;

                if (value.Width != 0 && this.Width > value.Width)
                    this.Width = value.Width;

                if (value.Height != 0 && this.Height > value.Height)
                    this.Height = value.Height;
            }
        }

        private Size _minSize = Size.Empty;
        /// <summary>
        /// 最小尺寸
        /// </summary>
        [AmbientValue(typeof(Size), "0, 0")]
        public virtual Size MinSize
        {
            get => this._minSize;
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                if (value.Width > 0 && value.Width > this._maxSize.Width) this._maxSize.Width = value.Width;
                if (value.Height > 0 && value.Height > this._maxSize.Height) this._maxSize.Height = value.Height;

                this._minSize = value;

                if (value.Width != 0 && this.Width < value.Width)
                    this.Width = value.Width;

                if (value.Height != 0 && this.Height < value.Height)
                    this.Height = value.Height;
            }
        }

        #endregion

        #region 显示属性
        //TODO: [提醒] [虚拟控件扩展] 每次显示属性发生变动需要调用绘制方法

        private string _text = "";
        /// <summary>
        /// 显示文本
        /// </summary>
        public virtual string Text
        {
            get => this._text;
            set
            {
                if (this._text != value)
                {
                    this._text = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private bool _showEllipsis = false;
        /// <summary>
        /// 显示省略号
        /// </summary>
        public bool ShowEllipsis
        {
            get => this._showEllipsis;
            set
            {
                if (this._showEllipsis != value)
                {
                    this._showEllipsis = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 文本的显示位置
        /// </summary>
        public virtual ContentAlignment TextAlign
        {
            get => this._textAlign;
            set
            {
                if (this._textAlign != value)
                {
                    this._textAlign = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Font _font = SystemFonts.DefaultFont;
        /// <summary>
        /// 字体
        /// </summary>
        public virtual Font Font
        {
            get => this._font;
            set
            {
                if (this._font != value)
                {
                    this._font = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Color _backColor = Color.Transparent;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public virtual Color BackColor
        {
            get => this._backColor;
            set
            {
                if (this._backColor != value)
                {
                    this._backColor = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Color _foreColor = Color.Black;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public virtual Color ForeColor
        {
            get => this._foreColor;
            set
            {
                if (this._foreColor != null)
                {
                    this._foreColor = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Image _image;
        /// <summary>
        /// 控件显示图像
        /// </summary>
        public virtual Image Image
        {
            get => this._image;
            set
            {
                this._image = value;
                this.PaintRequired?.Invoke(this, this.Rectangle);
            }
        }

        private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 图像的显示位置
        /// </summary>
        public virtual ContentAlignment ImageAlign
        {
            get => this._imageAlign;
            set
            {
                if (this._imageAlign != value)
                {
                    this._imageAlign = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Image _backgroundImage;
        /// <summary>
        /// 背景图像
        /// </summary>
        public virtual Image BackgroundImage
        {
            get => this._backgroundImage;
            set
            {
                this._backgroundImage = value;
                this.PaintRequired?.Invoke(this, this.Rectangle);
            }
        }

        private ImageLayout _backgroundImageLayout = ImageLayout.None;
        /// <summary>
        /// 背景图显示方式
        /// </summary>
        public virtual ImageLayout BackgroundImageLayout
        {
            get => this._backgroundImageLayout;
            set
            {
                if (this._backgroundImageLayout != value)
                {
                    this._backgroundImageLayout = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private int _borderSize = 1;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [DefaultValue(1)]
        public virtual int BorderSize
        {
            get => this._borderSize;
            set
            {
                if (this._borderSize != value)
                {
                    this._borderSize = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Color _borderColor = Color.Gray;
        /// <summary>
        /// 边框颜色
        /// </summary>
        public virtual Color BorderColor
        {
            get => this._borderColor;
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.None;
        /// <summary>
        /// 边框类型（None : 无边框）
        /// </summary>
        [DefaultValue(ButtonBorderStyle.None)]
        public virtual ButtonBorderStyle BorderStyle
        {
            get => this._borderStyle;
            set
            {
                if (this._borderStyle != value)
                {
                    this._borderStyle = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
            }
        }

        private Padding _padding = new Padding(3, 3, 3, 3);
        /// <summary>
        /// 内边距
        /// </summary>
        public virtual Padding Padding
        {
            get => this._padding;
            set
            {
                if (this._padding != value)
                {
                    this._padding = value;
                    this.PaintRequired?.Invoke(this, this.Rectangle);
                }
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
        public virtual int Right { get => this.Papa.Right; }

        /// <summary>
        /// 下边界
        /// </summary>
        public virtual int Bottom { get => this.Papa.Bottom; }

        /// <summary>
        /// 左坐标
        /// </summary>
        public virtual int Left
        {
            get => this.Papa.X;
            set
            {
                if (this.Papa.X != value)
                {
                    Rectangle lastRectangle = this.Rectangle;
                    this.Papa.X = value;
                    this.PaintRequired?.Invoke(this, lastRectangle);
                }
            }
        }

        /// <summary>
        /// 上坐标
        /// </summary>
        public virtual int Top
        {
            get => this.Papa.Y;
            set
            {
                if (this.Papa.Y != value)
                {
                    Rectangle lastRectangle = this.Rectangle;
                    this.Papa.Y = value;
                    this.PaintRequired?.Invoke(this, lastRectangle);
                }
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public virtual int Width
        {
            get => this.Papa.Width;
            set
            {
                int result = Math.Max(value, 0);
                if (this.MinSize.Width > 0) result = Math.Max(this.MinSize.Width, result);
                if (this.MaxSize.Width > 0) result = Math.Min(this.MaxSize.Width, result);

                if (this.Papa.Width != result)
                {
                    Rectangle lastRectangle = this.Rectangle;
                    this.Papa.Width = result;
                    this.PaintRequired?.Invoke(this, lastRectangle);
                }
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public virtual int Height
        {
            get => this.Papa.Height;
            set
            {
                int result = Math.Max(value, 0);
                if (this.MinSize.Height > 0) result = Math.Max(this.MinSize.Height, result);
                if (this.MaxSize.Height > 0) result = Math.Min(this.MaxSize.Height, result);

                if (this.Papa.Height != result)
                {
                    Rectangle lastRectangle = this.Rectangle;
                    this.Papa.Height = result;
                    this.PaintRequired?.Invoke(this, lastRectangle);
                }
            }
        }

        /// <summary>
        /// 显示区域
        /// </summary>
        public virtual Rectangle Rectangle
        {
            get => this.Papa;
            set
            {
                this.SetBounds(value.Left, value.Top, value.Width, value.Height);
            }
        }

        /// <summary>
        /// 控件尺寸
        /// </summary>
        public virtual Size Size
        {
            get => this.Papa.Size;
            set
            {
                this.SetSize(value.Width, value.Height);
            }
        }

        /// <summary>
        /// 控件坐标
        /// </summary>
        public virtual Point Location
        {
            get => this.Papa.Location;
            set
            {
                this.SetLocation(value.X, value.Y);
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
            int Xresult = Math.Max(width, 0), Yresult = Math.Max(height, 0);
            if (this.MinSize.Width > 0) Xresult = Math.Max(this.MinSize.Width, Xresult);
            if (this.MaxSize.Width > 0) Xresult = Math.Min(this.MaxSize.Width, Xresult);
            if (this.MinSize.Height > 0) Yresult = Math.Max(this.MinSize.Height, Yresult);
            if (this.MaxSize.Height > 0) Yresult = Math.Min(this.MaxSize.Height, Yresult);

            if (this.Papa.X != left || this.Papa.Y != top || this.Papa.Width != Xresult || this.Papa.Height != Yresult)
            {
                Rectangle lastRectangle = this.Rectangle;
                this.Papa.X = left;
                this.Papa.Y = top;
                this.Papa.Width = Xresult;
                this.Papa.Height = Yresult;
                this.PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public virtual void SetSize(int width, int height)
        {
            int Xresult = Math.Max(width, 0), Yresult = Math.Max(height, 0);
            if (this.MinSize.Width > 0) Xresult = Math.Max(this.MinSize.Width, Xresult);
            if (this.MaxSize.Width > 0) Xresult = Math.Min(this.MaxSize.Width, Xresult);
            if (this.MinSize.Height > 0) Yresult = Math.Max(this.MinSize.Height, Yresult);
            if (this.MaxSize.Height > 0) Yresult = Math.Min(this.MaxSize.Height, Yresult);

            if (this.Papa.Width != Xresult || this.Papa.Height != Yresult)
            {
                Rectangle lastRectangle = this.Rectangle;
                this.Papa.Width = Xresult;
                this.Papa.Height = Yresult;
                this.PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        public virtual void SetLocation(int left, int top)
        {
            if (this.Papa.X != left || this.Papa.Y != top)
            {
                Rectangle lastRectangle = this.Rectangle;
                this.Papa.X = left;
                this.Papa.Y = top;
                this.PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 将区域放大指定量
        /// </summary>
        /// <param name="width">放大宽度</param>
        /// <param name="height">放大高度</param>
        public virtual void Inflate(int width, int height)
        {
            int Xresult = Math.Max(this.Papa.Width + width, 0), Yresult = Math.Max(this.Papa.Height + height, 0);
            if (this.MinSize.Width > 0) Xresult = Math.Max(this.MinSize.Width, Xresult);
            if (this.MaxSize.Width > 0) Xresult = Math.Min(this.MaxSize.Width, Xresult);
            if (this.MinSize.Height > 0) Yresult = Math.Max(this.MinSize.Height, Yresult);
            if (this.MaxSize.Height > 0) Yresult = Math.Min(this.MaxSize.Height, Yresult);

            if (this.Papa.Width != Xresult || this.Papa.Height != Yresult)
            {
                Rectangle lastRectangle = this.Rectangle;
                this.Papa.Width = Xresult;
                this.Papa.Height = Yresult;
                this.PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 将区域替换为与目标区域的交集
        /// </summary>
        /// <param name="rect">目标区域</param>
        public virtual void Intersect(Rectangle rect)
        {
            Rectangle result = this.Rectangle;
            result.Intersect(rect);
            if (this.MinSize.Width > 0) result.Width = Math.Max(this.MinSize.Width, result.Width);
            if (this.MaxSize.Width > 0) result.Width = Math.Min(this.MaxSize.Width, result.Width);
            if (this.MinSize.Height > 0) result.Height = Math.Max(this.MinSize.Height, result.Height);
            if (this.MaxSize.Height > 0) result.Height = Math.Min(this.MaxSize.Height, result.Height);

            if (result.X != this.Papa.Left || result.Y != this.Papa.Top || result.Width != this.Papa.Width || result.Height != this.Papa.Height)
            {
                Rectangle lastRectangle = this.Rectangle;
                this.Papa.X = result.Left;
                this.Papa.Y = result.Top;
                this.Papa.Width = result.Width;
                this.Papa.Height = result.Height;
                this.PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 将区域调整指定的量
        /// </summary>
        /// <param name="point"></param>
        public virtual void Offset(Point point)
        {
            if (point.X != 0 || point.Y != 0)
            {
                Rectangle lastRectangle = this.Rectangle;
                this.Papa.Offset(point);
                this.PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        #endregion

        #region 区域关系计算

        /// <summary>
        /// 目标点是否包含在区域内
        /// </summary>
        /// <param name="x">水平坐标</param>
        /// <param name="y">垂直坐标</param>
        /// <returns></returns>
        public virtual bool Contains(int x, int y) => this.Papa.Contains(x, y);

        /// <summary>
        /// 目标点是否包含在区域内
        /// </summary>
        /// <param name="point">目标坐标</param>
        /// <returns></returns>
        public virtual bool Contains(Point point) => this.Papa.Contains(point);

        /// <summary>
        /// 目标区域是否完全包含在区域内
        /// </summary>
        /// <param name="rectangle">目标区域</param>
        /// <returns></returns>
        public virtual bool Contains(Rectangle rectangle) => this.Papa.Contains(rectangle);

        /// <summary>
        /// 是否与目标区域相交
        /// </summary>
        /// <param name="rect">目标区域</param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rect) => this.Papa.IntersectsWith(rect);

        /// <summary>
        /// 区域是否为空
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty() => this.Papa.IsEmpty;

        #endregion

        #region 鼠标响应支持

        private MouseStates mouseState = MouseStates.Normal;

        /// <summary>
        /// 鼠标状态
        /// </summary>
        public MouseStates MouseState
        {
            get => this.mouseState;
            protected set
            {
                if (this.mouseState != value)
                {
                    this.mouseState = value;
                    this.MouseStateChanged?.Invoke(this, value);
                    //this.PaintRequired?.Invoke(this, this.Rectangle);
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
            get => this._mouseable;
            set
            {
                if (this._mouseable != value)
                {
                    this._mouseable = value;
                    //禁用鼠标支持时若鼠标状态不是正常状态则恢复鼠标状态并请求重绘
                    if (!value && this.MouseState != MouseStates.Normal)
                    {
                        this.MouseState = MouseStates.Normal;
                        this.PaintRequired?.Invoke(this, this.Rectangle);
                    }
                    //触发鼠标支持状态改变事件
                    this.MouseableChanged?.Invoke(this, value);
                }
            }
        }

        #endregion

        #region 容器订阅事件

        /// <summary>
        /// 虚拟控件正在被释放
        /// </summary>
        public event EventHandler Disposing;

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
        public event EventHandler MouseEnter;

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// 鼠标悬停事件
        /// </summary>
        public event EventHandler MouseHover;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public event MouseEventHandler MouseDown;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        public event MouseEventHandler MouseUp;

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
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
            this.PaintRequired?.Invoke(this, this.Rectangle);
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        public void OnClick(EventArgs e)
        {
            //Console.WriteLine($"{Name} : 点击");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                //this.MouseState = MouseStates.Press;
                this.Click?.Invoke(this, e);
                //this.MouseState = MouseStates.Normal;
            }
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="e"></param>
        public void OnDoubleClick(EventArgs e)
        {
            //Console.WriteLine($"{Name} : 双击");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                //this.MouseState = MouseStates.Press;
                DoubleClick?.Invoke(this, e);
                //this.MouseState = MouseStates.Normal;
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseEnter(EventArgs e)
        {
            //Console.WriteLine($"{Name} : 鼠标进入");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                this.MouseState = MouseStates.Hover;
                this.MouseEnter?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseMove(MouseEventArgs e)
        {
            //Console.WriteLine($"{Name} : 鼠标移动");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                this.MouseState = MouseStates.Hover;
                this.MouseMove?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标悬停
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseHover(EventArgs e)
        {
            //Console.WriteLine($"{Name} : 鼠标悬停");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                this.MouseState = MouseStates.Hover;
                this.MouseHover?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseDown(MouseEventArgs e)
        {
            //Console.WriteLine($"{Name} : 鼠标按压");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                this.MouseState = MouseStates.Press;
                this.MouseDown?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseUp(MouseEventArgs e)
        {
            //Console.WriteLine($"{Name} : 鼠标抬起");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                this.MouseState = MouseStates.Hover;
                this.MouseUp?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseLeave(EventArgs e)
        {
            //Console.WriteLine($"{Name} : 鼠标离开");
            if (this.Visible && this.Enabled && this.Mouseable)
            {
                this.MouseState = MouseStates.Normal;
                this.MouseLeave?.Invoke(this, e);
            }
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.Disposing?.Invoke(this, EventArgs.Empty);

                    this._font = null;
                    this._backgroundImage = null;
                    this._image = null;
                }

                // 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。

                this.disposedValue = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
