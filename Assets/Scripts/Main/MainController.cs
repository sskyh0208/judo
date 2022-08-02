using UnityEngine;
using UnityEngine.UI;
using System;

public class MainController : MonoBehaviour
{
    public Text mainText;
    public static MainController instance;
    private string dateFormatPattern = "yyyy年MM月dd日";

    private void Start()
    {
        DispayDateText();
        checkEvent();
        // foreach(Abillity abillity in GameData.instance.player.abillities)
        // {
        //     Debug.Log(String.Format("技ID: {0}  センス: {1}　上限: {2}  能力: {3}", abillity.id, abillity.sense, abillity.limit, abillity.status));
        // }
        // Debug.Log(String.Format("監督名: {0}  身長: {1}  体重: {2}  才能: {3}  生年月日: {4}", GameData.instance.player.name, GameData.instance.player.height, GameData.instance.player.weight, GameData.instance.player.sense, GameData.instance.player.birthDay));

        foreach(PlayerManager member in GameData.instance.schoolManager.getSchool(GameData.instance.player.placeId, GameData.instance.player.schoolId).members)
        {
            Debug.Log(String.Format("{0} {1} {2} {3} {4} {5}", member.nameKaki, member.nameYomi, member.height, member.weight, member.sense, member.birthDay));
        }
    }

    private void DispayDateText()
    {
        mainText.text = GameData.instance.storyDate.ToString(dateFormatPattern);
    }

    public void Next()
    {   
        GameData.instance.NextDate();
        DispayDateText();
        checkEvent();
    }

    private void checkEvent()
    {
        Schedule DayEvent = GameData.instance.GetScheduleEvent();
        if(DayEvent.eventName != null)
        {
            Debug.Log(String.Format("本日{0}に{1}が開催される。", GameData.instance.storyDate.ToString(dateFormatPattern), DayEvent.eventName));
        }
        else
        {
            Debug.Log(String.Format("{0} イベントなし", GameData.instance.storyDate.ToString(dateFormatPattern)));
        }
    }

}
