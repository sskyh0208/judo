using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
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
        GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId).DoneTraining();
        GameData.instance.NextDate();
        DispayDateText();
        // イベントに参加
        if (CheckJoinEvent())
        {
            FadeIOManager.instance.FadeOutToIn( () => SceneManager.LoadScene("Event"));
        }

        if (GameData.instance.storyDate.Month == 4 && GameData.instance.storyDate.Day == 1)
        {
            Debug.Log(string.Format("{0}年 新学期スタート", GameData.instance.storyDate.Year));
            GameData.instance.GenerateNewYearGameDate();
        }

    }

    private bool CheckJoinEvent()
    {
        Schedule todayEvent = GameData.instance.GetTodayEvent();
        if(todayEvent.eventName != null)
        {
            Debug.Log(String.Format("本日{0}に{1}が開催される。", GameData.instance.storyDate.ToString(dateFormatPattern), todayEvent.eventName));
            foreach (Tournament taikai in GameData.instance.matchManager.GenarateNewTournaments(GameData.instance.storyDate, todayEvent))
            {
                if (taikai.is_myschool)
                {
                    GameData.instance.todayJoinTournament = taikai;
                    return true;
                }
            }
        }
        return false;
    }
}
