using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class WS_FFA_Casual : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject _pillarRef;
        [SerializeField] Transform _rightMostPlayer;
        [SerializeField] Transform _leftMostPlayer;

        [Header("Balancing")]
        [SerializeField] float _pillarSpeed = 1;
        [SerializeField] float _killHight = 1;

        List<GameObject> _pillar = new List<GameObject>();
        float _startTime;

        void Start() {
            for (int i = 0; i < Player._references.Count; i++) {
                Player._references[i].transform.position = Vector3.Lerp(_leftMostPlayer.position, _rightMostPlayer.position,
                                                                        Player._references.Count == 1 ? 0.5f : i /(Player._references.Count-1));
                _pillar.Add(Instantiate(_pillarRef, Player._references[i].transform.position, Player._references[i].transform.rotation));
            }
            _startTime = Time.timeSinceLevelLoad;
        }

        // Update is called once per frame
        void Update() {
            for(int i = 0; i < Player._references.Count; i++) {
                if(1/_pillarSpeed * Player._references[i]._stuts.Kills < Time.timeSinceLevelLoad - _startTime) {
                    _pillar[i].transform.position += new Vector3(0,_pillarSpeed,0);

                    Player._references[i].transform.position = _pillar[i].transform.position;
                }
            }
        }
    }
}