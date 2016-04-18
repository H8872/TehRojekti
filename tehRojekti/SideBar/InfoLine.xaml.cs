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
    public sealed partial class InfoLine : UserControl
    {
        public string LeftContent { get; set; }
        public string MiddleContent { get; set; }
        public string RightContent { get; set; }

        public int LocationY { get; set; }

        public InfoLine()
        {
            this.InitializeComponent();

            LeftContent = "";
            MiddleContent = "";
            RightContent = "";
        }

        public void UpdateInfo()
        {
            Left.Text = LeftContent;
            Middle.Text = MiddleContent;
            Right.Text = RightContent;

            if(MiddleContent != "")
            {
                SetValue(Canvas.TopProperty, LocationY - 7);
            }
            else SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
