using UnityEngine;

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

    private Color originalColor;
    private float originalGravity;
  

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
        objectRenderer.color = originalColor;
        rb.gravityScale = originalGravity;

        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
        //Samma som ovanstående anteckningar men för att spriten ska gå tillbaka till normalläge när man lämnar boxen
    }

}