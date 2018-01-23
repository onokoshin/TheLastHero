using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    
    //playerCharacter inherites base attributes and methods from characterbase class
    class Character : creature
    {
        //contant set to the maximum level 
        public const int capLevel = 20;
        
        //CHARACTER ONLY
        public int nextLevelExp { get; set; }
        public int currentExp { get; set; }

        public bool isCapLevel { get; set; }


        //Key - location (ex: arm, head, ring), Value - Item 
        public Dictionary<String, Item> equippedItem;

        //constructor
        public Character()
        {
            //maybe instantiate equippedItem so that characters can have weapons 
        }
    }
}
