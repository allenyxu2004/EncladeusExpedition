using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 100.0f;
    public Image reticleImage;
    public float meleeRange = 1.0f;
    public Color targetColor;
    Color originalColor;
    Color currentColor;

    public AudioSource projectileSFX;
    public AudioSource rifleSFX;
    public AudioSource sniperSFX;
    public AudioSource shotgunSFX;

    public AudioSource meleeSFX;

    public ParticleSystem muzzleFlash;

    Animator animator;

    void Start()
    {
        originalColor = reticleImage.color;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (LevelManager.isGameOver) return;

        if (PauseMenu.isGamePaused)
        {
            Color color = reticleImage.color;
            color.a = 0;
            reticleImage.color = color;
        }
        else
        {
            reticleImage.color = currentColor;
        }

        if (!PauseMenu.isGamePaused && Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            if (projectileSFX != null)
            {
                // Rifle
                if (PlayerPrefs.GetInt("Gun") == 1)
                {
                    rifleSFX.Play();
                }
                // Shotgun
                else if (PlayerPrefs.GetInt("Gun") == 2)
                {
                    shotgunSFX.Play();
                }
                // Sniper
                else if (PlayerPrefs.GetInt("Gun") == 3)
                {
                    sniperSFX.Play();
                }
                else
                {
                    projectileSFX.Play();
                }

                muzzleFlash.Play();
            }

            Destroy(projectile, 2);
        }

        if (!PauseMenu.isGamePaused && Input.GetButtonDown("Fire2"))
        {
            Melee();
            meleeSFX.Play();
        }
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }

    void ReticleEffect()
    {
        RaycastHit hit;
        if (!PauseMenu.isGamePaused && Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentColor = Color.Lerp(reticleImage.color, targetColor, Time.deltaTime * 2);
                reticleImage.color = currentColor;
                reticleImage.transform.localScale =
                    Vector3.Lerp(reticleImage.transform.localScale,
                    new Vector3(0.7f, 0.7f, 1),
                    Time.deltaTime * 2);
            }
            else
            {
                currentColor = Color.Lerp(reticleImage.color, originalColor, Time.deltaTime * 2);
                reticleImage.color = currentColor;
                reticleImage.transform.localScale =
                    Vector3.Lerp(reticleImage.transform.localScale,
                    Vector3.one,
                    Time.deltaTime * 2);
            }
        }
    }

    void Melee()
    {
        animator.SetTrigger("meleeAttack");
        RaycastHit hit;
        if (!PauseMenu.isGamePaused && Physics.Raycast(transform.position, transform.forward, out hit, meleeRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyHealth>().TakeDamage(100);
            }
        }
    }
}
