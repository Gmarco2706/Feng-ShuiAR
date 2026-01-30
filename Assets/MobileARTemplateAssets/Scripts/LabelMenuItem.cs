using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Templates.AR;

public class LabelMenuItem : MonoBehaviour
{
    [SerializeField] TMP_Text nomeText;
    [SerializeField] ARTemplateMenuManager arMenuManager;

    LabelManager labelManager;
    GameObject prefabMondo;
    string nomeSalvato;
    BaguaZone classeSalvata;

    void Awake()
    {
        if (arMenuManager == null)
            arMenuManager = Object.FindFirstObjectByType<ARTemplateMenuManager>();
    }

    public void Init(string nome, BaguaZone classe, GameObject prefab, LabelManager manager)
    {
        nomeSalvato = nome;
        classeSalvata = classe;
        prefabMondo = prefab;
        labelManager = manager;

        if (nomeText != null)
            nomeText.text = nome;

        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //salva la sezione
        if (labelManager != null)
            labelManager.SelectCustomLabel(nomeSalvato, classeSalvata, prefabMondo);

        //indica allo spawner che tipo di prefab usare
        if (arMenuManager == null || arMenuManager.ObjectSpawner == null)
            return;

        var spawner = arMenuManager.ObjectSpawner;
        if (spawner.objectPrefabs == null || spawner.objectPrefabs.Count == 0)
            return;

        spawner.objectPrefabs[0] = prefabMondo;
        spawner.spawnOptionIndex = 0;
    }
}
