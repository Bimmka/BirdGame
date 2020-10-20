#pragma warning disable 0649
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    [SerializeField] private Transform finish;                  //трансформ на конечную точку
    [SerializeField] private TeleportSystem finishSystem;       //ссылка на скрипт у объекта


    private bool teleported = false;                            //флаг, показывающий, что игрок прошел через телепорт
    ///
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !teleported)
        {
            collision.transform.position = finish.position;     //если произошло столкновение с игроком, то у игрока меня позицию
            finishSystem.SetTeleported();                       //ставим у конечной точки флаг в true, чтобы сразу обратно не закинуло
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) teleported = false;//когда персонаж выходит из телепорта, меняем у телепорта, из которого вышел игрока, флаг
    }
    /// <summary>
    /// Метод для изменения значения флага
    /// </summary>
    public void SetTeleported()
    {
        teleported = true;
    }
}
