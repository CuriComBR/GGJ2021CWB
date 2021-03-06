using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int pillsCount;
    public int crossCount;
    public int holyWaterCount;

    public AudioSource audioSource;
    public AudioClip waterSound;
    public AudioClip pillSound;
    public AudioClip crossSound;

    public Inventory GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        ResetInventory();
    }

    private IEnumerator PlaySound(AudioClip audioClip)
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void ResetInventory()
    {
        pillsCount = 0;
        crossCount = 0;
        holyWaterCount = 0;
    }

    public void AddPill()
    {
        StartCoroutine(PlaySound(pillSound));
        pillsCount++;
    }

    public void AddCross()
    {
        StartCoroutine(PlaySound(crossSound));
        pillsCount++;
    }

    public void AddHolyWater()
    {
        StartCoroutine(PlaySound(waterSound));
        pillsCount++;
    }
}
