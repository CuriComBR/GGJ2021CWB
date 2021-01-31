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

    public AudioSource audioSource;
    public AudioSource gameSource;

    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip timeEndingSound;

    private float remainingTime;
    private float endTime;

    private bool gameOver = false;
    private bool timeEnding = false;

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

            if(remainingTime <= 11 && !timeEnding)
            {
                timeEnding = true;
                timer.color = Color.red;
                InvokeRepeating("PlayEndingSound", 0, 1f);
            }
        }
       
        pills.text = Inventory.instance.pillsCount.ToString();
    }

    private void PlayEndingSound()
    {
        audioSource.clip = timeEndingSound;
        audioSource.Play();
    }

    private IEnumerator PlaySound(AudioClip audioClip,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void CheckVictory()
    {
        if(remainingTime <= 0)
        {
            CancelInvoke("PlayEndingSound");
            gameSource.Stop();
            gameOver = true;
            Lose();
        }
        else
        {
            if(Inventory.instance.pillsCount == goalNumberOfPills)
            {
                CancelInvoke("PlayEndingSound");
                gameSource.Stop();
                gameOver = true;
                Win();
            }
        }
    }

    public void Win()
    {
        Player.instance.Lock();
        StartCoroutine(PlaySound(winSound, 2f));
        StartCoroutine(GoToNextLevel());
    }

    public void Lose()
    {
        StartCoroutine(PlaySound(loseSound, 0));
        StartCoroutine(RestartLevel());
    }

    public IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(8);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(5);
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }
}
