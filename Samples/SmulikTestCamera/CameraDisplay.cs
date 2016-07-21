using Camera_NET;
using CameraService;
using ConfiguratinService;
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
        private bool SEMAPHORE = false;
        private int _numberOfCopiesEachSnapshout;
        private System.Windows.Forms.Timer Timer; 

        public CameraDisplay()
        {
            InitializeComponent();
            SetDefualtCameraSetting();
        }

        private void buttonTakePicture_Click(object sender, EventArgs e)
        {


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
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            var cameraMeneger = new CameraMeneger();
            var cameraDevice = cameraMeneger.GetDevice();
            var resolution = cameraMeneger.GetResolution(cameraDevice);
            var numberOfCopies = ConfigurationSettingManager.GetConfigurtionSetting("NumberOfImagesPerSnapshout");
            if (!string.IsNullOrEmpty(numberOfCopies))
            {
                _numberOfCopiesEachSnapshout = int.Parse(numberOfCopies);
            }


            cameraControl.SetCamera(cameraDevice, resolution);
            cameraControl.MixerEnabled = true;




        }

        private void SaveImageToPrintFolder(Bitmap img)
        {
            var printerManager = new PrintManager();
            var folder = printerManager.GetPrinterFolder();
            var fileName = Guid.NewGuid().ToString();
            for (int i = 0; i < _numberOfCopiesEachSnapshout; i++)
            {
                img.Save(string.Format(@"{0}\{1}{2}.jpg", folder, fileName, i.ToString()));
                
            }          
             //img.Save(string.Format(@"{0}\{1}{2}.jpg", folder, fileName, "2"));

            printerManager.ChackFolder();


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
                Timer.Stop();
                Debug.Write("take picher");

                cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources.flash);

                var bitmap = TakeSnapshout();
                cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, new Bitmap(1, 1));

                SaveImageToPrintFolder(bitmap);
                
                SEMAPHORE = false;

            }
            switch (counter)
            {

                case 1:
            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources._11);


                    break;
                case 2:
            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources._21);
                    break;
                case 3:

            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false, SmulikTestCamera.Properties.Resources._31);

                    break;
            }
            counter--;
        }

        private void CameraDisplay_Load(object sender, EventArgs e)
        {

        }


        private void cameraControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!SEMAPHORE)
            {
                SEMAPHORE = true;
                SetTimer();

            }
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
