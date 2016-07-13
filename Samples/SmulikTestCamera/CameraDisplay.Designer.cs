namespace SmulikTestCamera
{
    partial class CameraDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cameraControl = new Camera_NET.CameraControl();
            this.buttonTakePicture = new System.Windows.Forms.Button();
            this.buttonVideoPlip = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cameraControl
            // 
            this.cameraControl.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.cameraControl.DirectShowLogFilepath = "";
            this.cameraControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraControl.Location = new System.Drawing.Point(0, 0);
            this.cameraControl.Name = "cameraControl";
            this.cameraControl.Size = new System.Drawing.Size(503, 370);
            this.cameraControl.TabIndex = 0;
            this.cameraControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraControl_MouseDown);
            // 
            // buttonTakePicture
            // 
            this.buttonTakePicture.Location = new System.Drawing.Point(298, 31);
            this.buttonTakePicture.Name = "buttonTakePicture";
            this.buttonTakePicture.Size = new System.Drawing.Size(75, 40);
            this.buttonTakePicture.TabIndex = 1;
            this.buttonTakePicture.Text = "Take Picture";
            this.buttonTakePicture.UseVisualStyleBackColor = true;
            this.buttonTakePicture.Click += new System.EventHandler(this.buttonTakePicture_Click);
            // 
            // buttonVideoPlip
            // 
            this.buttonVideoPlip.Location = new System.Drawing.Point(32, 31);
            this.buttonVideoPlip.Name = "buttonVideoPlip";
            this.buttonVideoPlip.Size = new System.Drawing.Size(80, 39);
            this.buttonVideoPlip.TabIndex = 3;
            this.buttonVideoPlip.Text = "Take video plip";
            this.buttonVideoPlip.UseVisualStyleBackColor = true;
            this.buttonVideoPlip.Click += new System.EventHandler(this.buttonVideoPlip_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(32, 76);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Test Marge Image";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.BackgroundImage = global::SmulikTestCamera.Properties.Resources._31;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.button1.Location = new System.Drawing.Point(12, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 172);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CameraDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 370);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonVideoPlip);
            this.Controls.Add(this.buttonTakePicture);
            this.Controls.Add(this.cameraControl);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "CameraDisplay";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CameraDisplay_FormClosed);
            this.Load += new System.EventHandler(this.CameraDisplay_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Camera_NET.CameraControl cameraControl;
        private System.Windows.Forms.Button buttonTakePicture;
        private System.Windows.Forms.Button buttonVideoPlip;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

