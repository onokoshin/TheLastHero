using System;

namespace TheLastHero.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public Item()
        {
        }

        // created items based on level
        public Item(int level)
        {
        }

        // There will be 4 types of items: "Weapon", "Armor", "Ring",
        // "Consumable"
        public string Type { get; set; }

        // There will be of items: Head, LeftHand, RightHand, Body, Leg,
        // Foot, Left Finger, Right Finger
        public string Location { get; set; }
        public int Lvl { get; set; }
        public int Def { get; set; }
        public int Atk { get; set; }
        public int Spd { get; set; }
        public int Agi { get; set; }
        public string SpecialAbility { get; set; }

    }
}