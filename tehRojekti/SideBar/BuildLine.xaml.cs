using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace tehRojekti.SideBar
{
    public sealed partial class BuildLine : UserControl
    {
        public string LeftContent { get; set; }
        public string MiddleContent { get; set; }
        public string RightContent { get; set; }
        public string ButtonContent { get; set; }
        public bool RightVisible { get; set; }
        public bool ButtonVisible { get; set; }
        public bool ProgressBarVisible { get; set; }
        private double maxProgress = 100;
        private double progress;
        public double Progress
        {
            get
            {
                return progress;
            }
            set
            {
                if (value >= maxProgress)
                {
                    progress = maxProgress;
                }
                else
                {
                    progress = value;
                }
            }
        }
        public double ProgressSpeed { get; set; }
        public int buildOrder { get; set; }
        public int Building { get; set; }

        public int LocationY { get; set; }

        DispatcherTimer timer = new DispatcherTimer();

        public BuildLine()
        {
            this.InitializeComponent();

            LeftContent = "";
            MiddleContent = "";
            RightContent = "";
            ButtonContent = "";
            ProgressSpeed = 0;                              // Progress happens every half second, speed indicates how much progress is done in that time
            progressBar.Maximum = maxProgress;              // to reach max, now 100 so progress speed is like percents per half second c:
            (App.Current as App).BuildOrder++;
            buildOrder = (App.Current as App).BuildOrder;   // Build order == 1 for starting building and 2 for compliting
            (App.Current as App).BuildOrder++;              // And is determinated in inizializing build

            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            Progress += ProgressSpeed;
            ProgressUpdate();
        }

        public void UpdateInfo()
        {
            left.Text = LeftContent;
            middle.Text = MiddleContent;
            right.Text = RightContent;
            button.Content = ButtonContent;
            SetValue(Canvas.TopProperty, LocationY);

            if (MiddleContent != "")                                                //If theres stuff in middle, everything else is empty by default otherwise its hidden
            {
                progressBar.Visibility = Visibility.Collapsed;
                button.Visibility = Visibility.Collapsed;
                left.Visibility = Visibility.Collapsed;
                right.Visibility = Visibility.Collapsed;
            }
            else
            {
                middle.Visibility = Visibility.Collapsed;
            }

            if (RightVisible)
            {
                right.Visibility = Visibility.Visible;
            }
            else
            {
                right.Visibility = Visibility.Collapsed;
            }

            if (ButtonVisible)
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Collapsed;
            }

            if (ProgressBarVisible)
            {
                progressBar.Visibility = Visibility.Visible;
            }
            else
            {
                progressBar.Visibility = Visibility.Collapsed;
            }
        }

        public void ProgressUpdate()
        {
            progressBar.Value = Progress;
            if (Progress >= 100)                                    // When progress reaches 100 or over timer stops and tells to game page
            {
                timer.Stop();
                RightContent = "Completed";
                Progress = 0;
                progressBar.Value = 0;
                (App.Current as App).BuildState = buildOrder + 1;
                UpdateInfo();
                Debug.WriteLine(buildOrder.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            (App.Current as App).BuildState = buildOrder;
            Progress++;
            ButtonVisible = false;
            ProgressBarVisible = true;
            UpdateInfo();
            Debug.WriteLine(buildOrder.ToString());
        }
    }
}
