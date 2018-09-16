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
            this.button1 = new System.Windows.Forms.Button();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.customContainer1 = new LeonDirectUIDemo.CustomContainer();
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
            this.MainSplitContainer.Panel1.Controls.Add(this.button1);
            this.MainSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.customContainer1);
            this.MainSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.MainSplitContainer.Size = new System.Drawing.Size(800, 173);
            this.MainSplitContainer.SplitterDistance = 160;
            this.MainSplitContainer.TabIndex = 2;
            // 
            // customContainer1
            // 
            this.customContainer1.Description = "我是一个自适应的描述标签";
            this.customContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customContainer1.Location = new System.Drawing.Point(5, 5);
            this.customContainer1.Name = "customContainer1";
            this.customContainer1.PreviewImage = global::LeonDirectUIDemo.UIResource.white_emoticons_04;
            this.customContainer1.Size = new System.Drawing.Size(626, 163);
            this.customContainer1.TabIndex = 0;
            this.customContainer1.Title = "我是标题标签哦~";
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 173);
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
        private CustomContainer customContainer1;
    }
}

