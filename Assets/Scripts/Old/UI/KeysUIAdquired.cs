using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeysUIAdquired : MonoBehaviour
{
    public static KeysUIAdquired Instance { get; private set; }
    private TextMeshProUGUI _list;
    private List<string> _keyList = new List<string>();

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

    public void AddText(string keyRoom)
    {
        _keyList.Add(keyRoom);
        UpdateText();
    }

    public void UpdateText()
    {
        var textRoom = "";

        foreach (var key in _keyList)
        {
            textRoom += "" + key + "\n";

        }

        _list.text = textRoom;
    }

    public void RemoveText(string removeText)
    {
        if (!_keyList.Contains(removeText)) return;
        _keyList.Remove(removeText);
        UpdateText();
    }
}
