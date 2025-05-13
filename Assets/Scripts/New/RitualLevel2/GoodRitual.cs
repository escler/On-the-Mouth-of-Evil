using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodRitual : MonoBehaviour
{
    public static GoodRitual Instance { get; private set; }
    public bool leverActivated;
    [SerializeField] private Transform ritualPos;
    [SerializeField] private int level;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void StartRitual()
    {
        StartCoroutine(GoodRitualSteps());
    }

    IEnumerator GoodRitualSteps()
    {
        PlayerHandler.Instance.movement.ritualCinematic = true;
        Inventory.Instance.cantSwitch = true;

        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        
        //Lo que activamos al hacer el good ritual

        yield return new WaitForSeconds(1f);

        RitualManager.Instance.levitatingItems[0].SetActive(true);
        RitualManager.Instance.actualItemActive = RitualManager.Instance.levitatingItems[0].gameObject;
        
        GameObject item = RitualManager.Instance.levitatingItems[0].gameObject;
        PlayerHandler.Instance.movement.absorbEnd = true;
        PlayerHandler.Instance.movement.GoToVoodoo(item.transform.position);
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inVoodooPos);
        yield return new WaitForSeconds(2f);
        PlayerHandler.Instance.playerCam.CameraLock = true;
        item.GetComponent<LevitatingItem>().grabbed = true;
        if(Inventory.Instance.selectedItem != null) Inventory.Instance.selectedItem.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        PlayerHandler.Instance.movement.ritualCinematic = false;
        PlayerHandler.Instance.movement.absorbEnd = false;
        
        PathManager.Instance.ChangePrefs(DecisionsHandler.Instance.badPath ? "BadPath" : "GoodPath", level);
        GameManagerNew.Instance.LoadCurrencyStats("Hub",5);
    }
}
