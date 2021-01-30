using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Image inventoryBox;
    private Camera cam;

    bool opened = false;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (opened)
        {
            float distance = Vector2.Distance(Player.Instance.transform.position, transform.position);
            if (distance > 1f){
                inventoryBox.gameObject.SetActive(false);
                opened = false;
            }
        }
    }

    internal void Interact()
    {
        opened = true;
        Vector3 position = transform.position + new Vector3(0, 1);
        inventoryBox.transform.position = cam.WorldToScreenPoint(position);

        inventoryBox.gameObject.SetActive(true);
    }
}
