using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Assets.Script.Utils
{
    public class PlaneHeightChecker : MonoBehaviour
    {
        [Header("Soglia massima differenza altezza (metri)")]
        [SerializeField] private float maxHeightDifference = 0.1f;

        public delegate void HeightCheckEvent(ARPlane plane1, ARPlane plane2, float difference);
        public static event HeightCheckEvent OnHeightDifferenceOK;

        
        public float CheckHeightDifference(ARPlane plane1, ARPlane plane2)
        {
            if (plane1 == null || plane2 == null) return -1f;

            float height1 = plane1.transform.position.y;
            float height2 = plane2.transform.position.y;
            float difference = Mathf.Abs(height1 - height2);

            if (difference <= maxHeightDifference)
            {
                OnHeightDifferenceOK?.Invoke(plane1, plane2, difference);
                return difference;
            }
            return -1f;
        }

        
        public static float GetStaticHeightDifference(ARPlane plane1, ARPlane plane2, float maxDiff = 0.1f)
        {
            if (plane1 == null || plane2 == null) return -1f;
            float difference = Mathf.Abs(plane1.transform.position.y - plane2.transform.position.y);
            return difference <= maxDiff ? difference : -1f;
        }
    }
}
