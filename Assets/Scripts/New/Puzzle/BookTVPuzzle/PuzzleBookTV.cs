using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleBookTV : MonoBehaviour
{
    public static PuzzleBookTV Instance { get; private set; }
    BookPuzzleTV[] _books = new BookPuzzleTV[3];
    [SerializeField] private string codeNeeded;
    [SerializeField] private VHS vhs;
    [SerializeField] BookSpot[] bookSpots;
    [SerializeField] private KeyBad keyBad;
    [SerializeField] private AudioSource audioSource;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void AddBook(int slot, BookPuzzleTV book)
    {
        if(_books[slot] != null) return;
        _books[slot] = book;
        CheckCode();
    }

    public void RemoveBook(int slot)
    {
        _books[slot] = null;
    }

    private void CheckCode()
    {
        var code = "";
        for (int i = 0; i < _books.Length; i++)
        {
            if (_books[i] == null) break;
            code += _books[i].number.ToString();
        }

        if (codeNeeded != code) WrongCode();
        else CorrectCode();
    }

    private void WrongCode()
    {
        print("Wrong");
    }

    private void CorrectCode()
    {
        audioSource.Play();
        for (int i = 0; i < _books.Length; i++)
        {
            _books[i].enabled = false;
        }

        for (int i = 0; i < bookSpots.Length; i++)
        {
            bookSpots[i].GetComponent<BoxCollider>().enabled = false;
            bookSpots[i].enabled = false;
        }
        
        vhs.MoveSpot();
        keyBad.ChangeLight(true);
    }
}
