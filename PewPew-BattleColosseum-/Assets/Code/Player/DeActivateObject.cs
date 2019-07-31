using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DeActivateObject : MonoBehaviour {

        TextMeshProUGUI m_text;

        private void Start() {
            m_text = GetComponent<TextMeshProUGUI>();
        }

        public void Activate() {
            m_text.enabled = true;
        }

        public void Deactivate() {
            m_text.enabled = false;
        }
    }
}