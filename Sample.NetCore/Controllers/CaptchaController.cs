using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Lazy.Captcha.Core.Generator.Image;
using Lazy.Captcha.Core.Generator.Image.Option;
using Microsoft.AspNetCore.Mvc;

namespace Sample.NetCore.Controllers
{
    [Route("captcha")]
    [ApiController]
    public class CaptchaController : Controller
    {
        private readonly ICaptcha _captcha;

        public CaptchaController(ICaptcha captcha)
        {
            _captcha = captcha;
        }

        [HttpGet]
        public IActionResult Captcha(string id)
        {
            var info = _captcha.Generate(id);
            // ���ܻ��жദ��֤���ҹ���ʱ�䲻һ�����ɴ��ڶ�����������Ĭ�����á���https://gitee.com/pojianbing/lazy-captcha/issues/I4XHGM��
            //var info = _captcha.Generate(id,120);
            var stream = new MemoryStream(info.Bytes);
            return File(stream, "image/gif");
        }

        /// <summary>
        /// ��ʾʱʹ��HttpGet���η��㣬����������ش���
        /// </summary>
        [HttpGet("validate")]
        public bool Validate(string id, string code)
        {
            return _captcha.Validate(id, code);
        }

        /// <summary>
        /// һ����֤����У�飨https://gitee.com/pojianbing/lazy-captcha/issues/I4XHGM��
        /// ��ʾʱʹ��HttpGet���η��㣬����������ش���
        /// </summary>
        [HttpGet("validate2")]
        public bool Validate2(string id, string code)
        {
            return _captcha.Validate(id, code, false);
        }


        /// <summary>
        /// ��ʹ��ע�뷽ʽ��������Ƿ���ǰ�˶�̬չʾ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        [HttpGet("dynamic")]
        public IActionResult DynamicCaptcha(string id, string type, string font, bool textBold = true)
        {
            var captchaService = CaptchaServiceBuilder
              .New()
              .Width(98)
              .Height(35)
              .FontSize(26)
              .CaptchaType(Enum.Parse<CaptchaType>(type))
              .FontFamily(DefaultFontFamilys.Instance.GetFontFamily(font))
              .InterferenceLineCount(2)
              .Animation(false)
              .TextBold(textBold)
              .Build();
            var info = captchaService.Generate(id);
            var stream = new MemoryStream(info.Bytes);
            return File(stream, "image/gif");
        }

        /// <summary>
        /// ��������֤��ͼƬ
        /// </summary>
        /// <returns></returns>
        [HttpGet("image")]
        public IActionResult Image()
        {
            var imageGenerator = new DefaultCaptchaImageGenerator();
            var imageGeneratorOption = new CaptchaImageGeneratorOption()
            {
                // ��������
                ForegroundColors = DefaultColors.Instance.Colors
            };
            var bytes = imageGenerator.Generate("hello", imageGeneratorOption);
            var stream = new MemoryStream(bytes);
            return File(stream, "image/gif");
        }
    }
}