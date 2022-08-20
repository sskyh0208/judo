using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventController : MonoBehaviour
{
    Schedule todayEvent;
    // Start is called before the first frame update
    void Start()
    {
        List<string> tournamentIdList = new List<string>(){};
        switch (GameData.instance.todayEvent.eventId)
        {
            case "01":
                tournamentIdList = new List<string>()
                {
                    "073101",
                    "073102",
                    "073103",
                    "073201",
                    "073202",
                    "073203",
                    "073301",
                    "073302",
                    "073303",
                    "073304",
                    "073401",
                    "073402",
                    "073403",
                    "073404",
                    "073405",
                    "073501",
                    "073502",
                    "073503",
                    "073504",
                    "073505"
                };
                break;
            case "02":
                tournamentIdList = new List<string>()
                {
                    "073100",
                    "073200",
                    "073300",
                    "073400",
                    "073500"
                };
                break;
            case "03":
                tournamentIdList = new List<string>()
                {
                    "070000",
                };
                break;
            case "04":
                tournamentIdList = new List<string>()
                {
                    "000000",
                };
                break;
            case "05":
                tournamentIdList = new List<string>()
                {
                    "000000",
                };
                break;
        }
        foreach (string id in tournamentIdList)
        {
            Tournament taikai = new Tournament(
                GameData.instance.todayEvent,
                GameData.instance.storyDate,
                id
            );

            // 団体確認用
            if (taikai.CheckTeamMatch())
            {
                taikai.DoMatchAllSchool();
                taikai.DebugResultTeamMatchDetail();
                taikai.DebugRankingTeamMatch();
            }

            // 個人確認用
            if(taikai.CheckIndividualMatch())
            {
                for (int i = 1; i < 8; i++)
                {
                    taikai.DoIndividualMatch(i);
                    taikai.DebugResultMemberMatchDetail(i);
                    taikai.DebugRankingMemberMatch(i);
                }
            }
            GameData.instance.matchManager.history.Add(taikai);
        }

    }
}   
