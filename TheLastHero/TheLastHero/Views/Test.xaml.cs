using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TheLastHero.Models;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Test : ContentPage
    {
        public Character Character { get; set; }

        public Test()
        {
            InitializeComponent();

            Character = new Character
            {

            };

            BindingContext = this;
        }

        async void Accept_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "CreateCharacter", Character);
            await Navigation.PopModalAsync();
        }
    }
}