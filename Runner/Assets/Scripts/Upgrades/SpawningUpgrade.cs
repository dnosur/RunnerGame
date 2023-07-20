using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningUpgrade : MonoBehaviour
{

    [SerializeField] private GameObject[] upgrades; // ����� ����� ���������
    [SerializeField] private GameObject[] pointsToSpawn; //����� �����, ��� ���������� �������
    [SerializeField] private int chanceToSpawn; //��� ������ �����, ��� ������ ����� ���� ��� ������, ������ ���� ������ ������ 1 (���� 100%)
    
    private int upgradesToSpawn;

    private void Awake()
    {
        upgradesToSpawn = pointsToSpawn.Length;

        SpawnUpgrade();
    }


    private void SpawnUpgrade()
    {
        for (int i = 0; i < upgradesToSpawn; i++)
        {
            if(Random.Range(1, chanceToSpawn) == 1) { 
                GameObject temporary = Instantiate(upgrades[Random.Range(0,upgrades.Length)], transform);
                temporary.transform.position = pointsToSpawn[i].transform.position;         //������ ������ �� ����� �� ������
            }
        }
    }
}
