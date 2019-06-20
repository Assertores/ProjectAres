using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System;

namespace PPBC {
    public enum e_objType { BACKGROUND = 0, PROP, STAGE, PLAYERSTART, LIGHT, FORGROUND, BORDER, GLOBALLIGHT, MUSIC, SIZE, FLAG, BASKETHOOP, ENUMLENGTH}

    [Serializable]
    public struct d_mapData {
        public e_objType type;
        public int index;
        public Vector2 position;
        public float rotation;
        public Vector2 scale;

        public override string ToString() {
            return type.ToString() + ";" + index + ";" + position.x + ";" + position.y + ";" + rotation + ";" + scale.x + ";" + scale.y;
        }
    }

    [Serializable]
    public struct d_prop {
        public Sprite m_sprite;
        public Vector2[] m_collider;
        public override string ToString() {
            string value;
            value = m_sprite.name + ".png";
            return value;
        }
    }

    [Serializable]
    public struct d_mapLights {
        public Vector2 position;
        public Color color;
    }
    [Serializable]
    public struct d_mapPlayerStart {
        public Vector2 position;
        public int team;
    }
    [Serializable]
    public struct d_mapBackground {
        public Sprite background;
        public Vector4 killFeed;//xcenter, ycenter, zwidth, whight
    }

    [CreateAssetMenu(menuName = "Map")]
    public class MapDATA : ScriptableObject {
        
        public static MapDATA LoadMap(string name) {
            Debug.Log("loading " + name);

            string dir = StringCollection.MAPPARH + name + "/"; //Path.DirectorySeparatorChar;
            string file = "";
            List<string> errorcodes = new List<string>();
            //===== ===== Read form file ===== =====
            if (!Directory.Exists(dir)) {
                Debug.Log("path " + dir + " not valide");
                return null;
            }
            if(!File.Exists(dir + name + ".map")) {
                Debug.Log("file " + dir + name + ".map" + " not valide");
                return null;
            }
            TextReader saveFile = new StreamReader(dir + name + ".map");
            if(saveFile == null) {
                Debug.Log("unable to open file: " + dir + name + ".map");
                return null;
            }

            file = saveFile.ReadToEnd();
            saveFile.Close();

            //===== ===== createing holder variables ===== =====

            MapDATA value = new MapDATA();
            value.m_background = 0;
            value.m_globalLight = 0;
            value.m_music = 0;
            value.m_size = 0;

            List<Vector2> sizes = new List<Vector2>();
            List<d_mapBackground> backgrounds = new List<d_mapBackground>();
            List<Color> colors = new List<Color>();
            List<AudioClip> musics = new List<AudioClip>();
            List<d_prop> props = new List<d_prop>();
            List<Sprite> stages = new List<Sprite>();
            List<Sprite> forgrounds = new List<Sprite>();
            List<d_mapData> datas = new List<d_mapData>();
            
            //===== ===== creating temporary variables ===== =====

            Sprite tmpSprite;
            AudioClip tmpMusic;
            Vector2 tmpV2;
            List<Vector2> tmpV2list = new List<Vector2>();
            Color tmpColor;
            d_prop tmpProp;
            tmpProp.m_sprite = null;
            tmpProp.m_collider = new Vector2[0];
            d_mapData tmpMData;
            string[] tmpStgArr;
            int propLine = 0;
            int bgdLine = 0;
            d_mapBackground tmpBgd;
            tmpBgd.background = null;
            tmpBgd.killFeed = new Vector4();
            Vector4 tmpV4;

            int readPos = 0;//1 = Resource, 2 = Data
            e_objType readType = e_objType.BORDER;//Border = non

            //===== ===== procesing file data ===== =====

            string[] lines = file.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++) {
                //Debug.Log("Current line: " + lines[i]);
                //----- ----- get category ----- -----
                if (lines[i] == "#Resources") {
                    readPos = 1;
                    readType = e_objType.BORDER;
                    continue;
                } else if (lines[i] == "#Data") {
                    readPos = 2;
                    readType = e_objType.BORDER;
                    continue;
                }

                if (readPos == 0) {
                    errorcodes.Add("line " + i + " is in no category: " + lines[i]);
                    continue;
                }

                //----- ----- get sub category ----- -----
                if(lines[i] == "[SIZE]") {
                    readType = e_objType.SIZE;
                    continue;
                }else if (lines[i] == "[BACKGROUND]") {
                    readType = e_objType.BACKGROUND;
                    continue;
                }else if (lines[i] == "[COLOR]") {
                    readType = e_objType.LIGHT;//light = color
                    continue;
                } else if (lines[i] == "[MUSIC]") {
                    readType = e_objType.MUSIC;
                    continue;
                } else if (lines[i] == "[PROPS]") {
                    readType = e_objType.PROP;
                    continue;
                } else if (lines[i] == "[STAGE]") {
                    readType = e_objType.STAGE;
                    continue;
                } else if (lines[i] == "[FORGROUND]") {
                    readType = e_objType.FORGROUND;
                    continue;
                } else if (lines[i] == "[GLOBALLIGHT]") {
                    readType = e_objType.GLOBALLIGHT;
                    continue;
                } else if (lines[i] == "[DATA]") {
                    readType = e_objType.PLAYERSTART;//Playerstart = data
                    continue;
                }

                if (readType == e_objType.BORDER) {
                    errorcodes.Add("line " + i + " is in no subcategory: " + lines[i]);
                    continue;
                }

                //----- ----- interprate according to internal state ----- -----
                if (readPos == 1) {
                    switch (readType) {
                    case e_objType.BACKGROUND:
                        if(bgdLine == 1) {
                            tmpStgArr = lines[i].Split(';');
                            if (tmpStgArr.Length < 4) {
                                bgdLine = 0;
                            } else {
                                if (!float.TryParse(tmpStgArr[0], out tmpV4.x)) {
                                    errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                                    continue;
                                } else if (!float.TryParse(tmpStgArr[1], out tmpV4.y)) {
                                    errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                                    continue;
                                } else if (!float.TryParse(tmpStgArr[2], out tmpV4.z)) {
                                    errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                                    continue;
                                } else if (!float.TryParse(tmpStgArr[3], out tmpV4.w)) {
                                    errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                                    continue;
                                }
                                tmpBgd.killFeed = tmpV4;
                                backgrounds.Add(tmpBgd);
                            }
                        }
                        if(bgdLine == 0) {
                            tmpSprite = LoadSprite(dir, lines[i], 170);//background is less resolution than props
                            if (!tmpSprite) {
                                errorcodes.Add("line " + i + " is no valide sprite: " + lines[i]);
                                continue;
                            }
                            tmpBgd.background = tmpSprite;
                            bgdLine = 1;
                        }
                        break;
                    case e_objType.PROP:
                        if(propLine == 1) {
                            tmpStgArr = lines[i].Split(';');
                            if (tmpStgArr.Length < 2) {
                                propLine = 0;
                            } else if (!float.TryParse(tmpStgArr[0], out tmpV2.x)) {
                                errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                                continue;
                            } else if (!float.TryParse(tmpStgArr[1], out tmpV2.y)) {
                                errorcodes.Add("unable to convert second value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                                continue;
                            } else {
                                tmpV2list.Add(tmpV2);
                                if (i == lines.Length - 1 || lines[i+1].Split(';').Length < 2) {
                                    tmpProp.m_collider = tmpV2list.ToArray();
                                    props.Add(tmpProp);
                                }
                            }
                        }
                        if(propLine == 0) {
                            tmpSprite = LoadSprite(dir, lines[i]);
                            if (!tmpSprite) {
                                errorcodes.Add("line " + i + " is no valide sprite: " + lines[i]);
                                continue;
                            }
                            tmpProp.m_sprite = tmpSprite;
                            propLine = 1;
                            tmpV2list.Clear();
                        }
                        break;
                    case e_objType.STAGE:
                        tmpSprite = LoadSprite(dir, lines[i]);
                        if (!tmpSprite) {
                            errorcodes.Add("line " + i + " is no valide sprite: " + lines[i]);
                            continue;
                        }
                        stages.Add(tmpSprite);
                        break;
                    case e_objType.LIGHT://color
                        tmpStgArr = lines[i].Split(';');
                        if(tmpStgArr.Length < 3) {
                            errorcodes.Add("values in line " + i + "cant be casted to at leased 3 values: " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[0], out tmpColor.r)) {
                            errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[1], out tmpColor.g)) {
                            errorcodes.Add("unable to convert second value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[2], out tmpColor.b)) {
                            errorcodes.Add("unable to convert third value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        tmpColor.a = 1;

                        colors.Add(tmpColor);
                        break;
                    case e_objType.FORGROUND:
                        tmpSprite = LoadSprite(dir, lines[i]);
                        if (!tmpSprite) {
                            errorcodes.Add("line " + i + " is no valide sprite: " + lines[i]);
                            continue;
                        }
                        forgrounds.Add(tmpSprite);
                        break;
                    case e_objType.MUSIC: {
                        WWW form = new WWW("file:///" + dir + lines[i]);
                        while (!form.isDone) ;
                        if (form == null) {
                            errorcodes.Add("line " + i + " can't be read " + lines[i]);
                            continue;
                        }
                        if (form.error != null) {
                            errorcodes.Add("line " + i + ": " + form.error + " (" + lines[i] + ")");
                            continue;
                        }
                        tmpMusic = form.GetAudioClip();
                        if (!tmpMusic) {
                            errorcodes.Add("line " + i + " is no valide audio: " + lines[i]);
                            continue;
                        }
                        tmpMusic.name = lines[i];
                        musics.Add(tmpMusic);
                        break;
                    }
                    case e_objType.SIZE:
                        tmpStgArr = lines[i].Split(';');
                        if (tmpStgArr.Length < 2) {
                            errorcodes.Add("values in line " + i + "cant be casted to at leased 2 values: " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[0], out tmpV2.x)) {
                            errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[1], out tmpV2.y)) {
                            errorcodes.Add("unable to convert second value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }

                        sizes.Add(tmpV2);
                        break;
                    default:
                        break;
                    }
                } else {
                    switch (readType) {
                    case e_objType.BACKGROUND:
                        if (!int.TryParse(lines[i], out value.m_background)) {
                            errorcodes.Add("unable to convert" + lines[i] + " to float in line " + i + " and will be set to default value 0");
                            continue;
                        }
                        readType = e_objType.BORDER;
                        break;
                    case e_objType.PLAYERSTART://data
                        tmpStgArr = lines[i].Split(';');
                        if (tmpStgArr.Length < 7) {
                            errorcodes.Add("values in line " + i + "cant be casted to at leased 7 values: " + lines[i]);
                            continue;
                        }
                        if (!Enum.TryParse<e_objType>(tmpStgArr[0], out tmpMData.type)) {
                            errorcodes.Add("unable to convert first value " + tmpStgArr[0] + " to e_objType enumerator in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!int.TryParse(tmpStgArr[1], out tmpMData.index)) {
                            errorcodes.Add("unable to convert second value " + tmpStgArr[0] + " to int in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[2], out tmpMData.position.x)) {
                            errorcodes.Add("unable to convert third value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[3], out tmpMData.position.y)) {
                            errorcodes.Add("unable to convert forth value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[4], out tmpMData.rotation)) {
                            errorcodes.Add("unable to convert fith value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[5], out tmpMData.scale.x)) {
                            errorcodes.Add("unable to convert sixth value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }
                        if (!float.TryParse(tmpStgArr[6], out tmpMData.scale.y)) {
                            errorcodes.Add("unable to convert seventh value " + tmpStgArr[0] + " to float in line " + i + ": " + lines[i]);
                            continue;
                        }

                        datas.Add(tmpMData);

                        break;
                    case e_objType.GLOBALLIGHT:
                        if (!int.TryParse(lines[i], out value.m_globalLight)) {
                            errorcodes.Add("unable to convert" + lines[i] + " to float in line " + i + " and will be set to default value 0");
                            continue;
                        }
                        readType = e_objType.BORDER;
                        break;
                    case e_objType.MUSIC:
                        if (!int.TryParse(lines[i], out value.m_music)) {
                            errorcodes.Add("unable to convert" + lines[i] + " to float in line " + i + " and will be set to default value 0");
                            continue;
                        }
                        readType = e_objType.BORDER;
                        break;
                    case e_objType.SIZE:
                        if(!int.TryParse(lines[i], out value.m_size)) {
                            errorcodes.Add("unable to convert" + lines[i] + " to float in line " + i + " and will be set to default value 0");
                            continue;
                        }
                        readType = e_objType.BORDER;
                        break;
                    default:
                        break;
                    }
                }
            }
            
            value.p_background = backgrounds.ToArray();
            value.p_colors = colors.ToArray();
            value.p_forground = forgrounds.ToArray();
            value.p_music = musics.ToArray();
            value.p_props = props.ToArray();
            value.p_size = sizes.ToArray();
            value.p_stage = stages.ToArray();
            value.m_data = datas.ToArray();

            //TODO: validate datas;

            for (int i = 0; i < errorcodes.Count; i++) {
                Debug.Log(errorcodes[i] + Environment.NewLine);
            }

            return value;
        }

        static Sprite LoadSprite(string path, string name, int PPU = 512) {
            WWW tmp = new WWW("file:///" + path + name);
            while (!tmp.isDone) ;
            if (tmp == null) {
                return null;
            }
            if (tmp.error != null) {
                Debug.Log(tmp.error);
                return null;
            }
            Texture2D tmpTex = tmp.texture;
            if (!tmpTex)
                return null;
            Sprite value = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(tmpTex.width/2, tmpTex.height/2), PPU);
            if (!value)
                return null;
            value.name = name;
            return value;
        }

        public static void SaveMap(MapDATA data) {
            string value = "";
            value += "#Resources" + Environment.NewLine;

            value += "[SIZE]" + Environment.NewLine;
            for(int i = 0; i < data.p_size.Length; i++) {
                value += data.p_size[i].x + ";" + data.p_size[i].y + Environment.NewLine;
            }
            value += "[BACKGROUND]" + Environment.NewLine;
            for (int i = 0; i < data.p_background.Length; i++) {
                value += data.p_background[i].background.name + ".png" + Environment.NewLine;
                value += data.p_background[i].killFeed.x + ";" + data.p_background[i].killFeed.y + ";" + data.p_background[i].killFeed.z + ";" + data.p_background[i].killFeed.w;
            }
            value += "[COLOR]" + Environment.NewLine;
            for (int i = 0; i < data.p_colors.Length; i++) {
                value += data.p_colors[i].r + ";" + data.p_colors[i].g + ";" + data.p_colors[i].b + Environment.NewLine;
            }
            value += "[MUSIC]" + Environment.NewLine;
            for (int i = 0; i < data.p_music.Length; i++) {
                value += data.p_music[i].name + ".ogg" + Environment.NewLine;
            }
            value += "[PROPS]" + Environment.NewLine;
            for (int i = 0; i < data.p_props.Length; i++) {
                value += data.p_props[i].m_sprite.name + ".png" + Environment.NewLine;
                for(int j = 0; j < data.p_props[i].m_collider.Length; j++) {
                    value += data.p_props[i].m_collider[j].x + ";" + data.p_props[i].m_collider[j].y + Environment.NewLine;
                }
            }
            value += "[STAGE]" + Environment.NewLine;
            for (int i = 0; i < data.p_stage.Length; i++) {
                value += data.p_stage[i].name + ".png" + Environment.NewLine;
            }
            value += "[FORGROUND]" + Environment.NewLine;
            for (int i = 0; i < data.p_stage.Length; i++) {
                value += data.p_stage[i].name + ".png" + Environment.NewLine;
            }

            value += "#Data" + Environment.NewLine;

            value += "[SIZE]" + Environment.NewLine + data.m_size + Environment.NewLine;
            value += "[BACKGROUND]" + Environment.NewLine + data.m_background + Environment.NewLine;
            value += "[GLOBALLIGHT]" + Environment.NewLine + data.m_globalLight + Environment.NewLine;
            value += "[MUSIC]" + Environment.NewLine + data.m_music + Environment.NewLine;
            value += "[DATA]";
            for (int i = 0; i < data.m_data.Length; i++) {
                value += Environment.NewLine + data.m_data[i].ToString();
            }

            Directory.CreateDirectory(StringCollection.MAPPARH + data.name);
            StreamWriter saveFile = File.CreateText(StringCollection.MAPPARH + data.name + "/" + data.name + ".map");
            saveFile.Write(value);
            saveFile.Close();
        }

        public MapDATA Copy() {

            MapDATA value = new MapDATA();

            value.hideFlags = this.hideFlags;
            value.name = this.name;

            value.m_background = this.m_background;
            value.m_data = new d_mapData[this.m_data.Length];
            for(int i = 0; i < this.m_data.Length; i++) {
                value.m_data[i] = this.m_data[i];
            }
            value.m_globalLight = this.m_globalLight;
            value.m_music = this.m_music;
            value.m_size = this.m_size;

            value.p_background = new d_mapBackground[this.p_background.Length];
            for(int i = 0; i < this.p_background.Length; i++) {
                value.p_background[i] = this.p_background[i];
            }
            value.p_colors = new Color[this.p_colors.Length];
            for(int i = 0; i < this.p_colors.Length; i++) {
                value.p_colors[i] = this.p_colors[i];
            }
            value.p_forground = new Sprite[this.p_forground.Length];
            for(int i = 0; i < this.p_forground.Length; i++) {
                value.p_forground[i] = this.p_forground[i];
            }
            value.p_music = new AudioClip[this.p_music.Length];
            for(int i = 0; i < this.p_music.Length; i++) {
                value.p_music[i] = this.p_music[i];
            }
            value.p_ballSpawn = this.p_ballSpawn;
            value.p_props = new d_prop[this.p_props.Length];
            for(int i = 0; i < this.p_props.Length; i++) {
                value.p_props[i] = this.p_props[i];
            }
            value.p_size = new Vector2[this.p_size.Length];
            for(int i = 0; i < this.p_size.Length; i++) {
                value.p_size[i] = this.p_size[i];
            }
            value.p_stage = new Sprite[this.p_stage.Length];
            for(int i = 0; i < this.p_stage.Length; i++) {
                value.p_stage[i] = this.p_stage[i];
            }

            return value;
        }

        public Vector2[] p_size;
        public d_mapBackground[] p_background;
        public Color[] p_colors;
        public AudioClip[] p_music;
        public Vector2 p_ballSpawn;//toBeSaved

        public d_prop[] p_props;
        public Sprite[] p_stage;
        public Sprite[] p_forground;

        public int m_size;
        public int m_background;
        public int m_globalLight;
        public int m_music;
        

        public d_mapData[] m_data;
        
    }
}