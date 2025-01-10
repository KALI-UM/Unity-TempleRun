using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string rotateAxisName = "Horizontal";
    public static readonly string verticalAxisName = "Vertical";

    public float rotateThresholdTime = 1f;
    private float lastRotateTime;

    public float Rotate { get; private set; }
    public bool Jump { get; private set; }
    public float Slide { get; private set; }

    private void Start()
    {
        lastRotateTime = 0f;
    }

    void Update()
    {
        if (Time.time > lastRotateTime + rotateThresholdTime && Input.GetButtonDown(rotateAxisName))
        {
            Rotate = Input.GetAxisRaw(rotateAxisName);
            lastRotateTime = Time.time;
        }
        else
        {
            Rotate = 0;
        }

        Slide = Mathf.Clamp(-Input.GetAxis(verticalAxisName), 0f, 1f);
        Jump = Input.GetButtonDown(verticalAxisName) && Input.GetAxisRaw(verticalAxisName) > 0f;
    }

}
