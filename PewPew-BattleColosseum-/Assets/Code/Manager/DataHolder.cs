using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class DataHolder : MonoBehaviour {

        [System.Flags]
        enum e_mileStones { MS1, MS2, MS95, MSXP};//TODO: fill in milestones

        static bool isInit = false;

        public static bool[] s_players = new bool[5];
        public static Color[] s_playerColors;
        public static Color[] s_teamColors;

        //public static Dictionary<string, IGameMode> s_modis = new Dictionary<string, IGameMode>();
        public static string s_currentModi;

        //public static Dictionary<string, MapData> s_maps = new Dictionary<string, MapData>();
        public static string s_currentMap;

        public static GameObject[] s_characters;

        public static Player s_hoPlayer;//handing over Player

        #region Variables

        public bool[] m_players;
        public Color[] m_playerColors;
        public Color[] m_teamColors;

        public GameObject[] m_modis;
        public string m_startModi;

        //public MapData[] m_maps;
        public string m_startMap;

        public GameObject[] m_characters;

        #endregion

        private void Awake() {
            if (isInit) {
                Destroy(this);
                return;
            }

            s_players = m_players;
            s_playerColors = m_playerColors;
            s_teamColors = m_teamColors;
            //TODO: getComponent IGameMode; m_modis.name as key for s_modis
            s_currentModi = m_startModi != "" ? m_startModi : "" /*first modi*/;
            //TODO: map.shalow load; m_maps.name as key for s_maps
            s_currentMap = m_startMap != "" ? m_startMap : "" /*first map*/;
            s_characters = m_characters;

            isInit = true;
            Destroy(this);
        }
    }
}