using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastHero.Controller;
using TheLastHero.Models;
using TheLastHero.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpeningScreen : ContentPage
    {

        //GameEngineViewModel _viewModel = GameEngineViewModel.Instance;
        GameEngineViewModel _viewModel;
        
        public OpeningScreen()
        {
            InitializeComponent();
            _viewModel = new GameEngineViewModel();
            
        }

        async void BattlePageButton_OnClicked(object sender, EventArgs e)
        { 
            //post call to get all the items 
            GameGlobals.PostCallAsync(); 

            await Navigation.PushAsync(new CharacterSelectionPage());

            
        }

        async void CharactersPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharactersPage());
        }

        async void MonstersPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonstersPage());
        }

        async void ItemPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemsPage());
        }

        async void ScorePageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoresPage());
        }

        async void AboutPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        async void AutoBattleButton_OnClicked(object sender, EventArgs e)
        {

            var success = _viewModel.AutoBattle();
            
            if(success == 0)
            {
                //if auto battle can run successfully, it calls item post call to get items
                GameGlobals.PostCallAsync();

                //I figured that there is no need to display all the items 
                //var myOutput = GameGlobals.Output; 
                //var answer = await DisplayAlert("Returned List", myOutput, "Yes", "No");

                await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(_viewModel.BattleScore)));
            }else if(success == -1)
            {
                await DisplayAlert("Alert", "Please have at least 6 characters to play Auto Battle", "OK");
            }
            else if (success == -2)
            {
                await DisplayAlert("Alert", "Please have at least 1 monster to play Auto Battle", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Please have at least 1 item to play Auto Battle", "OK");
            }
            
        }

    }
}
