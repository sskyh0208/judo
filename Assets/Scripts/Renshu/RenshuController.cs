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
    private GameObject trainingMenuParent;
    private bool is_test = true;
    School targetSchool;

    private void Start() {

        // テスト用
        if(this.is_test){TestDataGenerate();}

        trainingMenuPanel = GameObject.Find("TrainingMenuPanel");
        targetSchool = GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId);
        trainingMenuParent = GameObject.Find("TrainingMenuParent");

        if(targetSchool.trainingMenu.Count > 0)
        {
            SetTrainingMenuInput();
        }

        SetMySchoolMemberSelectButton();
    }

    private void TestDataGenerate()
    {
        GameData.instance.LoadNewGameData();
        GameData.instance.todayEvent = GameData.instance.scheduleManager.GetSchedule(new DateTime(2022, 5, 1));
        GameData.instance.player = GameData.instance.schoolManager.GetSchool("073404087").supervisor;
    }

    public void SetTrainingMenu()
    {
        List<Tuple<RectTransform, Image, int>> barTrainingList = new List<Tuple<RectTransform, Image, int>>();
        List<Tuple<string, int>> trainingMenuTuple = new List<Tuple<string, int>>();
        int totalTrainingMinutes = 0;
        foreach (Transform child in trainingMenuParent.transform)
        {   
            if (child.transform.Find("Input").GetComponent<InputField>().text == "")
            {child.transform.Find("Input").GetComponent<InputField>().text = "0";}
            int trainingMinutes = Int32.Parse(child.transform.Find("Input").GetComponent<InputField>().text);
            if(trainingMinutes == 0){continue;}
            string trainingName = child.name;
            trainingMenuTuple.Add(new Tuple<string, int>(trainingName, trainingMinutes));
            totalTrainingMinutes += trainingMinutes;
        }

        ClearTrainingMenuPanel();
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
        else{
            // 練習内容の初期化
            targetSchool.ClearTrainingMenu();
            foreach(Tuple<string, int> trainingMenu in trainingMenuTuple)
            {
                // 表示用ゲージの作成
                GameObject trainingBar = new GameObject(trainingMenu.Item1);
                trainingBar.transform.parent = trainingMenuPanel.transform;
                Image image = trainingBar.AddComponent<Image>();
                Outline outline = trainingBar.AddComponent<Outline>();
                outline.effectDistance = new Vector2(5,5);
                RectTransform rect = trainingBar.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(0, 40);
                image.color = Color.gray;
                rect.DOSizeDelta(new Vector3(trainingMenu.Item2, 40), 0.5f);

                // 練習効果の作成
                targetSchool.SetTrainingMenu(trainingMenu.Item1, trainingMenu.Item2);
            }
        }

    }
    private void ClearTrainingMenuPanel()
    {
        // パネル内の表示をすべて削除
        foreach (Transform child in trainingMenuPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetTrainingMenuInput()
    {
        foreach (Tuple<string, int> trainingMenu in targetSchool.trainingMenu)
        {
            InputField setField;
            setField = trainingMenuParent.transform.Find(trainingMenu.Item1).transform.Find("Input").GetComponent<InputField>();
            setField.text = trainingMenu.Item2.ToString();
        }
    }

    private void SetMySchoolMemberSelectButton()
    {   
        School school = GameData.instance.GetPlayerSchool();
        GameObject membersScrollView = GameObject.Find("MembersScrollView");
        GameObject Viewport = membersScrollView.transform.Find("Viewport").gameObject;
        GameObject membersScrollViewContent = Instantiate(membersScrollViewContentPrefab, Viewport.transform);
        membersScrollViewContent.name = school.name;
        membersScrollView.GetComponent<ScrollRect>().content = membersScrollViewContent.GetComponent<RectTransform>();
        int count = 0;
        foreach (PlayerManager member in school.GetSortDescMembers())
        {
            GameObject memberPanel = Instantiate(memberDisplayPanelPrefab, membersScrollViewContent.transform);
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
}
