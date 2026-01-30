using UnityEngine;
using TMPro;

public class WorldLabel : MonoBehaviour
{
    [SerializeField] TMP_Text text3D;
    [SerializeField] BaguaClassComponent baguaClass; 

    public void Apply(string nome, BaguaZone classe)
    {
        if (text3D != null) text3D.text = nome;
        if (baguaClass != null) baguaClass.ClasseZona = classe; 
    }
}
