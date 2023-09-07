using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PortalController : MonoBehaviour
{
    public bool isBlocked = false;

    [SerializeField] private Transform teleportPosition;
    [SerializeField] private AudioSource portalAudioSource;
    private int FadeInAnimationHash = Animator.StringToHash("Teleport_In");
    private PortalController portalPair;
    private bool isActive = false;

    public void SetPortalPair(PortalController pair)
    {
        portalPair = pair;
    }

    public void ActivatePortal()
    {
        isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) return;

        isBlocked = true;

        if (!isActive) return;
        if (portalPair == null) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = portalPair.teleportPosition.position;
            other.transform.rotation = portalPair.teleportPosition.rotation;
            other.GetComponent<NavMeshAgent>().ResetPath();
            other.GetComponent<Animator>().CrossFadeInFixedTime(FadeInAnimationHash, 0f);
            portalAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) return;

        isBlocked = false;
    }
}