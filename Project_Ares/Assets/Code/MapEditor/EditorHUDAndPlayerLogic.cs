using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    public class EditorHUDAndPlayerLogic : MonoBehaviour {

        enum e_editingStyle { ROTATE = 0, MOVE = 1, SCALE = 2}

        [Header("References")]
        [SerializeField] TextMeshProUGUI m_typeRef;
        [SerializeField] TextMeshProUGUI m_IndexRef;

        [Header("Balancing")]
        [SerializeField] float m_scalerDampaning = 110;
        [SerializeField] float m_speed = 6;

        IControl m_controlRef;
        bool m_isDraging = false;
        ObjectReferenceHolder m_mapObj;
        Transform m_editorObj;
        e_editingStyle m_styple = e_editingStyle.MOVE;

        e_objType m_type;
        int m_rawIndex;
        int m_index;
        // Start is called before the first frame update
        void Start() {
            m_type = e_objType.BACKGROUND;
            m_typeRef.text = m_type.ToString();

            m_rawIndex = 0;
            m_index = m_rawIndex;
            m_IndexRef.text = m_index.ToString();
        }

        // Update is called once per frame
        void Update() {
            if(m_styple == e_editingStyle.MOVE) {
                if(m_controlRef.m_dir.x > 1 || m_controlRef.m_dir.x < -1 || m_controlRef.m_dir.y > 1 || m_controlRef.m_dir.y < -1) {
                    transform.parent.position += new Vector3(m_controlRef.m_dir.x, m_controlRef.m_dir.y, 0) * 0.5f;
                } else {
                    transform.parent.position += new Vector3(m_controlRef.m_dir.x * Mathf.Sqrt(1 - ((m_controlRef.m_dir.y * m_controlRef.m_dir.y) / 2)), m_controlRef.m_dir.y * Mathf.Sqrt(1 - ((m_controlRef.m_dir.x * m_controlRef.m_dir.x) / 2)), 0) * Time.deltaTime * m_speed;
                }
            }

            if (m_isDraging) {

                Transform tmp;
                if (m_mapObj)
                    tmp = m_mapObj.m_objectHolder;
                else
                    tmp = transform;
                
                switch (m_styple) {
                case e_editingStyle.ROTATE:
                    tmp.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-m_controlRef.m_dir.y, m_controlRef.m_dir.x));//vektor irgendwie drehen, damit es in der 2d plain bleibt
                    break;
                case e_editingStyle.MOVE:
                    if (m_editorObj)
                        m_editorObj.position = transform.position;
                    else
                        m_mapObj.transform.position = transform.position;
                    break;
                case e_editingStyle.SCALE:
                    float scaler = Vector2.SignedAngle(transform.up, m_controlRef.m_dir);
                    scaler /= m_scalerDampaning;
                    scaler = scaler * scaler * scaler;
                    tmp.localScale = new Vector3(scaler, scaler, 1);
                    break;
                default:
                    break;
                }
            }
        }

        private void OnDisable() {
            Drop();
        }

        public void SetControlRef(IControl reference) {
            m_controlRef = reference;
        }

        public void DragHandler() {
            if (m_isDraging) {
                Drop();
                return;
            }

            Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
            foreach (var it in hits) {
                if(it.tag == StringCollection.BIN) {
                    m_editorObj = it.transform;
                    m_mapObj = null;
                    m_isDraging = true;

                    m_typeRef.text = m_styple.ToString();
                    return;
                }
                ExitEditor ee = it.GetComponent<ExitEditor>();
                if (ee) {
                    m_editorObj = ee.transform;
                    m_mapObj = null;
                    m_isDraging = true;

                    m_typeRef.text = m_styple.ToString();
                    return;
                }
                ObjectReferenceHolder orh = it.GetComponent<ObjectReferenceHolder>();
                if (orh != null) {
                    m_type = orh.m_data.type;
                    CorrectIndex();

                    m_mapObj = orh;
                    m_isDraging = true;

                    m_typeRef.text = m_styple.ToString();
                    return;
                }
            }

            GameManager.s_singelton.m_mapHandler.LoadNewObj(CreateData());
        }

        void Drop() {
            if (m_isDraging && m_mapObj) {
                bool deleted = false;
                Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
                foreach (var it in hits) {
                    if(it.tag == StringCollection.BIN) {
                        Destroy(m_mapObj.gameObject);
                        deleted = true;
                        break;
                    }
                }
                if (!deleted) {
                    m_mapObj.m_data.position = m_mapObj.m_objectHolder.position;
                    m_mapObj.m_data.rotation = m_mapObj.m_objectHolder.rotation.eulerAngles.z;
                    m_mapObj.m_data.scale = m_mapObj.m_objectHolder.localScale;
                }
            }

            m_isDraging = false;
            m_styple = e_editingStyle.MOVE;
            m_mapObj = null;
            m_editorObj = null;

            m_typeRef.text = m_type.ToString();
        }

        d_mapData CreateData() {
            d_mapData tmp;
            tmp.type = m_type;
            tmp.index = m_index;
            tmp.position = transform.position;
            tmp.rotation = transform.rotation.eulerAngles.z;
            tmp.scale = transform.parent.localScale;
            return tmp;
        }

        public void ChangeType(bool forward) {
            if (m_isDraging) {
                if (forward) {
                    m_styple++;
                } else {
                    m_styple--;
                }
                m_styple = (e_editingStyle)fixedMod((int)m_styple, 3);//reference to enum at top of this script
                m_typeRef.text = m_styple.ToString();
                return;
            }

            if (forward)
                m_type++;
            else
                m_type--;

            m_type = (e_objType)fixedMod((int)m_type, 10);//reference to object type;
            m_typeRef.text = m_type.ToString();

            
            switch (m_type) {
            case e_objType.BACKGROUND:
                m_rawIndex = MapHandler.s_refMap.m_background;
                break;
            case e_objType.GLOBALLIGHT:
                m_rawIndex = MapHandler.s_refMap.m_globalLight;
                break;
            case e_objType.MUSIC:
                m_rawIndex = MapHandler.s_refMap.m_music;
                break;
            case e_objType.SIZE:
                m_rawIndex = MapHandler.s_refMap.m_size;
                break;
            default:
                m_rawIndex = 0;//everytime you change type it resets the index to 0
                break;
            }
            CorrectIndex();

            if (m_editorObj) {
                return;
            }

            if (m_type == e_objType.BACKGROUND) {
                GameManager.s_singelton.m_mapHandler.SetBackgroundIndex(m_index);
            } else if (m_type == e_objType.GLOBALLIGHT) {
                GameManager.s_singelton.m_mapHandler.SetGlobalLightColorIndex(m_index);
            } else if (m_type == e_objType.MUSIC) {
                GameManager.s_singelton.m_mapHandler.SetMusicIndex(m_index);
            } else if (m_type == e_objType.SIZE) {
                GameManager.s_singelton.m_mapHandler.SetSizeIndex(m_index);
            }
        }

        public void ChangeIndex(int value, bool relative) {
            if (relative) {
                m_rawIndex += value;
            } else {
                m_rawIndex = value;
            }

            CorrectIndex();

            if (m_editorObj) {
                return;
            }

            if(m_type == e_objType.BACKGROUND) {
                GameManager.s_singelton.m_mapHandler.SetBackgroundIndex(m_index);
            }else if(m_type == e_objType.GLOBALLIGHT) {
                GameManager.s_singelton.m_mapHandler.SetGlobalLightColorIndex(m_index);
            }else if(m_type == e_objType.MUSIC) {
                GameManager.s_singelton.m_mapHandler.SetMusicIndex(m_index);
            }else if(m_type == e_objType.SIZE) {
                GameManager.s_singelton.m_mapHandler.SetSizeIndex(m_index);
            }
            else if (m_isDraging && m_mapObj.m_objectHolder) {
                foreach(Transform it in m_mapObj.m_objectHolder) {
                    Destroy(it.gameObject);
                }
                m_mapObj.m_data = CreateData();
                GameObject tmp = GameManager.s_singelton.m_mapHandler.LoadObj(m_mapObj.m_data);
                tmp.transform.parent = m_mapObj.m_objectHolder;
                tmp.transform.localPosition = Vector3.zero;
                tmp.transform.localRotation = Quaternion.Euler(Vector3.zero);
                tmp.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        void CorrectIndex() {//arbeited auf current map reference. eventuell auf kopie arbeiten
            int max = 0;
            int min = 0;
            switch (m_type) {
            case e_objType.BACKGROUND:
                max = DataHolder.s_commonBackground.Length;
                min = MapHandler.s_refMap.p_background.Length;
                break;
            case e_objType.PROP:
                max = DataHolder.s_commonProps.Length;
                min = MapHandler.s_refMap.p_props.Length;
                break;
            case e_objType.STAGE:
                max = DataHolder.s_commonStage.Length;
                min = MapHandler.s_refMap.p_stage.Length;
                break;
            case e_objType.PLAYERSTART:
                max = -1;
                min = 0;
                break;
            case e_objType.LIGHT:
                max = DataHolder.s_commonColors.Length;
                min = MapHandler.s_refMap.p_colors.Length;
                break;
            case e_objType.FORGROUND:
                max = DataHolder.s_commonForground.Length;
                min = MapHandler.s_refMap.p_forground.Length;
                break;
            case e_objType.BORDER:
                max = DataHolder.s_commonProps.Length;
                min = MapHandler.s_refMap.p_props.Length;
                break;
            case e_objType.GLOBALLIGHT:
                max = DataHolder.s_commonColors.Length;
                min = MapHandler.s_refMap.p_colors.Length;
                break;
            case e_objType.MUSIC:
                max = DataHolder.s_commonMusic.Length;
                min = MapHandler.s_refMap.p_music.Length;
                break;
            case e_objType.SIZE:
                max = DataHolder.s_commonSize.Length;
                min = MapHandler.s_refMap.p_size.Length;
                break;
            default:
                break;
            }

            m_index = fixedMod(m_rawIndex + min, max + min) - min;

            m_IndexRef.text = m_index.ToString();
        }

        int fixedMod(int value, int mod) {
            if(mod < 0) {
                return value;
            }
            if(mod == 0) {
                return 0;
            }
            return ((value % mod) + mod) % mod;
        }
    }
}