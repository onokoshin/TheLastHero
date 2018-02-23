﻿using System;
using SQLite;

namespace TheLastHero.Models
{

    // This is a public class for Item
    public class Item
    {
        // unique id
        [PrimaryKey]
        public string Id { get; set; }

        // There will be 4 types of items: "Weapon", "Armor", "Ring",
        // "Consumable"
        public string Type { get; set; }

        // The name of the item
        public string Name { get; set; }

        // HP bonus
        public int HP { get; set; }

        // MP bouns
        public int MP { get; set; }

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

        public string ImgSource { get; set; }

        //default construct is left empty in case of the future-use 
        public Item()
        {
        }

        // creates items based on level
        public Item(int level)
        {
        }

        public void Update(Item newData)
        {
            if (newData == null)
            {
                return;
            }

            // Update all the fields in the Data, except for the Id
            Name = newData.Name;
            Type = newData.Type;
            HP = newData.HP;
            MP = newData.MP;
            Lvl = newData.Lvl;
            Def = newData.Def;
            Atk = newData.Atk;
            Spd = newData.Spd;
            Luk = newData.Luk;
            SpecialAbility = newData.SpecialAbility;
            EquippableLocation = newData.EquippableLocation;
            EquippedBy = newData.EquippedBy;
            ImgSource = newData.ImgSource;

        }

    }
}
