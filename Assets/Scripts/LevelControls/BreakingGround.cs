using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakingGround : MonoBehaviour
{
    [SerializeField]
    float SecondsToBreak;
    [SerializeField]
    float SecondsToRespawn;

    Coroutine coroutineIsRunning;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(DelayTrigger());
    }

    IEnumerator DelayTrigger()
    {
        //yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.transform.parent && other.transform.parent.gameObject.CompareTag("Player")))
        {
            if (coroutineIsRunning == null)
            {
                coroutineIsRunning = StartCoroutine(Fade());
            }
        }
    }

    IEnumerator Fade()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            gameObject.GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(SecondsToBreak / 10.0f);
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(SecondsToRespawn);
        c.a = 1.0f;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        coroutineIsRunning = null;
        yield return null;
    }
}
