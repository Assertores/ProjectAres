using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class CameraControler : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject _cameraRef;
        [SerializeField] GameObject _spectatorRef;

        List<Camera> _cameras = new List<Camera>();

        GameObject _fillerSpectator = null;

        #region Singelton

        public static CameraControler _singelton = null;

        private void Awake() {
            if (_singelton)
                Destroy(this);
            if (!_cameraRef.GetComponent<Camera>())
                Destroy(this);

            _singelton = this;
        }
        private void OnDestroy() {
            if (_singelton == this)
                _singelton = null;
        }

        #endregion

        private void Start() {
            SetCameraSizes();
        }

        public Camera AddCamera() {
            Camera value = Instantiate(_cameraRef, transform).GetComponent<Camera>();
            _cameras.Add(value);
            value.gameObject.name = "Camera " + value.gameObject.GetInstanceID();

            SetCameraSizes();

            return value;
        }

        public void RemoveCamera(Camera camera) {

            bool exists = false;
            foreach(var it in _cameras) {//weil er exists irgendwie ne Prediction braucht und ich keine ahrnung habe was er damit meint
                if(it == camera) {
                    exists = true;
                    break;
                }
            }
            if(!exists)
                return;

            _cameras.Remove(camera);
            Destroy(camera.gameObject);

            SetCameraSizes();
        }

        void SetCameraSizes() {
            //TODO: somewhat broaken
            switch (_cameras.Count) {
            case 1:
                _cameras[0].rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                if (_fillerSpectator) {
                    Destroy(_fillerSpectator);
                    break;
                }
                _cameras[0].rect = new Rect(0, 0, 0.5f, 1);
                _cameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                break;
            //case 3://spectator camera hinzufügen
                //_cameras[0].rect = new Rect(0, 0, 0.5f, 0.5f);
                //_cameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                //_cameras[2].rect = new Rect(0, 0.5f, 1, 0.5f);
                //break;
            case 4:
                _cameras[0].rect = new Rect(0, 0, 0.5f, 0.5f);
                _cameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                _cameras[2].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                _cameras[3].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                break;
            default:
                if (_fillerSpectator) {
                    Destroy(_fillerSpectator);
                    break;
                }
                //if (_spectatorRef) {//kein spectator mehr, feste kammera
                //    _fillerSpectator = Instantiate(_spectatorRef);
                //} else {
                //    _fillerSpectator = new GameObject();
                //    _fillerSpectator.AddComponent<Spectator>();
                //}
                //_fillerSpectator.name = "Spectator";
                break;
            }
        }
    }
}