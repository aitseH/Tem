//class containing all the stats about an attack

using UnityEngine;
using System.Collections;

using Tem;

namespace Tem {

    [System.Serializable]
    public class AttackStats {

        [Header("Attributes")]
        public int damageType=0;
        public float damageMin = 0;
        public float damageMax = 0;

        public float aoeRadius = 0;
        public bool diminishingAOE = true;

        public float critChance = 0;
        public float critMultiplier = 2;
        [Header("Physis")]
        public float impactForce = 0;

        public float explosionRadius = 0;
        public float explosionForce = 0;

        [Header("Effectd")]
        public int effectID = -1;
        public int effectIdx = 001;

        public void Init() {
            
        }


        public AttackStats Clone() {
            
            AttackStats stats = new AttackStats();

            stats.damageMin=damageMin;
            stats.damageMax=damageMax;

            stats.aoeRadius=aoeRadius;
            stats.diminishingAOE=diminishingAOE;

            stats.critChance=critChance;
            stats.critMultiplier=critMultiplier;

            stats.impactForce=impactForce;
            stats.explosionRadius=explosionRadius;
            stats.explosionForce=explosionForce;

            stats.effectID=effectID;
            stats.effectIdx=effectIdx;

            return stats;
        }
    }
}