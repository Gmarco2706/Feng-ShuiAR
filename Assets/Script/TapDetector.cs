using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System;

// RIMUOVI: using System.Numerics;  ← PROBLEMA QUI
using UnityEngine.InputSystem;

namespace Assets.Script.Utils
{
    public class TapDetector : MonoBehaviour
    {
        [Header("Riferimenti AR")]
        [SerializeField] ARRaycastManager arRaycastManager;
        [SerializeField] ARPlaneManager aRPlaneManager;

        static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        public static event Action<Vector3, ARPlane> OnTapOnPlaneDetected;  // Unity Vector3
        public static event Action<Vector3> OnTapWorldDetected;

        void Awake()
        {
            if (arRaycastManager == null)
            arRaycastManager = FindFirstObjectByType<ARRaycastManager>();

        if (aRPlaneManager == null)
            aRPlaneManager = FindFirstObjectByType<ARPlaneManager>();
        }

        void Update()
        {
            Vector2 screenPos = Vector2.zero;

            if (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true)
            {
                screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
                ProcessTap(screenPos);
            }
            else if (Mouse.current?.leftButton.wasPressedThisFrame == true)
            {
                screenPos = Mouse.current.position.ReadValue();
                ProcessTap(screenPos);
            }
        }

        void ProcessTap(Vector2 screenPos)
        {
            Debug.Log($"TAP S10: {screenPos}");

            if (IsPointerOverUI(screenPos)) return;

            if (arRaycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
            {
                var pose = hits[0].pose;
                var plane = hits[0].trackable as ARPlane;
                if (plane != null)
                {
                    OnTapOnPlaneDetected?.Invoke(pose.position, plane);
                    Debug.Log("TAP PLANE OK!");
                }
            }
            else if (arRaycastManager.Raycast(screenPos, hits, TrackableType.AllTypes))
            {
                OnTapWorldDetected?.Invoke(hits[0].pose.position);
            }
        }

        bool IsPointerOverUI(Vector2 screenPosition)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = screenPosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
