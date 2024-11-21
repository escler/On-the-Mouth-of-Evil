using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorView : MonoBehaviour
{
    public AudioSource openDoor, closeDoor;

    public void OpenDoorAudio()
    {
        openDoor.Play();
    }

    public void CloseDoorAudio()
    {
        closeDoor.Play();
    }
}
