using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class Enemy : MonoBehaviour
    {
        #region private, protected
        protected Player player;
        protected Vector3 currentPlayerPosition;
        protected Rigidbody rb;
        protected enum enemyState {patrol,alert}
        protected enemyState currentState;
        public int currentHealth;
        #endregion
        #region serialized variables
        [SerializeField] protected EnemyScriptableObject c_enemy;
        #endregion

        #region Unity Methods
        protected virtual void Start()
        //Sanity checks, and setting the default state for the enemy behaviours.
        {
            currentHealth = c_enemy.maxHealth;
            currentState = enemyState.patrol;
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            if (player == null)
            {
                player = FindAnyObjectByType<Player>();
            }
            if (c_enemy == null)
            {
                c_enemy = GetComponent<EnemyScriptableObject>();
            }
        }
        //Constantly returns the player's current position. Used by every enemy subclass.
        protected virtual void Update()
        {
            GetPlayerPosition();
            DetectPlayer();

        }
        #endregion

        #region my methods
        //Moves toward the player at the speed set in the scriptableobject, and adds drag.
        protected void MoveTowardPlayer()
        {
            Vector3 moveDirection = (currentPlayerPosition - transform.position).normalized;
            rb.AddForce(moveDirection * c_enemy.moveSpeed * Time.deltaTime, ForceMode.Force);
            rb.drag = c_enemy.drag;
        }
        //Gets the player's current position and assigns it to a variable.
        protected void GetPlayerPosition()
        {
            currentPlayerPosition = player.transform.position;
        }
        //Rotates the enemy to face the player.
        protected void RotateTowardPlayer()
        {
            transform.rotation = Quaternion.LookRotation(FlattenVector(currentPlayerPosition) - FlattenVector(transform.position));
        }
        //Detects if the player is within the enemy's view radius (adjustable in degrees) or within 2.5 units of the
        //  enemy. If either condition is true, then the enemy changes to alert mode, and begins to attack the player.
        protected void DetectPlayer()
        {
            float angle = Vector3.Angle(transform.forward, (currentPlayerPosition - transform.position));
            if (angle <= c_enemy.viewRadius | (currentPlayerPosition - transform.position).magnitude < 2.5)
            {
                currentState = enemyState.alert;
            }

            //Will change to alert status if player is
            //  a) Within a small radius around the enemy
            //  b) Within the view angle of the player
        }
        //Because I'm a dummy, I've been doing basically all the math for this game in 2D, despite the fact that it's top
        //  down 2D. SO i had to write a quick function that I can plug any vector into that sets the Y axis to 0. This
        //  solved a number of issues relating to enemies looking upwards as they approached the player.
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
        protected Vector3 FlattenVector(Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }
        public Tuple<int, int> GetEnemyInfo()
        {
            return Tuple.Create(c_enemy.maxHealth, currentHealth);
        }
        #endregion
    }
}