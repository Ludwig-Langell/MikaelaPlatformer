using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private AudioClip trampolineSoundEffect;
    [SerializeField] private GameObject trampolineParticleSystem;
    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rgbd = other.gameObject.GetComponent<Rigidbody2D>();

            if (rgbd != null)
            {
                rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0);
                rgbd.AddForce(new Vector2(0, jumpForce));
                anim.SetTrigger("Activate");
                audioSource.PlayOneShot(trampolineSoundEffect);
                other.GetComponent<PlayerMovement>().CanDoubleJumpAgain();
                Instantiate(trampolineParticleSystem, transform.position, Quaternion.identity);
            }
        }
    }
}
