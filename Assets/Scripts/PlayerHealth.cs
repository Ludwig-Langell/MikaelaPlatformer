using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int startingHealth = 5;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalHealthColor, criticalHealthColor;
    [SerializeField] private AudioClip damageSoundEffect;
    [SerializeField] private AudioClip healSoundEffect;
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
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
