using UnityEngine;

public class FrogAnimator : MonoBehaviour
{
    [SerializeField] private int jumpFrequency =750;
    private int jumpBuildup;
    private Animator anim;

    void Start()
    {
                anim =  GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        jumpBuildup += Random.Range(0, 3);
        if (jumpBuildup >= jumpFrequency)
        {
            anim.SetTrigger("FrogJump");
            jumpBuildup=0;
        }
    }
}
