namespace zap
{
    partial class PreviewBox
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
            this.pb_frame = new System.Windows.Forms.PictureBox();
            this.lbl_ratio = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_frame)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_frame
            // 
            this.pb_frame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_frame.Location = new System.Drawing.Point(0, 0);
            this.pb_frame.Name = "pb_frame";
            this.pb_frame.Size = new System.Drawing.Size(509, 319);
            this.pb_frame.TabIndex = 0;
            this.pb_frame.TabStop = false;
            // 
            // lbl_ratio
            // 
            this.lbl_ratio.AutoSize = true;
            this.lbl_ratio.Location = new System.Drawing.Point(402, 13);
            this.lbl_ratio.Name = "lbl_ratio";
            this.lbl_ratio.Size = new System.Drawing.Size(35, 13);
            this.lbl_ratio.TabIndex = 1;
            this.lbl_ratio.Text = "label1";
            // 
            // PreviewBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 319);
            this.Controls.Add(this.lbl_ratio);
            this.Controls.Add(this.pb_frame);
            this.Name = "PreviewBox";
            this.Text = "PreviewBox";
            ((System.ComponentModel.ISupportInitialize)(this.pb_frame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_frame;
        private System.Windows.Forms.Label lbl_ratio;
    }
}