using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private string playerTag = "Player";

    public enum Dir
    {
        Unknown = 0,
        HorzRight = 1,
        HorzLeft = -1,
        VertUp = 2,
        VertDown = -2,
        TurnAUp = 3,
        TurnADown = -3,
        TurnBUp = 4,
        TurnBDown = -4,
        TurnCUp = 5,
        TurnCDown = -5,
        TurnDUp = 6,
        TurnDDown = -6,
        //TDown,
        //TRight,
        //TUp,
        //TLeft,
    }

    [Flags]
    public enum Obstacle
    {
        None,
        Hole,
        Flame,
        TreeJump,
        TreeSlide,
    }

    private PlatformSpawner platformSpawner;

    public GameObject prefab;
    private Transform[] nextPosition = new Transform[2];


    public Transform[] NextPositions
    {
        get => nextPosition;
    }

    private Transform prevPosition;
    private int depth = 1;

    [SerializeField]
    private Dir direction;

    public Dir Direction
    {
        get => direction;
    }

    [SerializeField]
    private Dir nextDirection;

    [SerializeField]
    private Dir nextDirection1;

    public Dir NextDirection
    {
        get => nextDirection;
    }

    public Dir NextDirection1
    {
        get => nextDirection1;
    }

    private bool isPassed = false;

    private void Awake()
    {
        platformSpawner = GameObject.FindWithTag("GameController").GetComponent<PlatformSpawner>();
        nextPosition[0] = transform.Find("NextPosition");
        nextPosition[1] = transform.Find("NextPosition1");
    }

    private void FixedUpdate()
    {
        if (platformSpawner.CurrDepth > depth)
            platformSpawner.ReleasePlatform(this);
    }

    public void OnGetFromPool(int depth, Transform prev)
    {
        prevPosition = prev;
        gameObject.transform.position = prevPosition?.position ?? new(0, 0, 60);
        this.depth = depth;
        isPassed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPassed && other.gameObject.CompareTag(playerTag))
        {
            platformSpawner.OnExitPlatform();
            isPassed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {

        }
    }
}
