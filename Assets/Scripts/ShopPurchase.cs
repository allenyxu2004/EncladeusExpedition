using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPurchase : MonoBehaviour
{
    // PlayerPrefs Gun value 0
    public GameObject pistol;

    // PlayerPrefs Gun value 1
    public GameObject rifle;
    public int rifleDMG;
    public int rifleCost = 5;

    // PlayerPrefs Gun value 2
    public GameObject shotgun;
    public int shotgunDMG;
    public int shotgunCost = 10;

    // PlayerPrefs Gun value 3
    public GameObject sniper;
    public int sniperDMG;
    public int sniperCost = 15;

    public AudioSource purchaseSFX;
    public AudioSource cantBuySFX;

    //public static string currentGun = "Pistol";
    public static int damageModifier = 0;

    void Start()
    {
        //sniper = GameObject.FindGameObjectWithTag("Sniper");
        //rifle = GameObject.FindGameObjectWithTag("Rifle");
        //shotgun = GameObject.FindGameObjectWithTag("Shotgun");
        //pistol = GameObject.FindGameObjectWithTag("Pistol");

        if (!PlayerPrefs.HasKey("Gun"))
        {
            PlayerPrefs.SetInt("Gun", 0);
        }

        SetGun();
    }

    void PurchaseRifle()
    {
        if (PlayerHealth.meatCount >= rifleCost)
        {
            PlayerPrefs.SetInt("Gun", 1);
            PlayerHealth.meatCount = PlayerHealth.meatCount - rifleCost;

            SetGun();

            //currentGun = "Rifle";
            print("Rifle Purchased");
            purchaseSFX.Play();
            damageModifier = rifleDMG;
        }
        else
        {
            cantBuySFX.Play();
        }
    }

    void PurchaseShotgun()
    {
        if (PlayerHealth.meatCount >= shotgunCost)
        {
            PlayerPrefs.SetInt("Gun", 2);
            PlayerHealth.meatCount = PlayerHealth.meatCount - shotgunCost;
            
            SetGun();

            print("Shotgun Purchased");
            purchaseSFX.Play();
            damageModifier = shotgunDMG;
        }
        else
        {
            cantBuySFX.Play();
        }
       
    }

    void PurchaseSniper()
    {
        if (PlayerHealth.meatCount >= sniperCost)
        {
            PlayerPrefs.SetInt("Gun", 3);
            PlayerHealth.meatCount = PlayerHealth.meatCount - sniperCost;

            SetGun();

            print("Sniper Purchased");
            purchaseSFX.Play();
            damageModifier = sniperDMG;
        }
        else
        {
            cantBuySFX.Play();
        }


    }

    void SetGun()
    {
        int gun = PlayerPrefs.GetInt("Gun");
        switch (gun)
        {
            // Pistol
            case 0:
                pistol.SetActive(true);
                rifle.SetActive(false);
                shotgun.SetActive(false);
                sniper.SetActive(false);
                return;
            
            // Rifle
            case 1:
                pistol.SetActive(false);
                rifle.SetActive(true);
                shotgun.SetActive(false);
                sniper.SetActive(false);
                return;

            // Shotgun
            case 2:
                pistol.SetActive(false);
                rifle.SetActive(false);
                shotgun.SetActive(true);
                sniper.SetActive(false);
                return;

            // Sniper
            case 3:
                pistol.SetActive(false);
                rifle.SetActive(false);
                shotgun.SetActive(false);
                sniper.SetActive(true);
                return;

            // Gun not set
            default:
                pistol.SetActive(true);
                rifle.SetActive(false);
                shotgun.SetActive(false);
                sniper.SetActive(false);
                return;
        }
    }
}
