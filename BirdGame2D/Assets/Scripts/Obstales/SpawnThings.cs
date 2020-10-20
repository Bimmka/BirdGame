#pragma warning disable 0649
using System.Collections;
using UnityEngine;

public class SpawnThings : MonoBehaviour
{
    
    [SerializeField] private Transform spawn;              //трансформ точки спавна
    [SerializeField] private Transform playerSpawn;         //трансформ точки респа персонажа
    [SerializeField] private Transform[] gemSpawns;         //трансформы точек спавна печенек
    [SerializeField] private Transform[] goldSpawns;        //трансформы точек спавна зерен

    public float dX;                            //отклонение по х при спавне
    public float dY;                            //отклонение по у при спавне
    public float goldSpawnTime;                 //время, через которое спавнится следующее зерно
    public float gemSpawnTime;                  //время, через которое спавнится следующая печенька
    public float energySpawnTime;               //время, через которое спавнится следующий энергетик
    public float enemySpawnTime;                //время, через которое заспавнится ии
    public float spawnTimer;                    //время, через которое уменьшатся значения goldSpawnTime, gemSPawnTime
    public float spawnTimerDown;                //время, на которое уменьшится goldSpawnTime и gemSpawnTime


    private GameObject gold;                    //зерно
    private GameObject gem;                     //печенька
    private GameObject exnetsionWall;           //внешние стены
    private GameObject labyrint;                //лабиринт сам
    private GameObject player;                  //игрок
    private GameObject energy;                  //энергетик
    private GameObject enemy;


    /// <summary>
    /// В этом методе загружаем префабы и спавним их
    /// </summary>
    private void Awake()
    {
        exnetsionWall = (GameObject)Resources.Load("Walls/ExtensionWalls", typeof(GameObject));
        labyrint = (GameObject)Resources.Load("Walls/Labyrint", typeof(GameObject));
        player = (GameObject)Resources.Load("Player/Player", typeof(GameObject));
        gold = (GameObject)Resources.Load("Present/Gold", typeof(GameObject));
        gem = (GameObject)Resources.Load("Present/Gem", typeof(GameObject));
        energy = (GameObject)Resources.Load("Present/Energy", typeof(GameObject));
        enemy = (GameObject)Resources.Load("Enemy/Enemy", typeof(GameObject));
        Instantiate(exnetsionWall, spawn.transform.position, Quaternion.identity, spawn);
        Instantiate(labyrint, spawn.transform.position, Quaternion.identity, spawn);
        Instantiate(player, playerSpawn.transform.position, Quaternion.identity, playerSpawn);
    }
    private void Start()
    {
        StartCoroutine(CreateGold());
        StartCoroutine(CreateGem());
        StartCoroutine(TimeSpawnDown());
        StartCoroutine(CheckPlayer());
        StartCoroutine(CreateEnergy());
        StartCoroutine(CreateEnemy());
    }
    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(enemySpawnTime);
        Instantiate(enemy, playerSpawn.position, Quaternion.identity, playerSpawn);
        PlayerMove.instance.SetKillEnemy();
    }
    /// <summary>
    /// Корутин для спавна зерен
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateGold()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldSpawnTime);
            Instantiate(gold, goldSpawns[Random.Range(0, goldSpawns.Length)].position + new Vector3(Random.Range(-dX,dX),Random.Range(-dY,dY), 20), Quaternion.identity);
            // создаем зернышко в рандомной точке из массива и отклоняем на вектор
        }
    }
    /// <summary>
    /// Корутин для уменьшения времени спавна объектов
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeSpawnDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            goldSpawnTime -= spawnTimerDown;
            gemSpawnTime -= spawnTimerDown / 2;
        }       
    }
    /// <summary>
    /// Корутин для спавна печеньки
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateGem()
    {
        while (true)
        {
            yield return new WaitForSeconds(gemSpawnTime);
            Instantiate(gem, gemSpawns[Random.Range(0,gemSpawns.Length)].position + new Vector3(Random.Range(-dX/4, dX/4), Random.Range(-dY/4,dY/4), 20), Quaternion.identity);
        }
    }
    IEnumerator CreateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(energySpawnTime);
            Instantiate(energy, gemSpawns[Random.Range(0, gemSpawns.Length)].position + new Vector3(Random.Range(-dX / 4, dX / 4), Random.Range(-dY / 4, dY / 4), 20), Quaternion.identity);
        }
    }
    /// <summary>
    /// Корутин для проверки того, что персонаж жив
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (PlayerMove.instance.IsDead()) StopAllCoroutines();
            if (PlayerMove.instance.IsKill()) StartCoroutine(CreateEnemy());
        }
    }
}
