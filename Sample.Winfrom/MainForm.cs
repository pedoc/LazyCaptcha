using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Lazy.Captcha.Core.Generator.Image.Option;
using Newtonsoft.Json;
using Sample.Winfrom.Helpers;
using Sample.Winfrom.Models;
using Sample.Winfrom.OptionProviders;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lazy.Captcha.Core.Generator.Code;
using MoreLinq;

namespace Sample.Winfrom
{
    public partial class MainForm : Form
    {
        private string captchaId = string.Empty;
        private CaptchaService captchaService;
        private CaptchaOptionsJsonModel captchaOptions;
        private string tpl_File = Application.StartupPath + @"\templates\tpl.html";
        private string config_File = Application.StartupPath + @"\templates\config.html";
        private byte[] currentCaptchaBytes;

        private Dictionary<string, PictureBox> fontPictureBoxMap;

        public MainForm()
        {
            InitializeComponent();
            InitFontPictureBoxMap();
            BindDataSource();
            GenerateCaptcha();
        }

        private void InitFontPictureBoxMap()
        {
            fontPictureBoxMap = new Dictionary<string, PictureBox>
            {
                { "Actionj", this.Actionj_Pbx },
                { "Kaiti", this.Kaiti_Pbx },
                { "Fresnel", this.Fresnel_Pbx },
                { "Prefix", this.Prefix_Pbx },
                { "Ransom", this.Ransom_Pbx },
                { "Scandal", this.Scandal_Pbx },
                { "Epilog", this.Epilog_Pbx },
                { "Headache", this.Headache_Pbx },
                { "Lexo", this.Lexo_Pbx },
                { "Progbot", this.Progbot_Pbx },
                { "Robot", this.Robot_Pbx }
            };
        }

        private void BindDataSource()
        {
            this.CpatchaType_Cbx.DataSource = CaptchaTypeOptionProvider.Provide();
            this.FontFamily_Cbx.DataSource = FontFamilyOptionProvider.Provide();
        }

        private CaptchaOptionsJsonModel GenerateCaptchaOptions()
        {
            var fontFamilyOption = (FontFamilyOption)this.FontFamily_Cbx.SelectedItem;

            return new CaptchaOptionsJsonModel()
            {
                CaptchaType = (int)this.CpatchaType_Cbx.SelectedValue,
                CodeLength = (int)this.Length_Nud.Value,
                ExpirySeconds = 60,
                IgnoreCase = true,
                StoreageKeyPrefix = "",
                ImageOption = new CaptchaImageGeneratorOptionJsonModel
                {
                    Animation = this.Gif_Cbx.Checked,
                    FrameDelay = (int)this.FrameDelay_Ndp.Value,
                    FontSize = (int)this.FontSize_Nud.Value,
                    Width = (int)this.Width_Nud.Value,
                    Height = (int)this.Height_Nud.Value,
                    BubbleMinRadius = (int)this.BubbleMinRadius_Nud.Value,
                    BubbleMaxRadius = (int)this.BubbleMaxRadius_Nud.Value,
                    BubbleCount = (int)this.BubbleCount_Nud.Value,
                    BubbleThickness = (int)this.BubbleThickness_Nud.Value,
                    InterferenceLineCount = (int)this.InterferenceLineCount_Nud.Value,
                    FontFamily = fontFamilyOption == null ? "Actionj" : fontFamilyOption.Text,
                    Quality = (int)this.Quality_Nud.Value,
                    TextBold = TextBold_Cbx.Checked
                }
            };
        }

        private CaptchaService GenerateCaptchaService(CaptchaOptionsJsonModel options)
        {
            var fontFamily = FontFamilyOptionProvider.Provide().First(e => e.Text == options.ImageOption.FontFamily)
                .Value;
            return CaptchaServiceBuilder
                .New()
                .CodeLength(options.CodeLength)
                .CaptchaType((CaptchaType)options.CaptchaType)
                .FontFamily(fontFamily)
                .FontSize(options.ImageOption.FontSize)
                .BubbleCount(options.ImageOption.BubbleCount)
                .BubbleThickness(options.ImageOption.BubbleThickness)
                .BubbleMinRadius(options.ImageOption.BubbleMinRadius)
                .BubbleMaxRadius(options.ImageOption.BubbleMaxRadius)
                .InterferenceLineCount(options.ImageOption.InterferenceLineCount)
                .Animation(options.ImageOption.Animation)
                .FrameDelay(options.ImageOption.FrameDelay)
                .Width(options.ImageOption.Width)
                .Height(options.ImageOption.Height)
                .Quality(options.ImageOption.Quality)
                .TextBold(options.ImageOption.TextBold)
                .BackgroundColors(DefaultColors.Instance.BackgroundColors)
                .Build();
        }

        private CaptchaService GenerateCaptchaService()
        {
            this.captchaOptions = GenerateCaptchaOptions();
            return GenerateCaptchaService(this.captchaOptions);
        }

        private void GenerateConfig()
        {
            // 生成配置
            var wrapper = new CaptchaOptionsWrapper { CaptchaOptions = this.captchaOptions };
            var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            txtConfigJson.Text = json;
        }

        private void RenderCaptcha()
        {
            // 设定宽高
            this.CaptchaPbx.Width = this.captchaOptions.ImageOption.Width;
            this.CaptchaPbx.Height = this.captchaOptions.ImageOption.Height;

            // 第一次比较慢，之后会很快
            captchaId = Guid.NewGuid().ToString();
            CaptchaData data = captchaService.Generate(captchaId, txtContent.Text, 10);
            CaptchaPbx.Image = Image.FromStream(new MemoryStream(data.Bytes));
            currentCaptchaBytes = data.Bytes;
            Code_Lbl.Text = data.Code;

            // 生成全字体
            foreach (var fontFamily in FontFamilyOptionProvider.Provide())
            {
                var option = this.GenerateCaptchaOptions();
                option.ImageOption.Width = 98;
                option.ImageOption.Height = 35;
                option.ImageOption.FontFamily = fontFamily.Text;
                var service = this.GenerateCaptchaService(option);

                var id = Guid.NewGuid().ToString();
                data = service.Generate(id, txtContent.Text, 10);
                var pictureBox = this.fontPictureBoxMap[fontFamily.Text];
                pictureBox.Image = Image.FromStream(new MemoryStream(data.Bytes));
            }
        }

        /// <summary>
        /// 生成验证码图
        /// </summary>
        private void GenerateCaptcha()
        {
            this.captchaService = GenerateCaptchaService();
            this.RenderCaptcha();
            this.GenerateConfig();
        }

        private void CaptchaPbx_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        private void CpatchaType_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void FontFamily_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void FontSize_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void Length_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void BubbleCount_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void BubbleThickness_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void BubbleMinRadius_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void BubbleMaxRadius_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void InterferenceLineCount_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void Gif_Cbx_CheckedChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
            this.FrameDelay_Ndp.Enabled = this.Gif_Cbx.Checked;
        }

        private void FrameDelay_Ndp_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void Width_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void Height_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void Quality_Nud_ValueChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void TextBold_Cbx_CheckedChanged(object sender, EventArgs e)
        {
            this.GenerateCaptcha();
        }

        private void FetchSize_Btn_Click(object sender, EventArgs e)
        {
            var bytesCount = currentCaptchaBytes.Count();
            var size = (bytesCount / 1000.0);
            MessageBox.Show($"{size}kb");
        }

        private void Test_Btn_Click(object sender, EventArgs e)
        {
            this.Test_Btn.Enabled = false;
            var test = new PerformanceTest(this.captchaOptions, 1000);
            test.Complete += Test_Complete;
            test.Progress += Test_Progress;
            test.Start();
        }

        private void Test_Progress(int obj)
        {
            UIHelper.Invoke(this, () => { this.Progress_Lbl.Text = $"%{obj}"; });
        }

        private void Test_Complete(int loopCount, long elapsedMilliseconds, long dataSize)
        {
            var perConsum = Math.Round(elapsedMilliseconds * 1.0 / loopCount, 1);
            var oneSecondsCount = (int)(1000 / perConsum);

            UIHelper.Invoke(this, () => { this.Test_Btn.Enabled = true; });
            UIHelper.ShowMessageBox(this,
                $" 总计生成{loopCount}个\r\n 总计耗时{elapsedMilliseconds}毫秒\r\n 平均每个耗时{perConsum}毫秒\r\n 每秒可生成{oneSecondsCount}个\r\n 图片总计大小{UnitHelper.FormatFileSize(dataSize)}");
        }

        private void txtContent_TextChanged(object sender, EventArgs e)
        {
            if (txtContent.Text.Length > (int)this.Length_Nud.Value) return;

            GenerateCaptcha();
        }

        private void btnTrainGenerate_Click(object sender, EventArgs e)
        {
            const string dir = @"E:\OpenSource\PyCAPTCHA\dataset";
            string trainDir = Path.Combine(dir, "train");
            string valDir = Path.Combine(dir, "val");

            try
            {
                if (Directory.Exists(trainDir))
                    Directory.Delete(trainDir, true);
            }
            catch(Exception ex){}               

            try
            {
                if (Directory.Exists(valDir))
                    Directory.Delete(valDir, true);
            }
            catch(Exception ex){}

            Directory.CreateDirectory(trainDir);
            Directory.CreateDirectory(valDir);

            var count = 10_0000;
            for (int i = 1; i <= count; i++)
            {
                var captchaId = Guid.NewGuid().ToString();
                var code = RandomSeqGenerator2((int)this.Length_Nud.Value);
                CaptchaData data = captchaService.Generate(captchaId, code);
                var bitmap = ApplyFilter(data.Bytes);
                TryWrite(Path.Combine(trainDir, $"{code}.{i}.png"), bitmap);
            }

            for (int i = 1; i <= (int)(count * 0.01); i++)
            {
                var captchaId = Guid.NewGuid().ToString();
                var code = RandomSeqGenerator2((int)this.Length_Nud.Value);
                CaptchaData data = captchaService.Generate(captchaId, code);
                var bitmap = ApplyFilter(data.Bytes);
                TryWrite(Path.Combine(valDir, $"{code}.{i}.png"), bitmap);
            }

            MessageBox.Show(this, @"生成完成");

            static byte[] ApplyFilter(byte[] data)
            {
                return data;
                SKBitmap originalBitmap = SKBitmap.Decode(data);
                SKBitmap blackWhiteBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    for (int y = 0; y < originalBitmap.Height; y++)
                    {
                        // 获取原始图像的像素颜色
                        SKColor originalColor = originalBitmap.GetPixel(x, y);

                        // 计算灰度值
                        int grayValue = (int)(originalColor.Red * 0.299f +
                                              originalColor.Green * 0.587f +
                                              originalColor.Blue * 0.114f);

                        // 根据灰度值判断是否设置为黑色或白色
                        SKColor blackWhiteColor = grayValue > 127 ? SKColors.White : SKColors.Black;

                        // 在黑白图像上设置像素颜色
                        blackWhiteBitmap.SetPixel(x, y, blackWhiteColor);
                    }
                }

                var r = blackWhiteBitmap.Encode(SKEncodedImageFormat.Png, 100);
                return r.ToArray();
            }

            static void TryWrite(string path, byte[] data)
            {
                try
                {
                    File.WriteAllBytes(path, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            static string RandomSeqGenerator(int length)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    var @char = Random.Shared.Next(0, Characters.DEFAULT.Count);
                    sb.Append(Characters.DEFAULT[@char]);
                }

                return sb.ToString();
            }

            static string RandomSeqGenerator2(int length)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    var c1 = (char)Random.Shared.Next(48, 57 + 1);
                    var c2 = (char) Random.Shared.Next(97, 122 + 1);
                    var c3 = (char) Random.Shared.Next(65, 90 + 1);
                    sb.Append(new List<char>() { c1, c2, c3 }.Shuffle().First());
                }

                return sb.ToString();
            }
        }
    }
}