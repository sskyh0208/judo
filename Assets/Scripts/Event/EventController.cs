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

        List<List<School>> schoolLeaderBord = GenerateEventLeaderBoad<School>(joinSchoolList);

        foreach (List<School> bord in schoolLeaderBord)
        {
            int num = 1;
            foreach (School school in bord)
            {
                Debug.Log(string.Format("{0} {1}", num, school.name));
                num ++;
            }
            num = 1;
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

    public List<List<T>> GenerateEventLeaderBoad<T>(List<T> targetList)
    {
        List<T> leaderBoadLeftList = new List<T>();
        List<T> leaderBoadRightList = new List<T>();
        // 第1シード決め
        leaderBoadLeftList.Add(targetList[0]);
        // 第2シード決め
        leaderBoadRightList.Add(targetList[1]);

        // 第3と4以外をシャッフル
        List<T> shuffleTargetListOther = targetList.Skip(4).Take(targetList.Count).ToList();

        for (int i = 0; i < shuffleTargetListOther.Count; i ++)
        {
            if(i % 2 == 0){
                leaderBoadLeftList.Add(shuffleTargetListOther[i]);
            }
            else
            {
                leaderBoadRightList.Add(shuffleTargetListOther[i]);
            }
        }

        // 第3シード決め
        leaderBoadLeftList.Add(targetList[2]);
        // 第4シード決め
        leaderBoadRightList.Add(targetList[3]);


        return new List<List<T>>(){leaderBoadLeftList, leaderBoadRightList};
    }
}   
