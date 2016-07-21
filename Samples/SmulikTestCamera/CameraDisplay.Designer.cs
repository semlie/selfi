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
            // CameraDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 370);
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
    }
}

