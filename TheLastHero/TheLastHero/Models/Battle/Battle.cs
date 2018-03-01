﻿using System.Collections.Generic;

namespace TheLastHero.Models.Battle
{
    public class Battle
    {
        // Every four rounds, game engine will generate a new boss monster
        //const int BOSSROUND = 4;

        public const string HIGHLIGHTGREEN = "HighlightGreen.png";
        public const string HIGHLIGHTRED = "HighlightRed.png";
        public const string HIGHLIGHTGREY = "HighlightGrey.png";
        public const string GRASS = "Grass.png";
        public const string LAVA = "Lava.png";
        public const string SAND = "Sand.png";

        public string title { get; set; }

        // The data structure list contains selected six characters passed from    SelectCharacterPage
        public List<Character> characters;

        // The data structure list contains contains six monsters that will appear in each battle
        // The six monsters will be selected by using generateMonster() function 
        public List<Monster> monsters;

        // List of Items 
        public List<Item> itemPool;

        //use 2d array to instantiate a battlemap 6 x 5
        public string[,] battleMapTop = new string[5, 6];
        public string[,] battleMapBottom = new string[5, 6];
        public string[,] battleMapSelection = new string[5, 6];

        public string cell_00_top { get; set; }
        public string cell_01_top { get; set; }
        public string cell_02_top { get; set; }
        public string cell_03_top { get; set; }
        public string cell_04_top { get; set; }
        public string cell_05_top { get; set; }
        public string cell_10_top { get; set; }
        public string cell_11_top { get; set; }
        public string cell_12_top { get; set; }
        public string cell_13_top { get; set; }
        public string cell_14_top { get; set; }
        public string cell_15_top { get; set; }
        public string cell_20_top { get; set; }
        public string cell_21_top { get; set; }
        public string cell_22_top { get; set; }
        public string cell_23_top { get; set; }
        public string cell_24_top { get; set; }
        public string cell_25_top { get; set; }
        public string cell_30_top { get; set; }
        public string cell_31_top { get; set; }
        public string cell_32_top { get; set; }
        public string cell_33_top { get; set; }
        public string cell_34_top { get; set; }
        public string cell_35_top { get; set; }
        public string cell_40_top { get; set; }
        public string cell_41_top { get; set; }
        public string cell_42_top { get; set; }
        public string cell_43_top { get; set; }
        public string cell_44_top { get; set; }
        public string cell_45_top { get; set; }
        public string cell_00_bottom { get; set; }
        public string cell_01_bottom { get; set; }
        public string cell_02_bottom { get; set; }
        public string cell_03_bottom { get; set; }
        public string cell_04_bottom { get; set; }
        public string cell_05_bottom { get; set; }
        public string cell_10_bottom { get; set; }
        public string cell_11_bottom { get; set; }
        public string cell_12_bottom { get; set; }
        public string cell_13_bottom { get; set; }
        public string cell_14_bottom { get; set; }
        public string cell_15_bottom { get; set; }
        public string cell_20_bottom { get; set; }
        public string cell_21_bottom { get; set; }
        public string cell_22_bottom { get; set; }
        public string cell_23_bottom { get; set; }
        public string cell_24_bottom { get; set; }
        public string cell_25_bottom { get; set; }
        public string cell_30_bottom { get; set; }
        public string cell_31_bottom { get; set; }
        public string cell_32_bottom { get; set; }
        public string cell_33_bottom { get; set; }
        public string cell_34_bottom { get; set; }
        public string cell_35_bottom { get; set; }
        public string cell_40_bottom { get; set; }
        public string cell_41_bottom { get; set; }
        public string cell_42_bottom { get; set; }
        public string cell_43_bottom { get; set; }
        public string cell_44_bottom { get; set; }
        public string cell_45_bottom { get; set; }
        public string cell_00_selection { get; set; }
        public string cell_01_selection { get; set; }
        public string cell_02_selection { get; set; }
        public string cell_03_selection { get; set; }
        public string cell_04_selection { get; set; }
        public string cell_05_selection { get; set; }
        public string cell_10_selection { get; set; }
        public string cell_11_selection { get; set; }
        public string cell_12_selection { get; set; }
        public string cell_13_selection { get; set; }
        public string cell_14_selection { get; set; }
        public string cell_15_selection { get; set; }
        public string cell_20_selection { get; set; }
        public string cell_21_selection { get; set; }
        public string cell_22_selection { get; set; }
        public string cell_23_selection { get; set; }
        public string cell_24_selection { get; set; }
        public string cell_25_selection { get; set; }
        public string cell_30_selection { get; set; }
        public string cell_31_selection { get; set; }
        public string cell_32_selection { get; set; }
        public string cell_33_selection { get; set; }
        public string cell_34_selection { get; set; }
        public string cell_35_selection { get; set; }
        public string cell_40_selection { get; set; }
        public string cell_41_selection { get; set; }
        public string cell_42_selection { get; set; }
        public string cell_43_selection { get; set; }
        public string cell_44_selection { get; set; }
        public string cell_45_selection { get; set; }

        public void SetAllSelection(string img)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    battleMapSelection[i, j] = img;
                }
            }
        }

        public void SetAllBackground(string img)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    battleMapBottom[i, j] = img;
                }
            }
        }

        public void SetAllTop(string img)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    battleMapTop[i, j] = img;
                }
            }
        }

        public void RefreshAllCell()
        {
            cell_00_selection = battleMapSelection[0, 0];
            cell_01_selection = battleMapSelection[0, 1];
            cell_02_selection = battleMapSelection[0, 2];
            cell_03_selection = battleMapSelection[0, 3];
            cell_04_selection = battleMapSelection[0, 4];
            cell_05_selection = battleMapSelection[0, 5];
            cell_10_selection = battleMapSelection[1, 0];
            cell_11_selection = battleMapSelection[1, 1];
            cell_12_selection = battleMapSelection[1, 2];
            cell_13_selection = battleMapSelection[1, 3];
            cell_14_selection = battleMapSelection[1, 4];
            cell_15_selection = battleMapSelection[1, 5];
            cell_20_selection = battleMapSelection[2, 0];
            cell_21_selection = battleMapSelection[2, 1];
            cell_22_selection = battleMapSelection[2, 2];
            cell_23_selection = battleMapSelection[2, 3];
            cell_24_selection = battleMapSelection[2, 4];
            cell_25_selection = battleMapSelection[2, 5];
            cell_30_selection = battleMapSelection[3, 0];
            cell_31_selection = battleMapSelection[3, 1];
            cell_32_selection = battleMapSelection[3, 2];
            cell_33_selection = battleMapSelection[3, 3];
            cell_34_selection = battleMapSelection[3, 4];
            cell_35_selection = battleMapSelection[3, 5];
            cell_40_selection = battleMapSelection[4, 0];
            cell_41_selection = battleMapSelection[4, 1];
            cell_42_selection = battleMapSelection[4, 2];
            cell_43_selection = battleMapSelection[4, 3];
            cell_44_selection = battleMapSelection[4, 4];
            cell_45_selection = battleMapSelection[4, 5];
            cell_00_top = battleMapTop[0, 0];
            cell_01_top = battleMapTop[0, 1];
            cell_02_top = battleMapTop[0, 2];
            cell_03_top = battleMapTop[0, 3];
            cell_04_top = battleMapTop[0, 4];
            cell_05_top = battleMapTop[0, 5];
            cell_10_top = battleMapTop[1, 0];
            cell_11_top = battleMapTop[1, 1];
            cell_12_top = battleMapTop[1, 2];
            cell_13_top = battleMapTop[1, 3];
            cell_14_top = battleMapTop[1, 4];
            cell_15_top = battleMapTop[1, 5];
            cell_20_top = battleMapTop[2, 0];
            cell_21_top = battleMapTop[2, 1];
            cell_22_top = battleMapTop[2, 2];
            cell_23_top = battleMapTop[2, 3];
            cell_24_top = battleMapTop[2, 4];
            cell_25_top = battleMapTop[2, 5];
            cell_30_top = battleMapTop[3, 0];
            cell_31_top = battleMapTop[3, 1];
            cell_32_top = battleMapTop[3, 2];
            cell_33_top = battleMapTop[3, 3];
            cell_34_top = battleMapTop[3, 4];
            cell_35_top = battleMapTop[3, 5];
            cell_40_top = battleMapTop[4, 0];
            cell_41_top = battleMapTop[4, 1];
            cell_42_top = battleMapTop[4, 2];
            cell_43_top = battleMapTop[4, 3];
            cell_44_top = battleMapTop[4, 4];
            cell_45_top = battleMapTop[4, 5];
            cell_00_bottom = battleMapBottom[0, 0];
            cell_01_bottom = battleMapBottom[0, 1];
            cell_02_bottom = battleMapBottom[0, 2];
            cell_03_bottom = battleMapBottom[0, 3];
            cell_04_bottom = battleMapBottom[0, 4];
            cell_05_bottom = battleMapBottom[0, 5];
            cell_10_bottom = battleMapBottom[1, 0];
            cell_11_bottom = battleMapBottom[1, 1];
            cell_12_bottom = battleMapBottom[1, 2];
            cell_13_bottom = battleMapBottom[1, 3];
            cell_14_bottom = battleMapBottom[1, 4];
            cell_15_bottom = battleMapBottom[1, 5];
            cell_20_bottom = battleMapBottom[2, 0];
            cell_21_bottom = battleMapBottom[2, 1];
            cell_22_bottom = battleMapBottom[2, 2];
            cell_23_bottom = battleMapBottom[2, 3];
            cell_24_bottom = battleMapBottom[2, 4];
            cell_25_bottom = battleMapBottom[2, 5];
            cell_30_bottom = battleMapBottom[3, 0];
            cell_31_bottom = battleMapBottom[3, 1];
            cell_32_bottom = battleMapBottom[3, 2];
            cell_33_bottom = battleMapBottom[3, 3];
            cell_34_bottom = battleMapBottom[3, 4];
            cell_35_bottom = battleMapBottom[3, 5];
            cell_40_bottom = battleMapBottom[4, 0];
            cell_41_bottom = battleMapBottom[4, 1];
            cell_42_bottom = battleMapBottom[4, 2];
            cell_43_bottom = battleMapBottom[4, 3];
            cell_44_bottom = battleMapBottom[4, 4];
            cell_45_bottom = battleMapBottom[4, 5];
        }
    }
}