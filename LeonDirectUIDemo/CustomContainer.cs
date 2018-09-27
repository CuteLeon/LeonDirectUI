using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using LeonDirectUI.Container;
using LeonDirectUI.DUIControl;

namespace LeonDirectUIDemo
{

    public class CustomContainer : ContainerBase
    {

        #region 虚拟控件

        /// <summary>
        /// 标题标签
        /// </summary>
        ControlBase TitleLabel;

        /// <summary>
        /// 预览图像框
        /// </summary>
        ControlBase PreviewImageBox;

        /// <summary>
        /// 描述标签
        /// </summary>
        ControlBase DescriptionLabel;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        ControlBase CloseButton;

        #endregion

        #region 自定义属性

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get => this.TitleLabel.Text; set => this.TitleLabel.Text = value; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => this.DescriptionLabel.Text; set => this.DescriptionLabel.Text = value; }

        /// <summary>
        /// 预览图
        /// </summary>
        public Image PreviewImage { get => this.PreviewImageBox.Image; set => this.PreviewImageBox.Image = value; }

        #endregion

        #region 初始化布局

        /// <summary>
        /// 构造自定义容器类
        /// </summary>
        public CustomContainer() : base() { }

        /// <summary>
        /// 响应式调整布局
        /// </summary>
        public override void InitializeLayout()
        {
            //创建虚拟控件对象 并 维护子虚拟控件列表
            this.Add(this.TitleLabel = new ControlBase());
            this.Add(this.PreviewImageBox = new ControlBase());
            this.Add(this.DescriptionLabel = new ControlBase());
            this.Add(this.CloseButton = new ControlBase());

            this.SuspendPaint();

            this.TitleLabel.Name = "标题标签";
            this.TitleLabel.Text = "我是标题标签哦~";
            this.TitleLabel.ForeColor = Color.Orange;
            this.TitleLabel.BackColor = Color.Gray;
            this.TitleLabel.ShowEllipsis = true;
            this.TitleLabel.Mouseable = true;
            this.TitleLabel.Font = new Font(this.TitleLabel.Font, FontStyle.Bold);
            this.TitleLabel.Click += (s, e) => { Console.WriteLine("点击标题区域"); };
            this.TitleLabel.DoubleClick += (s, e) => { Console.WriteLine("双击标题区域"); };
            this.TitleLabel.MouseEnter += (s, e) => { this.TitleLabel.ForeColor = Color.OrangeRed; };
            this.TitleLabel.MouseLeave += (s, e) => { this.TitleLabel.ForeColor = Color.Orange; };
            this.TitleLabel.MouseDown += (s, e) => { this.TitleLabel.ForeColor = Color.Chocolate; };
            this.TitleLabel.MouseUp += (s, e) => { this.TitleLabel.ForeColor = Color.OrangeRed; };
            this.TitleLabel.BorderColor = Color.SkyBlue;
            this.TitleLabel.BorderSize = 3;
            this.TitleLabel.MaxSize = new Size(0, 28);
            this.TitleLabel.MinSize = new Size(0, 28);
            this.TitleLabel.BorderStyle = ButtonBorderStyle.Dashed;

            this.PreviewImageBox.Name = "预览图像框";
            this.PreviewImageBox.Text = string.Empty;
            this.PreviewImageBox.BackColor = Color.DimGray;
            this.PreviewImageBox.Mouseable = true;
            this.PreviewImageBox.Image = UIResource.white_emoticons_04;
            this.PreviewImageBox.BackgroundImage = UIResource._5_130505152605;
            this.PreviewImageBox.BackgroundImageLayout = ImageLayout.Zoom;
            this.PreviewImageBox.ImageAlign = ContentAlignment.MiddleCenter;
            this.PreviewImageBox.Click += (s, e) => { Console.WriteLine("点击预览图像区域"); };
            this.PreviewImageBox.MouseDown += (s, e) =>
            {
                this.PreviewImageBox.Image?.Dispose();
                this.Invalidate(this.PreviewImageBox.Rectangle);
                this.PreviewImageBox.Image = UIResource.IMG_2579;
            };
            this.PreviewImageBox.MouseUp += (s, e) =>
            {
                this.PreviewImageBox.Image?.Dispose();
                this.Invalidate(this.PreviewImageBox.Rectangle);
                this.PreviewImageBox.Image = UIResource.white_emoticons_04;
            };

            this.DescriptionLabel.Name = "描述标签";
            this.DescriptionLabel.Text = "我是一个自适应的描述标签";
            this.DescriptionLabel.BackColor = Color.LightGray;
            this.DescriptionLabel.ForeColor = Color.Gray;
            this.DescriptionLabel.ShowEllipsis = true;
            this.DescriptionLabel.Click += (s, e) => { Console.WriteLine("点击描述文本区域"); };

            this.CloseButton.Name = "关闭按钮";
            this.CloseButton.Text = "X";
            this.CloseButton.Mouseable = true;
            this.CloseButton.BackColor = Color.DarkGray;
            this.CloseButton.TextAlign = ContentAlignment.MiddleCenter;

            this.CloseButton.MouseEnter += (s, e) =>
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((x) =>
                {
                    this.CloseButton.Text = "确定要关闭吗？";
                    while (this.CloseButton.Width < 120)
                    {
                        this.CloseButton.Width += 20;
                        this.CloseButton.Left = this.DisplayRectangle.Width - this.CloseButton.Width;
                        this.TitleLabel.Width = this.CloseButton.Left;
                        Thread.Sleep(10);
                    }
                    this.CloseButton.Width = 120;
                    this.CloseButton.Left = this.DisplayRectangle.Width - this.CloseButton.Width;
                    this.TitleLabel.Width = this.CloseButton.Left;
                }));
            };
            this.CloseButton.MouseLeave += (s, e) =>
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((x) =>
                {
                    this.CloseButton.Text = "X";
                    while (this.CloseButton.Width > 28)
                    {
                        this.CloseButton.Width -= 20;
                        this.CloseButton.Left = this.DisplayRectangle.Width - this.CloseButton.Width;
                        this.TitleLabel.Width = this.CloseButton.Left;
                        Thread.Sleep(10);
                    }
                    this.CloseButton.Width = 28;
                    this.CloseButton.Left = this.DisplayRectangle.Width - this.CloseButton.Width;
                    this.TitleLabel.Width = this.CloseButton.Left;
                }));
            };
            this.CloseButton.MouseDown += (s, e) => { this.CloseButton.ForeColor = Color.WhiteSmoke; this.CloseButton.BackColor = Color.DimGray; };
            this.CloseButton.MouseUp += (s, e) => { this.CloseButton.ForeColor = Color.Black; this.CloseButton.BackColor = Color.Gray; };
            this.CloseButton.Click += (s, e) => { Application.Exit(); };

            this.ResumePaint();
        }

        /// <summary>
        /// 自定义响应式布局
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void ResetSize(int width, int height)
        {
            this.SuspendPaint();

            this.CloseButton.SetSize(Math.Min(width, 28), Math.Min(height, 28));
            this.CloseButton.SetLocation(width - this.CloseButton.Width, 0);

            this.TitleLabel.Width= this.CloseButton.Left;

            this.PreviewImageBox.SetLocation(0, this.TitleLabel.Bottom);
            this.PreviewImageBox.Height = height - this.PreviewImageBox.Top;
            this.PreviewImageBox.Width = Math.Min(width, this.PreviewImageBox.Height);

            this.DescriptionLabel.SetLocation(this.PreviewImageBox.Right, this.PreviewImageBox.Top);
            this.DescriptionLabel.SetSize(width - this.DescriptionLabel.Left, this.PreviewImageBox.Height);

            this.ResumePaint();
        }

        #endregion

        #region 设计模式效果

        private ControlBase PrimaryControl;
        readonly Random random = new Random();

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            //在这里设置需要突出显示的虚拟控件
            this.PrimaryControl = this.TitleLabel;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.DesignMode)
            {
                //使用斜线遮蔽其他区域
                ControlPaint.DrawSelectionFrame(
                    e.Graphics,
                    true,
                    new Rectangle(0, 0, this.Width, this.Height),
                    this.PrimaryControl?.Rectangle ?? Rectangle.Empty,
                    Color.Black
                    );

                //填充点状矩阵
                //ControlPaint.DrawGrid(
                //    e.Graphics,
                //    new Rectangle(0,0,this.Width,this.Height),
                //    new Size(10,10),Color.Gray);

                ControlPaint.DrawFocusRectangle(e.Graphics, this.PrimaryControl.Rectangle);

                ControlPaint.FillReversibleRectangle(
                    new Rectangle(this.random.Next(1000), this.random.Next(600), this.random.Next(500), this.random.Next(300)),
                    Color.Red
                    );
            }
        }

        #endregion

    }

}
