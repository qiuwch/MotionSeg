namespace zap
{
    partial class MovieScreen
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
            this.pb_preview_box = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview_box)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_preview_box
            // 
            this.pb_preview_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_preview_box.Location = new System.Drawing.Point(0, 0);
            this.pb_preview_box.Name = "pb_preview_box";
            this.pb_preview_box.Size = new System.Drawing.Size(298, 183);
            this.pb_preview_box.TabIndex = 0;
            this.pb_preview_box.TabStop = false;
            // 
            // MovieScreen
            // 
            this.Controls.Add(this.pb_preview_box);
            this.Name = "MovieScreen";
            this.Size = new System.Drawing.Size(298, 183);
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview_box)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_preview_box;
    }
}