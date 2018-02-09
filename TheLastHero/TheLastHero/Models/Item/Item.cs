using System;

namespace TheLastHero.Models
{
    public class Item
    {

        public Item()
        {

        }

        public Item(int round)
        {
            //creature c = new creature();

        }


        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

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