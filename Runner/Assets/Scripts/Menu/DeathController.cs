using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeathController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] Canvas scoreCanvas;

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
        scoreCanvas.gameObject.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
