using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            _viewModel.AutoBattle();
            await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(_viewModel.BattleScore)));
        }

    }
}
