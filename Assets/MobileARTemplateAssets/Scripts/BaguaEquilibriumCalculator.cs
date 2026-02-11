using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BaguaEquilibriumCalculator : MonoBehaviour
{
    [Header("Formula A")]
    [SerializeField] float mismatchPenaltyK = 1.5f;

    [Header("Optional UI output")]
    [SerializeField] TMP_Text outputText;

    [Header("Debug")]
    [SerializeField] bool debugLogs = false;

    
    readonly Dictionary<BaguaLabelDetector, (bool isCorrect, BaguaZone? zone)> state
        = new Dictionary<BaguaLabelDetector, (bool isCorrect, BaguaZone? zone)>();

    int M;   // match
    int MM;  // mismatch

    void OnEnable()
    {
        RegisterAllDetectorsInScene();

        if (debugLogs)
            Debug.Log($"[Equilibrium] OnEnable: registrati={state.Count}, M={M}, MM={MM}", this);
    }

    void OnDisable()
    {
        foreach (var kv in state)
        {
            if (kv.Key != null)
                kv.Key.OnPlacementChanged -= OnAnyPlacementChanged;
        }

        state.Clear();
        M = 0;
        MM = 0;
    }

    void RegisterAllDetectorsInScene()
    {
        var detectors = Object.FindObjectsByType<BaguaLabelDetector>(FindObjectsSortMode.None); // [web:288]

        if (debugLogs)
            Debug.Log($"[Equilibrium] RegisterAll: trovati={detectors.Length}", this);

        foreach (var d in detectors)
            Register(d);
    }

    void Register(BaguaLabelDetector detector)
    {
        if (detector == null) return;
        if (state.ContainsKey(detector)) return;

        detector.OnPlacementChanged += OnAnyPlacementChanged;

        var initial = (detector.IsCorrectPlacement, detector.CurrentZone);
        state[detector] = initial;

        
        ApplyDelta(oldValue: (false, null), newValue: initial);

        if (debugLogs)
            Debug.Log($"[Equilibrium] Register: {detector.name} zone={initial.Item2} correct={initial.Item1} => M={M}, MM={MM}", detector);
    }

    void OnAnyPlacementChanged(bool isCorrect, BaguaZone? zone)
    {
        
        
        foreach (var d in new List<BaguaLabelDetector>(state.Keys))
        {
            if (d == null) continue;

            var current = (d.IsCorrectPlacement, d.CurrentZone);
            var prev = state[d];

            if (current.Equals(prev)) continue;

            state[d] = current;
            ApplyDelta(prev, current);

            if (debugLogs)
                Debug.Log($"[Equilibrium] STATE CHANGE: {d.name} {prev.Item2}->{current.Item2} correct={current.Item1} => M={M}, MM={MM}", d);
        }
    }

    void ApplyDelta((bool isCorrect, BaguaZone? zone) oldValue, (bool isCorrect, BaguaZone? zone) newValue)
    {
        // rimuovi contributo precedente
        if (oldValue.zone.HasValue)
        {
            if (oldValue.isCorrect) M--;
            else MM--;
        }

        // aggiungi contributo nuovo
        if (newValue.zone.HasValue)
        {
            if (newValue.isCorrect) M++;
            else MM++;
        }

        // clamp di sicurezza
        if (M < 0) M = 0;
        if (MM < 0) MM = 0;
    }

    public void CalculateEquilibrium()
    {
        
        RegisterAllDetectorsInScene();

        if (debugLogs)
            Debug.Log($"[Equilibrium] CLICK: registrati={state.Count}, M={M}, MM={MM}", this);

        float equilibrio = (M + MM) == 0
            ? 0f
            : 100f * (M / (M + mismatchPenaltyK * (float)MM));

        string msg = $"Equilibrio:{equilibrio:0.#}%";
        Debug.Log(msg, this);

        if (outputText != null)
            outputText.text = msg;
    }
}
