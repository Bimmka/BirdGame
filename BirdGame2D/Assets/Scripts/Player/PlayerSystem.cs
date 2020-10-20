#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Player;

/// <summary>
/// Класс, отвечающий за взаимодействие персонажа с окружением, сохранение данных, подсчет очков
/// </summary>
public class PlayerSystem : MonoBehaviour
{
    [SerializeField] private Text goldText;                         //текст, отображающий количество подобранных зерен
    [SerializeField] private Text gemText;                          //текст, отображающий количество подобранных печеньев
    [SerializeField] private GameObject deathMenu;                  //меню, вызываемое при смерти персонажа
    [SerializeField] private GameObject pauseMenu;                  //меню, вызываемое при нажатии на Escape
    [SerializeField] private Camera mainCamera;         
    [SerializeField] private AudioSource fail;
    [SerializeField] private float shakeDuration;                   //время тряски камеры
    [SerializeField] private Text bestScoreText;                    //текст, отображающий лучший счет
    [SerializeField] private Text scoreText;                        //текст, отображающий ваш счет
    [SerializeField] private Text birdText;

    private int gold = 0;                                           //количество зерен подобранных
    private int gem = 0;                                            //количество печенек подобранных
    private int bird = 0;
    private int score;                                              //счет игрока
    private bool pause = false;                                     //флаг при паузе игры
    private bool shaked = false;                                    //флаг при тряски камеры
    private bool calculated = false;                                //флаг при подсчете очков 
    private string key = "Game";                                    //ключ для сохранения данных
    private SaveData data = new SaveData();                         //класс, содержащий сохраняемые данные

    public static PlayerSystem instance;
    private void Awake()
    {
        LoadJson();                                                 //загружаем json файл, чтобы отобразить лучший результат
        if (PlayerSystem.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerSystem.instance = this;
    }
    private void OnDestroy()
    {
        PlayerSystem.instance = null;
    }
    private void Update()
    {
        birdText.text = bird.ToString();
        goldText.text = gold.ToString();
        gemText.text = gem.ToString();
        if (PlayerMove.instance.IsDead())  PlayerDead();
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerMove.instance.IsDead()) GamePause();
    }
    /// <summary>
    /// Метод при паузе игры
    /// </summary>
    private void GamePause()
    {
        if (pause)                                  //если после нажатия на Escape уже было открыто меню-паузы, то возобновляем игру
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            pause = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            pause = true;
        }
    }
    /// <summary>
    /// Метод при сммерти персонажа
    /// </summary>
    public void PlayerDead()
    {
        if (!shaked)                                    //если камера не тряслась, то проигрываем
        {
            fail.Play();
            mainCamera.DOShakePosition(shakeDuration);
            shaked = true;
        }
        bestScoreText.text = data.bestScore.ToString();        
        deathMenu.SetActive(true);
        if (!calculated)
        {
            CalculateScore();
            scoreText.text = score.ToString();
        }

        }
    /// <summary>
    /// Метод для подсчета количества очков
    /// </summary>
    private void CalculateScore()
    {
        score = gold + 2 * gem + 3 * bird;                     //считаем заработанные очки
        if (score > data.bestScore) SaveJson();     //если заработанных очков больше, чем предыдущий рекорд, то это будет считаться лучшим счетом
        calculated = true;
    }
    /// <summary>
    /// Метод для работы с json
    /// </summary>
    private void LoadJson()
    {
        if (PlayerPrefs.HasKey(key))                //смотрим, что уже существует данный ключ
        {
            string value = PlayerPrefs.GetString(key);
            data = JsonUtility.FromJson<SaveData>(value);
        }
        else
        {
            data = new SaveData();
            data.bestScore = 0;
        }
    }
    /// <summary>
    /// Метод для сохранения в json
    /// </summary>
    private void SaveJson()
    {
        data.bestScore = score;
        string value = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Метод для увеличения количества зерен
    /// </summary>
    public void IncGold()
    {
        gold++;
    }
    /// <summary>
    /// Метод для увеличения количества печенек
    /// </summary>
    public void IncGem()
    {
        gem++;
    }
    public void IncBird()
    {
        bird++;
    }
    /// <summary>
    /// Метод для постановки флага паузы
    /// </summary>
    public void SetPause()
    {
        pause = false;
    }
}
