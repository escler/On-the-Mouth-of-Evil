using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSlot : MonoBehaviour, IInteractable, IInteractObject
{
    [SerializeField] private int position;
    [SerializeField] private Cube cubeInSlot;
    public Cube CubeInSlot => cubeInSlot;
    private bool _cubePlaced, _movingCube, _canRot;

    public void PlaceCube(Cube cube)
    {
        if (cubeInSlot != null) return;
        cubeInSlot = cube;
        cube.GetComponent<BoxCollider>().enabled = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        _cubePlaced = true;
        cube.MoveCube(transform);
        StartCoroutine(EnableBool());
    }

    private void RemoveCube()
    {
        if(cubeInSlot == null) return;
        
        cubeInSlot.GetComponent<BoxCollider>().enabled = true;
        cubeInSlot.OnGrabItem();
        cubeInSlot = null;
        _cubePlaced = false;
        _canRot = false;
    }

    IEnumerator EnableBool()
    {
        yield return new WaitForSeconds(0.1f);
        _canRot = true;
    }

    public void OnInteractItem()
    {
        RemoveCube();
    }

    public void OnInteract(bool hit, RaycastHit i)
    {

    }

    private void Update()
    {
        if(!_movingCube) return;
        if(Input.GetKeyDown(KeyCode.W)) MoveCube(Vector3.right);
        if(Input.GetKeyDown(KeyCode.S)) MoveCube(-Vector3.right);
        if(Input.GetKeyDown(KeyCode.D)) MoveCube(Vector3.forward);
        if(Input.GetKeyDown(KeyCode.A)) MoveCube(-Vector3.forward);
        if(Input.GetKeyDown(KeyCode.Q)) MoveCube(Vector3.up);
        if(Input.GetKeyDown(KeyCode.E)) MoveCube(-Vector3.up);
    }

    private void MoveCube(Vector3 direction)
    {
        _movingCube = false;
        var final = direction;
        StartCoroutine(MoveCubeCor(final));
    }

    IEnumerator MoveCubeCor(Vector3 final)
    {
        float time = 0;
        Vector3 start = cubeInSlot.transform.eulerAngles;
        while (time < 90)
        {
            cubeInSlot.transform.Rotate(final * 3, Space.World);
            time += 3;
            yield return new WaitForSeconds(0.01f);
        }

        _movingCube = true;
    }

    public void OnInteractWithObject()
    {
        if(cubeInSlot == null) return;
        if (_canRot)
        {
            ExitRotCube();
            return;
        }
        
        RotCube();

    }

    private void RotCube()
    {
        PlayerHandler.Instance.UnPossesPlayer();
        _cubePlaced = false;
        _movingCube = true;
        _canRot = true;
    }

    private void ExitRotCube()
    {
        PlayerHandler.Instance.PossesPlayer();
        _movingCube = false;
        _cubePlaced = true;
        _canRot = false;
    }
    
    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return _cubePlaced;
    }

    public void OnInteractWithThisObject()
    {
        OnInteractWithObject();
    }
}
