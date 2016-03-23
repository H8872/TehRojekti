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
using tehRojekti.SideBar;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace tehRojekti
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameMap : Page
    {
        public GameMap()
        {
            this.InitializeComponent();
            
            //InfoBox for Town Hall
            string Title = "Town Hall", Left = "Villagers", Middle = "", Right = "2";
            
            InfoBox townHall = new InfoBox(Title, Left, Middle, Right);
            InfoBarCanvas.Children.Add(townHall);
            
            //InfoBox for Resources

        }


        //TempDump
        /*<TextBlock x:Name="sideBarTitle2" Text="Title2" HorizontalAlignment="Center" FontSize="32" Margin="0,10,0,20"/>
            <Grid Margin="50,0">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                <TextBlock x:Name="sideBarLeftContent2" Grid.Column="0" Text="Content" HorizontalAlignment="Left"/>
                <TextBlock x:Name="sideBarMidContent2" Grid.Column="1" Text="Content" HorizontalAlignment="Center"/>
                <TextBlock x:Name="sideBarRightContent2" Grid.Column="2" Text="Content" HorizontalAlignment="Right"/>
            </Grid>
        </StackPanel>
        <StackPanel Grid.Row="1" HorizontalAlignment="Center" Orientation="Horizontal" Margin="0,20">
            <Button x:Name="DemolishButton" Content="Demolish" Margin="10,2"/>
            <Button x:Name="UpgradeButton" Content="Upgrade" Margin="10,2"/>
        </StackPanel>*/

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack) rootFrame.GoBack();
        }
    }
}
