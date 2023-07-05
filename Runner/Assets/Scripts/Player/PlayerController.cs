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

    [Header("Body")]
    [SerializeField] GameObject head;

    [Header("Characteristics")]
    [SerializeField] float speed = 5f;
    [SerializeField] int gravity = -20;
    [SerializeField] int slideTime = 1000;

    [Header("Score")]
    [SerializeField] float scorePoint = 0.1f;
                     int score;

    string roadName;

    int inputCode;

    bool isSliding;

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

        if (dir.y > 0 || !characterController.isGrounded) dir.y += gravity * Time.fixedDeltaTime;
        else dir.y = 0;

        characterController.Move(Time.fixedDeltaTime * dir);
        score += Convert.ToInt32(speed * scorePoint);

        if (inputCode == 1)
        {
            Vector3 newPos = transform.position;
            newPos.x -= 2;

            transform.position = newPos;
        }

        if(inputCode == 2)
        {
            Vector3 newPos = transform.position;
            newPos.x += 2;

            transform.position = newPos;
        }

        if(inputCode == 3)
        {
            dir.y = 10;
            transform.localScale = new Vector3(1, 1f, 1);
        }

        inputCode = 0;

        if (characterController.isGrounded)
        {
            gravity = -20;
        }
        if (!isSliding) transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        //Forward Down Debug
        Vector3 start = transform.position;
        start.y += 2.2f;
        start.z += 0.61f;

        RaycastHit[] rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down));

        if (rhit.Length != 0)
        {
            Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);
        }

        //Left Debug
        start = transform.position;
        start.y += 3f;
        start.x -= 2f;

        rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down), 3f);

        if (rhit.Length != 0)
        {
            Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);
        }

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.down));
        GameObject road = null;

        foreach (RaycastHit hit in hits )
        {
            if (hit.collider.tag == "RoadComponent")
            {
                road = hit.collider.gameObject;
                roadName = road.transform.parent.name;
                break;
            }
        }

        if (!road) return;

        if (SwipeController.swipeLeft && transform.position.x > -2)
        {
            if(CheckHorizontalMove(ref hits, true)) inputCode = 1;
        }

        if (SwipeController.swipeRight && transform.position.x < 2)
        {
            if (CheckHorizontalMove(ref hits, false)) inputCode = 2;
        }

        if (SwipeController.swipeUp && (bool)(dir.y <= 0 || characterController.isGrounded))
        {
            inputCode = 3;
        }

        if (SwipeController.swipeDown)
        {
            gravity = -80;
            transform.localScale = new Vector3(1, 0.5f, 1);

            Task.Run(() =>
            {
                isSliding = true;

                Thread.Sleep(slideTime);

                isSliding = false;
            });
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstruction")
        {
            Vector3 start;

            if (isSliding)
            {
                RaycastHit hit;

                start = transform.position;
                start.y += 0.5f;

                if (Physics.Raycast(start, transform.TransformDirection(Vector3.right), out hit, 1f) && hit.collider.tag == "Obstruction")
                {
                    Debug.DrawRay(start, transform.TransformDirection(Vector3.right * hit.distance), Color.red);
                    Debug.Log("Slide hit!");
                    Time.timeScale = 0;
                }
                return;
            }

            start = transform.position;
            start.y += 2.2f;
            start.z += 0.61f;

            RaycastHit[] rhit = Physics.RaycastAll(start, transform.TransformDirection(Vector3.down), 2.2f);
            Debug.DrawRay(start, transform.TransformDirection(Vector3.down * rhit[0].distance), Color.red);

            if (rhit.Length != 0 && rhit.LastOrDefault(obj => obj.collider.tag == "Obstruction").collider != null)
            {
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

        hits = Physics.RaycastAll(bodyPos, transform.TransformDirection(Vector3.down), 3f);

        if (hits.Length != 0) Debug.DrawRay(bodyPos, transform.TransformDirection(Vector3.down * hits.Last().distance), Color.red);

        if ((bool)(hits.Length == 1 && hits[0].collider.tag == "RoadComponent") || hits.Length == 0 ) return true;

        return false;
    }
}
