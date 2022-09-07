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
    public MatchManager matchManager;
    public Schedule todayEvent;
    public Tournament todayJoinTournament;

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
        matchManager = new MatchManager();
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
            int minSense = school.schoolRank;
            int maxSense = school.schoolRank + 3;
            string[] name = GameData.instance.nameManager.GenarateRandomName();
            DateTime newGenerateDt = new DateTime(generateDt.Year - r.Next(22, 55), generateDt.Month, generateDt.Day);
            string id = string.Format("{0}{1}{2}", generateDt.Year, school.id, "0");
            school.supervisor = new PlayerManager(id, name[0], name[1], newGenerateDt, 101, school.placeId, school.id, r.Next(minSense, maxSense));
        }
    }

    public void GenerateThisYearPlayers(DateTime baseDate, DateTime generateDt, int generateGrade)
    {
        System.Random r = new System.Random();

        // 経験者　～　強化選手をランダムで配置
        foreach (School school in schoolManager.schoolList.Values)
        {
            int schoolLimitMembersNum = school.GenerateThisYearLimitMembersNum();
            int randomHighSenseNum = r.Next(0, 100);
            for(int i = 0; i < schoolLimitMembersNum; i++){
                string[] name = GameData.instance.nameManager.GenarateRandomName();
                string id = string.Format("{0}{1}{2}", baseDate.Year, school.id, i + 1);
                if (i == randomHighSenseNum)
                {
                    Debug.Log("ハイセンス生成: " + school.name + " → " + name[0]+name[1]);
                    PlayerManager member = new PlayerManager(
                        id, name[0], name[1], generateDt, generateGrade, school.placeId, school.id, school.schoolRank + 2
                    );
                    school.members[id] = member;
                    if (member.height > 189)
                    {
                        Debug.Log(string.Format("190cm超え生成: {0} {1} {2}年 {3}cm {4}kg", school.name, member.nameKaki, member.positionId, member.height, member.weight));
                    }
                }
                else
                {
                    PlayerManager member = new PlayerManager(
                        id, name[0], name[1], generateDt, generateGrade, school.placeId, school.id, school.schoolRank
                    );
                    school.members[id] = member;
                    
                    if (member.height > 189)
                    {
                        Debug.Log(string.Format("190cm超え生成: {0} {1} {2}年 {3}cm {4}kg {5}", school.name, member.nameKaki, member.positionId, member.height, member.weight, member.totalStatus));
                    }
                }
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
                    id, name[0], name[1], generateDt, generateGrade, targetSchoolId.Substring(0, 2), targetSchoolId, 11
                );
            schoolManager.schoolList[targetSchoolId].members[id] = member;
            string targetCityId = targetSchoolId.Substring(2, 4);
            Debug.Log(string.Format("名前: {0} 高校: {1}", member.nameKaki, schoolManager.schoolList[targetSchoolId].name));
        }
    }
}
