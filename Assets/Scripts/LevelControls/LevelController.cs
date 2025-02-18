using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Levels = new List<GameObject>();
    [SerializeField]
    int CurrentLevel = 0;

    [SerializeField]
    GameObject EndMessage;

    GameObject CurrentLevelObj;
    GameObject PlayerRef;

    Checkpoint CheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRef = GameObject.Find("Player");
        ResetLevel();
        StartCoroutine(DelayedSetSpawn());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NextLevel();
        }
    }
#endif

    IEnumerator DelayedSetSpawn()
    {
        //yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        Respawn();
        yield return null;
    }

    public void ResetLevel()
    {
        if (CurrentLevelObj != null)
        {
            Destroy(CurrentLevelObj);
        }
        if (CheckPoint)
        {
            CheckPoint.enabled = false;
            CheckPoint = null;
        }
        CurrentLevelObj = Instantiate(Levels[CurrentLevel], Vector2.zero, Quaternion.identity);
        Respawn();
        
    }
    public void Respawn()
    {
        GameObject spawn = GameObject.Find("Spawn");

        PlayerController pc = PlayerRef.GetComponent<PlayerController>();
        pc.SetToDefaultState();
        pc.humanForm.transform.position = spawn.transform.position;
    }
    public void NextLevel()
    {
        if (CurrentLevel < Levels.Count-1)
        {
            if (CheckPoint)
            {
                CheckPoint.enabled = false;
                CheckPoint = null;
            }
            PlayerRef.SetActive(false);
            Destroy(CurrentLevelObj);
            CurrentLevelObj = Instantiate(Levels[++CurrentLevel], Vector2.zero, Quaternion.identity);
            Respawn();
            PlayerRef.SetActive(true);
            StartCoroutine(DelayedSetSpawn());
        }
        else
        {
            PlayerController pc = PlayerRef.GetComponent<PlayerController>();
            pc.SetToDefaultState();
            PlayerRef.SetActive(false);
            
            Vector3 pos = pc.humanForm.transform.position;
            pos.z = -5;
            Instantiate(EndMessage, pos, Quaternion.identity);
        }
    }

    public void newCheckPoint(Checkpoint c)
    {
        if (CheckPoint)
        {
            CheckPoint.toggleCheckpoint();
        }
        CheckPoint = c;
        GameObject spawn = GameObject.Find("Spawn");
        spawn.transform.position = c.transform.position;
    }
}
