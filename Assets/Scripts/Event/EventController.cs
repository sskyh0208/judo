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

        Tournament taikai = new Tournament(
            GameData.instance.todayEvent,
            GameData.instance.player.schoolId
        );

        taikai.SetTournamentDetail();

        // // 団体確認用
        // taikai.SortSchoolTotalStatus();
        // List<List<School>> schoolLeaderBord = taikai.GenerateEventLeaderBoad<School>(taikai.joinSchoolList);
        // Debug.Log("試合開始");
        // List<School> result = taikai.DoMatchAllSchool(schoolLeaderBord);
        // for (int i = 0; i < result.Count; i++)
        // {
        //     Debug.Log(string.Format("第{0}位 {1}", i + 1, result[i].name));
        // }

        // 個人確認用
        taikai.joinMember60List = taikai.GetSortMemberTotalStatus(taikai.joinMember60List);
        List<List<PlayerManager>> LeaderBord60 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMember60List);
        Debug.Log("60kg試合開始");
        List<PlayerManager> result60 = taikai.DoMatchAllMember(LeaderBord60);
        for (int i = 0; i < result60.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result60[i].nameKaki, GameData.instance.schoolManager.GetSchool(result60[i].schoolId).name, result60[i].positionId, result60[i].totalStatus));
        }

        taikai.joinMember66List = taikai.GetSortMemberTotalStatus(taikai.joinMember66List);
        List<List<PlayerManager>> LeaderBord66 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMember66List);
        Debug.Log("66kg試合開始");
        List<PlayerManager> result66 = taikai.DoMatchAllMember(LeaderBord66);
        for (int i = 0; i < result66.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result66[i].nameKaki, GameData.instance.schoolManager.GetSchool(result66[i].schoolId).name, result66[i].positionId, result66[i].totalStatus));
        }

        taikai.joinMember73List = taikai.GetSortMemberTotalStatus(taikai.joinMember73List);
        List<List<PlayerManager>> LeaderBord73 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMember73List);
        Debug.Log("73kg試合開始");
        List<PlayerManager> result73 = taikai.DoMatchAllMember(LeaderBord73);
        for (int i = 0; i < result73.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result73[i].nameKaki, GameData.instance.schoolManager.GetSchool(result73[i].schoolId).name, result73[i].positionId, result73[i].totalStatus));
        }

        taikai.joinMember81List = taikai.GetSortMemberTotalStatus(taikai.joinMember81List);
        List<List<PlayerManager>> LeaderBord81 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMember81List);
        Debug.Log("81kg試合開始");
        List<PlayerManager> result81 = taikai.DoMatchAllMember(LeaderBord81);
        for (int i = 0; i < result81.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result81[i].nameKaki, GameData.instance.schoolManager.GetSchool(result81[i].schoolId).name, result81[i].positionId, result81[i].totalStatus));
        }

        taikai.joinMember90List = taikai.GetSortMemberTotalStatus(taikai.joinMember90List);
        List<List<PlayerManager>> LeaderBord90 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMember90List);
        Debug.Log("90kg試合開始");
        List<PlayerManager> result90 = taikai.DoMatchAllMember(LeaderBord90);
        for (int i = 0; i < result90.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result90[i].nameKaki, GameData.instance.schoolManager.GetSchool(result90[i].schoolId).name, result90[i].positionId, result90[i].totalStatus));
        }

        taikai.joinMember100List = taikai.GetSortMemberTotalStatus(taikai.joinMember100List);
        List<List<PlayerManager>> LeaderBord100 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMember100List);
        Debug.Log("100kg試合開始");
        List<PlayerManager> result100 = taikai.DoMatchAllMember(LeaderBord100);
        for (int i = 0; i < result100.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result100[i].nameKaki, GameData.instance.schoolManager.GetSchool(result100[i].schoolId).name, result100[i].positionId, result100[i].totalStatus));
        }

        taikai.joinMemberOver100List = taikai.GetSortMemberTotalStatus(taikai.joinMemberOver100List);
        List<List<PlayerManager>> LeaderBordOver100 = taikai.GenerateEventLeaderBoad<PlayerManager>(taikai.joinMemberOver100List);
        Debug.Log("100kg超試合開始");
        List<PlayerManager> resultOver100 = taikai.DoMatchAllMember(LeaderBordOver100);
        for (int i = 0; i < resultOver100.Count; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, resultOver100[i].nameKaki, GameData.instance.schoolManager.GetSchool(resultOver100[i].schoolId).name, resultOver100[i].positionId, resultOver100[i].totalStatus));
        }

    }
}   
