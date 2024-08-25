using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjetivesUI : MonoBehaviour
{
    public static ObjetivesUI Instance { get; private set; }
    
    private TextMeshProUGUI _list;
    private Dictionary<int, string> _enemiesName = new Dictionary<int, string>();
    public GameObject bossKilledNotify, skillUI;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        _list = GetComponent<TextMeshProUGUI>();
        _list.text = "";
    }

    public void AddText(int index, string demonsName)
    {
        var add = _enemiesName.TryAdd(index, demonsName);
        if (!add) _enemiesName[index] = demonsName;
        
        var textDemon = "Clear Rooms:\n";

        foreach (var pair in _enemiesName)
        {
            textDemon += "" + pair.Value + "\n";

        }

        _list.text = textDemon;
    }

    public void BossText()
    {
        if (GameManager.Instance.bossKilled) return;
        _list.text = "Defeat Boss";
    }

    public void BossWin()
    {
        _list.text = "Leave House";
    }
    
    public void ClearText()
    {
        StartCoroutine("OnClearText");
    }
    
    IEnumerator OnClearText()
    {
        yield return new WaitForSeconds(2f);
        _list.text = "";
        _enemiesName.Clear();
        yield return null;
    }

    public void BossKilled()
    {
        bossKilledNotify.SetActive(true);
        skillUI.SetActive(true);
    }
}
