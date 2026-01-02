using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PlaneLabelController : MonoBehaviour
{
    [Header("Label Settings")]
    [SerializeField] GameObject labelPrefab;  
    [SerializeField] float labelHeightOffset = 0.05f;
    [SerializeField] Color floorLabelColor = Color.green;
    [SerializeField] Color touchedLabelColor = Color.yellow;

    private GameObject currentCarpetLabel;


    public void LabelCarpet(HashSet<ARPlane> validPlanes, string labelText)
    {
        // Rimuovi label precedente
        if (currentCarpetLabel != null)
            Destroy(currentCarpetLabel);

        if (validPlanes.Count == 0 || labelPrefab == null) return;

        //Cerca il centro della mesh del piano
        Vector3 carpetCenter = validPlanes.Aggregate(Vector3.zero, (sum, plane) => sum + plane.transform.position) / validPlanes.Count;
        carpetCenter.y += labelHeightOffset;

        //Crea la label al centro
        currentCarpetLabel = Instantiate(labelPrefab, carpetCenter, Quaternion.LookRotation(Camera.main.transform.forward));
        
        TextMeshPro textComp = currentCarpetLabel.GetComponentInChildren<TextMeshPro>();
        if (textComp != null)
        {
            textComp.text = $"{labelText}\n({validPlanes.Count} piani)";
            textComp.fontSize = 3f;
            textComp.color = labelText.Contains("Floor") ? floorLabelColor : touchedLabelColor;
            textComp.alignment = TextAlignmentOptions.Center;
        }

        Debug.Log($"Label tappeto '{labelText}' → {carpetCenter}");
    }
}
