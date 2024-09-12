using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Player Canvases")]
    [SerializeField] Canvas ScoreCanvas;
    [SerializeField] Canvas DeathCanvas;
    [SerializeField] Canvas MenuCanvas;

    [Header("Player")]
    [SerializeField] GameObject playerObj;
    [SerializeField] Camera     cameraObj;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerAnimationController playerAnimationController;

    //Menu Position - позиция игрока в меню
    //Game Position - позиция игрока во время игры
    [Header("Player Options")]
    [SerializeField] Transform playerMenuPosition;
    [SerializeField] Transform playerGamePosition;

    [Header("Camera options")]
    [SerializeField] Transform cameraMenuPosition;
    [SerializeField] Transform cameraGamePosition;

    [Header("Animators")]
    [SerializeField] Animator GameMenuAnimator;

    [Header("Menu Location")]
    [SerializeField] GameObject MenuLocation;

    [Header("Music")]
    [SerializeField] AudioSource MainGameMusic;

    [Header("Buttons")]
    [SerializeField] Button StartButton;
    [SerializeField] Button button;

    bool isMenu;

    // Start is called before the first frame update
    void Start()
    {
        OpenMenu();

        StartButton.onClick.AddListener(() =>
        {
            if (isMenu) CloseMenu();
        });

        button.onClick.AddListener(() =>
        {
            Debug.Log("Menu button click");
        });


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenMenu()
    {
        isMenu = true;
        GameMenuAnimator.SetBool("IsStart", false);

        ScoreCanvas.gameObject.SetActive(false);
        DeathCanvas.gameObject.SetActive(false);
        MenuCanvas.enabled = true;

        MenuLocation.SetActive(true);

        MainGameMusic.enabled = false;

        playerController.GetCharacterController().enabled = false;

        //Transform
        playerObj.transform.localPosition = playerMenuPosition.localPosition;
        playerObj.transform.localRotation = playerMenuPosition.localRotation;
        playerObj.transform.localScale = playerMenuPosition.localScale;

        //Camera
        cameraObj.transform.localPosition = cameraMenuPosition.localPosition;
        cameraObj.transform.localRotation = cameraMenuPosition.localRotation;
        cameraObj.transform.localScale = cameraMenuPosition.localScale;

        playerController.GetCharacterController().enabled = true;

        playerController.Stop();
    }

    public void CloseMenu()
    {
        isMenu = false;

        ScoreCanvas.gameObject.SetActive(true);
        DeathCanvas.gameObject.SetActive(false);
        MenuCanvas.enabled = false;

        MainGameMusic.enabled = true;

        playerController.GetCharacterController().enabled = false;

        //Transform
        playerObj.transform.localPosition = playerGamePosition.localPosition;
        playerObj.transform.localRotation = playerGamePosition.localRotation;
        playerObj.transform.localScale = playerGamePosition.localScale;

        //Camera
        cameraObj.transform.localPosition = cameraGamePosition.localPosition;
        cameraObj.transform.localRotation = cameraGamePosition.localRotation;
        cameraObj.transform.localScale = cameraGamePosition.localScale;

        playerController.GetCharacterController().enabled = true;

        GameMenuAnimator.SetBool("IsStart", true);
        playerController.Run();
    }

    public ref GameObject GetMenuLocation() { return ref MenuLocation; }

    //Get Menu Position
    public Transform GetPlayerMenuPosition() { return playerMenuPosition; }
    public Transform GetCameraMenuPosition() { return cameraMenuPosition; }

    //Get Game Position
    public Transform GetPlayerGamePosition() { return playerGamePosition; }
    public Transform GetCameraGamePosition() { return cameraGamePosition; }

    public bool IsMenu() { return isMenu; }
}
