using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BadRitual : MonoBehaviour
{
    public static BadRitual Instance { get; private set; }
    public bool baitBoxPlaced;
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
        DecisionsHandler.Instance.badPath = true;
        StartCoroutine(BadRitualSteps());
    }

    IEnumerator BadRitualSteps()
    {
        PlayerHandler.Instance.movement.ritualCinematic = true;
        Inventory.Instance.cantSwitch = true;

        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        yield return new WaitUntil(() => MorgueEnemy.Instance.inRitualNode);
        var enemy = MorgueEnemy.Instance;
        while (enemy.enemyVisibility < 8)
        {
            enemy.enemyVisibility += Time.deltaTime * 0.8f;
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);

            if (enemy.enemyVisibility >= 2f)
            {
                //_enemyAnimator.ChangeStateAnimation("Absorb", true);
            }

            yield return null;
        }

        enemy.absorbVFX.SetActive(true);
        enemy.magnetVFX.SetActive(true);
        
        while (enemy.enemyVisibility > 0)
        {
            enemy.enemyVisibility -= Time.deltaTime * 1.6f;
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);

            yield return null;
        }
        RitualManager.Instance.levitatingItems[1].SetActive(true);
        RitualManager.Instance.actualItemActive = RitualManager.Instance.levitatingItems[1].gameObject;
        enemy.absorbVFX.GetComponent<VisualEffect>().Stop();
        enemy.magnetVFX.GetComponent<VisualEffect>().Stop();

        yield return new WaitForSeconds(2f);
        
        GameObject item = RitualManager.Instance.levitatingItems[1].gameObject;
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
