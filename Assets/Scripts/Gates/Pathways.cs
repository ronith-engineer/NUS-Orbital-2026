using System;
using Unity.Cinemachine;
using UnityEngine;

public class Pathways : MonoBehaviour
{
    [Header("Pathway Settings")]
    [SerializeField] protected GameObject linkedPathway;
    [SerializeField] protected Transform player;


    protected bool playerNearby = false;
    





    protected virtual void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PlayerTeleporter.TeleportPlayer(player, linkedPathway.transform,CameraManager.Instance.MainVCam);
        }
    }


 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to open door");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            
        }
    }
}