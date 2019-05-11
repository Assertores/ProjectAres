using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectAres {
    public class BracketRefHolder : MonoBehaviour {

        public TextMeshProUGUI m_p1Name;
        public TextMeshProUGUI m_p1Alias;
        public Text m_p1placment;
        public TextMeshProUGUI m_p2Name;
        public TextMeshProUGUI m_p2Alias;
        public Text m_p2placment;
        public TextMeshProUGUI m_p3Name;
        public TextMeshProUGUI m_p3Alias;
        public Text m_p3placment;
        public TextMeshProUGUI m_p4Name;
        public TextMeshProUGUI m_p4Alias;
        public Text m_p4placment;

        public int id;

        public void Submit() {
            Manager.s_reference.FinishGame(id, int.Parse(m_p1placment.text), int.Parse(m_p2placment.text), int.Parse(m_p3placment.text), int.Parse(m_p4placment.text));
        }
    }
}