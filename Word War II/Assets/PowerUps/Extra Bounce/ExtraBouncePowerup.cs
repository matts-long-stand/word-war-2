using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.PowerUps
{
    class ExtraBouncePowerup : PowerUp
    {
        void Start()
        {
            GetComponentInChildren<Rigidbody>().AddTorque(transform.up * 100);
        }

        public override void ApplyPowerUp()
        {
            startTime = Time.realtimeSinceStartup;

            StartCoroutine(OnTimedEvent());
        }

        IEnumerator OnTimedEvent()
        {
            Debug.Log("Coroutine started");
            float timeElapsed = Time.realtimeSinceStartup - startTime;
            while (timeElapsed < 5f)
            {
                Debug.Log("current time " + timeElapsed);
                timeElapsed = Time.realtimeSinceStartup - startTime;
                yield return null;
            }

            owner.RemovePowerUp(this);
        }
    }
}
