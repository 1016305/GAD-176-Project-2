using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SAE.GAD176.Project2
{
    public class ShopTeleport : MonoBehaviour
    {
        #region private vars
        [Tooltip("Two teleporters use this script. Ensure one has this ticked, and the other does not.")]
        [SerializeField] private bool teleporter;
        private Vector3 shop = new Vector3(0, 0, -118);
        private Shop _shop;
        #endregion

        #region unity methods
        //Gets a reference for the shop class. In future make this a singleton*.
        private void Start()
        {
            _shop = GameObject.FindAnyObjectByType<Shop>();
        }
        //When the trigger is entered, run these two functions. Generate inventory is run from the Shop class.
        private void OnTriggerEnter(Collider other)
        {
            TeleportWait(other);
            _shop.GenerateInventory();
        }
        #endregion

        #region my methods
        //Teleports the player into and out of the shop. When this script is applied two to teleporters,
        //  the teleporter bool must be activated on one of them. This facilitates the back-and-forth
        //  aspect of the teleporter.
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
        #endregion
    }
}