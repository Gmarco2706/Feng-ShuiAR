using UnityEngine;


//componente per salvare i dati di ogni cella
public class BaguaCellData : MonoBehaviour
{
    public BaguaZone zone;
}

public class BaguaGrid : MonoBehaviour
{
    [SerializeField] Transform baguaPlaneMap; //assegna la mappa bagua
    [SerializeField] Transform cellRoot; //assegna le celle
    

    //Dato che la logica delle classi sarà gestita da un file Enum con le varie classi, andiamo a modificare gli stringi vettore di enum per i vari zonename
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
            BaguaZone zone = sectors[index];
                var cellGO = new GameObject($"Cell_{rowIndex}_{colIndex}_{zone}");
            cellGO.transform.SetParent(cellRoot, false);

            float x = (-sizeLocal.x * 0.5f) + ((colIndex + 0.5f) * cellW);
            float z = (-sizeLocal.z * 0.5f) + ((rowIndex + 0.5f) * cellD);
            cellGO.transform.localPosition = new UnityEngine.Vector3(x, 0.02f, z);

            var box = cellGO.AddComponent<BoxCollider>();
            box.isTrigger = true;

           //per evitare problemi di rilevamenti AR del piano si ingrandisce in altezza la cella in modo che il collider sia pi� facilmente rilevabile
            box.size = new UnityEngine.Vector3(cellW, 10.0f, cellD);
            box.center = new UnityEngine.Vector3(0, 5.0f, 0);

            // Aggiungiamo il componente e salviamo la stringa dentro
            var data = cellGO.AddComponent<BaguaCellData>();
            data.zone = zone;

           index++;


        }
    }
}

}