using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{
    public enum PlatformTheme
    {
        Temple,
        Cliff,
        Plank,
    }

    public int CurrDepth
    {
        get;
        private set;
    }

    public int themeChunkCount = 5;
    public int nextPlatformDepth = 5;

    public PlatformTheme CurrentPlatformTheme { get; private set; }

    [Serializable]
    public class EnumPrefabPair
    {
        public Platform.Direction direction;
        public GameObject prefab;
    }

    public List<EnumPrefabPair> platformPrefabs = new List<EnumPrefabPair>();

    private Queue<(int depth, GameObject)> nextPlatforms = new();

    Dictionary<Platform.Direction, ObjectPool<GameObject>> platformPools = new();
    Dictionary<Platform.Obstacle, ObjectPool<GameObject>> obstaclePool = new();


    private void Awake()
    {
        platformPrefabs.ForEach(dirPrefab =>
        {
            ObjectPool<GameObject> pool = new
            (
                createFunc: () => Instantiate(dirPrefab.prefab),
                actionOnGet: obj => GetPlatform(dirPrefab.direction, obj),
                //actionOnRelease: obj => obj.Cleanup(),
                //actionOnDestroy: obj => obj.Dispose(),
                //collectionCheck: false,
                defaultCapacity: 5,
                maxSize: 10
            );
            platformPools.Add(dirPrefab.direction, pool);
        }
        );

        var next = platformPools[Platform.Direction.Vertical].Get().GetComponent<Platform>();
        ActivePlatform(next);
        MakePlatform(CurrDepth + 2, nextPlatformDepth, next, Platform.Direction.Vertical);
    }

    private void OnEnable()
    {
        CurrentPlatformTheme = PlatformTheme.Temple;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnExitPlatform()
    {
        CurrDepth++;

        while (nextPlatforms.Count() != 0 && nextPlatforms.Peek().depth < CurrDepth + nextPlatformDepth)
        {
            var next = nextPlatforms.Dequeue().Item2.GetComponent<Platform>();
            ActivePlatform(next);
            MakeNextPlatform(CurrDepth + nextPlatformDepth, next, GetRandomDirection());
        }
    }


    private void GetPlatform(Platform.Direction direction, GameObject platform )
    {
        platform.SetActive(false);
        platform.GetComponent<Platform>().SetDirection(direction);
    }

    private void ActivePlatform(Platform platform)
    {
        platform.gameObject.SetActive(true);
        platform.SetPosition();
    }

    private void MakeNextPlatform(int nextDepth, Platform platform, Platform.Direction direction)
    {
        var nextTrs = platform.NextPlatforms;
        foreach (var nextTr in nextTrs)
        {
            if (nextTr != null)
            {
                var next = platformPools[direction].Get();
                next.GetComponent<Platform>().PrevPlatform = nextTr;
                nextPlatforms.Enqueue((nextDepth, next));
            }
        }
    }

    private void MakePlatform(int startDepth, int count, Platform platform, Platform.Direction direction)
    {
        Platform curr = platform;
        for (int i = 0; i < count; i++)
        {
            var nextTr = curr.NextPlatforms[0];
            var next = platformPools[direction].Get().GetComponent<Platform>();
            next.PrevPlatform = nextTr;
            nextPlatforms.Enqueue((startDepth + i, next.gameObject));

            curr = next;
        }
    }

    private Platform.Direction GetRandomDirection()
    {
        return (Platform.Direction)Random.Range(0, typeof(Platform.Direction).GetEnumValues().Length - 1);
    }
}
