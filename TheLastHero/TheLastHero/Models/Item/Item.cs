﻿using System;

namespace TheLastHero.Models
{

    // This is a public class for Item
    public class Item
    {
        // unique id
        public string Id { get; set; }

        // this is for testing only, will be removed in later version
        public string Text { get; set; }

        // this is for item description
        public string Description { get; set; }

        // There will be 4 types of items: "Weapon", "Armor", "Ring",
        // "Consumable"
        public string Type { get; set; }

        // Lvl means level, and it indicates the item level, the level will determine how powerful the item is,
        // there is a constructor that takes the level as parameter and calculate the Def, Atk, Spd, Agi 
        // and create the item regardingly. 
        public int Lvl { get; set; }

        //Def represents defense attribute, it can potentially enhance a creature’s defense attribute
        public int Def { get; set; }

        //Atk represents attack attribute, it can potentially enhance a creature’s attack attribute
        public int Atk { get; set; }

        //Spd represents speed attribute, it can potentially enhance a creature’s speed attribute
        public int Spd { get; set; }

        //Luk represents luck attribute, it can potentially enhance a creature’s critical hit chance and dodge chance
        public int Luk { get; set; }

        //SpecialAbility is still under development, we are planning to add some features like double attacks, or absorb part of the damage during battle.
        public string SpecialAbility { get; set; }

        // There will be of items: Head, LeftHand, RightHand, Body, Leg,
        // Foot, Left Finger, Right Finger
        // This limits where the items can and cannot be equipped.
        public string EquippableLocation { get; set; }

        // This parameter indicates which character is using this item.
        public string EquippedBy { get; set; }

        //default construct is left empty in case of the future-use 
        public Item()
        {
        }

        // creates items based on level
        public Item(int level)
        {
        }


    }
}
