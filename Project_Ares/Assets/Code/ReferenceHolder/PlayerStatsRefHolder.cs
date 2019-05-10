using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectAres
{   [System.Serializable]
    public class PlayerStatsRefHolder : MonoBehaviour
    {
        public GameObject m_parentObject;
        public TextMeshProUGUI m_kills;
        public TextMeshProUGUI m_assists;
        public TextMeshProUGUI m_deaths;
        public TextMeshProUGUI m_damageDealt;
        public TextMeshProUGUI m_damageTaken;
    }
}
