using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TarotCardPuzzle : MonoBehaviour
{
    public static TarotCardPuzzle Instance { get; private set; }
    
    public GameObject heldObj, paperPuzzleSalt;
    public Rigidbody heldObjRb;
    public Transform holdPos;
    private PlayerCam _playerCam;
    private PlayerHandler _player;
    private bool _canDrop, _canPlace;
    private float _sensX, _sensY, _xRot, _yRot;
    public GameObject[] piecesCard;
    private int _actualPiece, _piecePlacesCount;
    public Material cardMaterial;
    public Transform drawer;
    private float _angleX;
    Vector3 reference =Vector3.zero;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        holdPos = PlayerHandler.Instance.handPivot;
        _playerCam = PlayerHandler.Instance.playerCam;
        _player = PlayerHandler.Instance;
    }

    private void Update()
    {
        if (heldObj == null) return;
        if (!heldObj.GetComponent<PieceTarotCard>().onHand) heldObj = null;
        
        RotateObject();
        CompareOrientation();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_canPlace) PlaceObject();
        }
        
    }

    private void PlaceObject()
    {
        var actualPiece = heldObj;
        piecesCard[_actualPiece].GetComponent<MeshRenderer>().material = cardMaterial;
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem);
        Destroy(actualPiece);
        _piecePlacesCount++;
        CheckPuzzleState();
    }

    IEnumerator MoveDrawer()
    {
        while (_angleX < 50f)
        {
            drawer.Rotate(1, 0, 0);

            _angleX = drawer.localRotation.eulerAngles.x;
            yield return new WaitForSeconds(.01f);
        }

        paperPuzzleSalt.GetComponent<BoxCollider>().enabled = true;
        paperPuzzleSalt.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void CheckPuzzleState()
    {
        if (_piecePlacesCount < piecesCard.Length) return;

        Inventory.Instance.ChangeUI(Inventory.Instance.countSelected);
        _angleX = 0;
        StartCoroutine(MoveDrawer());
    }

    private void CompareOrientation()
    {
        var actualCardPiece = piecesCard[_actualPiece].transform;
        var orientation = Vector3.Dot(heldObj.transform.forward, actualCardPiece.forward) + Vector3.Dot(heldObj.transform.up, actualCardPiece.up);
        var distance = Vector3.Distance(heldObj.transform.position, actualCardPiece.position);
        if (orientation > 1.9f && distance < 1f)
        {
            actualCardPiece.GetComponent<MeshRenderer>().enabled = true;
            _canPlace = true;
        }
        else
        {
            actualCardPiece.GetComponent<MeshRenderer>().enabled = false;
            _canPlace = false;
        }
        
    }
    
    public void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), PlayerHandler.Instance.GetComponent<Collider>(), false);
        heldObj.layer = 9;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward);
        heldObj = null;
    }

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            heldObj.transform.position = Vector3.SmoothDamp(heldObj.transform.position,
                PlayerHandler.Instance.farFocusPos.position, ref reference,.1f);
            _playerCam.CameraLock = true;
            _canDrop = false;
            Inventory.Instance.cantSwitch = true;
            
            float XaxisRotation = Input.GetAxis("Mouse X") * _sensX * Time.deltaTime;
            float YaxisRotation = Input.GetAxis("Mouse Y") * _sensY *Time.deltaTime;

            _xRot += XaxisRotation;
            _yRot += YaxisRotation;
       
            //heldObj.transform.Rotate(Vector3.up, XaxisRotation);
            //heldObj.transform.Rotate(Vector3.right, YaxisRotation);
            //heldObjRb.transform.Rotate(-YaxisRotation,XaxisRotation,0);

            heldObj.transform.RotateAround(heldObj.transform.position, _playerCam.transform.right, YaxisRotation);
            heldObj.transform.RotateAround(heldObj.transform.position, _playerCam.transform.up, XaxisRotation);


            /*heldObj.transform.Rotate(transform.up, XaxisRotation);
            heldObj.transform.Rotate(transform.right, YaxisRotation);
            heldObjRb.transform.localEulerAngles += transform.up * XaxisRotation + transform.right * YaxisRotation;*/
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            _playerCam.CameraLock = false;
            _canDrop = true;
            Inventory.Instance.cantSwitch = false;
            heldObj.transform.position = PlayerHandler.Instance.handPivot.position;
        }
    }

    public void DeactivateMesh()
    {
        piecesCard[_actualPiece].GetComponent<MeshRenderer>().enabled = false;
    }

    public void PickUpObject(GameObject pickUpObj, int actualPiece)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            _actualPiece = actualPiece;
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObj.transform.localScale = Vector3.one;
            heldObj.layer = 2;
  
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);
        }
    }
}
