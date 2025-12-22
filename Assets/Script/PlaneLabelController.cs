using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script
{
    //permette di mostrare il piano etichettato dopo la selezione dal menù
    public class PlaneLabelController : MonoBehaviour
    {

        [SerializeField] GameObject buttonsPanel;


        [SerializeField] TextMeshProUGUI displayLabel;
        // Use this for initialization

        //utile per leggere le etichette da altri script
        public string CurrentLabel { get; private set; }= "untagged";
        void Start()
        {
            buttonsPanel.SetActive(true);
            displayLabel.text = "";
        }

        public void SelectLabel(string textButton)
        {
            CurrentLabel = textButton;

            displayLabel.text = textButton;

            buttonsPanel.SetActive(false);
        }
        

    }
}