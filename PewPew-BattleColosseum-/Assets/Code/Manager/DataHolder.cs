using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PPBC {
    [System.Flags]
    public enum e_mileStones { MS1, MS2, MS95, MSXP };//TODO: fill in milestones

    public class DataHolder : MonoBehaviour {
        
        public static bool s_isInit { get; private set; } = false;
        public static bool[] s_players = new bool[5];

        public static Color[] s_playerColors;
        public static Color[] s_teamColors;

        public static IGameMode[] s_modis;
        public static int s_currentModi = 0;

        public static List<MapData> s_maps;
        public static int s_currentMap = 0;

        public static GameObject[] s_characters;

        //===== ===== Maps Common Objects ===== =====

        public static Vector2[] s_commonSizes;
        public static BackgroundData[] s_commonBackgrounds;
        public static Color[] s_commonColors;
        public static AudioClip[] s_commonMusics;

        public static PropData[] s_commonProps;
        public static Sprite[] s_commonStages;
        public static Sprite[] s_commonForgrounds;
        public static GameObject s_commonLaserSpawner;

        #region Variables

        public Color[] m_playerColors;
        public Color[] m_teamColors;

        public GameObject[] m_modis;

        public MapData[] m_maps;

        public GameObject[] m_characters;

        [Header("CommonObjects")]
        public Vector2[] m_commonSizes;
        public BackgroundData[] m_commonBackgrounds;
        public Color[] m_commonColors;
        public AudioClip[] m_commonMusics;

        public PropData[] m_commonProps;
        public Sprite[] m_commonStages;
        public Sprite[] m_commonForgrounds;
        public GameObject m_commonLaserSpawner;

        #endregion

        //int enumSize = System.Enum.GetNames(typeof(e_mileStones)).Length;//https://stackoverflow.com/questions/856154/total-number-of-items-defined-in-an-enum
        private void Awake() {
            if (s_isInit) {
                Destroy(this);
                return;
            }
            s_isInit = true;

            s_playerColors = m_playerColors;
            s_teamColors = m_teamColors;

            List<IGameMode> gmList = new List<IGameMode>();
            for (int i = 0; i < m_modis.Length; i++) {
                IGameMode mode = m_modis[i].GetComponent<IGameMode>();
                if(mode != null) {
                    gmList.Add(mode);
                }
            }
            s_modis = gmList.ToArray();
            
            foreach(var it in m_maps) {
                s_maps.Add(it);
            }
            //TODO: map.shalow load;

            List<GameObject> tmp = new List<GameObject>();
            foreach (var it in m_characters) {
                if (it.GetComponent<ModelRefHolder>())
                    tmp.Add(it);
            }
            s_characters = tmp.ToArray();

            s_commonSizes = m_commonSizes;
            s_commonBackgrounds = m_commonBackgrounds;
            s_commonColors = m_commonColors;
            s_commonMusics = m_commonMusics;
            s_commonProps = m_commonProps;
            s_commonStages = m_commonStages;
            s_commonForgrounds = m_commonForgrounds;
            s_commonLaserSpawner = m_commonLaserSpawner;
            
            Destroy(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FixedMod(int lhs, int rhs) {
            return ((lhs % rhs) + rhs) % rhs;
        }
    }
}