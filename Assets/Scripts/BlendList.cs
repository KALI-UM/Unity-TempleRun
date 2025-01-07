using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlendList : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public CinemachineBlendListCamera blendListCamera;
    public Camera mainCamera;

    void Update()
    {
        // Blend 진행 여부 확인
        if (cinemachineBrain.ActiveBlend == null && blendListCamera.gameObject.activeSelf)
        {
            OnBlendComplete();
        }
    }

    void OnBlendComplete()
    {
        blendListCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
}
