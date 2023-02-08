using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public Text maximaPuntuacionText, coinAmountText, coinUpgradeText, coinUpgradePrice, heartUpgradeText, heartUpgradePrice, shieldUpgradeText, shieldUpgradePrice;

    private int coinLevel, heartLevel, shieldLevel;

    private int[] coinPrice = {75, 200, 450, 800, 1500};
    private int[] heartPrice = {500, 1000};
    private int[] shieldPrice = {200, 400, 700, 1000, 1400, 2000};

    private int coinAmount, maxCoinLevel, maxHeartLevel, maxShieldLevel;

    public GameObject instructionsUI, menuUI, upgradesUI, coinAmountUI;
    public GameObject coinUpgrade, coinMax, heartUpgrade, heartMax, shieldUpgrade, shieldMax;

    private void Start()
    {
        //Changes the time scale back to 1 in case the player came from the in-game pause menu
        Time.timeScale = 1;

        //Updates all the variables for the menu
        maximaPuntuacionText.text = "Highscore: " + PlayerPrefs.GetInt("Puntuacion", 0);
        coinAmount = PlayerPrefs.GetInt("CoinAmount", 0);
        coinAmountText.text = coinAmount.ToString();
        
        coinLevel = PlayerPrefs.GetInt("CoinLevel", 0);
        heartLevel = PlayerPrefs.GetInt("HeartLevel", 0);
        shieldLevel = PlayerPrefs.GetInt("ShieldLevel", 0);

        maxCoinLevel = coinPrice.Length;
        maxHeartLevel = heartPrice.Length;
        maxShieldLevel = shieldPrice.Length;
    }


    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }


    public void Salir()
    {
        Application.Quit();
    }

    //Opens the instructions menu
    public void OpenInstructions()
    {
        //Hides the menu and the amount of coins and shows the instructions page
        coinAmountUI.SetActive(false);
        instructionsUI.SetActive(true);
        menuUI.SetActive(false);
    }

    //Closes the instructions menu
    public void CloseInstructions()
    {
        //Hides the instructions page and shows the menu and the amount of coins
        menuUI.SetActive(true);
        instructionsUI.SetActive(false);
        coinAmountUI.SetActive(true);
    }

    //Opens the upgrades menu
    public void OpenUpgrades()
    {
        //Closes the menu and opens the upgrade page while keeping the amount of coins on the screen
        upgradesUI.SetActive(true);
        menuUI.SetActive(false);


        coinUpgradeText.text = coinLevel.ToString();
        //Shows the upgrade button by default, but it hides it and shows that it's max level if the player reached the maximum level for that upgrade
        if (coinLevel == maxCoinLevel)
        {
            coinUpgrade.SetActive(false);
            coinMax.SetActive(true);
        } else
        {
            coinUpgradePrice.text = "Upgrade Price\n" + coinPrice[coinLevel];
        }


        heartUpgradeText.text = heartLevel.ToString();
        //Shows the upgrade button by default, but it hides it and shows that it's max level if the player reached the maximum level for that upgrade
        if (heartLevel == maxHeartLevel)
        {
            heartUpgrade.SetActive(false);
            heartMax.SetActive(true);
        }
        else
        {
            heartUpgradePrice.text = "Upgrade Price\n" + heartPrice[heartLevel];
        }

        shieldUpgradeText.text = shieldLevel.ToString();
        //Shows the upgrade button by default, but it hides it and shows that it's max level if the player reached the maximum level for that upgrade
        if (shieldLevel == maxShieldLevel)
        {
            shieldUpgrade.SetActive(false);
            shieldMax.SetActive(true);
        }
        else
        {
            shieldUpgradePrice.text = "Upgrade Price\n" + shieldPrice[shieldLevel];
        }
    }

    //Closes the upgrades menu
    public void CloseUpgrades()
    {
        menuUI.SetActive(true);
        upgradesUI.SetActive(false);
    }

    //Upgrade for the coin
    public void UpgradeCoin()
    {
        //Checks if the player has enough coins for the upgrade
        if (coinAmount >= coinPrice[coinLevel])
        {
            //Reduces the amount of coins by the upgrade's price
            coinAmount -= coinPrice[coinLevel];
            coinAmountText.text = coinAmount.ToString();
            PlayerPrefs.SetInt("CoinAmount", coinAmount);
            //Increases the level of the upgrade
            coinLevel++;
            PlayerPrefs.SetInt("CoinLevel", coinLevel);
            //Updates the menu based on the upgrade's level
            coinUpgradeText.text = coinLevel.ToString();
            if (coinLevel == maxCoinLevel)
            {
                coinUpgrade.SetActive(false);
                coinMax.SetActive(true);
            }
            else
            {
                coinUpgradePrice.text = "Upgrade Price\n" + coinPrice[coinLevel];
            }
        }
    }

    //Upgrade for the coin
    public void UpgradeHeart()
    {
        //Checks if the player has enough coins for the upgrade
        if (coinAmount >= heartPrice[heartLevel])
        {
            //Reduces the amount of coins by the upgrade's price
            coinAmount -= heartPrice[heartLevel];
            coinAmountText.text = coinAmount.ToString();
            PlayerPrefs.SetInt("CoinAmount", coinAmount);
            //Increases the level of the upgrade
            heartLevel++;
            PlayerPrefs.SetInt("HeartLevel", heartLevel);
            //Updates the menu based on the upgrade's level
            heartUpgradeText.text = heartLevel.ToString();
            if (heartLevel == maxHeartLevel)
            {
                heartUpgrade.SetActive(false);
                heartMax.SetActive(true);
            }
            else
            {
                heartUpgradePrice.text = "Upgrade Price\n" + heartPrice[heartLevel];
            }
        }
    }

    //Upgrade for the coin
    public void UpgradeShield()
    {
        //Checks if the player has enough coins for the upgrade
        if (coinAmount >= shieldPrice[shieldLevel])
        {
            //Reduces the amount of coins by the upgrade's price
            coinAmount -= shieldPrice[shieldLevel];
            coinAmountText.text = coinAmount.ToString();
            PlayerPrefs.SetInt("CoinAmount", coinAmount);
            //Increases the level of the upgrade
            shieldLevel++;
            PlayerPrefs.SetInt("ShieldLevel", shieldLevel);
            //Updates the menu based on the upgrade's level
            shieldUpgradeText.text = shieldLevel.ToString();
            if (shieldLevel == maxShieldLevel)
            {
                shieldUpgrade.SetActive(false);
                shieldMax.SetActive(true);
            }
            else
            {
                shieldUpgradePrice.text = "Upgrade Price\n" + shieldPrice[shieldLevel];
            }
        }
    }
}
