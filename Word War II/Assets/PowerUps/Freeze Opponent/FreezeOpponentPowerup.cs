using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Assets.PowerUps
{
    class FreezeOpponentPowerup : PowerUp
    {
        Player[] players = null;

        void Start()
        {
            GetComponentInChildren<Rigidbody>().AddTorque(transform.forward * 100);
        }

        public override void ApplyPowerUp()
        {
            base.ApplyPowerUp();
            startTime = Time.realtimeSinceStartup;

            players = FindObjectsOfType<Player>();
            foreach(Player player in players) {
                if(player.name != owner.name)
                {
                    player.frozen = true;
                }
            }

            StartCoroutine(OnTimedEvent());
        }


        IEnumerator OnTimedEvent()
        {
            float timeElapsed = Time.realtimeSinceStartup - startTime;
            while (timeElapsed < 5f)
            {
                timeElapsed = Time.realtimeSinceStartup - startTime;
                yield return null;
            }

            foreach (Player player in players)
            {
                if (player.name != owner.name)
                {
                    player.frozen = false;
                }
            }

            owner.RemovePowerUp(this);
        }
    }
}
