using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneCreationLimiter : MonoBehaviour
{
    [Header("filter settings")]
    public float minArea = 0.5f;
    public int minVertices = 30;
    public float maxHeight = 0.1f;
    public float relativeAreaThreshold = 0.3f;

    private ARPlaneManager planeManager;
    private ARPlane mainPlane;
    private float mainArea = 0.0f;
    private readonly HashSet<ARPlane> planesToHide = new HashSet<ARPlane>();

    void Awake()
    {
        planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (planeManager == null)
        {
            Debug.LogError("ARPlaneManager non trovato nella scena.");
            return;
        }
        planeManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    void OnDestroy()
    {
        if (planeManager != null)
            planeManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
{
    // Gestisci added: filtra orientamento immediatamente
    foreach (var plane in args.added)
    {
        if (plane == null || !plane.gameObject) continue;
        if (plane.alignment != PlaneAlignment.HorizontalUp && plane.alignment != PlaneAlignment.HorizontalDown)
        {
            plane.gameObject.SetActive(false);
            continue;
        }
    }

    // Gestisci updated in sicurezza 
    foreach (var plane in args.updated)
    {
        if (plane == null || !plane.gameObject) continue;
    }

    
    foreach (var kvp in args.removed)
    {
        var plane = kvp.Value; // Accedi al Value
        if (plane == null) continue;
        
        if (planesToHide.Contains(plane))
            planesToHide.Remove(plane);
        if (plane == mainPlane)
        {
            mainPlane = null;
            mainArea = 0f;
        }
    }

    UpdateMainPlane();
    FilterPlanes();
}


    void UpdateMainPlane()
    {
        mainArea = 0f;
        mainPlane = null;
        foreach (var plane in planeManager.trackables)
        {
            if (plane == null || !plane.gameObject) continue;
            float planeArea = plane.extents.x * plane.extents.y * 4f;
            if (planeArea > mainArea)
            {
                mainArea = planeArea;
                mainPlane = plane;
            }
        }
    }

    void FilterPlanes()
{
    if (mainArea < minArea) return;

    foreach (var plane in planeManager.trackables)
    {
        if (plane == null || !plane.gameObject || plane == mainPlane) continue;

        float planeArea = plane.extents.x * plane.extents.y * 4f;
        int vertexCount = plane.boundary.IsCreated ? plane.boundary.Length : 0;

        bool shouldHideVisual = planeArea < minArea ||
                                vertexCount < minVertices ||
                                plane.transform.position.y > maxHeight ||
                                planeArea < mainArea * relativeAreaThreshold;

        if (shouldHideVisual)
        {
           
            var meshRenderer = plane.GetComponent<MeshRenderer>();
            if (meshRenderer != null) meshRenderer.enabled = false;
            
            
            var collider = plane.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
        }
    }
}

}