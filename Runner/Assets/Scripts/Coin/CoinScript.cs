using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] public int rotationSpeed = 160;
    [SerializeField] public float verticalIncrease = 0.4f;
    private float normalY;
    private bool moveUp = false;
    private void Start()
    {
        normalY = transform.position.y;
    }
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed*Time.deltaTime, Space.World); //Вращение вокруг
        if (moveUp) //Анимация вверх-вниз
        {
            if (transform.position.y <= normalY + verticalIncrease) transform.Translate(Vector3.up * verticalIncrease * Time.deltaTime, Space.World);
            else moveUp = false;
        }
        else
        {
            if (transform.position.y >= normalY) transform.Translate(Vector3.up * -verticalIncrease * Time.deltaTime, Space.World);
            else moveUp = true;
        }
    }
}
