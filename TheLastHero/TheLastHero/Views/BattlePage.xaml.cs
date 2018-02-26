using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TheLastHero.GameEngines;
using TheLastHero.Models;
using TheLastHero.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


/** This is our battle controller. all logical actions related to battle are 
 * written here. We will use a Queue structure for turn management, the queue 
 * takes all creatures based on their speed. Hence, the queue is in an order of 
 * which creature goes next. The queue is declared as “ speedQueue” in our code 
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
        // Batstle map is a grid layout  
        //Grid battleGrid = new Grid();
        private GameEngineViewModel _viewModel;
        Script _script = new Script();


        //Constructor 
        public BattlePage()
        {
            InitializeComponent();
            _viewModel = GameEngineViewModel.Instance;

            ResetQueue();

            _script.scriptCounter = 1;
            RunScript(_script, 0);




            BindingContext = _viewModel;
        }


        public void newRound()
        {

        }

        public void UpdateConsoleDialog(string input)
        {


        }

        private void RunScript(Script s, int s_num)
        {
            string hp = "";
            bool moved = false;
            bool attacked = false;
            // its this creature's turn
            if (_viewModel.gameEngine.speedQueue.Count > 0 && s_num != 0)
            {

                Queue<Creature> swapQueue = new Queue<Creature>();

                while (_viewModel.gameEngine.speedQueue.Count > 0)
                {
                    Creature c = _viewModel.gameEngine.speedQueue.Dequeue();
                    bool matchAndDead = false;
                    for (int i = 0; i < s.GetScripts()[s_num].Length; i = i + 7)
                    {
                        if (c.demoID == s.GetScripts()[s_num][i + 4])
                        {
                            hp = s.GetScripts()[s_num][i + 3].ToString();
                            if (s.GetScripts()[s_num][i + 5] == 1)
                            {
                                moved = true;
                            }
                            if (s.GetScripts()[s_num][i + 6] == 1)
                            {
                                attacked = true;
                            }
                            //found and dead
                            if (s.GetScripts()[s_num][i] == 0)
                            {
                                matchAndDead = true;
                            }
                        }

                    }
                    if (!matchAndDead)
                    {
                        swapQueue.Enqueue(c);
                    }
                }


                while (swapQueue.Count > 0)
                {
                    Creature c = swapQueue.Dequeue();
                    _viewModel.gameEngine.speedQueue.Enqueue(c);
                }


                var currentCreature = _viewModel.gameEngine.speedQueue.Dequeue();
                for (int i = 4; i > 0; i--)
                {
                    _viewModel.gameEngine.DialogCache[i] = _viewModel.gameEngine.DialogCache[i - 1];
                }
                _viewModel.gameEngine.DialogCache[0] = "Turn " + _script.scriptCounter + ": " + currentCreature.Name + " " + hp + "HP ";

                if (attacked && !moved)
                {
                    _viewModel.gameEngine.DialogCache[0] += "is attacking";

                }
                else if (attacked && moved)
                {
                    _viewModel.gameEngine.DialogCache[0] += " moved and is attacking";

                }
                else
                {
                    _viewModel.gameEngine.DialogCache[0] += " moved";

                }

                _viewModel.gameEngine.ConsoleDialog1 = _viewModel.gameEngine.DialogCache[0] + "\n"
                    + _viewModel.gameEngine.DialogCache[1] + "\n"
                    + _viewModel.gameEngine.DialogCache[2] + "\n"
                    + _viewModel.gameEngine.DialogCache[3] + "\n"
                    + _viewModel.gameEngine.DialogCache[4];

                _viewModel.gameEngine.speedQueue.Enqueue(currentCreature);
                //  set Dead


                /* for (int i = 0; i < s.GetScripts()[s_num].Length; i = i + 7)
                 {
                     //found and dead
                     if (s.GetScripts()[s_num][i + 4] == c.demoID && s.GetScripts()[s_num][i] == 0)
                     {
                         isDead = true;
                     }//otherwise

                 }
                 if (!isDead)
                 {
                     _viewModel.gameEngine. speedQueue.Enqueue(c);

                 }*/

            }
            else
            {
                _viewModel.gameEngine.ConsoleDialog1 = "empty queue";
            }

            _viewModel.gameEngine.SetAllTop("");

            if (s.scriptCounter < 41)
            {
                _viewModel.gameEngine.SetAllBackground("Grass.png");

            }
            else
            {
                _viewModel.gameEngine.SetAllBackground("Lava.png");

            }

            _viewModel.gameEngine.SetAllSelection("HighlightGrey.png");
            for (int i = 0; i < s.GetScripts()[s_num].Length; i = i + 7)
            {
                if (s.GetScripts()[s_num][i] == 1)
                {
                    _viewModel.gameEngine.battleMapTop[s.GetScripts()[s_num][i + 1], s.GetScripts()[s_num][i + 2]] = s.imgAry[s.GetScripts()[s_num][i + 4]];

                }
                else
                {

                    _viewModel.gameEngine.battleMapTop[s.GetScripts()[s_num][i + 1], s.GetScripts()[s_num][i + 2]] = "";

                }
            }
            _viewModel.gameEngine.RefreshAllCell();

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(0);
            }

            InitializeComponent();

            if (_viewModel.CharacterDataset.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
        }

        public async void Next_ClickedAsync(object sender, EventArgs e)
        {
            // do something
            if (true)
            //if (_script.scriptCounter == 44)
            {
                await Navigation.PushAsync(new GameOver());
            }
            else
            {
                RunScript(_script, _script.scriptCounter);
                _script.scriptCounter++;
                BindingContext = null;
                BindingContext = _viewModel;

            }



        }

        public void ResetQueue()
        {
            if (_viewModel.CreatureDataset.Count > 0)
            {
                _viewModel.gameEngine.speedQueue.Clear();
                foreach (Creature c in _viewModel.CreatureDataset)
                {

                    _viewModel.gameEngine.speedQueue.Enqueue(c);
                }

            }
        }

        public void Reset_Clicked(object sender, EventArgs e)
        {
            ResetQueue();

            _viewModel.gameEngine.ClearDialogCache();
            _script.scriptCounter = 1;
            // do something
            //_viewModel.Data.battle.battleMapTop[0, 0] = "KnightRight.png";
            _viewModel.gameEngine.SetAllSelection("HighlightGrey.png");
            _viewModel.gameEngine.SetAllBackground("Grass.png");
            _viewModel.gameEngine.SetAllTop("");

            foreach (var c in _viewModel.CharacterDataset)
            {
                _viewModel.gameEngine.battleMapTop[c.xPosition, c.yPosition] = c.ImgSource;
            }

            foreach (var m in _viewModel.MonsterDataset)
            {
                _viewModel.gameEngine.battleMapTop[m.xPosition, m.yPosition] = m.ImgSource;
            }

            _viewModel.gameEngine.RefreshAllCell();
            _viewModel.gameEngine.ConsoleDialog1 = "Reset Clicked";
            BindingContext = null;
            BindingContext = _viewModel;
        }

        //After every creature died, we will update our new battle map.
        public void UpdateGrid(Grid map, int[][] mapAry)
        {


        }


    }
}