using SAE.GAD176.Project2;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region private vars
    private ShootEnemy father;
    private Player player;
    private Vector3 lockPlayerPosition;
    private Vector3 direction;
    #endregion

    #region unity methods
    //Sanity checks and value assignment
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        lockPlayerPosition = player.transform.position;
        father = GameObject.FindAnyObjectByType<ShootEnemy>();
        direction = (lockPlayerPosition - transform.position).normalized;
    }
    //Moves the bullet towards the player by 10 every frame.
    private void FixedUpdate()
    {
        transform.position += direction * 10f * Time.deltaTime;
        
    }
    //Destroys the bullet when it leaves the screen.
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    //If the bullet touches the player, it runs the TakeDamage function from the player class.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(father.GetDamageValue());
            Destroy(this.gameObject);
        }
    }
    #endregion
}
