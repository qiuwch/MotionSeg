namespace zap
{
    partial class TestVideoPlayer
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
            this.videoPlayer1 = new zap.VideoPlayer();
            this.SuspendLayout();
            // 
            // videoPlayer1
            // 
            this.videoPlayer1.BackColor = System.Drawing.Color.Black;
            this.videoPlayer1.Location = new System.Drawing.Point(1, 1);
            this.videoPlayer1.Margin = new System.Windows.Forms.Padding(2);
            this.videoPlayer1.MinimumSize = new System.Drawing.Size(526, 94);
            this.videoPlayer1.Name = "videoPlayer1";
            this.videoPlayer1.Size = new System.Drawing.Size(526, 348);
            this.videoPlayer1.TabIndex = 0;
            // 
            // TestVideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 349);
            this.Controls.Add(this.videoPlayer1);
            this.Name = "TestVideoPlayer";
            this.Text = "TestVideoPlayer";
            this.ResumeLayout(false);

        }

        #endregion

        private VideoPlayer videoPlayer1;
    }
}