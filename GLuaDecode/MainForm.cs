using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using GLuaDecode;


namespace GLuaDecompress
{
    public class MainForm : Form, IMessageFilter
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private CheckBox checkbox1;
        private CheckBox checkbox2;
        private CheckBox checkbox4;
        private CheckBox checkbox3;
        private Label label9;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private HashSet<Control> ctm = new HashSet<Control>();

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Button button1;
        private Button button2;
        private Button button3;
        private FolderBrowserDialog folderbrowserdialog;
        private OpenFileDialog openfiledialog;
        // private IContainer components;
        private Decode decoder;
        private ProgressBar progressBar;
        private int gLuaMode;

        public MainForm()
        {
            Application.AddMessageFilter(this);

            ctm.Add(this);
            decoder = new Decode();
            this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
        }
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 ctm.Contains(Control.FromHandle(m.HWnd)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Click here and select your files")
            {
                int showfile = (int)openfiledialog.ShowDialog(this);
                button1.Text = "Click here and select destination folder";
                label7.Text = "> selected " + openfiledialog.FileNames.Length + " file(s)";
            }
            else if (openfiledialog.FileNames.Length != 0 && folderbrowserdialog.SelectedPath.Length != 0)
            {
                string[] dFiles = openfiledialog.FileNames;
                string sPath = folderbrowserdialog.SelectedPath;
                progressBar.Value = 0;
                progressBar.Maximum = dFiles.Length * 10;
                foreach (string path1 in dFiles)
                {
                    string path2 = sPath + "\\" + Path.GetFileName(path1);
                    if(File.Exists(path1))
                    {
                        Stream input = (Stream)new FileStream(path1, FileMode.Open, FileAccess.Read);
                        Stream output = (Stream)new FileStream(path2, FileMode.Create, FileAccess.Write);
                        if (decoder.doDecode(input, output, 4, true))
                        {
                           output.SetLength(output.Length - 1L);
                        }

                        input.Close();
                        output.Close();
                        progressBar.Value += 10;
                    }
                }
                button1.Text = "Decoded";
                label5.Visible = true;
            }
            else if (openfiledialog.FileNames.Length != 0)
            {
                int showfold = (int)folderbrowserdialog.ShowDialog(this);
                button1.Text = "Click here to decode your .lua";
                label8.Text = "> path '" + folderbrowserdialog.SelectedPath + "'";
            }
        }

        /*

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = null;
            LinearGradientBrush bg = null;

            int XX = progressBar.Location.X + 1;
            int YY = progressBar.Location.Y;
            int WW = progressBar.Width;
            int HH = progressBar.Height - 6;
            double value = progressBar.Value + 1;
            double min = progressBar.Minimum;
            double max = progressBar.Maximum;
            double scaleFactor = (value - min) / (max - min);
            
            Rectangle rec = new Rectangle(XX, YY, WW, HH);
            Rectangle bgrec = new Rectangle(XX, YY, WW, HH);
            if (ProgressBarRenderer.IsSupported)
            {
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);
            }

            rec.Width = (int)(((rec.Width - 1) * scaleFactor));
            bg = new LinearGradientBrush(bgrec, progressBar.BackColor, progressBar.BackColor, LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(bg, XX, YY, bgrec.Width, bgrec.Height);
            brush = new LinearGradientBrush(rec, progressBar.BackColor, progressBar.ForeColor, LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, XX, YY, rec.Width, rec.Height);

        }
        */

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.folderbrowserdialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openfiledialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkbox1 = new System.Windows.Forms.CheckBox();
            this.checkbox2 = new System.Windows.Forms.CheckBox();
            this.checkbox4 = new System.Windows.Forms.CheckBox();
            this.checkbox3 = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openfiledialog
            // 
            this.openfiledialog.Filter = " |*.lua";
            this.openfiledialog.Multiselect = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(56)))), ((int)(((byte)(117)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Light", 14.75F);
            this.button1.Location = new System.Drawing.Point(107, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(380, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Click here and select your files";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 35.75F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label1.Location = new System.Drawing.Point(147, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 65);
            this.label1.TabIndex = 5;
            this.label1.Text = "GLua";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 35.75F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(56)))), ((int)(((byte)(117)))));
            this.label2.Location = new System.Drawing.Point(261, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 65);
            this.label2.TabIndex = 6;
            this.label2.Text = ".decode";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label3.Location = new System.Drawing.Point(254, 350);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "| View on GitHub";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(56)))), ((int)(((byte)(117)))));
            this.progressBar.Location = new System.Drawing.Point(107, 229);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(380, 2);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label4.Location = new System.Drawing.Point(156, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(283, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Yet another one Garry\'s mod cache decoder";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label7.Location = new System.Drawing.Point(104, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 20);
            this.label7.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label8.Location = new System.Drawing.Point(104, 261);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 20);
            this.label8.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("Segoe UI Light", 35.75F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(56)))), ((int)(((byte)(117)))));
            this.label5.Location = new System.Drawing.Point(488, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 65);
            this.label5.TabIndex = 12;
            this.label5.Text = "⭯";
            this.label5.Visible = false;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Light", 7.75F);
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.button2.Location = new System.Drawing.Point(571, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 25);
            this.button2.TabIndex = 13;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI Light", 7.75F);
            this.button3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.button3.Location = new System.Drawing.Point(540, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(25, 25);
            this.button3.TabIndex = 14;
            this.button3.Text = "—";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label6.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label6.Location = new System.Drawing.Point(224, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "FAQ";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // checkbox1
            // 
            this.checkbox1.AutoSize = true;
            this.checkbox1.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.checkbox1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.checkbox1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkbox1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.checkbox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkbox1.Font = new System.Drawing.Font("Segoe UI Light", 10.25F);
            this.checkbox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.checkbox1.Location = new System.Drawing.Point(110, 240);
            this.checkbox1.Name = "checkbox1";
            this.checkbox1.Size = new System.Drawing.Size(205, 23);
            this.checkbox1.TabIndex = 16;
            this.checkbox1.Text = "Remove line breaks from code";
            this.checkbox1.UseVisualStyleBackColor = true;
            // 
            // checkbox2
            // 
            this.checkbox2.AutoSize = true;
            this.checkbox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkbox2.Font = new System.Drawing.Font("Segoe UI Light", 10.25F);
            this.checkbox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.checkbox2.Location = new System.Drawing.Point(110, 263);
            this.checkbox2.Name = "checkbox2";
            this.checkbox2.Size = new System.Drawing.Size(144, 23);
            this.checkbox2.TabIndex = 17;
            this.checkbox2.Text = "Dead Code Injection";
            this.checkbox2.UseVisualStyleBackColor = true;
            // 
            // checkbox4
            // 
            this.checkbox4.AutoSize = true;
            this.checkbox4.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.checkbox4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.checkbox4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkbox4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.checkbox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkbox4.Font = new System.Drawing.Font("Segoe UI Light", 10.25F);
            this.checkbox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.checkbox4.Location = new System.Drawing.Point(110, 309);
            this.checkbox4.Name = "checkbox4";
            this.checkbox4.Size = new System.Drawing.Size(166, 23);
            this.checkbox4.TabIndex = 19;
            this.checkbox4.Text = "Reverse names, vars, etc";
            this.checkbox4.UseVisualStyleBackColor = true;
            // 
            // checkbox3
            // 
            this.checkbox3.AutoSize = true;
            this.checkbox3.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.checkbox3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.checkbox3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkbox3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.checkbox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkbox3.Font = new System.Drawing.Font("Segoe UI Light", 10.25F);
            this.checkbox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.checkbox3.Location = new System.Drawing.Point(110, 286);
            this.checkbox3.Name = "checkbox3";
            this.checkbox3.Size = new System.Drawing.Size(177, 23);
            this.checkbox3.TabIndex = 18;
            this.checkbox3.Text = "Escape Unicode Sequence";
            this.checkbox3.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Light", 7F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.label9.Location = new System.Drawing.Point(241, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 24);
            this.label9.TabIndex = 20;
            this.label9.Text = "Dev / debug branch\r\nit can don\'t work properly";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.BackgroundImage = global::GLuaDecode.Properties.Resources.test;
            this.ClientSize = new System.Drawing.Size(600, 380);
            this.ControlBox = false;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.checkbox4);
            this.Controls.Add(this.checkbox3);
            this.Controls.Add(this.checkbox2);
            this.Controls.Add(this.checkbox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GLuaDecompress";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label5_Click(object sender, EventArgs e)
        {
            button1.Text = "Click here and select your files";
            label8.Text = "";
            label7.Text = "";
            folderbrowserdialog.SelectedPath = "";
            openfiledialog.FileName = "";
            progressBar.Value = 0;
            label5.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.com");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            new FAQForm().Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // private int gluaMode = 1; // 1 decode, 2 encode, 3 obfuscate
            if (gLuaMode == 1)
            {
                label2.Text = ".encode";
                label4.Text = "Yet another one Garry's mod cache encoder";
                gLuaMode = 2;
            }
            else if(gLuaMode == 2)
            {
                label2.Text = ".obfuscate";
                label4.Text = "Yet another one Garry's mod cache obfuscator";
                gLuaMode = 3;
            }
            else
            {
                label2.Text = ".decode";
                label4.Text = "Yet another one Garry's mod cache decoder";
                gLuaMode = 1;
            }
        }
    }
}