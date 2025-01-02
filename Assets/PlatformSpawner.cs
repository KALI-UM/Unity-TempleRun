using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public enum PlatformTheme
    {
        Temple,
        Cliff,
        Plank,
    }

    public int themeChunkCount = 5;
    public PlatformTheme CurrentPlatformTheme { get; private set; }

    public GameObject HorizonPrefab;
    public GameObject VeticalPrefab;

    private Queue<GameObject> nextPlatforms = new();
    private Queue<GameObject> usingPlatforms = new();


    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gobj = Instantiate(VeticalPrefab);
            gobj.SetActive(false);
            nextPlatforms.Enqueue(gobj);
        }

        for (int i = 0; i < 2; i++)
        {
            ActiveNextPlatform();
        }
    }

    private void OnEnable()
    {
        CurrentPlatformTheme = PlatformTheme.Temple;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActiveNextPlatform()
    {
        var next = nextPlatforms.Dequeue();
        next.transform.position = GetNextPosition();
        next.SetActive(true);

        usingPlatforms.Enqueue(next);

        if(nextPlatforms.Count()<5)
        {
            MakePlatform();
        }
    }

    private void MakePlatform()
    {
        var old = usingPlatforms.Dequeue();
        old.SetActive(false);
        nextPlatforms.Enqueue(old);
    }

    private Vector3 GetNextPosition()
    {
        var nextPlatform = usingPlatforms.ToList().LastOrDefault();
        return nextPlatform?.GetComponent<Platform>().NextPlatformPosition.position ?? new Vector3(0, 25, 0);
    }
}
