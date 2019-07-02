using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class TDMHudRefHolder : Sauerbraten {

        [Header("References")]
        [SerializeField] Collider2D lhs;
        [SerializeField] Collider2D rhs;

        public List<List<Player>> m_teams = new List<List<Player>>();

        private void Awake() {
            for (int i = 0; i < 2; i++) {
                m_teams.Add(new List<Player>());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Player player = collision.GetComponent<Player>();
            if (!player)
                return;

            print("its a player");

            if (lhs.IsTouching(collision)) {
                m_teams[0].Add(player);
                print("vor team 1");
            }else if (rhs.IsTouching(collision)) {
                m_teams[1].Add(player);
                print("vor team 2");
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