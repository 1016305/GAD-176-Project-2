using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTeleport : MonoBehaviour
{
    bool isInShop = false;
    Vector3 shop = new Vector3(0, 0, -118);
    private void OnTriggerEnter(Collider other)
    {
        if (isInShop == false)
        {
            other.gameObject.transform.position = shop;
            if (other.gameObject.transform.position == shop)
            {
                isInShop = true;
            }
        }
        if (isInShop == true)
        {
            other.gameObject.transform.position = Vector3.zero;
            if (other.gameObject.transform.position == Vector3.zero)
            {
                isInShop = false;
            }
        }
    }
    private void Update()
    {
        print(isInShop);
    }
}
