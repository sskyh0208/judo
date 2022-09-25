using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class RenshuController : MonoBehaviour
{
    public GameObject membersScrollViewContentPrefab;
    public GameObject memberDisplayPanelPrefab;

    private GameObject selectedMemberObj;
    private PlayerManager selectedMember;
    private GameObject trainingMenuPanel;
    private GameObject trainingMenuParentRight;
    private GameObject trainingMenuParentLeft;
    private bool is_test = true;
    School targetSchool;

    private void Start() {

        // テスト用
        if(this.is_test){TestDataGenerate();}

        trainingMenuPanel = GameObject.Find("TrainingMenuPanel");
        targetSchool = GameData.instance.GetPlayerSchool();
        trainingMenuParentLeft = GameObject.Find("LeftTraining");
        trainingMenuParentRight = GameObject.Find("RightTraining");

        SetMySchoolMemberSelectButton();
    }

    private void TestDataGenerate()
    {
        GameData.instance.LoadNewGameData();
        GameData.instance.todayEvent = GameData.instance.scheduleManager.GetSchedule(new DateTime(2022, 5, 1));
        GameData.instance.player = GameData.instance.schoolManager.GetSchool("073404087").supervisor;
    }

    private void SetMySchoolMemberSelectButton()
    {   
        GameObject membersScrollView = GameObject.Find("MembersScrollView");
        GameObject membersScrollViewContent = membersScrollView.transform.Find("Viewport").transform.Find("MemberScrollViewContent").gameObject;
        membersScrollView.GetComponent<ScrollRect>().content = membersScrollViewContent.GetComponent<RectTransform>();
        int count = 0;
        foreach (PlayerManager member in targetSchool.GetSortDescMembers())
        {
            GameObject memberPanel = Instantiate(memberDisplayPanelPrefab, membersScrollViewContent.transform);
            memberPanel.name = member.id;
            memberPanel.transform.Find("NameKakiText").GetComponent<Text>().text = member.nameKaki;
            memberPanel.transform.Find("NameYomiText").GetComponent<Text>().text = member.nameYomi;
            memberPanel.transform.Find("GradeText").GetComponent<Text>().text = member.positionId.ToString() + "年生";
            memberPanel.transform.Find("HeightText").GetComponent<Text>().text = string.Format("{0}cm", member.height);
            memberPanel.transform.Find("WeightText").GetComponent<Text>().text = string.Format("{0}kg", member.weight);
            memberPanel.transform.Find("ClassText").GetComponent<Text>().text = member.GetWeightClass();
            memberPanel.AddComponent<EventTrigger>();
            EventTrigger trigger = memberPanel.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedMember(memberPanel, member);
            });
            trigger.triggers.Add(entry);
            if(count == 0) {SelectedMember(memberPanel, member);}
            count ++;
        }

        membersScrollView.GetComponent<ScrollRect>().content = membersScrollViewContent.GetComponent<RectTransform>();
    }

    // 選択中の部員を設定する
    private void SelectedMember(GameObject targetMemberObj, PlayerManager targetMember)
    {
        if(selectedMemberObj != null)
        {
            isNotSelectedMemberText(selectedMemberObj);
            selectedMemberObj = null;
        }
        isSelectedMemberText(targetMemberObj);
        selectedMemberObj = targetMemberObj;
        selectedMember = targetMember;
        ClearTrainingMenuInput();
        SetTrainingMenuInput();
        ViewSelectedMemberInformation(targetMember);
    }

    private void isSelectedMemberText(GameObject obj)
    {   
        obj.transform.Find("NameKakiText").GetComponent<Text>().color = Color.red;
        obj.transform.Find("NameKakiText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        obj.transform.Find("NameYomiText").GetComponent<Text>().color = Color.red;
        obj.transform.Find("NameYomiText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        obj.transform.Find("GradeText").GetComponent<Text>().color = Color.red;
        obj.transform.Find("GradeText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        obj.transform.Find("HeightText").GetComponent<Text>().color = Color.red;
        obj.transform.Find("HeightText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        obj.transform.Find("WeightText").GetComponent<Text>().color = Color.red;
        obj.transform.Find("WeightText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        obj.transform.Find("ClassText").GetComponent<Text>().color = Color.red;
        obj.transform.Find("ClassText").GetComponent<Text>().fontStyle = FontStyle.Bold;
    }

    private void isNotSelectedMemberText(GameObject obj)
    {

        obj.transform.Find("NameKakiText").GetComponent<Text>().color = Color.white;
        obj.transform.Find("NameKakiText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        obj.transform.Find("NameYomiText").GetComponent<Text>().color = Color.white;
        obj.transform.Find("NameYomiText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        obj.transform.Find("GradeText").GetComponent<Text>().color = Color.white;
        obj.transform.Find("GradeText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        obj.transform.Find("HeightText").GetComponent<Text>().color = Color.white;
        obj.transform.Find("HeightText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        obj.transform.Find("WeightText").GetComponent<Text>().color = Color.white;
        obj.transform.Find("WeightText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        obj.transform.Find("ClassText").GetComponent<Text>().color = Color.white;
        obj.transform.Find("ClassText").GetComponent<Text>().fontStyle = FontStyle.Normal;
    }

    private void ViewSelectedMemberInformation(PlayerManager member)
    {
        GameObject memberInforPanel = GameObject.Find("MemberInforPanel");
        memberInforPanel.transform.Find("MemberName1Text").GetComponent<Text>().text = member.nameKaki;
        memberInforPanel.transform.Find("MemberName2Text").GetComponent<Text>().text = member.nameYomi;
        memberInforPanel.transform.Find("MemberGradeText").GetComponent<Text>().text = member.positionId.ToString();
        memberInforPanel.transform.Find("MemberBirthdayText").GetComponent<Text>().text = member.GetBirthdayDisplayString();
        memberInforPanel.transform.Find("MemberHeightText").GetComponent<Text>().text = member.height.ToString() + "cm";
        memberInforPanel.transform.Find("MemberWeightText").GetComponent<Text>().text = member.weight.ToString() + "kg";
        memberInforPanel.transform.Find("MemberWeightClassText").GetComponent<Text>().text = member.GetWeightClass();
        // パワー表示
        memberInforPanel.transform.Find("MemberPowerText").GetComponent<Text>().text = member.GetAbillity("900").displayString;
        memberInforPanel.transform.Find("MemberPowerSlider").GetComponent<Slider>().value = member.GetAbillity("900").limit;
        memberInforPanel.transform.Find("MemberPowerSlider").Find("Present Area").GetComponent<Slider>().value = member.GetAbillity("900").status;
        // スピード表示
        memberInforPanel.transform.Find("MemberSpeedText").GetComponent<Text>().text = member.GetAbillity("901").displayString;
        memberInforPanel.transform.Find("MemberSpeedSlider").GetComponent<Slider>().value = member.GetAbillity("901").limit;
        memberInforPanel.transform.Find("MemberSpeedSlider").Find("Present Area").GetComponent<Slider>().value = member.GetAbillity("901").status;
        // スタミナ表示
        memberInforPanel.transform.Find("MemberStaminaText").GetComponent<Text>().text = member.GetAbillity("902").displayString;
        memberInforPanel.transform.Find("MemberStaminaSlider").GetComponent<Slider>().value = member.GetAbillity("902").limit;
        memberInforPanel.transform.Find("MemberStaminaSlider").Find("Present Area").GetComponent<Slider>().value = member.GetAbillity("902").status;


        // 技表示
        foreach (Abillity waza in member.abillities)
        {
            GameObject memberStatusPanel = GameObject.Find("MemberStatusPanel");
            if(waza.groupId != "9")
            {
                string targetWazaPanelStr = string.Format("MemberWaza{0}Panel", waza.groupId);
                string targetWazaStr = string.Format("Waza ({0})", waza.id);
                GameObject targetWaza = memberStatusPanel.transform.Find(targetWazaPanelStr).transform.Find(targetWazaStr).gameObject;
                targetWaza.transform.Find("WazaText").GetComponent<Text>().text = waza.displayString;
                targetWaza.transform.Find("WazaSlider").GetComponent<Slider>().value = waza.limit;
                targetWaza.transform.Find("WazaSlider").Find("Present Area").GetComponent<Slider>().value = waza.status;
            }
        }
    }

    public void DiplayMemberTachiWaza1Panel()
    {
        GameObject memberStatusPanel = GameObject.Find("MemberStatusPanel");
        memberStatusPanel.transform.Find("MemberWaza3Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza4Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza5Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza6Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza7Panel").gameObject.SetActive(false);

        memberStatusPanel.transform.Find("MemberWaza0Panel").gameObject.SetActive(true);
        memberStatusPanel.transform.Find("MemberWaza1Panel").gameObject.SetActive(true);
        memberStatusPanel.transform.Find("MemberWaza2Panel").gameObject.SetActive(true);
    }
    public void DiplayMemberTachiWaza2Panel()
    {
        GameObject memberStatusPanel = GameObject.Find("MemberStatusPanel");
        memberStatusPanel.transform.Find("MemberWaza0Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza1Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza2Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza5Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza6Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza7Panel").gameObject.SetActive(false);

        memberStatusPanel.transform.Find("MemberWaza3Panel").gameObject.SetActive(true);
        memberStatusPanel.transform.Find("MemberWaza4Panel").gameObject.SetActive(true);
    }
    public void DiplayMemberTachiWaza3Panel()
    {
        GameObject memberStatusPanel = GameObject.Find("MemberStatusPanel");
        memberStatusPanel.transform.Find("MemberWaza0Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza1Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza2Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza3Panel").gameObject.SetActive(false);
        memberStatusPanel.transform.Find("MemberWaza4Panel").gameObject.SetActive(false);

        memberStatusPanel.transform.Find("MemberWaza5Panel").gameObject.SetActive(true);
        memberStatusPanel.transform.Find("MemberWaza6Panel").gameObject.SetActive(true);
        memberStatusPanel.transform.Find("MemberWaza7Panel").gameObject.SetActive(true);
    }

    private List<Tuple<string, int>> GetTrainingMenuInput()
    {
        List<Tuple<string, int>> trainingMenuTuple = new List<Tuple<string, int>>();
        int totalTrainingMinutes = 0;
        for (int i = 0; i < trainingMenuParentLeft.transform.childCount; i++)
        {   
            GameObject child = trainingMenuParentLeft.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith("Label"))
            {
                if (child.transform.Find("Input").GetComponent<InputField>().text == "")
                {child.transform.Find("Input").GetComponent<InputField>().text = "0";}
                int trainingMinutes = Int32.Parse(child.transform.Find("Input").GetComponent<InputField>().text);
                if(trainingMinutes == 0){continue;}
                string trainingName = child.name;
                trainingMenuTuple.Add(new Tuple<string, int>(trainingName, trainingMinutes));
                totalTrainingMinutes += trainingMinutes;
            }
        }

        for (int i = 0; i < trainingMenuParentRight.transform.childCount; i++)
        {   
            GameObject child = trainingMenuParentRight.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith("Label"))
            {
                if (child.transform.Find("Input").GetComponent<InputField>().text == "")
                {child.transform.Find("Input").GetComponent<InputField>().text = "0";}
                int trainingMinutes = Int32.Parse(child.transform.Find("Input").GetComponent<InputField>().text);
                if(trainingMinutes == 0){continue;}
                string trainingName = child.name;
                trainingMenuTuple.Add(new Tuple<string, int>(trainingName, trainingMinutes));
                totalTrainingMinutes += trainingMinutes;
            }
        }

        if(trainingMenuTuple.Count == 0)
        {
            // 設定されていないと警告を出す
            Debug.Log("練習が何も設定されていません。");
        }
        else if(targetSchool.CheckTrainingLimitMinutes(totalTrainingMinutes))
        {
            // 超過していると警告文を出す
            Debug.Log(string.Format("練習時間を超過しています。　上限: {0}  設定値: {1}", targetSchool.trainingLimitMinutes, totalTrainingMinutes));
        }
        return trainingMenuTuple;
    }

    public void SetSoloTrainingMenu()
    {
        // 練習内容の初期化
        selectedMember.ClearTrainingMenu();
        selectedMember.trainingMenu = GetTrainingMenuInput();
    }

    public void SetAllTrainingMenu()
    {
        List<Tuple<string, int>> trainingMenuTuple = GetTrainingMenuInput();
        foreach (PlayerManager member in targetSchool.GetSortDescMembers())
        {
            // 練習内容の初期化
            member.ClearTrainingMenu();
            member.trainingMenu = trainingMenuTuple;
        }
    }

    private void SetTrainingMenuInput()
    {
        foreach (Tuple<string, int> trainingMenu in selectedMember.trainingMenu)
        {
            InputField setField;
            setField = GameObject.Find(trainingMenu.Item1).transform.Find("Input").GetComponent<InputField>();
            setField.text = trainingMenu.Item2.ToString();
        }
    }

    private void ClearTrainingMenuInput()
    {
        for (int i = 0; i < trainingMenuParentLeft.transform.childCount; i++)
        {   
            GameObject child = trainingMenuParentLeft.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith("Label"))
            {
                child.transform.Find("Input").GetComponent<InputField>().text = "0";
            }
        }

        for (int i = 0; i < trainingMenuParentRight.transform.childCount; i++)
        {   
            GameObject child = trainingMenuParentRight.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith("Label"))
            {
                child.transform.Find("Input").GetComponent<InputField>().text = "0";
            }
        }
    }
}
