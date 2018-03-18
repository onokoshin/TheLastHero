using System;
using System.Collections.Generic;
using System.Linq;

namespace TheLastHero.Models
{
    public enum ItemTypeEnum
    {
        // Not specified
        Unknown = 0,

        // The head includes, Hats, Helms, Caps, Crowns, Hair Ribbons, Bunny Ears, and anything else that sits on the head
        weapon = 10,

        // Armor type
        Armor = 20,

        // Ring type
        Ring = 30,

    }

    // Helper functions for the Item Locations
    public static class ItemTypeList
    {
        // Gets the lsit of locations that an Item can have.
        // Does not include the Left and Right Finger
        public static List<string> GetItemType
        {
            get
            {
                var myList = Enum.GetNames(typeof(ItemTypeEnum)).ToList();
                var myReturn = myList.Where(a =>
                                            a.ToString() != ItemTypeEnum.Unknown.ToString()
                                            )
                                            .OrderBy(a => a)
                                            .ToList();
                return myReturn;
            }
        }



        // Given the String for an enum, return its value.  That allows for the enums to be numbered 2,4,6 rather than 1,2,3
        public static ItemTypeEnum ConvertStringToEnum(string value)
        {
            return (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), value);
        }


    }
}

