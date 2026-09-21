using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject panel, finishedText, unfinishedText;
    [SerializeField] private AudioClip finishedSoundEffect;
    [SerializeField] private AudioClip unfinishedSoundEffect;
    [SerializeField] private int levelIndex;
    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerQuest>().GetOrbs() >= other.GetComponent<PlayerQuest>().GetorbsToCollect())
            {
                panel.SetActive(true);
                finishedText.SetActive(true);
                anim.SetTrigger("Finished");    //Finished representerar Flag från videoserien
                audioSource.PlayOneShot(finishedSoundEffect);
                
                Invoke(nameof(LoadNextLevel), 3.5f);
                //3.5 sekunder innan nästa level laddas in
            }
            else
            {
                panel.SetActive(true);
                unfinishedText.SetActive(true);
                audioSource.PlayOneShot(unfinishedSoundEffect);
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelIndex);
        //NEW LEVEL
    }
    private void OnTriggerExit2D (Collider2D other)
    {
        panel.SetActive(false);
        finishedText.SetActive(false);
        unfinishedText.SetActive(false);
    }
}
