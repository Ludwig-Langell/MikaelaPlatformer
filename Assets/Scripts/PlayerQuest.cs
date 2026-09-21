using UnityEngine;
using TMPro;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] private int orbsToCollect = 15;
    [SerializeField] private TMP_Text orbText;
    [SerializeField] private AudioClip pickupSoundEffect;
    private int orbs = 0;
    private AudioSource audioSource;
    private void Start()
    {
        orbText.text = "" + orbs;
        //Mellan citationstecken ovan kan text läggas till, men ikon finns redan för antal samlade Orbs.
        audioSource = GetComponent<AudioSource>();
    }

    public void AddOrb()
    {
        orbs++;
        orbText.text = "" + orbs;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(pickupSoundEffect);
    }

    public int GetOrbs() { return orbs;}
    public int GetorbsToCollect() { return orbsToCollect; }
}
//Mikaela merge conflict test med mig själv nummer 2