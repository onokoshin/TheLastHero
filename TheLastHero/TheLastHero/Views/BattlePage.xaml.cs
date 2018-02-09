
using System;
using System.Collections;
using System.Collections.Generic;
using TheLastHero.GameEngine;
using TheLastHero.Models;
using Xamarin.Forms;

/** This is our battle controller. all logical actions related to battle are 
 * written here. We will use a Queue structure for turn management, the queue 
 * takes all creatures based on their speed. Hence, the queue is in an order of 
 * which creature goes next. The queue is declared as “nextOneQueue” in our code 
 * below.  In order to store creatures into a queue in a proper order, we are
 * going to sort both character list and monster list. Then we will compare head 
 * of each list’s creature’s speed to determine which creature would be enqueued
 * into our queue. This way, creatures with higher speed will get their turn 
 * first. Once, a creature is dequeued, we will first check whether the 
 * creature is alive or dead based on creature’s ‘liveStatus.’ 
 * If the creature is dead, we skip and not enqueue the creature back to
 * the queue. If the creature is alive, we will use turnManager to control 
 * its actions.   

Turn manager,turnManager, will control all the creatures’ battle actions: move, 
attack, skip and etc. In every round, every creature only be allowed to control 
action once. Character movement will be decide and input by user then
turnManager will implement users’ turn decisions. Monster movement will be 
decided by Game engine randomly. The turnManager will oftern update if any 
creature is eliminated in each round.

Grid Layout ,battleGrid, is our battle game board, we have a 4x6 size board, 
each grid element is clickable. We will capture the click event and do 
calculations and updates here. The position of monsters and characters are 
memorized by a 2 dimensional integer array. Int [][] map, 0 indicate this
tile is empty, if a monster or character is occupying this tile, we will 
register the array with its ID. Later on, we can use this map to do movement 
and attack calculation.  

There will be 6 monsters in each battle. The monster will grow stronger each 
round linearly, and our character has a cap level at 20. So eventually, all 
the monsters will become very powerful and kill all the characters. Every 4 
rounds, the characters will engage a boss fight, which there is only one 
monster. The boss monster is 6 times stronger than the normal monster. The 
BOSSROUND is a const indicate every 4 rounds there will be the boss fight.

**/
namespace TheLastHero.Views
{
    public partial class BattlePage : ContentPage
    {

        // Every four rounds, game engine will generate a new boss monster
        const int BOSSROUND = 4;

        // Batstle map is a grid layout  
        Grid battleGrid = new Grid();

        //use 2d array to instantiate a battlemap 6 x 4
        int[,] battleMap = new int[6, 4];

        // The data structure list contains selected six characters passed from    SelectCharacterPage
        public List<Character> characters;

        // The data structure list contains contains six monsters that will appear in each battle
        // The six monsters will be selected by using generateMonster() function 
        public List<Monster> monsters;

        // List of Items 
        public List<Item> itemPool;

        // indicates which level the player is playing, and it also tells manager to handle round related subjects.
        public int currentRound;
        // this queue tells game turn manager which creature is next to be able to move around, attack, or // equip it
        // The queue stores characters and monsters in an order of speed, in other words, they are stored in an order of which creature moves next 
        public Queue<Creature> nextOneQueue;
        // damage calculations are done inside this helper class. It has member function such as 
        // applyDamage(Creature attacker, Creature defender)
        DamageCalculation damageCalculation;
        // Turn manager will control all the creatures’ battle actions such as, move, attack, skip and etc.
        // usage example: turnManager.move(character_1, “forward”) and
        // turnManager.attack(character1, monster2)
        TurnManager turnManager;

        //Constructor 
        public BattlePage()
        {
            InitializeComponent();
        }



        // This method will be called upon beginning of the game, character movement,         
        //   monster movement, character die, monster die events. 

        public void startGame()
        {
            nextOneQueue = new Queue<Creature>();


            //TurnManager turnManager = new TurnManager();

            // game starts
            // nextOneQueue = InitializeQueue(monsters, characters);
            while (true)
            {
                // If either number of characters or number of monster equals zero, the battle is over.
                if (characters.Count == 0)
                {

                }
                else if (monsters.Count == 0)
                {
                    // we will generate a new set of monsters for a new round 
                }
                nextOneQueue.Dequeue();



            }

        }


        //After every creature died, we will update our new battle map.
        public void UpdateGrid(Grid map, int[][] mapAry)
        {

        }

        // Using queue data structure to store characters and monster in an order of which creature moves next based on a creature’s speed 
        public Queue InitializeQueue(List<Character> characters, List<Monster> monsters)
        {
            //check every character and monster to determine which creature gets enqueued first
            //sort character list and monster list based on their speed 
            //then we will compare head of each list to determine whether the character or monster is
            // faster. Then we will enqueue the creature with higher speed 
            return null;
        }

        //This method checks existing queue that contains characters and monsters to determine
        // which character or monster goes next
        public void CheckNextTurn()
        {

            //dequeue when creature died
            var Creature = nextOneQueue.Dequeue();

            //Check whether the creature is still alive
            // if (!creature.liveStatus)
            //    return;

            //if the creature is still alive, we are going to pass that creature into 
            // method overloading to check current creature(monster or character)
            // turnMgr.moveCharacter(Creature , int x, int y, int[][] map); 

            // turnManagement component

        }


        // This method will generate monsters for each round. Initially, the game will have 
        // six monsters and each four rounds, the method will generate a boss monster to       
        // characters
        public List<Monster> GenerateMonsters(int curRound)
        {

            //Check whether the monster is the boss monster
            bool isBoss = false;

            // After every three rounds, our game will generate a new boss monster
            if (curRound % BOSSROUND == 0)
            {
                isBoss = true;
                Monster monster = new Monster(curRound, isBoss);

            }
            else
            {
                // Generate normal monsters

                for (int i = 0; i < 6; i++)
                {
                    Monster monster = new Monster(curRound, isBoss);
                    monsters.Add(monster);
                }

            }
            return monsters;
        }

    }
}

