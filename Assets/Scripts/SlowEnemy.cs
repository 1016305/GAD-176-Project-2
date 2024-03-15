using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class SlowEnemy : Enemy
    {
        [SerializeField] private float enemyMeleeRange;
        private bool attackDelay = false;

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
                    //patrol behaviour
                    break;
                case enemyState.alert:
                    RotateTowardPlayer();
                    MoveTowardPlayer();
                    break;
            }
            
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Player>() && !attackDelay)
            {
                Player player = collision.gameObject.GetComponent<Player>();
                StartCoroutine(Attack(player));
            }
        }
        IEnumerator Attack(Player player)
        {
            attackDelay = true;
            player.TakeDamage(c_enemy.attackDamage);
            player.DamageKnockBack(Vector3.forward, 20);
            yield return new WaitForSeconds(c_enemy.attackSpeed);
            attackDelay = false;
        }
    }
}
