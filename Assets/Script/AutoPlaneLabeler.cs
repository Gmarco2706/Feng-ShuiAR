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

        
        private ARPlane lockedPlane = null;

        private void Awake()
        {
            if (!arRaycastManager) arRaycastManager = GetComponent<ARRaycastManager>();
            if (!arPlaneManager) arPlaneManager = GetComponent<ARPlaneManager>();
        }

        void Update()
        {
            if (Input.touchCount == 0)
                return;

            Touch touch = Input.GetTouch(0);

            if (IsPointerOverUI(touch))
                return;

            if (touch.phase == TouchPhase.Began) return;


            if (arRaycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                ARRaycastHit hit = hits[0];
                ARPlane aRPlane = arPlaneManager.GetPlane(hit.trackableId);

                if (lockedPlane == null)
                {
                    // Il primo piano che tocchi diventa il principale
                    lockedPlane = aRPlane;

                    // Vengono nascosti tutti gli altri piani
                    HideOtherPlanes(lockedPlane);

                    // Creiamo la prima etichetta
                    CreateLabelOnPlane(lockedPlane, hit.pose);
                }

                //controllo se il piano rilevato è quello corrente
                else
                {
                    // Controlliamo: stiamo toccando PROPRIO quel piano lì?
                    if (aRPlane == lockedPlane)
                    {
                        // SI: Allora permettiamo di aggiungere altre etichette
                        CreateLabelOnPlane(lockedPlane, hit.pose);
                    }
                    else
                    {
                        // NO: Stiamo toccando un altro piano (es. un muro), lo ignoriamo.
                        Debug.Log("Tocco ignorato: stiamo lavorando solo sul piano bloccato.");
                    }
                }
            }
        }

        void CreateLabelOnPlane(ARPlane plane, Pose pose)
        {
            GameObject label = Instantiate(labelPrefab, pose.position, pose.rotation);
            
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

        //funzione che disabilita la rilevazione di altri piani 
        void HideOtherPlanes(ARPlane keeper)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                if (plane != keeper)
                {
                    plane.gameObject.SetActive(false);
                }
            }
        }
}   }