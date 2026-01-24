using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Assets.MobileARTemplateAssets.Scripts
{
    public class BaguaLabelDetector : MonoBehaviour
    {
        
        private GameObject etichettaCorrente = null;
        //metodo chiamato quando l'oggetto con questo script(prefab etichetta) entra in collisione con un altro collider
        public void OnTriggerEnter(Collider other)
        {
            BaguaCellData cellData = other.GetComponent<BaguaCellData>();
            if (cellData != null)
            {
                Debug.Log(" entrato nella zona: " + cellData.zoneName);


                //in base al collider aggiunto sul prefab dell'etichetta,salvo l'oggetto etichettaCorrente
                etichettaCorrente = this.gameObject;

                TextMeshProUGUI Text = etichettaCorrente.GetComponentInChildren<TextMeshProUGUI>();
                if (Text != null)
                {
                    Text.color = Color.green;
                    Text.text = "zona:" + cellData.zoneName;
                }
                print("l'etichetta " + Text + " è entrata nella zona: " + cellData.zoneName);



                //Debug.Log("l'etichetta " + Text + " è entrata nella zona: " + cellData.zoneName);   



            }else 
                {
                Debug.Log("L'oggetto non ha il componente BaguaCellData.");
            }
        }
    }
}