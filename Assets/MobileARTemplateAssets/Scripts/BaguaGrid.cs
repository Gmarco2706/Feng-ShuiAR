using System.Numerics;
using UnityEngine;


public class BaguaGrid : MonoBehaviour
{
    [SerializeField] Transform baguaPlaneMap; //assegna la mappa bagua
    [SerializeField] Transform cellRoot; //assegna le celle
    string[] sectors = {"SW", "S", "SE", "W", "CENTER", "E", "NW", "N", "NE"};
    

    void Start()
    {
        SetupGridColliders();
    }
    

    public void SetupGridColliders()
{
    var mr = baguaPlaneMap.GetComponent<MeshRenderer>();
    if (mr == null)
    {
        Debug.LogError("BaguaMapPlane: manca MeshRenderer.");
        return;
    }

   float usable = 0.95f;
   UnityEngine.Vector3 sizeLocal = mr.localBounds.size;
   sizeLocal = new UnityEngine.Vector3(sizeLocal.x * usable, sizeLocal.y, sizeLocal.z * usable);

    float cellW = sizeLocal.x / 3f;
    float cellD = sizeLocal.z / 3f;

    int index = 0;
    for (int rowIndex = 0; rowIndex < 3; rowIndex++)
    {
        for (int colIndex = 0; colIndex < 3; colIndex++)
        {
            var cellGO = new GameObject($"Cell_{rowIndex}_{colIndex}_{sectors[index]}");
            cellGO.transform.SetParent(cellRoot, false);

            float x = (-sizeLocal.x * 0.5f) + ((colIndex + 0.5f) * cellW);
            float z = (-sizeLocal.z * 0.5f) + ((rowIndex + 0.5f) * cellD);
            cellGO.transform.localPosition = new UnityEngine.Vector3(x, 0.02f, z);

            var box = cellGO.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.size = new UnityEngine.Vector3(cellW, 0.1f, cellD);

            index++;
        }
    }
}

}