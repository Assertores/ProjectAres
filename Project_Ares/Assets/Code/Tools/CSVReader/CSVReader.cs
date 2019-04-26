using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsserTOOLres {
    public static class CSVReader {

        #region ===== ===== DATA ===== =====

        //map<path, IDKey>
        static Dictionary<string, string> s_iDFromFile = new Dictionary<string, string>();

        //if i get the Generic funktions to work ===== ===== ----- -----
        //static Dictionary<string, Dictionary<string, List<object>>> Library = new Dictionary<string, Dictionary<string, List<object>>>();//map<path, map<ID,List<Values>>>
        static Dictionary<string, Dictionary<string, List<string>>> s_library = new Dictionary<string, Dictionary<string, List<string>>>();//map<path, map<ID,List<Values>>>

        #endregion
        #region ===== ===== CORE ===== =====

        /// <summary>
        /// !!!USE API!!!
        /// loads a CSV file.;
        /// also will save it to an internal memory.;
        /// if the file is already loaded it will return the internal memory.
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="seperator">the seperator of the columns. default value ';'</param>
        /// <returns>a dictionary as id and value lists</returns>
        //if i get the Generic funktions to work ===== ===== ----- -----
        //public static Dictionary<string, List<object>> LoadCSV(string path, char seperator = ';') {
        public static Dictionary<string, List<string>> LoadCSV(string path, char seperator = ';') {
            if (s_library.ContainsKey(path)) {
                return s_library[path];
            }

            return ReloadCSV(path, seperator);
        }

        /// <summary>
        /// !!!USE API!!!
        /// loads a CSV event if it already was loaded and overwrites it in the Library
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="seperator">the seperator of the columns default value ';'</param>
        /// <returns>a dictionary as id and value lists</returns>
        //if i get the Generic funktions to work ===== ===== ----- -----
        //public static Dictionary<string, List<object>> ReloadCSV(string path, char seperator = ';') {
        public static Dictionary<string, List<string>> ReloadCSV(string path, char seperator = ';') {

            //if i get the Generic funktions to work ===== ===== ----- -----
            //Dictionary<string, List<object>> value = new Dictionary<string, List<object>>();
            Dictionary<string, List<string>> value = new Dictionary<string, List<string>>();

            string[] Lines;

#if UNITY_ANDROID
            WWW reader = new WWW(path);//TODO: make non Unity specific
            while (!reader.isDone) ;

            if(reader.error != "") {
                //if i get the Generic funktions to work ===== ===== ----- -----
                //value.Add("ERROR", new List<object>());
                value.Add("ERROR", new List<string>());
                return value;
            }
            Lines = reader.text.Split(System.Environment.NewLine[0]);
#else
            if (!File.Exists(path)) {
                //if i get the Generic funktions to work ===== ===== ----- -----
                //value.Add("ERROR", new List<object>());
                value.Add("ERROR", new List<string>());
                return value;
            }
            Lines = File.ReadAllLines(path);
#endif

            for (int i = 0; i < Lines.Length; i++) {
                string[] LineValues = Lines[i].Trim().Split(seperator);
                if (i == 0) {//TODO: identifier not in first column
                    if (s_iDFromFile.ContainsKey(path)) {
                        s_iDFromFile.Remove(path);
                    }
                    s_iDFromFile.Add(path, LineValues[0]);
                }
                //if i get the Generic funktions to work ===== ===== ----- -----
                //List<object> temp = new List<object>();
                List<string> temp = new List<string>();
                for (int j = 1; j < LineValues.Length; j++) {//TODO: identifier not in first column
                    temp.Add(LineValues[j]);
                }
                value.Add(LineValues[0], temp);//TODO: identifier not in first column
            }

            if (s_library.ContainsKey(path)) {
                s_library.Remove(path);
            }
            s_library.Add(path, value);

            return value;
        }

        //----- ----- From StreamingAsset ----- -----

        /// <summary>
        /// !!!USE API!!!
        /// loads a CSV file.;
        /// also will save it to an internal memory.;
        /// if the file is already loaded it will return the internal memory.
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="seperator">the seperator of the columns default value ';'</param>
        /// <returns>a dictionary as id and value lists</returns>
        //if i get the Generic funktions to work ===== ===== ----- -----
        //public static Dictionary<string, List<object>> LoadCSVFromStreamingAsset (string path, char seperator = ';') {
        public static Dictionary<string, List<string>> LoadCSVFromStreamingAsset(string path, char seperator = ';') {
            return LoadCSV(Application.streamingAssetsPath + "/" + path, seperator);
        }

        /// <summary>
        /// !!!USE API!!!
        /// loads a CSV event if it already was loaded and overwrites it in the Library
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="seperator">the seperator of the columns default value ';'</param>
        /// <returns>a dictionary as id and value lists</returns>
        //if i get the Generic funktions to work ===== ===== ----- -----
        //public static Dictionary<string, List<object>> ReloadCSVFromStreamingAsset(string path, char seperator = ';') {
        public static Dictionary<string, List<string>> ReloadCSVFromStreamingAsset(string path, char seperator = ';') {
            return ReloadCSV(Application.streamingAssetsPath + "/" + path, seperator);
        }

        #endregion
        #region ===== ===== Standard API ===== ===== //Dose not Work!
        //if i get the Generic funktions to work ===== ===== ----- -----
        /*
        public static bool getValue<T>(out T value, string path, string id, int columnIndex = 0, bool save = true) {
            if (save) {
                value = default(T);
                if (!Library.ContainsKey(path)) {
                    if (LoadCSV(path).ContainsKey("ERROR")) {
                        return false;
                    }
                }
                if (!Library[path].ContainsKey(id)) {
                    return false;
                }
                if (columnIndex >= Library[path][id].Count) {
                    return false;
                }
            }

            value = (T)Library[path][id][columnIndex];//!!!! Dose not work
            return true;
        }

        public static bool getValue<T>(out T value, string path, string id, string columnName, bool save = true, string headerID = "") {
            if (save) {
                value = default(T);
                if (!Library.ContainsKey(path)) {
                    if (LoadCSV(path).ContainsKey("ERROR")) {
                        return false;
                    }
                }
                if (!Library[path].ContainsKey(id)) {
                    return false;
                }
                if (Library[path][iDFromFile[path]].IndexOf(columnName) < 0) {
                    return false;
                }
                if (Library[path][(headerID == "") ? iDFromFile[path] : headerID].IndexOf(columnName) >= Library[path][id].Count) {
                    return false;
                }
            }

            return getValue(out value, path, id, Library[path][(headerID == "") ? iDFromFile[path] : headerID].IndexOf(columnName), false);
        }

        //----- ----- From StreamingAsset ----- -----

        public static bool getValueFromStreamingAsset<T>(out T value, string path, string id, int columnIndex = 0, bool save = true) {
            return getValue(out value, Application.streamingAssetsPath + "/" + path, id, columnIndex, save);
        }


        public static bool getValueFromStreamingAsset<T>(out T value, string path, string id, string columnName, bool save = true, string headerID = "") {
            return getValue(out value, Application.streamingAssetsPath + "/" + path, id, columnName, save, headerID);
        }
        */
        #endregion
        #region ===== ===== Old API ===== =====
        #region strings

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as string or as Error message starting with "ERROR:"</returns>
        public static string GetValueAsString(string path, string id, int columnIndex = 0, bool save = true) { //if the first column is the ID: column 0 will give you the next column after the ID. TL;DR: ID Column will be ignored
            if (save) {
                if (!s_library.ContainsKey(path)) {
                    if (LoadCSV(path).ContainsKey("ERROR")) {
                        return "ERROR: file " + path + " not found";
                    }
                }
                if (!s_library[path].ContainsKey(id)) {
                    return "ERROR: no " + id + " found in " + path;
                }
                if (columnIndex >= s_library[path][id].Count) {
                    return "ERROR: " + id + " in " + path + " has only " + s_library[path][id].Count + " columns but you wanted column index " + columnIndex;
                }
            }

            return (string)s_library[path][id][columnIndex];
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">header name</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to specify it. default value = "" which uses the first row as identifier</param>
        /// <returns>the value as string or as Errormessage starting with "ERROR:"</returns>
        public static string GetValueAsString(string path, string id, string columnName, bool save = true, string headerID = "") {
            if (save) {
                if (!s_library.ContainsKey(path)) {
                    if (LoadCSV(path).ContainsKey("ERROR")) {
                        return "ERROR: file " + path + " not found";
                    }
                }
                if (!s_library[path].ContainsKey(id)) {
                    return "ERROR: no " + id + " found in " + path;
                }
                if (s_library[path][s_iDFromFile[path]].IndexOf(columnName) < 0) {
                    return "ERROR: " + columnName + " not found in " + path;
                }
                if (s_library[path][(headerID == "") ? s_iDFromFile[path] : headerID].IndexOf(columnName) >= s_library[path][id].Count) {
                    return "ERROR: " + id + " in " + path + " has only " + s_library[path][id].Count + " columns but you wanted column index " + s_library[path][(headerID == "") ? s_iDFromFile[path] : headerID].IndexOf(columnName);
                }
            }

            return GetValueAsString(path, id, s_library[path][(headerID == "") ? s_iDFromFile[path] : headerID].IndexOf(columnName), false);
        }

        //----- ----- From StreamingAsset ----- -----

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as string or as Errormessage starting with "ERROR:"</returns>
        public static string GetValueAsStringFromStreamingAsset(string path, string id, int columnIndex = 0, bool save = true) {
            return GetValueAsString(Application.streamingAssetsPath + "/" + path, id, columnIndex, save);
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">header name</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to specify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as string or as Errormessage starting with "ERROR:"</returns>
        public static string GetValueAsStringFromStreamingAsset(string path, string id, string columnName, bool save = true, string headerID = "") {
            return GetValueAsString(Application.streamingAssetsPath + "/" + path, id, columnName, save, headerID);
        }

        #endregion
        #region ints

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as int or the minimum int value (-2147483648)</returns>
        public static int GetValueAsInt(string path, string id, int columnIndex = 0, bool save = true) {
            int value;
            if (!System.Int32.TryParse(GetValueAsString(path, id, columnIndex, save), out value)) {
                return System.Int32.MinValue;
            }

            return value;
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">header name</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to spesify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as int or the minimum int value (-2147483648)</returns>
        public static int GetValueAsInt(string path, string id, string columnName, bool save = true, string headerID = "") {
            int value;
            if (!System.Int32.TryParse(GetValueAsString(path, id, columnName, save, headerID), out value)) {
                return System.Int32.MinValue;
            }

            return value;
        }

        //----- ----- From StreamingAsset ----- -----

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as int or the minimum int value (-2147483648)</returns>
        public static int GetValueAsIntFromStreamingAsset(string path, string id, int columnIndex = 0, bool save = true) {
            return GetValueAsInt(Application.streamingAssetsPath + "/" + path, id, columnIndex, save);
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">headername</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to spesify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as int or the minimum int value (-2147483648)</returns>
        public static int GetValueAsIntFromStreamingAsset(string path, string id, string columnName, bool save = true, string headerID = "") {
            return GetValueAsInt(Application.streamingAssetsPath + "/" + path, id, columnName, save, headerID);
        }

        #endregion
        #region floats

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as float or the minimum float value (-3.40282347E+38)</returns>
        public static float GetValueAsFloat(string path, string id, int columnIndex = 0, bool save = true) {
            float value;
            if (!float.TryParse(GetValueAsString(path, id, columnIndex, save), out value)) {
                return float.MinValue;
            }
            return value;
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">headername</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to spesify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as float or the minimum float value (-3.40282347E+38)</returns>
        public static float GetValueAsFloat(string path, string id, string columnName, bool save = true, string headerID = "") {
            float value;
            if (!float.TryParse(GetValueAsString(path, id, columnName, save, headerID), out value)) {
                return float.MinValue;
            }

            return value;
        }

        //----- ----- From StreamingAsset ----- -----

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as float or the minimum float value (-3.40282347E+38)</returns>
        public static float GetValueAsFloatFromStreamingAsset(string path, string id, int columnIndex = 0, bool save = true) {
            return GetValueAsFloat(Application.streamingAssetsPath + "/" + path, id, columnIndex, save);
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">headername</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to spesify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as float or the minimum float value (-3.40282347E+38)</returns>
        public static float GetValueAsFloatFromStreamingAsset(string path, string id, string columnName, bool save = true, string headerID = "") {
            return GetValueAsFloat(Application.streamingAssetsPath + "/" + path, id, columnName, save, headerID);
        }

        #endregion
        #region bools

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as bool or false(sorry)</returns>
        public static bool GetValueAsBool(string path, string id, int columnIndex = 0, bool save = true) {
            bool value;
            if (!bool.TryParse(GetValueAsString(path, id, columnIndex, save), out value)) {
                return false;
            }

            return value;
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the whole path to the csv file (with ending)</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">headername</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to spesify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as bool or false(sorry)</returns>
        public static bool GetValueAsBool(string path, string id, string columnName, bool save = true, string headerID = "") {
            bool value;
            if (!bool.TryParse(GetValueAsString(path, id, columnName, save, headerID), out value)) {
                return false;
            }

            return value;
        }

        //----- ----- From StreamingAsset ----- -----

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnIndex">value index </param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <returns>the value as bool or false(sorry)</returns>
        public static bool GetValueAsBoolFromStreamingAsset(string path, string id, int columnIndex = 0, bool save = true) {
            return GetValueAsBool(Application.streamingAssetsPath + "/" + path, id, columnIndex, save);
        }

        /// <summary>
        /// API;
        /// accesses a specific value of a given key
        /// </summary>
        /// <param name="path">the path to the csv file (with ending) relative to the StreamingAssets Folder</param>
        /// <param name="id">identifier for which you want a value from</param>
        /// <param name="columnName">headername</param>
        /// <param name="save">if false all checks and safety measures are deactivated. default value = true</param>
        /// <param name="headerID">identifier for the header if you want to spesify it. default value = "" whitch uses the first row as identifier</param>
        /// <returns>the value as bool or false(sorry)</returns>
        public static bool GetValueAsBoolFromStreamingAsset(string path, string id, string columnName, bool save = true, string headerID = "") {
            return GetValueAsBool(Application.streamingAssetsPath + "/" + path, id, columnName, save, headerID);
        }

        #endregion
        #endregion
    }
}