using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;


public class EventController : MonoBehaviour
{
    Schedule todayEvent;
    public GameObject placeNameTextPrefab;

    private GameObject classScrollViewContent;
    private GameObject roundScrollView;
    private GameObject matchScrollView;
    private GameObject selectedClassObj;
    public GameObject roundScrollViewContentPrefab;
    public GameObject matchScrollViewContentPrefab;
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
            SetUiModule();
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

    private void SetUiModule()
    {
        classScrollViewContent = GameObject.Find("ClassScrollViewContent");
        roundScrollView = GameObject.Find("EventUICanvas").transform.Find("RoundScrollView").gameObject;
        matchScrollView = GameObject.Find("EventUICanvas").transform.Find("MatchScrollView").gameObject;
    }

    private void SetClassScrollViewContent()
    {
        // 団体戦、個人戦60kgなどのボタンを作成する
        if (this.taikai.ranking.school.Count != 0)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = "団体戦";
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundTeam(_text);
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
                SelectedRoundWeightClass(_text);
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
                SelectedRoundWeightClass(_text);
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
                SelectedRoundWeightClass(_text);
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
                SelectedRoundWeightClass(_text);
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
                SelectedRoundWeightClass(_text);
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
                SelectedRoundWeightClass(_text);
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
                SelectedRoundWeightClass(_text);
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

    private void SelectedRoundWeightClass(GameObject targetClassObj)
    {
        string _text = targetClassObj.GetComponent<Text>().text;
        this.selectedClassObj = targetClassObj;
        this.selectedMemberMatch = this.taikai.GetMemberMatch(this.ConvetToClassNum(_text));
        DisplayRoundScrollView(true);
        DisplayClassRoundScrollViewContent(true, this.ConvetToClassNum(_text));
    }

    private void SelectedRoundTeam(GameObject targetClassObj)
    {
        string _text = targetClassObj.GetComponent<Text>().text;
        this.selectedClassObj = targetClassObj;
        this.selectedSchoolMatch = this.taikai.allSchoolMatchResult;
        DisplayRoundScrollView(true);
        DisplayTeamRoundScrollViewContent(true);
    }

    private void DisplayRoundScrollView(bool isDisplay)
    {   
        roundScrollView.SetActive(isDisplay);
    }

    private void DisplayMatchScrollView(bool isDisplay)
    {   
        matchScrollView.SetActive(isDisplay);
    }

    private void DisplayClassRoundScrollViewContent(bool isDisplay, int weightClass)
    {
        GameObject Viewport = roundScrollView.transform.Find("Viewport").gameObject;
        GameObject roundScrollViewContent = Instantiate(roundScrollViewContentPrefab, Viewport.transform);
        roundScrollViewContent.name = weightClass.ToString();
        List<List<MemberMatch>> allRoundMemberMatchList = new List<List<MemberMatch>>();
        foreach (string matchIdPrefix in this.GetMatchRoundIdPrefixes(this.selectedMemberMatch))
        {
            GameObject _text = Instantiate(placeNameTextPrefab, roundScrollViewContent.transform);
            _text.GetComponent<Text>().text = GetMatchRoundLabel(matchIdPrefix);
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                DisplayClassMatchScrollViewContent(isDisplay, matchIdPrefix);
            });
            trigger.triggers.Add(entry);
        }
    }

    private void DisplayClassMatchScrollViewContent(bool isDisplay, string matchIdPrefix)
    {
        DisplayMatchScrollView(isDisplay);
        GameObject Viewport = matchScrollView.transform.Find("Viewport").gameObject;
        GameObject matchScrollViewContent = Instantiate(matchScrollViewContentPrefab, Viewport.transform);
        matchScrollViewContent.name = matchIdPrefix;
        List<MemberMatch> targetMatchList = new List<MemberMatch>();
        string pattern = "^" + matchIdPrefix;
        foreach (MemberMatch match in this.selectedMemberMatch)
        {
            if (Regex.IsMatch(match.id, pattern))
            {
                targetMatchList.Add(match);
            }
        }

        foreach (MemberMatch match in targetMatchList)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, matchScrollViewContent.transform);
            _text.GetComponent<Text>().text = match.id;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            // entry.callback.AddListener((eventDate) => {
            //     DisplayClassMatchScrollViewContent(isDisplay, matchIdPrefix);
            // });
            trigger.triggers.Add(entry);
        }
    }

    private void DisplayTeamRoundScrollViewContent(bool isDisplay)
    {

    }

    public string GetMatchRoundLabel(string matchIdPrefix)
    {
        string label = "";
        string roundStr = matchIdPrefix.Substring(14, 2);
        {
            switch (roundStr)
            {
                case "10":
                    label = "準々決勝";
                    break;
                case "11":
                    label = "準決勝";
                    break;
                case "12":
                    label = "決勝";
                    break;
                default:
                    label = roundStr + "回戦";
                    break;
            }
        }
        return label;
    }
    
    public List<string> GetMatchRoundIdPrefixes(List<MemberMatch> memberMatchList)
    {
        List<string> returnList = new List<string>();
        List<string> tmpList = new List<string>();
        foreach (MemberMatch match in memberMatchList)
        {
            tmpList.Add(match.id);
        }
        string checkIdPrefix = "";
        foreach (string matchId in tmpList.Distinct().ToList())
        {
            if (checkIdPrefix != matchId.Substring(0, 16))
            {
                checkIdPrefix = matchId.Substring(0, 16);
                returnList.Add(checkIdPrefix);
            }
        }
        returnList.Reverse();
        return returnList;
    }

}   
