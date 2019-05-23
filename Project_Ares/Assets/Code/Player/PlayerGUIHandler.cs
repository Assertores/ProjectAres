using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PPBC {
    public class PlayerGUIHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] Image m_characterIconRef;
        [SerializeField] TextMeshProUGUI m_characterNameRef;
        [SerializeField] GameObject m_characterSelector;
        [SerializeField] Image m_weaponIconRef;
        [SerializeField] TextMeshProUGUI m_playerNameRef;
        [SerializeField] Image m_healthBar;
        [SerializeField] public PlayerStatsRefHolder m_statRefHolder;     


        [SerializeField] Canvas m_canvas;

        [SerializeField] public TextMeshProUGUI m_debugStats;//eventuell in eine funktion verpacken
        
        

        #endregion

        public void ChangeCharacter(Sprite icon, string name) {
            m_characterIconRef.sprite = icon;
            m_characterNameRef.text = name;
        }

        public void ChangeWeapon(Sprite icon) {
            m_weaponIconRef.sprite = icon;
        }

        public void Reposition(float position) {
            transform.position = new Vector2 (m_canvas.pixelRect.width * position,transform.position.y);
        }

        public void SetName(string name) {
            m_playerNameRef.text = name;
        }

        public void SetCharChangeActive(bool activate) {
            m_characterSelector.SetActive(activate);
        }

        public void SetHealth(float fill) {
            m_healthBar.fillAmount = fill;
        }

        public void WriteStats( d_playerData m_stats) {
             
                m_statRefHolder.m_kills.text = "Kills: " + m_stats.m_kills.ToString();
                m_statRefHolder.m_assists.text = "Assists: " + m_stats.m_assists.ToString();
                m_statRefHolder.m_deaths.text = "Deaths: " + m_stats.m_deaths.ToString();
                m_statRefHolder.m_damageDealt.text = "Damage Dealt: " + Mathf.RoundToInt(m_stats.m_damageDealt).ToString();
                m_statRefHolder.m_damageTaken.text = "Damage Taken: " + Mathf.RoundToInt(m_stats.m_damageTaken).ToString();
                m_statRefHolder.m_parentObject.SetActive(true);
            
        }
        public void HideStats() {
            m_statRefHolder.m_parentObject.SetActive(false);
        }
    }
}