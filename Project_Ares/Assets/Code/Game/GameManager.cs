using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class GameManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject _gmObject;
        [SerializeField] GameObject _playerRev;

        IGameMode _gameMode;

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
            _gameMode = _gmObject.GetComponent<IGameMode>();//Interface werden nicht im inspector angezeigt
            _gameMode?.Init();//TODO: wie bekommt er den richtigen GameMode aus dem Menü

            if(Player.s_references.Count == 0) {
                GameObject tmp = Instantiate(_playerRev);
                if (tmp) {
                    GameObject tmpControle = new GameObject("Controler");
                    tmpControle.transform.parent = tmp.transform;

                    IControl reference = tmpControle.AddComponent<KeyboardControle>();//null reference checks
                    tmp.GetComponent<Player>().Init(reference);//null reference checks
                }
            }
        }

        public void Init(IGameMode mode) {
            _gameMode?.Stop();
            _gameMode = mode;
            mode.Init();
        }

        void Update() {

        }

        public void PlayerDied(Player player) {
            _gameMode.PlayerDied(player);
        }
    }
}