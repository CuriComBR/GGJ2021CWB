using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    private Queue<string> sentences;
    private bool start = false;
    private AudioSource audioSource;

    public AudioClip openDialog;
    public AudioClip closeDialog;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update(){
        if(start){ 
            if (Input.GetKeyDown(KeyCode.Space)){
                DisplayNextSentence();
            }

        }
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StarDialogue (Dialogue dialogue){
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(string sentence in dialogue.setences){
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        start = true;
    }
    public void DisplayNextSentence(){
        if (sentences.Count == 0){
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        PlaySound(openDialog);
    }

    void EndDialogue(){
        animator.SetBool("IsOpen", false);
        PlaySound(closeDialog);
        start = false;
        Player.instance.Unlock();
    }
}
// temp apenas para copiar
/*
public Dialogue dialogue;
 
FindObjectOfType<DialogueManager>().StarDialogue(dialogue);

*/