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
using Windows.Storage;
using System.Runtime.Serialization;

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
        List<ResourcesClass> listResources = new List<ResourcesClass>();

        private bool currentType; //True: Info / False: Build

        BuildingsBuiltClass BuildingsBuilt = new BuildingsBuiltClass();
        
        private int yardReqWood = 5;
        private int yardReqStone = 5;
        private int workReqWood = 15;
        private int workReqStone = 20;
        private int buildState;
        //private int buildingNow;
        //private int buildingOrder;

        InfoLine resourcesTitle = new InfoLine {};
        InfoLine wood = new InfoLine {};
        InfoLine stone = new InfoLine {};
        InfoLine food = new InfoLine {};
        InfoLine homeTitle = new InfoLine {};
        
        BuildLine buildYard;
        BuildLine buildWorkshop;

        public GameMap()
        {
            this.InitializeComponent();

            ReadSave();
            
            //Initializing resources
            //InitializeResources();
            //AddResources(allResources, homeResources);

            //Initializing sidebar stuff
            InitializeInfo();
            InitializeBuild();

            //First thing on sidebar is Home Info
            currentInfo = HomeInfo;
            currentBuild = HomeBuild;
            currentType = true;
            UpdateCanvas(currentType);

            //Start checking stuff
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000/ 60);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void ReadSave()
        {
            try
            {
                // find a file
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                Stream BuildingStream = await storageFolder.OpenStreamForReadAsync("BuildingsBuilt.dat");
                Stream ResourceStream = await storageFolder.OpenStreamForReadAsync("Resources.dat");

                // is it empty
                if (BuildingStream == null) BuildingsBuilt = new BuildingsBuiltClass();
                if (ResourceStream == null) InitializeResources();
                else
                {
                    // read data
                    DataContractSerializer ResourcesSerializer = new DataContractSerializer(typeof(List<ResourcesClass>));
                    listResources = (List<ResourcesClass>)ResourcesSerializer.ReadObject(ResourceStream);////////////////////////////////TÄSTÄ TULE NOLIA DDDDDDDDDDD::::::::
                    foreach (ResourcesClass ai in listResources)
                    {
                        switch (ai.Building)
                        {
                            case 0:
                                allResources = ai;
                                break;

                            case 1:
                                homeResources = ai;
                                break;

                            case 2:
                                yardResources = ai;
                                break;

                            case 3:
                                workResources = ai;
                                break;
                        }
                    }
                }

                // read data
                DataContractSerializer BuildingSerializer = new DataContractSerializer(typeof(BuildingsBuiltClass));
                BuildingsBuilt = (BuildingsBuiltClass)BuildingSerializer.ReadObject(BuildingStream);
                

            }
            catch (IOException ex)
            {
                Debug.WriteLine("IO Exeption happened (reading): " + ex.ToString());
                if(ex.HResult == -2147024894)
                {
                    InitializeResources();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Poop Following exception has happend (reading): " + ex.ToString());
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            /* Build states:
            1 = For Title
            2 = For Title
            3 = For Title
            4 = For Title
            5 = building yard
            6 = yard complete
            7 = building workshop
            8 = workshop complete
            */
            buildState = (App.Current as App).BuildState;
            //buildingOrder = (App.Current as App).BuildOrder;

            if (!BuildingsBuilt.yardBuilt)
            {

                if (allResources.Wood >= yardReqWood && allResources.Stone >= yardReqStone && buildYard.Progress == 0)
                {

                    if (buildYard.Progress == 0)
                    {
                        buildYard.RightVisible = false;
                        buildYard.ButtonVisible = true;
                    }
                    else if (buildYard.Progress > 0)
                    {
                        buildYard.ProgressBarVisible = true;
                        buildYard.ButtonVisible = false;
                    }
                }
                else if (allResources.Stone < yardReqStone && buildYard.Progress == 0 || allResources.Wood < yardReqWood && buildYard.Progress == 0)
                {
                    buildYard.RightVisible = true;
                    buildYard.ButtonVisible = false;
                }
            }
            else
            {
                buildYard.RightContent = "Completed";
                buildYard.RightVisible = true;
                if (yardGrid.Visibility == Visibility.Collapsed)
                {
                    AddResources(allResources, yardResources);
                    yardGrid.Visibility = Visibility.Visible;
                }
            }
            if (buildState == 5)
            {
                allResources.Wood -= yardReqWood;
                allResources.Stone -= yardReqStone;
                InitializeInfo();
            }
            else if (buildState == 6)
            {
                buildYard.ButtonVisible = false;
                buildYard.ProgressBarVisible = false;
                BuildingsBuilt.yardBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
            }


            if (!BuildingsBuilt.workshopBuilt)
            {
                if (allResources.Wood >= workReqWood && allResources.Stone >= workReqStone)
                {
                    if (buildWorkshop.Progress == 0)
                    {
                        buildWorkshop.RightVisible = false;
                        buildWorkshop.ButtonVisible = true;
                    }
                    else if (buildWorkshop.Progress > 0)
                    {
                        buildWorkshop.ProgressBarVisible = true;
                        buildWorkshop.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < workReqWood && buildWorkshop.Progress == 0 || allResources.Stone < workReqStone && buildWorkshop.Progress == 0)
                {
                    buildWorkshop.ButtonVisible = false;
                    buildWorkshop.RightVisible = true;
                }
            }
            else
            {
                buildWorkshop.RightContent = "Completed";
                buildWorkshop.RightVisible = true;
                buildWorkshop.ProgressBarVisible = false;
                if (workGrid.Visibility == Visibility.Collapsed)
                {
                    AddResources(allResources, workResources);
                    workGrid.Visibility = Visibility.Visible;
                }
            }
            if (buildState == 7)
            {
                allResources.Wood = allResources.Wood - workReqWood;
                allResources.Stone = allResources.Stone - workReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
            }
            else if (buildState == 8)
            {
                buildWorkshop.ButtonVisible = false;
                buildWorkshop.ProgressBarVisible = false;
                BuildingsBuilt.workshopBuilt = true;
                UpdateCanvas(currentType);
            }

            InitializeInfo();
            (App.Current as App).BuildState = 0;
        }

        ResourcesClass AddResources(ResourcesClass a, ResourcesClass b)
        {
            a.maxWood = a.maxWood + b.maxWood;
            a.maxStone += b.maxStone;
            a.maxFood += b.maxFood;
            a.Wood = a.Wood + b.Wood;
            a.Stone += b.Stone;
            a.Food += b.Food;
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
            YardInfo.Clear();
            InfoLine homeTitle = new InfoLine { MiddleContent = "Home" };
            InfoLine yardTitle = new InfoLine { MiddleContent = "Yard" };
            HomeInfo.Add(homeTitle);
            YardInfo.Add(yardTitle);
            foreach (InfoLine line in resourceList)
            {
                HomeInfo.Add(line);
                YardInfo.Add(line);
            }
            InfoLine availableTitle = new InfoLine { MiddleContent = "Available" };
            InfoLine yardAvailable = new InfoLine { LeftContent = "Yard", RightContent = "Available" };
            HomeInfo.Add(availableTitle);
            HomeInfo.Add(yardAvailable);

            listResources.Clear();
            listResources.Add(allResources);
            listResources.Add(homeResources);
            listResources.Add(yardResources);
            listResources.Add(workResources);
        }

        private void InitializeBuild()
        {
            BuildLine homeTitle = new BuildLine { MiddleContent = "Home" };
            BuildLine yardTitle = new BuildLine { MiddleContent = "Yard" };

            buildYard = new BuildLine { LeftContent = "Yard", ButtonContent = "[" + yardReqWood + "w , "+ yardReqStone + "s]" , RightContent = "[" + yardReqWood + "w , " + yardReqStone + "s]", ProgressSpeed = 10};
            buildWorkshop = new BuildLine { LeftContent = "Workshop", ButtonContent = "[" + workReqWood + "w , " + workReqStone + "s]", RightContent = "[" + workReqWood + "w , " + workReqStone + "s]", ProgressSpeed = 10 };

            HomeBuild.Add(homeTitle);
            HomeBuild.Add(buildYard);
            YardBuild.Add(yardTitle);
            YardBuild.Add(buildWorkshop);
        }

        private void InitializeResources()
        {
            allResources.Building = 0;
            allResources.maxWood = 5;
            allResources.maxFood = 5;
            allResources.maxStone = 5;
            allResources.Wood = 5;
            allResources.Stone = 5;
            allResources.Food = 5;

            homeResources.Building = 1;
            homeResources.maxWood = 15;
            homeResources.maxStone = 10;
            homeResources.maxFood = 25;
            homeResources.Wood = 5;
            homeResources.Stone = 5;
            homeResources.Food = 15;

            yardResources.Building = 2;
            yardResources.maxWood = 45;
            yardResources.maxStone = 45;
            yardResources.maxFood = 15;
            yardResources.Wood = 20;
            yardResources.Stone = 20;
            yardResources.Food = 5;

            workResources.Building = 3;
            workResources.maxWood = 50;
            workResources.maxStone = 45;
            workResources.maxFood = 15;
            workResources.Wood = 15;
            workResources.Stone = 15;
            workResources.Food = 5;

            AddResources(allResources, homeResources);
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
                    if (line.MiddleContent == "")
                    {
                        line.LocationY = 30 * linePos;
                    }
                    else
                    {
                        line.LocationY = 28 * linePos;
                    }
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    linePos++;
                }
            }
            else
            {
                foreach (BuildLine line in currentBuild)
                {
                    if (line.MiddleContent == "")
                    {
                        line.LocationY = 30 * linePos;
                    }
                    else
                    {
                        line.LocationY = 28 * linePos;
                        line.ProgressBarVisible = false;
                        line.RightVisible = true;
                    }
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    linePos++;
                }
            }
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
                currentBuild = YardBuild;
            }

            UpdateCanvas(currentType);
        }

        private void workGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != WorkshopInfo)
            {
                currentInfo = WorkshopInfo;
                currentBuild = WorkshopBuild;
            }

            UpdateCanvas(currentType);
        }

        private async void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InitializeInfo();

                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile saveBuildings = await storageFolder.CreateFileAsync("BuildingsBuilt.dat", CreationCollisionOption.OpenIfExists);
                StorageFile saveResources = await storageFolder.CreateFileAsync("Resources.dat", CreationCollisionOption.OpenIfExists);

                Stream BuildingStream = await saveBuildings.OpenStreamForWriteAsync();
                DataContractSerializer BuildingSerializer = new DataContractSerializer(typeof(BuildingsBuiltClass));
                BuildingSerializer.WriteObject(BuildingStream, BuildingsBuilt);
                await BuildingStream.FlushAsync();
                BuildingStream.Dispose();

                Stream ResourceStream = await saveResources.OpenStreamForWriteAsync();
                DataContractSerializer ResourceSerializer = new DataContractSerializer(typeof(List<ResourcesClass>));
                ResourceSerializer.WriteObject(ResourceStream, listResources);////////////////////////////////////////////////////////////////PALAUTTAA NOLIA DDD:
                await ResourceStream.FlushAsync();
                ResourceStream.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured: " + ex.Message);
            }
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack) rootFrame.GoBack();
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
