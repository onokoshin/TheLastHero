using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TheLastHero.Models
{
    public class Monster : Creature
    {

        // Damage the Monster can do.
        public int Damage { get; set; }

        // Remaining Experience Points to give
        public int ExperienceRemaining { get; set; }

        // Current experience gained, or to give
        public int ExperienceTotal { get; set; }

        //monster carry up to one item (not equipped item, it's a drop item)
        public string UniqueDropID { get; set; }

        public bool Drop;

        //default constructor
        public Monster()
        {
            //UniqueDrop = new Item();
            // Get the number of points at the next level, and set it for Experience Total...

            //for testing purposes, every monster is set to level 3 exp = 900
            ExperienceTotal = LevelTable.Instance.LevelDetailsList[2 + 1].Experience;
            ExperienceRemaining = ExperienceTotal;

        }

        //overloaded constructor 
        //monster attributes will be based on the round being passed from battle engine
        //Ex: round 1 monster - weak attributes, but round 10 monster - strong attributes
        //@param level - level will be passed in depending on which round it is 
        public Monster(int Round, bool IsBoss)
        {
            Drop = false;
            Random Rand = new Random();
            int RandNum = Rand.Next(1, 10);

            if (RandNum < 6 && !IsBoss)
            {
                Drop = true;
            }

            if (Drop)
            {
                Item item = new Item(Round);
                UniqueDropID = item.Id;
            }
            else
            {
                UniqueDropID = "";
            }
            //3, 6, 9...rounds are boss rounds 
            // This is not for initialize 3 times for UniqueDrop 
            //every 3 rounds will be boss rounds with stronger monsters

            //if (round % 3 == 0)
            //{
            //    //this indicates that it is a boss round
            //    //attributes will be enhanced

            //    Item UniqueDrop = new Item(round);

            //}
            //else
            //{
            //    Random rand = new Random();
            //    int randNum = rand.Next(1, 10);

            //    //50% of monsters will have items and the other 50% do not have
            //    // drop items 
            //    if (randNum < 6)
            //    {
            //        Item UniqueDrop = new Item(round);
            //    }
            //    else
            //    {
            //        bool None = true;
            //        Item UniqueDrop = new Item(round, None);
            //    }
            //}
        }

        //monster drops its item when it is killed
        //Items will return to item pool
        //dropItem is a basic function, it is nice to have it so we can reuse
        // it wherever needed. Like battle engine.
        public String DropItem()
        {
            return UniqueDropID;
        }

        public void Update(Monster newData)
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
            // Update all the fields in the Monster
            UniqueDropID = newData.UniqueDropID;
            Drop = newData.Drop;



        }

        // Upgrades a monster to a set level
        //public void ScaleLevel(int level)
        //{
        //    // Calculate Experience Remaining based on Lookup...
        //    Level = level;

        //    // Get the number of points at the next level, and set it for Experience Total...
        //    ExperienceTotal = LevelTable.Instance.LevelDetailsList[Level + 1].Experience;
        //    ExperienceRemaining = ExperienceTotal;

        //    Damage = GetLevelBasedDamage() + LevelTable.Instance.LevelDetailsList[Level].Attack;
        //    Attribute.Attack = LevelTable.Instance.LevelDetailsList[Level].Attack;
        //    Attribute.Defense = LevelTable.Instance.LevelDetailsList[Level].Defense;
        //    Attribute.Speed = LevelTable.Instance.LevelDetailsList[Level].Speed;
        //    Attribute.MaxHealth = 5 * Level;    // 1/2 of what Characters can get per level.. 
        //    Attribute.CurrentHealth = Attribute.MaxHealth;

        //    AttributeString = AttributeBase.GetAttributeString(Attribute);
        //}

        // Calculate How much experience to return
        // Formula is the % of Damage done up to 100%  times the current experience
        // Needs to be called before applying damage
        public int CalculateExperienceEarned(int damage)
        {
            if (damage < 1)
            {
                return 0;
            }

            int remainingHealth = Math.Max(CurrentHP - damage, 0); // Go to 0 is OK...
            double rawPercent = (double)remainingHealth / (double)CurrentHP;
            double deltaPercent = 1 - rawPercent;
            var pointsAllocate = (int)Math.Floor(ExperienceRemaining * deltaPercent);

            // Catch rounding of low values, and force to 1.
            if (pointsAllocate < 1)
            {
                pointsAllocate = 1;
            }

            // Take away the points from remaining experience
            ExperienceRemaining -= pointsAllocate;
            if (ExperienceRemaining < 0)
            {
                pointsAllocate = 0;
            }

            return pointsAllocate;
        }





        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            CurrentHP = CurrentHP - damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                // Death...
                LiveStatus = false;
            }
        }

        #region GetAttributes
        // Get Attributes

        // Get Attack
        public int GetAttack()
        {
            // Base Attack
            var myReturn = Atk;

            return myReturn;
        }

        // Get Speed
        public int GetSpeed()
        {
            // Base value
            var myReturn = Spd;

            return myReturn;
        }

        // Get Defense
        public int GetDefense()
        {
            // Base value
            var myReturn = Def;

            return myReturn;
        }

        // Get Max Health
        public int GetHealthMax()
        {
            // Base value
            var myReturn = MaxHP;

            //I can change the code here to test the result
            // Name: test driven development (TDD) 
            // What code do I need to write to pass this case? 


            //condition coverage is a bitch

            if (myReturn < 0)
            {
                return 0;
            }

            if (myReturn > 20 * 10)
            {
                return 200;
            }

            return myReturn;
        }

        // Get Current Health
        public int GetHealthCurrent()
        {
            // Base value
            var myReturn = CurrentHP;

            return myReturn;
        }

        // Get the Level based damage
        // Then add in the monster damage
        public int GetDamage()
        {
            var myReturn = 0; // = GetLevelBasedDamage();  BaseDamage Already calculated in
            myReturn += Damage;

            return myReturn;
        }

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            return GetDamage();
        }

        #endregion GetAttributes

        #region Items
        // Gets the unique item (if any) from this monster when it dies...
        //public Item GetUniqueItem()
        //{
        //    var myReturn = ItemsViewModel.Instance.GetItem(UniqueItem);

        //    return myReturn;
        //}

        //// Drop all the items the monster has
        //public List<Item> DropAllItems()
        //{
        //    var myReturn = new List<Item>();

        //    // Drop all Items
        //    Item myItem;

        //    myItem = ItemsViewModel.Instance.GetItem(UniqueItem);
        //    if (myItem != null)
        //    {
        //        myReturn.Add(myItem);
        //    }
        //    return myReturn;
        //}

        #endregion Items


    }

}
