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
        joinSchoolList = GameData.instance.schoolManager.GetSchools(schoolIdList);
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
}   
