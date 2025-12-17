using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneTapPlacingTry : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] GameObject placedPrefab;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    ARPlane selectedPlane;

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0];
            Pose hitPose = hit.pose;

            if (selectedPlane == null)
            {
                
                selectedPlane = planeManager.GetPlane(hit.trackableId);

                foreach (var plane in planeManager.trackables)
                {
                    if (plane != selectedPlane)
                        plane.gameObject.SetActive(false);
                }
            }

            Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
        }
    }
}
