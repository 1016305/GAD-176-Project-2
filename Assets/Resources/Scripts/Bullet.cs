using SAE.GAD176.Project2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player player;
    private Vector3 currentPlayerPosition;
    private int damage;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        currentPlayerPosition = player.transform.position;
        damage = GetComponentInParent<ShootEnemy>().GetDamageValue();
    }
    private void FixedUpdate()
    {
        transform.position += currentPlayerPosition * 10 * Time.deltaTime;
    }
    private void OnBecameInvisible()
    {
        Destroy(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().TakeDamage(damage);
            Destroy(this);
        }
    }
}
