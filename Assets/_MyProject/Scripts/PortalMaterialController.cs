using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalState
{
    validPreview,
    invalidPreview,
    placed
}

public class PortalMaterialController : MonoBehaviour
{
    [SerializeField] private Material validPreviewMaterial;
    [SerializeField] private Material invalidPreviewMaterial;
    [SerializeField] private Material placedMaterial;
    [SerializeField] private MeshRenderer portalMeshRenderer;

    public void ChangePortalMaterial(PortalState state)
    {
        switch (state)
        {
            case PortalState.validPreview:
                portalMeshRenderer.material = validPreviewMaterial;
                break;

            case PortalState.invalidPreview:
                portalMeshRenderer.material = invalidPreviewMaterial;
                break;

            case PortalState.placed:
                portalMeshRenderer.material = placedMaterial;
                break;

            default:
                break;
        }
    }
}