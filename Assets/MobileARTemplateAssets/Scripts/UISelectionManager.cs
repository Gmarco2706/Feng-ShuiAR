using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MobileARTemplateAssets.Scripts
{
    public class UISelectionManager : MonoBehaviour
    {

        public List<GameObject> allSelectionBoxes;

        // Questa funzione la colleghi al bottone
        public void SelectOnly(GameObject activeBox)
        {
            // Spegniamo tutti gli altri riquadri di selezione
            foreach (GameObject box in allSelectionBoxes)
            {
                if (box != null) box.SetActive(false);
            }

            // Riaccendiamo solo quello attivo
            if (activeBox != null) activeBox.SetActive(true);
        }
    }
}