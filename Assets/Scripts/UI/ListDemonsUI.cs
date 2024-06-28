using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ListDemonsUI : MonoBehaviour
{
    public static ListDemonsUI Instance { get; private set; }
    
    private TextMeshProUGUI _list;
    private Dictionary<int, string> _enemiesName = new Dictionary<int, string>();

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
}
