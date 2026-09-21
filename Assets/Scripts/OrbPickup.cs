using UnityEngine;

public class OrbPickup : MonoBehaviour
{
    [SerializeField] private GameObject orbParticleSystem;
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerQuest>().AddOrb();
            Instantiate(orbParticleSystem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
