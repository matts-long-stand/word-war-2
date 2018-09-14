using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using System.Collections;

namespace Assets.PowerUps
{
    class FreezeOpponentPowerup : PowerUp
    {
        Player[] players = null;
        float startTime;

        public override void ApplyPowerUp()
        {
            startTime = Time.realtimeSinceStartup;

            players = FindObjectsOfType<Player>();
            foreach(Player player in players) {
                if(player.name != owner)
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
                if (player.name != owner)
                {
                    player.frozen = false;
                }
            }
        }
    }
}
