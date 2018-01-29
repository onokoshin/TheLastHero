using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{

    //playerCharacter inherites base attributes and methods from characterbase class
    public class Character : creature
    {
        //contant set to the maximum level 
        public const int capLevel = 20;

        //CHARACTER ONLY
        public int nextLevelExp { get; set; }
        public int currentExp { get; set; }
        public bool isCapLevel { get; set; }

        // using enum for location 
        public enum locations { head, body, feet, leftHand, rightHand, leftFinger, rightFinger }

        //Key - location is from enum, Value - Item 
        public Dictionary<locations, Item> equippedItem;

        //constructor
        public Character()
        {
            //maybe instantiate equippedItem so that characters can have weapons
        }

        public void LevelUp()
        {

        }

        public int DealDamage(int d)
        {
            return 0;
        }

        public void EquipItem(Item item, locations location)
        {

        }

    }
}
