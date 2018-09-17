using LeonDirectUI.Container;
using LeonDirectUI.DUIControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public string Title { get => TitleLabel.Text; set => TitleLabel.Text = value; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => DescriptionLabel.Text; set => DescriptionLabel.Text = value; }

        /// <summary>
        /// 预览图
        /// </summary>
        public Image PreviewImage { get => PreviewImageBox.Image; set => PreviewImageBox.Image = value; }

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
            Controls.Add(TitleLabel = new ControlBase());
            Controls.Add(PreviewImageBox = new ControlBase());
            Controls.Add(DescriptionLabel = new ControlBase());
            Controls.Add(CloseButton = new ControlBase());

            TitleLabel.Name = "标题标签";
            TitleLabel.Text = "我是标题标签哦~";
            TitleLabel.ForeColor = Color.Orange;
            TitleLabel.BackColor = Color.DarkGray;
            TitleLabel.ShowEllipsis = true;
            TitleLabel.Font = new Font(TitleLabel.Font, FontStyle.Bold);
            TitleLabel.Click += (s, e) => { Console.WriteLine("点击标题区域"); };
            TitleLabel.DoubleClick += (s, e) => { Console.WriteLine("双击标题区域"); };

            PreviewImageBox.Name = "预览图像框";
            PreviewImageBox.BackColor = Color.Silver;
            PreviewImageBox.Image = UIResource.white_emoticons_04;
            PreviewImageBox.ImageAlign = ContentAlignment.MiddleCenter;
            PreviewImageBox.Click += (s, e) => { Console.WriteLine("点击预览图像区域"); };

            DescriptionLabel.Name = "描述标签";
            DescriptionLabel.Text = "我是一个自适应的描述标签";
            DescriptionLabel.BackColor = Color.LightGray;
            DescriptionLabel.ForeColor = Color.Gray;
            DescriptionLabel.ShowEllipsis = true;
            DescriptionLabel.Click += (s, e) => { Console.WriteLine("点击描述文本区域"); };

            CloseButton.Name = "关闭按钮";
            CloseButton.Text = "X";
            CloseButton.BackColor = Color.FromArgb(150, 240, 240, 240);
            CloseButton.TextAlign = ContentAlignment.MiddleCenter;
            CloseButton.Click += (s, e) => {
                if (MessageBox.Show("确定要关闭吗？","关闭？", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Application.Exit();
                }
            };
        }
        
        /// <summary>
        /// 自定义响应式布局
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void ResetSize(int width, int height)
        {
            CloseButton.SetSize(Math.Min(width, 28), Math.Min(height, 28));
            CloseButton.SetLocation(width - CloseButton.Width, 0);

            TitleLabel.SetBounds(0, 0, CloseButton.Left, CloseButton.Height);

            PreviewImageBox.SetLocation(0, TitleLabel.Bottom);
            PreviewImageBox.Height = Math.Max(height - PreviewImageBox.Top, 0);
            PreviewImageBox.Width = Math.Min(width, PreviewImageBox.Height);

            DescriptionLabel.SetLocation(PreviewImageBox.Right, PreviewImageBox.Top);
            DescriptionLabel.SetSize(width - DescriptionLabel.Left, PreviewImageBox.Height);
        }

        #endregion

    }

}
