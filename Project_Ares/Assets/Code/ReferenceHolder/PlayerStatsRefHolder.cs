using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC
{   [System.Serializable]
    public class PlayerStatsRefHolder : Sauerbraten
    {
        public GameObject m_parentObject;
        public TextMeshProUGUI m_kills;
        public TextMeshProUGUI m_assists;
        public TextMeshProUGUI m_deaths;
        public TextMeshProUGUI m_damageDealt;
        public TextMeshProUGUI m_damageTaken;
    }
}
