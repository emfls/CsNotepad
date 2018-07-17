using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CsNotePad
{
    public partial class Form1 : Form
    {
        private bool changed = false;

        private string strFind;
        private FormFind formFind;

        public Form1()
        {
            InitializeComponent();
        }

        private void saveTextToFile()
        {
            if(this.Text=="untitled")
            {
                if(saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    string str = saveFileDialog1.FileName;
                    StreamWriter sw = new StreamWriter(str, false);
                    sw.Write(textBoxNote.Text);
                    sw.Flush();
                    sw.Close();
                    this.Text = str;
                }
            }
            else
            {
                string str = this.Text;
                StreamWriter sw = new StreamWriter(str, false);
                sw.Write(textBoxNote.Text);
                sw.Flush();
                sw.Close();
            }
            this.changed = false;
        }

        private void textBoxNote_TextChanged(object sender, EventArgs e)
        {
            this.changed = true;    // Text updated
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.changed)
            {
                e.Cancel = true;

                var ret = MessageBox.Show("The text was changed.\n Do you want to save?", "NotePad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (ret == DialogResult.Yes)
                {
                    saveTextToFile();
                }
                else if (ret == DialogResult.Cancel)
                {
                    return;
                }
                this.Dispose();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.changed)
            {
                var ret = MessageBox.Show("The text was changed.\n Do you want to save?", "NotePad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if(ret == DialogResult.Yes)
                {
                    saveTextToFile();
                }
                else if(ret == DialogResult.Cancel)
                {
                    return;
                }
            }
            this.textBoxNote.ResetText();
            this.Text = "untitled";
            this.changed = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.changed)
            {
                var ret = MessageBox.Show("The text was changed.\n Do you want to save?", "NotePad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (ret == DialogResult.Yes)
                {
                    saveTextToFile();
                }
                else if (ret == DialogResult.Cancel)
                {
                    return;
                }
            }
            if(openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string str = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(str);
                textBoxNote.Text = sr.ReadToEnd();
                sr.Close();
                this.Text = str;
                this.changed = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveTextToFile();
            this.changed = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string str = saveFileDialog1.FileName;
                StreamWriter sw = new StreamWriter(str, false);
                sw.Write(textBoxNote.Text);
                sw.Flush();
                sw.Close();
                this.Text = str;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.Text = "";
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!(formFind == null || formFind.Visible == false))
            {
                formFind.Focus();
                return;
            }
            formFind = new FormFind();
            if(this.textBoxNote.SelectionLength == 0) 
                formFind.textBoxWord.Text = this.strFind;
            else 
                formFind.textBoxWord.Text = this.textBoxNote.SelectedText;

            formFind.buttonNext.Click += new EventHandler(this.buttonNext_Click);   // Event Registring
            formFind.Show();
        }

        private void findnextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(formFind == null || formFind.Visible == false))
            {
                formFind.textBoxWord.Text = this.strFind;
                this.buttonNext_Click(this, null);  // Event Call
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            int updown = -1;
            string str = this.textBoxNote.Text;
            string findWord = formFind.textBoxWord.Text;

            if(!formFind.checkBoxCap.Checked)
            {
                str = str.ToUpper();
                findWord = findWord.ToUpper();
            }

            if(formFind.radioButtonUp.Checked)
            {
                if(this.textBoxNote.SelectionStart != 0)
                {
                    updown = str.LastIndexOf(findWord, this.textBoxNote.SelectionStart-1);
                }
            }
            else
            {
                updown = str.IndexOf(findWord, this.textBoxNote.SelectionStart + findWord.Length);
            }

            if (updown == -1)
            {
                MessageBox.Show("Cannot find more!", "Notepad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.textBoxNote.Select(updown, findWord.Length);
                strFind = formFind.textBoxWord.Text;
                this.textBoxNote.Focus();
                this.textBoxNote.ScrollToCaret();
            }
            //formFind.Focus();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.SelectAll();
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToLongTimeString();
            string date = DateTime.Now.ToShortDateString();
            textBoxNote.AppendText(time + " " + date + "\n");
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxNote.WordWrap = !textBoxNote.WordWrap;
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxNote.Font = fontDialog1.Font;
            }
        }

        private void viewABoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}
