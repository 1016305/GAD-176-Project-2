using SAE.GAD176.Project2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

namespace SAE.GAD176.Project2 {
    public class Shop : MonoBehaviour
    {
        #region public variables
        [SerializeField] private List<Item> shopInventory = new List<Item>();
        [SerializeField] private Item[] itemsForSale = new Item[3];
        private Player player;
        private GameObject currentShopInfo;
        private int indexValue;
        private bool tick;
        #endregion

        #region Unity Methods
        //Note: Future sanity checks can be avoided by making the player a singleton*. 
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            GenerateInventory();
        }
        private void OnTriggerEnter(Collider collision)
        {
            ShopZones(collision);
        }
        private void OnTriggerExit(Collider other)
        {
            Destroy(currentShopInfo);
        }
        private void OnTriggerStay(Collider other)
        {
            ItemPurchase();
        }
        //Interim fix for input recognition not functioning correctly. Please do not put in the final project.
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                tick = true;
            }
            else if (!Input.GetKeyDown(KeyCode.Space))
            {
                tick = false;
            }
        }
        #endregion
        //Runs every time the teleporter is accessed. Refreshes the shop's inventory based on the remaining store items.
        //Runs both when the player goes in, and when the player leaves. A little redundant but eh. Sue me.
        public void GenerateInventory()
        {
            List<Item> tempItems = shopInventory.ToList();
            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, shopInventory.Count -1 - i);
                //Debug.Log(i + " " + rand);
                itemsForSale[i] = tempItems[rand];
                //Debug.Log(itemsForSale[i].name);
                tempItems.RemoveAt(rand);
            }
        }
        //DESCRIPTION: Selects the correct shop zone to display the item to the player.

        //REAL DESCRIPTION. This is a crime against nature. Truly a horiffic display of man's hubris. AS FAR AS I KNOW,
        //  unity does NOT contain a way to identify which collider is being entered if the collider exists as a child
        //  of another gameobject. It can find the colliding object, the parent object, but not the child. Why? I asked
        //  god and it started to rain outisde. 
        //This pulls all of the shopfront colliders into an array and judges the distance between the player and the
        //  collider object and selects th "correct" collider based on the distance. I apologise.
        private void ShopZones(Collider collision)
        {
            //Find the three tagged ItemZones and slap them into an array of GameObjects
            GameObject[] _shopFront = GameObject.FindGameObjectsWithTag("ItemZone");
            //Make a new list called distances. Get the distance between the center of the transform for all GameObjects.
            List<float> distances = new List<float>();
            //Get the magnitude of distances between the player and the ItemZones, and fill the list.
            foreach (GameObject shopFront in _shopFront)
            {
                distances.Add((shopFront.transform.position - player.transform.position).magnitude);
            }
            //New local gameObject shopInfo to load the textmeshpro canvas element
            GameObject shopInfo = Resources.Load<GameObject>("Prefabs/shopInventory");
            //Global variable stores the instantiated canvas element so it can be accessed for deletion
            currentShopInfo = Instantiate(shopInfo, GameObject.FindAnyObjectByType<Canvas>().transform);
            //Get the TMPro component of the canvas element
            TextMeshProUGUI _shopInfo = currentShopInfo.GetComponent<TextMeshProUGUI>();
            //Local int indexvalue gets the index value of the smallest number in the list (i.e. the one closest to the player
            indexValue = distances.IndexOf(distances.Min());

            //Parents the UI element to the relevant shop
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_shopFront[indexValue].transform.position);
            screenPos = new Vector3((screenPos.x + Screen.width / 3), (screenPos.y + Screen.height / 4), screenPos.z);
            _shopInfo.transform.position = screenPos;
            //Tells the player there is no item if the item has been purchased.
            if (itemsForSale[indexValue] == null)
            {
                _shopInfo.text = "No item available.";
            }
            //Gives the player the item info
            else if (itemsForSale[indexValue] != null)
            {
                _shopInfo.text = "ITEM:" + "\n \n" + itemsForSale[indexValue].i_name + "\n" + "_________" + "\n" + itemsForSale[indexValue].i_Description;
            }
        }
        //Function to add items to the player inventory, and subtract money from the player data.
        //Also checks if the player has enough money, or already has the item.
        private void ItemPurchase()
        {
            if (tick)
            {
                {
                    //Accesses the GetPlayerInfo tuple, and dumps the inventory to a list locally.
                    List<Item> playerData = player.GetPlayerInfo().Item5;
                    bool inventoryQuery = true;
                    //Flips a bool if the player has any items in their inventory. "Necessary" to avoid null reference issues
                    //  when indexing the player's inventory. Necessary as in I don't know how to make a generic method that
                    //  functions like this so I don't have copy/pasted code.
                    if (playerData.Count == 0)
                    {
                        inventoryQuery = false;
                    }
                    else if (playerData.Count != 0)
                    {
                        inventoryQuery = true;
                    }

                    //Checks to see if the player has an item already sold in the shop in their inventory. If they do, it will
                    //  change to shop text to display "alrady purchased"
                    if (inventoryQuery)
                    {
                        foreach (Item items in playerData)
                        {
                            if (itemsForSale[indexValue] == items)
                            {
                                currentShopInfo.GetComponent<TextMeshProUGUI>().text = "Already purchased.";
                            }
                            //If the player has enough money AND they don't already have that item in their inventory, then they
                            // can make the purchase.
                            //It then removes the item from the shop's front inventory.
                            //Aparrently you aren't supposed to edit a list while querying it in a foreach loop. If I had the time
                            //  and/or sanity I'd go back and change this to a for loop.
                            //It throws a tantrum but still functions ¯\\_ (ツ)_/¯
                            else if (player.GetMoney() >= 5 && itemsForSale[indexValue] != items && itemsForSale[indexValue] != null)
                            {
                                player.SpendMoney(itemsForSale[indexValue]);
                                itemsForSale[indexValue] = null;
                                currentShopInfo.GetComponent<TextMeshProUGUI>().text = "No item available.";
                            }
                            else if (player.GetMoney() < 5)
                            {
                                Debug.Log("POOR!");
                            }
                        }
                    }
                    //Identical code which will run if the inventory is empty. Does not run the check for items in the player's inventory.
                    if (!inventoryQuery)
                    {
                        if (player.GetMoney() >= 5 && itemsForSale[indexValue] != null)
                        {
                            player.SpendMoney(itemsForSale[indexValue]);
                            itemsForSale[indexValue] = null;
                            currentShopInfo.GetComponent<TextMeshProUGUI>().text = "No item available.";
                        }
                        else if (player.GetMoney() < 5)
                        {
                            currentShopInfo.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
                        }
                    }
                }
            }
        }
    }
}