using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {

    public enum e_gameMode {
        FFA_CASUAL,
        FAIR_TOURNAMENT,
        COOP_EDIT,
        TDM_TOURNAMENT
    }

    public class DataHolder : MonoBehaviour {

        static bool isInit = false;

        public static bool[] s_players = new bool[5];

        //public static string s_gameMode;

        public static List<string> s_playerNames { get; private set; } = new List<string>();
        static System.Random s_ranNameGen = new System.Random(0);//fixed seed to get the same result every time;

        public static List<CharacterData> s_characterDatas = new List<CharacterData>();

        public static MapDATA[] s_maps = null;
        public static int s_map = 0;
        public static e_gameMode s_gameMode = e_gameMode.COOP_EDIT;
        public static string s_level;

        //===== ===== Fair_Tournament ===== =====

        public static bool s_winnerPC = true;
        /// <summary>
        /// wird am ende des gamemodes schon hoch gezählt
        /// </summary>
        public static bool s_firstMatch = true;

        //===== ===== Maps Common Objects ===== =====

        public static Vector2[] s_commonSize;
        public static Sprite[] s_commonBackground;
        public static Color[] s_commonColors;
        public static AudioClip[] s_commonMusic;

        public static d_prop[] s_commonProps;
        public static Sprite[] s_commonStage;
        public static Sprite[] s_commonForground;
        public static GameObject s_commonLaserBariar;

        #region Variables

        [SerializeField] e_gameMode m_standardMode;
        [SerializeField] int m_mapIndex;
        [SerializeField] string[] m_names;
        [SerializeField] CharacterData[] m_characters;
        [SerializeField] MapDATA[] m_maps;

        [SerializeField] Vector2[] m_size;
        [SerializeField] Sprite[] m_background;
        [SerializeField] Color[] m_colors;
        [SerializeField] AudioClip[] m_music;

        [SerializeField] d_prop[] m_props;
        [SerializeField] Sprite[] m_stage;
        [SerializeField] Sprite[] m_forground;
        [SerializeField] GameObject m_laserBariar;

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
            s_maps = m_maps;

            s_commonBackground = m_background;
            s_commonColors = m_colors;
            s_commonForground = m_forground;
            s_commonLaserBariar = m_laserBariar;
            s_commonMusic = m_music;
            s_commonProps = m_props;
            s_commonSize = m_size;
            s_commonStage = m_stage;

            s_level = StringCollection.INGAME;
            s_gameMode = m_standardMode;
            s_map = m_mapIndex;

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