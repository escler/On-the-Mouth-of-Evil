using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class RitualManager : MonoBehaviour
{
    public AlphaMaterial[] ritualFloor, ritualBadFloor;
    public List<CandleRitual> candlesInRitual = new List<CandleRitual>();
    private int _candlesBurning;
    public int candlesPlaced;
    public bool candleTaked;
    private Candle _actualCandleTaked;
    public Candle firstCandlePlaced;
    private Transform cameraPos;
    private RaycastHit _hit;
    public LayerMask layermask;
    public Node ritualNode;
    public GameObject floor;
    public List<ParticleSystem> psCrater;
    public VisualEffect heat;
    public GameObject floorCrater;
    public bool altarCompleted;
    public bool candlesBurned;
    public GameObject crater;
    public GameObject[] levitatingItems;
    public GameObject actualItemActive;
    public GameObject circles;
    public GameObject godRayVFX;
    [SerializeField] private HouseEnemy enemy;
    [SerializeField] private Door[] doors;
   
    public static RitualManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        cameraPos = PlayerHandler.Instance.cameraPos;
    }

    public void AltarCompleted()
    {
        return;
        altarCompleted = true;
        circles.SetActive(true);
        if (DecisionsHandler.Instance.badPath)
        {
            //sonidoPuzzleSalt
            crater.SetActive(false);
            
            foreach (var a in ritualBadFloor)
            {
                a.FadeIn();
            }
            foreach (var a in ritualFloor)
            {
                a.FadeOut();
            }
        }
        else
        {
            //sonidoPuzzleSalt
            heat.Stop();
            crater.SetActive(true);
            
            foreach (var a in ritualBadFloor)
            {
                a.FadeOut();
            }
            foreach (var a in ritualFloor)
            {
                a.FadeIn();
            }
        }
    }

    public void CandlePlaced(Candle candle)
    {
        if (candlesPlaced <= 0)
        {
            firstCandlePlaced = candle;
            if (candle.badCandle)
            {
                foreach (var a in ritualBadFloor)
                {
                    a.FadeIn();
                }
            }
            else
            {
                foreach (var a in ritualFloor)
                {
                    a.FadeIn();
                }
            }
        }
        candlesPlaced++;
        if (candlesPlaced < 3) return;

        foreach (var c in candlesInRitual)
        {
            c.candle.gameObject.SetActive(false);
            c.gameObject.SetActive(false);
        }
        DecisionsHandler.Instance.badPath = firstCandlePlaced.badCandle;
        crater.SetActive(!firstCandlePlaced.badCandle);
        StartCoroutine(CheckCandles());
    }

    IEnumerator CheckCandles()
    {
        foreach (var d in doors)
        {
            d.SetDoor(false);
        }
        yield return new WaitForSeconds(1f);
        if (HouseEnemy.Instance == null) enemy.GetComponent<HouseEnemy>().enabled = true;
        HouseEnemy.Instance.RitualReady(ritualNode);
        circles.SetActive(false);
    }

    public void RemoveCandle(Candle candle)
    {
        candlesPlaced--;
        if (candlesPlaced > 0) return;
        firstCandlePlaced = null;
        if (candle.badCandle)
        {
            foreach (var a in ritualBadFloor)
            {
                a.FadeOut();
            }
        }
        else
        {
            foreach (var a in ritualFloor)
            {
                a.FadeOut();
            }
        }
    }

    public void ActivateCraterFloor()
    {
        foreach (var a in ritualFloor)
        {
            a.FadeOut();
        }
        floorCrater.GetComponent<Animator>().SetBool("Fall", true);
        foreach (var crater in psCrater)
        {
            crater.Play();
        }
    }

    public void CloseCrater()
    {
        floorCrater.GetComponent<Animator>().SetBool("Fall", false);
        foreach (var crater in psCrater)
        {
            crater.Stop();
        }
    }

    public void RitualFinish()
    {
        foreach (var a in ritualBadFloor)
        {
            a.FadeOut();
        }
        foreach (var a in ritualFloor)
        {
            a.FadeOut();
        }
    }
}
