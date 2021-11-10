using System.Collections.Generic;
using Fall2020_CSC403_Project;
using MyGameLibrary.Shop;

namespace MyGameLibrary.Character
{
    public class Character
    {
        public string CharacterName { get; set; }
        public int ID { get; set; }
        private bool _isDead { get; set; }
        public bool IsDead {
            get
            {
                return _isDead;
            }
            set
            {
                _isDead = value;
                if (value)
                {
                    // On character death, uses switch cases to modify the love score of other characters.
                    List<Item> valueChangeList = new List<Item>();
                    switch (this.ID) {
                        case 1:
                            CharacterCollection.CharacterDictionary[(CharacterID)2].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)3].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)4].LoveScore += -100;
                            CharacterCollection.CharacterDictionary[(CharacterID)5].LoveScore += -25;
                            break;
                        case 2:
                            CharacterCollection.CharacterDictionary[(CharacterID)1].LoveScore += 25;
                            CharacterCollection.CharacterDictionary[(CharacterID)3].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)4].LoveScore += -100;
                            CharacterCollection.CharacterDictionary[(CharacterID)5].LoveScore += -25;
                            break;
                        case 3:
                            CharacterCollection.CharacterDictionary[(CharacterID)1].LoveScore += 25;
                            CharacterCollection.CharacterDictionary[(CharacterID)2].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)4].LoveScore += -100;
                            CharacterCollection.CharacterDictionary[(CharacterID)5].LoveScore += 25;
                            break;
                        case 4:
                            CharacterCollection.CharacterDictionary[(CharacterID)1].LoveScore += 25;
                            CharacterCollection.CharacterDictionary[(CharacterID)2].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)3].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)5].LoveScore += -25;
                            break;
                        case 5:
                            CharacterCollection.CharacterDictionary[(CharacterID)5].LoveScore += 25;
                            CharacterCollection.CharacterDictionary[(CharacterID)2].LoveScore += -25;
                            CharacterCollection.CharacterDictionary[(CharacterID)3].LoveScore += 25;
                            CharacterCollection.CharacterDictionary[(CharacterID)4].LoveScore += -100;
                            break;
                    }
                }
            }
        }
        private int _lovescore { get; set; }
        public int LoveScore {
            get {
                return _lovescore;
            }
            set {
                if (IsDead == true) {
                    _lovescore = -1;
                    return;
                }
                switch (value)
                {
                    case int x when x > 100:
                        _lovescore = 100;
                        break;
                    case int x when x < 0:
                        _lovescore = 0;
                        break;
                    default:
                        _lovescore = value;
                        break;
                }
            }
        }

        //Items is ID of item, int is love point value of gift for this character
        public Dictionary<Items, int> ItemScores; 

        public Character(string name, int LScore, Dictionary<Items, int> dict)
        {
            CharacterName = name;
            LoveScore = LScore;
            ItemScores = dict;
            IsDead = false;
        }

        public void LoveUpdate(Items item)
        {
            LoveScore += ItemScores[item];
            if(ItemScores[item] == -100)
            {
                IsDead = true;
            }
        }
    }
}
