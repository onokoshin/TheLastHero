﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TheLastHero.Models;
using TheLastHero.ViewModels;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoreDetailPage : ContentPage
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private ScoreDetailViewModel _viewModel;

        public ScoreDetailPage(ScoreDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        public ScoreDetailPage()
        {
            InitializeComponent();

            var data = new Score
            {
                Name = "Item 1",
            };

            _viewModel = new ScoreDetailViewModel(data);
            BindingContext = _viewModel;
        }


        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditScorePage(_viewModel));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeleteScorePage(_viewModel));
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}