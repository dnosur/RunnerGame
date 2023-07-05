using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [Header("Roads")]

    [SerializeField] List<GameObject> RoadPrefabs; //Все дороги
                     List<GameObject> RoadMap; //Текущие дороги на карте

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        RoadMap = new List<GameObject>();

        //Генерируем первые две дороги
        StartCoroutine(SpawnCorutine(RoadPrefabs[GetObjectToSpawnId()]));
        StartCoroutine(SpawnCorutine(RoadPrefabs[GetObjectToSpawnId()]));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Если есть заспавненые дороги
        //Если игрок бежит по какой-то дроге
        //И первая заспавненая дорога - это не та, по которой бежит игрок
        if(RoadMap.Count != 0 &&
           playerController.GetRoadName().Length > 0 &&
           RoadMap[0].name != playerController.GetRoadName()) {
            Destroy(RoadMap[0]); //Удаляем с карты мира первую дорогу
            RoadMap.RemoveAt(0); //Удаляем её из массива дорог
            StartCoroutine(SpawnCorutine(RoadPrefabs[GetObjectToSpawnId()])); //Спавним новую дорогу
        }
    }

    //Генератор уникального id дороги, которая не заспавнена на карте
    int GetObjectToSpawnId()
    {
        if (RoadPrefabs.Count == 0) return -1;

        while (true)
        {
            int roadId = random.Next(0, RoadPrefabs.Count);

            if(RoadMap.Count > 0 && RoadMap.LastOrDefault(value => value.name == RoadPrefabs[roadId].name + "(Clone)") != null ) continue;
            return roadId;
        }
    }

    //Спавн дороги
    IEnumerator SpawnCorutine(GameObject road)
    {
        //Позиция дороги
        //Если у нас ещё не заспавнена ни одна дорога - её позиция будет 100, 0, 0
        //Иначе - берём последнюю заспавненную дорогу, и добавляем к значению x 100
        //Важно, что позиция дороги у нас локальная, а не глобальная!!!
        Vector3 pos = new Vector3(((RoadMap.Count > 0) ? RoadMap[RoadMap.Count - 1].transform.localPosition.x + 100 : 100), 0, 0);

        //Спавним дорогу, и рамещаем в нужную позицию
        GameObject spawnRoad = Instantiate(road, transform);
        spawnRoad.transform.localPosition = pos;
        Debug.Log(spawnRoad.transform.position);

        RoadMap.Add(spawnRoad);

        yield return null;
    }
}
