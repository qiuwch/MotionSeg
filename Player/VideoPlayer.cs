//+--------------------------------------------------------------------------+
//|                                                                          |
//|                             zMoviePlayer                                 |
//|                         DirectX Movie Player                             |
//|                                                                          |
//|                             Version 1.04                                 |
//+--------------------------------------------------------------------------+
//|                                                                          |
//|                         Author Patrice TERRIER                           |
//|                           copyright (c) 2007                             |
//|                                                                          |
//|                        pterrier@zapsolution.com                          |
//|                                                                          |
//|                          www.zapsolution.com                             |
//|                                                                          |
//+--------------------------------------------------------------------------+
//|                  Project started on : 04-20-2007 (MM-DD-YYYY)            |
//|                        Last revised : 05-21-2007 (MM-DD-YYYY)            |
//+--------------------------------------------------------------------------+

using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

using Win32;
// using SkinEngine;

// using Microsoft.DirectX.AudioVideoPlayback;

using Microsoft.Win32;
using System.IO;
using HbTech;

namespace zap
{
    public partial class VideoPlayer : UserControl
    {
        IntPtr hFORM_Main = IntPtr.Zero;         // Default to Zero

        bool ScrollEnable = false;
        bool HDmovie = false;
        bool Movie_Timer_Enabled = false;
        // Video Movie;
        // VideoCaptureWrapper Movie = new VideoCaptureWrapper();
        Size MovieDefaultSize;
        string movieDuration;
        float Aspect;
        int hours, minutes, seconds, MovieDuration, MovieCurrentPosition;
        int CommandPanelHeight;
        int InitialClientWidth;

        public VideoPlayer()
        {
            hFORM_Main = this.Handle;

            InitializeComponent();

            Size MovieArea;
            BTN_Play.Enabled = BTN_Pause.Enabled = BTN_Stop.Enabled = false;
            // Aspect = (float)Screen.ClientSize.Width / (float)Screen.ClientSize.Height;
            MovieArea = new Size(this.Width - this.ClientSize.Width, this.Height - this.ClientSize.Height);
            InitialClientWidth = this.ClientSize.Width;
            this.MinimumSize = new Size(this.Width, MovieArea.Height + Movie_Menu.Height + CommandPanel.Height + 1);
            CommandPanelHeight = CommandPanel.Height;
            
            FORM_Tooltip.SetToolTip(BTN_Play, "Play");
            FORM_Tooltip.SetToolTip(BTN_Pause, "Pause");
            FORM_Tooltip.SetToolTip(BTN_Stop, "Stop");
            FORM_Tooltip.SetToolTip(Movie_Track, "Search");


            Movie_Timer.Enabled = true;

            // Check if MediaFile matches a valid movie name, if YES then play it
            // CheckMovieName(MediaFile.ToLower());
        }

        /*
        protected override void WndProc(ref Message m)
        {
            string MediaFile = "";
            switch (m.Msg)
            {
                case Api.WM_DROPFILES: // Drag & drop
                    uint hDrop = (uint)m.WParam;
                    int FilesDropped = Api.DragQueryFile(hDrop, -1, null, 0);
                    if (FilesDropped != 0)
                    {
                        StringBuilder sFileName = new StringBuilder(Api.MAX_PATH);
                        //for (int i = 0; i < FilesDropped; i++)
                        //{
                        //    Api.DragQueryFile(hDrop, i, sFileName, Api.MAX_PATH);
                        //    MediaFile = sFileName.ToString().ToLower(); break;
                        //}
                        Api.DragQueryFile(hDrop, 0, sFileName, Api.MAX_PATH);
                        MediaFile = sFileName.ToString().ToLower();
                    }
                    Api.DragFinish(hDrop);
                    // Check if MediaFile matches a valid movie name, if YES then play it
                    CheckMovieName(MediaFile);
                    break;


            }
            base.WndProc(ref m);
        }
         */

        private void CheckMovieName(string MediaFile)
        {
            if (MediaFile.Length != 0)
            {
                Boolean DoIt = false;
                if (MediaFile.EndsWith(".avi")) DoIt = true;
                if (MediaFile.EndsWith(".wmv")) DoIt = true;
                if (MediaFile.EndsWith(".mpeg")) DoIt = true;
                if (MediaFile.EndsWith(".mpg")) DoIt = true;

                if (DoIt) PlayThisMovie(MediaFile);
            }
        }


        private void MAIN_Form_Resize(object sender, EventArgs e)
        {
            /*
            int height = this.ClientSize.Height - Movie_Menu.Height - CommandPanel.Height;
            int width = this.ClientSize.Width;
            float temp = (float)width / (float)height;
            if (temp >= Aspect)
            {
                width = Convert.ToInt32(height * Aspect);
                // Screen.Size = new Size(width, height);
                // Screen.Location = new Point((this.ClientSize.Width - width) / 2, Movie_Menu.Height);
            }
            else
            {
                height = Convert.ToInt32(width / Aspect);
                // Screen.Size = new Size(width, height);
                // Screen.Location = new Point(0, (this.ClientSize.Height - Movie_Menu.Height - CommandPanel.Height - height) / 2 + Movie_Menu.Height);
            }
             */ 
        }

        private void Movie_Timer_Tick(object sender, EventArgs e)
        {
            string HH, MM, SS;
            if (Movie_Timer_Enabled) // Update movie timer
            {
                // MovieCurrentPosition = (int)Movie.CurrentPosition;
                MovieCurrentPosition = (int)movie_screen.CurrentPosition; // in seconds
                Movie_Track.Value = 10 * MovieCurrentPosition; // This will update the tracker position
                hours = MovieCurrentPosition / 3600;
                minutes = (MovieCurrentPosition - hours * 3600) / 60;
                seconds = (MovieCurrentPosition - hours * 3600 - minutes * 60);

                HH = ("00" + hours.ToString());
                HH = HH.Substring(HH.Length - 2, 2);
                MM = ("00" + minutes.ToString());
                MM = MM.Substring(MM.Length - 2, 2);
                SS = ("00" + seconds.ToString());
                SS = SS.Substring(SS.Length - 2, 2);
                currentTime.Text = HH + ":" + MM + ":" + SS;

                if (currentTime.Text == movieDuration) BTN_Stop_Click(null, null);
            }

            DateTime dt = DateTime.Now;
            HH = ("00" + dt.Hour.ToString());
            HH = HH.Substring(HH.Length - 2, 2);
            MM = ("00" + dt.Minute.ToString());
            MM = MM.Substring(MM.Length - 2, 2);
            SS = ("00" + dt.Second.ToString());
            SS = SS.Substring(SS.Length - 2, 2);
            ShowTime.Text = HH + ":" + MM + ":" + SS;

        }

        private void Movie_Track_Scroll(object sender, EventArgs e)
        {
            if (ScrollEnable == true) {
                double position = (double)(Movie_Track.Value / 10.0);
                movie_screen.SetPosition(position);
                // Movie.CurrentPosition = (double)(Movie_Track.Value / 10.0);
            }
        }

        private void Movie_Track_MouseDown(object sender, MouseEventArgs e)
        {
            if (movie_screen.State != PlayState.Close)
            {
                // Movie.Pause();
                movie_screen.Pause();
                ScrollEnable = true;
                Movie_Track.Value = (int)((float)((float)e.X / (float)Movie_Track.Width) * (float)Movie_Track.Maximum);
                Movie_Track_Scroll(null, null);
            }
        }

        private void Movie_Track_MouseUp(object sender, MouseEventArgs e)
        {
            if (movie_screen.State != PlayState.Close)
            {
                ScrollEnable = false;
                if (BTN_Play.Visible == false)
                {
                    // Movie.Play();
                    movie_screen.Play();
                    //Movie_Timer.Enabled = true;
                    Movie_Timer_Enabled = true;
                }
            }
        }

        private void Movie_Track_Scroll(object sender, ScrollEventArgs e)
        {
            //if (HDmovie) return; // To avoid the DirectX infamous exception error
            if (ScrollEnable == true)
            {
                // Movie.CurrentPosition = (double)(Movie_Track.Value / 10.0);
                double position = (double)(Movie_Track.Value / 10.0); 
                movie_screen.SetPosition(position);
            }
        }

        private void Movie_Track_ValueChanged(object sender, EventArgs e)
        {
            //if (HDmovie) return; // To avoid the DirectX infamous exception error
            if (ScrollEnable == true)
            {
                double position = (double)(Movie_Track.Value / 10.0);
                movie_screen.SetPosition(position);
            }
        }

        private void BTN_Pause_Click(object sender, EventArgs e)
        {
            if (movie_screen.State != PlayState.Close)
            {
                BTN_Pause.Enabled = false;
                BTN_Play.Enabled = BTN_Stop.Enabled = true;
                //Movie_Timer.Enabled = false;
                Movie_Timer_Enabled = false;
                // Movie.Pause();
                movie_screen.Pause();
                BTN_Pause.Visible = false;
                BTN_Play.Visible = true;
            }
        }

        private void BTN_Play_Click(object sender, EventArgs e)
        {
            if (movie_screen.State != PlayState.Close)
            {
                BTN_Play.Enabled = false;
                BTN_Stop.Enabled = BTN_Pause.Enabled = true;
                // Movie.Play();
                movie_screen.Play();
                //Movie_Timer.Enabled = true;
                Movie_Timer_Enabled = true;

                BTN_Play.Visible = false;
                BTN_Pause.Visible = true;
            }
        }

        private void PlayThisMovie(string filename)
        {
            int LastIndex = filename.LastIndexOf(@"\");
            String movie_title = filename.Substring(LastIndex + 1, (filename.Length - LastIndex - 1));

            // Movie.Init(filename);

            // Set control default state
            Movie_Timer_Enabled = false;
            Movie_Track.Enabled = false;
            Movie_Track.Value = 0;
            BTN_Play.Enabled = BTN_Pause.Enabled = BTN_Stop.Enabled = false;

            if (!movie_screen.OpenFile(filename))
            {
                MessageBox.Show("Unable to play " + movie_title);
                return;
            }

            //Size MovieMinimumIdealSize = Movie.MinimumIdealSize;

            MovieDefaultSize = new Size(movie_screen.FrameWidth, movie_screen.FrameHeight);

            int UseWidth = Math.Max(MovieDefaultSize.Width, InitialClientWidth);
            int UseHeight = MovieDefaultSize.Height;
            Aspect = (float)((float)MovieDefaultSize.Width / (float)MovieDefaultSize.Height);
            HDmovie = false;
            if (UseWidth >= 1200) // Detect HD movie
            {
                UseWidth = (int)(UseWidth * .5f);
                UseHeight = (int)(UseHeight * .5f);
                HDmovie = true;
            }

            this.ClientSize = new Size(UseWidth, UseHeight + Movie_Menu.Height + CommandPanel.Height);
            
            Movie_Track.Enabled = true;
            Movie_Track.Value = 0;
            // MovieDuration = (int)Movie.Duration;
            // int total_frame = (int)Movie.GetTotalFrames(), frame_rate = (int)Movie.GetFrameRate();
            MovieDuration = (int)(movie_screen.FrameCount / movie_screen.FrameRate); // Convert this to seconds
            Movie_Track.Maximum = 10 * MovieDuration;
            hours = MovieDuration / 3600;
            minutes = (MovieDuration - hours * 3600) / 60;
            seconds = (MovieDuration - hours * 3600 - minutes * 60);

            string HH = ("00" + hours.ToString());
            HH = HH.Substring(HH.Length - 2, 2);
            string MM = ("00" + minutes.ToString());
            MM = MM.Substring(MM.Length - 2, 2);
            string SS = ("00" + seconds.ToString());
            SS = SS.Substring(SS.Length - 2, 2);
            movieDuration = HH + ":" + MM + ":" + SS;
            durationTime.Text = "/  " + movieDuration;

            this.Text = movie_title.ToLower();

            BTN_Play.Visible = false;
            BTN_Pause.Visible = true;

            BTN_Pause.Enabled = BTN_Stop.Enabled = true;

            //if (HDmovie) 
                MAIN_Form_Resize(null, null);

            // Movie.Play();
            //Movie_Timer.Enabled = true;
            Movie_Timer_Enabled = true;
        }

        private string CompletePath(string UsePath)
        {
            return UsePath.TrimEnd('\\') + @"\";
        }

        private void openMovieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UsePath = "";
            if (UsePath.Length != 0) {
                DirectoryInfo dirinfo = new DirectoryInfo(UsePath);
                if (!dirinfo.Exists) UsePath = "";
            }
            if (UsePath.Length == 0)
                UsePath = Path.GetPathRoot(Directory.GetCurrentDirectory());

            Movie_OpenFile.InitialDirectory = UsePath;
            if (Movie_OpenFile.ShowDialog() == DialogResult.OK)
            {
                this.Refresh(); // Redraw MAIN_Form
                UsePath = Path.GetDirectoryName(Movie_OpenFile.FileName);
                PlayThisMovie(Movie_OpenFile.FileName);
            }
        }

        private void BTN_Stop_Click(object sender, EventArgs e)
        {
            BTN_Stop.Enabled = false;
            BTN_Play.Enabled = true;
            //Movie_Timer.Enabled = false;
            Movie_Timer_Enabled = false;
            Movie_Track.Value = 0;
            // Movie.Stop();
            movie_screen.Stop();
            BTN_Play.Visible = true;
            BTN_Pause.Visible = false;
        }


    }
}