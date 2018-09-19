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
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_visible != value)
                {
                    _visible = value;
                    if (value) PaintRequired?.Invoke(this, Rectangle);
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

        private string _text = "";
        /// <summary>
        /// 显示文本
        /// </summary>
        public virtual string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_showEllipsis != value)
                {
                    _showEllipsis = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_textAlign != value)
                {
                    _textAlign = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_font != value)
                {
                    _font = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_backColor != value)
                {
                    _backColor = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_foreColor != null)
                {
                    _foreColor = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_imageAlign != value)
                {
                    _imageAlign = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_backgroundImageLayout != value)
                {
                    _backgroundImageLayout = value;
                    PaintRequired?.Invoke(this, Rectangle);
                }
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
                if (_padding != value)
                {
                    _padding = value;
                    PaintRequired?.Invoke(this, Rectangle);
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
                if (Papa.X != value)
                {
                    Rectangle lastRectangle = Rectangle;
                    Papa.X = value;
                    PaintRequired?.Invoke(this, lastRectangle);
                }
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
                if (Papa.Y != value)
                {
                    Rectangle lastRectangle = Rectangle;
                    Papa.Y = value;
                    PaintRequired?.Invoke(this, lastRectangle);
                }
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
                int result = Math.Max(value, 0);
                if (MinSize.Width > 0) result = Math.Max(MinSize.Width, result);
                if (MaxSize.Width > 0) result = Math.Min(MaxSize.Width, result);

                if (Papa.Width != result)
                {
                    Rectangle lastRectangle = Rectangle;
                    Papa.Width = result;
                    PaintRequired?.Invoke(this, lastRectangle);
                }
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
                int result = Math.Max(value, 0);
                if (MinSize.Height > 0) result = Math.Max(MinSize.Height, result);
                if (MaxSize.Height > 0) result = Math.Min(MaxSize.Height, result);

                if (Papa.Height != result)
                {
                    Rectangle lastRectangle = Rectangle;
                    Papa.Height = result;
                    PaintRequired?.Invoke(this, lastRectangle);
                }
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
            int Xresult = Math.Max(width, 0), Yresult = Math.Max(height, 0);
            if (MinSize.Width > 0) Xresult = Math.Max(MinSize.Width, Xresult);
            if (MaxSize.Width > 0) Xresult = Math.Min(MaxSize.Width, Xresult);
            if (MinSize.Height > 0) Yresult = Math.Max(MinSize.Height, Yresult);
            if (MaxSize.Height > 0) Yresult = Math.Min(MaxSize.Height, Yresult);

            if (Papa.X != left || Papa.Y != top || Papa.Width != Xresult || Papa.Height != Yresult)
            {
                Rectangle lastRectangle = Rectangle;
                Papa.X = left;
                Papa.Y = top;
                Papa.Width = Xresult;
                Papa.Height = Yresult;
                PaintRequired?.Invoke(this, lastRectangle);
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
            if (MinSize.Width > 0) Xresult = Math.Max(MinSize.Width, Xresult);
            if (MaxSize.Width > 0) Xresult = Math.Min(MaxSize.Width, Xresult);
            if (MinSize.Height > 0) Yresult = Math.Max(MinSize.Height, Yresult);
            if (MaxSize.Height > 0) Yresult = Math.Min(MaxSize.Height, Yresult);

            if (Papa.Width != Xresult || Papa.Height != Yresult)
            {
                Rectangle lastRectangle = Rectangle;
                Papa.Width = Xresult;
                Papa.Height = Yresult;
                PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        public virtual void SetLocation(int left, int top)
        {
            if (Papa.X != left || Papa.Y != top)
            {
                Rectangle lastRectangle = Rectangle;
                Papa.X = left;
                Papa.Y = top;
                PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 将区域放大指定量
        /// </summary>
        /// <param name="width">放大宽度</param>
        /// <param name="height">放大高度</param>
        public virtual void Inflate(int width, int height)
        {
            int Xresult = Math.Max(Papa.Width + width, 0), Yresult = Math.Max(Papa.Height + height, 0);
            if (MinSize.Width > 0) Xresult = Math.Max(MinSize.Width, Xresult);
            if (MaxSize.Width > 0) Xresult = Math.Min(MaxSize.Width, Xresult);
            if (MinSize.Height > 0) Yresult = Math.Max(MinSize.Height, Yresult);
            if (MaxSize.Height > 0) Yresult = Math.Min(MaxSize.Height, Yresult);

            if (Papa.Width != Xresult || Papa.Height != Yresult)
            {
                Rectangle lastRectangle = Rectangle;
                Papa.Width = Xresult;
                Papa.Height = Yresult;
                PaintRequired?.Invoke(this, lastRectangle);
            }
        }

        /// <summary>
        /// 将区域替换为与目标区域的交集
        /// </summary>
        /// <param name="rect">目标区域</param>
        public virtual void Intersect(Rectangle rect)
        {
            Rectangle result = Rectangle;
            result.Intersect(rect);
            if (MinSize.Width > 0) result.Width = Math.Max(MinSize.Width, result.Width);
            if (MaxSize.Width > 0) result.Width = Math.Min(MaxSize.Width, result.Width);
            if (MinSize.Height > 0) result.Height = Math.Max(MinSize.Height, result.Height);
            if (MaxSize.Height > 0) result.Height = Math.Min(MaxSize.Height, result.Height);

            if (result.X != Papa.Left || result.Y != Papa.Top || result.Width != Papa.Width || result.Height != Papa.Height)
            {
                Rectangle lastRectangle = Rectangle;
                Papa.X = result.Left;
                Papa.Y = result.Top;
                Papa.Width = result.Width;
                Papa.Height = result.Height;
                PaintRequired?.Invoke(this, lastRectangle);
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
                Rectangle lastRectangle = Rectangle;
                Papa.Offset(point);
                PaintRequired?.Invoke(this, lastRectangle);
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

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disposing?.Invoke(this, EventArgs.Empty);

                    this._font = null;
                    this._backgroundImage = null;
                    this._image = null;
                }

                // 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。

                disposedValue = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
