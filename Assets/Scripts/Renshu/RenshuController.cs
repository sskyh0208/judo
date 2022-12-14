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
    private GameObject trainingMenuList;
    private bool is_test = false;
    School targetSchool;

    private int displayTrainingMenuTabNum;
    private int selectTrainingBarNum;
    private GameObject selectedOutline;

    private void Start() {

        // ใในใ็จ
        if(this.is_test){TestDataGenerate();}

        targetSchool = GameData.instance.GetPlayerSchool();
        GameObject renshuUICanvas = GameObject.Find("RenshuUICanvas");
        trainingMenuPanel = renshuUICanvas.transform.Find("TrainingMenuPanel").gameObject;
        trainingMenuList = trainingMenuPanel.transform.Find("TrainingMenuInnerPanel").transform.Find("TrainingMenuList").gameObject;

        SetMySchoolMemberSelectButton();
        SetTrainingBar();
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
            memberPanel.transform.Find("GradeText").GetComponent<Text>().text = member.positionId.ToString() + "ๅนด็";
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

    // ้ธๆไธญใฎ้จๅกใ่จญๅฎใใ
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
        // ClearTrainingMenuInput();
        // SetTrainingMenuInput();
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
        // ใใฏใผ่กจ็คบ
        memberInforPanel.transform.Find("MemberPowerText").GetComponent<Text>().text = member.GetAbillity("900").displayString;
        memberInforPanel.transform.Find("MemberPowerSlider").GetComponent<Slider>().value = member.GetAbillity("900").limit;
        memberInforPanel.transform.Find("MemberPowerSlider").Find("Present Area").GetComponent<Slider>().value = member.GetAbillity("900").status;
        // ในใใผใ่กจ็คบ
        memberInforPanel.transform.Find("MemberSpeedText").GetComponent<Text>().text = member.GetAbillity("901").displayString;
        memberInforPanel.transform.Find("MemberSpeedSlider").GetComponent<Slider>().value = member.GetAbillity("901").limit;
        memberInforPanel.transform.Find("MemberSpeedSlider").Find("Present Area").GetComponent<Slider>().value = member.GetAbillity("901").status;
        // ในใฟใใ่กจ็คบ
        memberInforPanel.transform.Find("MemberStaminaText").GetComponent<Text>().text = member.GetAbillity("902").displayString;
        memberInforPanel.transform.Find("MemberStaminaSlider").GetComponent<Slider>().value = member.GetAbillity("902").limit;
        memberInforPanel.transform.Find("MemberStaminaSlider").Find("Present Area").GetComponent<Slider>().value = member.GetAbillity("902").status;
        // ็ทด็ฟ่จญๅฎ
        if (member.trainingMenu.trainingMenuNum > 0)
        {
            memberInforPanel.transform.Find("MemberSelectTrainingText").GetComponent<Text>().text = "ใกใใฅใผ" + member.trainingMenu.trainingMenuNum;
        }
        else
        {
            memberInforPanel.transform.Find("MemberSelectTrainingText").GetComponent<Text>().text = "ใชใ";
        }


        // ๆ่กจ็คบ
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

    private Dictionary<string, int> GetTrainingMenuInput()
    {
        Dictionary<string, int> trainingMenu = new Dictionary<string, int>();
        int totalTrainingMinutes = 0;
        for (int i = 0; i < trainingMenuList.transform.childCount; i++)
        {   
            GameObject child = trainingMenuList.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith("Label"))
            {
                if (child.transform.Find("Input").GetComponent<InputField>().text == "")
                {child.transform.Find("Input").GetComponent<InputField>().text = "0";}
                int trainingMinutes = Int32.Parse(child.transform.Find("Input").GetComponent<InputField>().text);
                if(trainingMinutes == 0){continue;}
                trainingMenu[child.name] = trainingMinutes;
                totalTrainingMinutes += trainingMinutes;
            }
        }
        return trainingMenu;
    }
    public void SelectTrainingBar(int selectNum)
    {
        // ้ธๆไธญใฎใใฌใผใใณใฐใใผใ่จญๅฎใใ
        this.selectTrainingBarNum = selectNum;
    }

    public void SetSoloTrainingMenu()
    {
        // ็ทด็ฟๅๅฎนใฎๅๆๅ
        selectedMember.ClearTrainingMenu();
        selectedMember.trainingMenu = new TrainingMenu(this.selectTrainingBarNum, targetSchool.GetTrainingMenu(this.selectTrainingBarNum));
        this.ViewSelectedMemberInformation(selectedMember);
    }

    public void SetAllTrainingMenu()
    {
        Dictionary<string, int> trainingMenu = targetSchool.GetTrainingMenu(this.selectTrainingBarNum);
        foreach (PlayerManager member in targetSchool.GetSortDescMembers())
        {
            // ็ทด็ฟๅๅฎนใฎๅๆๅ
            member.ClearTrainingMenu();
            member.trainingMenu = new TrainingMenu(this.selectTrainingBarNum, trainingMenu);
        }
        this.ViewSelectedMemberInformation(selectedMember);
    }

    private void SetTrainingMenuInput(Dictionary<string, int> trainingMenu)
    {
        foreach (KeyValuePair<string, int> item in trainingMenu)
        {
            InputField setField;
            setField = trainingMenuList.transform.Find(item.Key).transform.Find("Input").GetComponent<InputField>();
            setField.text = item.Value.ToString();
        }
    }

    public void OpenTrainingMenu(int setMenuNum)
    {
        this.displayTrainingMenuTabNum = setMenuNum;
        Dictionary<string, int> setMenuDict = targetSchool.GetTrainingMenu(setMenuNum);
        trainingMenuPanel.SetActive(true);
        if (setMenuDict.Count != 0){SetTrainingMenuInput(setMenuDict);}
        else{ResetValueInput();}
    }

    public void CloseTrainingMenu()
    {
        trainingMenuPanel.SetActive(false);
        targetSchool.SetTrainingMenu(this.displayTrainingMenuTabNum, this.GetTrainingMenuInput());
        SelectTraining(this.displayTrainingMenuTabNum);
        SetTrainingBar();
    }

    private void SetDefaultValueInput()
    {
        // nullๅคใฎinputfieldใซ0ใๅฅใใ
        for (int i = 0; i < trainingMenuList.transform.childCount; i++)
        {   
            GameObject child = trainingMenuList.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith(value: "Label"))
            {
                if (child.transform.Find("Input").GetComponent<InputField>().text == "")
                {child.transform.Find("Input").GetComponent<InputField>().text = "0";}
            }
        }
    }

    public void ResetValueInput()
    {
        // nullๅคใฎinputfieldใซ0ใๅฅใใ
        for (int i = 0; i < trainingMenuList.transform.childCount; i++)
        {   
            GameObject child = trainingMenuList.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith(value: "Label"))
            {
                child.transform.Find("Input").GetComponent<InputField>().text = "0";
            }
        }
        GameObject.Find("LimitMinutes").GetComponent<Text>().text = "0";
    }

    private void SetDisplayLimitMinutes()
    {
        int displayLimitMinutes = 0;
        for (int i = 0; i < trainingMenuList.transform.childCount; i++)
        {   
            GameObject child = trainingMenuList.transform.GetChild(i).gameObject;
            if(! child.name.StartsWith(value: "Label"))
            {
                if (child.transform.Find("Input").GetComponent<InputField>().text == "")
                {
                    child.transform.Find("Input").GetComponent<InputField>().text = "0";
                }
                displayLimitMinutes += int.Parse(child.transform.Find("Input").GetComponent<InputField>().text);
            }
        }
        GameObject.Find("LimitMinutes").GetComponent<Text>().text = displayLimitMinutes.ToString();
    }

    public void SetAutoMenu(string type)
    {
        // ็ทด็ฟใกใใฅใผใ่ชๅ่จญๅฎใใ
        ResetValueInput();
        int trainingLimitMinutes = targetSchool.trainingLimitMinutes;
        foreach (KeyValuePair<string, int> item in GameData.instance.trainingManager.GetTrainingMenuTemplateDictionary(trainingLimitMinutes, type))
        {
            trainingMenuList.transform.Find(item.Key).transform.Find("Input").GetComponent<InputField>().text = item.Value.ToString();
            trainingLimitMinutes -= item.Value;
        }
        SetDisplayLimitMinutes();
    }

    private void GenerateTrainingBarParts(GameObject trainingBar, Dictionary<string, int> targetTrainingMenu)
    {
        // ๅญ่ฆ็ด?ๅ้ค
        foreach (Transform child in trainingBar.transform)
        {
            Destroy(child.gameObject);
        }

        if (targetTrainingMenu.Count > 0)
        {
            foreach (KeyValuePair<string, int> training in targetTrainingMenu)
            {
                GameObject go = new GameObject(training.Key);
                go.transform.parent = trainingBar.transform;
                Image image = go.AddComponent<Image>();
                image.color = GetColor(GameData.instance.trainingManager.GetTraining(training.Key).colorCode);
                RectTransform rect = go.GetComponent<RectTransform>();

                float wariai = trainingBar.GetComponent<RectTransform>().sizeDelta.x / targetSchool.trainingLimitMinutes;
                float size = (int)(training.Value * wariai);

                rect.sizeDelta = new Vector2(size, trainingBar.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        else
        {
            GameObject go = new GameObject("Empty");
            go.transform.parent = trainingBar.transform;
            Image image = go.AddComponent<Image>();
            image.color = Color.gray;
            RectTransform rect = go.GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(trainingBar.GetComponent<RectTransform>().sizeDelta.x, trainingBar.GetComponent<RectTransform>().sizeDelta.y);
        }
        // return new Tuple<RectTransform, Image>(rect,i mage);
    }

    private void SetTrainingBar()
    {
        for (int i = 0; i <5; i++)
        {
            GameObject targetTrainingBar = null;
            Dictionary<string, int> targetTrainingMenu = new Dictionary<string, int>();
            switch (i)
            {
                default:
                case 0:
                    targetTrainingBar = GameObject.Find("TrainingMenuBar1").transform.Find("DisplayBartLine").gameObject;
                    targetTrainingMenu = targetSchool.trainingMenu1;
                    break;
                case 1:
                    targetTrainingBar = GameObject.Find("TrainingMenuBar2").transform.Find("DisplayBartLine").gameObject;
                    targetTrainingMenu = targetSchool.trainingMenu2;
                    break;
                case 2:
                    targetTrainingBar = GameObject.Find("TrainingMenuBar3").transform.Find("DisplayBartLine").gameObject;
                    targetTrainingMenu = targetSchool.trainingMenu3;
                    break;
                case 3:
                    targetTrainingBar = GameObject.Find("TrainingMenuBar4").transform.Find("DisplayBartLine").gameObject;
                    targetTrainingMenu = targetSchool.trainingMenu4;
                    break;
                case 4:
                    targetTrainingBar = GameObject.Find("TrainingMenuBar5").transform.Find("DisplayBartLine").gameObject;
                    targetTrainingMenu = targetSchool.trainingMenu5;
                    break;
            }
            this.GenerateTrainingBarParts(targetTrainingBar, targetTrainingMenu);
        }
    }

    private Color GetColor(string colorCode)
    {
        Color color = default(Color);
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            return color;
        }
        else
        {
            return new Color32(126, 126, b: 126, 126);
        }
    }

    public void SelectTraining(int trainingNum)
    {
        if (this.selectedOutline)
        {
            this.selectedOutline.SetActive(false);
        }
        if (targetSchool.GetTrainingMenu(trainingNum).Count > 0)
        {
            this.selectedOutline = GameObject.Find("TrainingMenuBar" + trainingNum).transform.Find("SelectedOutLine").gameObject;
            this.selectedOutline.SetActive(true);
            this.selectTrainingBarNum = trainingNum;
        }
    }
}
