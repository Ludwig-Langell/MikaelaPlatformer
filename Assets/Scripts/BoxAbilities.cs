using UnityEngine;

public class BoxAbilities : MonoBehaviour
{
    [SerializeField] private EnemyChaser enemyToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the actual enemy zone
        if (!other.CompareTag("EnemyZone"))
            return;

        if (enemyToActivate != null)
        {
            enemyToActivate.Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Only react to the actual enemy zone
        if (!other.CompareTag("EnemyZone"))
            return;

        if (enemyToActivate != null)
        {
            enemyToActivate.Deactivate();
        }
    }
}