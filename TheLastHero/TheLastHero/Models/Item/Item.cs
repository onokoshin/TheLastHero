using System;
using SQLite;
using TheLastHero.Controller;

namespace TheLastHero.Models
{

    // This is a public class for Item
    public class Item
    {
        // unique id
        [PrimaryKey]
        public string Id { get; set; }

        // Used for List Identification, not used for game interaction or exposed to users
        public string Guid { get; set; }

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
	public ItemLocationEnum Location { get; set; }

        // This parameter indicates which character is using this item.
        public string EquippedBy { get; set; }

        public string ImgSource { get; set; }

        // Description of the Item to show to the user, Example: Lets you Hop into the action
        public string Description { get; set; }

        // Range of the item, swords are 1, hats/rings are 0, bows are >1
        public int Range { get; set; }

        // The Value item modifies.  So a ring of Health +3, has a Value of 3
        public int Value { get; set; }

        // The Damage the Item can do if it is used as a weapon in the primary hand
        public int Damage { get; set; }

        // Enum of the different attributes that the item modifies, Items can only modify one item
        public AttributeEnum Attribute { get; set; }

        //default construct is left empty in case of the future-use 
        public Item()
        {
        }

        // creates items based on level
        public Item(int level)
        {
            CreateDefaultItem();
        }

        // Create a default item for the instantiation
        private void CreateDefaultItem()
        {
            Name = "Unknown";
            Description = "Unknown";
            ImgSource = ItemsController.DefaultImageURI;

            Range = 0;
            Value = 0;
            

            Location = ItemLocationEnum.Unknown;
            HP = 0;
            MP = 0;
            Lvl = 0; 
            Atk = 0;
            Def = 0;
            Spd = 0;
            Luk = 0;
            SpecialAbility = "";

            ImgSource = null;
        }

        public string FormatOutput()
        {
            var myReturn = Name + " , " +
                            Description + " for " +
                            Location.ToString() + " with " +
                            Attribute.ToString() +
                            "+" + Value + " , " +
                            "Damage : " + Damage + " , " +
                            "Range : " + Range;

            return myReturn.Trim();
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
		Location = newData.Location;
            EquippedBy = newData.EquippedBy;
            ImgSource = newData.ImgSource;

        }

    }
}
