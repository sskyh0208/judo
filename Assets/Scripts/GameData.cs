using System;
using System.Collections.Generic;
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
        foreach (School school in schoolManager.schoolArray)
        {
            int minSense = school.rank - 1;
            if(minSense < 1)
            {
                minSense = 1;
            }
            int maxSense = school.rank + 1;
            string[] name = GameData.instance.nameManager.GenarateRandomName();
            DateTime newGenerateDt = new DateTime(generateDt.Year - r.Next(22, 55), generateDt.Month, generateDt.Day);
            school.supervisor = new PlayerManager(name[0], name[1], newGenerateDt, 101, school.placeId, school.id, r.Next(minSense, school.rank));
        }
    }

    public void GenerateThisYearPlayers(DateTime generateDt, int generateGrade)
    {
        System.Random r = new System.Random();

        // 経験者　～　強化選手をランダムで配置
        foreach (School school in schoolManager.schoolArray)
        {
            if(school.members == null)
            {
                school.members = new List<PlayerManager>();
            }
            int schoolLimitMembersNum = school.GenerateThisYearLimitMembersNum();
            int maxSense = school.rank;
            int minSense = school.rank - 2;
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
                school.members.Add(
                    new PlayerManager(name[0], name[1], generateDt, generateGrade, school.placeId, school.id, r.Next(minSense, maxSense))
                );
            }
        }

        // 金メダリストレベル選手作る
        int genSense6PlayerNum = r.Next(1, 3);
        for(int i = 0; i < genSense6PlayerNum; i++){
            string[] name = GameData.instance.nameManager.GenarateRandomName();
            int targetSchoolNum = r.Next(0, schoolManager.schoolArray.Length);
            schoolManager.schoolArray[targetSchoolNum].members.Add(
                new PlayerManager(name[0], name[1], generateDt, generateGrade, schoolManager.schoolArray[targetSchoolNum].placeId, schoolManager.schoolArray[targetSchoolNum].id, 6)
            );
            Debug.Log(schoolManager.schoolArray[targetSchoolNum].name);
        }
    }
}
