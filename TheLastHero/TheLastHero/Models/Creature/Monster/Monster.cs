using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    public class monster : creature
    {
        //monster carry up to one item (not equipped item, it's a drop item) 
        public Item UniqueDrop { get; set; }

        //overloaded constructor 
        //monster attributes will be nased on the round being passed from battle engine
        //Ex: round 1 monster - weak attributes, but round 10 monster - strong attributes
        //@param level - level will be passed in depending on which round it is 
        public monster(int round)
        {
            //3, 6, 9...rounds are boss rounds 
            //every 3 rounds will be boss rounds with stronger monsters
            if (round % 3 == 0)
            {
                //this indicates that it is a boss round
                //attributes will be enhanced

                Item UniqueDrop = new Item(round);

            }
            else
            {
                Random rand = new Random();
                int randNum = rand.Next(1, 10);

                //50% of monsters will have items and the other 50% do not have drop items 
                if (randNum < 6)
                {
                    Item UniqueDrop = new Item(round);
                }
                else
                {
                    bool None = true;
                    Item UniqueDrop = new Item(round, None);
                }
            }
        }

        //monster drops its item 
        public Item dropItem()
        {
            return UniqueDrop;
        }
    }

}