using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using UnityEngine.Animations;
using System.Threading.Tasks;

public class BaguaPlacer : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARAnchorManager anchorManager;
    [SerializeField] GameObject baguaPrefab;
    [SerializeField] float canvasWidthPx = 600f;
    [SerializeField] float baguaHeightPx = 600f;
    [SerializeField] float fitMargin = 0.95f;
    [SerializeField] float yOffset = 0.01f;

    bool alignedToNorth = false;

    GameObject baguaInstance;
    ARAnchor baguaAnchor;

    void Start()
    {
        planeManager ??= FindFirstObjectByType<ARPlaneManager>();
        anchorManager ??= FindFirstObjectByType<ARAnchorManager>();
        Input.location.Start();
        Input.compass.enabled = true;
    }

    public void applyBagua()
    {
        ARPlane bestPlane = null;
        float maxScore = 0f;

        if (planeManager == null)
            planeManager = FindFirstObjectByType<ARPlaneManager>();

        if (anchorManager == null)
            anchorManager = FindFirstObjectByType<ARAnchorManager>();

        if (planeManager == null || planeManager.trackables.count == 0)
            return;

        Transform cam = Camera.main.transform;

        // Scegli il piano orizzontale più grande davanti alla camera
        foreach (var t in planeManager.trackables)
        {
            if (t is ARPlane p && p.alignment == PlaneAlignment.HorizontalUp)
            {
                float area = p.size.x * p.size.y;

                Vector3 toPlane = p.center - cam.position;
                float inFront = Vector3.Dot(cam.forward, toPlane.normalized);

                if (inFront <= 0f) continue;

                float score = area / (toPlane.sqrMagnitude + 0.01f);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestPlane = p;
                }
            }
        }

        if (bestPlane == null)
            return;

        if (baguaAnchor != null)
            Destroy(baguaAnchor.gameObject);

        if (anchorManager == null || anchorManager.descriptor == null || !anchorManager.descriptor.supportsTrackableAttachments)
            return;

        Pose pose = new Pose(bestPlane.center + Vector3.up * yOffset, bestPlane.transform.rotation);
        baguaAnchor = anchorManager.AttachAnchor(bestPlane, pose);

        if (baguaAnchor == null)
            return;

        if (baguaInstance == null)
            baguaInstance = Instantiate(baguaPrefab);

        baguaInstance.transform.SetParent(baguaAnchor.transform, false);

        //if per fare in modo che la mappa rimanga fissa e che non giri con il nord quando l'utente rotea irl
        if (!alignedToNorth)
        {
            float heading = Input.compass.trueHeading;
            baguaInstance.transform.localRotation *= Quaternion.Euler(0f, -heading, 0f);
            alignedToNorth = true;
        }

        float targetSize = Mathf.Min(bestPlane.size.x, bestPlane.size.y) * fitMargin; // metri
        float planeUnitySize = 10f;
        float scale = targetSize / planeUnitySize;

        baguaInstance.transform.localScale = new Vector3(scale, 1f, scale);

        var grid = baguaInstance.GetComponentInChildren<BaguaGrid>(true);
        if (grid != null)
            grid.SetupGridColliders();
        else
            Debug.LogError("BaguaGrid non trovato dentro baguaPrefab/baguaInstance.");

        Debug.Log($"Bagua su piano size {bestPlane.size}, targetSize {targetSize}, scale {scale}, pos {baguaInstance.transform.position}");
    }
}
