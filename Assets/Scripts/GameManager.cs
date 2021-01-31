using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Level Config")]
    [SerializeField] private int goalNumberOfPills;
    [SerializeField] private float timeToComplete;
    [SerializeField] private PillsRespawner pillsRespawner;
    [SerializeField] private Text timer;
    [SerializeField] private Text pills;

    private float remainingTime;
    private float endTime;

    private bool gameOver = false;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pillsRespawner.ShufflePills(goalNumberOfPills);
        endTime = Time.time + timeToComplete;
    }

    void Update()
    {
        if (!gameOver)
        {
            remainingTime = endTime - Time.time;
            timer.text = TimeSpan.FromSeconds(remainingTime).ToString("mm\\:ss");
            CheckVictory();
        }
       
        pills.text = Inventory.instance.pillsCount.ToString();
    }

    private void CheckVictory()
    {
        if(remainingTime <= 0)
        {
            gameOver = true;
            Lose();
        }
        else
        {
            if(Inventory.instance.pillsCount == goalNumberOfPills)
            {
                gameOver = true;
                Win();
            }
        }
    }

    public void Win()
    {
        Player.instance.Lock();
        //Anim song
        Invoke("GoToNextLevel", 8f);
    }

    public void Lose()
    {
        //Lose song
        Invoke("RestartLevel", 5f);
    }


    public void GoToNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void RestartLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }
}
