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
        [SerializeField] Material m_backGroundMaterial;

        [HideInInspector]
        public List<Transform> m_borders { get; private set; } = new List<Transform>();

        MapDATA m_refMap;

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
                if (m_backGroundMaterial) {
                    m_background.material = m_backGroundMaterial;
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
            if (index < 0 || index >= m_refMap.p_background.Length)
                return;

            m_backgroundIndex = index;
            m_background.sprite = m_refMap.p_background[m_backgroundIndex];
        }

        public void SetMusicIndex(int index) {
            if (index < 0 || index >= m_refMap.p_music.Length)
                return;

            m_backgroundAudioIndex = index;
            m_backgroundAudio.clip = m_refMap.p_music[m_backgroundAudioIndex];
            if(!m_backgroundAudio.isPlaying)
                m_backgroundAudio.Play();
        }
        
        public void SetGlobalLightColor(int index) {
            if (index < 0 || index >= m_refMap.p_colors.Length)
                return;

            m_directionalLightIndex = index;
            m_directionalLight.color = m_refMap.p_colors[m_directionalLightIndex];
        }

        public void SetSize(int index) {
            if (index < 0 || index >= m_refMap.p_size.Length)
                return;

            m_cameraSizeIndex = index;
            m_cameraSize.size = m_refMap.p_size[m_cameraSizeIndex];
        }

        public void LoadCurrentMap() {
            UnloadMap();

            m_refMap = DataHolder.s_maps[DataHolder.s_map];

            m_background.sprite = m_refMap.p_background[m_refMap.m_background];
            m_directionalLight.color = m_refMap.p_colors[m_refMap.m_globalLight];
            m_backgroundAudio.clip = m_refMap.p_music[m_refMap.m_music];
            m_backgroundAudio.Play();
            m_cameraSize.size = m_refMap.p_size[m_refMap.m_size];

            foreach (var it in m_refMap.m_data) {
                LoadNewObj(it);
            }

            GameObject a = Instantiate(m_refMap.p_laserBariar);
            a.transform.parent = m_levelHolder;
        }

        public void SaveMap(string name) {
            if (name == "") {
                name = DataHolder.s_maps[DataHolder.s_map].name;
            } else if (name != DataHolder.s_maps[DataHolder.s_map].name) {
                //datenstrucktur kopieren;
            }

            MapDATA map = DataHolder.s_maps[DataHolder.s_map];//Tempery

            map.m_background = m_backgroundIndex;
            map.m_globalLight = m_directionalLightIndex;
            map.m_music = m_backgroundAudioIndex;
            map.m_size = m_cameraSizeIndex;

            List<d_mapData> tmp = new List<d_mapData>();
            foreach (var it in ObjectReferenceHolder.s_references) {
                tmp.Add(it.m_data);
            }
            map.m_data = tmp.ToArray();

            DataHolder.s_maps[DataHolder.s_map] = map;
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

            switch (obj.type) {
            case e_objType.BACKGROUND:
                return null;
            case e_objType.PROP:
                if (obj.index < 0 || obj.index >= m_refMap.p_props.Length)
                    return null;

                tmp = Instantiate(m_refMap.p_props[obj.index], obj.position, Quaternion.Euler(new Vector3(0, 0, obj.rotation)));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);
                
                break;
            case e_objType.STAGE:
                if (obj.index < 0 || obj.index >= m_refMap.p_stage.Length)
                    return null;

                tmp = new GameObject("Stage " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.sprite = m_refMap.p_stage[obj.index];
                ren.sortingLayerName = StringCollection.STAGE;

                SpriteMask msk = tmp.AddComponent<SpriteMask>();
                msk.sprite = ren.sprite;
                break;
            case e_objType.PLAYERSTART:
                tmp = new GameObject("PlayerSpawn " + obj.index);
                tmp.transform.position = obj.position;

                tmp.AddComponent<PlayerStart>().team = obj.index;
                break;
            case e_objType.LIGHT:
                if (obj.index < 0 || obj.index >= m_refMap.p_colors.Length)
                    return null;

                tmp = Instantiate(m_lightPrefab, obj.position, Quaternion.Euler(Vector3.zero));
                tmp.GetComponentInChildren<Light>().color = m_refMap.p_colors[obj.index];
                break;
            case e_objType.FORGROUND:
                if (obj.index < 0 || obj.index >= m_refMap.p_stage.Length)
                    return null;

                tmp = new GameObject("Forground " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.sprite = m_refMap.p_stage[obj.index];
                ren.sortingLayerName = StringCollection.FORGROUND;
                break;
            case e_objType.BORDER:
                if (obj.index < 0 || obj.index >= m_refMap.p_props.Length)
                    return null;

                tmp = Instantiate(m_refMap.p_props[obj.index], obj.position, Quaternion.Euler(new Vector3(0, 0, obj.rotation)));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);

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