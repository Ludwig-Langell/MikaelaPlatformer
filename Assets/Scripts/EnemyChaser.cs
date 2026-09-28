using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 1;

    // Knockback
    [SerializeField] private float knockbackForce = 100f;
    [SerializeField] private float upwardsForce = 5f;
    [SerializeField] private GameObject enemyParticleSystem;

    [SerializeField] private AudioClip deathSoundEffect;

    private SpriteRenderer rend;
    private Transform player;

    private Vector3 startPosition;

    private bool isChasing = false;
    private bool isReturning = false;

    private const float arrivalThreshold = 0.05f;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        // för att komma ihåg vart fienden startar så den kan return dit sedan
        startPosition = transform.position;

        // hitta spelaren i annat objekt då det inte är public
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        // flippar spriten (ögat) när spelaren springer åt annat håll
        if (isChasing)
        {
            if (player.position.x < transform.position.x)
            {
                rend.flipX = true;
            }
            else
            {
                rend.flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isReturning)
        {
            ReturnToStart();
        }
    }

    private void ChasePlayer()
    {
        if (player == null)
        return;

        Vector2 nextPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.fixedDeltaTime);

        transform.position = nextPosition;
    }

    private void ReturnToStart()
    {
        Vector2 nextPosition = Vector2.MoveTowards(transform.position, startPosition, moveSpeed * Time.fixedDeltaTime);

        // gå tillbaka till startposition
        if (startPosition.x < transform.position.x)
        {
            rend.flipX = true;
        }
        else
        {
            rend.flipX = false;
        }

        transform.position = nextPosition;

        //fienden har kommit tillbaka till sin startposition nu
        if (Vector2.Distance(transform.position, startPosition) <= arrivalThreshold)
        {
            transform.position = startPosition;

            isReturning = false;
        }
    }

    // kallas på av boxabilities när spelaren går in i zonen
    public void Activate()
    {
        isChasing = true;
        isReturning = false;
    }

    // kallas på av boxabilities när spelaren lämnar zonen
    public void Deactivate()
    {
        isChasing = false;
        isReturning = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damageGiven);
            }

            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                if (other.transform.position.x > transform.position.x)
                {
                    movement.TakeKnockback(knockbackForce, upwardsForce);
                }
                else
                {
                    movement.TakeKnockback(-knockbackForce, upwardsForce);
                }
            }//spelaren är oneshot om ögat slår den, instant respawn, så om health är allt annat är null så tar den damage
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

            if (enemyParticleSystem != null)
            {
                Instantiate(enemyParticleSystem, transform.position, Quaternion.identity);
            }

            if (deathSoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(deathSoundEffect, transform.position, 1f);
            }

            Destroy(gameObject);
        }
    }
}
//experimenterat om man ska kunna döda den eller ej men kommit fram till att det ska vara one shot för att öka stressen när man blir jagad