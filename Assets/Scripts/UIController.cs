using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [HideInInspector] private LogController logController;
    [HideInInspector] private string levelTextDefault;

    [Header("Elements")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Text levelText;

    [Header("Sounds")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource gameOverSound;

    private void Start()
    {
        logController = GameObject.FindGameObjectWithTag("LogController").GetComponent<LogController>();
        levelTextDefault = levelText.text;
    }

    private void FixedUpdate()
    {
        levelText.text = levelTextDefault + logController.currentLevel.ToString();
    }

    public void ShowGameOverScreen()
    {
        bgm.Stop();
        gameOverSound.Play();

        Time.timeScale = 0;

        gameOverScreen.SetActive(true);
    }
}
