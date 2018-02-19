using System;

using TheLastHero.Models;

namespace TheLastHero.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Data { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Name;
            Data = item;
        }
    }
}
