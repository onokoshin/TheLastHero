using System;
using System.Collections.Generic;
using System.Text;
using TheLastHero.Controller;

// Global switches for the overall game to use...

namespace TheLastHero.Models
{
    public static class GameGlobals
    {
        public static int GameCount = 1; 
        // Turn on to force Rolls to be non random
        public static bool ForceRollsToNotRandom = false;

        // What number should return for random numbers (1 is good choice...)
        public static int ForcedRandomValue = 1;

        // What number to use for ToHit values (1,2, 19, 20)
        public static int ForceToHitValue = 20;

        // Allow Random Items when monsters die...
        public static bool AllowMonsterDropItems = true;

        // Turn Off Random Number Generation, and use the passed in values.
        public static void SetForcedRandomNumbers(int value, int hit)
        {
            ForceRollsToNotRandom = true;
            ForcedRandomValue = value;
            ForceToHitValue = hit;
        }

        // Flip the Random State (false to true etc...)
        // Call this after setting, to restore...
        public static void ToggleRandomState()
        {
            ForceRollsToNotRandom = !ForceRollsToNotRandom;
        }

        //these static bools control debug settings 
        public static bool EnableCriticalHitDamage = false;
        public static bool EnableMiss = false;
        public static bool EnableCriticalMissProblems = false;
        public static string Output = ""; 

        public static async void PostCallAsync()
        {
            var number = 10;    // 10 items
            var level = 0;  // Max Value of 6
            var attribute = AttributeEnum.Unknown;  // Any Attribute
            var location = ItemLocationEnum.Unknown;    // Any Location
            var random = true;  // Random between 1 and Level
            var updateDataBase = true;  // Add them to the DB

            var myDataList = await ItemsController.Instance.GetItemsFromGame(number, level, attribute, location, random, updateDataBase);

            var myOutput = "No Results";

            if (myDataList != null && myDataList.Count > 0)
            {
                // Reset the output
                myOutput = "";

                foreach (var item in myDataList)
                {
                    // Add them line by one, use \n to force new line for output display.
                    myOutput += item.FormatOutput() + "\n";
                }
            }

            Output = myOutput; 
 
        }
    }
}
