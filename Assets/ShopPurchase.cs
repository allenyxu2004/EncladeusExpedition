using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPurchase : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sniper;
    public int sniperDMG;
    public int sniperCost = 0;

    public GameObject rifle;
    public int rifleDMG;
    public int rifleCost = 0;

    public GameObject shotgun;
    public int shotgunDMG;
    public int shotgunCost = 0;

    public GameObject pistol;

    public AudioSource purchaseSFX;
    public AudioSource cantBuySFX;

    public static string currentGun = "Pistol";
    public static int damageModifier = 0;

    void Start()
    {
        //sniper = GameObject.FindGameObjectWithTag("Sniper");
        //rifle = GameObject.FindGameObjectWithTag("Rifle");
        //shotgun = GameObject.FindGameObjectWithTag("Shotgun");
        //pistol = GameObject.FindGameObjectWithTag("Pistol");

    }

    void PurchaseShotgun()
    {
        if (PlayerHealth.meatCount >= shotgunCost)
        {
            PlayerHealth.meatCount = PlayerHealth.meatCount - shotgunCost;
            sniper.SetActive(false);
            rifle.SetActive(false);
            shotgun.SetActive(true);
            pistol.SetActive(false);

            currentGun = "Shotgun";
            print("Switched");
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
            PlayerHealth.meatCount = PlayerHealth.meatCount - sniperCost;

            sniper.SetActive(true);
            rifle.SetActive(false);
            shotgun.SetActive(false);
            pistol.SetActive(false);

            currentGun = "Sniper";
            print("Switched");
            purchaseSFX.Play();
            damageModifier = sniperDMG;
        }
        else
        {
            cantBuySFX.Play();
        }


    }

    void PurchaseRifle()
    {
        if (PlayerHealth.meatCount >= rifleCost)
        {
            PlayerHealth.meatCount = PlayerHealth.meatCount - rifleCost;


            sniper.SetActive(false);
            rifle.SetActive(true);
            shotgun.SetActive(false);
            pistol.SetActive(false);

            currentGun = "Rifle";
            print("Switched");
            purchaseSFX.Play();
            damageModifier = rifleDMG;
        }
        else
        {
            cantBuySFX.Play();
        }



    }

}
