#pragma warning disable 0649
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem goldExplosion;
    [SerializeField] private ParticleSystem gemExplosion;
    [SerializeField] private ParticleSystem energyExplosion;
    [SerializeField] private AudioSource money;
    [SerializeField] private AudioSource energy;

    public float flyingForce;                                                           //подъемная сила

    private bool isDead = false;                                                        //флаг, что персонаж погиб
    private bool killEnemy;

    public static PlayerMove instance;
    private void Awake()
    {
        if (PlayerMove.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerMove.instance = this;
    }
    private void OnDestroy()
    {
        PlayerMove.instance = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) Moving();
        else PlayerDeath();
    }
    /// <summary>
    /// Метод для передвижения персонажа по нажатию кнопок WAD
    /// </summary>
    private void Moving()
    {
        if (Input.GetKey(KeyCode.W)) rbPlayer.AddForce(transform.up * flyingForce);
        if (Input.GetKey(KeyCode.D))
        {
            rbPlayer.AddForce((transform.right / 2 + transform.up / 2) * flyingForce);
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);                      //меняем localScale, чтобы персонаж как бы смотрел в сторону, в которую летит
        }
        if (Input.GetKey(KeyCode.A))
        {
            rbPlayer.AddForce((-transform.right / 2 + transform.up / 2) * flyingForce);
            transform.localScale = new Vector3(-1.1f, 1.1f, 1.1f);
        }
    }
    private void PlayerDeath()
    {
        animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacles")) isDead = true;
        if (collision.CompareTag("Gold"))
        {
            PlayerSystem.instance.IncGold();                         //увеличиваем количество подобранного золота
            Destroy(collision.gameObject);                          //уничтожаем подобранный объект
            goldExplosion.Play();                                   //проигрываем систему частиц
            rbPlayer.mass += 0.01f;                                 //увеличиваем массу персонажа
            money.Play();                                           //проигрываем звук подбора
        }
        if (collision.CompareTag("Gem"))
        {
            PlayerSystem.instance.IncGem();                         
            Destroy(collision.gameObject);
            gemExplosion.Play();
            rbPlayer.mass += 0.015f;
            money.Play();
        }
        if (collision.CompareTag("Energy"))
        {
            Destroy(collision.gameObject);
            energyExplosion.Play();
            rbPlayer.mass -= 0.0125f;
            energy.Play();
        }
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            killEnemy = true;
            PlayerSystem.instance.IncBird();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
    public bool IsKill()
    {
        return killEnemy;
    }
    public void SetKillEnemy()
    {
        killEnemy = false;
    }
}
