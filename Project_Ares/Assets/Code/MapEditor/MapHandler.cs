using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class MapHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] BoxCollider2D m_cameraSize;
        int m_cameraSizeIndex;
        [SerializeField] AudioSource m_backgroundAudio;
        int m_backgroundAudioIndex;
        [SerializeField] SpriteRenderer m_background;
        int m_backgroundIndex;
        [SerializeField] Light m_directionalLight;
        int m_directionalLightIndex;
        [SerializeField] Transform m_levelHolder;
        [SerializeField] GameObject m_lightPrefab;
        [SerializeField] GameObject m_ObjectInteractPrefab;
        [SerializeField] Material m_spriteMaterial;

        [HideInInspector]
        public List<Transform> m_borders { get; private set; } = new List<Transform>();

        public static MapDATA s_refMap { get; private set; }

        #endregion

        private void Start() {
            if (!m_cameraSize) {
                print("FATAL: no Camera");
                m_cameraSize = Camera.main.GetComponent<BoxCollider2D>();
                if (m_cameraSize) {
                    print("tryed to fix it");
                } else {
                    Destroy(this);
                    return;
                }
            }
            if (!m_backgroundAudio) {
                print("no Background audio");
                GameObject tmp = new GameObject("AUTO_BackgroundMusic");
                m_backgroundAudio = tmp.AddComponent<AudioSource>();
                m_backgroundAudio.loop = true;
            }
            if (!m_background) {
                print("no Background Sprite");
                GameObject tmp = new GameObject("AUTO_BackgroundSprite");
                m_background = tmp.AddComponent<SpriteRenderer>();
                m_background.sortingLayerName = StringCollection.BACKGROUND;
                if (m_spriteMaterial) {
                    m_background.material = m_spriteMaterial;
                }
            }
            if (!m_directionalLight) {
                print("no global light");
                GameObject tmp = new GameObject("AUTO_GlobalLight");
                tmp.transform.rotation = Quaternion.Euler(40.385f, -57.437f, -55.158f);
                m_directionalLight = tmp.AddComponent<Light>();
                m_directionalLight.type = LightType.Directional;
            }
            if (!m_levelHolder) {
                print("no gameobject to cast all levelelements to");
                m_levelHolder = new GameObject("AUTO_LevelHolder").transform;
            }
            if (!m_lightPrefab) {
                print("FATAL: no light prefab found");
                Destroy(this);
                return;
            }
            if(DataHolder.s_gameMode == e_gameMode.COOP_EDIT && (!m_ObjectInteractPrefab || !m_ObjectInteractPrefab.GetComponent<ObjectReferenceHolder>() || !m_ObjectInteractPrefab.GetComponent<ObjectReferenceHolder>().m_objectHolder) ) {
                print("FATAL: no object Interaction Prefab");
                Destroy(this);
                return;
            }
        }

        private void OnDestroy() {
            m_borders = new List<Transform>();
        }

        public void SetBackgroundIndex(int index) {
            if (index >= -s_refMap.p_background.Length && index < DataHolder.s_commonBackground.Length)
                m_backgroundIndex = index;
            else
                return;

            if (index >= 0) {
                m_background.sprite = DataHolder.s_commonBackground[index];
            } else {
                index *= -1;
                index--;
                m_background.sprite = s_refMap.p_background[index];
            }
        }

        public void SetMusicIndex(int index) {
            if (index >= -s_refMap.p_music.Length && index < DataHolder.s_commonMusic.Length)
                m_backgroundAudioIndex = index;
            else
                return;

            if (index >= 0) {
                m_backgroundAudio.clip = DataHolder.s_commonMusic[index];
            } else {
                index *= -1;
                index--;
                m_backgroundAudio.clip = s_refMap.p_music[index];
            }

            if(!m_backgroundAudio.isPlaying)
                m_backgroundAudio.Play();
        }
        
        public void SetGlobalLightColorIndex(int index) {
            if (index >= -s_refMap.p_colors.Length && index < DataHolder.s_commonColors.Length)
                m_directionalLightIndex = index;
            else
                return;

            if (index >= 0) {
                m_directionalLight.color = DataHolder.s_commonColors[index];
            } else {
                index *= -1;
                index--;
                m_directionalLight.color = s_refMap.p_colors[index];
            }
        }

        public void SetSizeIndex(int index) {
            if (index >= -s_refMap.p_size.Length && index < DataHolder.s_commonSize.Length)
                m_cameraSizeIndex = index;
            else
                return;

            if (index >= 0) {
                m_cameraSize.size = DataHolder.s_commonSize[index];
            } else {
                index *= -1;
                index--;
                m_cameraSize.size = s_refMap.p_size[index];
            }
        }

        public void LoadCurrentMap() {
            UnloadMap();

            print("load map " + DataHolder.s_map);
            foreach(var it in DataHolder.s_maps) {
                print(it.Key);
            }

            s_refMap = DataHolder.s_maps[DataHolder.s_map];
            if (!s_refMap) {
                print("map was not pre loaded");
                s_refMap = DataHolder.LoadMap(DataHolder.s_map);
                DataHolder.s_maps[DataHolder.s_map] = s_refMap;
            }
            print(s_refMap.p_background.Length);
            print(s_refMap.p_colors.Length);
            print(s_refMap.p_forground.Length);
            print(s_refMap.p_music.Length);
            print(s_refMap.p_props.Length);
            print(s_refMap.p_size.Length);
            print(s_refMap.p_stage.Length);
            foreach (var it in s_refMap.m_data) {
                print(it.index + " | " + it.position.ToString());
            }

            SetBackgroundIndex(s_refMap.m_background);
            SetGlobalLightColorIndex(s_refMap.m_globalLight);
            SetMusicIndex(s_refMap.m_music);
            SetSizeIndex(s_refMap.m_size);

            foreach (var it in s_refMap.m_data) {
                LoadNewObj(it);
            }

            GameObject a = Instantiate(DataHolder.s_commonLaserBariar);
            a.transform.parent = m_levelHolder;
        }

        public void SaveMap(string name) {
            if (name == "") {
                name = DataHolder.s_map;
            }
            name += ".map";
            name = name.Replace('\\', '_');
            name = name.Replace('/', '_');
            name = name.Replace(':', '_');
            name = name.Replace('*', '_');
            name = name.Replace('?', '_');
            name = name.Replace('"', '_');
            name = name.Replace('<', '_');
            name = name.Replace('>', '_');
            name = name.Replace('|', '_');
            name = name.Replace(' ', '_');

            MapDATA map = s_refMap.Copy();
            map.name = name;

            map.m_background = m_backgroundIndex;
            map.m_globalLight = m_directionalLightIndex;
            map.m_music = m_backgroundAudioIndex;
            map.m_size = m_cameraSizeIndex;

            List<d_mapData> tmp = new List<d_mapData>();
            foreach (var it in ObjectReferenceHolder.s_references) {
                tmp.Add(it.m_data);
            }
            map.m_data = tmp.ToArray();
            DataHolder.SaveMap(map);

            DataHolder.s_maps[name] = map;
            DataHolder.s_map = name;
        }

        public void LoadNewObj(d_mapData obj) {
            GameObject tmp = LoadObj(obj);
            if (!tmp)
                return;

            if (DataHolder.s_gameMode == e_gameMode.COOP_EDIT) {
                ObjectReferenceHolder orh = Instantiate(m_ObjectInteractPrefab, m_levelHolder).GetComponent<ObjectReferenceHolder>();
                orh.name = "[EDIT]_" + tmp.name;
                orh.m_data = obj;
                orh.transform.position = tmp.transform.position;
                orh.m_objectHolder.rotation = tmp.transform.rotation;
                orh.m_objectHolder.localScale = tmp.transform.localScale;
                
                tmp.transform.parent = orh.m_objectHolder;

                tmp.transform.localPosition = Vector3.zero;
                tmp.transform.localRotation = Quaternion.Euler(Vector3.zero);
                tmp.transform.localScale = new Vector3(1, 1, 1);
            } else {
                tmp.transform.parent = m_levelHolder;
            }
        }

        public GameObject LoadObj(d_mapData obj) {//eventuell überarbeiten, wenn common objects
            GameObject tmp = null;
            SpriteRenderer ren;
            PolygonCollider2D col;
            SpriteMask msk;

            switch (obj.type) {
            case e_objType.BACKGROUND:
                return null;
            case e_objType.PROP:
                if (obj.index < -s_refMap.p_props.Length || obj.index >= DataHolder.s_commonProps.Length)
                    return null;
                
                tmp = new GameObject("Prop " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);
                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.PROPS;
                col = tmp.AddComponent<PolygonCollider2D>();
                
                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_props[obj.index * -1 - 1].m_sprite;
                    col.SetPath(0, s_refMap.p_props[obj.index * -1 - 1].m_collider);
                } else {//common
                    ren.sprite = DataHolder.s_commonProps[obj.index].m_sprite;
                    col.SetPath(0, DataHolder.s_commonProps[obj.index].m_collider);
                }
                
                msk = tmp.AddComponent<SpriteMask>();
                msk.sprite = ren.sprite;
                break;
            case e_objType.STAGE:
                if (obj.index < -s_refMap.p_stage.Length || obj.index >= DataHolder.s_commonStage.Length)
                    return null;

                tmp = new GameObject("Stage " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.STAGE;

                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_stage[obj.index * -1 - 1];
                } else {//common
                    ren.sprite = DataHolder.s_commonStage[obj.index];
                }

                msk = tmp.AddComponent<SpriteMask>();
                msk.sprite = ren.sprite;
                break;
            case e_objType.PLAYERSTART:
                tmp = new GameObject("PlayerSpawn " + obj.index);
                tmp.transform.position = obj.position;

                tmp.AddComponent<PlayerStart>().m_team = obj.index;
                break;
            case e_objType.LIGHT:
                if (obj.index < -s_refMap.p_colors.Length || obj.index >= DataHolder.s_commonColors.Length)
                    return null;

                tmp = Instantiate(m_lightPrefab, obj.position, Quaternion.Euler(Vector3.zero));

                if (obj.index < 0) {//specific
                    tmp.GetComponentInChildren<Light>().color = s_refMap.p_colors[obj.index * -1 - 1];
                } else {//common
                    tmp.GetComponentInChildren<Light>().color = DataHolder.s_commonColors[obj.index];
                }
                break;
            case e_objType.FORGROUND:
                if (obj.index < -s_refMap.p_stage.Length || obj.index >= DataHolder.s_commonStage.Length)
                    return null;

                tmp = new GameObject("Forground " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.FORGROUND;

                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_stage[obj.index * -1 - 1];
                } else {//common
                    ren.sprite = DataHolder.s_commonStage[obj.index];
                }
                break;
            case e_objType.BORDER:
                if (obj.index < -s_refMap.p_props.Length || obj.index >= DataHolder.s_commonProps.Length)
                    return null;

                tmp = new GameObject("Prop " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);
                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.PROPS;
                col = tmp.AddComponent<PolygonCollider2D>();

                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_props[obj.index * -1 - 1].m_sprite;
                    col.SetPath(0, s_refMap.p_props[obj.index * -1 - 1].m_collider);
                } else {//common
                    ren.sprite = DataHolder.s_commonProps[obj.index].m_sprite;
                    col.SetPath(0, DataHolder.s_commonProps[obj.index].m_collider);
                }

                msk = tmp.AddComponent<SpriteMask>();
                msk.sprite = ren.sprite;

                m_borders.Add(tmp.transform);
                break;
            default:
                return null;
            }

            return tmp;
        }

        public void UnloadMap() {
            foreach (Transform it in m_levelHolder) {
                Destroy(it.gameObject);
            }
            m_borders = new List<Transform>();
        }
    }
}