
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastHero.GameEngines;
using TheLastHero.Models;
using TheLastHero.ViewModels;
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
        // Batstle map is a grid layout  
        Grid battleGrid = new Grid();
        private GameEngineViewModel _viewModel;

        //Constructor 
        public BattlePage()
        {
            InitializeComponent();
            _viewModel = GameEngineViewModel.Instance;
            _viewModel.gameEngine.SetAllSelection("HighlightGrey.png");
            _viewModel.gameEngine.SetAllBackground("Grass.png");
            _viewModel.gameEngine.battleMapTop[0, 1] = "MageRight.png";
            _viewModel.gameEngine.battleMapTop[0, 2] = "KnightRight.png";
            _viewModel.gameEngine.battleMapTop[0, 3] = "ThiefRight.png";
            _viewModel.gameEngine.battleMapTop[0, 4] = "ArcherRight.png";
            _viewModel.gameEngine.battleMapTop[4, 1] = "WolfLeft.png";
            _viewModel.gameEngine.battleMapTop[4, 2] = "WolfLeft.png";
            _viewModel.gameEngine.battleMapTop[4, 3] = "WolfLeft.png";
            _viewModel.gameEngine.battleMapTop[4, 4] = "WolfLeft.png";
            _viewModel.gameEngine.ConsoleDialog = "HelloWorld";
            _viewModel.gameEngine.RefreshAllCell();
            BindingContext = _viewModel;
        }

        public void UpdateConsoleDialog(string input)
        {


        }

        public void Demo_Clicked(object sender, EventArgs e)
        {
            // do something
            //_viewModel.Data.battle.battleMapTop[0, 0] = "KnightRight.png";
            _viewModel.gameEngine.battleMapSelection[0, 1] = "HighlightGreen.png";
            _viewModel.gameEngine.battleMapSelection[1, 1] = "HighlightRed.png";
            _viewModel.gameEngine.battleMapSelection[0, 0] = "HighlightRed.png";

            _viewModel.gameEngine.RefreshAllCell();
            _viewModel.gameEngine.ConsoleDialog = "Clicked";
            BindingContext = null;
            BindingContext = _viewModel;
        }


        //After every creature died, we will update our new battle map.
        public void UpdateGrid(Grid map, int[][] mapAry)
        {


        }


    }
}

