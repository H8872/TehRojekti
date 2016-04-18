using System;
using System.Collections.Generic;
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

        public int LocationY { get; set; }

        DispatcherTimer timer = new DispatcherTimer();

        public BuildLine()
        {
            this.InitializeComponent();

            LeftContent = "";
            MiddleContent = "";
            RightContent = "";
            ButtonContent = "";
            RightVisible = true;
            ButtonVisible = false;
            ProgressBarVisible = false;
            ProgressSpeed = 0;
            SetValue(ProgressBar.MaximumProperty, 100);

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            Progress += ProgressSpeed;
            ProgressUpdate();
        }

        public void UpdateInfo()
        {
            Left.Text = LeftContent;
            Middle.Text = MiddleContent;
            Right.Text = RightContent;
            Button.Content = ButtonContent;

            if (MiddleContent != "")
            {
                SetValue(Canvas.TopProperty, LocationY - 7);
                ProgressBar.Visibility = Visibility.Collapsed;
                Button.Visibility = Visibility.Collapsed;
                Left.Visibility = Visibility.Collapsed;
                Right.Visibility = Visibility.Collapsed;
            }
            else
            {
                Middle.Visibility = Visibility.Collapsed;
                SetValue(Canvas.TopProperty, LocationY);
            }

            if (RightVisible)
            {
                Right.Visibility = Visibility.Visible;
            }
            else
            {
                Right.Visibility = Visibility.Collapsed;
            }

            if (ButtonVisible)
            {
                Button.Visibility = Visibility.Visible;
            }
            else
            {
                Button.Visibility = Visibility.Collapsed;
            }

            if (ProgressBarVisible)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }
            else
            {
                ProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        public void ProgressUpdate()
        {
            ProgressBar.Value = Progress;
            if (Progress >= 100)
            {
                timer.Stop();
                ProgressBar.Visibility = Visibility.Collapsed;
                RightContent = "Completed";
                Right.Visibility = Visibility.Visible;
                (App.Current as App).Number = 2;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            (App.Current as App).Number = 1;
            Button.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Visible;
        }
    }
}
