using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    public class AttributeBase
    {
        // The Spd of the character, impacts movement, and initiative
        public int Spd { get; set; }

        // The Def score, to be used for defending against attacks
        public int Def { get; set; }

        // The Attack score to be used when attacking
        public int Atk { get; set; }

        // Current Health which is always at or below MaxHp
        public int CurrentHP { get; set; }

        // The highest value health can go
        public int MaxHp { get; set; }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = "Speed : " + Spd +
                            " , Defense : " + Def +
                            " , Attack : " + Atk +
                            " , CurrentHP : " + CurrentHP +
                            " , MaxHp : " + MaxHp;
            return myReturn.Trim();
        }

        // Construtor sets defaults
        public AttributeBase()
        {
            SetDefaultValues();
        }

        // Defaults are all value 1, and then adjusted by scaling up
        private void SetDefaultValues()
        {
            Spd = 1;
            Def = 1;
            Atk = 1;
            CurrentHP = 1;
            MaxHp = 1;
        }

        // Return attributebase based on a string as the constructor.
        public AttributeBase(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                SetDefaultValues();
                return;
            }

            var myAttributes = JsonConvert.DeserializeObject<AttributeBase>(data);

            Spd = myAttributes.Spd;
            Def = myAttributes.Def;
            Atk = myAttributes.Atk;
            CurrentHP = myAttributes.CurrentHP;
            MaxHp = myAttributes.MaxHp;
        }

        // Return a formated string of the datastruture
        public static string GetAttributeString(AttributeBase data)
        {
            var myString = (JObject)JToken.FromObject(data);

            return myString.ToString();
        }

        // Given a string of attributes, convert them to actual attributes
        public static AttributeBase GetAttributeFromString(string data)
        {
            AttributeBase myResult;

            // Convert the string to json object
            // convert the json object to the class
            // retun the class

            // make sure the object is properly formated json for the object type
            try
            {
                myResult = JsonConvert.DeserializeObject<AttributeBase>(data);
                return myResult;
            }

            catch (Exception)
            {
                // Failed, so fall through to the return of new.
                return new AttributeBase();
            }
        }
    }
}