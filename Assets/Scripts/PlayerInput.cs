using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string rotateAxisName = "Horizontal";
    public static readonly string verticalAxisName = "Vertical";

    public float Rotate { get; private set; }
    public bool Jump { get; private set; }
    public float Slide { get; private set; }

    void Update()
    {
        if (Input.GetButtonDown(rotateAxisName))
        {
            Rotate = Input.GetAxisRaw(rotateAxisName);
        }
        else
        {
            Rotate = 0;
        }

        Slide = Mathf.Clamp(-Input.GetAxis(verticalAxisName), 0f, 1f);
        Jump = Input.GetButtonDown(verticalAxisName) && Input.GetAxisRaw(verticalAxisName) > 0f;
    }

}
