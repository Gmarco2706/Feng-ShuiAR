using UnityEngine;

// Componente per salvare i dati di ogni cella
public class BaguaCellData : MonoBehaviour
{
    public BaguaZone zone;
}

public class BaguaGrid : MonoBehaviour
{
    [SerializeField] Transform baguaPlaneMap; // assegna la mappa bagua
    [SerializeField] Transform cellRoot;      // assegna il parent delle celle

    // 3x3, ordine: riga 0..2, colonna 0..2
    BaguaZone[] sectors =
    {
        BaguaZone.SudOvest_Relazioni,  BaguaZone.Sud_Fama,           BaguaZone.SudEst_Ricchezza,
        BaguaZone.Ovest_Creativita,    BaguaZone.Centro_Salute,      BaguaZone.Est_Famiglia,
        BaguaZone.NordOvest_Aiuti,     BaguaZone.Nord_Carriera,      BaguaZone.NordEst_Conoscenza
    };

    void Start()
    {
        SetupGridColliders();
    }

    public void SetupGridColliders()
    {
        if (sectors.Length != 9)
        {
            Debug.LogError("BaguaGrid: sectors deve avere esattamente 9 elementi (3x3).");
            return;
        }

        var mr = baguaPlaneMap.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            Debug.LogError("BaguaMapPlane: manca MeshRenderer.");
            return;
        }

        float usable = 0.95f;

        // bounds in spazio locale del renderer [web:295]
        Vector3 sizeLocal = mr.localBounds.size;
        sizeLocal = new Vector3(sizeLocal.x * usable, sizeLocal.y, sizeLocal.z * usable);

        float cellW = sizeLocal.x / 3f;
        float cellD = sizeLocal.z / 3f;

        int index = 0;
        for (int rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            for (int colIndex = 0; colIndex < 3; colIndex++)
            {
                BaguaZone zone = sectors[index];

                var cellGO = new GameObject($"Cell_{rowIndex}_{colIndex}_{zone}");
                cellGO.transform.SetParent(cellRoot, worldPositionStays: false); // mantiene transform locali [web:305]

                float x = (-sizeLocal.x * 0.5f) + ((colIndex + 0.5f) * cellW);
                float z = (-sizeLocal.z * 0.5f) + ((rowIndex + 0.5f) * cellD);
                cellGO.transform.localPosition = new Vector3(x, 0.02f, z);

                var box = cellGO.AddComponent<BoxCollider>();
                box.isTrigger = true; // trigger per OnTriggerEnter/Exit [web:328]

                // collider alto per facilitare il rilevamento
                box.size = new Vector3(cellW, 10.0f, cellD);
                box.center = new Vector3(0, 5.0f, 0);

                var data = cellGO.AddComponent<BaguaCellData>();
                data.zone = zone;

                index++;
            }
        }
    }
}
