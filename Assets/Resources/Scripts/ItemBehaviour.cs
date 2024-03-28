using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class ItemBehaviour : MonoBehaviour
    {
        [SerializeField] private Item item;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Player>())
            {
                collision.gameObject.GetComponent<Player>().PickUpItem(item);
                Destroy(this.gameObject);
            }
        }
    }
}
