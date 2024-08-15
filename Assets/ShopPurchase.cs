using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPurchase : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject sniper;
    GameObject rifle;
    GameObject shotgun;
    GameObject pistol;

    public static string currentGun = "Pistol";
    void Start()
    {
        sniper = GameObject.FindGameObjectWithTag("Sniper");
        rifle = GameObject.FindGameObjectWithTag("Rifle");
        shotgun = GameObject.FindGameObjectWithTag("Shotgun");
        pistol = GameObject.FindGameObjectWithTag("Pistol");

    }

    public void PurchaseShotgun()
    {
        sniper.SetActive(false);
        rifle.SetActive(false);
        shotgun.SetActive(true);
        pistol.SetActive(false);

        currentGun = "Shotgun";
        print("Switched");
    }

    public void PurchaseSniper()
    {
        sniper.SetActive(true);
        rifle.SetActive(false);
        shotgun.SetActive(false);
        pistol.SetActive(false);

        currentGun = "Sniper";
        print("Switched");

    }

    public void PurchaseRifle()
    {
        sniper.SetActive(false);
        rifle.SetActive(true);
        shotgun.SetActive(false);
        pistol.SetActive(false);

        currentGun = "Rifle";
        print("Switched");

    }

}
