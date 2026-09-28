using UnityEngine;

public class BoxAbilities : MonoBehaviour
{
    [SerializeField] private EnemyChaser enemyToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!other.CompareTag("EnemyZone"))
            return;

        if (enemyToActivate != null)
        {
            enemyToActivate.Activate();
        }//aktiverar fienden när spelaren är i zonen
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (!other.CompareTag("EnemyZone"))
            return;

        if (enemyToActivate != null)
        {
            enemyToActivate.Deactivate();
        }//avaktiverar fienden när spelaren lämnar zonen
    }
}