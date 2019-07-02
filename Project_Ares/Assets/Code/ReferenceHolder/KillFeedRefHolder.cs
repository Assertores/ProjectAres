using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class KillFeedRefHolder : Sauerbraten {
        public TextMeshProUGUI m_killerName;
        public Image m_killerIcon;
        public Image m_weaponIcon;
        public Image m_victimIcon;
        public TextMeshProUGUI m_victimName;
    }
}