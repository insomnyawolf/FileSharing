
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FileSharing.Helpers
{
    public static class ImageResizer
    {

        // More info on https://stackoverflow.com/questions/11020710/is-graphics-drawimage-too-slow-for-bigger-images
        public static Bitmap ResizeImage(Bitmap Origin, Size MaxTargetSize, bool Upscale)
        {

            var originSize = new Size
            {
                Width = Origin.Width,
                Height = Origin.Height,
            };

            var target = AspectRatioResizeCalculator(originSize, MaxTargetSize, Upscale);

            var DestImage = new Bitmap(target.Width, target.Height);

            using var Graphics = System.Drawing.Graphics.FromImage(DestImage);
            Graphics.CompositingMode = CompositingMode.SourceCopy;
            Graphics.CompositingQuality = CompositingQuality.HighQuality;

            // Quality
            //Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Balance
            //Graphics.InterpolationMode = InterpolationMode.Bilinear;

            // Speed
            Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var ImageAttributes = new ImageAttributes();
            ImageAttributes.SetWrapMode(WrapMode.TileFlipXY);

            var destRect = new Rectangle(0, 0, DestImage.Width, DestImage.Height);

            Graphics.DrawImage(Origin, destRect, 0, 0, Origin.Width, Origin.Height, GraphicsUnit.Pixel, ImageAttributes);

            return DestImage;
        }

        private static Size AspectRatioResizeCalculator(Size Origin, Size Target, bool Upscale = false)
        {
            var width = Origin.Width;
            var height = Origin.Height;

            decimal coefficientFitWidth = CoefficientChange(width, Target.Width);
            decimal coefficientFitHeight = CoefficientChange(height, Target.Height);

            decimal coefficient = coefficientFitWidth < coefficientFitHeight ? coefficientFitWidth : coefficientFitHeight;

            // Avoid Upscaling
            if (!Upscale && coefficient > 1)
            {
                return Origin;
            }

            width = decimal.ToInt32(coefficient * Origin.Width);
            height = decimal.ToInt32(coefficient * Origin.Height);

            // Images must have at least 1 px on both sides
            // This fixes it
            // I know it's trash but i can't figure a better way of doing this ;-;
            coefficient = 0;
            if (width < 1)
            {
                coefficient = CoefficientChange(width, 1);
            }
            else if (height < 1)
            {
                coefficient = CoefficientChange(height, 1);
            }

            if (coefficient != 0)
            {
                height = decimal.ToInt32(coefficient * Origin.Width);
                width = decimal.ToInt32(coefficient * Origin.Height);
            }

            return new Size
            {
                Width = width,
                Height = height,
            };
        }

        private static decimal CoefficientChange(int valorInicial, int valorFinal)
        {
            return 100M / valorInicial * valorFinal / 100;
        }
    }
}
