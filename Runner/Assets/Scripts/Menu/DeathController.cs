using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DeathController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] Canvas scoreCanvas;

    [Header("Scores")]
    [SerializeField] TextMeshProUGUI scoreCount;
    [SerializeField] TextMeshProUGUI coinsCount;

    [Header("Death Text")]
    [SerializeField] TextMeshProUGUI deathScoreCount;
    [SerializeField] TextMeshProUGUI deathHighscoreCount;
    [SerializeField] TextMeshProUGUI deathCoinsCount;

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    [Header("Road Controller")]
    [SerializeField] RoadController roadController;

    [Header("Menu Controller")]
    [SerializeField] MenuController menuController;

    [Header("Death Screen")]
    [SerializeField] GameObject deathScreen;

    public void ShowDeathScreen()
    {
        scoreCount.text = playerController.GetScore().ToString();
        scoreCanvas.gameObject.SetActive(false);
        deathScreen.SetActive(true);

        Saves.ScoreModel score = SavesController.GetData<Saves.ScoreModel>("score");

        deathScoreCount.text = scoreCount.text;
        deathHighscoreCount.text = score.highscore.ToString();
        deathCoinsCount.text = coinsCount.text;

        if (Convert.ToInt32(scoreCount.text) > score.highscore) score.highscore = Convert.ToInt32(scoreCount.text);

        score.coins += Convert.ToInt32(coinsCount.text);

        SavesController.Save<Saves.ScoreModel>("score", score);
    }


    //Добавьте здесь перезапуск музыки 
    public void RestartScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        roadController.ClearMap();
        menuController.CloseMenu();

        Time.timeScale = 1;
    }

    public void Menu()
    {
        roadController.ClearMap();
        menuController.OpenMenu();

        Time.timeScale = 1;
    }
}
