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

        List<InfoLine> currentInfo = new List<InfoLine>();
        List<InfoLine> HomeInfo = new List<InfoLine>();
        List<InfoLine> YardInfo = new List<InfoLine>();
        List<InfoLine> WorkshopInfo = new List<InfoLine>();

        List<InfoLine> resourceList = new List<InfoLine>();

        List<BuildLine> currentBuild = new List<BuildLine>();
        List<BuildLine> HomeBuild = new List<BuildLine>();
        List<BuildLine> YardBuild = new List<BuildLine>();
        List<BuildLine> WorkshopBuild = new List<BuildLine>();

        ResourcesClass allResources = new ResourcesClass();
        ResourcesClass homeResources = new ResourcesClass();
        ResourcesClass yardResources = new ResourcesClass();
        ResourcesClass workResources = new ResourcesClass();

        private bool currentType; //True: Info / False: Build

        private int yardReqWood = 5;
        private int yardReqStone = 5;
        private int buildInfo;

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
            (App.Current as App).Number = buildInfo;

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
            if(buildInfo == 1)
            {
                allResources.Wood -= 5;
                allResources.Stone -= 5;
            }
            else if(buildInfo == 2)
            {
                allResources = AddResources(allResources, yardResources);
            }

            (App.Current as App).Number = 0;
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
            InfoLine homeTitle = new InfoLine { MiddleContent = "Home" };
            HomeInfo.Add(homeTitle);
            foreach (InfoLine line in resourceList)
            {
                HomeInfo.Add(line);
            }

        }

        private void InitializeBuild()
        {
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
            if (type)
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
                    line.LocationY = 27 * linePos;
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    linePos++;
                }
            } 
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = false;
            UpdateCanvas(currentType);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = true;
            UpdateCanvas(currentType);
        }
    }
}
