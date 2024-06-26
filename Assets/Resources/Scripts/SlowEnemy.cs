using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class SlowEnemy : Enemy
    {
        #region private vars
        [SerializeField] private float enemyMeleeRange;
        private bool attackDelay = false;
        #endregion

        #region Unity methods
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            
        }
        //switch case using the inherited Enum
        private void FixedUpdate()
        {
            switch (currentState)
            {
                case enemyState.patrol:
                    ReturnToStart();
                    break;
                case enemyState.alert:
                    RotateTowardPlayer();
                    MoveTowardPlayer();
                    break;
            }      
        }
        //Attacks the player if their two colliders touch.
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Player>() && !attackDelay)
            {
                Player player = collision.gameObject.GetComponent<Player>();
                StartCoroutine(Attack(player));
            }
        }
        #endregion
        #region my methods
        //Attack function. Attacks the player using values from the scriptable object, then waits for X seconds
        //  before attacking again.
        private IEnumerator Attack(Player player)
        {
            attackDelay = true;
            player.TakeDamage(c_enemy.attackDamage);
            player.DamageKnockBack(Vector3.forward, 20);
            yield return new WaitForSeconds(c_enemy.attackSpeed);
            attackDelay = false;
        }
        #endregion
    }
}
