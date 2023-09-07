using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent playerNavMesh;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private FootstepController footstepController;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MoveToClickedPosition();
        }

        float velocity = playerNavMesh.velocity.magnitude / playerNavMesh.speed;
        playerAnimator.SetFloat("Speed", velocity);

        if (velocity == 0)
            footstepController.StopWalking();
        else
            footstepController.StartWalking();
    }

    private void MoveToClickedPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitFound = Physics.Raycast(ray, out hit);

        if (!hitFound) return;

        playerNavMesh.destination = hit.point;
    }
}