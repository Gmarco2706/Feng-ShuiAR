using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System.Numerics;



namespace Assets.Script.Utils
{
public class TapDetector : MonoBehaviour
{
    [Header("Riferimenti AR (auto-find se null)")]
    [SerializeField] ARRaycastManager arRaycastManager;
    [SerializeField] ARPlaneManager aRPlaneManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public static event Action<UnityEngine.Vector3, ARPlane> OnTapOnPlaneDetected;
    public static event Action<UnityEngine.Vector3> OnTapWorldDetected;

        [Obsolete]
        private void Awake()
    {
        if (arRaycastManager == null)
            arRaycastManager = FindObjectOfType<ARRaycastManager>();
        if (aRPlaneManager == null)
            aRPlaneManager = FindObjectOfType<ARPlaneManager>();
    }
    void Update()
        {
            if (Input.touchCount == 0) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) return;

            if (IsPointerOverUI(touch.position)) return;

            // Raycast su PIANI AR
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;
                ARPlane plane = aRPlaneManager.GetPlane(hits[0].trackableId);
                
                OnTapOnPlaneDetected?.Invoke(pose.position, plane);
            }
            else
            {
                // Raycast qualsiasi superficie (fallback)
                if (arRaycastManager.Raycast(touch.position, hits, TrackableType.AllTypes))
                {
                    OnTapWorldDetected?.Invoke(hits[0].pose.position);
                }
            }
        }
    bool IsPointerOverUI(UnityEngine.Vector2 screenPosition)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = screenPosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
}
}