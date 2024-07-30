using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZlorp : MonoBehaviour
{
    public GameObject particleEffect;

    public int scoreValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.CompareTag("Projectile"))
        {
            DestroyEnemy();
            //FindObjectOfType<LevelManager>().AddScore(scoreValue);
        }
    }

    void DestroyEnemy()
    {
        Instantiate(particleEffect, transform.position, transform.rotation);

        // Hides the game object so it can be destoryed on screen while still playing the animation
        gameObject.SetActive(false);


        Destroy(gameObject, 0.5f);
    }
}
