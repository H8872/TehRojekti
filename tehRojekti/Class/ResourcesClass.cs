using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tehRojekti.Class
{
    public class ResourcesClass
    {
        public int Building { get; set; }
        public int maxFood { get; set; }
        private int food;
        private int i=0;
        public int Food
        {
            get
            {
                return food;
            }
            set
            {
                if (i < 3)  // Number of different resources with max amount~~  Saving/Loading foops up so this goes around that issue
                {
                    food = value;
                    i++;
                }
                else
                {
                    if (value >= maxFood)
                    {
                        food = maxFood;
                    }
                    else food = value;
                }
                
            }
        }

        public int maxWood { get; set; }
        private int wood;
        public int Wood
        {
            get
            {
                return wood;
            }
            set
            {
                if (i < 3)
                {
                    wood = value;
                    i++;
                }
                else
                {
                    if (value >= maxWood)
                    {
                        wood = maxWood;
                    }
                    else wood = value;
                }
            }
        }

        public int maxStone { get; set; }
        private int stone;
        public int Stone
        {
            get
            {
                return stone;
            }
            set
            {
                if (i < 3)
                {
                    stone = value;
                    i++;
                }
                else
                {
                    if (value >= maxStone)
                    {
                        stone = maxStone;
                    }
                    else stone = value;
                }
            }
        }
    }
}
