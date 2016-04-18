using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tehRojekti.Class
{
    class ResourcesClass
    {
        public int maxFood { get; set; }
        private int food = 10;
        public int Food
        {
            get
            {
                return food;
            }
            set
            {
                if (food + value > maxFood)
                {
                    food = maxFood;
                }
                else food += value;
            }
        }

        public int maxWood { get; set; }
        private int wood = 10;
        public int Wood
        {
            get
            {
                return wood;
            }
            set
            {
                if (wood + value > maxWood)
                {
                    wood = maxWood;
                }
                else wood += value;
            }
        }

        public int maxStone { get; set; }
        private int stone = 10;
        public int Stone
        {
            get
            {
                return stone;
            }
            set
            {
                if (stone + value > maxStone)
                {
                    stone = maxStone;
                }
                else stone += value;
            }
        }
    }
}
