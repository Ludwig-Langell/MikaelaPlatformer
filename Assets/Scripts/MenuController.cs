using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject controlsPanel;


    public void StartGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

        public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }
}
