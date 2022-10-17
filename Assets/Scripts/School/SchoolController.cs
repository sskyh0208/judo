using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SchoolController : MonoBehaviour
{
    public GameObject placeNameTextPrefab;
    private GameObject selectedPlaceObj;
    private Place selectedPlace;
    private GameObject selectedSchoolObj;
    private Text SchoolNameText;
    private School selectedSchool;
    private GameObject schoolScrollView;
    public GameObject schoolScrollViewContentPrefab;

    public GameObject memberDisplayPanelPrefab;
    private GameObject membersScrollView;
    public GameObject membersScrollViewContentPrefab;
    public CanvasGroup canvasGroup;
    private float fadeTime = 1f;

    private GameObject schoolStatusPanel;
    private GameObject supervisorStatusPanel;

    private GameObject selectedMemberObj;
    private PlayerManager selectedMember;

    void Start()
    {
        schoolStatusPanel = GameObject.Find("SchoolUICanvas").transform.Find("SchoolStatusPanel").gameObject;
        supervisorStatusPanel = GameObject.Find("SchoolUICanvas").transform.Find("SupervisorStatusPanel").gameObject;
        schoolScrollView = GameObject.Find("SchoolUICanvas").transform.Find("SchoolScrollView").gameObject;
        membersScrollView = GameObject.Find("SchoolUICanvas2").transform.Find("MembersScrollView").gameObject;
        GeneratePlaceSelectButton();
        GenerateAllSchoolSelectButton(GameData.instance.placeManager.placeArray);
    }

    // 各県を選択するボタンを画面に作成する。
    private void GeneratePlaceSelectButton()
    {
        GameObject placeScrollViewContent = GameObject.Find("PlaceScrollViewContent");
        foreach (var name in GameData.instance.placeManager.GetAllPlaceName())
        {
            GameObject _text = Instantiate(placeNameTextPrefab, placeScrollViewContent.transform);
            _text.GetComponent<Text>().text = name;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedPlace(_text);
            });
            trigger.triggers.Add(entry);
        }
    }


    // 選択中の県を設定する
    private void SelectedPlace(GameObject targetPlaceObj)
    {
        if(selectedSchoolObj != null)
        {
            isNotSelectedText(selectedSchoolObj.GetComponent<Text>());
            selectedSchoolObj = null;
            selectedSchool = null;
            // Text SchoolNameText = GameObject.Find("SchoolNameText").GetComponent<Text>();
            // SchoolNameText.text = "";
        }
        if(selectedPlaceObj != null)
        {
            DisplaySchoolSelectScrollView(false);
            DisplaySchoolSelectScrollViewContent(false, selectedPlace.id);
            isNotSelectedText(selectedPlaceObj.GetComponent<Text>());
            selectedPlaceObj = null;
            selectedPlace = null;
        }
        isSelectedText(targetPlaceObj.GetComponent<Text>());
        selectedPlaceObj = targetPlaceObj;
        selectedPlace = GameData.instance.placeManager.getPlaceDataWithName(targetPlaceObj.GetComponent<Text>().text);
        DisplaySchoolSelectScrollView(true);
        DisplaySchoolSelectScrollViewContent(true, selectedPlace.id);
    }

    // 選択中の学校を設定する
    private void SelectedSchool(GameObject targetSchoolObj, School targetSchool)
    {
        if(!schoolStatusPanel.activeSelf)
        {
            schoolStatusPanel.SetActive(true);
            supervisorStatusPanel .SetActive(true);
        }
        if(selectedSchoolObj != null)
        {
            isNotSelectedText(selectedSchoolObj.GetComponent<Text>());
            selectedSchoolObj = null;
        }
        isSelectedText(targetSchoolObj.GetComponent<Text>());
        selectedSchoolObj = targetSchoolObj;
        selectedSchool = targetSchool;
        DisplaySupervisorInformation(targetSchool.supervisor);
        DisplaySchoolInformation(targetSchool);
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

    // 表示用ScrollViewを表示させる
    private void DisplaySchoolSelectScrollView(bool isDisplay)
    {   
        schoolScrollView.SetActive(isDisplay);
    }

    // 表示用ViewContentを表示させる
    private void DisplaySchoolSelectScrollViewContent(bool isDisplay, string placeId)
    {   
        GameObject schoolScrollViewContent = schoolScrollView.transform.Find("Viewport").Find(placeId).gameObject;
        if(isDisplay){
            schoolScrollView.GetComponent<ScrollRect>().content = schoolScrollViewContent.GetComponent<RectTransform>();
        }
        schoolScrollViewContent.SetActive(isDisplay);

    }

    // すべての学校ボタンを作る
    private void GenerateAllSchoolSelectButton(Place[] placeArray)
    {
        foreach(Place place in placeArray)
        {
            GenerateSchoolSelectButton(place);
        }
    }

    // 県内の学校を選択するボタンを画面に作成する。
    public void GenerateSchoolSelectButton(Place place)
    {   
        GameObject Viewport = schoolScrollView.transform.Find("Viewport").gameObject;
        GameObject schoolScrollViewContent = Instantiate(schoolScrollViewContentPrefab, Viewport.transform);
        schoolScrollViewContent.name = place.id;
        List<School> targetSchoolArray = GameData.instance.schoolManager.GetSamePlaceAllSchool(place.id);
        foreach (School school in targetSchoolArray)
        {
            GameObject _text = Instantiate(placeNameTextPrefab, schoolScrollViewContent.transform);
            _text.GetComponent<Text>().text = school.name;
            _text.AddComponent<EventTrigger>();
            EventTrigger trigger = _text.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventDate) => {
                SelectedSchool(_text, school);
            });
            trigger.triggers.Add(entry);
        }
        schoolScrollViewContent.SetActive(false);
    }

    private void isSelectedText(Text text)
    {   
        text.color = Color.red;
        text.fontStyle = FontStyle.Bold;
    }

    private void isNotSelectedText(Text text)
    {
        text.color = Color.white;
        text.fontStyle = FontStyle.Normal;
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

    private void DisplaySchoolInformation(School school)
    {
        // 学校名
        Text schoolNameText = schoolStatusPanel.transform.Find("SchoolNameText").GetComponent<Text>();
        schoolNameText.text = school.name;

        // 監督
        Text schoolSupervisorText = schoolStatusPanel.transform.Find("SchoolSupervisorText").GetComponent<Text>();
        schoolSupervisorText.text = school.supervisor.nameKaki;

        // 部員数
        Text schoolMembersNumText = schoolStatusPanel.transform.Find("SchoolMembersNumText").GetComponent<Text>();
        schoolMembersNumText.text = school.members.Count.ToString();

    }

    // 監督情報を設定する
    public void DisplaySupervisorInformation(PlayerManager supervisor)
    {
        Text playerName1Text = GameObject.Find("PlayerName1Text").GetComponent<Text>();
        Text playerName2Text = GameObject.Find("PlayerName2Text").GetComponent<Text>();
        Text playerBirthdayText = GameObject.Find("PlayerBirthdayText").GetComponent<Text>();
        Text playerHeightText = GameObject.Find("PlayerHeightText").GetComponent<Text>();
        Text playerWeightText = GameObject.Find("PlayerWeightText").GetComponent<Text>();
        Text playerPowerText = GameObject.Find("PlayerPowerText").GetComponent<Text>();
        Text playerSpeedText = GameObject.Find("PlayerSpeedText").GetComponent<Text>();
        Text playerStaminaText = GameObject.Find("PlayerStaminaText").GetComponent<Text>();
        Text playerWaza0Text = GameObject.Find("PlayerWaza0Text").GetComponent<Text>();
        Text playerWaza1Text = GameObject.Find("PlayerWaza1Text").GetComponent<Text>();
        Text playerTokuiwazaText = GameObject.Find("PlayerTokuiwazaText").GetComponent<Text>();

        playerName1Text.text = supervisor.nameKaki;
        playerName2Text.text = supervisor.nameYomi;
        playerBirthdayText.text = supervisor.GetBirthdayDisplayString();
        playerHeightText.text = supervisor.height.ToString();
        playerWeightText.text = supervisor.weight.ToString();
        playerPowerText.text = supervisor.powerString;
        playerSpeedText.text = supervisor.speedString;
        playerStaminaText.text = supervisor.staminaString;
        playerWaza0Text.text = supervisor.waza0String;
        playerWaza1Text.text = supervisor.waza1String;
        playerTokuiwazaText.text = supervisor.GetDisplayPlayerTokuiwaza();
    }

    public void ShowSchoolMembers()
    {
        GenerateSchoolMemberSelectButton(selectedSchool);
        DisplaySchoolMemberSelectScrollViewContent(isDisplay: true, selectedSchool.id);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, fadeTime)
            .OnComplete( () => {
                canvasGroup.blocksRaycasts = true;
            });
        
        Debug.Log(string.Format("名声: {0} {1}", selectedSchool.name, selectedSchool.fame));
    }

    public void CloseSchoolMembers()
    {
        DisplaySchoolMemberSelectScrollViewContent(false, selectedSchool.id);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(0, fadeTime)
            .OnComplete( () => {
                canvasGroup.blocksRaycasts = false;
            });
    }

    // 選択された学校の部員を選択するボタンを作成する
    public void GenerateSchoolMemberSelectButton(School school)
    {   
        GameObject Viewport = membersScrollView.transform.Find("Viewport").gameObject;
        GameObject membersScrollViewContent = Instantiate(membersScrollViewContentPrefab, Viewport.transform);
        membersScrollViewContent.name = school.id;
        int count = 0;
        foreach (PlayerManager member in school.GetSortDescMembers())
        {
            GameObject memberPanel = Instantiate(memberDisplayPanelPrefab, membersScrollViewContent.transform);
            memberPanel.transform.Find("NameKakiText").GetComponent<Text>().text = member.nameKaki;
            memberPanel.transform.Find("NameYomiText").GetComponent<Text>().text = member.nameYomi;
            memberPanel.transform.Find("GradeText").GetComponent<Text>().text = member.positionId.ToString();
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
        membersScrollViewContent.SetActive(false);
    }

    private void DisplaySchoolMemberSelectScrollViewContent(bool isDisplay, string placeId)
    {   
        GameObject membersScrollViewContent = membersScrollView.transform.Find("Viewport").Find(placeId).gameObject;
        if(isDisplay){
            membersScrollView.GetComponent<ScrollRect>().content = membersScrollViewContent.GetComponent<RectTransform>();
        }
        membersScrollViewContent.SetActive(isDisplay);
        membersScrollView.SetActive(isDisplay);

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
            if(waza.typeId == "0" || waza.typeId == "1")
            {
                string targetWazaPanelStr = string.Format("MemberWaza{0}Panel", waza.typeId);
                string targetWazaStr = string.Format("Waza ({0})", waza.id);
                GameObject targetWaza = GameObject.Find(targetWazaPanelStr).transform.Find(targetWazaStr).gameObject;
                targetWaza.transform.Find("WazaText").GetComponent<Text>().text = waza.displayString;
                targetWaza.transform.Find("WazaSlider").GetComponent<Slider>().value = waza.limit;
                targetWaza.transform.Find("WazaSlider").Find("Present Area").GetComponent<Slider>().value = waza.status;
            }
        }
    }
    
}
