using System.Drawing.Printing;
using ConfiguratinService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace PrintService
{
    public class PrintManager
    {
        private string _spoolerDir;
        private string _framePath;

        private int _spoolerSize;
        private int _imageInWidth;
        private int _imageInHeight;
        private int _borderSize;

        private readonly string TEMPDIR = "temp";
        private readonly string WAITDIR = "redey";
        private readonly string PRINTEDDIR = "printed";
        private readonly string PULLPRINTEDDIR = "fullPagePrinted";

        public PrinterEngine PrinterEngine { get; set; }
        private Func<string, string[], int, Bitmap> Tempalte;

        public string GetPrinterFolder()
        {
            if (!string.IsNullOrEmpty(_spoolerDir))
            {
                InitPrinter();
            }

            return string.Format(@"{0}\{1}", _spoolerDir, WAITDIR);
        }


        public PrintManager()
        {
            InitPrinter();
        }

        public void SetTempalte(Func<string, string[], int, Bitmap> template)
        {

            Tempalte = template;
        }

        private void InitPrinter()
        {
            var spoolerDir = ConfigurationSettingManager.GetConfigurtionSetting("SpoolerDir");
            var spoolerSize = ConfigurationSettingManager.GetConfigurtionSetting("SpoolerDirSize");
            var imagesInWidth = ConfigurationSettingManager.GetConfigurtionSetting("ImagesInWidth");
            var imagesInHeight = ConfigurationSettingManager.GetConfigurtionSetting("ImagesInHeight");
            var borderSize = ConfigurationSettingManager.GetConfigurtionSetting("BorderSize");
            var framePath = ConfigurationSettingManager.GetConfigurtionSetting("FramePath");

            IsDIr(spoolerDir);
            IsDIr(spoolerDir + "\\" + WAITDIR);

            PrinterEngine = new PrinterEngine();


            if (!string.IsNullOrEmpty(framePath))
            {
                _framePath = framePath;

            }
            if (!string.IsNullOrEmpty(borderSize))
            {
                _borderSize = int.Parse(borderSize);
                PrinterEngine.SetBorderSize(_borderSize);

            }
            if (!string.IsNullOrEmpty(spoolerDir))
            {
                _spoolerDir = spoolerDir;

            }
            if (string.IsNullOrEmpty(imagesInHeight) || string.IsNullOrEmpty(imagesInWidth))
            {
                throw new Exception("Config Error set number of images per page");
            }
            else
            {
                _imageInHeight = int.Parse(imagesInHeight);
                _imageInWidth = int.Parse(imagesInWidth);
                _spoolerSize = _imageInWidth * _imageInHeight;

            }

        }

        public void ChackFolder()
        {
            if (!string.IsNullOrEmpty(_spoolerDir) || _spoolerSize == 0)
            {
                InitPrinter();
            }

            IsDIr(_spoolerDir);

            var temp = string.Format(@"{0}\{1}", _spoolerDir, TEMPDIR);
            IsDIr(temp);

            var source = string.Format(@"{0}\{1}", _spoolerDir, WAITDIR);
            IsDIr(source);

            var destination = string.Format(@"{0}\{1}", _spoolerDir, PRINTEDDIR);
            IsDIr(destination);


            var images = Directory.GetFiles(source, "*.jpg", SearchOption.TopDirectoryOnly);
            if (images.Count() >=_spoolerSize)
            {
                //MoveFIleToDir(temp, images);
                MoveAllFIlesToDir(source, temp);

                PrintAllImagesInTempFolder(temp);
                MoveAllFIlesToDir(temp, destination);

            }
        }
        private Image BuildFullPageOfImages(Image background, IEnumerable<Image> images, int numberInWidth, int numberInHeight, string frame = null)
        {
            var frameImage = (frame!=null)?Bitmap.FromFile(frame):null;
            return PrinterEngine.CreateImagesWithBackgroundOnFullPAge(background, images.ToArray(), numberInWidth, numberInHeight, frameImage);
        }
        private void PrintAllImagesInTempFolder(string tempFolder)
        {
            var imagesPathDir =new DirectoryInfo(tempFolder);
            var imagesPath = imagesPathDir.GetFiles( "*.jpg", SearchOption.TopDirectoryOnly).OrderBy(file=>file.LastWriteTime);
            var images = imagesPath.Select(img=>Bitmap.FromFile(img.FullName)).ToArray();
            var imagesChank = Helpers.SplitArray<Image>(images, _spoolerSize);
            
           // PrinterEngine.CreateImagesWithBackgroundOnFullPAge(new Bitmap(1,1),)
            var backgroundImage = ConfigurationSettingManager.GetConfigurtionSetting("Background");
            if (string.IsNullOrEmpty(backgroundImage))
            {
                throw new Exception("No BackGround Setting found");
            }
            var backgraund = Bitmap.FromFile(backgroundImage);
            //@TODO change from const to config 
            var fullPageImages = imagesChank.Select(imgChank => BuildFullPageOfImages(backgraund, imgChank, _imageInWidth, _imageInHeight,_framePath));
            foreach (var page in fullPageImages)
            {
                PrintImage(page);
                var fullPageDir = string.Format(@"{0}/{1}", _spoolerDir, PULLPRINTEDDIR);
                IsDIr(fullPageDir);

                var saveAt = string.Format(@"{0}/{1}.jpg",fullPageDir, Guid.NewGuid().ToString());
                page.Save(saveAt);
            }

            foreach (var item in imagesChank)
            {
                foreach (var i in item)
                {
                    i.Dispose();
                }
            }
        }



        public void PrintImage(Image b)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (object printSender, PrintPageEventArgs printE) =>
            {
                //printE.Graphics.DrawImageUnscaled(b, new Point(0, 0));
                printE.Graphics.DrawImage(b, printE.PageBounds);
            };

            PrintDialog dialog = new PrintDialog();
            dialog.ShowDialog();
            pd.PrinterSettings = dialog.PrinterSettings;
            
            pd.Print();
        }

        private void MoveAllFIlesToDir(string source, string destination)
        {
            var files = Directory.GetFiles(source);
            foreach (var item in files)
            {
                try
                {
                    File.Move(item, string.Format(@"{0}\{1}", destination, Path.GetFileName(item)));

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

        }

        private void MoveFIleToDir(string destination, string[] images)
        {
            for (int i = 0; i < _spoolerSize; i++)
            {
                var filePath = images[i];
                var destFileName = Path.GetFileName(filePath);
                File.Move(filePath, string.Format(@"{0}\{1}", destination, destFileName));
            }
        }

        private void IsDIr(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }



    }
}
