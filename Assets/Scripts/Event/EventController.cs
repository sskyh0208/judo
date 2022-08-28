using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour
{
    Schedule todayEvent;
    public GameObject placeNameTextPrefab;
    private GameObject resultScrollView;
    private GameObject selectedClassObj;
    private List<SchoolMatch> selectedSchoolMatch;
    private List<MemberMatch> selectedMemberMatch;

    private Tournament taikai;
    bool is_test = false;
    // Start is called before the first frame update
    void Start()
    {
        // テスト用
        TestDataGenerate();
        List<string> tournamentIdList = GetTournamentIdList(GameData.instance.todayEvent);
        foreach (string id in tournamentIdList)
        {
            this.taikai = new Tournament(
                GameData.instance.todayEvent,
                GameData.instance.storyDate,
                id
            );

            // 団体確認用
            if (this.taikai.CheckTeamMatch())
            {
                this.taikai.DoMatchAllSchool();
                this.taikai.DebugResultTeamMatchDetail();
                this.taikai.DebugRankingTeamMatch();
            }

            // 個人確認用
            if(this.taikai.CheckIndividualMatch())
            {
                for (int i = 1; i < 8; i++)
                {
                    this.taikai.DoIndividualMatch(i);
                    this.taikai.DebugResultMemberMatchDetail(i);
                    this.taikai.DebugRankingMemberMatch(i);
                }
            }
            GameData.instance.matchManager.history.Add(this.taikai);
            SetClassScrollViewContent();
        }

    }
    private List<string> GetTournamentIdList(Schedule schedule)
    {
        List<string> idList = new List<string>();
        if (this.is_test)
        {
            idList = new List<string>()
                {
                    GameData.instance.player.schoolId.Substring(0, 6)
                };
        }
        else
        {
            
            switch (schedule.eventId)
            {
                case "01":
                    idList = new List<string>()
                    {
                        "073101",
                        "073102",
                        "073103",
                        "073201",
                        "073202",
                        "073203",
                        "073301",
                        "073302",
                        "073303",
                        "073304",
                        "073401",
                        "073402",
                        "073403",
                        "073404",
                        "073405",
                        "073501",
                        "073502",
                        "073503",
                        "073504",
                        "073505"
                    };
                    break;
                case "02":
                    idList = new List<string>()
                    {
                        "073100",
                        "073200",
                        "073300",
                        "073400",
                        "073500"
                    };
                    break;
                case "03":
                    idList = new List<string>()
                    {
                        "070000",
                    };
                    break;
                case "04":
                    idList = new List<string>()
                    {
                        "000000",
                    };
                    break;
                case "05":
                    idList = new List<string>()
                    {
                        "000000",
                    };
                    break;
                default:
                    idList = new List<string>();
                    break;
            }
        }   
        return idList;
    }

    private void TestDataGenerate()
    {
        GameData.instance.LoadNewGameData();
        GameData.instance.todayEvent = GameData.instance.scheduleManager.GetSchedule(new DateTime(2022, 5, 1));
        GameData.instance.player = GameData.instance.schoolManager.GetSchool("073404087").supervisor;
        this.is_test = true;
    }

    private void SetClassScrollViewContent()
    {
        resultScrollView = GameObject.Find("EventUICanvas").transform.Find("ResultScrollView").gameObject;
        // 団体戦、個人戦60kgなどのボタンを作成する
        GameObject classScrollViewContent = GameObject.Find("ClassScrollViewContent");
        if (this.taikai.ranking.school.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "団体戦";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultTeam(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.members60.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "60kg級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.members66.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "66kg級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.members73.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "73kg級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.members81.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "81kg級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.members90.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "90kg級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.members100.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "100kg級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
        if (this.taikai.ranking.membersOver100.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "100kg超級";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultWeightClass(_text);
            });
            trigger.triggers.Add(entry);
        }
    }

    private int ConvetToClassNum(string weightClass)
    {
        int weightNum = 0;
        switch (weightClass)
        {
            default:
            case "60kg級":
                weightNum = 1;
                break;
            case "66kg級":
                weightNum = 2;
                break;
            case "73kg級":
                weightNum = 3;
                break;
            case "81kg級":
                weightNum = 4;
                break;
            case "90kg級":
                weightNum = 5;
                break;
            case "100kg級":
                weightNum = 6;
                break;
            case "100kg超級":
                weightNum = 7;
                break;
        }
        return weightNum;
    }

    private void SelectedResultWeightClass(GameObject targetClassObj)
    {
        string _text = targetClassObj.GetComponent<Text>().text;
        this.selectedClassObj = targetClassObj;
        this.selectedMemberMatch = this.taikai.GetMemberMatch(this.ConvetToClassNum(_text));
        DisplayResultScrollView(true);
        DisplayClassResultScrollViewContent(true, this.ConvetToClassNum(_text));
    }

    private void SelectedResultTeam(GameObject targetClassObj)
    {
        string _text = targetClassObj.GetComponent<Text>().text;
        this.selectedClassObj = targetClassObj;
        this.selectedSchoolMatch = this.taikai.allSchoolMatchResult;
        DisplayResultScrollView(true);
        DisplayTeamResultScrollViewContent(true);
    }

    private void DisplayResultScrollView(bool isDisplay)
    {   
        resultScrollView.SetActive(isDisplay);
    }

    private void DisplayClassResultScrollViewContent(bool isDisplay, int weightClass)
    {
        GameObject Viewport = resultScrollView.transform.Find("Viewport").gameObject;
        foreach (MemberMatch match in this.selectedMemberMatch)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, Viewport.transform);
            _text.GetComponent<Text>().text = match.id;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedResultTeam(_text);
            });
            trigger.triggers.Add(entry);
        }
    }

    private void DisplayTeamResultScrollViewContent(bool isDisplay)
    {

    }

}   
