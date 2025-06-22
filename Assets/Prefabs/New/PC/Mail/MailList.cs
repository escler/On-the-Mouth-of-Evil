using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MailList : MonoBehaviour
{
    [SerializeReference] private string[] goodSubject;
    [SerializeReference] private string[] badSubject;
    private string[] goodText = new []
    {
        "Dear Child of the Faith,\n\nYour actions in the recent mission have brought joy to the hearts of the devoted. The path of righteousness is not easy, but you walk it with grace.\nHeaven watches, and approves.\nWith blessings,\n\nThe Ecclesiastical Council",
        "Dear Faithful One,\n\nWe rejoice at the news of your deeds. Compassion, courage, and obedience to the Sacred Doctrine are visible in your steps.\nKeep your soul steady. The journey is long, but salvation is near.\nIn sacred trust,\n\nThe Ecclesiastical Council",
        "Dear Disciple,\n\nThe trials you have overcome reflect a spirit molded by the divine.\nNot all choose to do good when darkness offers shortcuts. But you did. And the Light remembers.\nRemain steadfast.\nBy His Grace,\n\nThe Ecclesiastical Council",
        "Dear Servant of Truth,\n\nYour work is bearing fruit, and the Church stands behind you in prayer and strength.\nYou are not alone. Angels guide you. The congregation honors your devotion.\nSoon, the veil shall lift.\nWith solemn reverence,\n\nThe Ecclesiastical Council",
        "Dear Chosen One,\n\nYou have reached the end of this holy journey, and your soul shines.\nFew walk the road without falter, but you have not only walked—it is written—you have uplifted others along the way.\nYour name shall be spoken in the light.\nMay eternal peace find you,\n\nThe Ecclesiastical Council"
    };
    private string[] badText = new []
    {
        "To the One Who Strayed,\n\nWe have observed your actions with grave concern. The mission was not meant to serve selfish ends.\nThere is time to repent, but the first step is to acknowledge your deviation.\nSeek the truth, before the truth seeks you.\nIn sorrow,\n\nThe Ecclesiastical Council",
        "Wandering Soul,\n\nYour continued disregard for the sacred path endangers not just your soul, but others who follow you.\nThe Church does not turn away lightly—but our patience thins.\nDo not confuse silence for acceptance.\nHeed this warning.\n\nThe Ecclesiastical Council",
        "To the Fallen Pilgrim,\n\nThere are whispers now, of your name among the shadows. The Divine sees all.\nEven now, redemption is not beyond your reach—but the longer you wait, the farther you drift.\nReturn. Or be lost.\nWith dwindling hope,\n\nThe Ecclesiastical Council",
        "Lost One,\n\nYou have scorned every sign, rejected every plea.\nYou walk a path where few return. Know that the Church mourns the soul you once were.\nThere may be no more letters after this.\nIn lament,\n\nThe Ecclesiastical Council",
        "To the Condemned,\n\nThis is the last time we reach out.\nYour actions have spoken, and so shall the heavens. The door is closed—not by our will, but by yours.\nNo light awaits where you go.\nMay your soul find what it sought,\n\nThe Ecclesiastical Council"
    };
    [SerializeField] private GameObject[] mails;

    [SerializeField] private GameObject textGO;

    private void OnEnable()
    {
        var mailEntry = MailHandler.Instance.GetEmailLog();
        if (mailEntry == null) return;

        for (int i = 0; i < mailEntry.Count; i++)
        {
            mails[i].SetActive(true);
            var text = mails[i].GetComponentInChildren<TextMeshProUGUI>();
            text.text = mailEntry[i].path == "bad" ? badSubject[mailEntry[i].message_index - 1] : goodSubject[mailEntry[i].message_index - 1];
            mails[i].GetComponent<MailData>().emailText = mailEntry[i].path == "bad" ? 
                badText[mailEntry[i].message_index - 1] : goodText[mailEntry[i].message_index - 1];
        }
    }

    public void ChangeText(string text)
    {
        textGO.SetActive(true);
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    
    
}
