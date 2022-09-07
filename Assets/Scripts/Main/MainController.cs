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
        else
        {
            // 参加しない場合は結果を裏で作成
            DoneEvent();
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
            if (todayEvent.eventId == "01")
            {
                return true;
            }
            if(todayEvent.eventType == "all" || todayEvent.eventType == "school")
            {
                string pattern = "";
                switch (todayEvent.eventId)
                {
                    case "02":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + GameData.instance.player.schoolId.Substring(0, 4) + "[0-9]{2}01";
                        break;
                    case "03":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + GameData.instance.player.schoolId.Substring(0, 2) +"[0-9]{4}02";
                        break;
                    case "04":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + "[0-9]{6}01";
                        break;
                    case "05":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + "[0-9]{6}02";
                        break;
                    case "06":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + "[0-9]{6}03";
                        break;
                }
                List<Ranking> targetRankingList = GameData.instance.matchManager.GetRankingList(GameData.instance.storyDate.Year, pattern);
                foreach (Ranking targetRanking in targetRankingList)
                {
                    for(int i = 0; i < targetRanking.school.Count; i++)
                    {
                        if (GameData.instance.player.schoolId == targetRanking.school[i].id)
                        {
                            return true;
                        }
                        if(i >= todayEvent.filterValue && todayEvent.filterValue != 99) {break;}
                    }
                }
            }
            if(todayEvent.eventType == "all" || todayEvent.eventType == "member")
            {
                List<PlayerManager> targetMembers = new List<PlayerManager>();
                List<PlayerManager> joinMembers = new List<PlayerManager>();
                string pattern = "";
                switch (todayEvent.eventId)
                {
                    case "02":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + GameData.instance.player.schoolId.Substring(0, 4) + "[0-9]{2}01";
                        break;
                    case "03":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + GameData.instance.player.schoolId.Substring(0, 2) +"[0-9]{4}02";
                        break;
                    case "04":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + "[0-9]{6}01";
                        break;
                    case "05":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + "[0-9]{6}02";
                        break;
                    case "06":
                        pattern = "^" + GameData.instance.storyDate.Year.ToString() + "[0-9]{6}03";
                        break;
                }
                List<Ranking> targetRankingList = GameData.instance.matchManager.GetRankingList(GameData.instance.storyDate.Year, pattern);
                foreach (Ranking targetRanking in targetRankingList)
                {
                    for (int weightClass = 1; weightClass < 8; weightClass++)
                    {

                        switch (weightClass)
                        {
                            case 1:
                                targetMembers = targetRanking.members60;
                                break;
                            case 2:
                                targetMembers = targetRanking.members66;
                                break;
                            case 3:
                                targetMembers = targetRanking.members73;
                                break;
                            case 4:
                                targetMembers = targetRanking.members81;
                                break;
                            case 5:
                                targetMembers = targetRanking.members90;
                                break;
                            case 6:
                                targetMembers = targetRanking.members100;
                                break;
                            case 7:
                                targetMembers = targetRanking.membersOver100;
                                break;
                        }
                        targetMembers = GameData.instance.matchManager.SortMemberTotalStatus(targetMembers);
                        for(int i = 0; i < targetMembers.Count; i++)
                        {
                            if (GameData.instance.player.schoolId == targetMembers[i].schoolId)
                            {
                                return true;
                            }
                            if(i >= todayEvent.filterValue && todayEvent.filterValue != 99) {break;}
                            // 48人以上は参加させない
                            if (joinMembers.Count == 48)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private void DoneEvent()
    {
        List<string> tournamentIdList = GameData.instance.todayEvent.GetTournamentIdList();
        foreach (string id in tournamentIdList)
        {
            Tournament taikai = new Tournament(
                GameData.instance.todayEvent,
                GameData.instance.storyDate,
                id
            );

            GameData.instance.matchManager.history.Add(taikai);
        }
    }
}
