using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 1;

    //Knockback
    [SerializeField] private float knockbackForce = 100f;
    [SerializeField] private float upwardsForce = 5f;
    [SerializeField] private GameObject enemyParticleSystem;
    
    [SerializeField] private AudioClip deathSoundEffect;
    //private AudioSource audioSource; behövs ej pga annan lösning, se slutet av scriptet
    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        //audioSource = GetComponent<AudioSource>(); behövs ej pga annan lösning, se slutet av scriptet
    }

    private void Update()
    {
        if (moveSpeed < 0)
        {
            rend.flipX = true;
        }
        if (moveSpeed > 0)
        {
            rend.flipX = false;
        }
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock") || other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }

        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);

            if(other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(knockbackForce, upwardsForce);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(-knockbackForce, upwardsForce);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rgbd = other.attachedRigidbody;
            
            if (rgbd != null)
            {
                rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0);
                rgbd.AddForce(new Vector2(0, bounciness));
                
            }
            Instantiate(enemyParticleSystem, transform.position, Quaternion.identity);

            if (deathSoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(deathSoundEffect, transform.position, 1f);
                //Rad ovan är för att spela ljudeffekt trots att objektet blir destroyed. Rådfråga om annan lösning under handledning?
                // Eftersom AudioSource nu ej är tillagt kan inte ljudet kontrolleras och ljudnivån är väldigt låg.
            }
            Destroy(gameObject);
            //audioSource.PlayOneShot(deathSoundEffect); funkar ej pga är en component till objektet som förstörs     
        }
    }
}