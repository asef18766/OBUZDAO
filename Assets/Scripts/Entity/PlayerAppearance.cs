using UnityEngine;

namespace Entity
{
    public class PlayerAppearance : MonoBehaviour
    {
        [SerializeField] private float moveScale = 0.5f;
        private Animator animator;
        private float moveSpeed = 0;

        private void Start()
        {
            this.animator = GetComponent<Animator>();
        }

        private void Update()
        {
            this.moveSpeed -= 10 * Time.deltaTime;
            this.moveSpeed = Mathf.Max(0, this.moveSpeed);
            animator.SetFloat("xSpeed", this.moveSpeed);
        }

        public void Move(Vector3 mvDir)
        {
            transform.position += mvDir * moveScale * Time.deltaTime;
            // setup rotation
            var rot = transform.eulerAngles;
            rot.y = mvDir.x > 0 ? 0 : 180;
            transform.eulerAngles = rot;
            // set speed
            this.moveSpeed = 1;
        }
    }
}