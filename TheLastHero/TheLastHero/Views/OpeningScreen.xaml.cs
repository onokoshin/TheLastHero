using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpeningScreen : ContentPage
    {
        public OpeningScreen()
        {
            InitializeComponent();
        }

        async void MainPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        async void BattlePageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattlePage());
        }

        async void CharactersPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharactersPage());
        }

        async void MonstersPageButton_OnClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Monster());
        }

        async void ItemPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemsPage());
        }

        async void InventoryPageButton_OnClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new Inventory());
        }

        async void ScorePageButton_OnClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Score());
        }


    }
}