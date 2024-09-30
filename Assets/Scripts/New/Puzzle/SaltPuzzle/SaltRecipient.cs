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

    public void HightlightObject(Material mat)
    {
        if (_cantInteract) return;
        foreach (var mesh in meshes)
        {
            mesh.material = mat;
        }
    }

    public void UnHighlightObject()
    {
        bodyMesh.materials = baseMats;
        topMesh.material = _normalTopMat;
    }

    public void OnRecipientPress()
    {
        if (_cantInteract) return;
        _buttonPress = !_buttonPress;
        var selectedSalt = Inventory.Instance.selectedItem.GetComponent<Transform>();
        selectedSalt.position = saltPivot.position;
        selectedSalt.SetParent(saltPivot);
        Inventory.Instance.selectedItem.GetComponent<Salt>().PlacingBool();
        SaltPuzzleTable.Instance.canInteractWithSalt = true;
        if (_buttonPress) SaltPuzzle.Instance.AddRecipient(this, buttonNumber);
        else SaltPuzzle.Instance.DeleteRecipient(this);
        _cantInteract = false;
        UnHighlightObject();
        StartCoroutine(WaitForMove());
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
        _buttonPress = false;
        nextPos = _buttonPress ? pressPosition : unPressPosition;
    }

    private void OnEnable()
    {
        _cantInteract = false;
    }

    private void OnDisable()
    {
        if (!finish) return;
        _cantInteract = true;
        UnHighlightObject();
    }

    public void OpenAnimation()
    {
        _animator.SetBool("Interact", true);
    }

    public void DisableBool()
    {
        _animator.SetBool("Interact", false);
    }

    IEnumerator WaitForMove()
    {
        Inventory.Instance.cantSwitch = true;
        OpenAnimation();
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("OpenRecipient") && _animator.GetBool("Interact") == false);
        MoveRecipient();
        var selectedSalt = Inventory.Instance.selectedItem.GetComponent<Transform>();
        selectedSalt.position = PlayerHandler.Instance.handPivot.position;
        selectedSalt.SetParent(PlayerHandler.Instance.handPivot);
        Inventory.Instance.cantSwitch = false;
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        SaltPuzzleTable.Instance.canInteractWithSalt = false;
    }

    public void OnInteractItem()
    {
        
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
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
