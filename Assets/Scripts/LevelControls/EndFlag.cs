using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFlag : MonoBehaviour
{
    [SerializeField]
    GameObject LC;

    [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
        LC = GameObject.Find("LevelController");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.transform.parent && other.transform.parent.gameObject.CompareTag("Player")))
        {
            LC.BroadcastMessage("NextLevel");
        }
    }
}
