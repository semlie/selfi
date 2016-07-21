using ImageService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService
{
    public class PrinterEngine 
    {
        public ImageServiceManager ImageServices { get; set; }
        public PrinterEngine()
        {
            ImageServices = new ImageServiceManager();
            
        }
        private int _borderSize = 45;
        public void SetBorderSize(int pixel)
        {
            _borderSize = pixel;
        }



        public Bitmap CreateImagesWithBackgroundOnFullPAge(Image background, Image[] images, int numberInWidth, int numberInHeight, Image frame = null)
        {
            if (numberInHeight * numberInWidth < images.Count())
            {
                throw new Exception("Number of images bigger then page size");
            }
            else
            {
                //fix sizeof frame
                var size = ImageServices.MaxSizeOfImagesInFullPage(numberInWidth, numberInHeight, background.Size);
                var intSize = Size.Round(size);

                //fit Frame to size
                if (frame == null)
                {
                    frame = new Bitmap(intSize.Width, intSize.Height);
                }
                var newFrameSize = Size.Round(ImageServices.FitImageDimationsToSize(frame, size));
                var newFrame = ResizeImage(frame, newFrameSize.Width, newFrameSize.Height);

                var imagesFitFrame = images.Select(img => ImageServices.FitImageToFrame(newFrame,img,_borderSize));
                var imagesWithFrame = imagesFitFrame.Select(img => ImageServices.AddFrameToImage(newFrame, img)).ToArray();


                for (int i = 0,k=0; i < numberInHeight; i++)
                {
                    for (int j = 0; j < numberInWidth; j++)
                    {
                        if (imagesWithFrame.Count()>i+j)
                        {
                            buildFullPageOfImages(background, imagesWithFrame[k], j, i, intSize);
                            k++;
                        }
                    }
                }
            }
            return (Bitmap)background;
        }


        private void buildFullPageOfImages(Image background, Image bitmap, int i, int j, Size size)
        {
            var potion = new PointF(size.Width * i, size.Height * j);
            Bitmap b = ResizeImage(bitmap, size.Width, size.Height);
            using (var g = Graphics.FromImage(background))
            {
                g.DrawImage(b, potion);
            }

        }


        public Bitmap ResizeImage(Image image, int width, int height)
        {
            return ImageServiceManager.ResizeImage(image, width, height);
        }
    }
}
