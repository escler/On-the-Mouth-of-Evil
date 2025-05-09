using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSlot : MonoBehaviour, IInteractable, IInteractObject
{
    [SerializeField] private int position;
    [SerializeField] private Cube cubeInSlot;
    public Cube CubeInSlot => cubeInSlot;
    private bool _cubePlaced, _movingCube, _canRot, _rotatingPhase;
    public bool RotatingPhase => _rotatingPhase;
    [SerializeField] private AudioSource rotateAudioSource, audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaceCube(Cube cube)
    {
        if (cubeInSlot != null) return;
        _cubePlaced = true;
        audioSource.Play();
        cube.GetComponent<BoxCollider>().enabled = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        cube.MoveCube(transform);
        StartCoroutine(PlaceCubeCor(cube));
        CubePuzzle.Instance.CheckCode();
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

    IEnumerator PlaceCubeCor(Cube cube)
    {
        yield return new WaitForSeconds(0.1f);
        cubeInSlot = cube;
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
        if(!_rotatingPhase) return;
        if(Input.GetKeyDown(KeyCode.W)) MoveCube(Vector3.right);
        if(Input.GetKeyDown(KeyCode.S)) MoveCube(-Vector3.right);
        if(Input.GetKeyDown(KeyCode.D)) MoveCube(Vector3.forward);
        if(Input.GetKeyDown(KeyCode.A)) MoveCube(-Vector3.forward);
        if(Input.GetKeyDown(KeyCode.Q)) MoveCube(Vector3.up);
        if(Input.GetKeyDown(KeyCode.E)) MoveCube(-Vector3.up);
    }

    private void MoveCube(Vector3 direction)
    {
        if (_movingCube) return;
        _movingCube = true;
        var final = direction;
        StartCoroutine(MoveCubeCor(final));
    }

    IEnumerator MoveCubeCor(Vector3 final)
    {
        float time = 0;
        rotateAudioSource.Play();
        while (time < 90)
        {
            cubeInSlot.transform.Rotate(final * 3, Space.World);
            time += 3;
            yield return new WaitForSeconds(0.01f);
        }

        _movingCube = false;
    }

    public void OnInteractWithObject()
    {
        if(cubeInSlot == null) return;
        if (_movingCube) return;
        if (_rotatingPhase)
        {
            ExitRotCube();
            CubePuzzle.Instance.CheckCode();
        }
        else
        {
            RotCube();
        }
    }

    private void RotCube()
    {
        StartCoroutine(MovePlaceCube(transform.GetChild(0).position));
        CanvasManager.Instance.rotateInfo.SetActive(true);
        PlayerHandler.Instance.UnPossesPlayer();
        _rotatingPhase = true;
        _cubePlaced = false;
        StartCoroutine(EnableBool());
    }

    private void ExitRotCube()
    {
        StartCoroutine(MovePlaceCube(transform.position));
        CanvasManager.Instance.rotateInfo.SetActive(false);
        PlayerHandler.Instance.PossesPlayer();
        _rotatingPhase = false;
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

    IEnumerator MovePlaceCube(Vector3 final)
    {
        var start = cubeInSlot.transform.position;
        float time = 0;
        while (time < 1)
        {
            cubeInSlot.transform.position = Vector3.Lerp(start, final, time);
            time += Time.deltaTime * 4;
            yield return null;
        }
    }
}
