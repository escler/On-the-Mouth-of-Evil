using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltRecipient : MonoBehaviour
{
    public int buttonNumber;
    public bool _buttonPress, _cantInteract;
    private Material _normalMat;
    public Transform pressPosition, unPressPosition, nextPos;
    private Vector3 reference = Vector3.zero;
    public bool finish;
    
    private List<MeshRenderer> meshes;

    private void Awake()
    {
        meshes = new List<MeshRenderer>();
        _normalMat = GetComponentInChildren<MeshRenderer>().material;
        var objectMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in objectMeshes)
        {
            meshes.Add(mesh);
        }

        nextPos = unPressPosition;
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
        foreach (var mesh in meshes)
        {
            mesh.material = _normalMat;
        }
    }

    public void OnRecipientPress()
    {
        if (_cantInteract) return;
        _buttonPress = !_buttonPress;
        if (_buttonPress) SaltPuzzle.Instance.AddRecipient(this, buttonNumber);
        else SaltPuzzle.Instance.DeleteRecipient(this);
        _cantInteract = false;
        UnHighlightObject();
        MoveRecipient();
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
        print("ASD");
        UnHighlightObject();
    }
}
