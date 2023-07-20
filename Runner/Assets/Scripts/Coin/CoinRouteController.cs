using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRouteController : MonoBehaviour
{
    /*вместо того чтоб делать префабы с путями и спавнить их, 
     просто сделать в редакторе дороги путь и добавить в скрипт, оно просто убирает другие руты*/
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
