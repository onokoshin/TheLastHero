using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TheLastHero.Models;
using TheLastHero.Controller;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Data { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Data = new Item
            {
                Name = "Item name",
                Description = "This is an item description.",
                Guid = Guid.NewGuid().ToString(),
                HP = 0,
                MP = 0,
                Lvl = 0,
                Atk = 0,
                Def = 0,
                Spd = 0,
                Luk = 0,
                SpecialAbility = "",
                EquippableLocation = "",
                EquippedBy = "",
                Range = 0,
                Value = 0,
                Damage = 0,
                Type = ItemTypeEnum.Ring,
                Location = ItemLocationEnum.Finger,
                Attribute = AttributeEnum.Attack
                
            };

            BindingContext = this;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            // If the image in teh data box is empty, use the default one..
            if (string.IsNullOrEmpty(Data.ImgSource))
            {
                Data.ImgSource = ItemsController.DefaultImageURI;
            }

            if (string.IsNullOrEmpty(Data.Name))
            {
                Data.Name = "Default Item";
            }


            //assigns an appropriate string value 
            switch (Data.Location)
            {
                case ItemLocationEnum.Feet:
                    Data.EquippableLocation = "Feet";
                    break;
                case ItemLocationEnum.Finger:
                    Data.EquippableLocation = "Finger";
                    break;
                case ItemLocationEnum.Head:
                    Data.EquippableLocation = "Head";
                    break;
                case ItemLocationEnum.Necklass:
                    Data.EquippableLocation = "Necklass";
                    break;
                case ItemLocationEnum.OffHand:
                    Data.EquippableLocation = "OffHand";
                    break;
                case ItemLocationEnum.PrimaryHand:
                    Data.EquippableLocation = "Primaryhand";
                    break;
                default:
                    Data.EquippableLocation = "Finger";
                    break; 

            }

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


            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // The stepper function for Range
        void Range_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            RangeValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Value
        void Value_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ValueValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Damage
        void Damage_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            DamageValue.Text = String.Format("{0}", e.NewValue);
        }
    }
}
