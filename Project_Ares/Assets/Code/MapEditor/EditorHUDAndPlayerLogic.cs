﻿using System.Collections;
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

        IControl m_controlRef;
        bool m_isDraging = false;
        ObjectReferenceHolder m_editObj;
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
            if(m_styple == e_editingStyle.MOVE)
                transform.parent.position += new Vector3(m_controlRef.m_dir.x * Mathf.Sqrt(1 - ((m_controlRef.m_dir.y * m_controlRef.m_dir.y) / 2)), m_controlRef.m_dir.y * Mathf.Sqrt(1 - ((m_controlRef.m_dir.x * m_controlRef.m_dir.x) / 2)), 0);

            if (m_isDraging) {
                
                switch (m_styple) {
                case e_editingStyle.ROTATE:
                    m_editObj.m_objectHolder.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-m_controlRef.m_dir.y, m_controlRef.m_dir.x));//vektor irgendwie drehen, damit es in der 2d plain bleibt
                    break;
                case e_editingStyle.MOVE:
                    m_editObj.transform.position = transform.position;
                    break;
                case e_editingStyle.SCALE:
                    float scaler = Vector2.SignedAngle(transform.up, m_controlRef.m_dir);
                    scaler /= m_scalerDampaning;
                    scaler = scaler * scaler * scaler;
                    m_editObj.m_objectHolder.localScale = new Vector3(scaler, scaler, 1);
                    break;
                default:
                    break;
                }
            }
        }

        public void SetControlRef(IControl reference) {
            m_controlRef = reference;
        }

        public void DragHandler() {
            if (m_isDraging) {
                Drop();
                return;
            }

            m_typeRef.text = m_styple.ToString();

            Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
            foreach (var it in hits) {
                ObjectReferenceHolder orh = it.GetComponent<ObjectReferenceHolder>();
                if (orh != null) {
                    if(orh.m_data.type == m_type) {
                        m_editObj = orh;
                        m_isDraging = true;
                    }
                    return;
                }
            }

            GameManager.s_singelton.m_mapHandler.LoadNewObj(CreateData());
        }

        void Drop() {
            if (m_isDraging) {
                m_editObj.m_data.position = m_editObj.m_objectHolder.position;
                m_editObj.m_data.rotation = m_editObj.m_objectHolder.rotation.eulerAngles.z;
                m_editObj.m_data.scale = m_editObj.m_objectHolder.localScale;

                m_isDraging = false;
            }
            m_styple = e_editingStyle.MOVE;

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

            CorrectIndex();

            if (m_type == e_objType.BACKGROUND) {
                GameManager.s_singelton.m_mapHandler.SetBackgroundIndex(m_index);
            } else if (m_type == e_objType.GLOBALLIGHT) {
                GameManager.s_singelton.m_mapHandler.SetGlobalLightColor(m_index);
            } else if (m_type == e_objType.MUSIC) {
                GameManager.s_singelton.m_mapHandler.SetMusicIndex(m_index);
            } else if (m_type == e_objType.SIZE) {
                GameManager.s_singelton.m_mapHandler.SetSize(m_index);
            }
        }

        public void ChangeIndex(int value, bool relative) {
            if (relative) {
                m_rawIndex += value;
            } else {
                m_rawIndex = value;
            }

            CorrectIndex();

            if(m_type == e_objType.BACKGROUND) {
                GameManager.s_singelton.m_mapHandler.SetBackgroundIndex(m_index);
            }else if(m_type == e_objType.GLOBALLIGHT) {
                GameManager.s_singelton.m_mapHandler.SetGlobalLightColor(m_index);
            }else if(m_type == e_objType.MUSIC) {
                GameManager.s_singelton.m_mapHandler.SetMusicIndex(m_index);
            }else if(m_type == e_objType.SIZE) {
                GameManager.s_singelton.m_mapHandler.SetSize(m_index);
            }
            else if (m_isDraging && m_editObj.m_objectHolder) {
                foreach(Transform it in m_editObj.m_objectHolder) {
                    Destroy(it.gameObject);
                }
                m_editObj.m_data = CreateData();
                GameManager.s_singelton.m_mapHandler.LoadObj(m_editObj.m_data).transform.parent = m_editObj.m_objectHolder;
            }
        }

        void CorrectIndex() {//arbeited auf current map reference. eventuell auf kopie arbeiten
            int max = 0;
            switch (m_type) {
            case e_objType.BACKGROUND:
                max = DataHolder.s_maps[DataHolder.s_map].p_background.Length;
                break;
            case e_objType.PROP:
                max = DataHolder.s_maps[DataHolder.s_map].p_props.Length;
                break;
            case e_objType.STAGE:
                max = DataHolder.s_maps[DataHolder.s_map].p_stage.Length;
                break;
            case e_objType.PLAYERSTART:
                max = -1;
                break;
            case e_objType.LIGHT:
                max = DataHolder.s_maps[DataHolder.s_map].p_colors.Length;
                break;
            case e_objType.FORGROUND:
                max = DataHolder.s_maps[DataHolder.s_map].p_forground.Length;
                break;
            case e_objType.BORDER:
                max = DataHolder.s_maps[DataHolder.s_map].p_props.Length;
                break;
            case e_objType.GLOBALLIGHT:
                max = DataHolder.s_maps[DataHolder.s_map].p_colors.Length;
                break;
            case e_objType.MUSIC:
                max = DataHolder.s_maps[DataHolder.s_map].p_music.Length;
                break;
            case e_objType.SIZE:
                max = DataHolder.s_maps[DataHolder.s_map].p_size.Length;
                break;
            default:
                break;
            }

            m_index = fixedMod(m_rawIndex, max);

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