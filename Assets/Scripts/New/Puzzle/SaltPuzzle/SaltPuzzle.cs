using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaltPuzzle : MonoBehaviour
{
    public static SaltPuzzle Instance { get; private set; }
    
    public SaltRecipient[] recipients;
    public Dictionary<SaltRecipient, int> recipientSolution;
    public int[] solution;
    private int count;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        recipientSolution = new Dictionary<SaltRecipient, int>();
    }

    public void AddRecipient(SaltRecipient recipient, int number)
    {
        recipientSolution.Add(recipient, number);
        if (recipientSolution.Count == 4) CheckPuzzleSolve();
    }

    public void DeleteRecipient(SaltRecipient recipient)
    {
        if (!recipientSolution.ContainsKey(recipient)) return;

        recipientSolution.Remove(recipient);
    }

    private void CheckPuzzleSolve()
    {
        count = 0;
        foreach (var pair in recipientSolution)
        {
            if (solution.Contains(pair.Value))
            {
                count++;
                continue;
            }

            break;
        }

        if (count >= 4)
        {
            foreach (var recipient in recipients)
            {
                recipient.finish = true;
                RitualManager.Instance.AltarCompleted();
            }
        }
        else
        {
            foreach (var recipient in recipients)
            {
                recipient.ResetRecipient();   
            }

            recipientSolution.Clear();
        }
    }
}
