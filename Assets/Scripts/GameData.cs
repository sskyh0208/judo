using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public DateTime storyDate;
    public static GameData instance;
    public ScheduleManager scheduleManager;
    public SchoolManager schoolManager;
    public PlaceManager placeManager;
    // プレイヤーデータ
    public PlayerManager player {get; set;}
    public AbillityManager abillityManager;
    public NameManager nameManager;
    public Dictionary<string, string[]> schoolCityRank;
    public Dictionary<string, string[]> schoolPlaceRank;
    public Dictionary<string, string[]> schoolRegionRank;
    public Dictionary<string, string[]> schoolCountryRank;
    public Dictionary<string, string[]> memberCityRank;
    public Dictionary<string, string[]> memberPlaceRank;
    public Dictionary<string, string[]> memberRegionRank;
    public Dictionary<string, string[]> memberCountryRank;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNewGameData()
    {
        placeManager = LoadJapanData();
        scheduleManager =LoadScheduleData();
        schoolManager = LoadSchoolData();
        schoolManager.SetAllSchool();
        abillityManager = LoadAbillityData();
        storyDate = GenerateNewStartDate();
        nameManager = LoadNameData();
        for(int i = 0; i < 3; i++)
        {
            DateTime generateDt = new DateTime(storyDate.Year - 16 - i, 4, 1);
            GenerateThisYearPlayers(generateDt, i + 1);
        }
        GenerateSupervisor(storyDate);
    }

    // スケジュールイベント取得
    private ScheduleManager LoadScheduleData()
    {
        string inputString = Resources.Load<TextAsset>("schedule").ToString();
        return JsonUtility.FromJson<ScheduleManager>(inputString);
    }

    // 日本のデータ取得
    private PlaceManager LoadJapanData()
    {
        string inputString = Resources.Load("japan").ToString();
        return JsonUtility.FromJson<PlaceManager>(inputString);
    }

    // 学校のデータ取得
    private SchoolManager LoadSchoolData()
    {
        string inputString = Resources.Load("school").ToString();
        return JsonUtility.FromJson<SchoolManager>(inputString);
    }

    // アビリティのデータ取得
    private AbillityManager LoadAbillityData()
    {
        string inputString = Resources.Load("abillity").ToString();
        return JsonUtility.FromJson<AbillityManager>(inputString);
    }

    // 名前のデータ取得
    private NameManager LoadNameData()
    {
        string inputString = Resources.Load("name").ToString();
        return JsonUtility.FromJson<NameManager>(inputString);
    }

    // 開始日作成
    private DateTime GenerateNewStartDate()
    {
            return new DateTime(2022, 4, 1);
    }

    public DateTime NextDate()
    {
        storyDate = storyDate.AddDays(1);
        return storyDate;
    }

    public Schedule GetScheduleEvent()
    {
        Schedule targetEvent = GameData.instance.scheduleManager.GetSchedule(storyDate);
        return targetEvent;
    }


    public void GenerateSupervisor(DateTime generateDt)
    {
        System.Random r = new System.Random();
        foreach (School school in schoolManager.schoolList.Values)
        {
            int minSense = school.schoolRank - 1;
            if(minSense < 1)
            {
                minSense = 1;
            }
            int maxSense = school.schoolRank + 1;
            string[] name = GameData.instance.nameManager.GenarateRandomName();
            DateTime newGenerateDt = new DateTime(generateDt.Year - r.Next(22, 55), generateDt.Month, generateDt.Day);
            string id = string.Format("{0}{1}{2}", generateDt.Year, school.id, "0");
            school.supervisor = new PlayerManager(id, name[0], name[1], newGenerateDt, 101, school.placeId, school.id, r.Next(minSense, school.schoolRank));
        }
    }

    public void GenerateThisYearPlayers(DateTime generateDt, int generateGrade)
    {
        System.Random r = new System.Random();

        // 経験者　～　強化選手をランダムで配置
        foreach (School school in schoolManager.schoolList.Values)
        {
            int schoolLimitMembersNum = school.GenerateThisYearLimitMembersNum();
            int maxSense = school.schoolRank;
            int minSense = school.schoolRank - 2;
            if (maxSense < 2)
            {
                maxSense = 2;
            }
            if (minSense < 1)
            {
                minSense = 1;
            }
            for(int i = 0; i < schoolLimitMembersNum; i++){
                string[] name = GameData.instance.nameManager.GenarateRandomName();
                string id = string.Format("{0}{1}{2}", generateDt.Year, school.id, i + 1);
                school.members[id] = new PlayerManager(
                    id, name[0], name[1], generateDt, generateGrade, school.placeId, school.id, r.Next(minSense, maxSense)
                );
            }
        }

        // 金メダリストレベル選手作る
        int genSense6PlayerNum = r.Next(1, 3);
        List<string> schoolIds = schoolManager.schoolList.Keys.ToList();
        for(int i = 0; i < genSense6PlayerNum; i++){
            string[] name = GameData.instance.nameManager.GenarateRandomName();
            string targetSchoolId = schoolIds[r.Next(0, schoolIds.Count)];
            string id = string.Format("{0}{1}{2}", generateDt.Year, targetSchoolId, schoolManager.schoolList[targetSchoolId].members.Count + i + 1);
            schoolManager.schoolList[targetSchoolId].members[id] = 
                new PlayerManager(
                    id,name[0], name[1], generateDt, generateGrade, targetSchoolId.Substring(0, 2), targetSchoolId, 6
                );
        }
    }
}
