using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private PlayerRayCast playerraycaster;
        #endregion

        #region Serialized Variables, also controlled by items
        [SerializeField] private float moveSpeed = 5000f;
        [SerializeField] private float moveDrag = 10f;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private float attackRange = 3;
        [SerializeField] private float attackDamage = 5;
        [SerializeField] private int attackDelayTime = 2;
        private int currentHealth;
        #endregion

        #region Unity Methods

        //Sanity checks, and setting the default HP
        void Start()
        {
            currentHealth = maxHealth;
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
            playerraycaster.Attack();
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
            rb.AddForce(movementVector * moveSpeed * Time.deltaTime, ForceMode.Force);
            rb.drag = moveDrag;
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
        public Tuple<int, int> GetHealth()
        {
            return Tuple.Create(maxHealth, currentHealth);
        }

        //Player has an attack angle of 45 degrees relative to local z+. Can only attack enemies if they are within this
        //  range. Multiple rays are cast within the 45 degree range. Max length of rays are determined by the attackRange
        // variable, which can be adjusted by having items.
        
        Nicholas listen to me. Use that Vector3.Rotate or whatever its called but instead of the rotate axis bing
            vector3.up, have the rotate axis be the transform.position. Okay
            #endregion
        }
    }
