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
            MainContainer.Add(new ControlBase());
        }

        private void MainContainer_ControlAdded(object sender, ControlBase e)
        {
            Console.WriteLine($"增加虚拟控件：{e.Text}");
        }

        private void MainContainer_ControlRemoved(object sender, ControlBase e)
        {
            Console.WriteLine($"移除虚拟控件：{e.Text}");
        }
    }
}
