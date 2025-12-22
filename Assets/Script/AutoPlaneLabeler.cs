using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


namespace Assets.Script
{
    public class AutoPlaneLabeler : MonoBehaviour
    {
        [SerializeField] ARRaycastManager arRaycastManager;
        [SerializeField] ARPlaneManager arPlaneManager;


        [SerializeField] GameObject labelPrefab;

        static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        //dizionario per tenere traccia delle etichette già associate ai piani
        private Dictionary<ARPlane,GameObject> planeLabels = new Dictionary<ARPlane, GameObject>();


        
        void Update()
        {
            if (Input.touchCount==0)
                return;

            Touch touch = Input.GetTouch(0);

            if (IsPointerOverUI(touch))
                return;

            if (touch.phase == TouchPhase.Began) return;


            if (arRaycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                ARRaycastHit hit = hits[0];
                ARPlane aRPlane = arPlaneManager.GetPlane(hit.trackableId);

                //controllo se il piano rilevato è già stato memorizzato nel dizionario e dunque etichettato
                if(!planeLabels.ContainsKey(aRPlane))
                {
                    //viene assegnata l'etichetta al nuovo piano rilevato
                    CreateLabelOnPlane(aRPlane, hit.pose);

                }
                else
                {
                    //opzionalmente, si potrebbe aggiornare la posizione dell'etichetta esistente
                    Debug.Log("Piano già etichettato.");
                }
            }
        }

        void CreateLabelOnPlane(ARPlane plane, Pose pose)
        {
            GameObject label = Instantiate(labelPrefab, pose.position, pose.rotation);
            
            planeLabels.Add(plane, label);
        }

        //funzione per verificare se il tocco è sopra un bottone del menu UI
        bool IsPointerOverUI(Touch touch)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(touch.position.x, touch.position.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}