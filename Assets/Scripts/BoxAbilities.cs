using UnityEngine;
using System.Collections; //behövs för coroutines

[System.Serializable]
public class ColorMapping
{
    public string tag;
    public Color color;
    public float gravityScale = 1f;
    public bool flipGravityVisual = false;
}
//För att sätta tag och color i samma "box"
//System.Serializeable istället för SerializeField är pga att Unity endast kan visa och låta oss redigera built-ins som int, color, gameobject etc.
public class BoxAbilities : MonoBehaviour
{
    [SerializeField] private SpriteRenderer objectRenderer;
    [SerializeField] private ColorMapping[] colorMappings;
    [SerializeField] private Rigidbody2D rb; //För att kunna dra rigidbodyn på boxabilities scriptet i inspectorn, eftersom jag ej använder getcomponent för den
    [SerializeField] private float exitGraceTime = 0.15f; //Tid utanför zonen innan effekten faktiskt återställs, för att undvika flimmer vid kanten

    private Color originalColor;
    private float originalGravity;
    private Coroutine revertCoroutine;
    //Originalfärg och gravity för spriten
  

    void Start()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<SpriteRenderer>();
        }

        originalColor = objectRenderer.color;
        originalGravity = rb.gravityScale;
        //Originalfärg och gravity för spriten
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Om en revert är på gång, avbryt den eftersom spelaren fortfarande är kvar i en zon
        if (revertCoroutine != null)
        {
            StopCoroutine(revertCoroutine);
            revertCoroutine = null;
        }

        foreach (ColorMapping mapping in colorMappings)
        {
            if (other.CompareTag(mapping.tag))
            {
                objectRenderer.color = mapping.color;
                rb.gravityScale = mapping.gravityScale;                                             //Ändrar gravity

                Vector3 scale = transform.localScale;
                scale.y = mapping.flipGravityVisual ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);     //Ändrar spritens flip (beroende på om det är reverse gravity)
                transform.localScale = scale;

                break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Istället för att återställa direkt, vänta exitGraceTime sekunder - avbryts i OnTriggerEnter2D om spelaren kommer in igen i tid
        if (revertCoroutine != null)
        {
            StopCoroutine(revertCoroutine);
        }
        revertCoroutine = StartCoroutine(RevertAfterDelay());
    }
 
    private IEnumerator RevertAfterDelay()
    {
        yield return new WaitForSeconds(exitGraceTime);
        objectRenderer.color = originalColor;
        rb.gravityScale = originalGravity;

        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
        //spriten ska gå tillbaka till normalläge efter en viss tid när man lämnar boxen (undviker att spriten flippar ur)

        revertCoroutine = null;
    }

}