using System;
using System.Collections.Generic;
using TheLastHero.Models;
using TheLastHero.Models.Battle;
using TheLastHero.ViewModels;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using TheLastHero.GameEngines;

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

        private GameEngineViewModel _viewModel;

        private CharactersViewModel charactersViewModel;

        //Constructor 
        public BattlePage(CharactersViewModel Data)
        {
            charactersViewModel = Data;
            InitializeComponent();

            // _viewModel = GameEngineViewModel.Instance;
            _viewModel = new GameEngineViewModel();
            _viewModel.gameOver = false;
            _viewModel.magicRevive = true;
            _viewModel.MoveFirstCreature(charactersViewModel);
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

        private void Potion_Clicked(object sender, EventArgs e)
        {
            _viewModel.UsePotion();
            BindingContext = null;
            BindingContext = _viewModel;
        }

        private void FocusAtk_Clicked(object sender, EventArgs e)
        {
            _viewModel.UseFocusAtk();
            BindingContext = null;
            BindingContext = _viewModel;
        }

        private void HandleButtonClicked(int x, int y)
        {
            _viewModel.HandleButtonClicked(x, y);

            if (_viewModel.gameOver)
            {
                Navigation.InsertPageBefore(new GameOver(), Navigation.NavigationStack[1]);
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }

            BindingContext = null;
            BindingContext = _viewModel;
        }
    }
}
