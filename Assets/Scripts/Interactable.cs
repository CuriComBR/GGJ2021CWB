using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Image dialogBox;
    [SerializeField] public InteractionType interactionType;
    [SerializeField] public bool hasPill;
    [SerializeField] public bool playDialogOnStart;

    private AudioSource audioSource;

    public Dialogue dialogue;
    //Posição para aonde a porta vão levar o jogador
    public float portalX = 0;
    public float portalY = 0;
    //para fazer o fade in e out quando vai mudar de sala, pode usar também para mudar de fase
    public Animator animator;
    //Salva a frase original para caso tiver a pilular depois poder devolver e nas próximas interações com o mesmo objeto avisar que não tem mais.
    private string frase;
    //mudei para porta no lugar de observação pois as observações vão ser identicas as conversas, só colocar o nome do objeto como name e o texto nas sentencas
    //Sempre deixar como uma sentece quando for para loot e deixar no objeto a frase padrão para quando não tem a pilula, a frase para quando o hasPill for true é mudada no script.
    public enum InteractionType
    {
        Dialog, Door, Loot
    }

    // Start is called before the first frame update
    void Awake()
    {
        frase = dialogue.setences[0];
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (playDialogOnStart)
        {
            Player.instance.Lock();
            DialogInteraction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Interact()
    {
        Player.instance.Lock();

        switch (interactionType)
        {
            case InteractionType.Dialog:
                DialogInteraction();
                break;
            case InteractionType.Door:
                StartCoroutine(Teleport());
                break;
            case InteractionType.Loot:
                LootInteraction();
                break;
        }
    }

    private void LootInteraction()
    {
        //Já consegue trabalhar com o texto quando tem e depois voltando o texto para o padrão, se conseguir marcar o bool de 
        //hasPill aleatóriamente todos os objetos já tem um texto para quando não tem pilula e o texto para quando está com a pilula é mudado por aqui no script.
        if (hasPill)
        {
            //não funcionou essa função por isso eu desativei, só precisa registrar que a pilula está com o player e mostrar na UI, depois ele pode voltar para a vovo e terminar a fase.
            Inventory.instance.AddPill();
            dialogue.setences[0] = ("Ótimo achei uma Pilula!");
            hasPill = false;
        }
        FindObjectOfType<DialogueManager>().StarDialogue(dialogue);
        dialogue.setences[0] = frase;
    }

    //apenas paras as portas usaveis na fase, usar DialogInteraction para as demais
    IEnumerator Teleport(){
        animator.SetBool("Fade", true);
        yield return new WaitForSeconds(1);
        Player.instance.transform.position = new Vector2(portalX,portalY);
        Player.instance.Unlock();
        animator.SetBool("Fade", false);
    }

    private void DialogInteraction()
    {
        if (playDialogOnStart)
        {
            audioSource.Play();
        }
        FindObjectOfType<DialogueManager>().StarDialogue(dialogue);
    }
}
