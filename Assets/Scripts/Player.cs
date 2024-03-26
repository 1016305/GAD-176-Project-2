using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    /// <summary>
    /// Player class, controlled by the player. Contains all the necessary functions for the player to operate.
    /// Movement, rotation, attack, switching items, etc all contained here.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region private, protected
        
        private Rigidbody rb;
        private Vector3 movementVector;
        private Vector3 mouseWorldPosition;
        private Camera cam;
        protected bool attackDelay = false;
        private List<Item> inventory = new List<Item>();
        private Item activeItem;
        #endregion

        #region Serialized Variables, also controlled by items
        private int currentHealth;
        [SerializeField] private PlayerScriptableObject c_player;
        #endregion

        #region Unity Methods

        //Sanity checks, and setting the default HP
        void Start()
        {
            if (c_player == null)
            {
                c_player = GetComponent<PlayerScriptableObject>();
            }
            currentHealth = c_player.maxHealth;
            if (cam == null)
            {
                cam = Camera.main;
            }
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }
        //Input detection is run in Update, but movement is run in FixedUpdate
        void Update()
        {
            GetInput();
            Attack();
        }

        private void FixedUpdate()
        {
            Move();
            RotateToMouse();
        }
        #endregion
        #region My Methods
        //GetInput gets input. Legacy input manager for the movement axies, Horizontal and Vertical.
        //Gets mouse position for rotation.
        private void GetInput()
        {
            //Movement Input
            movementVector.x = Input.GetAxis("Horizontal");
            movementVector.z = Input.GetAxis("Vertical");

            //Look Input
            //ScreenToWorldPoint must set the Z axis to be the distance from the camera. Otherwise it will not return the
            //  mouse position on screen.
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, (cam.transform.position.y - transform.position.y));
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);


        }

        //Movement uses rigidbody addforce. It does not use AddRelativeForce, as the direction the player is looking is
        //  independent from the direction they are moving.
        //Drag is added as an adjustable variable to control how quickly the player will slow down after movement input
        // ceases.
        private void Move()
        {
            rb.AddForce(movementVector * c_player.moveSpeed * Time.deltaTime, ForceMode.Force);
            rb.drag = c_player.moveDrag;
            cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z);
        }

        //RotateToMouse will rotate the player in the direction of the mouse's location on screen. This took me FAR too
        //  long to figure out, so this whole assembly is a big mess. However, it still works.
        private void RotateToMouse()
        {
            transform.LookAt(new Vector3(mouseWorldPosition.x, transform.position.y, mouseWorldPosition.z), Vector3.up);
        }

        //Basic setter for other classes to access to damage the player.
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }

        //Universal getter. I found a new thing I hate. It's called a tuple. I can use it to return multiple values from
        //  a single method, wherein I can choose which data is output. Summary so I know what values return per item
        //  when I access it in other classes.
        /// <summary>
        /// Item1 = maxHealth, Item2 = currentHealth
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> GetPlayerInfo()
        {
            return Tuple.Create(c_player.maxHealth, currentHealth);
        }

        //Attack is called if the "Fire1" or Left Mouse Button is down. This allows the player to click and hold to continually
        //  attack.

        public void Attack()
        {
            if (Input.GetAxis("Fire1") != 0 && !attackDelay)
            {
                StartCoroutine(DelayedAttack());
            }
            
        }
        //Delayed attack is the attack function, with a delay in seconds to promote strategic play.
        private IEnumerator DelayedAttack()
        {
            attackDelay = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, c_player.attackRange))
            {
                hit.collider.GetComponentInParent<Enemy>().TakeDamage(c_player.attackDamage);
                Debug.Log(hit.collider.GetComponentInParent<Enemy>().currentHealth);
                
                //Put all the attack gubbins in here okey!!

                Debug.DrawLine(transform.position, hit.point, Color.red, 3);
            }
            else
            {
                Debug.Log("Miss");
            }
            yield return new WaitForSeconds(c_player.attackDelayTime);
            attackDelay = false;

        }
        public void DamageKnockBack(Vector3 relativeDirection, float knockbackForce)
        {
            rb.AddForce(relativeDirection * knockbackForce, ForceMode.Impulse);
        }
        public void PickUpItem(Item item)
        {
            inventory.Add(item);
            if (activeItem == null)
            {
                activeItem = inventory[0];
            }
        }
        public void SwitchItem()
        {
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (inventory == null)
                {
                    Debug.Log("No items!");
                }
                else
                {

                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (inventory == null)
                {
                    Debug.Log("No items!");
                }
                else
                {

                }
            }
        }
        #endregion
    }
}

