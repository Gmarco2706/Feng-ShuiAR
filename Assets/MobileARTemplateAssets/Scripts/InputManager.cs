
using System.Collections;
using System.Text;
using TMPro;

using UnityEngine;
using UnityEngine.UI;


using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


namespace Assets.MobileARTemplateAssets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField InputField;
        // Use this for initialization

        [SerializeField]
        private GameObject LabelPrefab;

        [SerializeField]
        private GameObject ObjectMenu;

        private GameObject etichettacorrente;


        [SerializeField]
        private GameObject buttoncorrente;

        [SerializeField]
        private Transform content;
        

        //metodo che gestisce l'inserimento del prefab etichetta in base a dove l'utente esegue il tap
        public void InsertObject()
        {
            if (InputField.text != "")
            {
                etichettacorrente=GameObject.Instantiate (LabelPrefab);
                
                etichettacorrente.GetComponentInChildren<TextMeshProUGUI>().text = InputField.text;

                GameObject button= GameObject.Instantiate(buttoncorrente,content);

                TextMeshProUGUI text= button.GetComponentInChildren<TextMeshProUGUI>();

                text.text = InputField.text;

             }



            InputField.text = "";
        }
        public void Delete()
        {

        }
    }

}