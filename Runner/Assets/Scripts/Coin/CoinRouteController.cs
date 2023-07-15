using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRouteController : MonoBehaviour
{
    /*������ ���� ���� ������ ������� � ������ � �������� ��, 
     ������ ������� � ��������� ������ ���� � �������� � ������, ��� ������ ������� ������ ����*/
    [SerializeField] GameObject[] Routes;
    void Start()
    {
        if (Routes.Length > 0)      
        {
            int len = Routes.Length;
            int route = Random.Range(0, len);
            for (int i = 0; i <len; i++)
            {
                if (i != route) Destroy(Routes[i]);
            }
        }
    }
}
