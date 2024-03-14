using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class UIManager : MonoBehaviour
    {
        #region Private, Protected
        private TextMeshProUGUI displayPlayerHealth;
        private Player currentPlayer;
        #endregion

        #region Unity Methods
        //Sanity checks
        void Start()
        {
            if (displayPlayerHealth == null)
            {
                displayPlayerHealth = GameObject.Find("playerHealth").GetComponent<TextMeshProUGUI>();
            }
            if (currentPlayer == null)
            {
                currentPlayer = Object.FindAnyObjectByType<Player>();
            }
        }


        void Update()
        {
            //Uses the getter tuple from the player to get the max, and current health.
            var playerData = currentPlayer.GetPlayerInfo();
            displayPlayerHealth.text = ("Current Health: \n" + playerData.Item2.ToString() + "/" + playerData.Item1.ToString());
        }
        #endregion
    }
}
