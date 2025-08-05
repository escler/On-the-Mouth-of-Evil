using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TarotCardPuzzle : MonoBehaviour
{
    public static TarotCardPuzzle Instance { get; private set; }

    public GameObject heldObj;
    public Rigidbody heldObjRb;
    public Transform holdPos;
    private PlayerCam _playerCam;
    private PlayerHandler _player;
    private bool _canDrop, _canPlace, _canPlaceInverse;
    private float _sensX, _sensY, _xRot, _yRot;
    public GameObject[] piecesCardGood;
    private int _actualPiece, _piecePlacesCount;
    public Material cardMaterial;
    public Transform drawer;
    private float _angleX;
    Vector3 reference = Vector3.zero;
    private bool _rotating;

    public bool CanPlace => _canPlace;
    public bool CanPlaceInverse => _canPlaceInverse;
    public AudioSource _good;
    public AudioSource _bad;

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
        
        MoveObject(_rotating ? PlayerHandler.Instance.cameraPos.position + PlayerHandler.Instance.cameraPos.forward * heldObj.GetComponent<PieceTarotCard>().offset: PlayerHandler.Instance.handPivot.position);
        
        CompareOrientationGoodCard();
        
        if (_rotating) RotateObject();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_canPlace || _canPlaceInverse) PlaceObject();
        }
        
    }

    public void PressButton()
    {
        _rotating = !_rotating;
        PlayerHandler.Instance.focusView = _rotating;
        CanvasManager.Instance.rotateInfo.SetActive(_rotating);
        heldObj.GetComponent<Item>().CantBobbing = _rotating;
        ChangePlayerLockState();
    }
    private void PlaceObject()
    {
        MusicManager.Instance.PlaySound("WoodGrab", false);
        var actualPiece = heldObj;
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem, Inventory.Instance.countSelected);
        
        piecesCardGood[_actualPiece].GetComponent<MeshRenderer>().material = cardMaterial;
        piecesCardGood[_actualPiece].GetComponent<MeshRenderer>().enabled = true;

        Destroy(actualPiece);
        _piecePlacesCount++;
        CheckPuzzleState();
        _rotating = false;
        ChangePlayerLockState();
    }

    IEnumerator MoveDrawer()
    {
        if(RitualManager.Instance.altarCompleted) RitualManager.Instance.AltarCompleted();
        while (_angleX < 50f)
        {
            drawer.Rotate(1, 0, 0);

            _angleX = drawer.localRotation.eulerAngles.x;
            yield return new WaitForSeconds(.01f);
        }
    }

    private void CheckPuzzleState()
    {
        if (_piecePlacesCount < piecesCardGood.Length) return;

        _angleX = 0;
        StartCoroutine(MoveDrawer());
    }

    private void CompareOrientationGoodCard()
    {
        var actualCardPiece = piecesCardGood[_actualPiece].transform;
        var orientation = Vector3.Dot(heldObj.transform.forward, actualCardPiece.forward) + Vector3.Dot(heldObj.transform.up, actualCardPiece.up);
        var distance = Vector3.Distance(PlayerHandler.Instance.transform.position, actualCardPiece.position);
        if (orientation > 1.8f && distance < 2f)
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

    void MoveObject(Vector3 position)
    {
        heldObj.transform.position = Vector3.SmoothDamp(heldObj.transform.position,
            position, ref reference,.1f);
    }

    void ChangePlayerLockState()
    {
        _playerCam.CameraLock = _rotating;
        _canDrop = !_rotating;
        Inventory.Instance.cantSwitch = _rotating;
        if(_rotating) PlayerHandler.Instance.UnPossesPlayer();
        else PlayerHandler.Instance.PossesPlayer();
    }

    void RotateObject()
    {
        _playerCam.CameraLock = true;
        _canDrop = false;
        PlayerHandler.Instance.UnPossesPlayer();
        Inventory.Instance.cantSwitch = true;
        
        float XaxisRotation = Input.GetAxis("Horizontal") * _sensX * Time.deltaTime;
        float YaxisRotation = Input.GetAxis("Vertical") * _sensY *Time.deltaTime;
        float ZaxisRotation = Input.GetAxis("ZAxis") * _sensY *Time.deltaTime;
   
        heldObj.transform.RotateAround(heldObj.transform.position, _playerCam.transform.right, YaxisRotation);
        heldObj.transform.RotateAround(heldObj.transform.position, _playerCam.transform.up, XaxisRotation);
        heldObj.transform.RotateAround(heldObj.transform.position, _playerCam.transform.forward, ZaxisRotation);
    }

    public void DeactivateMesh()
    {
        piecesCardGood[_actualPiece].GetComponent<MeshRenderer>().enabled = false;
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
  
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);
        }
    }
}
