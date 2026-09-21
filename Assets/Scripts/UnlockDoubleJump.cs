using UnityEngine;

public class UnlockDoubleJump : MonoBehaviour
{
    [SerializeField] private GameObject doubleJumpPickupParticleSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().UnlockDoubleJump();
            Instantiate(doubleJumpPickupParticleSystem, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }

}
