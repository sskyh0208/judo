using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventController : MonoBehaviour
{
    Schedule todayEvent;
    List<School> joinSchoolList;
    List<PlayerManager> joinMember60List;
    List<PlayerManager> joinMember66List;
    List<PlayerManager> joinMember73List;
    List<PlayerManager> joinMember81List;
    List<PlayerManager> joinMember90List;
    List<PlayerManager> joinMember100List;
    List<PlayerManager> joinMemberOver100List;
    // Start is called before the first frame update
    void Start()
    {
        todayEvent = GameData.instance.todayEvent;
        string placeId = GameData.instance.player.schoolId.Substring(0, 6);

        joinSchoolList = new List<School>();
        joinMember60List = new List<PlayerManager>();
        joinMember66List = new List<PlayerManager>();
        joinMember73List = new List<PlayerManager>();
        joinMember81List = new List<PlayerManager>();
        joinMember90List = new List<PlayerManager>();
        joinMember100List = new List<PlayerManager>();
        joinMemberOver100List = new List<PlayerManager>();

        // 団体戦を含むかどうかかどうか
        if (todayEvent.eventType == "all" || todayEvent.eventType == "school")
        {SetJoinScools(placeId);}
        // 個人戦を含むかどうか
        if (todayEvent.eventType == "all" || todayEvent.eventType == "member")
        {SetJoinMembers(placeId);}

        SortSchoolTotalStatus();
        int count = 1;
        foreach (School school in joinSchoolList)
        {
            Debug.Log(string.Format("No.{0}   {1}  部員数{2}", count, school.name, school.members.Count));
            count ++;
        }

        // // 団体確認用
        // List<List<School>> schoolLeaderBord = GenerateEventLeaderBoad<School>(joinSchoolList);
        // Debug.Log("試合開始");
        // List<School> result = DoMatchAllSchool(schoolLeaderBord);
        // for (int i = 1; i < result.Count + 1; i++)
        // {
        //     Debug.Log(string.Format("第{0}位 {1}", i, result[i - 1].name));
        // }

        // // 個人確認用
        // joinMember73List = GetSortMemberTotalStatus(joinMember73List);
        // List<List<PlayerManager>> LeaderBord73 = GenerateEventLeaderBoad<PlayerManager>(joinMember73List);
        // Debug.Log("試合開始");
        // List<PlayerManager> result = DoMatchAllMember(LeaderBord73);
        // for (int i = 1; i < result.Count + 1; i++)
        // {
        //     Debug.Log(string.Format("第{0}位 {1} {2} {3}年生", i, result[i - 1].nameKaki, GameData.instance.schoolManager.GetSchool(result[i - 1].schoolId).name, result[i - 1].positionId));
        // }

        // 個人確認用
        joinMember100List = GetSortMemberTotalStatus(joinMember100List);
        List<List<PlayerManager>> LeaderBord100 = GenerateEventLeaderBoad<PlayerManager>(joinMember100List);
        Debug.Log("試合開始");
        List<PlayerManager> result = DoMatchAllMember(LeaderBord100);
        for (int i = 1; i < result.Count + 1; i++)
        {
            Debug.Log(string.Format("第{0}位 {1} {2} {3}年生", i, result[i - 1].nameKaki, GameData.instance.schoolManager.GetSchool(result[i - 1].schoolId).name, result[i - 1].positionId));
        }

    }

    public void SetJoinScools(string placeId)
    {
        string region = placeId.Substring(0, 2);
        string place = placeId.Substring(2, 2);
        string city = placeId.Substring(2, 4);

        
        List<string> schoolIdList = new List<string>();
        Dictionary<int, string> targetRanking = null;
        YearRanking thisYearRanking = GameData.instance.rankingManager.GetYearRanking(GameData.instance.storyDate.Year);
        // 他校取得範囲決め
        switch (todayEvent.filterType)
        {
            // ランキングに存在する条件の場合取得
            default:
            case "countryRank":
                targetRanking = thisYearRanking.GetCountryRanking("0").school;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    schoolIdList.Add(targetRanking[i]);
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;

            case "regionRank":
                targetRanking = thisYearRanking.GetRegionRanking(region).school;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    schoolIdList.Add(targetRanking[i]);
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;
            case "placeRank":
                targetRanking = thisYearRanking.GetPlaceRanking(place).school;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    schoolIdList.Add(targetRanking[i]);
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;
            case "cityRank":
                targetRanking = thisYearRanking.GetCityRanking(city).school;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    schoolIdList.Add(targetRanking[i]);
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;
        }
        joinSchoolList = GameData.instance.schoolManager.GetApplyJoinEventMembersNumSchools(schoolIdList);
    }

    public void SetJoinMembers(string placeId)
    {
        string region = placeId.Substring(0, 2);
        string place = placeId.Substring(2, 2);
        string city = placeId.Substring(2, 4);
        
        Dictionary<int, string> targetRanking = null;
        YearRanking thisYearRanking = GameData.instance.rankingManager.GetYearRanking(GameData.instance.storyDate.Year);
        // 他校取得範囲決め
        switch (todayEvent.filterType)
        {
            // ランキングに存在する条件の場合取得
            default:
            case "countryRank":
                targetRanking = thisYearRanking.GetCountryRanking("0").members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetCountryRanking("0").members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;

            case "regionRank":
                targetRanking = thisYearRanking.GetRegionRanking(region).members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetRegionRanking(region).members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;

            case "placeRank":
                targetRanking = thisYearRanking.GetPlaceRanking(place).members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetPlaceRanking(place).members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;
            case "cityRank":
                targetRanking = thisYearRanking.GetCityRanking(city).members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetCityRanking(city).members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= todayEvent.filterValue && todayEvent.filterValue != 0) {break;}
                }
                break;
        }
        
    }

    // 総合力が高い順に学校を並べ替え
    public void SortSchoolTotalStatus()
    {
        List<School> tmpSchoolList = new List<School>();
        foreach (School school in joinSchoolList)
        {
            school.SetSchoolTotalStatus();
            tmpSchoolList.Add(school);
        }
        joinSchoolList = tmpSchoolList.OrderByDescending(school => school.totalStatus).ToList();
    }
    
    public List<PlayerManager> GetSortMemberTotalStatus(List<PlayerManager> targetList)
    {
        List<PlayerManager> tmpMemberList = new List<PlayerManager>();
        foreach (PlayerManager member in targetList)
        {
            member.SetTotalStatus();
            tmpMemberList.Add(member);
        }
        return tmpMemberList.OrderByDescending(member => member.totalStatus).ToList();
    }

    public List<List<T>> GenerateEventLeaderBoad<T>(List<T> targetList)
    {
        List<List<T>> leaderBoad = new List<List<T>>();

        // 上位の二つを抽出
        T firstTarget = targetList[0];
        targetList.RemoveAt(0);
        T secondTarget = targetList[0];
        targetList.RemoveAt(0);

        // 残りをシャッフルして２番目に強いやつを最後に入れる
        for (int i = 0; i < targetList.Count; i++)
        {
            T tmp = targetList[i];
            int randomIndex = UnityEngine.Random.Range(i, targetList.Count);
            targetList[i] = targetList[randomIndex];
            targetList[randomIndex] = tmp;
        }

        // 二番目に強いやつを最後に入れる
        targetList.Add(secondTarget);
        // 奇数の場合一番強いやつをシードにする
        if ((targetList.Count + 1) % 2 != 0) {
            leaderBoad.Add(new List<T>(){firstTarget});
        }
        else
        {
            targetList.Insert(0, firstTarget);
        }

        // 子要素初期化
        List<T> matchList = new List<T>();
        // 組み合わせ
        foreach (T target in targetList)
        {
            matchList.Add(target);
            if (matchList.Count == 2)
            {
                leaderBoad.Add(matchList);
                matchList = new List<T>();
            }
        }

        return leaderBoad;
    }

    public List<School> DoMatchAllSchool(List<List<School>> targetBoad)
    {
        List<School> result = new List<School>();
        List<List<School>> matchList = new List<List<School>>(targetBoad);
        while (true)
        {
            List<School> winnerList = new List<School>();
            foreach (List<School> targetMatch in matchList)
            {
                // 試合をする
                List<School> resultMatch = DoMatchSchool(targetMatch);
                if (resultMatch.Count > 1)
                {
                    // 負け
                    result.Add(resultMatch[1]);
                }
                // 勝ち
                winnerList.Add(resultMatch[0]);
                if (resultMatch.Count > 1)
                {Debug.Log(string.Format("勝ち: {0}    負け: {1}", resultMatch[0].name, resultMatch[1].name));}
                else
                {Debug.Log(string.Format("不戦勝: {0}", resultMatch[0].name));}
            }
            if (winnerList.Count == 1)
            {   
                // 優勝決まり
                result.Add(winnerList[0]);
                break;
            }

            List<School> nextMatch = new List<School>();
            matchList = new List<List<School>>();
            bool seedFlag = false;
            if (winnerList.Count % 2 != 0)
            {
                seedFlag = true;
            }
            foreach (School winnerSchool in winnerList)
            {
                nextMatch.Add(winnerSchool);
                if (nextMatch.Count == 2 || seedFlag)
                {
                    matchList.Add(nextMatch);
                    nextMatch = new List<School>();
                    seedFlag = false;
                }
            }
            if (nextMatch.Count > 0)
            {
                matchList.Add(nextMatch);
            }

        }
        result.Reverse();
        return result;
    }
    public List<PlayerManager> DoMatchAllMember(List<List<PlayerManager>> targetBoad)
    {
        List<PlayerManager> result = new List<PlayerManager>();
        List<List<PlayerManager>> matchList = new List<List<PlayerManager>>(targetBoad);
        while (true)
        {
            List<PlayerManager> winnerList = new List<PlayerManager>();
            foreach (List<PlayerManager> targetMatch in matchList)
            {
                // 試合をする
                List<PlayerManager> resultMatch = DoMatchMember(targetMatch);
                if (resultMatch.Count > 1)
                {
                    // 負け
                    result.Add(resultMatch[1]);
                }
                // 勝ち
                winnerList.Add(resultMatch[0]);
            }
            if (winnerList.Count == 1)
            {   
                // 優勝決まり
                result.Add(winnerList[0]);
                break;
            }

            List<PlayerManager> nextMatch = new List<PlayerManager>();
            matchList = new List<List<PlayerManager>>();
            bool seedFlag = false;
            if (winnerList.Count % 2 != 0)
            {
                seedFlag = true;
            }
            foreach (PlayerManager winnerMember in winnerList)
            {
                nextMatch.Add(winnerMember);
                if (nextMatch.Count == 2 || seedFlag)
                {
                    matchList.Add(nextMatch);
                    nextMatch = new List<PlayerManager>();
                    seedFlag = false;
                }
            }
            if (nextMatch.Count > 0)
            {
                matchList.Add(nextMatch);
            }

        }
        result.Reverse();
        return result;
    }

    // 勝ったほうを0番に入れた配列を作成
    public List<School> DoMatchSchool(List<School> targets)
    {
        // 対戦相手がいない場合はそのまま返す
        if (targets.Count == 1)
        {
            return targets;
        }
        List<School> result = new List<School>();
        // ここで試合をする
        // test
        if(targets[0].totalStatus <= targets[1].totalStatus)
        {targets.Reverse(); result = targets;}
        result = targets;
        return result;
    }

    // 勝ったほうを0番に入れた配列を作成
    public List<PlayerManager> DoMatchMember(List<PlayerManager> targets)
    {
        if (targets.Count == 1)
        {
            return targets;
        }
        // 対戦相手がいない場合はそのまま返す
        List<PlayerManager> result = new List<PlayerManager>();
        // ここで試合をする
        // test
        if(targets[0].totalStatus <= targets[1].totalStatus)
        {targets.Reverse(); result = targets;}
        result = targets;
        return result;
    }
}   
