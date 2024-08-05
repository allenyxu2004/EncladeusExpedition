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

    public AudioSource projectileSFX;
    public AudioClip meleeSFX;

    void Start()
    {
        originalColor = reticleImage.color;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            if (projectileSFX != null)
            {
                projectileSFX.Play();
            }

            Destroy(projectile, 2);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            AudioSource.PlayClipAtPoint(meleeSFX, transform.position);

            Melee();
        }
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }

    void ReticleEffect()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                reticleImage.color = Color.Lerp(reticleImage.color, targetColor, Time.deltaTime * 2);
                reticleImage.transform.localScale =
                    Vector3.Lerp(reticleImage.transform.localScale,
                    new Vector3(0.7f, 0.7f, 1),
                    Time.deltaTime * 2);
            }
            else
            {
                reticleImage.color = Color.Lerp(reticleImage.color, originalColor, Time.deltaTime * 2);
                reticleImage.transform.localScale =
                    Vector3.Lerp(reticleImage.transform.localScale,
                    Vector3.one,
                    Time.deltaTime * 2);
            }
        }
    }

    void Melee()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, meleeRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
