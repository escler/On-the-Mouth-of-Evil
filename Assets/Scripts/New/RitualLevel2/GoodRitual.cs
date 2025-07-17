using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GoodRitual : MonoBehaviour
{
    public static GoodRitual Instance { get; private set; }
    public bool leverActivated, nestOnFire;
    [SerializeField] private Transform ritualPos;
    [SerializeField] private int level;
    [SerializeField] private GameObject godRays, ovenFire;
    [SerializeField] private SpriteRenderer stencilBurn;
    [SerializeField] private VisualEffect vaporEffect;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        var stencilColor = stencilBurn.color;
        stencilColor.a = 0;
        stencilBurn.color = stencilColor;
    }

    public void StartFireOven()
    {
        ovenFire.SetActive(true);
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
        while (enemy.enemyVisibility < 4)
        {
            enemy.enemyVisibility += Time.deltaTime * 1.6f;
            enemy.enemyVisibility = Mathf.Clamp(enemy.enemyVisibility, 0, 8);
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);

            yield return null;
        }

        enemy.enemyVisibility = 5;
        enemy.enemyVisibility = Mathf.Clamp(enemy.enemyVisibility, 0, 8);
        enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);

        
        enemy.anim.ChangeState("Exorcism", true);
        
        yield return new WaitUntil(() => enemy.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Burn"));
        enemy.firePs.SetActive(true);
        //vaporEffect.Play();
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

        StartCoroutine(StencilAppear());
        enemy.vfxDissolve.Play();
        
        while (enemy.enemyVisibility > 0)
        {
            enemy.enemyVisibility -= Time.deltaTime * 2;
            enemy.enemyMaterial.SetFloat("_Power", enemy.enemyVisibility);
            yield return null;
        }
        
        //vaporEffect.Stop();
        enemy.vfxDissolve.Stop();
        
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

    IEnumerator StencilAppear()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            var stencilColor = stencilBurn.color;
            stencilColor.a = Mathf.Lerp(0, 1, time);
            stencilBurn.color = stencilColor;
            yield return null;
        }
    }
}
