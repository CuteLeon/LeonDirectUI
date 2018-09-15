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
        IPainter painter = new CommonPainter();
        ContainerBase container = new CustomContainer();

        public DemoForm()
        {
            InitializeComponent();

            container.SetContainer(MainContainer);
            painter.SetGraphics(MainContainer.CreateGraphics());
            container.SetPainter(painter);

            MainContainer.SizeChanged += (s, e) => container.ResetSize(MainContainer.Width, MainContainer.Height);
            MainContainer.Paint += (s, e) => container.PaintAll();
            /*
             */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            container.ResetSize(MainContainer.Width, MainContainer.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            container.PaintAll();
        }
    }
}
