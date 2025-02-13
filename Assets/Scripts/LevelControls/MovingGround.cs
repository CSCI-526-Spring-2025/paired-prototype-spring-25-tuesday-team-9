using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingGround : MonoBehaviour
{
    [SerializeField] List<GameObject> PlatformLocations;
    [SerializeField] GameObject Platform;
    [SerializeField] float speed;
    [SerializeField] float delayTime;

    [SerializeField]
    int index = 0;

    bool run = true;

    // Start is called before the first frame update
    void Update()
    {
        if (run)
        {
            Vector3 direction = PlatformLocations[index].transform.position - Platform.transform.position;
            float mag = direction.magnitude;
            if (mag < speed * Time.deltaTime)
            {
                Platform.transform.position = PlatformLocations[index].transform.position;
                index = (index + 1) % PlatformLocations.Count;

                run = false;
                StartCoroutine(delay());
            }
            else
            {
                Platform.transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(delayTime);
        run = true;
    }

}
