using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEditor
{
    public partial class EditorWnd : Form
    {
        public EditorWnd()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = new System.IO.FileStream(openFileDialog1.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                textBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.FileStream fs = new System.IO.FileStream(openFileDialog1.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, Encoding.UTF8);
            sw.Write(textBox1.Text);
            sw.Close();
        }
    }
}
