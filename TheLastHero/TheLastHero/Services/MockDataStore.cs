﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastHero.Models;

namespace TheLastHero.Services
{
    //The mockdatastore stores mock data so that we can test and play the game 
    public class MockDataStore : IDataStore
    {
        //each east is to store different data, item, monster, chracter, and score objects 
        private List<Item> _itemDataset = new List<Item>();
        private List<Monster> _monsterDataset = new List<Monster>();
        private List<Score> _scoreDataset = new List<Score>();
        private List<Character> _characterDataset = new List<Character>();

        //const string to locate items 
        const string HEAD = "HEAD";
        const string BODY = "BODY";
        const string FEET = "FEET";
        const string LEFTHAND = "LEFTHAND";
        const string RIGHTHAND = "RIGHTHAND";
        const string LEFTFINGER = "LEFTFINGER";
        const string RIGHTFINGER = "RIGHTFINGER";

        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static MockDataStore _instance;

        public static MockDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MockDataStore();
                }
                return _instance;
            }
        }

        //the constructor instantiate basic datas 
        private MockDataStore()
        {
            var mockItems = new List<Item>
            {
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Hat", HP=8, MP=5, Type = ItemTypeEnum.Armor, Lvl=3, Atk=10,Def=10, Spd=12, Luk=1,SpecialAbility="null", EquippableLocation="Head", Location=ItemLocationEnum.Head, EquippedBy="Empty", ImgSource="ClothHead.png", Range=0, Damage=0, Value=10},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Rope", HP=2, MP=20, Type = ItemTypeEnum.Armor, Lvl=1, Atk=12,Def=5, Spd=6, Luk=2,SpecialAbility="null", EquippableLocation="Body", Location=ItemLocationEnum.Necklass,EquippedBy="Empty", ImgSource="ClothBody.png", Range=0, Damage=0, Value=10},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Shoes", HP=10, MP=5, Type = ItemTypeEnum.Armor, Lvl=2, Atk=9,Def=2, Spd=10, Luk=5,SpecialAbility="null", EquippableLocation="Feet", Location=ItemLocationEnum.Feet,EquippedBy="Empty", ImgSource="ClothFeet.png", Range=0, Damage=0, Value=10},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Staff", HP=0, MP=0, Type = ItemTypeEnum.weapon, Lvl=4, Atk=100,Def=2, Spd=4, Luk=7,SpecialAbility="null", EquippableLocation="PrimaryHand",Location=ItemLocationEnum.PrimaryHand, EquippedBy="Empty", ImgSource="Staff3.png", Range=0, Damage=4, Value=10},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Staff", HP=0, MP=0, Type = ItemTypeEnum.weapon, Lvl=3, Atk=100,Def=2, Spd=4, Luk=7,SpecialAbility="null", EquippableLocation="OffHand",Location=ItemLocationEnum.OffHand, EquippedBy="Empty", ImgSource="Staff2.png", Range=0, Damage=6, Value=10},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Ring", HP=5, MP=15, Type = ItemTypeEnum.Ring, Lvl=4, Atk=98,Def=5, Spd=2, Luk=9,SpecialAbility="null", EquippableLocation="LeftFinger",Location=ItemLocationEnum.LeftFinger, EquippedBy="Empty", ImgSource="Ring.png", Range=0, Damage=3, Value=10},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Ring", HP=5, MP=15, Type = ItemTypeEnum.Ring, Lvl=5, Atk=98,Def=5, Spd=2, Luk=9,SpecialAbility="null", EquippableLocation="RightFinger",Location=ItemLocationEnum.RightFinger, EquippedBy="Empty", ImgSource="Ring.png", Range=0, Damage=2, Value=10}
            };

            foreach (var data in mockItems)
            {
                _itemDataset.Add(data);
            }

            var mockScores = new List<Score>
            {
                new Score { Id = Guid.NewGuid().ToString(), Name = "First Score", ScoreTotal = 12345, GameDate = DateTime.Now, AutoBattle = false,  CharacterAtDeathList = "Some characters!", MonstersKilledList = "Some Monsters!", TurnCount=0, RoundCount=0, MonsterSlainNumber = 0, ExperienceGainedTotal=0, ItemsDroppedList="Some Items!"},
                new Score { Id = Guid.NewGuid().ToString(), Name = "Second Score", ScoreTotal = 12345,  GameDate = DateTime.Now, AutoBattle = false, CharacterAtDeathList = "Some characters!", MonstersKilledList = "Some Monsters!", TurnCount=0, RoundCount=0, MonsterSlainNumber = 0, ExperienceGainedTotal=0, ItemsDroppedList="Some Items!" },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Third Score", ScoreTotal = 12345,  GameDate = DateTime.Now, AutoBattle = false, CharacterAtDeathList = "Some characters!", MonstersKilledList = "Some Monsters!", TurnCount=0, RoundCount=0, MonsterSlainNumber = 0, ExperienceGainedTotal=0, ItemsDroppedList="Some Items!"},
                new Score { Id = Guid.NewGuid().ToString(), Name = "Fourth Score", ScoreTotal = 12345,  GameDate = DateTime.Now, AutoBattle = false, CharacterAtDeathList = "Some characters!", MonstersKilledList = "Some Monsters!", TurnCount=0, RoundCount=0, MonsterSlainNumber = 0, ExperienceGainedTotal=0, ItemsDroppedList="Some Items!" },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Fifth Score", ScoreTotal = 12345,  GameDate = DateTime.Now, AutoBattle = false, CharacterAtDeathList = "Some characters!", MonstersKilledList = "Some Monsters!", TurnCount=0, RoundCount=0, MonsterSlainNumber = 0, ExperienceGainedTotal=0, ItemsDroppedList="Some Items!" },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Sixth Score", ScoreTotal = 12345,  GameDate = DateTime.Now, AutoBattle = false, CharacterAtDeathList = "Some characters!", MonstersKilledList = "Some Monsters!", TurnCount=0, RoundCount=0, MonsterSlainNumber = 0, ExperienceGainedTotal=0, ItemsDroppedList="Some Items!" },
            };

            foreach (var data in mockScores)
            {
                _scoreDataset.Add(data);
            }


            var mockMonsters = new List<Monster>
            {
                new Monster { Id = Guid.NewGuid().ToString(), Name = "First Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=100,Def=0, Spd=1, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="HeronLeft.png", xPosition=2,yPosition=0/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Second Monster", Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=100,Def=0, Spd=2, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="TigerLeft.png",xPosition=4,yPosition=1/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Third Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=100,Def=0, Spd=3, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="WolfLeft.png",xPosition=4,yPosition=2/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Fourth Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=100,Def=0, Spd=4, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="HawkLeft.png",xPosition=4,yPosition=3/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Fifth Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=100,Def=0, Spd=5, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Ghost", ImgSource="SkeletonLeft.png",xPosition=4,yPosition=4/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Sixth Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=100,Def=0, Spd=6, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Ghost", ImgSource="SkeletonLeft2.png",xPosition=4,yPosition=5/*UniqueDrop= new Item()*/},
            };

            foreach (var data in mockMonsters)
            {
                _monsterDataset.Add(data);
            }
            var mockCharacters = new List<Character>
            {
                new Character{ Id = Guid.NewGuid().ToString(), Name = "First Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=200,Def=0, Spd=12, Luk=1, Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=2 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="KnightRight.png",xPosition=0,yPosition=0},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Second Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=200,Def=0, Spd=11, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=2 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="MageRight.png",xPosition=0,yPosition=1},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Third Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=200,Def=0, Spd=10, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=2 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="WarriorRight.png",xPosition=0,yPosition=2},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Fourth Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=200,Def=0, Spd=9, Luk=1,Type="Human", AtkRange=2, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=2 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="ArcherRight.png",xPosition=0,yPosition=3},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Fifth Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=200,Def=0, Spd=8, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=2 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="FighterRight.png",xPosition=0,yPosition=4},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Sixth Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=200,Def=0, Spd=7, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=2 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="ThiefRight.png",xPosition=0,yPosition=5},
            };

            foreach (var data in mockCharacters)
            {
                _characterDataset.Add(data);
            }

        }

        #region Item
        // Item
        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Guid);
            if (oldData == null)
            {
                _itemDataset.Add(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Item(data);
            if (UpdateResult)
            {
                await AddAsync_Item(data);
                return true;
            }

            return false;
        }


        //Add item to databse
        public async Task<bool> AddAsync_Item(Item data)
        {
            _itemDataset.Add(data);

            return await Task.FromResult(true);
        }
        //update item in dabatbase
        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Guid == data.Guid);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        //delete item from database
        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Guid == data.Guid);
            _itemDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        //getItems from databse
        public async Task<Item> GetAsync_Item(string id)
        {
            return await Task.FromResult(_itemDataset.FirstOrDefault(s => s.Guid == id));
        }

        //get all items from database
        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            return await Task.FromResult(_itemDataset);
        }

        #endregion

        #region monster
        //add monster to databse 
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            _monsterDataset.Add(data);

            return await Task.FromResult(true);
        }

        //update monster in database 
        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        //delete monster from database
        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _monsterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        //get monster from database
        public async Task<Monster> GetAsync_Monster(string id)
        {
            return await Task.FromResult(_monsterDataset.FirstOrDefault(s => s.Id == id));
        }

        //get all mosnters from database
        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            return await Task.FromResult(_monsterDataset);
        }

        #endregion

        #region Score
        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            _scoreDataset.Add(data);

            return await Task.FromResult(true);
        }

        //update score in databse 
        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        //delete data in database 
        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _scoreDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        //get all score in database
        public async Task<Score> GetAsync_Score(string id)
        {
            return await Task.FromResult(_scoreDataset.FirstOrDefault(s => s.Id == id));
        }

        //get all score 
        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            return await Task.FromResult(_scoreDataset);
        }

        #endregion


        #region Character
        public async Task<bool> AddAsync_Character(Character data)
        {
            _characterDataset.Add(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync_Character(Character data)
        {
            var myData = _characterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync_Character(Character data)
        {
            var myData = _characterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _characterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Character> GetAsync_Character(string id)
        {
            return await Task.FromResult(_characterDataset.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            return await Task.FromResult(_characterDataset);
        }

        #endregion



    }

}
