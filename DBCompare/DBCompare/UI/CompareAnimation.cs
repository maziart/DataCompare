using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBCompare.UI
{
    public partial class CompareAnimation : UserControl
    {
        public CompareAnimation()
        {
            InitializeComponent();
            Images = new[] { DBCompare.Properties.Resources.Comp1, DBCompare.Properties.Resources.Comp2, DBCompare.Properties.Resources.Comp3, DBCompare.Properties.Resources.Comp2 };
            Index = 0;
            ShowPicture();
        }

        private void ShowPicture()
        {
            pictureBox1.Image = Images[Index % 4];
            Index++;
        }
        private Image[] Images;
        private int Index;

        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowPicture();
        }
        public void Start()
        {
            timer1.Start();
        }
        public void SetImage(Image image)
        {
            timer1.Stop();
            if (image == null)
            {
                ShowPicture();
            }
            else
            {
                pictureBox1.Image = image;
            }
        }
    }
}
