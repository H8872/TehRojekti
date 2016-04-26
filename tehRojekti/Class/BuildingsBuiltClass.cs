using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tehRojekti.Class
{
    public class BuildingsBuiltClass
    {
        public bool homeBuilt { get; set; }
        public bool yardBuilt { get; set; }
        public bool workshopBuilt { get; set; }
        public bool storageRoomBuilt { get; set; }
        public bool kitchenBuilt { get; set; }
        public bool plainsBuilt { get; set; }
        public bool mountainBuilt { get; set; }
        public bool forestBuilt { get; set; }
        public bool storageBuilt { get; set; }
        public bool siloBuilt { get; set; }
        public bool farmBuilt { get; set; }
        public bool mineBuilt { get; set; }
        public bool lumberBuilt { get; set; }
        public bool villageBuilt { get; set; }
        public bool tradeBuilt { get; set; }
        public bool tradeRouteBuilt { get; set; }

        public void BuildingBuiltClass()
        {
            homeBuilt = true;
            yardBuilt = false;
            storageRoomBuilt = false;
            kitchenBuilt = false;
            plainsBuilt = false;
            mountainBuilt = false;
            forestBuilt = false;
            storageBuilt = false;
            siloBuilt = false;
            farmBuilt = false;
            mineBuilt = false;
            lumberBuilt = false;
            villageBuilt = false;
            tradeBuilt = false;
            tradeRouteBuilt = false;
        }
    }
}
