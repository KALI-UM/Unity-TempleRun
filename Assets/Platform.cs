using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private string playerTag = "Player";

    [Flags]
    public enum Type
    {
        None,
        Jump,
        Slide,

    }

    public GameObject prefab;
    private GameObject child;
    private Transform nextPosition;

    private PlatformSpawner platformSpawner;

    public Transform NextPlatformPosition
    {
        get => nextPosition;
    }


    private void Awake()
    {
        platformSpawner = GameObject.FindWithTag("GameController").GetComponent<PlatformSpawner>();
        nextPosition = transform.Find("NextPosition");
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            platformSpawner.ActiveNextPlatform();
        }
    }

}
