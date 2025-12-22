using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class BillBoard : MonoBehaviour
    {

        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (mainCamera == null) return;

            // Ruota l'oggetto per guardare la telecamera + 180 gradi (perché la UI è specchiata di base)
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}