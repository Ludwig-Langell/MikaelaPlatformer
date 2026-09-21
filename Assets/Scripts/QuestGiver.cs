using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject textPopup;
    [SerializeField] private AudioClip questgiverSoundEffect;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textPopup.SetActive(true);
            audioSource.PlayOneShot(questgiverSoundEffect);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textPopup.SetActive(false);
        }
    }
}
