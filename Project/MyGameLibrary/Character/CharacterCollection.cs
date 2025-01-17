﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGameLibrary.Character;
using MyGameLibrary.Shop;

namespace MyGameLibrary.Character
{
    public static class CharacterCollection
    {
        public static Dictionary<CharacterID, Character> CharacterDictionary;

        private static Dictionary<Items, int> s_dictWendyItemResponse;
        private static Dictionary<Items, int> s_dictTonyItemResponse;
        private static Dictionary<Items, int> s_dictRonaldItemResponse;
        private static Dictionary<Items, int> s_dictGreenItemResponse;
        private static Dictionary<Items, int> s_dictBKItemResponse;

        public static Character Wendy;
        public static Character Tony;
        public static Character Ronald;
        public static Character Green;
        public static Character BurgerKing;

        static CharacterCollection() {
            DeclareDict();
            string dateLocations = "\\data\\story\\";
            string promLocations = "\\data\\story\\";
            Wendy = new Character("Wendy", ID: 1,LScore: 40, HappyResponse: "...", UnhappyResponse: "...", DeathResponse: "You fool... a frozen steak, agh, never I... I... feel weak... this may be the end.",
                DateLocation: dateLocations, DateName: "Wendy_Date.txt", PromLocation: promLocations, PromName: "Wendy_Prom.txt", s_dictWendyItemResponse);
            Tony = new Character("Tony", ID: 2, LScore: 50, HappyResponse: "Whoa dude! Thanks so much! This is really cool!", UnhappyResponse: "Oh, uh, thanks? ", DeathResponse: "What the heck man! Tigers can't have tiger lily! We die! AGh...",
                DateLocation: dateLocations, DateName: "Tony_Date.txt", PromLocation: promLocations, PromName: "Tony_Prom.txt", s_dictTonyItemResponse);
            Ronald = new Character("Ronald", ID: 3, LScore: 50, HappyResponse: "Thanks!!!", UnhappyResponse: "This is SO LAME AND BORING!!!!", DeathResponse: "Hah! What a fun gun. Plow plow... a-ah it's not a toy I... I don't feel well.",
                DateLocation: dateLocations, DateName: "Ronald_Date.txt", PromLocation: promLocations, PromName: "Ronald_Prom.txt", s_dictRonaldItemResponse);
            Green = new Character("Green", ID: 4, LScore: 50, HappyResponse: "Oh! Thank you! You didn’t have to do this for me!! That’s so sweet…", UnhappyResponse: " Oh. Well, thanks for the thought!", DeathResponse: "N-no! I'm made out of chocolate... I'm, I'm melting...",
                DateLocation: dateLocations, DateName: "Green_Date.txt", PromLocation: promLocations, PromName: "Green_Prom.txt", s_dictGreenItemResponse);
            BurgerKing = new Character("Burger King", ID: 5, LScore: 50, HappyResponse: "Why thank you!", UnhappyResponse: "You think THIS is a gift befitting a king? Who do you think you are?", DeathResponse: "L-let them eat cake.",
                DateLocation: dateLocations, DateName: "Burger_Date.txt", PromLocation: promLocations, PromName: "Burger_Prom.txt", s_dictBKItemResponse);

            CharacterDictionary = new Dictionary<CharacterID, Character> {
                {(CharacterID) 1, Wendy},
                {(CharacterID) 2, Tony},
                {(CharacterID) 3, Ronald},
                {(CharacterID) 4, Green},
                {(CharacterID) 5, BurgerKing}
            };

        }

        //Declare love values for each gift item, dict1-dict5 corresponds to character ID 1-5
        private static void DeclareDict() {
            s_dictWendyItemResponse = new Dictionary<Items, int> {
                {(Items) 1, -10},
                {(Items) 2, -10},
                {(Items) 3, -10},
                {(Items) 4, -10},
                {(Items) 5, -100},
                {(Items) 6, -10},
                {(Items) 7, -10},
                {(Items) 8, -10},
                {(Items) 9, -10},
                {(Items) 10, -10},
                {(Items) 11, -10},
                {(Items) 12, -10},
                {(Items) 13, -10},
                {(Items) 14, 100},
                {(Items) 15, -10},
                {(Items) 16, -10}
            };
            s_dictTonyItemResponse = new Dictionary<Items, int> {
                {(Items) 1, 5},
                {(Items) 2, 15},
                {(Items) 3, -5},
                {(Items) 4, -100},
                {(Items) 5, 10},
                {(Items) 6, 5},
                {(Items) 7, -5},
                {(Items) 8, -10},
                {(Items) 9, -5},
                {(Items) 10, -10},
                {(Items) 11, -10},
                {(Items) 12, -5},
                {(Items) 13, 5},
                {(Items) 14, -20},
                {(Items) 15, 5},
                {(Items) 16, 50}
            };
            s_dictRonaldItemResponse = new Dictionary<Items, int> {
                {(Items) 1, 10},
                {(Items) 2, 5},
                {(Items) 3, 10},
                {(Items) 4, -5},
                {(Items) 5, 5},
                {(Items) 6, 5},
                {(Items) 7, -5},
                {(Items) 8, -10},
                {(Items) 9, 10},
                {(Items) 10, -100},
                {(Items) 11, 10},
                {(Items) 12, 75},
                {(Items) 13, -10},
                {(Items) 14, -5},
                {(Items) 15, 30},
                {(Items) 16, -10}
            };
            s_dictGreenItemResponse = new Dictionary<Items, int> {
                {(Items) 1, 5},
                {(Items) 2, 5},
                {(Items) 3, -10},
                {(Items) 4, 5},
                {(Items) 5, -10},
                {(Items) 6, -100},
                {(Items) 7, -20},
                {(Items) 8, 100},
                {(Items) 9, -5},
                {(Items) 10, -10},
                {(Items) 11, -10},
                {(Items) 12, -5},
                {(Items) 13, -10},
                {(Items) 14, -10},
                {(Items) 15, 5},
                {(Items) 16, 10}
            };
            s_dictBKItemResponse = new Dictionary<Items, int> {
                {(Items) 1, -10},
                {(Items) 2, -5},
                {(Items) 3, -10},
                {(Items) 4, 5},
                {(Items) 5, 10},
                {(Items) 6, -10},
                {(Items) 7, 10},
                {(Items) 8, 10},
                {(Items) 9, -10},
                {(Items) 10, -10},
                {(Items) 11, -100},
                {(Items) 12, -5},
                {(Items) 13, -10},
                {(Items) 14, -10},
                {(Items) 15, 100},
                {(Items) 16, 5}
            };
        }
    }
}
