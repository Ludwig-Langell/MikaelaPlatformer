using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroVideo : MonoBehaviour
{
    [SerializeField] private int levelIndex;
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
        

                Invoke(nameof(LoadNextLevel), 17.0f);
                //17 sekunder innan nästa level laddas in
        }
    }
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelIndex);
        //NEW LEVEL
    }

}