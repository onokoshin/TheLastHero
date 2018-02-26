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
            Navigation.InsertPageBefore(new ScoresPage(), Navigation.NavigationStack[1]);

            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);

        }
    }
}
