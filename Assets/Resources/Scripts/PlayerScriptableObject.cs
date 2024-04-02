using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Players/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    [Tooltip("The speed at which the player will move in the game world. \nUses rigidbody for movement, so this number may need to be quite high.")]
        public float moveSpeed = 5000f;
    [Tooltip("How quickly the player will slow down after movement input has stopped.")]
        public float moveDrag = 10f;
    [Tooltip("The enemy's maximum health points. \nWill start with this number when the game begins.")]
        public int maxHealth = 100;
    [Tooltip("The maximum distance between a player the enemy for an attack to land.")]
        public float attackRange = 3;
    [Tooltip("The damage the player deals on a successful attack.")]
        public int attackDamage = 5;
    [Tooltip("The time (in seconds) between player attacks.")]
        public float attackDelayTime = 2;
    [Tooltip("How far the enemy is knocked back when the player attacks.")]
        public float knockbackAmmount = 3;
    [Tooltip("The ammount of money the player has. \nNOTE: Not reset when game is reset.")]
        public int money = 0;
}
