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
        public int Food
        {
            get
            {
                return food;
            }
            set
            {
                if (value >= maxFood)
                {
                    food = maxFood;
                }
                else food = value;
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
                if (value >= maxWood)
                {
                    wood = maxWood;
                }
                else wood = value;
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
                if (value >= maxStone)
                {
                    stone = maxStone;
                }
                else stone = value;
            }
        }
    }
}
