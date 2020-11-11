using UnityEngine;

namespace CameraOperation
{
    public class CameraController : MonoBehaviour
    {
        public Transform FollowTarget = null;

        private void Update()
        {
            if (FollowTarget == null) return;
            var targetPosition = FollowTarget.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        }
    }
}