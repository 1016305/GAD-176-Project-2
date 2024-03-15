using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [Tooltip("The enemy's maximum health points. \nWill start with this number when the game begins.")]
        public int maxHealth;
    [Tooltip("The speed at which the enemy will move towards the player. \nUses rigidbody for movement, so this number may need to be quite high.")]
        public float moveSpeed;
    [Tooltip("The ammount of health the player will lose when hit.")]
        public int attackDamage;
    [Tooltip("The time (in seconds) between the enemy's attacks.")]
        public float attackSpeed;
    [Tooltip("How quickly the enemy will slow down after movement input has stopped.")]
        public float drag;
    [Tooltip("How quickly the enemy will rotate to face the player after they have been alerted.")]
        public float rotateSpeed;
    [Tooltip("The radius (in degrees) in front of the enemy that they will see the player and become alerted.")]
        public float viewRadius;
}
