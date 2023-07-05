using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] CharacterController characterController;

    [Header("Characteristics")]
    [SerializeField] float speed = 5f;
    [SerializeField] int gravity = -20;
    [SerializeField] int slideTime = 1000;

    [Header("Score")]
    [SerializeField] float scorePoint = 0.1f;
                     int score;

    //Название дороги, по которой бежит игрок. 
    //Нужно чтоб игрок не вышел за граници имеющихся дорог
    string roadName;

    //Кодировка дейсвтйи игрока, где свайп влево - 1, вправо - 2, и тд
    int inputCode;

    //Указывает на то, находиться ли игрок в состоянии скольжения вприсяди 
    bool isSliding;

    //Ускорение по навправлению
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        roadName = "";

        inputCode = 0;

        dir = new Vector3(0, 0, speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Если ускорение по y > 0, что обозначает, что мы хотим совершить прыжок
        //Или мы не находимся на замле
        //Игрока будет тянуть к земле.
        if (dir.y > 0 || !characterController.isGrounded) dir.y += gravity * Time.fixedDeltaTime;
        else{ //Если же игрок на земле, и не прыгает - ускорение по y = 0.
            dir.y = 0;
        }

        //Перемещение игрока.
        //Метод Move не перемещает игрока на указаную позицию, а даёт ускорение в указаную сторону.
        characterController.Move(Time.fixedDeltaTime * dir);
        score += Convert.ToInt32(speed * scorePoint); //Подсчитываем очки игрока

        //Если хотим переместиться влево
        if (inputCode == 1)
        {
            //Перемещаем игрока влево
            Vector3 newPos = transform.position;
            newPos.x -= 2;

            transform.position = newPos;
        }

        //Если вправо
        if(inputCode == 2)
        { //Аналогично
            Vector3 newPos = transform.position;
            newPos.x += 2;

            transform.position = newPos;
        }

        //Если свайп вверх
        if(inputCode == 3)
        {
            dir.y = 10; //Придаём ускорение по y на 10
            transform.localScale = new Vector3(1, 1f, 1); //Если игрок был в состоянии присяди - подымаем его
            isSliding = false;
        }

        //Возвращаем в начальное состояние
        inputCode = 0;

        //Если игрок на замле
        if (characterController.isGrounded)
        {
            gravity = -20; //Устанавливаем базовую гравитацию
        }

        //Если игрок не в состоянии скольжения - выравниваем его к нормальным размерам
        if (!isSliding) transform.localScale = new Vector3(1, 1, 1);
    }

    //Отвечает по большей части за отлавливание слайдов, так как обновляется чаще, чем fixedUpdate
    private void Update()
    {
        //Forward Down Debug
        //Откладка. Выводит красную полоску перед игроком.
        //Указывает в какой момент отлавливается прямое столкновение игрока с объектом

        //Вносим координаты начала линии
        Vector3 start = transform.position;
        start.y += 2.2f;
        start.z += 0.61f;

        RaycastHit[] rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down));

        //Если во что-то попали
        if (rhit.Length != 0)
        {
            //Отрисовываем луч к последнему попавшемо объекту
            Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);
        }


        //Луч для получения дороги, по которой бежит игрок
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.down));
        GameObject road = null;

        foreach (RaycastHit hit in hits)
        {
            //Если попали по дороге
            if (hit.collider.tag == "RoadComponent")
            {
                //Устанавливаем объект road, и поле названия дороги
                road = hit.collider.gameObject;
                roadName = road.transform.parent.name;
                break; //Выходим из цикла
            }
        }

        //На случай если вышли с дорог - метод дальше не выполняется 
        if (!road) return;

        //Свайп влево
        if (SwipeController.swipeLeft && transform.position.x > -2)
        {
            //Проверяем на возможность передвинуть игрока влево.
            //Нету ли слева посторонних объектов, которые мешают передвижению игрока, и тп
            if(CheckHorizontalMove(ref hits, true)) inputCode = 1;
        }

        //Вправо
        if (SwipeController.swipeRight && transform.position.x < 2)
        {
            if (CheckHorizontalMove(ref hits, false)) inputCode = 2;
        }

        //Вверх
        if (SwipeController.swipeUp && (bool)(dir.y == 0 || characterController.isGrounded))
        {
            inputCode = 3;
        }

        //Вниз
        if (SwipeController.swipeDown && !isSliding)
        {
            gravity = -80; //Притягиваем игрока к земле
            transform.localScale = new Vector3(1, 0.5f, 1); //Уменьшаем цилиндр 

            //Запускаем другой поток.
            Task.Run(() =>
            {
                isSliding = true;

                //Время, которое в slideTime, игрок будет в состоянии скольжения
                Thread.Sleep(slideTime);

                isSliding = false;
            });
        }

    }

    //Проверка на столкновения игрока с объектами
    private void OnCollisionEnter(Collision collision)
    {
        //Если столкнулись с препятствиями 
        if (collision.gameObject.tag == "Obstruction")
        {
            Vector3 start;

            //Если игрок в скольжении
            if (isSliding)
            {
                RaycastHit hit;

                start = transform.position;
                start.y += 0.5f;

                //Если игрок врезаля в препятствие
                if (Physics.Raycast(start, transform.TransformDirection(Vector3.right), out hit, 1f) && hit.collider.tag == "Obstruction")
                {
                    //Останавливаем игру
                    Debug.DrawRay(start, transform.TransformDirection(Vector3.right * hit.distance), Color.red);
                    Debug.Log("Slide hit!");
                    Time.timeScale = 0;
                }

                //Выходим из метода
                return;
            }

            //Проводим луч перед игроком
            start = transform.position;
            start.y += 2.2f;
            start.z += 0.61f;

            //Сверху - вниз
            RaycastHit[] rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down), 2.2f);
            Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);

            //Если перед игроком препятствие - значит он врезался в препятствие
            if (rhit.Length != 0 && rhit.LastOrDefault(obj => obj.collider.tag == "Obstruction").collider != null)
            { //Останавливаем игру
                Debug.Log("Hit!");
                Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);
            }
        }
    }

    //Getters & Setters

    public string GetRoadName() { return roadName; }

    public int GetScore() { return score; }

    //Functions

    private bool CheckHorizontalMove(ref RaycastHit[] hits, bool side)
    {
        Vector3 bodyPos = transform.position;
        bodyPos.y += 3f;

        if (side) bodyPos.x -= 2;
        else bodyPos.x += 2;

        //Проводим сверху - вниз с левой или правой стороны игрока (в зависимости от свайпа)
        hits = Physics.RaycastAll(bodyPos, transform.TransformDirection(Vector3.down), 3f);

        if (hits.Length != 0) Debug.DrawRay(bodyPos, transform.TransformDirection(Vector3.down * hits.Last().distance), Color.red);

        //Если слева от игрока только дорога, либо вообще ничего (луч не попал даже в дорогу, из-за ограничений по длине)
        //Значит переместиться в указуню сторону можно
        if ((bool)(hits.Length == 1 && hits[0].collider.tag == "RoadComponent") || hits.Length == 0 ) return true;

        return false;
    }
}
