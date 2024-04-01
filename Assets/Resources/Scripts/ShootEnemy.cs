using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class ShootEnemy : Enemy
    {
        private Object bullet;
        private bool doAttack = false;
        Vector3 direction;
        // Start is called before the first frame update
        protected override void Start()
        {
            bullet = Resources.Load("Prefabs/Bullet");
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
                    break;
                case enemyState.alert:
                    RotateTowardPlayer();
                    if (!doAttack)
                    {
                        StartCoroutine(ShootAtPlayer());
                    }
                    break;
            }
        }
        private IEnumerator ShootAtPlayer()
        {
            doAttack = true;
            direction = Vector3.forward;
            Instantiate(bullet, transform.position, Quaternion.LookRotation(transform.position - currentPlayerPosition), this.transform);
            yield return new WaitForSeconds(3);
            doAttack = false;
        }
        public int GetDamageValue()
        {
            int damage = c_enemy.attackDamage;
            return damage;
        }
    }
}