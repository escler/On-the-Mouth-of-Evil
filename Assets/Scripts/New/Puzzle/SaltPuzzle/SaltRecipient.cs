using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltRecipient : MonoBehaviour, IInteractable
{
    public int buttonNumber;
    public bool _buttonPress, _cantInteract;
    public Material _normalTopMat;
    public Material[] baseMats;
    public Transform pressPosition, unPressPosition, nextPos, saltPivot;
    private Vector3 reference = Vector3.zero;
    public bool finish;
    private Animator _animator;
    public MeshRenderer bodyMesh, topMesh;
    private bool animationRunning, test;
    
    private List<MeshRenderer> meshes;

    private void Awake()
    {
        meshes = new List<MeshRenderer>();
        var objectMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in objectMeshes)
        {
            meshes.Add(mesh);
        }

        nextPos = unPressPosition;
        _animator = GetComponent<Animator>();
    }

    public void OnRecipientPress()
    {
        if (_cantInteract) return;
        _buttonPress = !_buttonPress;
        _animator.SetBool("Filled", _buttonPress);
        if(_buttonPress)
        {
            var selectedSalt = Inventory.Instance.selectedItem.GetComponent<Transform>();
            selectedSalt.position = saltPivot.position;
            selectedSalt.SetParent(saltPivot);
            Inventory.Instance.selectedItem.GetComponent<Salt>().PlacingBool();
        }
        SaltPuzzleTable.Instance.canInteractWithSalt = true;
        _cantInteract = false;
        if (_buttonPress) StartCoroutine(WaitToFill());
        else StartCoroutine(WaitToClear());
    }

    private void MoveRecipient()
    {
        nextPos = _buttonPress ? pressPosition : unPressPosition;
    }

    private void Update()
    {
        
        if (Vector3.Distance(transform.position, nextPos.position) < 0.01f)
        {
            if (finish)
            {
                enabled = false;
                return;
            }
            _cantInteract = false;
            return;
        }
        transform.position = Vector3.SmoothDamp(transform.position, nextPos.position, ref reference, 20f * Time.deltaTime);
    }

    public void ResetRecipient()
    {
        if (_buttonPress)
        {
            _buttonPress = false;
            _animator.SetBool("Filled", _buttonPress);
            StartCoroutine(WaitToClear());
        }
    }

    private void OnEnable()
    {
        _cantInteract = false;
    }

    private void OnDisable()
    {
        if (!finish) return;
        _cantInteract = true;
    }

    IEnumerator WaitToFill()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("OpenRecipient"));
        Inventory.Instance.cantSwitch = true;
        animationRunning = true;
        yield return new WaitUntil(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenRecipient"));
        SaltPuzzle.Instance.AddRecipient(this, buttonNumber);
        var selectedSalt = Inventory.Instance.selectedItem.GetComponent<Transform>();
        selectedSalt.position = PlayerHandler.Instance.handPivot.position;
        selectedSalt.SetParent(PlayerHandler.Instance.handPivot);
        Inventory.Instance.cantSwitch = false;
        SaltPuzzleTable.Instance.canInteractWithSalt = false;
    }
    
    IEnumerator WaitToClear()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("UnloadFill"));
        Inventory.Instance.cantSwitch = true;
        animationRunning = true;
        yield return new WaitUntil(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName("UnloadFill"));
        SaltPuzzle.Instance.DeleteRecipient(this);
        Inventory.Instance.cantSwitch = false;
        SaltPuzzleTable.Instance.canInteractWithSalt = false;
    }

    public void OnInteractItem()
    {
        
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void SetAnimationFinish()
    {
        animationRunning = false;
    }

    public string ShowText()
    {
        if (Inventory.Instance.selectedItem == null)
        {
            print("Llegue aca");
            return "";
        }
        if (Inventory.Instance.selectedItem.itemName == "Salt")
        {
            return _buttonPress ? "Clear Recipient" : "Fill recipient";
        }
        print("Llegue aca2");
        return "";
    }
}
