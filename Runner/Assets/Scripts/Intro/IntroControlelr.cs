using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroControlelr : MonoBehaviour
{

    [SerializeField] VideoPlayer clip;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitForEnd());
        Debug.Log(clip.frame);

        if(/*clip.frame >= 750 &&*/ Input.GetKey(KeyCode.Space))
        {
            StopAllCoroutines();
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds((float)clip.length);
        SceneManager.LoadScene(1);
    }
}
