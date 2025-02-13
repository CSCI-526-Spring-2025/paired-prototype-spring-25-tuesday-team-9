using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] 
    GameObject inactiveVisual;
    [SerializeField]
    GameObject activeVisual;

    [SerializeField]
    GameObject test;

    LevelController LC;
    bool active = false;

    private void Start()
    {
        LC = GameObject.Find("LevelController").GetComponent<LevelController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        test = other.gameObject;
        if (other.gameObject.CompareTag("Player") || (other.transform.parent && other.transform.parent.gameObject.CompareTag("Player")))
        {
            LC.newCheckPoint(this);
            toggleCheckpoint();

            
        }
    }

    public void toggleCheckpoint()
    {
        inactiveVisual.SetActive(active);
        activeVisual.SetActive(!active);
        active = !active;
    }
}
