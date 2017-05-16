using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using Tem;

namespace Tem {
	public class CollectibleDB : MonoBehaviour {

		public List<Collectible> collectibleList = new List<Collectible>();

		public static CollectibleDB LoadDB() {
			GameObject obj = Resources.Load("DB/DB_Collectible", typeof(GameObject)) as GameObject;

			#if UNITY_EDITOR
				if(obj==null) obj=CreatePrefab();
			#endif

			return obj.GetComponent<CollectibleDB>();
		}

		public static List<Collectible> Load() {
			GameObject obj=Resources.Load("DB_TDSTK/DB_Collectible", typeof(GameObject)) as GameObject;
			
			#if UNITY_EDITOR
				if(obj==null) obj=CreatePrefab();
			#endif
			
			CollectibleDB instance=obj.GetComponent<CollectibleDB>();
			return instance.collectibleList;
		}

		#if UNITY_EDITOR
			private static GameObject CreatePrefab(){
				GameObject obj=new GameObject();
				obj.AddComponent<CollectibleDB>();
				GameObject prefab=PrefabUtility.CreatePrefab("Assets/Tem/Resources/DB/DB_Collectible.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
				DestroyImmediate(obj);
				AssetDatabase.Refresh ();
				return prefab;
			}
		#endif
	}
}
