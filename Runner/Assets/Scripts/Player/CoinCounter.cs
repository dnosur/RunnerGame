using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI coinCount;

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        coinCount.text = playerController.GetCoins().ToString();
    }
}
