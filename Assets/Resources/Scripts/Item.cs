using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Players/Item")]
public class Item : ScriptableObject
{
    public string i_name;
    public string i_Description;
    //player stat modifiers
    public float i_moveSpeed;
    public float i_moveDrag;
    public int i_maxHealth;
    public float i_attackRange;
    public int i_attackDamage;
    public float i_attackDelayTime;
    public float i_knockbackAmmount;
}

