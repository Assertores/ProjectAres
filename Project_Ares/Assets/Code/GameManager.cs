using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class GameManager : MonoBehaviour {

        #region Singelton

        static GameManager _singelton_ = null;
        public static GameManager _singelton  {
            get {
                if (!_singelton_)
                    _singelton_ = new GameObject {
                        name = "GameManager"
                    }.AddComponent<GameManager>();
                return _singelton_;
                }
            }

        void Awake() {
            if(_singelton_ == null) {
                _singelton_ = this;
            }else if (_singelton_ != this) {
                Destroy(gameObject);
                return;
            }
        }

        #endregion

        void Start() {
            
        }

        void Update() {

        }
    }
}