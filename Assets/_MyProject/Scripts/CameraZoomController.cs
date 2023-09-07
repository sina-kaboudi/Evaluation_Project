using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private int maxOrthoSize;
    [SerializeField] private int minOrthoSize;

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            playerCamera.m_Lens.OrthographicSize = Mathf.Max(playerCamera.m_Lens.OrthographicSize - 1, minOrthoSize);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            playerCamera.m_Lens.OrthographicSize = Mathf.Min(playerCamera.m_Lens.OrthographicSize + 1, maxOrthoSize);
        }
    }
}