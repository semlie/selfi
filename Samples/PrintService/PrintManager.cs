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

namespace PrintService
{
    public class PrintManager
    {
        private string _spoolerDir;
        private int _spoolerSize;
        private readonly string TEMPDIR = "temp";
        private readonly string WAITDIR = "redey";
        private readonly string PRINTEDDIR = "printed";

        private Func<string, string[], int, Bitmap> Tempalte;

        public string GetPrinterFolder()
        {
            if (string.IsNullOrEmpty(_spoolerDir))
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
            if (!string.IsNullOrEmpty(spoolerDir))
            {
                _spoolerDir = spoolerDir;

            }
            if (!string.IsNullOrEmpty(spoolerSize))
            {
                _spoolerSize = int.Parse(spoolerSize);

            }

        }

        public void ChackFolder()
        {
            if (!string.IsNullOrEmpty(_spoolerDir) || _spoolerSize == 0)
            {
                InitPrinter();
            }

            IsDIr(_spoolerDir);
            //chack all dirs is exist 
            //take only number of spoolerSize and move to temp dir 
            // run template function and build one image to  print 
            // print them
            //move to printed dir 
            var temp = string.Format(@"{0}\{1}", _spoolerDir, TEMPDIR);
            IsDIr(temp);

            var source = string.Format(@"{0}\{1}", _spoolerDir, WAITDIR);
            IsDIr(source);

            var destination = string.Format(@"{0}\{1}", _spoolerDir, PRINTEDDIR);
            IsDIr(destination);


            var images = Directory.GetFiles(source, "*.jpg", SearchOption.TopDirectoryOnly);
            while (images.Count() >=_spoolerSize)
            {
                MoveFIleToDir(temp, images);
                PrintAllImagesInTempFolder(temp);
                MoveAllFIlesToDir(temp, destination);
                images = Directory.GetFiles(source, "*.jpg", SearchOption.TopDirectoryOnly);

            }
        }

        private void PrintAllImagesInTempFolder(string tempFolder)
        {
            var images = Directory.GetFiles(tempFolder, "*.jpg", SearchOption.TopDirectoryOnly);
            var backgroundImage = ConfigurationSettingManager.GetConfigurtionSetting("Background");
            if (string.IsNullOrEmpty(backgroundImage))
            {
                throw new Exception("No BackGround Setting found");
            }
            var printingImage = ImagesWithDesign(backgroundImage, images);

            PrintImage(printingImage);
        }

        private Bitmap ImagesWithDesign(string backgroundImage, string[] images)
        {
            if (Tempalte == null)
            {
                throw new Exception("Template Function Not seted");
            }
            var finelImage = Tempalte(backgroundImage, images, _spoolerSize);

            return finelImage;

        }



        public void PrintImage(Bitmap b)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (object printSender, PrintPageEventArgs printE) =>
            {
                printE.Graphics.DrawImageUnscaled(b, new Point(0, 0));
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
                catch (Exception)
                {
                    
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
