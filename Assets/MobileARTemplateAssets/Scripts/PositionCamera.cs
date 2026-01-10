using System.Collections;
using UnityEngine;

namespace Assets.MobileARTemplateAssets.Scripts
{
    public class PositionCamera : MonoBehaviour
    {

        // Use this for initialization

        private Camera mainCamera;
        void Start()
        {
           mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);

        }
    }
}