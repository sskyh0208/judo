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
        string holdPlaceId = GameData.instance.player.schoolId.Substring(startIndex: 0, 6);
        List<string> targetList = GameData.instance.schoolManager.GetSamePlaceIdList(holdPlaceId);
        foreach (string id in targetList)
        {
            Tournament taikai = new Tournament(
                GameData.instance.todayEvent,
                GameData.instance.storyDate,
                id
            );

            // 団体確認用
            if (taikai.CheckTeamMatch())
            {
                Debug.Log("試合開始");
                List<School> result = taikai.DoMatchAllSchool();
                for (int i = 0; i < result.Count; i++)
                {
                    Debug.Log(string.Format("{0} 第{1}位 {2}", GameData.instance.todayEvent.eventId, i + 1, result[i].name));
                }
                taikai.ranking.school = result;
            }

            // 個人確認用
            if(taikai.CheckIndividualMatch())
            {
                Debug.Log("60kg試合開始");
                List<PlayerManager> result60 = taikai.DoIndividualMatch("60");
                DebugMemberResult(result60);
                Debug.Log("66kg試合開始");
                List<PlayerManager> result66 = taikai.DoIndividualMatch("66");
                DebugMemberResult(result66);
                Debug.Log("73kg試合開始");
                List<PlayerManager> result73 = taikai.DoIndividualMatch("73");
                DebugMemberResult(result73);
                Debug.Log("81kg試合開始");
                List<PlayerManager> result81 = taikai.DoIndividualMatch("81");
                DebugMemberResult(result81);
                Debug.Log("90kg試合開始");
                List<PlayerManager> result90 = taikai.DoIndividualMatch("90");
                DebugMemberResult(result90);
                Debug.Log("100kg試合開始");
                List<PlayerManager> result100 = taikai.DoIndividualMatch("100");
                DebugMemberResult(result100);
                Debug.Log("Over100kg試合開始");
                List<PlayerManager> resultOver100 = taikai.DoIndividualMatch("Over100");
                DebugMemberResult(resultOver100);
                taikai.ranking.members60 = result60;
                taikai.ranking.members66 = result66;
                taikai.ranking.members73 = result73;
                taikai.ranking.members81 = result81;
                taikai.ranking.members90 = result90;
                taikai.ranking.members100 = result100;
                taikai.ranking.membersOver100 = resultOver100;
            }
            GameData.instance.matchManager.history.Add(taikai);
            if (!GameData.instance.todayEvent.eventId.Equals("01"))
            {
                break;
            }
        }
    }

    public void DebugMemberResult(List<PlayerManager> members)
    {
        for (int i = 0; i < members.Count; i++)
        {
            Debug.Log(
                string.Format(
                    "{0} 第{1}位 {2} {3} {4}年生 {5}cm {6}kg {7}",
                    GameData.instance.todayEvent.eventId,
                    i + 1,
                    members[i].nameKaki,
                    GameData.instance.schoolManager.GetSchool(members[i].schoolId).name, members[i].positionId,
                    members[i].height,
                    members[i].weight,
                    members[i].totalStatus
                )
            );
        }
    }
}   
