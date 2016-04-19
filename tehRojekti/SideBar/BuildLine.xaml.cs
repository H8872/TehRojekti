using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public double Progress { get; set; }
        public double ProgressSpeed { get; set; }
        public int BuildOrder { get; set; }
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
            //RightVisible = true;
            //ButtonVisible = false;
            //ProgressBarVisible = false;
            ProgressSpeed = 0;
            progressBar.Maximum = 100;
            (App.Current as App).BuildOrder++;
            BuildOrder = (App.Current as App).BuildOrder;
            (App.Current as App).BuildOrder++;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
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

            if (MiddleContent != "")
            {
                SetValue(Canvas.TopProperty, LocationY);
                progressBar.Visibility = Visibility.Collapsed;
                button.Visibility = Visibility.Collapsed;
                left.Visibility = Visibility.Collapsed;
                right.Visibility = Visibility.Collapsed;
            }
            else
            {
                middle.Visibility = Visibility.Collapsed;
                SetValue(Canvas.TopProperty, LocationY);
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
            if (Progress >= 100)
            {
                timer.Stop();
                ProgressBarVisible = false;
                RightContent = "Completed";
                right.Text = "Completed";
                RightVisible = true;
                Progress = 100;
                (App.Current as App).BuildState = BuildOrder + 1;
                UpdateInfo();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            (App.Current as App).BuildState = BuildOrder;
            ButtonVisible = false;
            ProgressBarVisible = true;
            UpdateInfo();
        }
    }
}
