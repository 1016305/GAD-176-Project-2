using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Players/Item")]
public class Item : ScriptableObject
{
    [Tooltip("Name of item to display.")]
        public string i_name;
    [Tooltip("A description of the item's effects.")]
        public string i_Description;

    //player stat modifiers
    [Tooltip("The value to add to the player's move speed. Adds to the initial value, does not override.")]
        public float i_moveSpeed;
    [Tooltip("The value to add to the player's move drag. Use a negative value to subtract. \nAdjusts how quickly the player will slow down after input has stopped.")]
        public float i_moveDrag;
    [Tooltip("The value to add to the player's maximum hitpoints count.")]
        public int i_maxHealth;
    [Tooltip("The value to add to the player's maximum attack reach.")]
        public float i_attackRange;
    [Tooltip("The value to add to the player's base damage.")]
        public int i_attackDamage;
    [Tooltip("The value to add to the player's attack time. Use a negative value to reduce time.")]
        public float i_attackDelayTime;
    [Tooltip("The value to add to the player's knockback distance.")]
        public float i_knockbackAmmount;
    [Tooltip("Tick this box to double the ammount of money that the player recieves upon defeating an enemy.")]
        public bool i_moneyDouble;
}

