using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HbTech;
using System.Threading;


namespace zap
{
    public partial class PreviewBox : Form
    {
        public PreviewBox()
        {
            InitializeComponent();
            Thread capture_thread = new Thread(
                () =>
                {

                    VideoCaptureWrapper wrapper = new VideoCaptureWrapper();
                    wrapper.Init("mv2_001.avi");
                    
                    while (true)
                    {
                        Bitmap frame = wrapper.CaptureFrame();
                        this.Invoke(new MethodInvoker(() =>
                            {
                                this.pb_frame.Image = frame;
                                double current_frame = wrapper.GetCurrentFrames();
                                double total_frame = wrapper.GetTotalFrames();

                                // double ratio = wrapper.GetCurrentMSEC();
                                lbl_ratio.Text = String.Format("{0}/{1}", current_frame, total_frame);
                            }));
                    }
                });
            capture_thread.Start();
        }

    }
}
