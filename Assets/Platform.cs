using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private string playerTag = "Player";

    public enum Direction
    {
        Horizon,
        Vertical,
        TurnA,
        TurnB,
        TurnC,
        TurnD,
        //TDown,
        //TRight,
        //TUp,
        //TLeft,
    }

    [Flags]
    public enum Obstacle
    {
        None,
        Jump,
        Slide,

    }



    public GameObject prefab;
    private GameObject child;
    private Transform[] nextPosition = new Transform[2];


    private PlatformSpawner platformSpawner;

    public Transform[] NextPlatforms
    {
        get => nextPosition;
    }

    public Transform PrevPlatform
    {
        get; set;
    }

    [SerializeField]
    private Direction direction;

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    private void Awake()
    {
        platformSpawner = GameObject.FindWithTag("GameController").GetComponent<PlatformSpawner>();
        nextPosition[0] = transform.Find("NextPosition");
        nextPosition[1] = transform.Find("NextPosition1");
    }

    private void OnEnable()
    {
        child = transform.Find("PlatformModel").gameObject;

        if (child == null)
        {
            child = Instantiate(prefab);
            child.transform.localPosition = new Vector3(-30, 0, 0);
            child.transform.localRotation = Quaternion.Euler(0, 0, 0);
            child.name = "PlatformModel";
            child.transform.SetParent(transform);
        }
    }

    public void SetPosition()
    {
        gameObject.transform.position = PrevPlatform?.position ?? new(0, 0, 60);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            platformSpawner.OnExitPlatform();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {

        }
    }
}
