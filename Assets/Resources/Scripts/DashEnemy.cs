using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class DashEnemy : Enemy
    {
        bool doAttack = false;
        private Vector3 playerLocation;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
        private void FixedUpdate()
        {
            switch (currentState)
            {
                case enemyState.patrol:
                    ReturnToStart();
                    doAttack = false;
                    break;
                case enemyState.alert:
                    RotateTowardPlayer();
                    if (!doAttack)
                    {
                        StartCoroutine(DashAttack());
                    }
                    break;
            }
        }
        //Dashes at the player to atttack.
        //Get player co-ordinates, lock in
        //Wait 0.5 seconds
        //Dash to co-ordinates
        //Wait X time
        private IEnumerator DashAttack()
        {
            float t = 0;
            //Debug.Log("Alert");
            doAttack = true;
            RotateTowardPlayer();
            yield return new WaitForSeconds(0.5f);
            while (t <= 1)
            {
                t += 1 * Time.deltaTime;
                Debug.Log(t);
                MoveTowardPlayer();
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3f);
            t = 0;
            doAttack = false;
        }
    }
}
