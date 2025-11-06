using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject gameOverScreen;


   


    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }


    public void Menu()
    {
       SceneManager.LoadScene(0);
    }


}
