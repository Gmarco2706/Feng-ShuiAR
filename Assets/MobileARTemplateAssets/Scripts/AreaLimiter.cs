using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class AreaLimiter : MonoBehaviour
{
    [Header("Plane rendering limit")]
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private Transform reference;// main camera
    [SerializeField] private float renderRadius = 2.0f; //metri di rendering 


    void Rest()
    {
        if (!arPlaneManager) arPlaneManager = FindFirstObjectByType<ARPlaneManager>();
        if (!reference && Camera.main) reference = Camera.main.transform;
    }
    void Update()
    {
        if (!arPlaneManager || !reference) return; 

        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane == null) continue; 
            float d = UnityEngine.Vector3.Distance(plane.transform.position, reference.position);
            bool shouldRender = d <= renderRadius;
            if (plane.gameObject.activeSelf != shouldRender)
            {
                plane.gameObject.SetActive(shouldRender);
            }
        }
        
    }
    //metodo per eventuale slider UI 
    public void SetRenderRadius(float radiusMeters) => renderRadius = Mathf.Max(0.1f, radiusMeters);
}