﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [System.Flags]
    public enum e_mileStones { MS1, MS2, MS95, MSXP };//TODO: fill in milestones

    public class DataHolder : MonoBehaviour {
        
        public static bool isInit { get; private set; } = false;
        [HideInInspector]public static bool[] s_players = new bool[5];

        public static Color[] s_playerColors;
        public static Color[] s_teamColors;

        public static IGameMode[] s_modis;
        public static int s_currentModi = 0;

        //public static MapData[] s_maps;
        public static int s_currentMap = 0;

        public static GameObject[] s_characters;

        #region Variables

        public Color[] m_playerColors;
        public Color[] m_teamColors;

        public GameObject[] m_modis;

        //public MapData[] m_maps;

        public GameObject[] m_characters;

        #endregion

        private void Awake() {
            if (isInit) {
                Destroy(this);
                return;
            }
            
            s_playerColors = m_playerColors;
            s_teamColors = m_teamColors;

            IGameMode mode;
            for (int i = 0; i < m_modis.Length; i++) {
                mode = m_modis[i].GetComponent<IGameMode>();
                if(mode != null) {
                    s_modis[i] = mode;
                }
            }

            //TODO: map.shalow load;

            List<GameObject> tmp = new List<GameObject>();
            foreach (var it in m_characters) {
                if (it.GetComponent<ModelRefHolder>())
                    tmp.Add(it);
            }
            s_characters = tmp.ToArray();

            isInit = true;
            Destroy(this);
        }
    }
}