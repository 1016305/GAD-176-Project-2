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
                        Debug.Log("Alert");
                        StartCoroutine(DashAttack());
                    }
                    break;
            }
        }
        //Get player co-ordinates, lock in
        //Wait 0.5 seconds
        //Dash to co-ordinates
        //Wait X time
        private IEnumerator DashAttack()
        {
            Debug.Log("Attack");
            doAttack = true;
            yield return new WaitForSeconds(0.5f);
            while (transform.position != playerLocation)
            {
                Vector3 moveDirection = (currentPlayerPosition - transform.position).normalized;
                rb.AddForce(moveDirection * c_enemy.moveSpeed * Time.deltaTime, ForceMode.Force);
                rb.drag = c_enemy.drag;
            }
            yield return new WaitForSeconds(c_enemy.attackSpeed);
            doAttack = false;
        }
    }
}
