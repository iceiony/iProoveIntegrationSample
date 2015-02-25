namespace Sample
{
    partial class AuthenticateForm
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
            this.AutenticationBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // AutenticationBrowser
            // 
            this.AutenticationBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutenticationBrowser.Location = new System.Drawing.Point(0, 0);
            this.AutenticationBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.AutenticationBrowser.Name = "AutenticationBrowser";
            this.AutenticationBrowser.Size = new System.Drawing.Size(432, 345);
            this.AutenticationBrowser.TabIndex = 0;
            // 
            // AuthenticateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 345);
            this.Controls.Add(this.AutenticationBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "AuthenticateForm";
            this.Text = "Authenticate";
            this.Load += new System.EventHandler(this.Authenticate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser AutenticationBrowser;

    }
}