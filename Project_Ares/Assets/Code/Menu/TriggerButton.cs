using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PPBC {
    public class TriggerButton : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] UnityEvent m_onEvent;
        [SerializeField] UnityEvent m_offEvent;

        [Header("Balancing")]
        [Tooltip("true = triggers as soon as the trigger area isn't empty, false = triggers only if all players are within the trigger area")]
        [SerializeField] bool m_singelTarget;

        int m_count = 0;

        #endregion

        private void OnTriggerEnter2D(Collider2D collision) {
            Player p = collision.GetComponent<Player>();
            if (!p) {
                return;
            }

            m_count++;

            if (m_singelTarget ? m_count == 1 : m_count == Player.s_references.Count) {
                DataHolder.s_hoPlayer = p;
                m_onEvent?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            Player p = collision.GetComponent<Player>();
            if (!p) {
                return;
            }

            if (m_singelTarget ? m_count == 1 : m_count == Player.s_references.Count) {
                DataHolder.s_hoPlayer = p;
                m_offEvent?.Invoke();
            }

            m_count--;
            if (m_count < 0)
                m_count = 0;
            
        }
    }
}