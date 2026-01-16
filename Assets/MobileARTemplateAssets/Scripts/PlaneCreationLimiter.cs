using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlaneCreationLimiter : MonoBehaviour
{
    [Header("filter settings")]
    public float minArea = 0.5f; // metri quadrati
    public int minVertices = 30; // numero minimo di vertici del piano
    public float maxHeight = 0.1f; // aletzza max dal suolo
    public float relativeAreaThreshold = 0.3f; // percentuale minima di area rispetto al bounding box

    private ARPlaneManager planeManager;
    private ARPlane mainPlane;
    private float mainArea = 0.0f; // metri quadrati
    private readonly List<ARPlane> tempPlanes = new List<ARPlane>(); //per ritardare la distruzione dei piani altrimenti non partirebbe neanche la scan iniziale

    void Awake()
    {
        planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (planeManager == null)
        {
            Debug.LogError("ARPlaneManager not found in the scene.");
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
        foreach (var plane in args.added)
        {
            // Mantieni solo piani orizzontali con faccia in su
            if (plane.alignment != PlaneAlignment.HorizontalUp && plane.alignment != PlaneAlignment.HorizontalDown)
            {
                Destroy(plane.gameObject);
                continue;
            }
            tempPlanes.Add(plane);
        }
        UpdateMainPlane();
        FilterPlane();
    }

    void UpdateMainPlane()
    {
        foreach (var plane in planeManager.trackables)
        {
            // Calcola area dal extent (extent = metà dimensione)
            float planeArea = plane.extents.x * plane.extents.y * 4f;
            
            if (planeArea > mainArea)
            {
                mainPlane = plane;
                mainArea = planeArea;
            }   
        }
    }

    void FilterPlane()
    {
        if (mainArea < minArea) return; // Aspetta piano principale

        var toDestroy = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            if (plane == mainPlane) continue;

            // Calcola area dal extent (extent = metà dimensione)
            float planeArea = plane.extents.x * plane.extents.y * 4f;
            int vertexCount = plane.boundary.Length;

            bool destroy = planeArea < minArea ||
                           vertexCount < minVertices ||
                           plane.transform.position.y > maxHeight ||
                           (mainArea > 0 && planeArea < mainArea * relativeAreaThreshold);

            if (destroy) toDestroy.Add(plane);
        }

        foreach (var plane in toDestroy)
        {
            Destroy(plane.gameObject);
        }
        tempPlanes.Clear();
    }



}
