using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARDragRotateManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARRaycastManager arRaycastManager;

    [Header("Selection filter (optional)")]
    [SerializeField] private LayerMask selectableLayers = ~0; // tutto di default

    // AR raycast hits sono ordinati per distanza: hits[0] è il più vicino
    private static readonly List<ARRaycastHit> s_Hits = new();

    private Transform selected;
    private Vector3 offsetFromPlanePoint;

    // Two-finger twist rotation
    private bool rotating;
    private float prevTwistAngle;

    void Update()
    {
        if (Input.touchCount == 1) HandleOneFinger();
        else if (Input.touchCount == 2) HandleTwoFingers();
        else { selected = null; rotating = false; }
    }

    void HandleOneFinger()
    {
        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
        {
            // 1) Selezione oggetto (collider 3D)
            Ray ray = Camera.main.ScreenPointToRay(t.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableLayers))
            {
                selected = hit.transform;

                // 2) Calcola offset rispetto al punto sul plane AR sotto al dito
                if (arRaycastManager != null &&
                    arRaycastManager.Raycast(t.position, s_Hits, TrackableType.Planes))
                {
                    Vector3 planePoint = s_Hits[0].pose.position;
                    offsetFromPlanePoint = selected.position - planePoint;
                }
                else
                {
                    offsetFromPlanePoint = Vector3.zero;
                }
            }
            else
            {
                selected = null;
            }
        }

        if (selected != null && (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary))
        {
            // Sposta sul piano AR
            if (arRaycastManager != null &&
                arRaycastManager.Raycast(t.position, s_Hits, TrackableType.Planes))
            {
                Vector3 planePoint = s_Hits[0].pose.position;
                selected.position = planePoint + offsetFromPlanePoint;
            }
        }

        if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
        {
            selected = null;
        }
    }

    void HandleTwoFingers()
    {
        if (selected == null) return;

        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        // Angolo della linea tra le dita (twist gesture)
        float angle = Mathf.Atan2(t2.position.y - t1.position.y, t2.position.x - t1.position.x) * Mathf.Rad2Deg;

        if (!rotating || t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
        {
            prevTwistAngle = angle;
            rotating = true;
            return;
        }

        float delta = Mathf.DeltaAngle(prevTwistAngle, angle);

        // Ruota attorno all'asse verticale (Y). Se vuoi ruotare localmente usa Space.Self.
        selected.Rotate(0f, -delta, 0f, Space.World);

        prevTwistAngle = angle;
    }
}
