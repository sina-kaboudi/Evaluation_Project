using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PortalPlacer : MonoBehaviour
{
    public LayerMask groundLayerMask;

    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private float portalResetTime;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip portalPlacementClip;
    [SerializeField] private AudioClip portalDeletionClip;

    private const int MaxPortalsAllowed = 2;

    private RaycastHit hit;
    private Camera mainCamera;
    private PortalMaterialController portalMaterialController;
    private PortalController portalToBuild;
    private PortalController firstPortal;
    private PortalController secondPortal;
    private int numberOfPortalsPlaced = 0;
    private Coroutine clearPortalsCoroutine;

    public void ClearAllPortals()
    {
        if (firstPortal != null) Destroy(firstPortal.gameObject);
        if (secondPortal != null) Destroy(secondPortal.gameObject);

        StopCoroutine(clearPortalsCoroutine);
        playerAudioSource.PlayOneShot(portalDeletionClip);
        numberOfPortalsPlaced = 0;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        firstPortal = null;
        secondPortal = null;
    }

    private void Update()
    {
        if (numberOfPortalsPlaced >= MaxPortalsAllowed) return;

        bool isOverUI = EventSystem.current.IsPointerOverGameObject();
        if (isOverUI) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, groundLayerMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnPortalPreview(hit.point);
            }
            else if (Input.GetMouseButton(0))
            {
                MovePortalPreview();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                TryToPlacePortal();
            }
        }
    }

    private void SpawnPortalPreview(Vector3 position)
    {
        GameObject portalInstance = Instantiate(portalPrefab, position, Quaternion.identity);
        portalToBuild = portalInstance.GetComponent<PortalController>();
        portalMaterialController = portalInstance.GetComponent<PortalMaterialController>();
    }

    private void MovePortalPreview()
    {
        portalToBuild.transform.position = hit.point;

        if (portalToBuild.isBlocked)
            portalMaterialController.ChangePortalMaterial(PortalState.invalidPreview);
        else
            portalMaterialController.ChangePortalMaterial(PortalState.validPreview);
        
    }

    private void TryToPlacePortal()
    {
        if (portalToBuild.isBlocked)
        {
            Destroy(portalToBuild.gameObject);
            return;
        }

        portalMaterialController.ChangePortalMaterial(PortalState.placed);

        if (firstPortal == null)
            firstPortal = portalToBuild;
        else
            secondPortal = portalToBuild;

        portalToBuild.ActivatePortal();
        numberOfPortalsPlaced++;
        playerAudioSource.PlayOneShot(portalPlacementClip);

        if (numberOfPortalsPlaced >= MaxPortalsAllowed)
        {
            firstPortal.SetPortalPair(secondPortal);
            secondPortal.SetPortalPair(firstPortal);
            clearPortalsCoroutine = StartCoroutine(StartPortalResetCooldown());
        }
    }

    private IEnumerator StartPortalResetCooldown()
    {
        yield return new WaitForSeconds(portalResetTime);
        ClearAllPortals();
    }
}