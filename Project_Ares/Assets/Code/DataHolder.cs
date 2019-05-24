using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {

    public enum e_gameMode {
        FFA_CASUAL,
        FAIR_TOURNAMENT
    }

    public class DataHolder : MonoBehaviour {

        static bool isInit = false;

        public static bool[] s_players = new bool[5];

        //public static string s_gameMode;

        public static List<string> s_playerNames { get; private set; } = new List<string>();
        static System.Random s_ranNameGen = new System.Random(0);//fixed seed to get the same result every time;

        public static List<CharacterData> s_characterDatas = new List<CharacterData>();

        public static List<MapDATA> s_maps = new List<MapDATA>();
        public static int s_map = 0;
        public static e_gameMode s_gameMode = e_gameMode.FFA_CASUAL;
        public static string s_level;

        //===== ===== Fair_Tournament ===== =====

        public static bool s_winnerPC = true;
        /// <summary>
        /// wird am ende des gamemodes schon hoch gezählt
        /// </summary>
        public static bool s_firstMatch = true;

        #region Variables

        [SerializeField] string[] m_names;
        [SerializeField] CharacterData[] m_characters;
        [SerializeField] MapDATA[] m_maps;

        #endregion
        #region MonoBehaviour


        private void Awake() {
            if (isInit) {
                Destroy(this);
                return;
            }

            if (m_names != null) {
                foreach (var it in m_names) {
                    s_playerNames.Add(it);
                }
            }
            if(m_characters != null) {
                foreach (var it in m_characters) {
                    s_characterDatas.Add(it);
                }
            }
            if (m_maps != null) {
                foreach(var it in m_maps) {
                    s_maps.Add(it);
                }
            }

            s_level = StringCollection.COLOSSEUM;

            isInit = true;
            Destroy(this);
        }

        #endregion
        #region Helper Funktions

        public static string GetPlayerName(int index) {
            if (index < 0) {
                print("nameindex to low");
                return "";
            }

            if (index < s_playerNames.Count) {
                return s_playerNames[index];
            }

            int value;
            string tmp = "";
            for (int i = s_playerNames.Count; i <= index; i++) {
                value = s_ranNameGen.Next();
                tmp = string.Format("{0}{1}{2}", (char)(value % 25 + 65), (char)((value / 100) % 25 + 65), (char)((value / 10000) % 25 + 65));
                s_playerNames.Add(tmp);
            }
            return tmp;
        }

        #endregion
    }
}