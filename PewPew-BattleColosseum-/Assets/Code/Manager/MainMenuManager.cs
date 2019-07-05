using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class MainMenuManager : MonoBehaviour {

        public void NextMap() {
            //DataHolder.s_currentMap = DataHolder.FixedMod(DataHolder.s_currentMap + 1, DataHolder.s_maps.Lenght);
        }

        public void PreviousMap() {
            //DataHolder.s_currentMap = DataHolder.FixedMod(DataHolder.s_currentMap - 1, DataHolder.s_maps.Lenght);
        }

        public void NextModi() {
            DataHolder.s_currentModi = DataHolder.FixedMod(DataHolder.s_currentModi + 1, DataHolder.s_modis.Length);
        }

        public void PreviousModi() {
            DataHolder.s_currentModi = DataHolder.FixedMod(DataHolder.s_currentModi - 1, DataHolder.s_modis.Length);
        }
    }
}