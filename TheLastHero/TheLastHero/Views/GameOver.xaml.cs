using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TheLastHero.Views
{
    public partial class GameOver : ContentPage
    {
        public GameOver()
        {
            InitializeComponent();
        }

        async void ScorePageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoresPage());
        }
    }
}
