using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SAE.GAD176.Project2
{
    public class ShopTeleport : MonoBehaviour
    {
        [Tooltip("Two teleporters use this script. Ensure one has this ticked, and the other does not.")]
        [SerializeField] private bool teleporter;
        private bool isInShop = false;
        Vector3 shop = new Vector3(0, 0, -118);
        Shop _shop;

        private void Start()
        {
            _shop = GameObject.FindAnyObjectByType<Shop>();
        }
        private void OnTriggerEnter(Collider other)
        {
            TeleportWait(other);
            _shop.GenerateInventory();
        }
        private void TeleportWait(Collider other)
        {
            if (teleporter)
            {
                other.transform.position = shop;
            }
            else if (!teleporter)
            {
                other.transform.position = Vector3.zero;
            }
        }
    }
}