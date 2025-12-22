using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace Assets.Script
{
    public class AutoLabelButton : MonoBehaviour
    {

        void Start()
        {
            Button myButton = GetComponent<Button>();

            TextMeshProUGUI buttonText = myButton.GetComponentInChildren<TextMeshProUGUI>();

            PlaneLabelController labelController = GetComponentInParent<PlaneLabelController>();

            if (myButton != null && buttonText != null && labelController != null)
            {
                //utente clicca il bottone
                myButton.onClick.AddListener(() =>
                {
                    //richiama il metodo che recupera l'etichetta del bottono selezionato
                    labelController.SelectLabel(buttonText.text);
                });
            }
            else
            {
                Debug.LogWarning($"AutoLabelButton: Manca qualche componente sul bottone {gameObject.name}");
            }
        }
        
        
        
        

     
    }
}