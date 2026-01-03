using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Assets.Script.Utils;
using System.Collections.Generic;
using System.Linq;

public class ARFloorManager : MonoBehaviour
{
    [Header("Riferimenti (auto-find)")]
    [SerializeField] ARPlaneManager aRPlaneManager;

    [SerializeField] PlaneHeightChecker heightChecker;
    [SerializeField] PlaneLabelController labelController;

    [Header("Visual")]
    [SerializeField] float transparencyAmount = 0.3f;
    [SerializeField] LayerMask wallLayer = 1 << 8;  // Layer muri

    private ARPlane lastValidPlane;
    private HashSet<ARPlane> validFloorPlanes = new HashSet<ARPlane>();
    private HashSet<ARPlane> hiddenPlanes = new HashSet<ARPlane>();

    private void Awake()
    {
        if (aRPlaneManager == null) aRPlaneManager = Object.FindAnyObjectByType<ARPlaneManager>();
        if (heightChecker == null) heightChecker = Object.FindAnyObjectByType<PlaneHeightChecker>();
        if (labelController == null) labelController = Object.FindAnyObjectByType<PlaneLabelController>();

        TapDetector.OnTapOnPlaneDetected += OnPlaneTapped;
    }

    private void OnDestroy()
    {
        TapDetector.OnTapOnPlaneDetected -= OnPlaneTapped;
    }
    // Gestisce il tap su un piano 
    private void OnPlaneTapped(Vector3 position, ARPlane plane)
    {
        if (lastValidPlane != null)
        {
            float heightDiff = heightChecker.CheckHeightDifference(lastValidPlane, plane);
            if (heightDiff >= 0)
            {
                AddToValidFloor(plane);
                ApplyTransparentMaterial(plane.gameObject);
                HideDifferentHeightPlanes(plane);
                
                
                if (labelController != null)
                    labelController.LabelCarpet(validFloorPlanes, "Floor Carpet");

                Debug.Log($"Tappeto: {validFloorPlanes.Count} piani!");
            }
            else
            {
                Debug.Log("Altezza diversa: nascosto");
                HidePlane(plane);
            }
        }
        else
        {
            lastValidPlane = plane;
            AddToValidFloor(plane);
            if (labelController != null)
                labelController.LabelCarpet(validFloorPlanes, "Touched");
        }
    }
    // Aggiunge un piano alla lista dei piani validi per la mesh
    private void AddToValidFloor(ARPlane plane)
    {
        validFloorPlanes.Add(plane);
    }
    // Nasconde i piani con altezza differente rispetto al piano di riferimento
    private void HideDifferentHeightPlanes(ARPlane referencePlane)
    {
        foreach (var trackable in aRPlaneManager.trackables)
        {
            if (trackable is ARPlane otherPlane && !validFloorPlanes.Contains(otherPlane))
            {
                float diff = heightChecker.CheckHeightDifference(referencePlane, otherPlane);
                if (diff < 0)
                    HidePlane(otherPlane);
            }
        }
    }
    // Nasconde un piano specifico
    private void HidePlane(ARPlane plane)
    {
        if (!hiddenPlanes.Contains(plane) && plane.gameObject.activeSelf)
        {
            plane.gameObject.SetActive(false);
            hiddenPlanes.Add(plane);
        }
    }

    private void ApplyTransparentMaterial(GameObject planeObj)
    {
        Renderer rend = planeObj.GetComponent<Renderer>();
        if (rend != null)
        {
            Material newMat = new Material(rend.material);
            newMat.color = new Color(0, 1, 1, transparencyAmount);  // Azzurro trasparente
            newMat.SetFloat("_Mode", 3);  // Transparent mode
            rend.material = newMat;
        }
    }

    //per ogni frame, controlla se qualche piano valido è nascosto da un muro
    private void Update()
    {
        Camera arCam = Camera.main;
        if (arCam == null) return;

        foreach (ARPlane plane in validFloorPlanes.ToList())
        {
            if (plane == null || !plane.gameObject.activeSelf) continue;

            Vector3 planeCenter = plane.transform.position;
            Vector3 screenPos = arCam.WorldToScreenPoint(planeCenter);
            Ray ray = arCam.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, wallLayer))
            {
                if (hit.distance < Vector3.Distance(arCam.transform.position, planeCenter))
                {
                    plane.gameObject.SetActive(false);
                    hiddenPlanes.Add(plane);
                }
            }
        }
    }
}
