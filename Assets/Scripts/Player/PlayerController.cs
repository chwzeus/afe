using AIWorldAdminRPG.NPC;
using UnityEngine;

namespace AIWorldAdminRPG.Player
{
    // 3D 탑다운/쿼터뷰 이동 + 상호작용
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float interactionRange = 2f;

        private Rigidbody rb;
        private Vector3 movement;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");
            movement.y = 0f;

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }

        private void FixedUpdate()
        {
            var delta = movement.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + delta);
        }

        private void TryInteract()
        {
            var hits = Physics.OverlapSphere(transform.position, interactionRange);
            foreach (var hit in hits)
            {
                var interaction = hit.GetComponent<NPCInteraction>();
                if (interaction != null)
                {
                    interaction.Interact();
                    return;
                }
            }
        }
    }
}
