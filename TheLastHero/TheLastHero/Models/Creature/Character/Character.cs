using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{

    //Character inherites Creature's attributes and methods from characterbase class
    public class Character : Creature
    {

        // contant set to the maximum level
        // character cannot level up after level 20. 20 will be the maximum
        // level for character in this game, but monster will grow stronger and
        // eventually kill all the heros.
        public const int CapLevel = 20;

        /* This integer type property is for calculating how much experience for
         * character to level up
         */
        public int nextLevelExp { get; set; }
        /* This integer type property is for calculating how much experience for
         * character to level up
         */
        public int currentExp { get; set; }
        /* 
        * In default, is CapLevel is set to false because character wouldn’t start at Level 20. When character’s level reaches the capLevel, 20, we will change isCapLevel to true so that the character can no longer reach higher level. 
         */
        public bool isCapLevel { get; set; }

        // using enum for location of body parts where character can equip item
        public enum locations { head, body, feet, leftHand, rightHand, leftFinger, rightFinger }

        //Key - location is from enum, Value - Item 
        // the reason we use dictionary is because, Dictionary structure provide a key and value pair, Dict is perfect for location and item relationship.
        public Dictionary<locations, Item> equippedItem;

        //constructor for character
        public Character()
        {
            //instantiate equippedItem so that characters can have weapons
            //equippedItem = new Dictionary<>(); 

        }

        //This method would be triggered when currentExp is either equal or larger than nextLevelExp so that the character’s level will be increased by one
        public void LevelUp()
        {
            //Level would only be increased if level is less than capLevel 
            if (Lvl < CapLevel)
            {

                // read levelup map
                // update attributes accordingly here
                // atk += levelMap.get(atk, level)
                Lvl += 1;
            }
        }
        //We override this function from Creature in Character is because Character has weapon which Monster does not have.
        //Check How much damage a character has taken from a monster’s attack.
        //This is a reusable block of code and we could use it in different classes.
        public int DealDamage(int d)
        {
            // if (lefthand != null)
            // int finalDmg = level + equipedItem.get(lefthand).getAtk() + atk
            // return finalDmg;
            return 0;
        }
        //This void type method will help character to resign items. If characters get new items and this method will replace the new item for character.
        //This is a reusable block of code and we could use it in different classes.
        public void EquipItem(Item item, locations location)
        {
            //equippedItem[location] = item; 
        }

    }
}

