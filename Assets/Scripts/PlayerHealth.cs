using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int startingHealth = 5;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalHealthColor, criticalHealthColor;
    [SerializeField] private AudioClip damageSoundEffect;
    [SerializeField] private AudioClip healSoundEffect;
    [SerializeField] private AudioClip respawnSoundEffect;
    private AudioSource audioSource;
    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthbar();
        audioSource.PlayOneShot(damageSoundEffect);


        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        currentHealth = startingHealth;
        UpdateHealthbar();
        transform.position = spawnPosition.position;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        audioSource.PlayOneShot(respawnSoundEffect);
    }

    private void UpdateHealthbar()
    {
        healthSlider.value = currentHealth;

        if(currentHealth <= 2)
        {
            fillImage.color = criticalHealthColor;
        }
        else
        {
            fillImage.color = normalHealthColor;
        }
    }     

    public bool RestoreHealth(int healthToRestore)
    {
        if (currentHealth >= startingHealth)
        {
            return false;
        }
        currentHealth += healthToRestore;
        UpdateHealthbar();
        audioSource.PlayOneShot(healSoundEffect);

        if(currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        return true;
    }
}
