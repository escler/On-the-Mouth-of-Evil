using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform pivotBook;
    [SerializeField] private GameObject bookPlace;
    [SerializeField] private BookPuzzleTV bookPuzzleTV;
    [SerializeField] private GameObject glowBook;
    public BookPuzzleTV BookPuzzleTV => bookPuzzleTV;
    public int slot;
    private bool _canGrab;

    public void PlaceBook(BookPuzzleTV book)
    {
        if (bookPuzzleTV != null || _canGrab) return;
        PuzzleBookTV.Instance.HideGlows();
        bookPuzzleTV = book;
        StartCoroutine(EnableBool(true));
        PuzzleBookTV.Instance.AddBook(slot,book);
        book.transform.position = pivotBook.position;
        book.transform.rotation = pivotBook.localRotation;
        book.GetComponent<BoxCollider>().enabled = false;
        book.GetComponent<Rigidbody>().isKinematic = true;
        book.transform.parent = pivotBook;

    }

    public void ShowGlow()
    {
        if (bookPuzzleTV != null) return;
        glowBook.SetActive(true);
    }
    
    public void HideGlow()
    {
        glowBook.SetActive(false);
    }
    public void OnInteractItem()
    {
        if(bookPuzzleTV == null) return;
        if(!_canGrab) return;
        PuzzleBookTV.Instance.RemoveBook(slot);
        bookPuzzleTV.GetComponent<BoxCollider>().enabled = true;
        bookPuzzleTV.OnGrabItem();
        bookPuzzleTV = null;
        _canGrab = false;
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {

    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return bookPuzzleTV == null ? false : true;
    }

    IEnumerator EnableBool(bool state)
    {
        yield return new WaitForSeconds(0.2f);
        _canGrab = state;
    }
}
