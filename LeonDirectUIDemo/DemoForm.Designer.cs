namespace LeonDirectUIDemo
{
    partial class DemoForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
            this.button1 = new System.Windows.Forms.Button();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.MainContainer = new LeonDirectUIDemo.CustomContainer();
            this.sizableContainer1 = new LeonDirectUI.Container.SizableContainer();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(5, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "测试按钮";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MainSplitContainer.Panel1.Controls.Add(this.button2);
            this.MainSplitContainer.Panel1.Controls.Add(this.button1);
            this.MainSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.MainContainer);
            this.MainSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.MainSplitContainer.Size = new System.Drawing.Size(800, 173);
            this.MainSplitContainer.SplitterDistance = 160;
            this.MainSplitContainer.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(5, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "测试按钮";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainContainer
            // 
            this.MainContainer.Description = "我是一个自适应的描述标签";
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(5, 5);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.PreviewImage = ((System.Drawing.Image)(resources.GetObject("MainContainer.PreviewImage")));
            this.MainContainer.Size = new System.Drawing.Size(626, 163);
            this.MainContainer.TabIndex = 0;
            this.MainContainer.Text = "customContainer1";
            this.MainContainer.Title = "我是标题标签哦~";
            this.MainContainer.ControlAdded += new System.EventHandler<LeonDirectUI.DUIControl.ControlBase>(this.MainContainer_ControlAdded);
            this.MainContainer.ControlRemoved += new System.EventHandler<LeonDirectUI.DUIControl.ControlBase>(this.MainContainer_ControlRemoved);
            // 
            // sizableContainer1
            // 
            this.sizableContainer1.BackColor = System.Drawing.Color.Salmon;
            this.sizableContainer1.BackgroundImage = global::LeonDirectUIDemo.UIResource.IMG_2579;
            this.sizableContainer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.sizableContainer1.Dragable = true;
            this.sizableContainer1.Location = new System.Drawing.Point(20, 20);
            this.sizableContainer1.MinimumSize = new System.Drawing.Size(90, 160);
            this.sizableContainer1.Name = "sizableContainer1";
            this.sizableContainer1.Sizable = true;
            this.sizableContainer1.Size = new System.Drawing.Size(112, 200);
            this.sizableContainer1.TabIndex = 3;
            this.sizableContainer1.Text = "sizableContainer1";
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 173);
            this.Controls.Add(this.sizableContainer1);
            this.Controls.Add(this.MainSplitContainer);
            this.Name = "DemoForm";
            this.Text = "Form1";
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private CustomContainer MainContainer;
        private System.Windows.Forms.Button button2;
        private LeonDirectUI.Container.SizableContainer sizableContainer1;
    }
}

