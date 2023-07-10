using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable SYSLIB0014

namespace ImageDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDownloadDst.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "download");
            CreateDirectoryIfNotExists();
        }

        private void CreateDirectoryIfNotExists()
        {
            if (Directory.Exists(txtDownloadDst.Text) == false)
            {
                Directory.CreateDirectory(txtDownloadDst.Text);
            }
        }

        private void ShowMsg(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(() => { ShowMsg(msg); });
            }
            else
            {
                toolStripStatusLabel1.Text = msg;
                //statusStrip1.Text = msg;
            }
        }

        private volatile bool _stoped;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTargetUrl.Text))
            {
                ShowMsg("下载地址不能为空");
                return;
            }

            if (string.IsNullOrEmpty(txtDownloadDst.Text))
            {
                ShowMsg("下载目录不能为空");
                return;
            }

            CreateDirectoryIfNotExists();

            var url = txtTargetUrl.Text;
            var dst = txtDownloadDst.Text;

            var totalCount = (int)numericUpDown1.Value;

            var count = 0;

            _stoped = false;

            if (!checkBox1.Checked)
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < totalCount; i++)
                    {
                        if (_stoped)
                        {
                            break;
                        }

                        using var wb = new WebClient();
                        var file = Path.Combine(dst, $"{i}.png");
                        wb.DownloadFile(url, file);
                        Interlocked.Increment(ref count);
                        ShowMsg($"已下载={count}");
                    }
                });
            }
            else
            {
                for (var i = 0; i < Environment.ProcessorCount; i++)
                {
                    var chunks = Enumerable
                        .Range(0, totalCount).Chunk((totalCount / Environment.ProcessorCount) + 1)
                        .ToList();
                    foreach (var chunk in chunks)
                    {
                        Task.Factory.StartNew(o =>
                        {
                            var range = (int[])o!;
                            foreach (var i in range)
                            {
                                if (_stoped)
                                {
                                    break;
                                }

                                using var wb = new WebClient();
                                var file = Path.Combine(dst, $"{i}.png");
                                wb.DownloadFile(url, file);
                                Interlocked.Increment(ref count);
                                ShowMsg($"已下载={count}");
                            }
                        }, chunk);
                    }
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _stoped = true;
        }
    }
}