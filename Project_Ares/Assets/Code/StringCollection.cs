using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public static class StringCollection {

        #region AXIS

        public static string AXISX = "Horizontal";
        public static string AXISY = "Vertical";
        public static string FIRE = "Fire1";

        #endregion
        #region SCENES

        public static string EXAMPLESZENE = "SampleScene";
        public static string INGAME = "Map";
        public static string MAINMENU = "Menu";
        public static string ENDSCREEN = "EndScreen";
        public static string GAMELOGO = "GameLogo";

        #endregion
        #region TAGS

        public static string PLAYER = "Player";

        #endregion
        #region SortingLayer

        public static string SPACE = "Space";
        public static string BACKGROUND = "Background";
        public static string STAGE = "Stage";
        public static string PROPS = "Props";
        public static string CHARACTERS = "Characters";
        public static string PROJECTILES = "Projectiles";
        public static string WEAPONS = "Weapons";
        public static string FORGROUND = "Forground";
        public static string EFFECTS = "Effects";
        public static string GUI = "GUI";

        #endregion

        public static string DATAPATH = Application.dataPath + "/DATA/";
    }
}