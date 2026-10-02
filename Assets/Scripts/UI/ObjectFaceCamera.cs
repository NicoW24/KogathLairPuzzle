using UnityEngine;

namespace Game.UI
{
    public class ObjectFaceCamera : MonoBehaviour
    {
        Transform mainCameraTransform;

        void Start()
        {
            if (Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }
        }

        void LateUpdate()
        {
            if (mainCameraTransform != null)
            {
                transform.LookAt(transform.position + mainCameraTransform.forward);
            }
        }
    }
}
