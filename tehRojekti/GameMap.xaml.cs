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
using tehRojekti.Class;
using System.Diagnostics;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace tehRojekti
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameMap : Page
    {
        int linePos;

        DispatcherTimer timer = new DispatcherTimer();

        ObservableCollection<InfoLine> currentInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> HomeInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> YardInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> WorkshopInfo = new ObservableCollection<InfoLine>();

        ObservableCollection<InfoLine> resourceList = new ObservableCollection<InfoLine>();

        ObservableCollection<BuildLine> currentBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> HomeBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> YardBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> WorkshopBuild = new ObservableCollection<BuildLine>();

        ResourcesClass allResources = new ResourcesClass();
        ResourcesClass homeResources = new ResourcesClass();
        ResourcesClass yardResources = new ResourcesClass();
        ResourcesClass workResources = new ResourcesClass();

        private bool currentType; //True: Info / False: Build

        private int buildingYard = 0;
        private int yardReqWood = 5;
        private int yardReqStone = 5;
        private int buildState;
        private int buildingNow;

        InfoLine resourcesTitle = new InfoLine {};
        InfoLine wood = new InfoLine {};
        InfoLine stone = new InfoLine {};
        InfoLine food = new InfoLine {};
        InfoLine homeTitle = new InfoLine {};
        
        BuildLine buildYard = new BuildLine { LeftContent = "Yard", ButtonContent = "[5w , 5s]", RightContent = "[5w , 5s]" };

        public GameMap()
        {
            this.InitializeComponent();

            //Initializing resources
            InitializeResources();

            //Initializing sidebar stuff
            (App.Current as App).BuildOrder = 0;
            InitializeInfo();
            InitializeBuild();

            //First thing on sidebar is Home Info
            currentInfo = HomeInfo;
            currentBuild = HomeBuild;
            currentType = true;
            UpdateCanvas(currentType);

            //Start checking stuff
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            buildState = (App.Current as App).BuildState;
            buildingNow = (App.Current as App).BuildingNow;

            if (allResources.Wood >= yardReqWood && allResources.Stone >= yardReqStone)
            {
                if(buildYard.Progress == 0)
                {
                    buildYard.RightVisible = false;
                    buildYard.ButtonVisible = true;
                }
                else
                {
                    buildYard.ProgressBarVisible = true;
                    buildYard.ButtonVisible = false;
                }
            }
            else if(allResources.Wood < yardReqWood && allResources.Stone < yardReqStone && buildYard.Progress == 0)
            {
                buildYard.RightVisible = true;
                buildYard.ButtonVisible = false;
            }
            if(buildState == 1 && buildingNow == 0)
            {
                allResources.Wood -= 5;
                allResources.Stone -= 5;
                buildingYard = 1;
            }
            else if(buildState == 2 && buildingYard == 1)
            {
                allResources = AddResources(allResources, yardResources);
                
                yardGrid.Visibility = Visibility.Visible;
            }

            (App.Current as App).BuildState = 0;
        }

        ResourcesClass AddResources(ResourcesClass a, ResourcesClass b)
        {
            a.maxFood += b.maxFood;
            a.Food += b.Food;
            a.maxWood += b.maxWood;
            a.Wood += b.Wood;
            a.maxStone += b.maxStone;
            a.Stone += b.Stone;
            return a;
        }

        private void InitializeInfo()
        {
            resourceList.Clear();
            //initializing InfoLines for resources
            resourcesTitle = new InfoLine { MiddleContent = "Resources" };
            wood = new InfoLine { LeftContent = "Wood", RightContent = allResources.Wood.ToString() };
            stone = new InfoLine { LeftContent = "Stone", RightContent = allResources.Stone.ToString() };
            food = new InfoLine { LeftContent = "Food", RightContent = allResources.Food.ToString() };

            resourceList.Add(resourcesTitle);
            resourceList.Add(food);
            resourceList.Add(wood);
            resourceList.Add(stone);

            //Adding resourcesList to HomeInfo
            HomeInfo.Clear();
            InfoLine homeTitle = new InfoLine { MiddleContent = "Home" };
            HomeInfo.Add(homeTitle);
            foreach (InfoLine line in resourceList)
            {
                HomeInfo.Add(line);
            }

        }

        private void InitializeBuild()
        {
            HomeBuild.Clear();

            buildYard = new BuildLine { LeftContent = "Yard", ButtonContent = "[5w , 5s]" , RightContent = "[5w , 5s]" , ProgressSpeed = 10};

            BuildLine homeTitle = new BuildLine { MiddleContent = "Home" };
            HomeBuild.Add(homeTitle);
            HomeBuild.Add(buildYard);
        }

        private void InitializeResources()
        {
            allResources.maxWood = 5;
            allResources.maxFood = 5;
            allResources.maxStone = 5;
            allResources.Wood = 5;
            allResources.Stone = 5;
            allResources.Food = 5;

            homeResources.maxWood = 10;
            homeResources.maxFood = 15;
            homeResources.maxStone = 10;
            homeResources.Wood = 5;
            homeResources.Stone = 0;
            homeResources.Food = 10;

            yardResources.maxWood = 5;
            yardResources.maxFood = 0;
            yardResources.maxStone = 5;
            yardResources.Wood = 15;
            yardResources.Stone = 15;
            yardResources.Food = 5;

            workResources.maxWood = 5;
            workResources.maxFood = 5;
            workResources.maxStone = 5;
            workResources.Wood = 5;
            workResources.Stone = 5;
            workResources.Food = 5;
        }
        
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack) rootFrame.GoBack();
        }

        private void homeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(currentInfo != HomeInfo)
            {
                currentInfo = HomeInfo;
                currentBuild = HomeBuild;
            }

            UpdateCanvas(currentType);
        }

        private void yardGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != YardInfo)
            {
                currentInfo = YardInfo;
                currentBuild = HomeBuild;
            }

            UpdateCanvas(currentType);
        }

        public void UpdateCanvas(bool type)
        {
            SideCanvas.Children.Clear();
            linePos = 0;
            //Adding current info to canvas, and updating them
            if (type) // True = Info / False = Build
            {
                foreach (InfoLine line in currentInfo)
                {
                    line.LocationY = 27 * linePos;
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    linePos++;
                }
            }
            else
            {
                foreach (BuildLine line in currentBuild)
                {
                    if(line.MiddleContent == "")
                    {
                        line.LocationY = 27 * linePos;
                    }
                    else
                    {
                        line.LocationY = 20 * linePos;
                    }
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    linePos++;
                }
            } 
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = false;
            InitializeBuild();
            UpdateCanvas(currentType);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = true;
            InitializeInfo();
            UpdateCanvas(currentType);
        }
    }
}
