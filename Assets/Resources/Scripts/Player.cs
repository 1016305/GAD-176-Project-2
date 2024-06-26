using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private List<Item> inventory = new List<Item>();
        private Item activeItem;
        [SerializeField] private int inventoryIndex = 0;
        #endregion

        #region Serialized Variables, also controlled by items
        [SerializeField] private int currentHealth;
        [SerializeField] private PlayerScriptableObject c_player;
        #endregion

        #region playerStats to be accessed
        private float moveSpeed;
        private float moveDrag;
        private int maxHP;
        private float attackRange;
        private int attackDamage;
        private float attackDelayTime;
        private float knockbackAmmount;
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
            SetDefaultStats();
        }
        //Input detection is run in Update, but movement is run in FixedUpdate
        void Update()
        {
            GetInput();
            Attack();
            IsDead();
            SwitchItem();
        }

        private void FixedUpdate()
        {
            Move();
            RotateToMouse();
            
        }

        #endregion
        #region My Methods
        private void SetDefaultStats()
        {
            moveSpeed = c_player.moveSpeed;
            moveDrag = c_player.moveDrag;
            maxHP = c_player.maxHealth;
            attackRange = c_player.attackRange;
            attackDamage = c_player.attackDamage;
            attackDelayTime = c_player.attackDelayTime;
            knockbackAmmount = c_player.knockbackAmmount;
        }
        //GetInput gets input. Legacy input manager for the movement axies, Horizontal and Vertical.
        //Gets mouse position for rotation.
        //In future this can directly return a Vector3
        //private Vector3 GetInput()
        //{
        //    Vector3 input = new Vector3((Input.GetAxis("Horizontal")), 0, (Input.GetAxis("Vertical")));
        //    return input;
        //}
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
        //Checks if the player's HP is at or below 0. If below zero, it will set the value to zero. If at zero, it will delete the player
        //  and stop the game. Contains commands to quit both compiled application and in-editor application.
        private void IsDead()
        {
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            if (currentHealth == 0)
            {
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
                Destroy(this.gameObject);
                
            }
        }
        

        //Attack is called if the "Fire1" or Left Mouse Button is down. This allows the player to click and hold to continually
        //  attack
        private void Attack()
        {
            if (Input.GetAxis("Fire1") != 0 && !attackDelay)
            {
                StartCoroutine(DelayedAttack());
            }

        }
        //Delayed attack is the attack function, with a delay in seconds to promote strategic play.
        private IEnumerator DelayedAttack()
        {
            //Raycast from the player forwards, distance is determined by the c_player.attackRange value.
            attackDelay = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
            {
                //Runs the enemy's takeDamage, and the enemy's knockback.
                Enemy e = hit.collider.GetComponentInParent<Enemy>();
                if (e)
                {
                    e.TakeDamage(attackDamage);
                    Debug.Log(e.currentHealth);
                    e.DamageKnockBack(Vector3.forward, knockbackAmmount);
                    e.currentState = Enemy.enemyState.alert;
                }
            }
            else
            {
                Debug.Log("Miss");
            }
            yield return new WaitForSeconds(attackDelayTime);
            attackDelay = false;

        }
        //Player's knockback. Identical to the enemy's, but knockback force is adjustable through items
        public void DamageKnockBack(Vector3 relativeDirection, float knockbackForce)
        {
            rb.AddForce(relativeDirection * knockbackForce, ForceMode.Impulse);
        }
        //Runs only once. When the player adds an item to their inventory, this forces to item into the equipped slot.
        //Not currently relevant, but it one point I had the items as physical gameobjects. I left this is in case I
        //  wanted to change it back.

        //NB re-engineered it to equip and apply the item
        public void PickUpItem(Item item)
        {
            inventory.Add(item);
            if (activeItem == null)
            {
                activeItem = inventory[0];
                SetStatModifiers();
            }
            else
            {
                activeItem = inventory[inventory.Count - 1];
                SetStatModifiers();
            }
        }
        
        //Allows the player to switch between items held in the inventory. Q to cycle backwards, E to cycle forwards.
        //Active item is controlled by the int inventoryIndex, which is the int used to pick the index of the item from the inventory. Shocker.
        //Will check if the int goes below zero, or goes above the maximum number of items in the list, and will roll around to the last item
        // in the list, and the first one, respectively.
        //When a new item is selected, it runs SetStatModifiers, to apply the stat changes implied by the item.
        private void SwitchItem()
        {            
            if (Input.GetKeyDown(KeyCode.E) && inventory.Count != 0)
            {
                inventoryIndex++;
                if (inventoryIndex > inventory.Count - 1)
                {
                    inventoryIndex = 0;
                    activeItem = inventory[0];
                    SetStatModifiers();
                }
                else
                {
                    activeItem = inventory[inventoryIndex];
                    SetStatModifiers();
                }
            }
            if (Input.GetKeyDown(KeyCode.Q) && inventory.Count != 0)
            {
                inventoryIndex--;
                if (inventoryIndex < 0)
                {
                    inventoryIndex = inventory.Count - 1;
                    activeItem = inventory[inventory.Count - 1];
                    SetStatModifiers();
                }
                activeItem = inventory[inventoryIndex];
                SetStatModifiers();
            }
            

        }
        //Adjusts the player's stats to be affected by the item that is currently equipped.
        //First runs SetDefaultStats(), to change all stats back to the base defined in the player scriptable object.
        //  This is done beacuse stat modifiers are added to the base stats, not replaced by them.
        //  This was done because when making new items, each stat would need to be manually set to the player's default stats.
        //  This was done because some of the stats are types that cannot be null, like floats.
        //Then grabs the stats of the items, and adds them to the newly default stats.
        private void SetStatModifiers()
        {
            SetDefaultStats();
            moveSpeed += activeItem.i_moveSpeed;
            moveDrag += activeItem.i_moveDrag;
            maxHP += activeItem.i_maxHealth;
            attackRange += activeItem.i_attackRange;
            attackDamage += activeItem.i_attackDamage;
            attackDelayTime += activeItem.i_attackDelayTime;
            knockbackAmmount += activeItem.i_knockbackAmmount;
        }
        #endregion
        #region Getters'n'Setters
        //Basic setter for other classes to access to damage the player.
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
        //Adjusts the player's money count. Includes an alternate function to double the player's income if they
        //  have a specific item equipped. I did this badly. If should be a modifier stored in the stats like the rest. Im sorry.
        public void GetMoney(int money)
        {
            if (activeItem == null)
            {
                c_player.money += money;
            }
            else if (activeItem == activeItem.i_moneyDouble)
            {
                c_player.money += money * 2;
            }
            else
            {
                c_player.money += money;
            }
        }
        //Returns an int for the player's current money. Used by shops to determine if the player can afford an item.
        //I named this the same as the other GetMoney method. It was not by accident. I did not make a mistake that turned
        //  out to be an example of polymorphism. This was 100% intended.
        public int GetMoney()
        {
            return c_player.money;
        }
        //Universal getter. I found a new thing I hate. It's called a tuple. I can use it to return multiple values from
        //  a single method, wherein I can choose which data is output. Summary so I know what values return per item
        //  when I access it in other classes.
        /// <summary>
        /// Item1 = maxHealth, Item2 = currentHealth, Item3 = activeItem, Item4 = money, Item5 = Inventory
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int, Item, int, List<Item>> GetPlayerInfo()
        {
            return Tuple.Create(maxHP, currentHealth, activeItem, c_player.money, inventory);
        }
        //Subtracts money from the player's values and adds an item. Functions to determine whether or not the player
        //  can afford the item is controlled by the shop.
        public void SpendMoney(Item item)
        {
            c_player.money -= 5;
            PickUpItem(item);
        }
        #endregion
    }
}

