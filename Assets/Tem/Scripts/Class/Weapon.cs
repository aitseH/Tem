using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Tem;

namespace Tem {

    public class Weapon : MonoBehaviour {

        [HideInInspector] public int ID=0;
        public Sprite icon;
        public string weaponName="Weapon";
        public string desp="";

        [Space(10)]
        public GameObject shootObject;
        public List<Transform> shootPointList=new List<Transform>();
        public float shootPointDelay=0.05f;

        [Space(10)]
        public bool continousFire=true;

        [Header("Base Stats")]
        public float range=20;
        public float cooldown=0.15f;
        [HideInInspector] public float currentCD=0.25f;

        public int clipSize=30;
        public int currentClip=30;

        public int ammoCap=300;
        public int ammo=300;

        public float reloadDuration=2;
        [HideInInspector] public float currentReload=0;

        public float recoilMagnitude=.2f;
        [HideInInspector] public float recoil=0;

        public float recoilCamShake=0;

        public int spread=0;
        public float spreadAngle=15;

        public AttackStats aStats = new AttackStats();
        public AttackStats CloneAttackStats() { return aStats.Clone();}

        [Header("Audio")]
        public AudioClip shootSFX;
        public AudioClip reloadSFX;

        [HideInInspector] public bool temporary = false;

    }
}