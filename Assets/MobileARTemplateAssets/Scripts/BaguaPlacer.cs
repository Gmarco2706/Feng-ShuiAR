using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using UnityEngine.Animations;
using System.Threading.Tasks;



public class BaguaPlacer : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] GameObject baguaPrefab;
    [SerializeField] float canvasWidthPx = 600f;
    [SerializeField] float baguaHeightPx = 600f;
    [SerializeField] float fitMargin = 0.95f;
    [SerializeField] float yOffset = 0.01f;


    GameObject baguaInstance;



    void Start()
    {
        planeManager ??= FindFirstObjectByType<ARPlaneManager>();
        Input.location.Start();
        Input.compass.enabled = true;
    }

  public void applyBagua()
{
    ARPlane bestPlane = null;
    float maxScore = 0f;

    if (planeManager == null)
        planeManager = FindFirstObjectByType<ARPlaneManager>();

    if (planeManager == null || planeManager.trackables.count == 0)
        return;

    Transform cam = Camera.main.transform;

    // Scegli il piano orizzontale più grande DAVANTI alla camera
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

    
    if (baguaInstance == null)
        baguaInstance = Instantiate(baguaPrefab);


    baguaInstance.transform.position = bestPlane.center + Vector3.up * 0.01f;

    
    baguaInstance.transform.rotation = bestPlane.transform.rotation;

    
    float targetSize = Mathf.Min(bestPlane.size.x, bestPlane.size.y) * fitMargin; // metri
    float planeUnitySize = 10f;
    float scale = targetSize / planeUnitySize;

    baguaInstance.transform.localScale = new Vector3(scale, 1f, scale);

    Debug.Log($"Bagua su piano size {bestPlane.size}, targetSize {targetSize}, scale {scale}, pos {baguaInstance.transform.position}");
}


}