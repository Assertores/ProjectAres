using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class TDMHudRefHolder : MonoBehaviour {

        [Header("References")]
        [SerializeField] Collider2D lhs;
        [SerializeField] Collider2D rhs;

        public List<List<Player>> m_teams = new List<List<Player>>(2);

        private void OnTriggerEnter2D(Collider2D collision) {
            Player player = collision.GetComponent<Player>();
            if (!player)
                return;

            if (lhs.IsTouching(collision)) {
                m_teams[0].Add(player);
            }else if (rhs.IsTouching(collision)) {
                m_teams[1].Add(player);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            Player player = collision.GetComponent<Player>();
            if (!player)
                return;

            foreach(var it in m_teams) {
                if (it.Remove(player))
                    return;
            }
        }
    }
}