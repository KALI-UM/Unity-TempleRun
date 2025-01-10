using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement movement;
    public Transform startPoint;

    public float playerCutSceneTime = 4f;

    bool isDead = false;
    public enum State
    {
        CutScene,
        Running,
        Dead,
    }

    public State CurrState { get; private set; }


    void Start()
    {
        isDead = false;
        transform.position = startPoint.position;
        CurrState = State.CutScene;
        StartCoroutine(CoCutSceneTime());
    }

    private IEnumerator CoCutSceneTime()
    {
        movement.enabled = false;
        yield return new WaitForSeconds(playerCutSceneTime);
        CurrState = State.Running;
        movement.enabled = true;
        Debug.Log("플레이 가능");
    }

    void Update()
    {
        
    }

    private void OnDead()
    {
        isDead = true;
        CurrState = State.Dead;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DeadZone")
        {
            OnDead();

            
        }
    }


}
