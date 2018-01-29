using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TheLastHero.Views
{
    public partial class CharacterPage : ContentPage
    {
        public CharacterPage()
        {
            InitializeComponent();

        }

        async void CreateCharacter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void EditCharacter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void DeleteCharacter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}
