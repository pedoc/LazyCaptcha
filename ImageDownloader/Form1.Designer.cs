using System.Drawing;
using System.Windows.Forms;

namespace ImageDownloader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtTargetUrl = new TextBox();
            txtDownloadDst = new TextBox();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            label3 = new Label();
            btnStart = new Button();
            btnStop = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            checkBox1 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 25);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "下载地址";
            // 
            // txtTargetUrl
            // 
            txtTargetUrl.Location = new Point(74, 22);
            txtTargetUrl.Name = "txtTargetUrl";
            txtTargetUrl.Size = new Size(362, 23);
            txtTargetUrl.TabIndex = 1;
            txtTargetUrl.Text = "https://learn.microsoft.com/en-us/dotnet/standard/threading/media/vs-cancellationtoken.png";
            // 
            // txtDownloadDst
            // 
            txtDownloadDst.Location = new Point(74, 63);
            txtDownloadDst.Name = "txtDownloadDst";
            txtDownloadDst.Size = new Size(362, 23);
            txtDownloadDst.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 66);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 2;
            label2.Text = "保存目录";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(74, 101);
            numericUpDown1.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(69, 23);
            numericUpDown1.TabIndex = 4;
            numericUpDown1.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 104);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 5;
            label3.Text = "下载数量";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(242, 101);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 6;
            btnStart.Text = "开始下载";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(344, 101);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 7;
            btnStop.Text = "停止下载";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 143);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(446, 22);
            statusStrip1.TabIndex = 8;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(159, 102);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(75, 21);
            checkBox1.TabIndex = 9;
            checkBox1.Text = "并行下载";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(446, 165);
            Controls.Add(checkBox1);
            Controls.Add(statusStrip1);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(label3);
            Controls.Add(numericUpDown1);
            Controls.Add(txtDownloadDst);
            Controls.Add(label2);
            Controls.Add(txtTargetUrl);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form1";
            Text = "图片下载器";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtTargetUrl;
        private TextBox txtDownloadDst;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private Label label3;
        private Button btnStart;
        private Button btnStop;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private CheckBox checkBox1;
    }
}