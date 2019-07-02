using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class PillarRefHolder : Sauerbraten {
        public TextMeshProUGUI m_screen;
        public TextMeshProUGUI m_playerName;
        public TextMeshProUGUI m_pillarField;
        public TextMeshProUGUI m_changePcText;
        public Image m_pillarGradient;

        [HideInInspector]
        public bool finished = false;
    }
}