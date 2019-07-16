using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class MapHandler : MonoBehaviour {

        public static MapHandler s_singelton;
        public static MapData s_refMap { get; private set; }

        #region Variables

        [Header("References")]
        BoxCollider2D m_cameraSize;
        int m_cameraSizeIndex;
        int m_backgroundAudioIndex;
        [SerializeField] SpriteRenderer m_background;
        int m_backgroundIndex;
        int m_directionalLightIndex;
        [SerializeField] Transform m_levelHolder;
        [SerializeField] GameObject m_lightPrefab;
        [SerializeField] GameObject m_ObjectInteractPrefab;
        [SerializeField] Material m_spriteMaterial;
        [SerializeField] RectTransform m_KillFeed;

        #endregion

        private void Awake() {
            if(s_singelton != null && s_singelton != this) {
                Destroy(this);
                return;
            }

            s_singelton = this;
        }

        private void Start() {
            m_cameraSize = Camera.main.GetComponent<BoxCollider2D>();
            if (!m_background) {
                print("no Background Sprite");
                GameObject tmp = new GameObject("AUTO_BackgroundSprite");
                tmp.layer = LayerMask.NameToLayer(StringCollection.L_BACKGROUND);

                m_background = tmp.AddComponent<SpriteRenderer>();
                m_background.sortingLayerName = StringCollection.L_BACKGROUND;
                if (m_spriteMaterial) {
                    m_background.material = m_spriteMaterial;
                }
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
            if ((DataHolder.s_currentModi == -1 || DataHolder.s_modis[DataHolder.s_currentModi].m_name == StringCollection.M_COOPEDIT) &&
                (!m_ObjectInteractPrefab || !m_ObjectInteractPrefab.GetComponent<ObjRefHolder>() || !m_ObjectInteractPrefab.GetComponent<ObjRefHolder>().m_objectHolder)) {
                print("FATAL: no object Interaction Prefab");
                Destroy(this);
                return;
            }
            if (!m_KillFeed) {
                print("FATAL: no reference to KillFeed");
                Destroy(this);
                return;
            }
            if (DataHolder.s_currentModi == -1 || DataHolder.s_modis[DataHolder.s_currentModi].m_name == StringCollection.M_COOPEDIT) {
                if(DataHolder.s_currentModi == -1)
                    DataHolder.s_currentModi = 0;
                LoadCurrentMap(true);
            } else {
                LoadCurrentMap();
            }
        }

        private void OnDestroy() {
            if(s_singelton == this) {
                s_singelton = null;
            }
        }

        #region SetIndex

        public void SetBackgroundIndex(int index) {
            if (index >= -s_refMap.p_backgrounds.Length && index < DataHolder.s_commonBackgrounds.Length)
                m_backgroundIndex = index;
            else
                return;

            if (index >= 0) {
                m_background.sprite = DataHolder.s_commonBackgrounds[index].m_image;
                m_KillFeed.position = DataHolder.s_commonBackgrounds[index].m_position;
                m_KillFeed.sizeDelta = DataHolder.s_commonBackgrounds[index].m_size;
            } else {
                index *= -1;
                index--;
                m_background.sprite = s_refMap.p_backgrounds[index].m_image;
                m_KillFeed.position = s_refMap.p_backgrounds[index].m_position;
                m_KillFeed.sizeDelta = s_refMap.p_backgrounds[index].m_size;
            }
        }

        public void SetMusicIndex(int index) {
            if (index >= -s_refMap.p_musics.Length && index < DataHolder.s_commonMusics.Length)
                m_backgroundAudioIndex = index;
            else
                return;

            if (index >= 0) {
                GlobalAudioManager.ChangeAudio(DataHolder.s_commonMusics[index]);
            } else {
                index *= -1;
                index--;
                GlobalAudioManager.ChangeAudio(s_refMap.p_musics[index]);
            }
        }

        public void SetGlobalLightColorIndex(int index) {
            if (index >= -s_refMap.p_colors.Length && index < DataHolder.s_commonColors.Length)
                m_directionalLightIndex = index;
            else
                return;

            if (index >= 0) {
                DataHolder.s_dirLight.color = DataHolder.s_commonColors[index];
            } else {
                index *= -1;
                index--;
                DataHolder.s_dirLight.color = s_refMap.p_colors[index];
            }
        }

        public void SetSizeIndex(int index) {
            if (index >= -s_refMap.p_sizes.Length && index < DataHolder.s_commonSizes.Length)
                m_cameraSizeIndex = index;
            else
                return;

            if (index >= 0) {
                m_cameraSize.size = DataHolder.s_commonSizes[index];
            } else {
                index *= -1;
                index--;
                m_cameraSize.size = s_refMap.p_sizes[index];
            }
        }

        #endregion

        public void LoadCurrentMap(bool withHolder = false) {
            UnloadMap();

            print("load map " + DataHolder.s_maps[DataHolder.s_currentMap].m_name);

            s_refMap = DataHolder.s_maps[DataHolder.s_currentMap];

            if((DataHolder.s_currentModi == -1 || DataHolder.s_modis[DataHolder.s_currentModi].m_name == StringCollection.M_COOPEDIT)) {
                s_refMap.EditBoot();
            } else {
                s_refMap.EditBoot();//TODO: change to Boot
            }
            DataHolder.s_maps[DataHolder.s_currentMap] = s_refMap;

            SetBackgroundIndex(s_refMap.m_background);
            SetGlobalLightColorIndex(s_refMap.m_globalLight);
            SetMusicIndex(s_refMap.m_music);
            SetSizeIndex(s_refMap.m_size);

            foreach (var it in s_refMap.m_data) {
                string name = DataHolder.s_modis[DataHolder.s_currentModi].m_name;
                switch (it.type) {
                case e_objType.LASERSPAWN:
                    if (false)//gamemodes eintragen
                        continue;
                    break;
                case e_objType.FLAG:
                    if (name == StringCollection.M_FFA ||
                        name == StringCollection.M_TDM)
                        continue;
                    break;
                case e_objType.BASKETHOOP:
                    if (name == StringCollection.M_FFA ||
                        name == StringCollection.M_TDM)
                        continue;
                    break;
                default:
                    break;
                }
                LoadNewObj(it, withHolder);
            }
            
            DataHolder.s_modis[DataHolder.s_currentModi].SetUpGame();
        }

        public void SaveMap(string name) {
            MapData map = CreateMapData(name);
            map.SaveToJSON();

            DataHolder.s_currentMap = DataHolder.s_maps.Count;
            DataHolder.s_maps.Add(map);
        }

        public MapData CreateMapData(string name = "") {
            if (name == "") {
                name = s_refMap.name;
            }

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

            MapData map = new MapData(s_refMap);
            map.name = name;
            map.m_name = name;

            map.m_background = m_backgroundIndex;
            map.m_globalLight = m_directionalLightIndex;
            map.m_music = m_backgroundAudioIndex;
            map.m_size = m_cameraSizeIndex;

            List<d_mapData> tmp = new List<d_mapData>();
            foreach (var it in ObjRefHolder.s_references) {
                tmp.Add(it.Update());
            }
            map.m_data = tmp.ToArray();

            return map;
        }

        public void LoadNewObj(d_mapData obj, bool withHolder = false) {
            GameObject tmp = LoadObj(obj);
            if (!tmp)
                return;
            
            if (withHolder || DataHolder.s_modis[DataHolder.s_currentModi].m_name == StringCollection.M_COOPEDIT) {
                ObjRefHolder orh = Instantiate(m_ObjectInteractPrefab, m_levelHolder).GetComponent<ObjRefHolder>();
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

        public GameObject LoadObj(d_mapData obj) {
            GameObject tmp = null;
            SpriteRenderer ren;
            PolygonCollider2D col;
            SpriteMask msk;

            switch (obj.type) {
            case e_objType.BACKGROUND:
                SetBackgroundIndex(obj.index);
                break;
            case e_objType.GLOBALLIGHT:
                SetGlobalLightColorIndex(obj.index);
                break;
            case e_objType.MUSIC:
                SetMusicIndex(obj.index);
                break;
            case e_objType.SIZE:
                SetSizeIndex(obj.index);
                break;
            case e_objType.PROP:
                if (obj.index < -s_refMap.p_props.Length || obj.index >= DataHolder.s_commonProps.Length)
                    return null;

                tmp = new GameObject("Prop " + obj.index);
                tmp.tag = "Level";
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);
                tmp.layer = LayerMask.NameToLayer(StringCollection.L_LEVEL);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.L_PROPS;
                col = tmp.AddComponent<PolygonCollider2D>();

                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_props[obj.index * -1 - 1].m_image;
                    col.SetPath(0, s_refMap.p_props[obj.index * -1 - 1].m_collider);
                } else {//common
                    ren.sprite = DataHolder.s_commonProps[obj.index].m_image;
                    col.SetPath(0, DataHolder.s_commonProps[obj.index].m_collider);
                }

                msk = tmp.AddComponent<SpriteMask>();
                msk.sprite = ren.sprite;
                break;
            case e_objType.STAGE:
                if (obj.index < -s_refMap.p_stages.Length || obj.index >= DataHolder.s_commonStages.Length)
                    return null;

                tmp = new GameObject("Stage " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);
                tmp.layer = LayerMask.NameToLayer(StringCollection.L_LEVEL);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.L_STAGE;

                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_stages[obj.index * -1 - 1];
                } else {//common
                    ren.sprite = DataHolder.s_commonStages[obj.index];
                }

                msk = tmp.AddComponent<SpriteMask>();
                msk.sprite = ren.sprite;
                break;
            case e_objType.SPAWNPOINT:
                tmp = new GameObject("PlayerSpawn " + obj.index);
                tmp.transform.position = obj.position;

                tmp.AddComponent<SpawnPoint>().m_team = obj.index;
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
                if (obj.index < -s_refMap.p_stages.Length || obj.index >= DataHolder.s_commonStages.Length)
                    return null;

                tmp = new GameObject("Forground " + obj.index);
                tmp.transform.position = obj.position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, obj.rotation));
                tmp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, 1);
                tmp.layer = LayerMask.NameToLayer(StringCollection.L_LEVEL);

                ren = tmp.AddComponent<SpriteRenderer>();
                ren.material = m_spriteMaterial;
                ren.sortingLayerName = StringCollection.L_FORGROUND;

                if (obj.index < 0) {//specific
                    ren.sprite = s_refMap.p_stages[obj.index * -1 - 1];
                } else {//common
                    ren.sprite = DataHolder.s_commonStages[obj.index];
                }
                break;
            case e_objType.LASERSPAWN:
                tmp = Instantiate(DataHolder.s_commonLaserSpawner, obj.position, Quaternion.Euler(Vector3.zero));
                tmp.GetComponent<LaserSpawner>().Init(obj.index);
                tmp.layer = LayerMask.NameToLayer(StringCollection.L_LEVEL);
                break;
            case e_objType.FLAG:
                break;
            case e_objType.BASKETHOOP:
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
        }
    }
}