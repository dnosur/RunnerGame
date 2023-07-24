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

    [Header("Screen Controllers")]
    [SerializeField] DeathController deathController;

    [Header("Speed")]
    [SerializeField] float speed = 5f;
    [SerializeField] private float speedIncreaseAmount = 0.10f;
    [SerializeField] private float speedIncreaseInterval = 2f;
    [SerializeField] private float maxSpeed = 20f;

    [Header("Characteristics")]
    [SerializeField] int gravity = -20;
    [SerializeField] int slideTime = 1000;

                     int health = 100;

    [Header("Score")]
    [SerializeField] float scorePoint = 0.1f;
                     int score;
                     int coins = 0;

    //�������� ������, �� ������� ����� �����. 
    //����� ���� ����� �� ����� �� ������� ��������� �����
    string roadName;

    //��������� �������� ������, ��� ����� ����� - 1, ������ - 2, � ��
    int inputCode;

    //��������� �� ��, ���������� �� ����� � ��������� ���������� �������� 
    bool isSliding;

    //��������� �� ������������
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        roadName = "";

        inputCode = 0;
        StartCoroutine(IncreaseSpeedRoutine());
        dir = new Vector3(0, 0, speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //���� ��������� �� y > 0, ��� ����������, ��� �� ����� ��������� ������
        //��� �� �� ��������� �� �����
        //������ ����� ������ � �����.
        if (dir.y > 0 || !characterController.isGrounded) dir.y += gravity * Time.fixedDeltaTime;
        else{ //���� �� ����� �� �����, � �� ������� - ��������� �� y = 0.
            dir.y = 0;
        }

        //����������� ������.
        //����� Move �� ���������� ������ �� �������� �������, � ��� ��������� � �������� �������.
        characterController.Move(Time.fixedDeltaTime * dir);
        score += Convert.ToInt32(speed * scorePoint); //������������ ���� ������

        //���� ����� ������������� �����
        if (inputCode == 1)
        {
            //���������� ������ �����
            Vector3 newPos = transform.position;
            newPos.x -= 2;

            transform.position = newPos;
        }

        //���� ������
        if(inputCode == 2)
        { //����������
            Vector3 newPos = transform.position;
            newPos.x += 2;

            transform.position = newPos;
        }

        //���� ����� �����
        if(inputCode == 3)
        {
            dir.y = 10; //������ ��������� �� y �� 10
            transform.localScale = new Vector3(1, 1f, 1); //���� ����� ��� � ��������� ������� - �������� ���
            isSliding = false;
        }

        //���������� � ��������� ���������
        inputCode = 0;

        //���� ����� �� �����
        if (characterController.isGrounded)
        {
            gravity = -20; //������������� ������� ����������
        }

        //���� ����� �� � ��������� ���������� - ����������� ��� � ���������� ��������
        if (!isSliding) transform.localScale = new Vector3(1, 1, 1);
    }

    //�������� �� ������� ����� �� ������������ �������, ��� ��� ����������� ����, ��� fixedUpdate
    private void Update()
    {
        //Forward Down Debug
        //��������. ������� ������� ������� ����� �������.
        //��������� � ����� ������ ������������� ������ ������������ ������ � ��������

        //������ ���������� ������ �����
        Vector3 start = transform.position;
        start.y += 2.2f;
        start.z += 0.61f;

        RaycastHit[] rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down), 2.2f);

        //���� �� ���-�� ������
        if (rhit.Length != 0)
        {
            //������������ ��� � ���������� ��������� �������
            Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.yellow);
        }


        //��� ��� ��������� ������, �� ������� ����� �����
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.down));
        GameObject road = null;

        foreach (RaycastHit hit in hits)
        {
            //���� ������ �� ������
            if (hit.collider.tag == "RoadComponent")
            {
                //������������� ������ road, � ���� �������� ������
                road = hit.collider.gameObject;
                roadName = road.transform.parent.name;
                break; //������� �� �����
            }
        }

        //�� ������ ���� ����� � ����� - ����� ������ �� ����������� 
        if (!road) return;

        //����� �����
        if (SwipeController.swipeLeft && transform.position.x > -2)
        {
            //��������� �� ����������� ����������� ������ �����.
            //���� �� ����� ����������� ��������, ������� ������ ������������ ������, � ��
            if (CheckHorizontalMove(ref hits, true)) inputCode = 1;
            else SideKick();
        }

        //������
        if (SwipeController.swipeRight && transform.position.x < 2)
        {
            if (CheckHorizontalMove(ref hits, false)) inputCode = 2;
            else SideKick();
        }

        //�����
        if (SwipeController.swipeUp && (bool)(dir.y == 0 || characterController.isGrounded))
        {
            inputCode = 3;
        }

        //����
        if (SwipeController.swipeDown && !isSliding)
        {
            gravity = -80; //����������� ������ � �����
            transform.localScale = new Vector3(1, 0.5f, 1); //��������� ������� 

            //��������� ������ �����.
            Task.Run(() =>
            {
                isSliding = true;

                //�����, ������� � slideTime, ����� ����� � ��������� ����������
                Thread.Sleep(slideTime);

                isSliding = false;
            });
        }

    }

    //�������� �� ������������ ������ � ���������
    private void OnCollisionEnter(Collision collision)
    {
        //���� ����������� � ������������� 
        if (collision.gameObject.tag == "Obstruction")
        {
            Vector3 start;

            //���� ����� � ����������
            if (isSliding)
            {
                RaycastHit hit;

                start = transform.position;
                start.y += 0.5f;

                //���� ����� ������� � �����������
                if (Physics.Raycast(start, transform.TransformDirection(Vector3.right), out hit, 1f) && hit.collider.tag == "Obstruction")
                {
                    //������������� ����
                    Debug.DrawRay(start, transform.TransformDirection(Vector3.right * hit.distance), Color.red);
                    Debug.Log("Slide hit!");
                    Die();
                }

                //������� �� ������
                return;
            }

            //�������� ��� ����� �������
            start = transform.position;
            start.y += 2.2f;
            start.z += 0.61f;

            //������ - ����
            RaycastHit[] rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down), 2.2f);
            
            if(rhit.Length > 0) Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);

            //���� ����� ������� ����������� - ������ �� �������� � �����������
            if (rhit.Length != 0 && rhit.LastOrDefault(obj => obj.collider.tag == "Obstruction").collider != null)
            { //������������� ����
                if (rhit[0].distance >= 1.7f)
                {
                    LegKick();
                    Debug.Log("Kick! " + rhit[0].distance);
                }
                else
                {
                    Debug.Log("Hit! " + rhit[0].distance);
                }

                Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            coins += 1;
            Destroy(other.gameObject);
        }
    }

    //Getters & Setters

    public string GetRoadName() { return roadName; }

    public int GetScore() { return score; }

    public int GetCoins() { return coins; }

    //Functions

    private bool CheckHorizontalMove(ref RaycastHit[] hits, bool side)
    {
        Vector3 bodyPos = transform.position;
        bodyPos.y += 3f;

        if (side) bodyPos.x -= 2;
        else bodyPos.x += 2;

        //�������� ������ - ���� � ����� ��� ������ ������� ������ (� ����������� �� ������)
        hits = Physics.RaycastAll(bodyPos, transform.TransformDirection(Vector3.down), 3f);

        if (hits.Length != 0) Debug.DrawRay(bodyPos, transform.TransformDirection(Vector3.down * hits.Last().distance), Color.red);

        //���� ����� �� ������ ������ ������, ���� ������ ������ (��� �� ����� ���� � ������, ��-�� ����������� �� �����)
        //������ ������������� � ������� ������� �����
        if (hits.Length == 0) return true;
        if (hits.Length >= 1 && hits[0].collider.tag == "Obstruction" && hits[0].distance >= 1.7f) return true;
        if (hits.Length == 1 && hits[0].collider.tag == "RoadComponent") return true;

        return false;
    }

    private void Die()
    {
        Time.timeScale = 0;
        deathController.ShowDeathScreen();
    }

    private void SideKick()
    {
        if (health == 100)
        {
            StartCoroutine(Freeze(1));
        }
        else { Die(); }
    }

    private void LegKick()
    {
        dir.y = 5;

        if (health == 100) { StartCoroutine(Freeze(speed / 2)); }
        else Die();
    }

    //Coroutines

    private IEnumerator IncreaseSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);

            if (speed < maxSpeed)
            {
                speed += speedIncreaseAmount;
                dir.z = speed;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {
                break;
            }
        }
    }

    private IEnumerator Freeze(float speed)
    {
        float speedTemp = this.speed;

        this.speed = speed;
        dir.z = speed;

        health = 50;

        yield return new WaitForSeconds(2);

        health = 100;
        this.speed = speedTemp;
        dir.z = this.speed;
    }
}
