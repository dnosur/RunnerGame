using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeathController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreCount;
    [SerializeField] TextMeshProUGUI highscoreCount;
    [SerializeField] TextMeshProUGUI coinsCount;

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    [Header("deathScreen")]
    [SerializeField] GameObject deathScreen;

    public void ShowDeathScreen()
    {
        scoreCount.text = playerController.GetScore().ToString();
        deathScreen.SetActive(true);
    }


    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
