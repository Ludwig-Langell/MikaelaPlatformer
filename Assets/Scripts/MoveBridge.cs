using UnityEngine;

public class MoveBridge : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private AudioClip bridgeSoundEffect;
    private AudioSource audioSource;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            anim.SetTrigger("Move");
            lever.SetActive(false);
            audioSource.PlayOneShot(bridgeSoundEffect);
        }
    }

}
