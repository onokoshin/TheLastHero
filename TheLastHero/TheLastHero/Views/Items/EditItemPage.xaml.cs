using System;

using TheLastHero.Models;
using TheLastHero.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditItemPage : ContentPage
    {
        // ReSharper disable once NotAccessedField.Local
        private ItemDetailViewModel _viewModel;

        public Item Data { get; set; }

        public EditItemPage(ItemDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();

            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {


            //assigns value to an appropriate attribute 
            switch (Data.Attribute)
            {
                case AttributeEnum.Attack:
                    Data.Atk = Data.Value;
                    break;
                case AttributeEnum.CurrentHealth:
                    Data.HP = Data.Value;
                    break;
                case AttributeEnum.Defense:
                    Data.Def = Data.Value;
                    break;
                case AttributeEnum.Speed:
                    Data.Spd = Data.Value;
                    break;
                case AttributeEnum.MaxHealth:
                    Data.HP = Data.Value;
                    break;
                default:
                    break;

            }

            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // The stepper function for Value
        void Value_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ValueValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Range
        void Range_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            RangeValue.Text = String.Format("{0}", e.NewValue);
        }
    }
}
