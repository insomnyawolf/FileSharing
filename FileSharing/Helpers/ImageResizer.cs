using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FileSharing.Helpers
{
    public static class ImageResizer
    {

        // More info on https://stackoverflow.com/questions/11020710/is-graphics-drawimage-too-slow-for-bigger-images
        public static Image ResizeImage(Image Origin, Size MaxTargetSize, bool Upscale)
        {
            var originSize = new Size
            {
                Width = Origin.Width,
                Height = Origin.Height,
            };

            var target = AspectRatioResizeCalculator(originSize, MaxTargetSize, Upscale);

            Origin.Mutate(x => x.Resize(target.Width, target.Height, KnownResamplers.Bicubic));

            return Origin;
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
