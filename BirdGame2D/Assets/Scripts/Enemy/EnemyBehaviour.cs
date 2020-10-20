#pragma warning disable 0649
using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private AIDestinationSetter setter;     //ссылка на скрипт для поиска пути к цели
    [SerializeField] private AIPath aiPath;                  //ссылка на скрипт для поиска пути

    public Vector2 box;                                      //для натройки OverlapBoxAll
    public LayerMask layer;                                  //слой с полезностями для игрока
                         
    private void Start()
    {
        SetTarget();
    }
    private void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f) transform.localScale = new Vector3(1f, 1f, 1f);           //чтобы враг смотрел в ту сторону, в которую летит
        else if (aiPath.desiredVelocity.x <= -0.01f) transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    IEnumerator SearchThing()
    {
        yield return new WaitForSeconds(5f);
        SetTarget();  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem") || collision.CompareTag("Gold") || collision.CompareTag("Energy"))
        {
            Destroy(collision.gameObject);
            SetTarget();
        }
    }
    /// <summary>
    /// Метод для постановки таргета врагу
    /// </summary>
    private void SetTarget()
    {
        if (!PlayerMove.instance.IsDead())                                  
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, box, 360, layer);      //получаем все коллайдеры с определенным слоем
            if (colliders.Length != 0) setter.target = colliders[0].transform;                          //если есть хотя бы один, то ставим его как цель
            else StartCoroutine(SearchThing());                                                         //иначе ждем 5с и повторяем
        }
        
    }

}
