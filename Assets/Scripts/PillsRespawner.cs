using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillsRespawner : MonoBehaviour
{
    List<Interactable> interactables;

    private void Awake()
    {
        GameObject[] goInteractables= GameObject.FindGameObjectsWithTag("Interactable");

        if(goInteractables.Length > 0)
        {
            interactables = new List<Interactable>();
            for (int i = 0; i < goInteractables.Length; i++)
            {
                Interactable aux = goInteractables[i].GetComponent<Interactable>();
                if (aux.interactionType == Interactable.InteractionType.Loot)
                {
                    aux.hasPill = false;
                    interactables.Add(aux);
                }
            }
        }
    }

    public void ShufflePills(int numberOfPills)
    {
        List<int> sortedList = new List<int>();
       
        while(sortedList.Count < numberOfPills)
        {
            int randomNumber = Random.Range(0, interactables.Count);
            if (!sortedList.Contains(randomNumber))
            {
                Interactable sorted = interactables[randomNumber];
                sorted.hasPill = true;
                sortedList.Add(randomNumber);
            }
        }
    }
}
