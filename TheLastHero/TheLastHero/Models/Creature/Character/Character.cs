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
        int nextLevelExp;
        int currentExp;

        bool isCapLevel;


        //Key - location (ex: arm, head, ring), Value - Item 
        Dictionary<String, Item> equippedItem;

        //constructor
        public Character()
        {
            //maybe instantiate equippedItem so that characters can have weapons 
        }

        public int NextLevelExp { get => nextLevelExp; set => nextLevelExp = value; }
        public int CurrentExp { get => currentExp; set => currentExp = value; }
        public bool IsCapLevel { get => isCapLevel; set => isCapLevel = value; }
    }
}
