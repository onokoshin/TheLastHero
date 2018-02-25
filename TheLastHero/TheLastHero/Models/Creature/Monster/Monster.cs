using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TheLastHero.Models
{
    public class Monster : Creature
    {
        [PrimaryKey]
        public string Id { get; set; }
        //monster carry up to one item (not equipped item, it's a drop item)
        public string UniqueDropID { get; set; }

        public bool Drop;

        //default constructor
        public Monster()
        {
            //UniqueDrop = new Item();
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


    }

}