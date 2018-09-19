using LeonDirectUI.Container;
using LeonDirectUI.DUIControl;
using LeonDirectUI.Interface;
using LeonDirectUI.Painter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUIDemo
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowCloneForm();
        }

        private void MainContainer_ControlAdded(object sender, ControlBase e)
        {
            Console.WriteLine($"MainContainer 增加虚拟控件：{e.Text}");
        }

        private void MainContainer_ControlRemoved(object sender, ControlBase e)
        {
            Console.WriteLine($"MainContainer 移除虚拟控件：{e.Text}");
        }

        /// <summary>
        /// 显示克隆用例
        /// </summary>
        private void ShowCloneForm()
        {
            //克隆窗口
            Form cloneForm = new Form()
            {
                Text = $"克隆窗口：{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffff")}",
                AutoSize = true, //根据容器自动调整窗体大小
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
            };

            //克隆容器
            CloneContainerBase container = new CloneContainerBase(null)
            {
                Parent = cloneForm,
                Margin = new Padding(0, 0, 0, 0)
            };
            //动态创建的控件需要手动释放，否则无法通过容器基类的Dispose方法解除虚拟控件绘制请求事件的绑定
            cloneForm.HandleDestroyed += (s, e) => { container.Dispose(); };

            //绑定待克隆的目标容器
            container.CloneContainer(MainContainer);

            cloneForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while(MainContainer.Controls.Length>0)
                MainContainer[0].Dispose();

            MainContainer.Dispose();
        }
    }
}
