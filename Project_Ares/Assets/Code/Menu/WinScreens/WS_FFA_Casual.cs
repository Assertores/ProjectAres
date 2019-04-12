using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class WS_FFA_Casual : WinScreen {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject _pillarRef;
        [SerializeField] Transform _rightMostPlayer;
        [SerializeField] Transform _leftMostPlayer;

        [Header("Balancing")]
        [SerializeField] float _pillarSpeed = 1;
        [SerializeField] float _killHight = 1;

        List<GameObject> _pillar = new List<GameObject>();
        float _startTime;

        #endregion
        #region WinScreen
        #region MonoBehaviour

        void Start() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].Invincible(true);

                if (!Player.s_references[i].m_alive) {
                    Player.s_references[i].Respawn(transform.position);
                }
                Player.s_references[i].m_rb.velocity = Vector2.zero;

                Player.s_references[i].transform.position = Vector3.Lerp(_leftMostPlayer.position, _rightMostPlayer.position, ((float)i+1) /(Player.s_references.Count+1));
                Player.s_references[i].InControle(false);
                _pillar.Add(Instantiate(_pillarRef, Player.s_references[i].transform.position, Player.s_references[i].transform.rotation));
            }
            _startTime = Time.timeSinceLevelLoad;
        }

        // Update is called once per frame
        void Update() {
            for(int i = 0; i < Player.s_references.Count; i++) {
                if(1/_pillarSpeed * Player.s_references[i].m_stats.m_kills > Time.timeSinceLevelLoad - _startTime) {
                    _pillar[i].transform.position += new Vector3(0,_pillarSpeed * Time.deltaTime,0);

                    Player.s_references[i].transform.position = _pillar[i].transform.position;
                } else {
                    Player.s_references[i].InControle(true);
                }
            }
        }

        #endregion
        #endregion
    }
}