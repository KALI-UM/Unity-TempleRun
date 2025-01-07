using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCameraMovement : MonoBehaviour
{
    public Transform playerPivot;
    public float rotationSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = playerPivot.position;
        Quaternion targetRotation = playerPivot.rotation;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}
