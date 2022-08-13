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

        // 団体確認用
        if (taikai.CheckTeamMatch())
        {
            Debug.Log("試合開始");
            List<School> result = taikai.DoMatchAllSchool();
            for (int i = 0; i < result.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1}", i + 1, result[i].name));
            }
        }

        // 個人確認用
        if(taikai.CheckIndividualMatch())
        {
            Debug.Log("60kg試合開始");
            List<PlayerManager> result60 = taikai.DoIndividualMatch("60");
            for (int i = 0; i < result60.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result60[i].nameKaki, GameData.instance.schoolManager.GetSchool(result60[i].schoolId).name, result60[i].positionId, result60[i].totalStatus));
            }
            Debug.Log("66kg試合開始");
            List<PlayerManager> result66 = taikai.DoIndividualMatch("66");
            for (int i = 0; i < result66.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result66[i].nameKaki, GameData.instance.schoolManager.GetSchool(result66[i].schoolId).name, result66[i].positionId, result66[i].totalStatus));
            }
            Debug.Log("73kg試合開始");
            List<PlayerManager> result73 = taikai.DoIndividualMatch("73");
            for (int i = 0; i < result73.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result73[i].nameKaki, GameData.instance.schoolManager.GetSchool(result73[i].schoolId).name, result73[i].positionId, result73[i].totalStatus));
            }
            Debug.Log("81kg試合開始");
            List<PlayerManager> result81 = taikai.DoIndividualMatch("81");
            for (int i = 0; i < result81.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result81[i].nameKaki, GameData.instance.schoolManager.GetSchool(result81[i].schoolId).name, result81[i].positionId, result81[i].totalStatus));
            }
            Debug.Log("90kg試合開始");
            List<PlayerManager> result90 = taikai.DoIndividualMatch("90");
            for (int i = 0; i < result90.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result90[i].nameKaki, GameData.instance.schoolManager.GetSchool(result90[i].schoolId).name, result90[i].positionId, result90[i].totalStatus));
            }
            Debug.Log("100kg試合開始");
            List<PlayerManager> result100 = taikai.DoIndividualMatch("100");
            for (int i = 0; i < result100.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, result100[i].nameKaki, GameData.instance.schoolManager.GetSchool(result100[i].schoolId).name, result100[i].positionId, result100[i].totalStatus));
            }
            Debug.Log("Over100kg試合開始");
            List<PlayerManager> resultOver100 = taikai.DoIndividualMatch("Over100");
            for (int i = 0; i < resultOver100.Count; i++)
            {
                Debug.Log(string.Format("第{0}位 {1} {2} {3}年生 {4}", i + 1, resultOver100[i].nameKaki, GameData.instance.schoolManager.GetSchool(resultOver100[i].schoolId).name, resultOver100[i].positionId, resultOver100[i].totalStatus));
            }
        }

    }
}   
