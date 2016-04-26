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

        Random rand = new Random();

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer CheckBuildStateTimer = new DispatcherTimer();

        private bool currentType; //True: Info / False: Build

        BuildingsBuiltClass BuildingsBuilt = new BuildingsBuiltClass();

        ObservableCollection<InfoLine> currentInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> HomeInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> YardInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> WorkshopInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> StorageInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> SiloInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> FarmInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> MineInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> LumberInfo = new ObservableCollection<InfoLine>();
        ObservableCollection<InfoLine> TradingInfo = new ObservableCollection<InfoLine>();

        ObservableCollection<InfoLine> resourceList = new ObservableCollection<InfoLine>();

        ObservableCollection<BuildLine> currentBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> HomeBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> YardBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> WorkshopBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> StorageBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> SiloBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> FarmBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> MineBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> LumberBuild = new ObservableCollection<BuildLine>();
        ObservableCollection<BuildLine> TradingBuild = new ObservableCollection<BuildLine>();

        ResourcesClass allResources = new ResourcesClass();         // 0
        ResourcesClass homeResources = new ResourcesClass();        // 1
        ResourcesClass yardResources = new ResourcesClass();        // 2
        ResourcesClass workResources = new ResourcesClass();        // 3
        ResourcesClass storageRoomResources = new ResourcesClass(); // 4
        ResourcesClass kitchenResources = new ResourcesClass();     // 5
        ResourcesClass storageResources = new ResourcesClass();     // 6
        ResourcesClass siloResources = new ResourcesClass();        // 7
        ResourcesClass farmResources = new ResourcesClass();        // 8
        ResourcesClass mineResources = new ResourcesClass();        // 9
        ResourcesClass lumberResources = new ResourcesClass();      // 10
        ResourcesClass tradeResources = new ResourcesClass();       // 11
        List<ResourcesClass> listResources;
        
        private int yardReqWood = 5;
        private int yardReqStone = 5;
        private int workReqWood = 1;
        private int workReqStone = 1;
        private int storageRoomReqWood = 1;
        private int storageRoomReqStone = 1;
        private int kitchenReqWood = 1;
        private int kitchenReqStone = 1;
        private int roadToPlainsReqStone = 1;
        private int roadToPlainsReqWood = 1;
        private int roadToMountainReqStone = 1;
        private int roadToMountainReqWood = 1;
        private int roadToForestReqStone = 1;
        private int roadToForestReqWood = 1;
        private int storageReqWood = 1;
        private int storageReqStone = 1;
        private int siloReqWood = 1;
        private int siloReqStone = 1;
        private int farmReqWood = 1;
        private int farmReqStone = 1;
        private int mineReqWood = 1;
        private int mineReqStone = 1;
        private int lumberReqWood = 1;
        private int lumberReqStone = 1;
        private int roadToVillageReqWood = 1;
        private int roadToVillageReqStone = 1;
        private int tradeReqWood = 1;
        private int tradeReqStone = 1;
        private int tradeRouteReqWood = 1;
        private int tradeRouteReqStone = 1;
        private int toolsReqWood = 1;
        private int toolsReqStone = 1;
        private int exploreReqFood = 1;
        private int buildState;
        //private int buildingNow;
        //private int buildingOrder;

        InfoLine resourcesTitle = new InfoLine {}; // <Donev
        InfoLine wood = new InfoLine {};
        InfoLine stone = new InfoLine {};
        InfoLine food = new InfoLine {};
        InfoLine homeTitle = new InfoLine {};  // <Done^
        
        BuildLine buildYard; // 1 2 doen
        BuildLine buildWorkshop; // 3 4 dieb
        BuildLine buildStorageRoom; // 5 6
        BuildLine buildKitchen;// 7 8
        BuildLine buildRoadToPlains; // 9 10
        BuildLine buildRoadToMountain; // 11 12
        BuildLine buildRoadToForest;// 13 14
        BuildLine buildStorageHouse; // 15 16
        BuildLine buildSilo; // 17 18
        BuildLine buildFarm; // 19 20
        BuildLine buildMine; // 21 22
        BuildLine buildLumberHouse; // 23 24
        BuildLine buildRoadToVillage; // 25 26
        BuildLine buildTradingHouse; // 27 28
        BuildLine buildTradeRoute; // 29 30
        BuildLine buildTools; // 31 32
        BuildLine buildExplore; // 33 34

        BuildLine homeBuildTitle;
        BuildLine yardTitle;
        BuildLine workshopTitle;
        BuildLine farmTitle;
        BuildLine mineTitle;
        BuildLine lumberTitle;

        BuildLine availableBuild;
        BuildLine blankBuild;
        BuildLine completedBuild;

        public GameMap()
        {
            this.InitializeComponent();

            //Reading save files, if no save files, initializes resources
            ReadSave();

            //Initializing sidebar stuff
            InitializeInfo();
            InitializeBuild();

            //First thing on sidebar is Home Info
            currentInfo = HomeInfo;
            currentBuild = HomeBuild;
            currentType = true;

            //Start checking stuff
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateCanvas(currentType);
            
            CheckBuildState();
        }

        private async void ReadSave() // Title tells
        {
            try
            {
                // find a file
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                Stream BuildingStream = await storageFolder.OpenStreamForReadAsync("BuildingsBuilt.xml");

                // is it empty
                if (BuildingStream == null) BuildingsBuilt = new BuildingsBuiltClass();

                // read data
                DataContractSerializer BuildingSerializer = new DataContractSerializer(typeof(BuildingsBuiltClass));
                BuildingsBuilt = (BuildingsBuiltClass)BuildingSerializer.ReadObject(BuildingStream);

                Stream ResourceStream = await storageFolder.OpenStreamForReadAsync("Resources.xml");
                // is it empty
                if (ResourceStream == null) InitializeResources();
                else
                {
                    // read data
                    DataContractSerializer ResourcesSerializer = new DataContractSerializer(typeof(List<ResourcesClass>));
                    listResources = (List<ResourcesClass>)ResourcesSerializer.ReadObject(ResourceStream);
                    foreach (ResourcesClass ai in listResources)
                    {
                        switch (ai.Building)            // relevant numbers told on top while defining ResourcesClasses
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
                                
                            case 4:
                                storageRoomResources = ai;
                                break;

                            case 5:
                                kitchenResources = ai;
                                break;

                            case 6:
                                storageResources = ai;
                                break;

                            case 7:
                                siloResources = ai;
                                break;

                            case 8:
                                farmResources = ai;
                                break;

                            case 9:
                                mineResources = ai;
                                break;

                            case 10:
                                lumberResources = ai;
                                break;

                            case 11:
                                tradeResources = ai;
                                break;

                            default:
                                Debug.WriteLine(ai.Building.ToString());
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Following exception has happend (reading): " + ex.ToString());
                InitializeResources();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            buildState = (App.Current as App).BuildState;   //Has build state changed?

            if (buildState != 0)                            //If it has, do stuff that lags if done constantly
            {
                CheckBuildState();
                UpdateCanvas(currentType);
            }

            UpdateBuild();
            InitializeInfo();
            //if info is on update canvas, if you update while build is on, buildLine button doesn't work properly
            if (currentType)
            {
                UpdateCanvas(currentType);
            }
        }

        ResourcesClass AddResources(ResourcesClass AddTo, ResourcesClass BeingAdded) // simple math thingy that adds ResourcesClasses together...
        {
            AddTo.maxWood += BeingAdded.maxWood;
            AddTo.maxStone += BeingAdded.maxStone;
            AddTo.maxFood += BeingAdded.maxFood;
            AddTo.Wood += BeingAdded.Wood;
            AddTo.Stone += BeingAdded.Stone;
            AddTo.Food += BeingAdded.Food;
            return AddTo;
        }

        private void CheckBuildState() // Checks build states, like is it done or started
        {
            /* Build states: (used below)
            1 = building yard                   17 = building silo                  33 = building actually just exploring
            2 = yard complete                   18 = silo complete                  34 = exploring complete
            3 = building workshop               19 = building farm                  35 = 
            4 = workshop complete               20 = farm complete                  36 = 
            5 = building storage                21 = building mine                  37 = 
            6 = storage complete                22 = mine complete                  38 = 
            7 = building kitchen                23 = building lumber house          39 =
            8 = kitchen complete                24 = lumber house complete          40 =
            9 = building road to plains         25 = building road to village       41 = building
            10 = road to plains complete        26 = road to village complete       42 =  complete
            11 = building road to mountain      27 = building Trading House             ^thats how stuff continues^
            12 = road to mountain complete      28 = trading house complete
            13 = building road to forest        29 = building trade route
            14 = road to forest complete        30 = trade route complete
            15 = building storage house         31 = building tools
            16 = storage house complete         32 = tools complete
            */

            if (buildState == 1)
            {
                allResources.Wood -= yardReqWood;
                allResources.Stone -= yardReqStone;
                InitializeInfo();
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 2)
            {
                buildYard.RightVisible = true;
                buildYard.ButtonVisible = false;
                buildYard.ProgressBarVisible = false;
                BuildingsBuilt.yardBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
                AddResources(allResources, yardResources);
                yardTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

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
                buildYard.ProgressBarVisible = false;
                buildYard.ButtonVisible = false;
                yardGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 3)
            {
                allResources.Wood -= workReqWood;
                allResources.Stone -= workReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
            }
            else if (buildState == 4)
            {
                buildWorkshop.RightVisible = true;
                buildWorkshop.ButtonVisible = false;
                buildWorkshop.ProgressBarVisible = false;
                BuildingsBuilt.workshopBuilt = true;
                UpdateCanvas(currentType);
                AddResources(allResources, workResources);
                workTextBlock.Text = "*";
                UpdateBuild();
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
                buildWorkshop.ButtonVisible = false;
                workGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 5)
            {
                allResources.Wood -= storageRoomReqWood;
                allResources.Stone -= storageRoomReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
            }
            else if (buildState == 6)
            {
                buildStorageRoom.RightVisible = true;
                buildStorageRoom.ButtonVisible = false;
                buildStorageRoom.ProgressBarVisible = false;
                BuildingsBuilt.storageRoomBuilt = true;
                UpdateCanvas(currentType);
                AddResources(allResources, storageRoomResources);
                UpdateBuild();
            }

            if (!BuildingsBuilt.storageRoomBuilt)
            {
                if (allResources.Wood >= storageRoomReqWood && allResources.Stone >= storageRoomReqStone)
                {
                    if (buildStorageRoom.Progress == 0)
                    {
                        buildStorageRoom.RightVisible = false;
                        buildStorageRoom.ButtonVisible = true;
                    }
                    else if (buildStorageRoom.Progress > 0)
                    {
                        buildStorageRoom.ProgressBarVisible = true;
                        buildStorageRoom.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < storageRoomReqWood && buildStorageRoom.Progress == 0 || allResources.Stone < storageRoomReqStone && buildStorageRoom.Progress == 0)
                {
                    buildStorageRoom.ButtonVisible = false;
                    buildStorageRoom.RightVisible = true;
                }
            }
            else
            {
                buildStorageRoom.RightContent = "Completed";
                buildStorageRoom.RightVisible = true;
                buildStorageRoom.ProgressBarVisible = false;
                buildStorageRoom.ButtonVisible = false;
            }

            if (buildState == 7)
            {
                allResources.Wood -= storageReqWood;
                allResources.Stone -= storageReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 8)
            {
                buildKitchen.RightVisible = true;
                buildKitchen.ButtonVisible = false;
                buildKitchen.ProgressBarVisible = false;
                BuildingsBuilt.kitchenBuilt = true;
                UpdateCanvas(currentType);
                AddResources(allResources, kitchenResources);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.kitchenBuilt)
            {
                if (allResources.Wood >= kitchenReqWood && allResources.Stone >= kitchenReqStone)
                {
                    if (buildKitchen.Progress == 0)
                    {
                        buildKitchen.RightVisible = false;
                        buildKitchen.ButtonVisible = true;
                    }
                    else if (buildKitchen.Progress > 0)
                    {
                        buildKitchen.ProgressBarVisible = true;
                        buildKitchen.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < kitchenReqWood && buildKitchen.Progress == 0 || allResources.Stone < kitchenReqStone && buildKitchen.Progress == 0)
                {
                    buildKitchen.ButtonVisible = false;
                    buildKitchen.RightVisible = true;
                }
            }
            else
            {
                buildKitchen.RightContent = "Completed";
                buildKitchen.RightVisible = true;
                buildKitchen.ProgressBarVisible = false;
                buildKitchen.ButtonVisible = false;
            }

            if (buildState == 9)
            {
                allResources.Wood -= roadToPlainsReqWood;
                allResources.Stone -= roadToPlainsReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 10)
            {
                buildRoadToPlains.RightVisible = true;
                buildRoadToPlains.ButtonVisible = false;
                buildRoadToPlains.ProgressBarVisible = false;
                BuildingsBuilt.plainsBuilt = true;
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.plainsBuilt)
            {
                if (allResources.Wood >= roadToPlainsReqWood && allResources.Stone >= roadToPlainsReqStone)
                {
                    if (buildRoadToPlains.Progress == 0)
                    {
                        buildRoadToPlains.RightVisible = false;
                        buildRoadToPlains.ButtonVisible = true;
                    }
                    else if (buildRoadToPlains.Progress > 0)
                    {
                        buildRoadToPlains.ProgressBarVisible = true;
                        buildRoadToPlains.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < roadToPlainsReqWood && buildRoadToPlains.Progress == 0 || allResources.Stone < roadToPlainsReqStone && buildRoadToPlains.Progress == 0)
                {
                    buildRoadToPlains.ButtonVisible = false;
                    buildRoadToPlains.RightVisible = true;
                }
            }
            else
            {
                buildRoadToPlains.RightContent = "Completed";
                buildRoadToPlains.RightVisible = true;
                buildRoadToPlains.ProgressBarVisible = false;
                buildRoadToPlains.ButtonVisible = false;
            }

            if (buildState == 11)
            {
                allResources.Wood -= roadToMountainReqWood;
                allResources.Stone -= roadToMountainReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 12)
            {
                buildRoadToMountain.RightVisible = true;
                buildRoadToMountain.ButtonVisible = false;
                buildRoadToMountain.ProgressBarVisible = false;
                BuildingsBuilt.mountainBuilt = true;
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.mountainBuilt)
            {
                if (allResources.Wood >= roadToMountainReqWood && allResources.Stone >= roadToMountainReqStone)
                {
                    if (buildRoadToMountain.Progress == 0)
                    {
                        buildRoadToMountain.RightVisible = false;
                        buildRoadToMountain.ButtonVisible = true;
                    }
                    else if (buildRoadToMountain.Progress > 0)
                    {
                        buildRoadToMountain.ProgressBarVisible = true;
                        buildRoadToMountain.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < roadToMountainReqWood && buildRoadToMountain.Progress == 0 || allResources.Stone < roadToMountainReqStone && buildRoadToMountain.Progress == 0)
                {
                    buildRoadToMountain.ButtonVisible = false;
                    buildRoadToMountain.RightVisible = true;
                }
            }
            else
            {
                buildRoadToMountain.RightContent = "Completed";
                buildRoadToMountain.RightVisible = true;
                buildRoadToMountain.ProgressBarVisible = false;
                buildRoadToMountain.ButtonVisible = false;
            }

            if (buildState == 13)
            {
                allResources.Wood -= roadToForestReqWood;
                allResources.Stone -= roadToForestReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 14)
            {
                buildRoadToForest.RightVisible = true;
                buildRoadToForest.ButtonVisible = false;
                buildRoadToForest.ProgressBarVisible = false;
                BuildingsBuilt.forestBuilt = true;
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.forestBuilt)
            {
                if (allResources.Wood >= roadToForestReqWood && allResources.Stone >= roadToForestReqStone)
                {
                    if (buildRoadToForest.Progress == 0)
                    {
                        buildRoadToForest.RightVisible = false;
                        buildRoadToForest.ButtonVisible = true;
                    }
                    else if (buildRoadToForest.Progress > 0)
                    {
                        buildRoadToForest.ProgressBarVisible = true;
                        buildRoadToForest.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < roadToForestReqWood && buildRoadToForest.Progress == 0 || allResources.Stone < roadToForestReqStone && buildRoadToForest.Progress == 0)
                {
                    buildRoadToForest.ButtonVisible = false;
                    buildRoadToForest.RightVisible = true;
                }
            }
            else
            {
                buildRoadToForest.RightContent = "Completed";
                buildRoadToForest.RightVisible = true;
                buildRoadToForest.ProgressBarVisible = false;
                buildRoadToForest.ButtonVisible = false;
            }

            if (buildState == 15)
            {
                allResources.Wood -= storageReqWood;
                allResources.Stone -= storageReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 16)
            {
                buildStorageHouse.RightVisible = true;
                buildStorageHouse.ButtonVisible = false;
                buildStorageHouse.ProgressBarVisible = false;
                BuildingsBuilt.storageBuilt = true;
                UpdateCanvas(currentType);
                AddResources(allResources, storageResources);
                storageTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.storageBuilt)
            {
                if (allResources.Wood >= storageReqWood && allResources.Stone >= storageReqStone)
                {
                    if (buildStorageHouse.Progress == 0)
                    {
                        buildStorageHouse.RightVisible = false;
                        buildStorageHouse.ButtonVisible = true;
                    }
                    else if (buildStorageHouse.Progress > 0)
                    {
                        buildStorageHouse.ProgressBarVisible = true;
                        buildStorageHouse.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < storageReqWood && buildStorageHouse.Progress == 0 || allResources.Stone < storageReqStone && buildStorageHouse.Progress == 0)
                {
                    buildStorageHouse.ButtonVisible = false;
                    buildStorageHouse.RightVisible = true;
                }
            }
            else
            {
                buildStorageHouse.RightContent = "Completed";
                buildStorageHouse.RightVisible = true;
                buildStorageHouse.ProgressBarVisible = false;
                buildStorageHouse.ButtonVisible = false;
                storageGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 17)
            {
                allResources.Wood -= siloReqWood;
                allResources.Stone -= siloReqStone;
                InitializeInfo();
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 18)
            {
                buildSilo.RightVisible = true;
                buildSilo.ButtonVisible = false;
                buildSilo.ProgressBarVisible = false;
                BuildingsBuilt.siloBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
                AddResources(allResources, siloResources);
                yardTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.siloBuilt)
            {
                if (allResources.Wood >= siloReqWood && allResources.Stone >= siloReqStone && buildSilo.Progress == 0)
                {
                    if (buildSilo.Progress == 0)
                    {
                        buildSilo.RightVisible = false;
                        buildSilo.ButtonVisible = true;
                    }
                    else if (buildSilo.Progress > 0)
                    {
                        buildSilo.ProgressBarVisible = true;
                        buildSilo.ButtonVisible = false;
                    }
                }
                else if (allResources.Stone < siloReqStone && buildSilo.Progress == 0 || allResources.Wood < siloReqWood && buildSilo.Progress == 0)
                {
                    buildSilo.RightVisible = true;
                    buildSilo.ButtonVisible = false;
                }
            }
            else
            {
                buildSilo.RightContent = "Completed";
                buildSilo.RightVisible = true;
                buildSilo.ProgressBarVisible = false;
                buildSilo.ButtonVisible = false;
                siloGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 19)
            {
                allResources.Wood -= farmReqWood;
                allResources.Stone -= farmReqStone;
                InitializeInfo();
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 20)
            {
                buildFarm.RightVisible = true;
                buildFarm.ButtonVisible = false;
                buildFarm.ProgressBarVisible = false;
                BuildingsBuilt.farmBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
                AddResources(allResources, farmResources);
                farmTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.farmBuilt)
            {
                if (allResources.Wood >= farmReqWood && allResources.Stone >= farmReqStone && buildFarm.Progress == 0)
                {
                    if (buildFarm.Progress == 0)
                    {
                        buildFarm.RightVisible = false;
                        buildFarm.ButtonVisible = true;
                    }
                    else if (buildFarm.Progress > 0)
                    {
                        buildFarm.ProgressBarVisible = true;
                        buildFarm.ButtonVisible = false;
                    }
                }
                else if (allResources.Stone < farmReqStone && buildFarm.Progress == 0 || allResources.Wood < farmReqWood && buildFarm.Progress == 0)
                {
                    buildFarm.RightVisible = true;
                    buildFarm.ButtonVisible = false;
                }
            }
            else
            {
                buildFarm.RightContent = "Completed";
                buildFarm.RightVisible = true;
                buildFarm.ProgressBarVisible = false;
                buildFarm.ButtonVisible = false;
                farmGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 21)
            {
                allResources.Wood -= mineReqWood;
                allResources.Stone -= mineReqStone;
                InitializeInfo();
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 22)
            {
                buildMine.RightVisible = true;
                buildMine.ButtonVisible = false;
                buildMine.ProgressBarVisible = false;
                BuildingsBuilt.mineBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
                AddResources(allResources, mineResources);
                mineTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.mineBuilt)
            {
                if (allResources.Wood >= mineReqWood && allResources.Stone >= mineReqStone && buildMine.Progress == 0)
                {
                    if (buildMine.Progress == 0)
                    {
                        buildMine.RightVisible = false;
                        buildMine.ButtonVisible = true;
                    }
                    else if (buildMine.Progress > 0)
                    {
                        buildMine.ProgressBarVisible = true;
                        buildMine.ButtonVisible = false;
                    }
                }
                else if (allResources.Stone < mineReqStone && buildMine.Progress == 0 || allResources.Wood < mineReqWood && buildMine.Progress == 0)
                {
                    buildMine.RightVisible = true;
                    buildMine.ButtonVisible = false;
                }
            }
            else
            {
                buildMine.RightContent = "Completed";
                buildMine.RightVisible = true;
                buildMine.ProgressBarVisible = false;
                buildMine.ButtonVisible = false;
                mineGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 23)
            {
                allResources.Wood -= lumberReqWood;
                allResources.Stone -= lumberReqStone;
                InitializeInfo();
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 24)
            {
                buildLumberHouse.RightVisible = true;
                buildLumberHouse.ButtonVisible = false;
                buildLumberHouse.ProgressBarVisible = false;
                BuildingsBuilt.lumberBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
                AddResources(allResources, lumberResources);
                lumberTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.lumberBuilt)
            {
                if (allResources.Wood >= lumberReqWood && allResources.Stone >= lumberReqStone && buildLumberHouse.Progress == 0)
                {
                    if (buildLumberHouse.Progress == 0)
                    {
                        buildLumberHouse.RightVisible = false;
                        buildLumberHouse.ButtonVisible = true;
                    }
                    else if (buildLumberHouse.Progress > 0)
                    {
                        buildLumberHouse.ProgressBarVisible = true;
                        buildLumberHouse.ButtonVisible = false;
                    }
                }
                else if (allResources.Stone < lumberReqStone && buildLumberHouse.Progress == 0 || allResources.Wood < lumberReqWood && buildLumberHouse.Progress == 0)
                {
                    buildLumberHouse.RightVisible = true;
                    buildLumberHouse.ButtonVisible = false;
                }
            }
            else
            {
                buildLumberHouse.RightContent = "Completed";
                buildLumberHouse.RightVisible = true;
                buildLumberHouse.ProgressBarVisible = false;
                buildLumberHouse.ButtonVisible = false;
                lumberGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 25)
            {
                allResources.Wood -= roadToVillageReqWood;
                allResources.Stone -= roadToVillageReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 26)
            {
                buildRoadToVillage.RightVisible = true;
                buildRoadToVillage.ButtonVisible = false;
                buildRoadToVillage.ProgressBarVisible = false;
                BuildingsBuilt.mountainBuilt = true;
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.villageBuilt)
            {
                if (allResources.Wood >= roadToVillageReqWood && allResources.Stone >= roadToVillageReqStone)
                {
                    if (buildRoadToVillage.Progress == 0)
                    {
                        buildRoadToVillage.RightVisible = false;
                        buildRoadToVillage.ButtonVisible = true;
                    }
                    else if (buildRoadToVillage.Progress > 0)
                    {
                        buildRoadToVillage.ProgressBarVisible = true;
                        buildRoadToVillage.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < roadToVillageReqWood && buildRoadToVillage.Progress == 0 || allResources.Stone < roadToVillageReqStone && buildRoadToVillage.Progress == 0)
                {
                    buildRoadToVillage.ButtonVisible = false;
                    buildRoadToVillage.RightVisible = true;
                }
            }
            else
            {
                buildRoadToVillage.RightContent = "Completed";
                buildRoadToVillage.RightVisible = true;
                buildRoadToVillage.ProgressBarVisible = false;
                buildRoadToVillage.ButtonVisible = false;
            }

            if (buildState == 27)
            {
                allResources.Wood -= tradeReqWood;
                allResources.Stone -= tradeReqStone;
                InitializeInfo();
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 28)
            {
                buildTradingHouse.RightVisible = true;
                buildTradingHouse.ButtonVisible = false;
                buildTradingHouse.ProgressBarVisible = false;
                BuildingsBuilt.tradeBuilt = true;
                InitializeInfo();
                UpdateCanvas(currentType);
                AddResources(allResources, tradeResources);
                tradingTextBlock.Text = "*";
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.tradeBuilt)
            {
                if (allResources.Wood >= tradeReqWood && allResources.Stone >= tradeReqStone && buildTradingHouse.Progress == 0)
                {
                    if (buildTradingHouse.Progress == 0)
                    {
                        buildTradingHouse.RightVisible = false;
                        buildTradingHouse.ButtonVisible = true;
                    }
                    else if (buildTradingHouse.Progress > 0)
                    {
                        buildTradingHouse.ProgressBarVisible = true;
                        buildTradingHouse.ButtonVisible = false;
                    }
                }
                else if (allResources.Stone < tradeReqStone && buildTradingHouse.Progress == 0 || allResources.Wood < tradeReqWood && buildTradingHouse.Progress == 0)
                {
                    buildTradingHouse.RightVisible = true;
                    buildTradingHouse.ButtonVisible = false;
                }
            }
            else
            {
                buildTradingHouse.RightContent = "Completed";
                buildTradingHouse.RightVisible = true;
                buildTradingHouse.ProgressBarVisible = false;
                buildTradingHouse.ButtonVisible = false;
                tradingGrid.Visibility = Visibility.Visible;
            }

            if (buildState == 29)
            {
                allResources.Wood -= tradeRouteReqWood;
                allResources.Stone -= tradeRouteReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 30)
            {
                buildTradeRoute.RightVisible = true;
                buildTradeRoute.ButtonVisible = false;
                buildTradeRoute.ProgressBarVisible = false;
                BuildingsBuilt.tradeRouteBuilt = true;
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (!BuildingsBuilt.tradeRouteBuilt)
            {
                if (allResources.Wood >= tradeRouteReqWood && allResources.Stone >= tradeRouteReqStone)
                {
                    if (buildTradeRoute.Progress == 0)
                    {
                        buildTradeRoute.RightVisible = false;
                        buildTradeRoute.ButtonVisible = true;
                    }
                    else if (buildTradeRoute.Progress > 0)
                    {
                        buildTradeRoute.ProgressBarVisible = true;
                        buildTradeRoute.ButtonVisible = false;
                    }
                }
                else if (allResources.Wood < tradeRouteReqWood && buildTradeRoute.Progress == 0 || allResources.Stone < tradeRouteReqStone && buildTradeRoute.Progress == 0)
                {
                    buildTradeRoute.ButtonVisible = false;
                    buildTradeRoute.RightVisible = true;
                }
            }
            else
            {
                buildTradeRoute.RightContent = "Completed";
                buildTradeRoute.RightVisible = true;
                buildTradeRoute.ProgressBarVisible = false;
                buildTradeRoute.ButtonVisible = false;
            }

            if (buildState == 31)
            {
                allResources.Wood -= toolsReqWood;
                allResources.Stone -= toolsReqStone;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 32)
            {
                buildTools.ProgressBarVisible = false;
                buildTools.RightContent = "[" + toolsReqWood + "w , " + toolsReqStone + "s]";
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (allResources.Wood >= toolsReqWood && allResources.Stone >= toolsReqStone)
            {
                if (buildTools.Progress == 0)
                {
                    buildTools.RightVisible = false;
                    buildTools.ButtonVisible = true;
                }
                else if (buildTools.Progress > 0)
                {
                    buildTools.ProgressBarVisible = true;
                    buildTools.ButtonVisible = false;
                }
            }
            else if (allResources.Wood < toolsReqWood && buildTools.Progress == 0 || allResources.Stone < toolsReqStone && buildTools.Progress == 0)
            {
                buildTools.ButtonVisible = false;
                buildTools.RightVisible = true;
            }

            if (buildState == 33)
            {
                allResources.Food -= exploreReqFood;
                InitializeInfo();
                UpdateCanvas(currentType);
                Debug.WriteLine(buildState.ToString());
            }
            else if (buildState == 34)
            {
                buildExplore.ProgressBarVisible = false;
                buildExplore.RightContent = "Complete!";
                if (!BuildingsBuilt.villageBuilt)
                {
                    BuildingsBuilt.villageBuilt = true;
                }
                else
                {
                    int exp = rand.Next(1, 10 + 1);                   //10% chance to find the boooook~~~ :o
                    switch (exp)
                    {
                        case 1:
                            BuildingsBuilt.bookFound = true;
                            Debug.WriteLine("Book Found!");
                            break;
                        default:
                            Debug.WriteLine("Nothing found!");
                            break;
                    }

                }
                UpdateCanvas(currentType);
                UpdateBuild();
                Debug.WriteLine(buildState.ToString());
            }

            if (allResources.Food >= exploreReqFood)
            {
                if (buildExplore.Progress == 0)
                {
                    buildExplore.RightVisible = false;
                    buildExplore.ButtonVisible = true;
                }
                else if (buildExplore.Progress > 0)
                {
                    buildExplore.ProgressBarVisible = true;
                    buildExplore.ButtonVisible = false;
                }
            }
            else if (allResources.Food < exploreReqFood && buildExplore.Progress == 0)
            {
                buildExplore.ButtonVisible = false;
                buildExplore.RightVisible = true;
            }


            if ((App.Current as App).BuildState != 0)
            {
                (App.Current as App).BuildState = 0;
                buildState = 0;
                CheckBuildState();
            }
            
            UpdateBuild();
        }

        private void InitializeInfo() // This really just updates them, because InfoLines don't have really any content
        {
            resourceList.Clear();

            HomeInfo.Clear();
            YardInfo.Clear();
            WorkshopInfo.Clear();
            StorageInfo.Clear();
            SiloInfo.Clear();
            FarmInfo.Clear();
            MineInfo.Clear();
            LumberInfo.Clear();
            TradingInfo.Clear();

            InfoLine homeInfoTitle = new InfoLine { MiddleContent = "Home" };
            InfoLine yardTitle = new InfoLine { MiddleContent = "Yard" };
            InfoLine workshopTitle = new InfoLine { MiddleContent = "Workshop" };
            InfoLine storageTitle = new InfoLine { MiddleContent = "Storage" };
            InfoLine siloTitle = new InfoLine { MiddleContent = "Silo" };
            InfoLine farmTitle = new InfoLine { MiddleContent = "Farm" };
            InfoLine mineTitle = new InfoLine { MiddleContent = "Mine" };
            InfoLine lumberTitle = new InfoLine { MiddleContent = "Lumber House" };
            InfoLine tradeTitle = new InfoLine { MiddleContent = "Trade" };

            //initializing InfoLines for resources
            resourcesTitle = new InfoLine { MiddleContent = "Resources" };
            food = new InfoLine { LeftContent = "Food", RightContent = allResources.Food.ToString() + "(" + allResources.maxFood.ToString() + ")" };
            stone = new InfoLine { LeftContent = "Stone", RightContent = allResources.Stone.ToString() + "(" + allResources.maxStone.ToString() + ")" };
            wood = new InfoLine { LeftContent = "Wood", RightContent = allResources.Wood.ToString() + "(" + allResources.maxWood.ToString() + ")" };
            resourceList.Add(resourcesTitle);
            resourceList.Add(food);
            resourceList.Add(wood);
            resourceList.Add(stone);

            //Adding resourcesList to HomeInfo
            HomeInfo.Add(homeInfoTitle);
            YardInfo.Add(yardTitle);
            WorkshopInfo.Add(workshopTitle);
            StorageInfo.Add(storageTitle);
            SiloInfo.Add(siloTitle);
            FarmInfo.Add(farmTitle);
            MineInfo.Add(mineTitle);
            LumberInfo.Add(lumberTitle);
            TradingInfo.Add(tradeTitle);

            foreach (InfoLine line in resourceList)
            {
                HomeInfo.Add(line);
                YardInfo.Add(line);
                SiloInfo.Add(line);
                StorageInfo.Add(line);
            }
            FarmInfo.Add(food);
            LumberInfo.Add(wood);
            MineInfo.Add(stone);

        }

        private void InitializeBuild() // Build Lines are initializes here...
        {
            buildYard = new BuildLine { LeftContent = "Yard", ButtonContent = "[" + yardReqWood + "w , " + yardReqStone + "s]", RightContent = "[" + yardReqWood + "w , " + yardReqStone + "s]", ProgressSpeed = 50 };
            buildWorkshop = new BuildLine { LeftContent = "Workshop", ButtonContent = "[" + workReqWood + "w , " + workReqStone + "s]", RightContent = "[" + workReqWood + "w , " + workReqStone + "s]", ProgressSpeed = 55 };
            buildStorageRoom = new BuildLine { LeftContent = "Storage Room", ButtonContent = "[" + storageRoomReqStone + "w , " + storageRoomReqStone + "s]", RightContent = "[" + storageRoomReqWood + "w , " + storageRoomReqStone + "s]", ProgressSpeed = 55 };
            buildKitchen = new BuildLine { LeftContent = "Kitchen", ButtonContent = "[" + kitchenReqWood + "w , " + kitchenReqStone + "s]", RightContent = "[" + kitchenReqWood + "w , " + kitchenReqStone + "s]", ProgressSpeed = 50 };
            buildRoadToPlains = new BuildLine { LeftContent = "Road: Plains", ButtonContent = "[" + roadToPlainsReqWood + "w , " + roadToPlainsReqStone + "s]", RightContent = "[" + roadToPlainsReqWood + "w , " + roadToPlainsReqStone + "s]", ProgressSpeed = 55 };
            buildRoadToMountain = new BuildLine { LeftContent = "Road: Mountain", ButtonContent = "[" + roadToMountainReqWood + "w , " + roadToMountainReqStone + "s]", RightContent = "[" + roadToMountainReqWood + "w , " + roadToMountainReqStone + "s]", ProgressSpeed = 55 };
            buildRoadToForest = new BuildLine { LeftContent = "Road: Forest", ButtonContent = "[" + roadToForestReqWood + "w , " + roadToForestReqStone + "s]", RightContent = "[" + roadToForestReqWood + "w , " + roadToForestReqStone + "s]", ProgressSpeed = 55 };
            buildStorageHouse = new BuildLine { LeftContent = "Storage House", ButtonContent = "[" + storageReqWood + "w , " + storageReqStone + "s]", RightContent = "[" + mineReqWood + "w , " + mineReqStone + "s]", ProgressSpeed = 50 };
            buildSilo = new BuildLine { LeftContent = "Silo", ButtonContent = "[" + siloReqWood + "w , " + siloReqStone + "s]", RightContent = "[" + siloReqWood + "w , " + siloReqStone + "s]", ProgressSpeed = 50 };
            buildFarm = new BuildLine { LeftContent = "Farm", ButtonContent = "[" + farmReqWood + "w , " + farmReqStone + "s]", RightContent = "[" + farmReqWood + "w , " + farmReqStone + "s]", ProgressSpeed = 55 };
            buildMine = new BuildLine { LeftContent = "Mine", ButtonContent = "[" + mineReqWood + "w , " + mineReqStone + "s]", RightContent = "[" + mineReqWood + "w , " + mineReqStone + "s]", ProgressSpeed = 55 };
            buildLumberHouse = new BuildLine { LeftContent = "Lumber House", ButtonContent = "[" + lumberReqWood + "w , " + lumberReqStone + "s]", RightContent = "[" + lumberReqWood + "w , " + lumberReqStone + "s]", ProgressSpeed = 55 };
            buildRoadToVillage = new BuildLine { LeftContent = "Road to Village", ButtonContent = "[" + roadToVillageReqWood + "w , " + roadToVillageReqStone + "s]", RightContent = "[" + roadToVillageReqWood + "w , " + roadToVillageReqStone + "s]", ProgressSpeed = 50 };
            buildTradingHouse = new BuildLine { LeftContent = "Trading House", ButtonContent = "[" + tradeReqWood + "w , " + tradeReqStone + "s]", RightContent = "[" + tradeReqWood + "w , " + tradeReqStone + "s]", ProgressSpeed = 50 };
            buildTradeRoute = new BuildLine { LeftContent = "Trade Route", ButtonContent = "[" + tradeRouteReqWood + "w , " + tradeRouteReqStone + "s]", RightContent = "[" + tradeRouteReqWood + "w , " + tradeRouteReqStone + "s]", ProgressSpeed = 50 };
            buildTools = new BuildLine { LeftContent = "Tools", ButtonContent = "[" + toolsReqWood + "w , " + toolsReqStone + "s]", RightContent = "[" + toolsReqWood + "w , " + toolsReqStone + "s]", ProgressSpeed = 50 };
            buildExplore = new BuildLine { LeftContent = "Explore World", ButtonContent = "Explore", RightContent = "Explore", ProgressSpeed = 50 };
            // Add buildlines with buttons here~ without buttons below, so its easier to code functions for them in CheckBuildState

            homeBuildTitle = new BuildLine { MiddleContent = "Home" };
            yardTitle = new BuildLine { MiddleContent = "Yard" };
            workshopTitle = new BuildLine { MiddleContent = "Workshop" };
            farmTitle = new BuildLine { MiddleContent = "Farm" };
            mineTitle = new BuildLine { MiddleContent = "Mine" };
            lumberTitle = new BuildLine { MiddleContent = "Lumber House" };

            availableBuild = new BuildLine { MiddleContent = "Available" };
            blankBuild = new BuildLine();
            completedBuild = new BuildLine { MiddleContent = "Completed" };

            UpdateBuild();
        }

        private void UpdateBuild()
        {
            HomeBuild.Clear();
            YardBuild.Clear();
            WorkshopBuild.Clear();


            HomeBuild.Add(homeBuildTitle);
            HomeBuild.Add(availableBuild);                                        // If building is available, add it to canvas
            if (!BuildingsBuilt.yardBuilt)
            {
                HomeBuild.Add(buildYard);
            }
            if (BuildingsBuilt.workshopBuilt && !BuildingsBuilt.kitchenBuilt)
            {
                HomeBuild.Add(buildKitchen);
            }
            if (BuildingsBuilt.workshopBuilt && !BuildingsBuilt.storageRoomBuilt)
            {
                HomeBuild.Add(buildStorageRoom);
            }
            HomeBuild.Add(blankBuild);
            HomeBuild.Add(completedBuild);                                          // If building is completed, add it to canvas     these apply all buildings below
            if (BuildingsBuilt.yardBuilt)
            {
                HomeBuild.Add(buildYard);
            }
            if (BuildingsBuilt.kitchenBuilt)
            {
                HomeBuild.Add(buildKitchen);
            }
            if (BuildingsBuilt.storageRoomBuilt)
            {
                HomeBuild.Add(buildStorageRoom);
            }

            YardBuild.Add(yardTitle);
            YardBuild.Add(buildExplore);
            YardBuild.Add(availableBuild);
            if (!BuildingsBuilt.workshopBuilt)
            {
                YardBuild.Add(buildWorkshop);
            }
            if (BuildingsBuilt.workshopBuilt)
            {
                if (!BuildingsBuilt.plainsBuilt)
                {
                    YardBuild.Add(buildRoadToPlains);
                }
                if (!BuildingsBuilt.mountainBuilt)
                {
                    YardBuild.Add(buildRoadToMountain);
                }
                if (!BuildingsBuilt.forestBuilt)
                {
                    YardBuild.Add(buildRoadToForest);
                }
                if (BuildingsBuilt.plainsBuilt && !BuildingsBuilt.farmBuilt)
                {
                    YardBuild.Add(buildFarm);
                }
                if (BuildingsBuilt.mountainBuilt && !BuildingsBuilt.mineBuilt)
                {
                    YardBuild.Add(buildMine);
                }
                if (BuildingsBuilt.forestBuilt && !BuildingsBuilt.lumberBuilt)
                {
                    YardBuild.Add(buildLumberHouse);
                }
                if (!BuildingsBuilt.siloBuilt)
                {
                    YardBuild.Add(buildSilo);
                }
                if (!BuildingsBuilt.storageBuilt)
                {
                    YardBuild.Add(buildStorageHouse);
                }
                if (BuildingsBuilt.villageBuilt && !BuildingsBuilt.tradeBuilt)
                {
                    YardBuild.Add(buildTradingHouse);
                }
                if (BuildingsBuilt.tradeBuilt && !BuildingsBuilt.tradeRouteBuilt)
                {
                    YardBuild.Add(buildTradeRoute);
                }
            }
            YardBuild.Add(blankBuild);
            YardBuild.Add(completedBuild);
            if (BuildingsBuilt.workshopBuilt)
            {
                YardBuild.Add(buildWorkshop);
            }
            if (BuildingsBuilt.plainsBuilt)
            {
                YardBuild.Add(buildRoadToPlains);
            }
            if (BuildingsBuilt.mountainBuilt)
            {
                YardBuild.Add(buildRoadToMountain);
            }
            if (BuildingsBuilt.forestBuilt)
            {
                YardBuild.Add(buildRoadToForest);
            }
            if (BuildingsBuilt.farmBuilt)
            {
                YardBuild.Add(buildFarm);
            }
            if (BuildingsBuilt.mineBuilt)
            {
                YardBuild.Add(buildMine);
            }
            if (BuildingsBuilt.lumberBuilt)
            {
                YardBuild.Add(buildLumberHouse);
            }
            if (BuildingsBuilt.siloBuilt)
            {
                YardBuild.Add(buildSilo);
            }
            if (BuildingsBuilt.storageBuilt)
            {
                YardBuild.Add(buildStorageHouse);
            }
            if (BuildingsBuilt.tradeBuilt)
            {
                YardBuild.Add(buildTradingHouse);
            }
            if (BuildingsBuilt.tradeRouteBuilt)
            {
                YardBuild.Add(buildTradeRoute);
            }

            WorkshopBuild.Add(workshopTitle);
            WorkshopBuild.Add(availableBuild);
            WorkshopBuild.Add(buildTools);
            WorkshopBuild.Add(blankBuild);
            WorkshopBuild.Add(completedBuild);
        }

        private void InitializeResources()          // Default resource stuffs goes here...
        {
            allResources.Building = 0;
            allResources.maxFood = 5;
            allResources.maxStone = 5;
            allResources.maxWood = 5;
            allResources.Wood = 5;
            allResources.Stone = 5;
            allResources.Food = 5;

            homeResources.Building = 1;
            homeResources.maxFood = 25;
            homeResources.maxStone = 10;
            homeResources.maxWood = 15;
            homeResources.Wood = 5;
            homeResources.Stone = 5;
            homeResources.Food = 15;

            yardResources.Building = 2;
            yardResources.maxFood = 15;
            yardResources.maxStone = 45;
            yardResources.maxWood = 45;
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

            storageRoomResources.Building = 4;
            storageRoomResources.maxFood = 5;
            storageRoomResources.maxStone = 20;
            storageRoomResources.maxWood = 20;
            storageRoomResources.Food = 0;
            storageRoomResources.Stone = 10;
            storageRoomResources.Wood = 10;

            kitchenResources.Building = 5;
            kitchenResources.maxFood = 20;
            kitchenResources.maxStone = 5;
            kitchenResources.maxWood = 5;
            kitchenResources.Food = 10;
            kitchenResources.Stone = 0;
            kitchenResources.Wood = 5;
            
            storageResources.Building = 6;
            storageResources.maxFood = 5;
            storageResources.maxStone = 75;
            storageResources.maxWood = 75;
            storageResources.Food = 0;
            storageResources.Stone = 20;
            storageResources.Wood = 20;

            siloResources.Building = 7;
            siloResources.maxFood = 75;
            siloResources.maxStone = 5;
            siloResources.maxWood = 5;
            siloResources.Food = 20;
            siloResources.Stone = 0;
            siloResources.Wood = 0;

            farmResources.Building = 8;
            farmResources.maxFood = 50;
            farmResources.maxStone = 5;
            farmResources.maxWood = 5;
            farmResources.Food = 20;
            farmResources.Stone = 0;
            farmResources.Wood = 0;

            mineResources.Building = 9;
            mineResources.maxFood = 5;
            mineResources.maxStone = 50;
            mineResources.maxWood = 5;
            mineResources.Food = 0;
            mineResources.Stone = 20;
            mineResources.Wood = 0;

            lumberResources.Building = 10;
            lumberResources.maxFood = 5;
            lumberResources.maxStone = 5;
            lumberResources.maxWood = 50;
            lumberResources.Food = 0;
            lumberResources.Stone = 0;
            lumberResources.Wood = 20;

            tradeResources.Building = 11;
            tradeResources.maxFood = 30;
            tradeResources.maxStone = 30;
            tradeResources.maxStone = 30;
            tradeResources.Food = 5;
            tradeResources.Stone = 5;
            tradeResources.Wood = 5;

            AddResources(allResources, homeResources);
        }
        
        public void UpdateCanvas(bool type)
        {
            SideCanvas.Children.Clear();
            SideCanvas.Height = 0;
            linePos = 0;
            //Adding current info to side canvas, and updating them
            if (type) // True = Info / False = Build
            {
                foreach (InfoLine line in currentInfo)
                {
                    if (line.MiddleContent == "")
                    {
                        line.LocationY = 30 * linePos;
                        linePos++;
                        SideCanvas.Height = linePos * 30;
                    }
                    else
                    {
                        line.LocationY = 28 * linePos;
                        linePos++;
                        SideCanvas.Height = linePos * 28;
                    }
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    if (linePos > 15)  // if over 15 items, show scroll bar
                    {
                        CanvasScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    }
                    else
                    {
                        CanvasScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    }
                }
            }
            else
            {
                foreach (BuildLine line in currentBuild)
                {
                    if (line.MiddleContent == "")
                    {
                        line.LocationY = 30 * linePos;
                        linePos++;
                        SideCanvas.Height = linePos * 30;
                    }
                    else
                    {
                        line.LocationY = 28 * linePos;
                        line.ProgressBarVisible = false;
                        line.RightVisible = true;
                        linePos++;
                        SideCanvas.Height = linePos * 28;
                    }
                    line.UpdateInfo();
                    SideCanvas.Children.Add(line);
                    if (linePos > 15)  // if over 15 items, show scroll bar
                    {
                        CanvasScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    }
                    else
                    {
                        CanvasScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    }
                }
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();  // Stops timer, saves stuffs, not building progresses and goes to menu

            WriteSave();

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack) rootFrame.GoBack();
        }

        public async void WriteSave()
        {
            try
            {
                listResources = new List<ResourcesClass>();
                listResources.Add(allResources);
                listResources.Add(homeResources);
                listResources.Add(yardResources);
                listResources.Add(workResources);

                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile saveFile = await storageFolder.CreateFileAsync("BuildingsBuilt.xml", CreationCollisionOption.ReplaceExisting);
                await saveFile.DeleteAsync(StorageDeleteOption.Default);
                saveFile = await storageFolder.CreateFileAsync("Resources.xml", CreationCollisionOption.ReplaceExisting);
                await saveFile.DeleteAsync(StorageDeleteOption.Default);
                StorageFile saveBuildings = await storageFolder.CreateFileAsync("BuildingsBuilt.xml", CreationCollisionOption.OpenIfExists);
                StorageFile saveResources = await storageFolder.CreateFileAsync("Resources.xml", CreationCollisionOption.OpenIfExists);

                Stream BuildingStream = await saveBuildings.OpenStreamForWriteAsync();
                DataContractSerializer BuildingSerializer = new DataContractSerializer(typeof(BuildingsBuiltClass));
                BuildingSerializer.WriteObject(BuildingStream, BuildingsBuilt);
                await BuildingStream.FlushAsync();
                BuildingStream.Dispose();

                Stream ResourceStream = await saveResources.OpenStreamForWriteAsync();
                DataContractSerializer ResourceSerializer = new DataContractSerializer(typeof(List<ResourcesClass>));
                ResourceSerializer.WriteObject(ResourceStream, listResources);
                await ResourceStream.FlushAsync();
                ResourceStream.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured: " + ex.Message);
            }
        }

        // These buttons change current type of infostuff into its own and updates side canvas
        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = false;
            CheckBuildState();
            UpdateCanvas(currentType);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = true;
            CheckBuildState();
            UpdateCanvas(currentType);
        }

        //Grids are all pretty much the same. when tapped, changes current info stuffs to its own and clears any possible notificators from grids textblock
        private void homeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(currentInfo != HomeInfo)
            {
                currentInfo = HomeInfo;
                currentBuild = HomeBuild;
            }

            homeTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void yardGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != YardInfo)
            {
                currentInfo = YardInfo;
                currentBuild = YardBuild;
            }

            yardTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void workGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != WorkshopInfo)
            {
                currentInfo = WorkshopInfo;
                currentBuild = WorkshopBuild;
            }

            workTextBlock.Text = "";

            UpdateCanvas(currentType);
        }
        
        private void farmGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(currentInfo != FarmInfo)
            {
                currentInfo = FarmInfo;
                currentBuild = FarmBuild;
            }

            farmTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void mineGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != MineInfo)
            {
                currentInfo = MineInfo;
                currentBuild = MineBuild;
            }

            mineTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void lumberGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != LumberInfo)
            {
                currentInfo = LumberInfo;
                currentBuild = LumberBuild;
            }

            lumberTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void storageGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (currentInfo != StorageInfo)
            {
                currentInfo = StorageInfo;
                currentBuild = StorageBuild;
            }

            storageTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void siloGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(currentInfo != SiloInfo)
            {
                currentInfo = SiloInfo;
                currentBuild = SiloBuild;
            }

            siloTextBlock.Text = "";

            UpdateCanvas(currentType);
        }

        private void tradingGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(currentInfo != TradingInfo)
            {
                currentInfo = TradingInfo;
                currentBuild = TradingBuild;
            }

            tradingTextBlock.Text = "";

            UpdateCanvas(currentType);
        }
    }
}
