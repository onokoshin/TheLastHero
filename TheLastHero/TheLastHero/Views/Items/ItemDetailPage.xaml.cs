﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TheLastHero.Models;
using TheLastHero.ViewModels;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        private ItemDetailViewModel _viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var data = new Item
            {
                // Text = "Item 1",
                //  Description = "This is an item description."
            };

            _viewModel = new ItemDetailViewModel(data);
            BindingContext = _viewModel;
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditItemPage(_viewModel));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeleteItemPage(_viewModel));
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}