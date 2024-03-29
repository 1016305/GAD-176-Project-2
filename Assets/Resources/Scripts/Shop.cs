using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SAE.GAD176.Project2 {
    public class Shop : MonoBehaviour
    {
        public List<Item> shopInventory = new List<Item>(10);
        public Item[] itemsForSale = new Item[3];
        private Player player;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            GenerateInventory();
        }
        private void OnTriggerStay(Collider collision)
        {
            ShopZones(collision);
        }
        public void GenerateInventory()
        {
            List<Item> tempItems = shopInventory.ToList();
            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, shopInventory.Count - 1);
                //Debug.Log(i + " " + rand);
                itemsForSale[i] = tempItems[rand];
                //Debug.Log(itemsForSale[i].name);
                tempItems.RemoveAt(rand);
            }
        }
        //DESCRIPTION: Selects the correct shop zone to display the item to the player.

        //REAL DESCRIPTION. This is a crime against nature. Truly a horiffic display of man's hubris. As far as I know,
        //  unity does NOT contain a way to identify which collider is being entered if the collider exists as a child
        //  of another gameobject. It can find the colliding object, the parent object, but not the child. Why? I asked
        //  god and it started to rain outisde. 
        //This pulls all of the shopfront colliders into an array and judges the distance between the player and the
        //  collider object and selects th "correct" collider based on the distance. I apologise.
        public void ShopZones(Collider collision)
        {
            GameObject[] _shopFront = GameObject.FindGameObjectsWithTag("ItemZone");
            List<float> distances = new List<float>();
            foreach (GameObject shopFront in _shopFront)
            {
                distances.Add((shopFront.transform.position - player.transform.position).magnitude);
            }
            int indexValue = distances.IndexOf(distances.Min());
            print(distances.Count);
            print(_shopFront[indexValue].name + " " + itemsForSale[indexValue]);

            
        }
    }
}
