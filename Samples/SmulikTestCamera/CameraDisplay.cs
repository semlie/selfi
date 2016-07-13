using Camera_NET;
using CameraService;
using PrintService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmulikTestCamera
{
    public partial class CameraDisplay : Form
    {
        private CameraChoice _CameraChoice = new CameraChoice();
        private int CounterBase = 3;
        private int counter = 3;
        
        private System.Windows.Forms.Timer Timer; 

        public CameraDisplay()
        {
            InitializeComponent();
            SetDefualtCameraSetting();
        }

        private void buttonTakePicture_Click(object sender, EventArgs e)
        {
            SetTimer();


        }

        private Bitmap TakeSnapshout(){
            if (!cameraControl.CameraCreated)
            {
                throw new Exception("Camera not Set");
            }

            Bitmap bitmap = null;
            try
            {
                bitmap = cameraControl.SnapshotSourceImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error while getting a snapshot");
            }

            return bitmap;
        }

        private void SetDefualtCameraSetting()
        {
            var cameraMeneger = new CameraMeneger();
            var cameraDevice = cameraMeneger.GetDevice();
            var resolution = cameraMeneger.GetResolution(cameraDevice);
            //_CameraChoice.UpdateDeviceList();
            //if (_CameraChoice == null)
            //{
            //    throw new Exception("No camara in this system");
            //}
            //cameraControl.SetCamera(_CameraChoice.Devices[0].Mon, null);
            //ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);
            cameraControl.SetCamera(cameraDevice, resolution);
            cameraControl.MixerEnabled = true;




        }
        private Bitmap ImageTempalte(string background, string[] imagesPath, int maxSizeInPage)
        {

            return new Bitmap(1, 1);
        }
        private void SaveImageToPrintFolder(Bitmap img)
        {

            var printerManager = new PrintManager();
            printerManager.SetTempalte(ImageTempalte);
            var folder = printerManager.GetPrinterFolder();
            img.Save(string.Format(@"{0}\{1}.jpg", folder, Guid.NewGuid().ToString()));

            printerManager.ChackFolder();


        }


        private void PrintImage(Bitmap img)
        {
            SaveImageToPrintFolder(img);

           //  Bitmap b = new Bitmap(img,640,480);
           // using (var g = Graphics.FromImage(b))
           // {
           //     g.DrawString("Hello", this.Font, Brushes.Black, new PointF(0, 0));
           // }
           //
           // PrintDocument pd = new PrintDocument();
           // pd.PrintPage += (object printSender, PrintPageEventArgs printE) =>
           // {
           //     printE.Graphics.DrawImageUnscaled(b, new Point(0, 0));
           // };
           //
           // PrintDialog dialog = new PrintDialog();
           // dialog.ShowDialog();
           // pd.PrinterSettings = dialog.PrinterSettings;
           // 
           // pd.Print();
        }

        private void CameraDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            cameraControl.CloseCamera();
        }

        private void SetTimer()
        {
            counter = CounterBase;
            Timer = new System.Windows.Forms.Timer();
            Timer.Tick += new EventHandler(timer_Tick);
            Timer.Interval = 1000; // 1 second
            Timer.Start();
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //button1.Visible = true;

            if (counter == 0)
            {
                Debug.Write("take picher");
                button1.Visible = false;
                button1.BackgroundImage = SmulikTestCamera.Properties.Resources._31;
                cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources.flash);

                var bitmap = TakeSnapshout();
                cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, new Bitmap(1, 1));

                PrintImage(bitmap);
                Timer.Stop();


            }
            switch (counter)
            {

                case 1:
            button1.BackgroundImage = SmulikTestCamera.Properties.Resources._11;
            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources._11);


                    break;
                case 2:
            button1.BackgroundImage = SmulikTestCamera.Properties.Resources._21;
            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources._21);
                    break;
                case 3:

            button1.BackgroundImage = SmulikTestCamera.Properties.Resources._31;
            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources._31);

                    break;
            }
            counter--;
        }

        private void CameraDisplay_Load(object sender, EventArgs e)
        {

        }

        private void buttonVideoPlip_Click(object sender, EventArgs e)
        {
            var length = 210;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            
            if (!cameraControl.CameraCreated)
                return;
            for (int i = 0; i < length; i++)
            {

                Bitmap bitmap = null;
                try
                {
                    bitmap = cameraControl.SnapshotSourceImage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Error while getting a snapshot");
                }

                if (bitmap == null)
                    return;

                SaveImage(bitmap,i); 
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;  
            Debug.WriteLine(elapsedMs);
        }

        private void SaveImage(Bitmap bitmap,int n)
        {
            var dirPath = @"test";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                    
            }
            bitmap.Save(string.Format(@"{0}/{1}.jpg", dirPath, n.ToString()));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonTakePicture_Click(sender, e);
        }

        private void cameraControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            buttonTakePicture_Click(sender, e);

        }

        private void cameraControl_KeyDown(object sender, KeyEventArgs e)
        {
            buttonTakePicture_Click(sender, e);

        }

        private void cameraControl_MouseDown(object sender, MouseEventArgs e)
        {
            buttonTakePicture_Click(sender, e);

        }
        private void TestPrinterEngine()
        {
            var path  = @"I:\data\test\GitHubSelffy\Camera_Net\Samples\SmulikTestCamera\bin\Debug\vintage_flower_stationery_2.jpg";
            var img = Bitmap.FromFile(path);
            var framePath = @"I:\data\test\GitHubSelffy\Camera_Net\Samples\SmulikTestCamera\bin\Debug\frameBackground.png";
            var images = Directory.GetFiles(@"I:\data\test\GitHubSelffy\Camera_Net\Samples\SmulikTestCamera\bin\Debug\printer\temp").Select(x => Bitmap.FromFile(x)).ToArray();
            var engine = new PrinterEngine();
            var frame = Bitmap.FromFile(framePath);
            var finel = engine.CreateImagesWithBackgroundOnFullPAge(img,images,2,4,frame);
            finel.Save("testmarging.jpg");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestPrinterEngine();
        }

        private Bitmap GenerateColorKeyBitmap(bool useAntiAlias,Image img)
        {
            int w = cameraControl.OutputVideoSize.Width;
            int h = cameraControl.OutputVideoSize.Height;

            if (w <= 0 || h <= 0)
                return null;

            // Create RGB bitmap
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(bmp);

            // configure antialiased drawing or not
            if (useAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            }

            // Clear the bitmap with the color key
            g.Clear(cameraControl.GDIColorKey);

            // Draw zoom text if needed

            // Draw text logo for example
            {
                Font font = new Font("Tahoma", 16);
                Brush textColorKeyed = new SolidBrush(Color.Yellow);
                //img = new Bitmap(img);
                if (w<img.Width||h<img.Height)
                {
                    img = ImageService.ImageServiceManager.ResizeImage(img, w, h);
                }
                var centerPoint = ImageService.ImageServiceManager.GetCenterPoint(bmp, img);
                //new Rectangle(0, 0, img.Width, img.Height)
                g.DrawImage(img,centerPoint);

            }


            // dispose Graphics
            g.Dispose();

            // return the bitmap
            return bmp;
        }

    }
}
