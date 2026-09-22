using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int damageGiven = 1;

    //Knockback
    [SerializeField] private float knockbackForce = 100f;
    [SerializeField] private float upwardsForce = 5f;
    [SerializeField] private GameObject enemyParticleSystem;
    [SerializeField] private Transform spiderTarget;
    [SerializeField] private Transform spiderHome;
    
    private bool PlayerDetected;

    private void OnCollisionEnter2D(Collision2D other)
    {
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
    private void FixedUpdate()
    {
        if (PlayerDetected==true)
        {
            transform.position = Vector2.MoveTowards(transform.position, spiderTarget.position, moveSpeed * Time.deltaTime);
            if (transform.position == spiderTarget.position)
            {
                Invoke(nameof(CrawlUp), 1.5f);
            }
        }
        else if (PlayerDetected==false && transform.position != spiderHome.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, spiderHome.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDetected=true;
        }

    }
    private void CrawlUp()
    {
        PlayerDetected=false;
    }
}