using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static Platform;
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
    public int visiblePlatformsCount = 5;

    public PlatformTheme CurrentPlatformTheme { get; private set; }

    [SerializeField]
    private List<(Platform.Dir direction, GameObject prefab)> platformPrefabs = new();

    private Dictionary<Platform.Dir, List<Platform.Dir>> connectableDirections = new();

    [SerializeField]
    private Platform lastPlatform;
    private Queue<Platform> lastPlatforms = new();
    private Queue<Platform> nextPlatforms = new();
    private Queue<Platform.Dir> dirs = new();

    private int depth = 1;

    private Dictionary<Platform.Dir, ObjectPool<GameObject>> platformPools = new();
    private Dictionary<Platform.Obstacle, ObjectPool<GameObject>> obstaclePool = new();

    private void Awake()
    {
        InitializeConnectableDirections();
        InitializePlatformPool();

    }

    private void Start()
    {
        CurrDepth = 0;
        depth = 1;

        lastPlatforms.Enqueue(lastPlatform);
        Platform prev;
        for (int i = 0; i < visiblePlatformsCount - depth; i++)
        {
            prev = lastPlatforms.Dequeue();
            MakeNextPlatform(prev, Platform.Dir.VertUp);
            (lastPlatforms, nextPlatforms) = (nextPlatforms, lastPlatforms);
            depth++;
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

    private void InitializePlatformPool()
    {
        string prefabPath = "Assets/PlatformPrefab/{0}.prefab";
        foreach (Platform.Dir dir in Enum.GetValues(typeof(Platform.Dir)))
        {
            string fullpath = String.Format(prefabPath, Enum.GetName(typeof(Platform.Dir), dir));
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(fullpath);
            //if (asset == null)
            //{
            //    var origin = platformPrefabs.Find(pair => (int)pair.direction == -(int)dir).prefab;
            //    asset = Instantiate(origin);
            //    if (dir.ToString().Contains("Down"))
            //    {
            //        asset.transform.localScale = new(1, 1, -1);
            //    }
            //    else if (dir.ToString().Contains("Left"))
            //    {
            //        asset.transform.localScale = new(-1, 1, 1);
            //    }
            //    PrefabUtility.SaveAsPrefabAsset(asset, String.Format(prefabPath, dir));
            //}

            if (asset != null)
            {
                platformPrefabs.Add((dir, asset));
            }
        }

        platformPools.Clear();
        platformPrefabs.ForEach(dirPrefab =>
        {
            ObjectPool<GameObject> pool = new
            (
                createFunc: () => Instantiate(dirPrefab.prefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                //actionOnDestroy: obj => obj.Dispose(),
                //collectionCheck: false,
                defaultCapacity: 5,
                maxSize: 10
            );
            platformPools.Add(dirPrefab.direction, pool);
        }
        );
    }

    private void InitializeConnectableDirections()
    {
        List<Platform.Dir> horzR = new();
        horzR.Add(Platform.Dir.HorzRight);
        horzR.Add(Platform.Dir.TurnCUp);
        horzR.Add(Platform.Dir.TurnCDown);
        connectableDirections.Add(Platform.Dir.HorzRight, horzR);

        List<Platform.Dir> horzL = new();
        horzL.Add(Platform.Dir.HorzLeft);
        horzL.Add(Platform.Dir.TurnDUp);
        horzL.Add(Platform.Dir.TurnDDown);
        connectableDirections.Add(Platform.Dir.HorzLeft, horzL);

        List<Platform.Dir> vertU = new();
        vertU.Add(Platform.Dir.VertUp);
        vertU.Add(Platform.Dir.TurnAUp);
        vertU.Add(Platform.Dir.TurnBUp);
        connectableDirections.Add(Platform.Dir.VertUp, vertU);

        List<Platform.Dir> vertD = new();
        vertD.Add(Platform.Dir.VertDown);
        vertD.Add(Platform.Dir.TurnADown);
        vertD.Add(Platform.Dir.TurnBDown);
        connectableDirections.Add(Platform.Dir.VertDown, vertD);

    }

    public void OnExitPlatform()
    {
        CurrDepth++;

        while (lastPlatforms.Count() != 0)
        {
            var last = lastPlatforms.Dequeue();
            MakeNextPlatform(last, GetRandomDirection(last.NextDirection));
        }
        (lastPlatforms, nextPlatforms) = (nextPlatforms, lastPlatforms);
        depth++;
    }

    private void MakeNextPlatform(Platform prev, Platform.Dir direction)
    {
        Debug.Log($"depth {depth + 1} dir{direction}");

        var nextTrs = prev.NextPositions;
        foreach (var nextTr in nextTrs)
        {
            if (nextTr != null)
            {
                //NeedToChangeStaight(direction)


                var next = platformPools[direction].Get().GetComponent<Platform>();
                next.OnGetFromPool(depth + 1, nextTr);
                nextPlatforms.Enqueue(next);
            }
        }
    }

    private Platform.Dir GetRandomDirection(Platform.Dir direction)
    {
        int index = Random.Range(0, connectableDirections[direction].Count());
        return connectableDirections[direction][index];
    }

    public void ReleasePlatform(Platform platform)
    {
        platformPools[platform.Direction].Release(platform.gameObject);
    }

    private bool IsTurn(Platform.Dir dir)
    {
        return (dir == Platform.Dir.TurnAUp ||
        dir == Platform.Dir.TurnADown ||
        dir == Platform.Dir.TurnBUp ||
        dir == Platform.Dir.TurnBDown ||
        dir == Platform.Dir.TurnCUp ||
        dir == Platform.Dir.TurnCDown ||
        dir == Platform.Dir.TurnDUp ||
        dir == Platform.Dir.TurnDDown);
    }
    private bool NeedToChangeStaight(Platform.Dir currDir)
    {
        bool turn = IsTurn(currDir);
        bool turncnt= dirs.ToList().Count(dir => IsTurn(dir)) >= 2;

        return turn&&turncnt;
    }
}
