using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Метод для загрузки основного игрового уровня
    /// </summary>
    public void LoadLevel() 
    {
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// Метод для выхода из игры
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// Метод для загрузки меню
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Метод для прродолжения игры из меню-паузы
    /// </summary>
    public void ContinueGame()
    {
        Time.timeScale = 1f;                    //возвращаем обратно timeScale, чтобы время шло
        gameObject.SetActive(false);            //отключаем объект PauseMenu
        PlayerSystem.instance.SetPause();       //устанавливаем флаг на false
    }
    /// <summary>
    /// Метод для загрузки меню из меню-паузы
    /// </summary>
    public void FromdPauseToMenu()  
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
