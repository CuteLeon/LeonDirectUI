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
        IPaint painter = new CommonPainter();
        ContainerBase container = new ContainerBase();
        ControlBase control_0 = new ControlBase();
        ControlBase control_1 = new ControlBase();
        ControlBase control_2 = new ControlBase();

        public DemoForm()
        {
            InitializeComponent();

            container.SetContainer(label1);
            painter.SetGraphics(label1.CreateGraphics());
            control_0.SetPainter(painter);
            control_1.SetPainter(painter);
            control_2.SetPainter(painter);

            control_0.Rectangle = new Rectangle(0,0,200,100);

            container.Controls.Add(control_0);
            container.Controls.Add(control_1);
            container.Controls.Add(control_2);
        }

        protected override void WndProc(ref Message m)
        {
            //container.MessageReceived(ref m);
            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ControlBase control in container.Controls)
            {
                control.Paint();
            }
        }

    }
}
