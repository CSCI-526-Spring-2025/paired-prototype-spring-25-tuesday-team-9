using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
public GameObject test;

    private void OnTriggerEnter2D(Collider2D other)
    {
        test = other.gameObject;
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.transform.parent.GetComponent<PlayerController>();
            if (player != null)
            {
                player.RespawnPlayer(); 
            }
        }
    }
}
