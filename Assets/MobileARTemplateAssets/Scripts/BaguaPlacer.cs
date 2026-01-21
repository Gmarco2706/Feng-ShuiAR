using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BaguaPlacer : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARAnchorManager anchorManager;
    [SerializeField] GameObject baguaPrefab;

    [SerializeField] float fitMargin = 0.95f;
    [SerializeField] float yOffset = 0.01f;

    bool alignedToNorth = false;

    GameObject baguaInstance;
    ARAnchor baguaAnchor;

    void Start()
    {
        if (planeManager == null) planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (anchorManager == null) anchorManager = FindFirstObjectByType<ARAnchorManager>();

        Input.location.Start();
        Input.compass.enabled = true;
    }

    public void applyBagua()
    {
        Debug.Log("INIZIA CODICE BAGUA!!!!!!!!!!!!!!!");

        if (planeManager == null) planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (anchorManager == null) anchorManager = FindFirstObjectByType<ARAnchorManager>();

        if (planeManager == null || planeManager.trackables.count == 0)
        {
            Debug.Log("Nessun piano trovato.");
            return;
        }

        if (anchorManager.descriptor == null || !anchorManager.descriptor.supportsTrackableAttachments)
        {
            Debug.LogError("AttachAnchor non supportato (supportsTrackableAttachments = false).");
            return;
        }

        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Camera.main non trovata.");
            return;
        }
        Transform cam = mainCam.transform;

        // 1) Trova il piano orizzontale più grande davanti alla camera
        ARPlane bestPlane = null;
        float bestArea = 0f;

        foreach (var t in planeManager.trackables)
        {
            ARPlane p = t as ARPlane;
            if (p == null) continue;
            if (p.alignment != PlaneAlignment.HorizontalUp) continue;

            // davanti alla camera?
            Vector3 toPlane = (p.transform.position - cam.position);
            float inFront = Vector3.Dot(cam.forward, toPlane.normalized);
            if (inFront <= 0f) continue;

            float area = p.size.x * p.size.y; // metri^2 [web:62]
            if (area > bestArea)
            {
                bestArea = area;
                bestPlane = p;
            }
        }

        if (bestPlane == null)
        {
            Debug.Log("Nessun piano orizzontale valido davanti alla camera.");
            return;
        }

        // 2) Distruggi anchor precedente
        if (baguaAnchor != null)
        {
            Destroy(baguaAnchor.gameObject);
            baguaAnchor = null;
        }

        // 3) Pose stabile sul centro del piano scelto
        Vector3 planePos = bestPlane.transform.position;
        Pose pose = new Pose(planePos + Vector3.up * yOffset, Quaternion.identity);

        baguaAnchor = anchorManager.AttachAnchor(bestPlane, pose);
        if (baguaAnchor == null)
        {
            Debug.LogError("AttachAnchor ha restituito null.");
            return;
        }

        // 4) Ricrea sempre l'istanza
        if (baguaInstance != null) Destroy(baguaInstance);
        baguaInstance = Instantiate(baguaPrefab, baguaAnchor.transform);
        baguaInstance.SetActive(true);

        // Debug utili
        Debug.Log($"BEST plane size={bestPlane.size} area={bestArea} planePos={planePos}");
        Debug.Log($"bagua worldPos={baguaInstance.transform.position} activeInHierarchy={baguaInstance.activeInHierarchy}");

        // 5) Allinea una sola volta al nord
        if (!alignedToNorth)
        {
            float heading = Input.compass.trueHeading;
            baguaInstance.transform.localRotation *= Quaternion.Euler(0f, -heading, 0f);
            alignedToNorth = true;
        }

        // 6) Scala per adattarsi al piano più grande
        float targetSize = Mathf.Min(bestPlane.size.x, bestPlane.size.y) * fitMargin; // metri
        float planeUnitySize = 10f; // dimensione "nativa" del tuo prefab in Unity units
        float scale = targetSize / planeUnitySize;
        baguaInstance.transform.localScale = new Vector3(scale, 1f, scale);

        var grid = baguaInstance.GetComponentInChildren<BaguaGrid>(true);
        if (grid != null) grid.SetupGridColliders();

        Debug.Log("FINE CODICE BAGUA!!!!!!!!!!!!!!!");
    }
}
