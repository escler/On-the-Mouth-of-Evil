using UnityEngine;

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
        
        MoveObject();
        RotateObject();
        CompareOrientation();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_canPlace) PlaceObject();
            else if(_canDrop) ThrowObject();
        }
        
    }

    private void PlaceObject()
    {
        piecesCard[_actualPiece].GetComponent<MeshRenderer>().material = cardMaterial;
        Destroy(heldObj);
        _piecePlacesCount++;
        CheckPuzzleState();
    }

    private void CheckPuzzleState()
    {
        if (_piecePlacesCount < piecesCard.Length) return;

        paperPuzzleSalt.SetActive(true);
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
    
    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), PlayerHandler.Instance.GetComponent<Collider>(), false);
        heldObj.layer = 9;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward);
        heldObj = null;
    }
    
    void MoveObject()
    {
        heldObj.transform.position = _player.puzzlePivot.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            _playerCam.CameraLock = true;
            _canDrop = false; 
            
            float XaxisRotation = Input.GetAxis("Mouse X") * _sensX * Time.deltaTime;
            float YaxisRotation = Input.GetAxis("Mouse Y") * _sensY *Time.deltaTime;

            _xRot += XaxisRotation;
            _yRot += YaxisRotation;
       
            heldObj.transform.Rotate(transform.up, XaxisRotation);
            heldObj.transform.Rotate(transform.right, YaxisRotation);
            //heldObjRb.transform.Rotate(-YaxisRotation,XaxisRotation,0);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            _playerCam.CameraLock = false;
            _canDrop = true;

        }
    }

    public void PickUpObject(GameObject pickUpObj, int actualPiece)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            _actualPiece = actualPiece;
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = _player.puzzlePivot;
            heldObj.transform.localScale = Vector3.one;
            heldObj.layer = 2;
  
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);
        }
    }
}
