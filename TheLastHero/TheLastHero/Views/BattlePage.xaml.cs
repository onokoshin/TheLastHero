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

            // auto play only
            //ResetQueue();

            //_script.scriptCounter = 1;
            //RunScript(_script, 0);

            _viewModel.battle.SetAllBackground("Grass.png");

            _viewModel.battle.RefreshAllCell();

            BindingContext = _viewModel;
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

        private void RunScript(Script s, int s_num)
        {
            string hp = "";
            bool moved = false;
            bool attacked = false;
            // dequeue current creature from speed queue
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
                            // remove dead creature
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

                if (_viewModel.gameEngine.speedQueue.Count > 0)
                {
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

                    _viewModel.gameEngine.speedQueue.Enqueue(currentCreature);
                }
                else
                {
                    _viewModel.gameEngine.DialogCache[0] = "Game Over";
                }


                _viewModel.gameEngine.ConsoleDialog1 = _viewModel.gameEngine.DialogCache[0] + "\n"
                    + _viewModel.gameEngine.DialogCache[1] + "\n"
                    + _viewModel.gameEngine.DialogCache[2] + "\n"
                    + _viewModel.gameEngine.DialogCache[3] + "\n"
                    + _viewModel.gameEngine.DialogCache[4];

                //  set Dead


            }
            else
            {
                _viewModel.gameEngine.ConsoleDialog1 = "empty queue";
            }

            // update grid
            _viewModel.battle.SetAllTop("");

            if (s.scriptCounter < 34)
            {
                _viewModel.battle.SetAllBackground("Grass.png");

            }
            else
            {
                _viewModel.battle.SetAllBackground("Sand.png");

            }

            _viewModel.battle.SetAllSelection("HighlightGrey.png");
            for (int i = 0; i < s.GetScripts()[s_num].Length; i = i + 7)
            {

                if (s.GetScripts()[s_num][i] == 1)
                {
                    _viewModel.battle.battleMapTop[s.GetScripts()[s_num][i + 1], s.GetScripts()[s_num][i + 2]] = s.imgAry[s.GetScripts()[s_num][i + 4]];

                }
                else
                {

                    _viewModel.battle.battleMapTop[s.GetScripts()[s_num][i + 1], s.GetScripts()[s_num][i + 2]] = "";

                }
            }
            _viewModel.battle.RefreshAllCell();

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

        public void Next_Clicked(object sender, EventArgs e)
        {
            // do something
            //if (true)

            _viewModel.battle.cell_00_bottom = "Sand.png";
            if (_script.scriptCounter > 49)
            {
                Navigation.InsertPageBefore(new GameOver(), Navigation.NavigationStack[1]);
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            }
            else
            {
                RunScript(_script, _script.scriptCounter);
                _script.scriptCounter++;
                BindingContext = null;
                BindingContext = _viewModel;

            }
        }



        public void Reset_Clicked(object sender, EventArgs e)
        {
            ResetQueue();

            _viewModel.gameEngine.ClearDialogCache();
            _script.scriptCounter = 1;
            // do something
            //_viewModel.Data.battle.battleMapTop[0, 0] = "KnightRight.png";
            _viewModel.battle.SetAllSelection("HighlightGrey.png");
            _viewModel.battle.SetAllBackground("Grass.png");
            _viewModel.battle.SetAllTop("");

            foreach (var c in _viewModel.CharacterDataset)
            {
                _viewModel.battle.battleMapTop[c.xPosition, c.yPosition] = c.ImgSource;
            }

            foreach (var m in _viewModel.MonsterDataset)
            {
                _viewModel.battle.battleMapTop[m.xPosition, m.yPosition] = m.ImgSource;
            }

            _viewModel.battle.RefreshAllCell();
            _viewModel.gameEngine.ConsoleDialog1 = "Reset Clicked";
            BindingContext = null;
            BindingContext = _viewModel;
        }

        public void HandleButtonClicked(int x, int y)
        {
            // clean highlight
            _viewModel.battle.SetAllSelection("HighlightGrey.png");

            _viewModel.gameEngine.ConsoleDialog1 = x.ToString() + " " + y.ToString();
            _viewModel.battle.battleMapSelection[x, y] = "HighlightGreen.png";
            if (x > 0)
                _viewModel.battle.battleMapSelection[x - 1, y] = "HighlightRed.png";
            if (x < 4)
                _viewModel.battle.battleMapSelection[x + 1, y] = "HighlightRed.png";
            if (y > 0)
                _viewModel.battle.battleMapSelection[x, y - 1] = "HighlightRed.png";
            if (y < 5)
                _viewModel.battle.battleMapSelection[x, y + 1] = "HighlightRed.png";

            _viewModel.battle.RefreshAllCell();

            BindingContext = null;
            BindingContext = _viewModel;
        }

        public void Cell00Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 0); }
        public void Cell01Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 1); }
        public void Cell02Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 2); }
        public void Cell03Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 3); }
        public void Cell04Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 4); }
        public void Cell05Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 5); }
        public void Cell10Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 0); }
        public void Cell11Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 1); }
        public void Cell12Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 2); }
        public void Cell13Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 3); }
        public void Cell14Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 4); }
        public void Cell15Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 5); }
        public void Cell20Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 0); }
        public void Cell21Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 1); }
        public void Cell22Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 2); }
        public void Cell23Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 3); }
        public void Cell24Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 4); }
        public void Cell25Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 5); }
        public void Cell30Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 0); }
        public void Cell31Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 1); }
        public void Cell32Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 2); }
        public void Cell33Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 3); }
        public void Cell34Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 4); }
        public void Cell35Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 5); }
        public void Cell40Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 0); }
        public void Cell41Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 1); }
        public void Cell42Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 2); }
        public void Cell43Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 3); }
        public void Cell44Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 4); }
        public void Cell45Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 5); }
    }
}