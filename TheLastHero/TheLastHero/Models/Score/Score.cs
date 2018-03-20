using System;
using SQLite;

namespace TheLastHero.Models
{
    //this is the key class that contains all the scoring related data 
    public class Score
    {
        [PrimaryKey]
        public string Id { get; set; }
        //score name 
        public string Name { get; set; }

        //score total is going to be based on experiences 
        public int ScoreTotal { get; set; }

        // The Date the game played, and when the score was saved
        public DateTime GameDate { get; set; }

        // Tracks if auto battle is true, or if user battle = false
        public bool AutoBattle { get; set; }

        // The number of turns the battle took to finish
        public int TurnCount { get; set; }

        // The number of rounds the battle took to finish
        public int RoundCount { get; set; }

        // The count of monsters slain during battle
        public int MonsterSlainNumber { get; set; }

        // The total experience points all the characters received during the battle
        public int ExperienceGainedTotal { get; set; }

        // A list of all the characters at the time of death and their stats.  Needs to be in json format, so saving a string
        public string CharacterAtDeathList { get; set; }

        // All of the monsters killed and their stats. Needs to be in json format, so saving as a string
        public string MonstersKilledList { get; set; }

        // All of the items dropped and their stats. Needs to be in json format, so saving as a string
        public string ItemsDroppedList { get; set; }

        //constructor builds the basic structure by assigning default values
        public Score()
        {
            Id = Guid.NewGuid().ToString();
            Name = "Score" + GameGlobals.GameCount; 
            GameDate = DateTime.Now;    // Set to be now by default.
            AutoBattle = false;         //assume user battle
            CharacterAtDeathList = null;
            MonstersKilledList = null;
            ItemsDroppedList = null;

            TurnCount = 0;
            RoundCount = 0;
            ExperienceGainedTotal = 0;
            MonsterSlainNumber = 0;
        }

        //updates local properties
        public void Update(Score newData)
        {
            if (newData == null)
            {
                return;
            }

            // Update all the fields in the Data, except for the Id
            Name = newData.Name;
            ScoreTotal = newData.ScoreTotal;
            GameDate = newData.GameDate;
            AutoBattle = newData.AutoBattle;
            MonsterSlainNumber = newData.MonsterSlainNumber;
            ExperienceGainedTotal = newData.ExperienceGainedTotal;
            CharacterAtDeathList = newData.CharacterAtDeathList;
            MonstersKilledList = newData.MonstersKilledList;
            ItemsDroppedList = newData.ItemsDroppedList;

        }

        #region ScoreItems

        // Adding a character to the score output as a text string
        public bool AddCharacterToList(Character data)
        {
            if (data == null)
            {
                return false;
            }

            CharacterAtDeathList += data.FormatOutput() + "\n";
            return true;
        }

        // All a monster to the list of monsters and their stats
        public bool AddMonsterToList(Monster data)
        {
            if (data == null)
            {
                return false;
            }

            MonstersKilledList += data.FormatOutput() + "\n";
            return true;
        }

        //All an item to the list of items for score and their stats
        public bool AddItemToList(Item data)
        {
            if (data == null)
            {
                return false;
            }

            ItemsDroppedList += data.FormatOutput() + "\n";
            return true;
        }
        #endregion ScoreItems
    }
}
