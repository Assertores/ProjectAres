using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public static class StringCollection {

        #region Scenes

        public static string S_MAINMENU = "MainMenu";
        public static string S_MAP = "Map";
        public static string S_WINSCREEN = "WinScreen";
        public static string S_CREDTIS = "Credits";
        public static string S_ESCMENU = "EscMenu";
        public static string S_TEAMSELECT = "TeamSelect";
        public static string S_GAMESS = "GameSplashScreen";
        public static string S_GASS = "GASplashScreen";
        public static string S_TSS = "TeamSplashScreen";

        #endregion
        #region Tags

        public static string T_PLAYER = "Player";
        public static string T_MAINCAM = "MainCamera";
        public static string T_LEVEL = "Level";
        public static string T_PROJECTILES = "Projectiles";
        public static string T_BIN = "Bin";
        public static string T_CAMBB = "CameraBoundingBox";

        #endregion
        #region Layers

        public static string L_BACKGROUND = "Background";
        public static string L_ANIMATED = "Animated";
        public static string L_STAGE = "Stage";
        public static string L_PROPS = "Props";
        public static string L_PLAYER = "Player";
        public static string L_WEAPON = "Weapon";
        public static string L_VFX = "VFX";
        public static string L_GUI = "GUI";
        public static string L_FORGROUND = "Forground";
        public static string L_LEVEL = "Level";

        #endregion
        #region Paths

        public static string P_DATAPATH = System.IO.Path.Combine(Application.dataPath, "DATA") + System.IO.Path.DirectorySeparatorChar;
        public static string P_MAPPARH = System.IO.Path.Combine(Application.dataPath, "MAP") + System.IO.Path.DirectorySeparatorChar;

        #endregion
        #region Animations

        public static string A_FLYING = "00_Fliegen";
        public static string A_IDLE = "01_Idle";
        public static string A_IDLEAIR = "02_Idle_Luft";
        public static string A_IMPACT = "03_Aufprall";
        public static string A_HIT = "04_Treffer";
        public static string A_DIE = "05_Sterben";
        public static string A_RESPAWN = "06_Respawn";
        public static string A_WIN = "07_Win";
        public static string A_LOSE = "08_Lose";
        public static string A_AIM = "09_Zielen";
        public static string A_CHARGING = "10_Überhitzen";
        public static string A_SHOOTROCKET = "11_RaketeSchießen";
        public static string AR_CHARGING = "Rocket_Charge";
        public static string AR_IDLE = "Rocket_Idle";
        public static string AR_SHOOT = "Rocket_Shoot";
        public static string AR_CHANGE = "Rocket_Weapon_Change";
        public static string AS_IDLE = "SMG_Idle";
        public static string AS_SHOOT = "SMG_Shoot";
        public static string AS_CHANGE = "SMG_Weapon_Change";

        #endregion
        #region ModiNames

        public static string M_FFA = "Free for All";
        public static string M_TDM = "Team Death Match";
        public static string M_CTF = "Capture the Flag";
        public static string M_KOTH = "King of the Hill";
        public static string M_COOPEDIT = "Coop Edit";
        public static string M_LMS = "Last man standing";
        public static string M_BR = "Battle Royal";
        public static string M_BASKETBALL = "Basketball";
        public static string M_FOOTBALL = "Football";
        public static string M_PAINT = "Paint";
        public static string M_JUG = "Juggernaut";
        public static string M_SZ = "SaveZone";

        #endregion
    }
}