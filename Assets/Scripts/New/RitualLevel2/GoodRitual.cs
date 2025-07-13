using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodRitual : MonoBehaviour
{
    public static GoodRitual Instance { get; private set; }
    public bool leverActivated, nestOnFire;
    [SerializeField] private Transform ritualPos;
    [SerializeField] private int level;
    [SerializeField] private GameObject godRays;

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
        DecisionsHandler.Instance.badPath = false;
        StartCoroutine(GoodRitualSteps());
    }

    IEnumerator GoodRitualSteps()
    {
        yield return new WaitUntil(() => nestOnFire);
        var enemy = MorgueEnemy.Instance;
        enemy.ritualDone = true;
        PlayerHandler.Instance.movement.ritualCinematic = true;
        Inventory.Instance.cantSwitch = true;

        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        
        //Lo que activamos al hacer el good ritual

        yield return new WaitUntil(() => MorgueEnemy.Instance.inRitualNode);
        while (enemy.enemyVisibility < 5)
        {
            enemy.enemyVisibility += Time.deltaTime * 1.6f;
            enemy.enemyVisibility = Mathf.Clamp(enemy.enemyVisibility, 0, 8);
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);
            print(enemy.enemyVisibility);

            if (enemy.enemyVisibility >= 2f)
            {
                //_enemyAnimator.ChangeStateAnimation("Absorb", true);
            }

            yield return null;
        }
        
        enemy.anim.ChangeState("Exorcism", true);
        
        yield return new WaitUntil(() => enemy.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Burn"));
        enemy.firePs.SetActive(true);
        enemy.burnFlame.Play();
        enemy.burnSound.Play();
        var enemyClip = enemy.anim.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        var speed = enemy.anim.animator.speed;
        var realDuration = enemyClip / speed;
        float startVisibility = enemy.enemyVisibility;
        float elapsed = 0f;

        yield return new WaitUntil(() => enemy.startDisappear);

        while (enemy.enemyVisibility > 3)
        {
            enemy.enemyVisibility -= Time.deltaTime;
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        
        while (enemy.enemyVisibility > 0)
        {
            enemy.enemyVisibility -= Time.deltaTime * 2;
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);
            yield return null;
        }
        
        var emission = enemy.firePs.GetComponentsInChildren<ParticleSystem>();
        foreach (var e in emission)
        {
            var emit = e.emission;
            emit.rateOverTime = 0;
        }
        
        yield return new WaitForSeconds(1f);
        godRays.SetActive(true);

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
        MailHandler.Instance.AddEmail("good");
    }
}
