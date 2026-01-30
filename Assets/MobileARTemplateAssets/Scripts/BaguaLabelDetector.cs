using UnityEngine;
using TMPro;
using System;

public class BaguaLabelDetector : MonoBehaviour
{
    public bool IsCorrectPlacement {get; private set;} 
    public BaguaZone? CurrentZone {get; private set;}
    public event Action<bool, BaguaZone> OnPlacementChanged;
    BaguaClassComponent classComp;
    void Awake()
    {
        classComp = GetComponent<BaguaClassComponent>();
        if(classComp == null)
        {
            Debug.LogError("BaguaClassComponent non trovato sul GameObject.");
        }
    } 
    void OnTriggerEnter(Collider other)
    {
        var cellData = other.GetComponent<BaguaCellData>();
        if (cellData == null) return;

        //aggiornamento stato
        CurrentZone = cellData.zone;
        Debug.Log($"{name}" + $"entrato in zona {CurrentZone}");

        if (classComp == null)
        {
            IsCorrectPlacement = false;
            return;
        }

        IsCorrectPlacement = (classComp.ClasseZona == cellData.zone);

        Debug.Log($"{name} in zona {cellData.zone}. Match: {IsCorrectPlacement}");

        
    }
    void OnTriggerExit(Collider other)
    {
        var cellData = other.GetComponent<BaguaCellData>();
        if (cellData != null) return;

        //reset dell'etichetta
        if (CurrentZone.HasValue && cellData.zone == CurrentZone.Value)
        {
            CurrentZone = null;
            IsCorrectPlacement = false;
        }
    }
}
