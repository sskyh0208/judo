using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class MainController : MonoBehaviour
{
    public Text mainText;
    public static MainController instance;
    private string dateFormatPattern = "yyyy年MM月dd日";

    private void Start()
    {
        DispayDateText();
    }

    private void DispayDateText()
    {
        mainText.text = GameData.instance.storyDate.ToString(dateFormatPattern);
    }

    public void Next()
    {   
        GameData.instance.NextDate();
        DispayDateText();
        // イベントに参加
        if (CheckJoinEvent())
        {
            FadeIOManager.instance.FadeOutToIn( () => SceneManager.LoadScene("Event"));
        }

        GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId).DoneTraining();
    }

    private bool CheckJoinEvent()
    {
        Schedule todayEvent = GameData.instance.GetTodayEvent();
        if(todayEvent.eventName != null)
        {
            Debug.Log(String.Format("本日{0}に{1}が開催される。", GameData.instance.storyDate.ToString(dateFormatPattern), todayEvent.eventName));
            if(todayEvent.eventType == "all" || todayEvent.eventType == "school")
            {
                // フィルターをチェック
                return true;
            }
            if(todayEvent.eventType == "all" || todayEvent.eventType == "member")
            {
                return true;
            }
        }
        return false;
    }

}
