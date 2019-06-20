using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class CameraController : Sauerbraten {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_cameraRef;
        [SerializeField] GameObject m_spectatorRef;

        List<Camera> m_cameras = new List<Camera>();

        GameObject m_fillerSpectator = null;

        #endregion
        #region MonoBehaviour
        #region Singelton

        static CameraController s_singelton_ = null;
        public static CameraController s_singelton {
            get {
                if (!s_singelton_)
                    s_singelton_ = new GameObject {
                        name = "CameraControler"
                    }.AddComponent<CameraController>();
                return s_singelton_;
            }
        }

        private void Awake() {
            if (s_singelton_)
                Destroy(this);
            if (!m_cameraRef || !m_cameraRef.GetComponent<Camera>())
                Destroy(this);

            s_singelton_ = this;
        }
        private void OnDestroy() {
            if (s_singelton_ == this)
                s_singelton_ = null;
        }

        #endregion

        private void Start() {
            SetCameraSizes();
        }

        #endregion

        public Camera AddCamera() {
            Camera value = Instantiate(m_cameraRef, transform).GetComponent<Camera>();
            m_cameras.Add(value);
            value.gameObject.name = "Camera " + value.gameObject.GetInstanceID();

            SetCameraSizes();

            return value;
        }

        public void RemoveCamera(Camera camera) {

            bool exists = false;
            foreach(var it in m_cameras) {//weil er exists irgendwie ne Prediction braucht und ich keine ahrnung habe was er damit meint
                if(it == camera) {
                    exists = true;
                    break;
                }
            }
            if(!exists)
                return;

            m_cameras.Remove(camera);
            Destroy(camera.gameObject);

            SetCameraSizes();
        }

        void SetCameraSizes() {
            //TODO: somewhat broaken
            switch (m_cameras.Count) {
            case 1:
                m_cameras[0].rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                if (m_fillerSpectator) {
                    Destroy(m_fillerSpectator);
                    break;
                }
                m_cameras[0].rect = new Rect(0, 0, 0.5f, 1);
                m_cameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                break;
            //case 3://spectator camera hinzufügen
                //_cameras[0].rect = new Rect(0, 0, 0.5f, 0.5f);
                //_cameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                //_cameras[2].rect = new Rect(0, 0.5f, 1, 0.5f);
                //break;
            case 4:
                m_cameras[0].rect = new Rect(0, 0, 0.5f, 0.5f);
                m_cameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                m_cameras[2].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                m_cameras[3].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                break;
            default:
                if (m_fillerSpectator) {
                    Destroy(m_fillerSpectator);
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