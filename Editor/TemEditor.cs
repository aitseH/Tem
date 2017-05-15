using UnityEngine;
using UnityEditor;

using System;

using System.Collections;
using System.Collections.Generic;

using Tem;

namespace Tem {

    public class TemEditor {


        public static bool IsPrefab(GameObject obj){
            return obj==null ? false : PrefabUtility.GetPrefabType(obj)==PrefabType.Prefab;
        }

        public static bool dirty =false;



        protected static EffectDB effectDB;
        protected static List<int> effecteIDList = new List<int>();
        protected static string[] effectLabel;
        public static void LoadEffect(){
            effectDB = EffectDB.LoadDB();

            for(int i=0; i<effectDB.effectList.Count; i++) {
                if(effectDB.effectList[i] !=null) {
                    effecteIDList.Add(effectDB.effectList[i].ID);
                }else {
                    effectDB.effectList.RemoveAt(i);
                    i-=1;
                }
            }
            UpdateLabel_Effect();

            TemEditorWindow.SetEffectDB(effectDB, effecteIDList, effectLabel);
            TemEditorInspector.SetEffectDB(effectDB, effecteIDList, effectLabel);

        }

        public static void UpdateLabel_Effect() {
            effectLabel = new string[effectDB.effectList.Count+1];
            effectLabel[0] = "Unassigned";
            for(int i =0; i<effectDB.effectList.Count; i++){
                string name=effectDB.effectList[i].name;
                if(name=="") name="unamed";
                while(Array.IndexOf(effectLabel, name) >=0) name+="_";
                effectLabel[i+1]=name;
            }

            TemEditorWindow.SetEffectDB(effectDB, effecteIDList, effectLabel);
            TemEditorInspector.SetEffectDB(effectDB, effecteIDList, effectLabel);

            dirty = !dirty;
        }



        public static WeaponDB weaponDB;
        public static List<int> weaponIDList = new List<int>();
        public static string[] weaponLabel;
        public static void LoadWeapon() {
            weaponDB = WeaponDB.LoadDB();

            for(int i=0; i< weaponDB.weaponList.Count; i++) {
                if(weaponDB.weaponList[i]!= null) {
                    weaponIDList.Add(weaponDB.weaponList[i].ID);
                }
                else {
                    weaponDB.weaponList.RemoveAt(i);
                    i -= 1;
                }
            }

            UpdateLabel_Weapon();

            TemEditorWindow.SetWeaponDB(weaponDB, weaponIDList, weaponLabel);
            TemEditorInspector.SetWeaponDB(weaponDB, weaponIDList, weaponLabel);
        }

        public static void UpdateLabel_Weapon() {
            weaponLabel = new string[weaponDB.weaponList.Count + 1];
            weaponLabel[0] = "Unassigned";

            for(int i=0; i<weaponDB.weaponList.Count; i++) {
                string name = weaponDB.weaponList[i].weaponName;
                if (name == "")
                    name = "unamed";
                while (Array.IndexOf(weaponLabel, name) >= 0)
                    name += '_';
                weaponLabel[i + 1] = name;
            }

            TemEditorWindow.SetWeaponDB(weaponDB, weaponIDList, weaponLabel);

            dirty = !dirty;
        }
    }
}