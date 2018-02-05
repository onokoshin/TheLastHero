using System;
using System.Collections;
using System.Collections.Generic;
using TheLastHero.Models;
using Xamarin.Forms;

namespace TheLastHero.Views
{
    public partial class BattlePage : ContentPage
    {
        const int BOSSROUND = 4;

        // Batstle map is a grid layout  
        Grid battleGrid = new Grid();

        //use 2d array to instantiate a battlemap 6 x 4
        int[,] battleMap = new int[6, 4];

        // The data structure list contains selected six characters passed from SelectCharacterPage
        public List<Character> characters;

        // The data structure list contains contains six monsters that will appear in each battle
        // The six monsters will be selected by using generateMonster() function 
        public List<Monster> monsters;

        // List of Items 
        public List<Item> itemPool;

        // indicates which level the player is playing, and it also tells manager to handle round related 
        // issues.
        public int currentRound;

        // this queue tells game turn manager which creature is next to be able to move around, attack, or // equip it
        // The queue stores characters and monsters in an order of speed, in other words, they are stored in an order of which creature moves next 
        public Queue<Creature> nextOneQueue;


        public BattlePage()
        {

            InitializeComponent();
        }

        public void startGame()
        {
            nextOneQueue = new Queue<Creature>();
            // read 

            //TurnManager turnManager = new TurnManager();

            // game starts
            // nextOneQueue = InitializeQueue(monsters, characters);
            while (true)
            {
                if (characters.Count == 0)
                    //Game over
                    // pushAsync(Report.xaml)   
                    if (monsters.Count == 0)
                    {
                        // we will generate a new set of monsters for a new round 
                    }
                nextOneQueue.Dequeue();

                // This method will be called upon beginning of the game, character movement, monster movement, character die, monster die events. 


            }

        }

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

            //dequeue
            var Creature = nextOneQueue.Dequeue();

            //Check whether the creature is still alive
            // if (!creature.liveStatus)
            //    return;

            //if the creature is still alive, we are going to pass that creature into 
            // method overloading to check current creature(monster or character)
            // turnMgr.moveCharacter(Creature , int x, int y, int[][] map); 

            // turnManagement component

        }



        public List<Monster> GenerateMonsters(int curRound)
        {

            bool isBoss = false;

            // After every three rounds, our game will generate a new boss monster
            if (curRound % BOSSROUND == 0)
            {
                isBoss = true;
                Monster monster = new Monster(curRound, isBoss);

            }
            else
            {
                //Need to reconsider 
                // discuss it later
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