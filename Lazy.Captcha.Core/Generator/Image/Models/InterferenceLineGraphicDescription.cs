using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lazy.Captcha.Core.Generator.Image.Models
{
    [DebuggerDisplay("Color={Color},Start={Start},Ctrl1={Ctrl1},Ctrl2={Ctrl2},End={End},BlendPercentage={BlendPercentage}")]
    public class InterferenceLineGraphicDescription
    {
        public bool IsAntialias { get; set; } = true;
        public SKColor Color { get; set; }
        public SKPoint Start { get; set; }
        public SKPoint? Ctrl1 { get; set; }
        public SKPoint? Ctrl2 { get; set; }
        public SKPoint End { get; set; }
        public float BlendPercentage { get; set; } = 1;
    }
}
