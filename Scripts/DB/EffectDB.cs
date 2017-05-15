using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;

using Tem;

namespace Tem {

    public class EffectDB : MonoBehaviour {

        private static bool initiated = false;

        private static List<Effect> allEffectList = new List<Effect>();

        public static void Init(){
            if(initiated)return;
            initiated = true;

            GameObject obj = Resources.Load("DB/DB_Effect", typeof(GameObject)) as GameObject;
            allEffectList = new List<Effect>(obj.GetComponent<EffectDB>().effectList);
        }


        public List<Effect> effectList = new List<Effect>();

        public static EffectDB LoadDB() {
            GameObject obj = Resources.Load("DB/DB_Effect", typeof(GameObject)) as GameObject;

            #if UNITY_EDITOR
				if(obj==null) obj=CreatePrefab();
			#endif
			
			return obj.GetComponent<EffectDB>();
        }



        #if UNITY_EDITOR
			private static GameObject CreatePrefab(){
				GameObject obj=new GameObject();
				obj.AddComponent<EffectDB>();
				GameObject prefab=PrefabUtility.CreatePrefab("Assets/Tem/Resources/DB/DB_Effect.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
				DestroyImmediate(obj);
				AssetDatabase.Refresh ();
				return prefab;
			}
		#endif

    }
}