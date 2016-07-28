using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageServiceManager
    {

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height,PixelFormat.Format64bppPArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public static PointF GetCenterPoint(Image background, Image image)
        {
            if ((background.Width - image.Width) < 0 || (background.Height - image.Height) < 0)
            {
                throw new Exception("Background small then image");
            }
            return new PointF((background.Width - image.Width) / 2, (background.Height - image.Height) / 2);
        }

        public SizeF MaxSizeOfImagesInFullPage(int MaxNumberWidth, int MaxNumberHeight, SizeF backgroundSize)
        {
            var width = MaxSizeOfImagesInWidth(MaxNumberWidth, backgroundSize);
            var height = MaxSizeOfImagesInHeight(MaxNumberHeight, backgroundSize);
            return new SizeF(width, height);

        }
        public float MaxSizeOfImagesInWidth(int MaxNumber, SizeF backgroundSize)
        {
            return backgroundSize.Width / MaxNumber;
        }

        public float MaxSizeOfImagesInHeight(int MaxNumber, SizeF backgroundSize)
        {
            return backgroundSize.Height / MaxNumber;
        }

        public Bitmap FitImageToFrame(Image frame, Image img, int borderSize)
        {
            if ((frame.Width - (img.Width + borderSize)) < 0 || (frame.Height - (img.Height + borderSize)) < 0)
            {
                var size =Size.Round( FitImagesDimationsSaveAspectRatio(frame, img));
                img = ResizeImage(img, size.Width - borderSize, size.Height - borderSize);
            }
            return (Bitmap)img;
        }

        public Bitmap AddFrameToImage(Image frame, Image img)
        {
            var potion = ImageServiceManager.GetCenterPoint(frame, img);
            using (var g = Graphics.FromImage(frame))
            {
                g.DrawImage(img, potion);
            }
            return new Bitmap(frame);
        }

        public SizeF FitImagesDimationsSaveAspectRatio(Image sourceImage, Image destImage)
        {
            float srcProportion = sourceImage.Height*1.0f / sourceImage.Width;
            float destProportion = destImage.Height*1.0f / destImage.Width;
            if (srcProportion<destProportion)
            {
                return MultipaleSize(destImage.Size, sourceImage.Height * 1.0f / destImage.Height);
            }
            else
            {
                return MultipaleSize(destImage.Size, sourceImage.Width * 1.0f / destImage.Width);

            }
        }

        private SizeF MultipaleSize(Size size, float multi)
        {
            var width = size.Width * multi;
            var height = size.Height * multi;
            return new SizeF(width, height);
        }

        public SizeF FitImageDimationsToSize(Image sourceImage, SizeF maxSizeAvilable)
        {
            var newW = sourceImage.Width / maxSizeAvilable.Width;
            var newH = sourceImage.Height / maxSizeAvilable.Height;
            if (newH >= newW)
            {
                return new SizeF(sourceImage.Width / newH, sourceImage.Height / newH);
            }
            else
            {
                return new SizeF(sourceImage.Width / newW, sourceImage.Height / newW);
            }

        }
    }
}
