using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{

    public float interactRange = 4.0f;

    Interactable lastInteractable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver) return;

        Interactable interactable = GetPlayerInteractables();
        if (interactable != null)
        {
            interactable.SetUI(true);
            if (lastInteractable != interactable)
            {
                if (lastInteractable != null)
                {
                    lastInteractable.SetUI(false);
                }
                lastInteractable = interactable;
            }
        }
        else
        {
            if (lastInteractable != null)
            {
                lastInteractable.SetUI(false);
                lastInteractable = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(interactable != null)
            {
                interactable.Interact(this.gameObject);
            }
        }
    }

    public Interactable GetPlayerInteractables()
    {
        Interactable closestInteractable = null;
        float closestDistance = float.MaxValue;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange);
        foreach (var hitCollider in hitColliders)
        {
            Interactable interactable = hitCollider.GetComponent<Interactable>();
            if (interactable != null)
            {
                float interactableDistance = Vector3.Distance(transform.position, interactable.GetTransform().position);
                if (interactableDistance < closestDistance)
                {
                    closestDistance = interactableDistance;
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
}
