using System;

namespace TheLastHero.Models
{
    public class Item
    {
        bool None;

        public Item()
        {

        }

        public Item(int round)
        {
            //creature c = new creature();

        }

        public Item(int round, bool None)
        {
            this.None = None; 

        }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }


}