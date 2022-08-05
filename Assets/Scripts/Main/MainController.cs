using UnityEngine;
using UnityEngine.UI;
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
        CheckEvent();
    }

    private void DispayDateText()
    {
        mainText.text = GameData.instance.storyDate.ToString(dateFormatPattern);
    }

    public void Next()
    {   
        GameData.instance.NextDate();
        DispayDateText();
        CheckEvent();

        GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId).DoneTraining();
    }

    private void CheckEvent()
    {
        Schedule dayEvent = GameData.instance.GetScheduleEvent();
        if(dayEvent.eventName != null)
        {
            Debug.Log(String.Format("本日{0}に{1}が開催される。", GameData.instance.storyDate.ToString(dateFormatPattern), dayEvent.eventName));
            School result = null;
            if(dayEvent.filters["school"] != null)
            {
                // フィルターをチェック
                result = GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId);
            }
            if(dayEvent.filters["member"] != null)
            {
                result = GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId);
            }
            // 参加する
            if (result != null)
            {
            }
        }
        else
        {
            Debug.Log(String.Format("{0} イベントなし", GameData.instance.storyDate.ToString(dateFormatPattern)));
        }
    }

}
