using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [Header("Roads")]

    [SerializeField] List<GameObject> RoadPrefabs;
                     List<GameObject> RoadMap;

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        RoadMap = new List<GameObject>();
        StartCoroutine(SpawnCorutine(RoadPrefabs[GetObjectToSpawnId()]));
        StartCoroutine(SpawnCorutine(RoadPrefabs[GetObjectToSpawnId()]));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(RoadMap.Count != 0 &&
           playerController.GetRoadName().Length > 0 &&
           RoadMap[0].name != playerController.GetRoadName()) {
            Destroy(RoadMap[0]);
            RoadMap.RemoveAt(0);
            StartCoroutine(SpawnCorutine(RoadPrefabs[GetObjectToSpawnId()]));
        }
    }

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

    IEnumerator SpawnCorutine(GameObject road)
    {
        Vector3 pos = new Vector3(((RoadMap.Count > 0) ? RoadMap[RoadMap.Count - 1].transform.localPosition.x + 100 : 100), 0, 0);

        GameObject spawnRoad = Instantiate(road, transform);
        spawnRoad.transform.localPosition = pos;
        Debug.Log(spawnRoad.transform.position);

        RoadMap.Add(spawnRoad);

        yield return null;
    }

    void Spawn(GameObject road)
    {
        Vector3 pos = new Vector3(((RoadMap.Count > 0) ? RoadMap[RoadMap.Count - 1].transform.localPosition.x + 100 : 100), 0, 0);

        GameObject spawnRoad = Instantiate(road, transform);
        spawnRoad.transform.localPosition = pos;
        Debug.Log(spawnRoad.transform.position);

        RoadMap.Add(spawnRoad);
    }
}
