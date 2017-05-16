using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Tem;

namespace Tem
{

    public enum _CollectType { Self };

	[RequireComponent(typeof(Collider))]
    public class Collectible : MonoBehaviour
    {

        [HideInInspector] public int ID = -1;
        public Sprite icon;
        public string collectibleName = "Collectible";
        public string desp = "";
        public _CollectType type = _CollectType.Self;

        [Header("Self")]
        public int addHP = 0;
        public int effectID = -1;
        [HideInInspector] private int effectIDx = -1;


		[Header("Common")]
		public bool selfDestruct = false;
		public float selfDestructDuration = 5f;
		public bool blinkBeforeDestroy;
		public float blinkDuration;
		public GameObject blinkObj;
		public Transform GetBlinkObjT(){ return blinkObj==null ? null : blinkObj.transform; }
		

        void Awake()
        {
			gameObject.GetComponent<Collider>().isTrigger=true;
			
			effectIDx = EffectDB.GetEffectIndex(effectID);

        }

		void OnEnalbe() {
			if(selfDestruct) {
				ObjectPool.Unspawn(gameObject, selfDestructDuration);

				if(blinkBeforeDestroy && blinkObj != null) StartCoroutine(Blink());
			}
		}

		IEnumerator Blink(){
			float delay=Mathf.Max(0, selfDestructDuration-blinkDuration);
			yield return new WaitForSeconds(delay);
			
			while(true){
				blinkObj.SetActive(false);
				yield return new WaitForSeconds(0.15f);
				blinkObj.SetActive(true);
				yield return new WaitForSeconds(0.35f);
			}
		}

		void OnTriggerEnter(Collider col) {
			if(col.gameObject.tag != "Player") return;

			if(type == _CollectType.Self) {
				ApplyEffectSelf(col.gameObject);
			}
		}

		void ApplyEffectSelf(GameObject obj) {
			if(addHP>0) {
				obj.SendMessage("GainHP", addHP);
			}
			if(effectIDx>=0) {
				obj.SendMessage("ApplyEffect", EffectDB.CloneItem(effectIDx));
			}
		}
		
    }
}

