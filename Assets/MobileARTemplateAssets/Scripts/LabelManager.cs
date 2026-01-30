using UnityEngine;
using UnityEngine.XR.Templates.AR; // per ObjectSpawner event

public class LabelManager : MonoBehaviour
{
    [Header("Form")]
    [SerializeField] CreateLabelFormUGUI createForm;

    [Header("Menu")]
    [SerializeField] Transform customLabelContent;
    [SerializeField] GameObject etichettaGenericaMenuPrefab;

    [Header("Spawner")]
    [SerializeField] GameObject etichettaGenericaMondoPrefab;

    [Header("AR Template")]
    [SerializeField] ARTemplateMenuManager arMenuManager;

    string selectedNome;
    BaguaZone selectedClasse;
    GameObject selectedPrefab;

    void Awake()
    {
        if (createForm != null)
            createForm.OnConfirm += CreaEtichettaCustom;

        if (arMenuManager == null)
            arMenuManager = Object.FindFirstObjectByType<ARTemplateMenuManager>();
    }

    void Start()
    {
        // ci iscriviamo dopo che la scena ha inizializzato tutto
        if (arMenuManager != null && arMenuManager.ObjectSpawner != null)
            arMenuManager.ObjectSpawner.objectSpawned += OnObjectSpawned;
    }

    void OnDestroy()
    {
        if (createForm != null)
            createForm.OnConfirm -= CreaEtichettaCustom;

        if (arMenuManager != null && arMenuManager.ObjectSpawner != null)
            arMenuManager.ObjectSpawner.objectSpawned -= OnObjectSpawned;
    }

    public void SelectCustomLabel(string nome, BaguaZone classe, GameObject prefab)
    {
        selectedNome = nome;
        selectedClasse = classe;
        selectedPrefab = prefab;
    }

    void OnObjectSpawned(GameObject spawned)
    {
        // filtro: applico solo se la selezione corrente è il prefab generico custom
        if (selectedPrefab != etichettaGenericaMondoPrefab)
            return;

        var worldLabel = spawned.GetComponentInChildren<WorldLabel>(true);
        if (worldLabel != null)
            worldLabel.Apply(selectedNome, selectedClasse);
    }

    void CreaEtichettaCustom(string nome, BaguaZone classe)
    {
        if (customLabelContent == null || etichettaGenericaMenuPrefab == null)
        {
            Debug.LogError("LabelManager mancante di riferimenti");
            return;
        }

        var menuItemGO = Instantiate(etichettaGenericaMenuPrefab, customLabelContent);
        var menuItem = menuItemGO.GetComponent<LabelMenuItem>();
        if (menuItem != null)
        {
            menuItem.Init(nome, classe, etichettaGenericaMondoPrefab, this);
            Debug.Log($"✓ Etichetta '{nome}' aggiunta al menu");
        }
        else
        {
            Debug.LogError("LabelMenuItem mancante sul prefab");
        }
    }
}
