using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SAE.GAD176.Project2
{
    public class ShopTeleport : MonoBehaviour
    {
        bool isInShop = false;
        Vector3 shop = new Vector3(0, 0, -118);
        Shop _shop;

        private void Start()
        {
            _shop = GameObject.FindAnyObjectByType<Shop>();
        }
        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(TeleportWait(other));
            _shop.GenerateInventory();
        }
        private IEnumerator TeleportWait(Collider other)
        {
            if (!isInShop)
            {
                other.transform.position = shop;
                yield return new WaitForSeconds(0.5f);
                isInShop = true;
                yield break;
            }
            else if (isInShop)
            {
                other.transform.position = Vector3.zero;
                yield return new WaitForSeconds(0.5f);
                isInShop = false;
                yield break;
            }
        }
    }
}