using UnityEngine;

// serve solo a creare il campo e metterlo come componente per ogni etichetta prefab e a poterla cambiare in seguito
public class BaguaClassComponent : MonoBehaviour
{
    [Header("Classe (zona corretta) di questo oggetto")]
    [SerializeField] private BaguaZone classeZona;

    public BaguaZone ClasseZona
    {
        get => classeZona;
        set => classeZona = value;
    }
}
