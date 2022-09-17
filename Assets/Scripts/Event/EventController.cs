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
    public GameObject ipponIcon;
    public GameObject wazaariIcon;
    public GameObject yukoIcon;
    public GameObject placeNameTextPrefab;
    public GameObject resultTextPrefab;
    public GameObject resultTextLabelPrefab;
    public GameObject resultTeamTextPrefab;
    public GameObject resultRoundLabelPrefab;
    private GameObject classScrollViewContent;
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
        if(this.is_test){TestDataGenerate();}
        this.taikai = GameData.instance.todayJoinTournament;
        SetUiModule();
        SetClassScrollViewContent();

    }
    private void TestDataGenerate()
    {
        GameData.instance.LoadNewGameData();
        GameData.instance.todayEvent = GameData.instance.scheduleManager.GetSchedule(new DateTime(2022, 5, 1));
        GameData.instance.player = GameData.instance.schoolManager.GetSchool("073404087").supervisor;
    }

    private void SetUiModule()
    {
        classScrollViewContent = GameObject.Find("ClassScrollViewContent");
        matchScrollView = GameObject.Find("EventUICanvas").transform.Find("MatchScrollView").gameObject;
    }

    private void SetClassScrollViewContent()
    {
        // 団体戦、個人戦60kgなどのボタンを作成する
        if (this.taikai.ranking.school.Count != 0)
        {
            string weightClass = "団体戦";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.members60.Count != 0)
        {
            string weightClass = "60kg級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.members66.Count != 0)
        {
            string weightClass = "66kg級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.members73.Count != 0)
        {
            string weightClass = "73kg級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.members81.Count != 0)
        {
            string weightClass = "81kg級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.members90.Count != 0)
        {
            string weightClass = "90kg級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.members100.Count != 0)
        {
            string weightClass = "100kg級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
        if (this.taikai.ranking.membersOver100.Count != 0)
        {
            string weightClass = "100kg超級";
            GameObject _text = Instantiate(placeNameTextPrefab, classScrollViewContent.transform);
            _text.GetComponent<Text>().text = weightClass;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedRoundWeightClass(_text);
            });
            trigger.triggers.Add(entry);
            GenerateClassMatchScrollViewContent(weightClass);
        }
    }

    private int ConvetToClassNum(string weightClass)
    {
        int weightNum = 0;
        switch (weightClass)
        {
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
            default:
                break;
        }
        return weightNum;
    }

    private void SelectedRoundWeightClass(GameObject targetClassObj)
    {
        if(this.selectedClassObj != null)
        {
            DisplayMatchScrollView(false, this.selectedClassObj.GetComponent<Text>().text);
        }
        this.selectedClassObj = targetClassObj;
        DisplayMatchScrollView(true, targetClassObj.GetComponent<Text>().text);

    }

    private void DisplayMatchScrollView(bool isDisplay, string weightClass)
    {   
        matchScrollView.SetActive(isDisplay);
        GameObject matchScrollViewContent = matchScrollView.transform.Find("Viewport").Find(weightClass).gameObject;
        if(isDisplay){
            matchScrollView.GetComponent<ScrollRect>().content = matchScrollViewContent.GetComponent<RectTransform>();
        }
        matchScrollViewContent.SetActive(isDisplay);
    }

    private void GenerateClassMatchScrollViewContent(string weightClass)
    {
        GameObject Viewport = matchScrollView.transform.Find("Viewport").gameObject;
        GameObject matchScrollViewContent = Instantiate(matchScrollViewContentPrefab, Viewport.transform);
        matchScrollViewContent.name = weightClass;
        string matchRoundStr = "";
        if (weightClass == "団体戦")
        {
            matchScrollViewContent.GetComponent<VerticalLayoutGroup>().spacing = 40;
            foreach (SchoolMatch match in this.taikai.allSchoolMatchResult)
            {
                if (match.loser == null) {continue;}
                string roundStr = GetMatchRoundLabel(match.id);
                if (matchRoundStr != roundStr) {
                    matchRoundStr = roundStr;
                    GameObject roundLabel = Instantiate(resultRoundLabelPrefab, matchScrollViewContent.transform);
                    roundLabel.transform.Find("label").GetComponent<Text>().text = matchRoundStr;
                }
                GameObject resultTeamText = Instantiate(resultTeamTextPrefab, matchScrollViewContent.transform);
                GameObject detail = resultTeamText.transform.Find("detail").gameObject;
                detail.transform.Find("red").transform.Find("schoolName").GetComponent<Text>().text = match.red.name;
                detail.transform.Find("red").transform.Find("placeName").GetComponent<Text>().text = GameData.instance.placeManager.getPlaceDataWithId(match.red.placeId).name;
                detail.transform.Find("white").transform.Find("schoolName").GetComponent<Text>().text = match.white.name;
                detail.transform.Find("white").transform.Find("placeName").GetComponent<Text>().text = GameData.instance.placeManager.getPlaceDataWithId(match.white.placeId).name;
                string result = string.Format("{0} - {1}", match.redWinCount, match.whiteWinCount);
                detail.transform.Find("result").GetComponent<Text>().text = result;
                if (GameData.instance.player.schoolId == match.red.id)
                {detail.transform.Find("red").GetComponent<Image>().color = Color.red;}
                if (GameData.instance.player.schoolId == match.white.id)
                {detail.transform.Find("white").GetComponent<Image>().color = Color.red;}

                GameObject senpo = resultTeamText.transform.Find("senpo").gameObject;
                senpo.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.senpo.red.nameKaki;
                senpo.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.senpo.white.nameKaki;
                SetMatchResult(senpo.transform.Find("detail").gameObject, match.senpo);

                GameObject jiho = resultTeamText.transform.Find("jiho").gameObject;
                jiho.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.jiho.red.nameKaki;
                jiho.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.jiho.white.nameKaki;
                SetMatchResult(jiho.transform.Find("detail").gameObject, match.jiho);

                GameObject chuken = resultTeamText.transform.Find("chuken").gameObject;
                chuken.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.chuken.red.nameKaki;
                chuken.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.chuken.white.nameKaki;
                SetMatchResult(chuken.transform.Find("detail").gameObject, match.chuken);

                GameObject fukusho = resultTeamText.transform.Find("fukusho").gameObject;
                fukusho.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.fukusho.red.nameKaki;
                fukusho.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.fukusho.white.nameKaki;
                SetMatchResult(fukusho.transform.Find("detail").gameObject, match.fukusho);

                GameObject taisho = resultTeamText.transform.Find("taisho").gameObject;
                taisho.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.taisho.red.nameKaki;
                taisho.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.taisho.white.nameKaki;
                SetMatchResult(taisho.transform.Find("detail").gameObject, match.taisho);

                if (match.daihyo != null)
                {
                    GameObject daihyo = resultTeamText.transform.Find("daihyo").gameObject;
                    daihyo.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.daihyo.red.nameKaki;
                    daihyo.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.daihyo.white.nameKaki;
                    SetMatchResult(daihyo.transform.Find("detail").gameObject, match.daihyo);
                }
            }
            matchScrollViewContent.SetActive(false);

        }
        else
        {
            matchScrollViewContent.GetComponent<VerticalLayoutGroup>().spacing = 0;
            List<MemberMatch> dispList = this.taikai.GetMemberMatch(this.ConvetToClassNum(weightClass));
            foreach (MemberMatch match in dispList)
            {
                if (match.loser == null) {continue;}
                string roundStr = GetMatchRoundLabel(match.id);
                if (matchRoundStr != roundStr) {
                    matchRoundStr = roundStr;
                    GameObject roundLabel = Instantiate(resultRoundLabelPrefab, matchScrollViewContent.transform);
                    roundLabel.transform.Find("label").GetComponent<Text>().text = matchRoundStr;
                    GameObject resultLabel = Instantiate(resultTextLabelPrefab, matchScrollViewContent.transform);
                }
                GameObject detail = Instantiate(resultTextPrefab, matchScrollViewContent.transform);
                detail.transform.Find("red").transform.Find("redName").GetComponent<Text>().text = match.red.nameKaki;
                School redSchool = GameData.instance.schoolManager.GetSchool(match.red.schoolId);
                string redSchoolName = string.Format("{0}・{1}({2})", GameData.instance.placeManager.getPlaceDataWithId(redSchool.placeId).name, redSchool.name, match.red.positionId);
                detail.transform.Find("red").transform.Find("redSchoolName").GetComponent<Text>().text = redSchoolName;
                detail.transform.Find("white").transform.Find("whiteName").GetComponent<Text>().text = match.white.nameKaki;
                School whiteSchool = GameData.instance.schoolManager.GetSchool(match.white.schoolId);
                string whiteSchoolName = string.Format("{0}・{1}({2})", GameData.instance.placeManager.getPlaceDataWithId(whiteSchool.placeId).name, whiteSchool.name, match.white.positionId);
                detail.transform.Find("white").transform.Find("whiteSchoolName").GetComponent<Text>().text = whiteSchoolName;
                detail.transform.Find("detail").transform.Find("winWaza").GetComponent<Text>().text = match.winnerAbillity.name;
                detail.transform.Find("detail").transform.Find("time").GetComponent<Text>().text = match.GetTimeStr();
                if (GameData.instance.player.schoolId == redSchool.id)
                {detail.transform.Find("red").GetComponent<Image>().color = Color.red;}
                if (GameData.instance.player.schoolId == whiteSchool.id)
                {detail.transform.Find("white").GetComponent<Image>().color = Color.red;}
                // 赤の勝ち
                if (match.winnerFlag == 1)
                {
                    if (match.redIppon > 0)
                    {
                        GameObject ippon = Instantiate(ipponIcon, detail.transform.Find("detail").transform.Find(n: "redWin").transform);
                    }
                    else if (match.redWazaari == 1)
                    {
                        GameObject wazaari = Instantiate(wazaariIcon, detail.transform.Find("detail").transform.Find(n: "redWin").transform);
                    }
                    else if (match.redYuko > 0)
                    {
                        GameObject yuko = Instantiate(yukoIcon, detail.transform.Find("detail").transform.Find(n: "redWin").transform);
                    }
                }
                else if (match.winnerFlag == 2)
                {
                    if (match.whiteIppon > 0)
                    {
                        GameObject ippon = Instantiate(ipponIcon, detail.transform.Find("detail").transform.Find(n: "whiteWin").transform);
                    }
                    else if (match.whiteWazaari == 1)
                    {
                        GameObject wazaari = Instantiate(wazaariIcon, detail.transform.Find("detail").transform.Find(n: "whiteWin").transform);
                    }
                    else if (match.whiteYuko > 0)
                    {
                        GameObject yuko = Instantiate(yukoIcon, detail.transform.Find("detail").transform.Find(n: "whiteWin").transform);
                    }
                }
                matchScrollViewContent.SetActive(false);
            }
        }
    }

    private void SetMatchResult(GameObject detail, MemberMatch match)
    {
        detail.transform.Find("time").GetComponent<Text>().text = match.GetTimeStr();
        if (match.winnerFlag == 1)
        {
            detail.transform.Find("winWaza").GetComponent<Text>().text = match.winnerAbillity.name;
            if (match.redIppon > 0)
            {
                GameObject ippon = Instantiate(ipponIcon, detail.transform.Find(n: "redWin").transform);
            }
            else if (match.redWazaari == 1)
            {
                GameObject wazaari = Instantiate(wazaariIcon, detail.transform.Find(n: "redWin").transform);
            }
            else if (match.redYuko > 0)
            {
                GameObject yuko = Instantiate(yukoIcon, detail.transform.Find(n: "redWin").transform);
            }
        }
        else if (match.winnerFlag == 2)
        {
            detail.transform.Find("winWaza").GetComponent<Text>().text = match.winnerAbillity.name;
            if (match.whiteIppon > 0)
            {
                GameObject ippon = Instantiate(ipponIcon, detail.transform.Find(n: "whiteWin").transform);
            }
            else if (match.whiteWazaari == 1)
            {
                GameObject wazaari = Instantiate(wazaariIcon, detail.transform.Find(n: "whiteWin").transform);
            }
            else if (match.whiteYuko > 0)
            {
                GameObject yuko = Instantiate(yukoIcon, detail.transform.Find(n: "whiteWin").transform);
            }
        }
        else
        {
            detail.transform.Find("winWaza").GetComponent<Text>().text = "引き分け";
        }
    }

    private void DisplayTeamRoundScrollViewContent(bool isDisplay)
    {

    }

    public string GetMatchRoundLabel(string matchId)
    {
        string label = "";
        string roundStr = matchId.Substring(14, 2);
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
                    label = roundStr.TrimStart(new char[]{'0'}) + "回戦";
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
