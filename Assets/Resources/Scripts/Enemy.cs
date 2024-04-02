using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class Enemy : MonoBehaviour
    {
        #region private, protected
        protected Player player;
        protected Vector3 currentPlayerPosition;
        protected Rigidbody rb;
        public enum enemyState { patrol, alert }
        public enemyState currentState;
        public int currentHealth;
        private GameObject UISys;
        private Vector3 startPos;
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
            startPos = transform.position;
            CreateHealthBar();
        }
        //Constantly returns the player's current position. Used by every enemy subclass.
        protected virtual void Update()
        {
            GetPlayerPosition();
            DetectPlayer();
            IsDead();
            UpdateHealth();
            if (GetPlayerDistance() > 10)
            {
                currentState = enemyState.patrol;
            }
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
        }//Returns the magnitude of the distance between the player and the enemy.
        protected float GetPlayerDistance()
        {
            float dist = (currentPlayerPosition - transform.position).magnitude;
            return dist;
        }
        //Rotates the enemy to face the player.
        protected void RotateTowardPlayer()
        {
            transform.rotation = Quaternion.LookRotation(FlattenVector(currentPlayerPosition) - FlattenVector(transform.position));
        }
        //Detects if the player is within the enemy's view radius (adjustable in degrees) or within 2.5 units of the
        //  enemy. If either condition is true, then the enemy changes to alert mode, and begins to attack the player.

        //Will change to alert status if player is
        //  a) Within a small radius around the enemy
        //  b) Within the view angle of the player
        protected void DetectPlayer()
        {
            float angle = Vector3.Angle(transform.forward, (currentPlayerPosition - transform.position));
            if (angle <= c_enemy.viewRadius && (currentPlayerPosition - transform.position).magnitude <= c_enemy.viewDistance)
            {
                currentState = enemyState.alert;
            }
            else if ((currentPlayerPosition - transform.position).magnitude < 2.5)
            {
                currentState = enemyState.alert;
            }
        }
        //Returns the enemy to the location where it was placed in the scene.
        protected void ReturnToStart()
        {
            Vector3 moveDirection = (startPos - transform.position).normalized;
            rb.AddForce(moveDirection * c_enemy.moveSpeed * Time.deltaTime, ForceMode.Force);
            rb.drag = c_enemy.drag;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        //Public setter for health, to be accessed by the player when they attack.
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
        //Checks if the enemy has 0 or less HP. If so, it will delete the enemy and remove the HP bar.
        protected void IsDead()
        {
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                Destroy(UISys);
                player.GetMoney(3);
            }
        }
        //Creates the HP Bar when the enemy is loaded in. Takes the local variable for current HP, which is adjusted by
        //  the setter, and the maximum HP as defined in the scriptable Object for this enemy.
        //Instantiates a TextMeshProUGUI prefab and parents it to the canvas for display. It then renames it for easier
        // reference.
        protected void CreateHealthBar()
        {     
            GameObject ui = Resources.Load<GameObject>("Prefabs/_enemyHP");
            UISys = Instantiate(ui, GameObject.FindAnyObjectByType<Canvas>().transform);
            UISys.name = this.gameObject.name + "1"; 
        }
        //Keeps the health bar of each enemy up to date by pushing the local currentHP to the TextMeshPro. Also makes each
        //  enemies HP stick just above the enemy so it is always displayed to the player. Just for funzies, I made the
        //  enemy HP disappear when the player leaves a set distance from the enemy.
        protected void UpdateHealth()
        {
            if ((currentPlayerPosition - transform.position).magnitude >= 7)
            {
                UISys.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0);
            }
            if ((currentPlayerPosition - transform.position).magnitude < 7)
            {
                UISys.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 1);
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            screenPos = new Vector3(screenPos.x, (screenPos.y + 40), screenPos.z);
            UISys.transform.position = screenPos;
            UISys.GetComponent<TextMeshProUGUI>().text = (currentHealth.ToString()+ "/"+ c_enemy.maxHealth.ToString());
        }
        //Called by the player when they attack the enemy. Knocks the enemy back by the ammount defined by the player's scriptable object.
        public void DamageKnockBack(Vector3 relativeDirection, float knockbackForce)
        {
            rb.AddForce(relativeDirection * knockbackForce, ForceMode.Impulse);
        }
        //Because I'm a dummy, I've been doing basically all the math for this game in 2D, despite the fact that it's top
        //  down 2D. SO i had to write a quick function that I can plug any vector into that sets the Y axis to 0. This
        //  solved a number of issues relating to enemies looking upwards as they approached the player.
        protected Vector3 FlattenVector(Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }
        #endregion
    }
}