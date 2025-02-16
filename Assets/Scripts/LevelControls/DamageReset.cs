using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReset : MonoBehaviour
{
    GameObject LC;

    [SerializeField] bool resetLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        LC = GameObject.Find("LevelController");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.transform.parent && other.transform.parent.gameObject.CompareTag("Player")))
        {
            if (!resetLevel)
            {
                LC.BroadcastMessage("Respawn");
            }
            else
            {
                LC.BroadcastMessage("ResetLevel");
            }
        }
    }
}
