using System;
using System.Collections.Generic;
using System.Text;
using TheLastHero.Models;

namespace TheLastHero.GameEngines
{
    //the helper engine's job is to have rolldice so that our game can call the rollDice function
    public static class HelperEngine
    {
        //creates a random generator 
        private static Random rnd = new Random();

        //rollDice function rolls die/dice and returns the value 
        public static int RollDice(int rolls, int dice)
        {
            if (rolls < 1)
            {
                return 0;
            }

            if (dice < 1)
            {
                return 0;
            }

            if (GameGlobals.ForceRollsToNotRandom)
            {
                return GameGlobals.ForcedRandomValue * rolls;
            }

            var myReturn = 0;

            for (var i = 0; i < rolls; i++)
            {
                // Add one to the dice, because random is between.  So 1-10 is rnd.Next(1,11)
                myReturn += rnd.Next(1, dice + 1);
            }

            return myReturn;
        }
    }
}
