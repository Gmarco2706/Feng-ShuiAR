using UnityEngine;
using System;

public class BaguaLabelDetector : MonoBehaviour
{
    public bool IsCorrectPlacement { get; private set; }
    public BaguaZone? CurrentZone { get; private set; }


    public event Action<bool, BaguaZone?> OnPlacementChanged;

    BaguaClassComponent classComp;

    void Awake()
    {
        classComp = GetComponent<BaguaClassComponent>();
        if (classComp == null)
            Debug.LogError("BaguaClassComponent non trovato sul GameObject.");
    }

    void OnTriggerEnter(Collider other)
    {
        var cellData = other.GetComponent<BaguaCellData>();
        if (cellData == null) return;

        // aggiorno stato
        CurrentZone = cellData.zone;

        // calcolo match/mismatch
        IsCorrectPlacement = (classComp != null && classComp.ClasseZona == cellData.zone);

        Debug.Log($"{name} entrato in zona {cellData.zone}. Match: {IsCorrectPlacement}");

        // Notifica live: qui “nasce” il contributo M/MM per il calcolo
        OnPlacementChanged?.Invoke(IsCorrectPlacement, CurrentZone); 
    }

    void OnTriggerExit(Collider other)
    {
        var cellData = other.GetComponent<BaguaCellData>();
        if (cellData == null) return;

        
        if (CurrentZone.HasValue && cellData.zone == CurrentZone.Value)
        {
            CurrentZone = null;
            IsCorrectPlacement = false;

            Debug.Log($"{name} uscito dalla zona {cellData.zone}. Reset.");

            // Notifica live: rimuovi contributo (ora “nessuna zona”)
            OnPlacementChanged?.Invoke(false, null);
        }
    }
}
