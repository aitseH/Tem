using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;


using Tem;

namespace Tem {

    public class TemEditorInspector : Editor {

        protected static bool styleDefined = false;
        protected static GUIStyle headerStyle;
        protected static GUIStyle foldoutStyle;
        protected static GUIStyle conflictStyle;
        protected static GUIStyle toggleHeaderStyle;

        protected GUIContent cont;
        protected GUIContent contN=GUIContent.none;
        protected GUIContent[] contL;

        public override void OnInspectorGUI() {

            DefineStyle();
        }

        protected static void DefineStyle() {
            if (styleDefined)
                return;

            styleDefined = true;

            headerStyle = new GUIStyle("Label");
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.black;

            toggleHeaderStyle = new GUIStyle("Toggle");
            headerStyle.fontStyle = FontStyle.Bold;

            foldoutStyle = new GUIStyle("Foldout");
            foldoutStyle.fontStyle = FontStyle.Bold;
            foldoutStyle.normal.textColor = Color.black;

            conflictStyle = new GUIStyle("Label");
            conflictStyle.normal.textColor = Color.red;

        }



		protected static EffectDB effectDB;
		protected static List<int> effectIDList=new List<int>();
		protected static string[] effectLabel;
		protected static void LoadEffect(){ TemEditor.LoadEffect(); }
		protected static void UpdateLabel_Effect(){ TemEditor.UpdateLabel_Effect(); }
		public static void SetEffectDB(EffectDB db, List<int> IDList, string[] label){
			effectDB=db;
			effectIDList=IDList;
			effectLabel=label;
		}

        protected static WeaponDB weaponDB;
		protected static List<int> weaponIDList=new List<int>();
		protected static string[] weaponLabel;
		protected static void LoadWeapon(){ TemEditor.LoadWeapon(); }
		protected static void UpdateLabel_Weapon(){ TemEditor.UpdateLabel_Weapon(); }
		public static void SetWeaponDB(WeaponDB db, List<int> IDList, string[] label){
			weaponDB=db;
			weaponIDList=IDList;
			weaponLabel=label;
		}
    }
}