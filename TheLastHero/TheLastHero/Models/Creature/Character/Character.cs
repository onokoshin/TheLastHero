using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using TheLastHero.GameEngines;

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
        public int NextLevelExp { get; set; }
        /* This integer type property is for calculating how much experience for
         * character to level up
         */
        public int CurrentExp { get; set; }
        /* 
        * In default, is CapLevel is set to false because character wouldn’t start at Level 20. When character’s level reaches the capLevel, 20, we will change isCapLevel to true so that the character can no longer reach higher level. 
         */
        public bool IsCapLevel { get; set; }

        // using enum for location of body parts where character can equip item
        public ItemLocationEnum Locations { get; set; }

        //Key - location is from enum, Value - Item 
        // the reason we use dictionary is because, Dictionary structure provide a key and value pair, Dict is perfect for location and item relationship.
        public Dictionary<ItemLocationEnum, Item> EquippedItem;

        //This is going to be used for scoring purposes 
        public string EquippedItemListString { get; set; }
        //public List<string> EquippedItemList { get; set; }
        //partySlotNum is used in character selection
        public int PartySlotNum { get; set; }

        //total damage is for scoring purpose 
        public int totalDamage { get; set; }

        //constructor for character
        //When default character is created, everything will be set to 1
        public Character()
        {
            //instantiate equippedItem so that characters can have weapons
            //equippedItem = new Dictionary<>(); 
            //EquippedItemList = new List<string>();
            SetDefaultValues();
            LiveStatus = true;

        }

        //this function calculate damage for critical miss
        //the worst case -- character's hp goes down to 1
        public void TakeDamageCriticalMiss(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            CurrentHP = CurrentHP - damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 1;
                // Death...
                // character will never die from criticalMiss
            }
        }
        //This void type method will help character to resign items. If characters get new items and this method will replace the new item for character.
        //This is a reusable block of code and we could use it in different classes.
        public void EquipItem(Item item, ItemLocationEnum location)
        {
            if (EquippedItem.ContainsKey(location))
                EquippedItem.Remove(location);
            EquippedItem.Add(location, item);
            //EquippedItemList.Add(item.Name);
            UpdateEquippedItemListString();

        }

        //updates EquippedItemList using this function
        private void UpdateEquippedItemListString()
        {
            /* EquippedItemListString = "";
             foreach (string s in EquippedItemList)
                 EquippedItemListString += s;
             */
        }

        //removes item from a character
        public List<Item> RemoveItem(List<Item> itemList, ItemLocationEnum location)
        {
            itemList.Add(EquippedItem.Where(e => e.Key == location).First().Value);
            UpdateEquippedItemListString();
            return itemList;
        }

        //simple update function to makesure character is updated
        public void Update(Character newData)
        {
            if (newData == null)
            {
                return;
            }


            //update all the fields in creature except for Id
            Name = newData.Name;
            Friendly = newData.Friendly;
            ImgSource = newData.ImgSource;
            MaxHP = newData.MaxHP;
            CurrentHP = newData.MaxHP;
            MaxMP = newData.MaxMP;
            CurrentMP = newData.CurrentMP;
            Lvl = newData.Lvl;
            Def = newData.Def;
            Atk = newData.Atk;
            Spd = newData.Spd;
            Luk = newData.Luk;
            MoveRange = newData.MoveRange;
            AtkRange = newData.AtkRange;
            LiveStatus = newData.LiveStatus;
            Type = newData.Type;
            NextLevelExp = newData.NextLevelExp;
            CurrentExp = newData.CurrentExp;
            IsCapLevel = newData.IsCapLevel;
            EquippedItem = newData.EquippedItem;


        }

        //set default values for character
        private void SetDefaultValues()
        {
            Id = Guid.NewGuid().ToString();
            Spd = 0;
            Def = 0;
            Atk = 0;
            CurrentHP = 0;
            MaxHP = 0;
            MaxMP = 0;
            Lvl = 0;
            Luk = 0;
            MoveRange = 1;
            AtkRange = 1;
            ImgSource = "EmptySlot2.png";
            Name = "Empty Slot";
            Type = "";
            Friendly = true;
            CurrentExp = 0;
            IsCapLevel = false;
            LiveStatus = true;
            NextLevelExp = 100;
            EquippedItem = new Dictionary<ItemLocationEnum, Item>();

        }

        #region Basics
        //Levels up chracter based on level table 
        private void LevelUp()
        {
            // Walk the Level Table descending order
            // Stop when experience is >= experience in the table
            for (var i = LevelTable.Instance.LevelDetailsList.Count - 1; i > 0; i--)
            {
                // Check the Level
                // If the Level is > Experience for the Index, increment the Level.
                if (LevelTable.Instance.LevelDetailsList[i].Experience <= CurrentExp)
                {
                    var NewLvl = LevelTable.Instance.LevelDetailsList[i].Level;

                    // When leveling up, the current health is adjusted up by an offset of the MaxHealth, rather than full restore
                    var OldCurrentHealth = CurrentHP;
                    var OldMaxHealth = MaxHP;

                    // Set new Health
                    // New health, is d10 of the new level.  So leveling up 1 level is 1 d10, leveling up 2 levels is 2 d10.
                    var NewHealthAddition = HelperEngine.RollDice(NewLvl - Lvl, 10);

                    // Increment the Max health
                    MaxHP += NewHealthAddition;

                    // Calculate new current health
                    // old max was 10, current health 8, new max is 15 so (15-(10-8)) = current health
                    CurrentHP = (MaxHP - (OldMaxHealth - OldCurrentHealth));

                    // Refresh the Attriburte String
                    // I WILL DEAL WITH THIS LATER. !!!!!!!!!!!!!
                    //AttributeString = AttributeBase.GetAttributeString(this.Attribute);

                    // Set the new level
                    Lvl = NewLvl;

                    // Done, exit
                    break;
                }
            }
        }

        // Add experience
        public void AddExperience(int newExperience)
        {
            // Don't allow going lower in experience
            if (newExperience < 0)
            {
                return;
            }

            // Increment the Experience
            CurrentExp += newExperience;

            // Then check for Level UP
            if (CurrentExp >= LevelTable.Instance.LevelDetailsList[Lvl].Experience && Lvl < CapLevel)
            {
                LevelUp();
            }
            if (Lvl > CapLevel)
                Lvl = CapLevel;
        }



        #endregion Basics

        #region GetAttributes
        // Get Attributes

        // Get Attack
        public int GetAttack()
        {
            // Base Attack
            var myReturn = Atk;

            // Attack Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Lvl].Attack;

            // Get Attack bonus from Items <-- uncomment this line of code when item is fully implemented

            myReturn += GetItemBonus("Atk");
           
            return myReturn;
        }

        // Get Speed
        public int GetSpeed(Item item)
        {
            // Base value
            var myReturn = Spd;

            // Get Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Lvl].Speed;

            // Get bonus from Items  <-- uncomment this line of code when item is fully implemented
            myReturn += GetItemBonus("Spd");

            return myReturn;
        }

        // Get Defense
        public int GetDefense(Item item)
        {
            // Base value
            var myReturn = Def;

            // Get Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Lvl].Defense;

            // Get bonus from Items
            myReturn += GetItemBonus("Def");

            return myReturn;
        }

        // Get Max Health
        public int GetHealthMax(Item item)
        {
            // Base value
            var myReturn = MaxHP;
            return myReturn;
        }


        // Get Current Health
        public int GetHealthCurrent(Item item)
        {
            // Base value
            var myReturn = CurrentHP;

            // Get bonus from Items
            myReturn += GetItemBonus("HP");

            return myReturn;
        }

        private int GetItemBonus(String attribute)
        {
            int total = 0;

            if (EquippedItem.Count() > 0)
            {
                if (attribute.Equals("Atk"))
                {
                    foreach (var p in EquippedItem)
                    {
                        total += p.Value.Atk;
                    }
                }
                else if (attribute.Equals("Spd"))
                {
                    foreach (var p in EquippedItem)
                    {
                        total += p.Value.Spd;
                    }
                }
                else if (attribute.Equals("Def"))
                {
                    foreach (var p in EquippedItem)
                    {
                        total += p.Value.Def;
                    }
                }
                else if (attribute.Equals("HP"))
                {
                    foreach (var p in EquippedItem)
                    {
                        total += p.Value.HP;
                    }
                }
            }
            return total;
        }

        // Returns the Dice for the item
        // Sword 10, is Sword Dice 10
        //public int GetDamageDice()
        //{
        //    var myReturn = 0;

        //    var myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
        //    if (myItem != null)
        //    {
        //        // Damage is base damage plus dice of the weapon.  So sword of Damage 10 is d10
        //        myReturn += myItem.Damage;
        //    }

        //    return myReturn;
        //}

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            var myReturn = GetLevelBasedDamage();

            //ITEM STUFF!!!!!!!!!!!!
            //var myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            //if (myItem != null)
            //{
            //    // Damage is base damage plus dice of the weapon.  So sword of Damage 10 is d10
            //    myReturn += HelperEngine.RollDice(1, myItem.Damage);
            //}

            return myReturn;
        }

        // Get Level based Damage
        // 1/4 of the Level of the Player is the base damage they do.
        public int GetLevelBasedDamage()
        {
            return (int)Math.Ceiling(Lvl * .25);
        }

        #endregion GetAttributes

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = string.Empty;
            myReturn += Name;
            myReturn += " , Level : " + Lvl.ToString();
            myReturn += " , Total Experience : " + CurrentExp;
            myReturn += " , Attack :  " + Atk;
            myReturn += " , Defense :  " + Def;
            myReturn += " , Speed :  " + Spd;
            myReturn += " , Type: " + Type;
            myReturn += " , MoveingRange: " + MoveRange;
            myReturn += " , AttackRange: " + AtkRange;
            //Item section comes here 
            myReturn += " , Items : " + EquippedItemListString;
            myReturn += ", Damage : " + totalDamage;

            return myReturn;
        }
    }
}
