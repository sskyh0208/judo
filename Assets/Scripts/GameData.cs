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
    public RankingManager rankingManager;

    public Schedule todayEvent;
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
            Debug.Log(string.Format("{0}年生作成中...", i + 1));
            DateTime generateDt = new DateTime(storyDate.Year - 16 - i, 4, 1);
            GenerateThisYearPlayers(new DateTime(storyDate.Year - i, 4, 1), generateDt, i + 1);
        }
        GenerateSupervisor(storyDate);
        // ランキング初期化
        rankingManager = new RankingManager();
        rankingManager.allYearRanking[storyDate.Year] = new YearRanking(storyDate);
        SetDefaultRanking();
    }

    public void GenerateNewYearGameDate()
    {
        GameData.instance.schoolManager.DoneGuradiationAllSchools();
        DateTime generateDt = new DateTime(storyDate.Year - 16, 4, 1);
        GenerateThisYearPlayers(new DateTime(storyDate.Year, 4, 1), generateDt, 1);
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
        SetTodayEvent();
        return storyDate;
    }

    public void SetTodayEvent()
    {
        todayEvent = GetTodayEvent();
    }

    public Schedule GetTodayEvent()
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

    public void GenerateThisYearPlayers(DateTime baseDate, DateTime generateDt, int generateGrade)
    {
        System.Random r = new System.Random();

        // 経験者　～　強化選手をランダムで配置
        foreach (School school in schoolManager.schoolList.Values)
        {
            int schoolLimitMembersNum = school.GenerateThisYearLimitMembersNum();
            int maxSense = school.schoolRank + 1;
            int minSense = school.schoolRank - 1;
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
                string id = string.Format("{0}{1}{2}", baseDate.Year, school.id, i + 1);
                PlayerManager member = new PlayerManager(
                    id, name[0], name[1], generateDt, generateGrade, school.placeId, school.id, r.Next(minSense, maxSense)
                );
                school.members[id] = member;
            }
        }

        // 金メダリストレベル選手作る
        int genSense6PlayerNum = r.Next(1, 3);
        List<string> schoolIds = schoolManager.schoolList.Keys.ToList();
        Debug.Log(string.Format("金メダリストレベル作成数: {0}人", genSense6PlayerNum));
        for(int i = 0; i < genSense6PlayerNum; i++){
            string[] name = GameData.instance.nameManager.GenarateRandomName();
            string targetSchoolId = schoolIds[r.Next(0, schoolIds.Count)];
            string id = string.Format("{0}{1}{2}", baseDate.Year, targetSchoolId, schoolManager.schoolList[targetSchoolId].members.Count + i + 1);
            PlayerManager member = new PlayerManager(
                    id, name[0], name[1], generateDt, generateGrade, targetSchoolId.Substring(0, 2), targetSchoolId, 6
                );
            schoolManager.schoolList[targetSchoolId].members[id] = member;
            string targetCityId = targetSchoolId.Substring(2, 4);
            Debug.Log(string.Format("名前: {0} 高校: {1}", member.nameKaki, schoolManager.schoolList[targetSchoolId].name));
        }
    }

    private void SetDefaultRanking()
    {
        Dictionary<string, List<string>> addRankingSchool = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMember60 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMember66 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMember73 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMember81 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMember90 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMember100 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> addRankingMemberOver100 = new Dictionary<string, List<string>>();
        
        string cityId = "";
        foreach (School school in GameData.instance.schoolManager.schoolList.Values)
        {
            string targetCityId = school.id.Substring(2, 4);
            // 県か市が違えば初期化
            if (cityId != targetCityId) {
                cityId = targetCityId;
                addRankingSchool[cityId] = new List<string>();
                addRankingMember60[cityId] = new List<string>();
                addRankingMember66[cityId] = new List<string>();
                addRankingMember73[cityId] = new List<string>();
                addRankingMember81[cityId] = new List<string>();
                addRankingMember90[cityId] = new List<string>();
                addRankingMember100[cityId] = new List<string>();
                addRankingMemberOver100[cityId] = new List<string>();
            }
            addRankingSchool[cityId].Add(school.id);
            foreach (PlayerManager member in school.GetSortDescMembers())
            {
                if(member.weight < 60){addRankingMember60[cityId].Add(member.id);}
                else if(member.weight < 66){addRankingMember66[cityId].Add(member.id);}
                else if(member.weight < 73){addRankingMember73[cityId].Add(member.id);}
                else if(member.weight < 81){addRankingMember81[cityId].Add(member.id);}
                else if(member.weight < 90){addRankingMember90[cityId].Add(member.id);}
                else if(member.weight < 100){addRankingMember100[cityId].Add(member.id);}
                else {addRankingMemberOver100[cityId].Add(member.id);}
            }
        }

        // 存在する市のkeyを抽出
        List<string> generateCityRankingKeys = new List<string>();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMember60.Keys).ToList();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMember66.Keys).ToList();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMember73.Keys).ToList();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMember81.Keys).ToList();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMember90.Keys).ToList();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMember100.Keys).ToList();
        generateCityRankingKeys = generateCityRankingKeys.Concat(addRankingMemberOver100.Keys).ToList();
        
        // その年のランキング作成
        foreach(string key in generateCityRankingKeys.Distinct())
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[key] = new Ranking(key, storyDate);
        }

        foreach (string addCityId in addRankingSchool.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingSchool[addCityId], 0);
        }
        foreach (string addCityId in addRankingMember60.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMember60[addCityId], 1);
        }
        foreach (string addCityId in addRankingMember66.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMember66[addCityId], 2);
        }
        foreach (string addCityId in addRankingMember73.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMember73[addCityId], 3);
        }
        foreach (string addCityId in addRankingMember81.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMember81[addCityId], 4);
        }
        foreach (string addCityId in addRankingMember90.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMember90[addCityId], 5);
        }
        foreach (string addCityId in addRankingMember100.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMember100[addCityId], 6);
        }
        foreach (string addCityId in addRankingMemberOver100.Keys)
        {
            GameData.instance.rankingManager.allYearRanking[storyDate.Year].cityRanking[addCityId].SetRankingData(addRankingMemberOver100[addCityId], 7);
        }
    }
}
