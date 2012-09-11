using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HbTech;

public enum PlayState
{
    Close,
    Pause,
    Play
}

namespace zap
{
    public partial class MovieScreen : UserControl
    {
        private Timer movie_timer;
        private VideoCaptureWrapper wrapper;
        public PlayState State { get { return this.play_state; }}
        private PlayState play_state = PlayState.Close;
        

        public MovieScreen()
        {
            InitializeComponent();

            movie_timer = new Timer();
            movie_timer.Tick += new EventHandler(movie_timer_Tick);
            play_state = PlayState.Close;
        }

        void movie_timer_Tick(object sender, EventArgs e)
        {
            if (play_state == PlayState.Play)
            {
                Bitmap frame = wrapper.CaptureFrame();
                if (frame == null)
                {
                    this.Stop();
                }
                this.pb_preview_box.Image = frame;
            }
        }

        public bool OpenFile(String filename)
        {
            wrapper = new VideoCaptureWrapper();
            if (wrapper.Init(filename))
            {
                movie_timer.Start();
                int frame_rate = wrapper.GetFrameRate();
                movie_timer.Interval = (int)(1000 / frame_rate);
                this.Play();
                return true;
            }
            return false;
        }


        public void Play()
        {
            play_state = PlayState.Play;
        }

        public void Pause()
        {
            play_state = PlayState.Pause;
        }

        public void Stop()
        {
            play_state = PlayState.Close;
            movie_timer.Stop();
        }

        public int FrameWidth 
        {
            get
            {
                return wrapper.GetWidth();
            }
        }
        public int FrameHeight 
        {
            get
            {
                return wrapper.GetHeight();
            }
        }

        public int FrameRate
        {
            get
            {
                return wrapper.GetFrameRate();
            }
        }

        public int FrameCount
        {
            get
            {
                return wrapper.GetTotalFrames();
            }
        }
        public double CurrentPosition 
        {
            get
            {
                if (play_state == PlayState.Close) return 0;
                double current_frame = wrapper.GetCurrentFrames(), frame_count = wrapper.GetTotalFrames();
                int fps = wrapper.GetFrameRate();
                // double ratio = wrapper.GetCurrentRatio();  // this is not available
                return current_frame / fps;
            }
            /* Set as property will cause unexpected exception for visual studio designer */
            /*
            set
            {
                if (play_state == PlayState.Close) return;
                int fps = wrapper.GetFrameRate();
                current_position = value;
                wrapper.SetCurrentFrames(current_position * fps);
            }
             */ 
        }

        public void SetPosition(double position)
        {
            if (play_state == PlayState.Close) return;
            int fps = wrapper.GetFrameRate();
            wrapper.SetCurrentFrames(position * fps);
        }



    }
}
