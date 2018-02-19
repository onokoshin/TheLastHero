using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastHero.Models;

namespace TheLastHero.Services
{

    public class MockDataStore : IDataStore
    {

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



        private MockDataStore()
        {
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Name = "Mage Hat", HP=0, MP=5, Type = HEAD, Lvl=3, Atk=0,Def=10, Spd=1, Luk=1,SpecialAbility="null", EquippableLocation="null", EquippedBy="null", ImgSource="ClothHead.png"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Mage Rope", HP=2, MP=20, Type = BODY, Lvl=1, Atk=0,Def=5, Spd=1, Luk=1,SpecialAbility="null", EquippableLocation="null", EquippedBy="null", ImgSource="ClothBody.png"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Mage Shoes", HP=10, MP=5, Type = FEET, Lvl=1, Atk=0,Def=2, Spd=5, Luk=1,SpecialAbility="null", EquippableLocation="null", EquippedBy="null", ImgSource="ClothFeet.png"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Mage Staff", HP=0, MP=0, Type = LEFTHAND, Lvl=1, Atk=100,Def=2, Spd=5, Luk=1,SpecialAbility="null", EquippableLocation="null", EquippedBy="null", ImgSource="Staff3.png"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Mage Ring", HP=0, MP=5, Type = LEFTFINGER, Lvl=1, Atk=2,Def=5, Spd=1, Luk=1,SpecialAbility="null", EquippableLocation="null", EquippedBy="null", ImgSource="Ring.png"}
            };

            foreach (var data in mockItems)
            {
                _itemDataset.Add(data);
            }

            var mockMonsters = new List<Monster>
            {
                new Monster { Id = Guid.NewGuid().ToString(), Name = "First Monster" , Friendly = false, MaxHP = 2000, CurrentHP = 1800, Lvl = 12, Atk=0,Def=10, Spd=1, Luk=1},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Second Monster", Friendly = false, MaxHP = 523, CurrentHP = 234, Lvl = 15, Atk=0,Def=10, Spd=1, Luk=1},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Third Monster" , Friendly = false, MaxHP = 1200, CurrentHP = 800, Lvl = 2, Atk=0,Def=10, Spd=1, Luk=1},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Fourth Monster" , Friendly = false, MaxHP = 900, CurrentHP = 567, Lvl = 6, Atk=0,Def=10, Spd=1, Luk=1},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Fifth Monster" , Friendly = false, MaxHP = 455, CurrentHP = 123, Lvl = 8, Atk=0,Def=10, Spd=1, Luk=1},
                new Monster { Id = Guid.NewGuid().ToString(), Name = "Sixth Monster" , Friendly = false, MaxHP = 233, CurrentHP = 12, Lvl = 11, Atk=0,Def=10, Spd=1, Luk=1},
            };

            foreach (var data in mockMonsters)
            {
                _monsterDataset.Add(data);
            }
            
            var mockScores = new List<Score>
            {
                new Score { Id = Guid.NewGuid().ToString(), Name = "First Score", ScoreTotal = 12345, GameDate = new DateTime(2018, 02, 18), MonsterSlainNumber = 50 },
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
            
            var mockCharacters = new List<Character>
            {
                new Character{ Id = Guid.NewGuid().ToString(), Name = "First Character", MaxHP = 100, CurrentHP = 90, Friendly = true, Lvl = 12, Atk=0,Def=10, Spd=1, Luk=1,},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Second Character", MaxHP = 100, CurrentHP = 90, Friendly = true, Lvl = 12, Atk=0,Def=10, Spd=1, Luk=1},
                new Character{ Id = Guid.NewGuid().ToString(), Name = "Third Character", MaxHP = 100, CurrentHP = 90, Friendly = true, Lvl = 12, Atk=0,Def=10, Spd=1, Luk=1},
            };

            foreach (var data in mockCharacters)
            {
                _characterDataset.Add(data);
            }
    
        }

        // Item
        public async Task<bool> AddAsync_Item(Item data)
        {
            _itemDataset.Add(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _itemDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            return await Task.FromResult(_itemDataset.FirstOrDefault(s => s.Id == id));
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