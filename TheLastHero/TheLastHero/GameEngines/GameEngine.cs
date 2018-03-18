using System;
using System.Collections;
using System.Collections.Generic;
using TheLastHero.Models;
using TheLastHero.Models.Battle;

namespace TheLastHero.GameEngines
{
    //the GameEngine class contains data structures that are heavily used in our game 
    //This also contains dialog 
    public class GameEngine
    {

        public string[] DialogCache { get; set; } = new string[5];
        public string ConsoleDialog1 { get; set; }

        // damage calculations are done inside this helper class. It has member function such as 
        // applyDamage(Creature attacker, Creature defender)
        // DamageCalculation damageCalculation;

        // Turn manager will control all the creatures’ battle actions such as, move, attack, skip and etc.
        // usage example: turnManager.move(character_1, “forward”) and
        // turnManager.attack(character1, monster2)
        // TurnManager turnManager;

        // indicates which level the player is playing, and it also tells manager to handle round related subjects.
        public int currentRound { get; set; }

        // this queue tells game turn manager which creature is next to be able to move around, attack, or // equip it
        // The queue stores characters and monsters in an order of speed, in other words, they are stored in an order of which creature moves next 
        //public Queue<Creature> speedQueue { get; set; } = new Queue<Creature>();
        public Queue<Monster> monsterQueue { get; set; } = new Queue<Monster>();
        public Queue<Character> characterQueue { get; set; } = new Queue<Character>();
        public Queue<Character> movedCharacters = new Queue<Character>();
        public Queue<Monster> movedMonsters = new Queue<Monster>();

        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static GameEngine _instance;

        public static GameEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEngine();
                }
                return _instance;
            }
        }

        //empty constructor
        private GameEngine()
        {
        }

        //updates game dialog 
        public void ClearDialogCache()
        {
            for (int i = 0; i < 5; i++)
            {
                DialogCache[i] = "";
            }
        }


    }
}
