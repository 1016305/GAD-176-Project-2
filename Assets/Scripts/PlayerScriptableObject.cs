using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Players/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public float moveSpeed = 5000f;
    public float moveDrag = 10f;
    public int maxHealth = 100;
    public float attackRange = 3;
    public float attackDamage = 5;
    public float attackDelayTime = 2;
}
