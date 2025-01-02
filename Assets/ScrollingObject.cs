using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private PlayerMovement player;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        transform.Translate(-1 * player.transform.forward * player.Speed * Time.deltaTime, Space.World);
    }
}
