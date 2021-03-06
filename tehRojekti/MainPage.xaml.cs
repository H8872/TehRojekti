﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml.Serialization;
using tehRojekti.SideBar;
using Windows.Storage;
using System.Runtime.Serialization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace tehRojekti
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            
            // This is for ResourcesClasses, so has to be nulled here
            (App.Current as App).BuildOrder = 0;
            // these are attributes that get info from BuildLines to GamePage and where needed so they are nulled here
            (App.Current as App).BuildState = 0;

            ApplicationView.PreferredLaunchViewSize = new Size(1000, 800);
            ApplicationView.PreferredLaunchWindowingMode
                = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // add and navigate to a Game page
                this.Frame.Navigate(typeof(GameMap));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).Exit();
        }

        private async void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            // Functions as Delete save for now

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile saveFile = await storageFolder.CreateFileAsync("BuildingsBuilt.xml", CreationCollisionOption.ReplaceExisting);
            await saveFile.DeleteAsync(StorageDeleteOption.Default);
            saveFile = await storageFolder.CreateFileAsync("Resources.xml", CreationCollisionOption.ReplaceExisting);
            Debug.WriteLine(saveFile.Path);
            await saveFile.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}
