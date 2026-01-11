using System;
using MelonLoader;
using UnityEngine;

namespace UKAIW
{
    public class EnemyBloodFuel : ModoBehaviour
    {
        public override void ModoFixedUpdate()
        {
        }

        public override void ModoLateUpdate()
        {
        }

        public override void ModoOnDestroy()
        {
            Player.PreHurt -= PlayerPreHurt;
        }

        public override void ModoOnDisable()
        {
        }

        public override void ModoOnEnable()
        {
        }

        public override void ModoUpdate()
        {
        }

        public override void OnClonedFrom(ModoBehaviour ClonedFrom)
        {
        }

        public override void OnModRemoved()
        {
        }

        protected override void ModoAwake()
        {
        }

        protected override void ModoStart()
        {
            Player.PreHurt += PlayerPreHurt;
        }

        private void PlayerPreHurt(NewMovement player, int damage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        {
            if (Cheats.IsCheatEnabled(Cheats.BloodFueledEnemies))
            {
                var playerPos = player.rb.transform.position;
                var pos = Transform.position;

                var dist = Vector3.Distance(playerPos, pos);

                float maxDist = damage / Options.BloodFuelEnemiesDistDivisor;

                float normalizedDist = 1.0f - Mathf.Min(1.0f, dist / maxDist);
                var eadd = (EnemyAdditions)Mono;
                var eid = eadd.Eid;
                float heal = (damage * normalizedDist);
                heal *= Options.BloodFuelEnemiesHealScalar;

                if (eid.zombie != null)
                {
                    eid.zombie.health = Mathf.Min(eadd.PrefabMod.Prefab.GetComponent<EnemyIdentifier>().zombie.health, eid.zombie.health + heal);
                }
                else if (eid.drone != null)
                {
                    eid.drone.health = Mathf.Min(eadd.PrefabMod.Prefab.GetComponent<EnemyIdentifier>().drone.health, eid.drone.health + heal);
                }
                else if (eid.machine != null)
                {
                    eid.machine.health = Mathf.Min(eadd.PrefabMod.Prefab.GetComponent<EnemyIdentifier>().machine.health, eid.machine.health + heal);
                }
                else if (eid.statue != null)
                {
                    eid.statue.health = Mathf.Min(eadd.PrefabMod.Prefab.GetComponent<EnemyIdentifier>().statue.health, eid.statue.health + heal);
                }
                else if (eid.spider != null)
                {
                    eid.spider.health = Mathf.Min(eadd.PrefabMod.Prefab.GetComponentInChildren<EnemyIdentifier>().spider.health, eid.spider.health + heal);
                }
            }
        }
    }
}