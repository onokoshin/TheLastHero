using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastHero.Models;

namespace TheLastHero.Services
{

    public class MockDataStore : IDataStore
    {

        private List<Item> _itemDataset = new List<Item>();
        private List<Monster> _monsterDataset = new List<Monster>();
        private List<Score> _scoreDataset = new List<Score>();
        private List<Character> _characterDataset = new List<Character>();

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

        private MockDataStore()
        {
            var mockItems = new List<Item>
            {
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Hat", HP=8, MP=5, Type = "Armor", Lvl=3, Atk=10,Def=10, Spd=12, Luk=1,SpecialAbility="null", EquippableLocation="Head", Location=ItemLocationEnum.Head, EquippedBy="Empty", ImgSource="ClothHead.png"},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Rope", HP=2, MP=20, Type = "Armor", Lvl=1, Atk=12,Def=5, Spd=6, Luk=2,SpecialAbility="null", EquippableLocation="Body", Location=ItemLocationEnum.Necklass,EquippedBy="Empty", ImgSource="ClothBody.png"},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Shoes", HP=10, MP=5, Type = "Armor", Lvl=2, Atk=9,Def=2, Spd=10, Luk=5,SpecialAbility="null", EquippableLocation="Feet", Location=ItemLocationEnum.Feet,EquippedBy="Empty", ImgSource="ClothFeet.png"},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Staff", HP=0, MP=0, Type = "Weapon", Lvl=4, Atk=100,Def=2, Spd=4, Luk=7,SpecialAbility="null", EquippableLocation="PrimaryHand",Location=ItemLocationEnum.PrimaryHand, EquippedBy="Empty", ImgSource="Staff3.png"},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Staff", HP=0, MP=0, Type = "Weapon", Lvl=3, Atk=100,Def=2, Spd=4, Luk=7,SpecialAbility="null", EquippableLocation="OffHand",Location=ItemLocationEnum.OffHand, EquippedBy="Empty", ImgSource="Staff2.png"},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Ring", HP=5, MP=15, Type = "Ring", Lvl=4, Atk=98,Def=5, Spd=2, Luk=9,SpecialAbility="null", EquippableLocation="LeftFinger",Location=ItemLocationEnum.LeftFinger, EquippedBy="Empty", ImgSource="Ring.png"},
                new Item { Guid = Guid.NewGuid().ToString(), Name = "Mage Ring", HP=5, MP=15, Type = "Ring", Lvl=5, Atk=98,Def=5, Spd=2, Luk=9,SpecialAbility="null", EquippableLocation="RightFinger",Location=ItemLocationEnum.RightFinger, EquippedBy="Empty", ImgSource="Ring.png"}
            };

            foreach (var data in mockItems)
            {
                _itemDataset.Add(data);
            }

            var mockScores = new List<Score>
            {
                new Score { Id = Guid.NewGuid().ToString(), Name = "First Score", ScoreTotal = 12345, GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50},
                new Score { Id = Guid.NewGuid().ToString(), Name = "Second Score", ScoreTotal = 12345,  GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50 },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Third Score", ScoreTotal = 12345,  GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50 },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Fourth Score", ScoreTotal = 12345,  GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50 },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Fifth Score", ScoreTotal = 12345,  GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50 },
                new Score { Id = Guid.NewGuid().ToString(), Name = "Sixth Score", ScoreTotal = 12345,  GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50 },
            };

            foreach (var data in mockScores)
            {
                _scoreDataset.Add(data);
            }


            var mockMonsters = new List<Monster>
            {
                new Monster { Id = Guid.NewGuid().ToString(), Name = "First Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=999,Def=0, Spd=5, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="HeronLeft.png", xPosition=2,yPosition=0/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Second Monster", Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=999,Def=0, Spd=5, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="TigerLeft.png",xPosition=4,yPosition=1/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Third Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=999,Def=0, Spd=5, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="WolfLeft.png",xPosition=4,yPosition=2/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Fourth Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=999,Def=0, Spd=1, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Beast", ImgSource="HawkLeft.png",xPosition=4,yPosition=3/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Fifth Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=999,Def=0, Spd=1, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Ghost", ImgSource="SkeletonLeft.png",xPosition=4,yPosition=4/*UniqueDrop= new Item()*/},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Sixth Monster" , Friendly = false, MaxHP = 100, CurrentHP = 100, Lvl = 1, Atk=999,Def=0, Spd=5, Luk=1, AtkRange=1, CurrentMP=100, Drop=false, LiveStatus=true, MaxMP=100, MoveRange=1, Type="Ghost", ImgSource="SkeletonLeft2.png",xPosition=4,yPosition=5/*UniqueDrop= new Item()*/},
            };

            foreach (var data in mockMonsters)
            {
                _monsterDataset.Add(data);
            }
            var mockCharacters = new List<Character>
            {
                new Character{ Id = Guid.NewGuid().ToString(), Name = "First Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=999,Def=0, Spd=2, Luk=1, Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=3 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="KnightRight.png",xPosition=0,yPosition=0},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Second Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=999,Def=0, Spd=2, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=3 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="MageRight.png",xPosition=0,yPosition=1},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Third Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=999,Def=0, Spd=2, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=3 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="WarriorRight.png",xPosition=0,yPosition=2},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Fourth Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=999,Def=0, Spd=2, Luk=1,Type="Human", AtkRange=2, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=3 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="ArcherRight.png",xPosition=0,yPosition=3},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Fifth Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=999,Def=0, Spd=2, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=3 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="FighterRight.png",xPosition=0,yPosition=4},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Sixth Character", MaxHP = 100, CurrentHP = 100, Friendly = true, Lvl = 1, Atk=999,Def=0, Spd=2, Luk=1,Type="Human", AtkRange=1, CurrentExp=0, CurrentMP=100, IsCapLevel=false, LiveStatus= true, MaxMP=100, MoveRange=3 , NextLevelExp=100, EquippedItem=new Dictionary<ItemLocationEnum, Item>(),ImgSource="ThiefRight.png",xPosition=0,yPosition=5},
            };

            foreach (var data in mockCharacters)
            {
                _characterDataset.Add(data);
            }

        }

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
        public async Task<bool> AddAsync_Item(Item data)
        {
            _itemDataset.Add(data);

            return await Task.FromResult(true);
        }

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

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Guid == data.Guid);
            _itemDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            return await Task.FromResult(_itemDataset.FirstOrDefault(s => s.Guid == id));
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            return await Task.FromResult(_itemDataset);
        }


        public async Task<bool> AddAsync_Monster(Monster data)
        {
            _monsterDataset.Add(data);

            return await Task.FromResult(true);
        }

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

        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _monsterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Monster> GetAsync_Monster(string id)
        {
            return await Task.FromResult(_monsterDataset.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            return await Task.FromResult(_monsterDataset);
        }


        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            _scoreDataset.Add(data);

            return await Task.FromResult(true);
        }

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

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _scoreDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            return await Task.FromResult(_scoreDataset.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            return await Task.FromResult(_scoreDataset);
        }


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
