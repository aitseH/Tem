using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;

using Tem;

namespace Tem {

    public class WeaponDB : MonoBehaviour {

        private static bool initiated = false;
        private static List<Weapon> allweaponList=new List<Weapon>();
        public static void Init() {
            if (initiated)
                return;
            initiated = true;

            GameObject obj = Resources.Load("DB/DB_Weapon", typeof(GameObject)) as GameObject;
            allweaponList = new List<Weapon>(obj.GetComponent<WeaponDB>().weaponList);
        }

        public List<Weapon> weaponList=new List<Weapon>();

        public static WeaponDB LoadDB() {
            GameObject obj = Resources.Load("DB/DB_Weapon", typeof(GameObject)) as GameObject;

            #if UNITY_EDITOR
            if(obj==null) obj= CreatePrefab();
            #endif

            return obj.GetComponent<WeaponDB>();
        }

        public static List<Weapon> Load() {
            GameObject obj = Resources.Load("DB/DB_Weapon", typeof(GameObject)) as GameObject;
            #if UNITY_EDITOR
            if(obj==null) obj= CreatePrefab();
            #endif

            WeaponDB instance = obj.GetComponent<WeaponDB>();
            return instance.weaponList;
        }


        #if UNITY_EDITOR
        private static GameObject CreatePrefab() {
            GameObject obj = new GameObject();
            obj.AddComponent<Weapon>();
            GameObject prefab = PrefabUtility.CreatePrefab("Assets/Tem/Resources/DB/DB_Weapon.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
            AssetDatabase.Refresh();
            return prefab;
        }
        #endif
    }
}

