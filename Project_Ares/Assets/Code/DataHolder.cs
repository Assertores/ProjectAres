using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class DataHolder : MonoBehaviour {

        

        public static bool[] s_players = new bool[5];
        public static string s_LevelName;
        //public static string s_gameMode;

        public static List<string> s_playerNames { get; private set; } = new List<string>();
        static System.Random s_ranNameGen = new System.Random(0);//fixed seed to get the same result every time;

        public static List<CharacterData> s_characterDatas = new List<CharacterData>();

        #region Variables

        [SerializeField] string[] m_names;
        [SerializeField] CharacterData[] m_characters;

        #endregion
        #region MonoBehaviour


        private void Awake() {
            if (m_names != null && s_playerNames.Count == 0) {
                for (int i = 0; i < m_names.Length; i++) {
                    s_playerNames.Add(m_names[i]);
                }
                m_names = null;
            }
            if(m_characters != null && s_characterDatas.Count == 0) {
                for (int i = 0; i < m_characters.Length; i++) {
                    s_characterDatas.Add(m_characters[i]);
                }
            }

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