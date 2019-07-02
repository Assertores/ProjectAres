using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class WriteStatsToCSV : Sauerbraten, IScriptQueueItem {

        #region Variables

        [SerializeField] string m_TrackingPath = "Tracking.csv";

        #endregion
        #region MonoBehaviour

        private void Start() {
            EndScreenManager.s_ref?.AddItem(this, 0);
        }

        #endregion
        #region IScriptQueueItem

        public bool DoTick() {
            return true;
        }

        public bool FirstTick() {

            Directory.CreateDirectory(StringCollection.DATAPATH);
            foreach (var it in Player.s_references) {
                File.AppendAllText(StringCollection.DATAPATH + m_TrackingPath, it.m_stats.ToString() + ";;" + it.m_stats.m_spawnTimeSinceLevelLoad + ";" + it.m_stats.m_timeInLobby + ";" + it.m_stats.m_sMGTime + ";" + it.m_stats.m_rocketTime + ";" + it.m_stats.m_weaponSwitchCount + ";" + DataHolder.s_characterDatas[it.m_currentChar].m_name + ";" + it.m_stats.m_deathBySuicide + ";" + it.m_stats.m_firstNameChange + ";" + it.m_stats.m_firstCaracterChange + ";" + it.m_stats.m_firstWeaponChange + ";" + it.m_stats.m_firstShot + System.Environment.NewLine);
            }

            File.AppendAllText(StringCollection.DATAPATH + m_TrackingPath, "!=====" + System.Environment.NewLine);

            return true;
        }

        #endregion
    }
}