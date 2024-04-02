using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SAE.GAD176.Project2
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager current;
        #region Private, Protected
        private TextMeshProUGUI displayPlayerHealth;
        private TextMeshProUGUI displayCurrentItem;
        private TextMeshProUGUI displayMoney;
        private Player currentPlayer;
        [SerializeField] private List<GameObject> activeEnemies = new List<GameObject>();
        #endregion

        #region Unity Methods
        //Sanity checks
        void Start()
        {
            if (displayPlayerHealth == null)
            {
                displayPlayerHealth = GameObject.FindGameObjectWithTag("UI_playerHealth").GetComponent<TextMeshProUGUI>();
            }
            if (displayCurrentItem == null)
            {
                displayCurrentItem = GameObject.FindGameObjectWithTag("UI_currentItem").GetComponent<TextMeshProUGUI>();
            }
            if (displayMoney == null)
            {
                displayMoney = GameObject.FindGameObjectWithTag("UI_money").GetComponent<TextMeshProUGUI>();
            }
            if (currentPlayer == null)
            {
                currentPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();
            }
        }
        void Update()
        {
            PlayerInfo();  
        }
        void PlayerInfo()
        {
            //Uses the getter tuple from the player to get the max, and current health.
            var playerData = currentPlayer.GetPlayerInfo();
            displayPlayerHealth.text = ("Current Health: \n" + playerData.Item2.ToString() + "/" + playerData.Item1.ToString());
            displayMoney.text = "Money: " + playerData.Item4.ToString();
            if (playerData.Item3 == null)
            {
                displayCurrentItem.text = "";
            }
            else
            {
                displayCurrentItem.text = ("Current Item: " + playerData.Item3.name);
            }
        }
        public void AddUIToList(GameObject enemyStat)
        {
            activeEnemies.Add(enemyStat);
        }
        #endregion
    }
}
