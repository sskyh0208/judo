using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewGameController : MonoBehaviour
{
    public GameObject textPrefab;
    private GameObject selectedPlaceObj;
    private Place selectedPlace;
    private GameObject selectedSchoolObj;
    private Text SchoolNameText;
    private School selectedSchool;
    private GameObject schoolScrollView;
    public GameObject schoolScrollViewContentPrefab;
    private InputField nameInputField;

    void Start()
    {
        nameInputField = GameObject.Find("NameInputField").GetComponent<InputField>();
        schoolScrollView = GameObject.Find("NewGameUICanvas").transform.Find("SchoolScrollView").gameObject;
        GameData.instance.LoadNewGameData();
        GeneratePlaceSelectButton();
        GenerateAllSchoolSelectButton(GameData.instance.placeManager.placeArray);
        GameData.instance.player = GenerateNewPlayer();
        SetDisplayPlayerStatus();
        Debug.Log(10 + Int16.Parse("01"));
    }

    // 各県を選択するボタンを画面に作成する。
    private void GeneratePlaceSelectButton()
    {
        GameObject placeScrollViewContent = GameObject.Find("PlaceScrollViewContent");
        foreach (var name in GameData.instance.placeManager.GetAllPlaceName())
        {
            GameObject _text = Instantiate(textPrefab, placeScrollViewContent.transform);
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
            selectedSchoolObj.GetComponent<Text>().color = Color.white;
            selectedSchoolObj.GetComponent<Text>().fontStyle = FontStyle.Normal;
            selectedSchoolObj = null;
            selectedSchool = null;
            Text SchoolNameText = GameObject.Find("SchoolNameText").GetComponent<Text>();
            SchoolNameText.text = "";
        }
        if(selectedPlaceObj != null)
        {
            DisplaySchoolSelectScrollView(false);
            DisplaySchoolSelectScrollViewContent(false, selectedPlace.id);
            selectedPlaceObj.GetComponent<Text>().color = Color.white;
            selectedPlaceObj.GetComponent<Text>().fontStyle = FontStyle.Normal;
            selectedPlaceObj = null;
            selectedPlace = null;
        }
        targetPlaceObj.GetComponent<Text>().color = Color.red;
        targetPlaceObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
        selectedPlaceObj = targetPlaceObj;
        selectedPlace = GameData.instance.placeManager.getPlaceDataWithName(targetPlaceObj.GetComponent<Text>().text);
        DisplaySchoolSelectScrollView(true);
        DisplaySchoolSelectScrollViewContent(true, selectedPlace.id);
    }

    // 選択中の学校を設定する
    private void SelectedSchool(GameObject targetSchoolObj, School targetSchool)
    {
        Text SchoolNameText = GameObject.Find("SchoolNameText").GetComponent<Text>();
        if(selectedSchoolObj != null)
        {
            selectedSchoolObj.GetComponent<Text>().color = Color.white;
            selectedSchoolObj.GetComponent<Text>().fontStyle = FontStyle.Normal;
            selectedSchoolObj = null;
        }
        targetSchoolObj.GetComponent<Text>().color = Color.red;
        targetSchoolObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
        selectedSchoolObj = targetSchoolObj;
        selectedSchool = targetSchool;
        SchoolNameText.text = targetSchool.name;
    }

    private void DisplaySchoolSelectScrollView(bool isDisplay)
    {   
        schoolScrollView.SetActive(isDisplay);
    }

    private void DisplaySchoolSelectScrollViewContent(bool isDisplay, string placeId)
    {   
        GameObject schoolScrollViewContent = schoolScrollView.transform.Find("Viewport").Find(placeId).gameObject;
        if(isDisplay){
            schoolScrollView.GetComponent<ScrollRect>().content = schoolScrollViewContent.GetComponent<RectTransform>();
        }
        schoolScrollViewContent.SetActive(isDisplay);

    }

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
        List<School> targetSchoolArray = GameData.instance.schoolManager.GetSamePlaceAllSchool("00"+place.id+"00");
        foreach (School school in targetSchoolArray)
        {
            GameObject _text = Instantiate(textPrefab, schoolScrollViewContent.transform);
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

    // 作成したボタンを画面から削除する。
    public void DeleteGeneratedSelectButton()
    {
    } 

    // 監督のステータスをランダムに作成する。
    public void GenerateRandomStatusCoach()
    {

    }

    // 全学校のステータスを作成する。
    private void GenerateStatusAllSchool()
    {

    }

    // 学校のステータスをランダムに作成する。
    private School GenerateRandomStatusSchool(School school)
    {
        return school;
    }

    public PlayerManager　GenerateNewPlayer()
    {
        string defaultPlayerNameKaki = "山田太郎";
        nameInputField.text = defaultPlayerNameKaki;
        int playerSense = new System.Random().Next(7, 7);
        PlayerManager player = new PlayerManager("0", defaultPlayerNameKaki, "", new DateTime(1988, 2, 8), positionId: 101, "0", "0", playerSense);
        return player;
    }

    // プレイヤーを設定する
    public void SetPlayer()
    {
        string playerId = string.Format("{0}{1}{2}", GameData.instance.storyDate.Year, selectedSchool.id, "0");
        GameData.instance.player.id = playerId;
        GameData.instance.player.nameKaki = nameInputField.text;
        GameData.instance.player.nameKaki = nameInputField.text;
        GameData.instance.player.schoolId = selectedSchool.id;

        GameData.instance.schoolManager.SetSuperVoisor(selectedSchool.id, GameData.instance.player);

        GameObject SceneController = GameObject.Find("SceneController");
        SceneController.GetComponent<SceneTransitionController>().LoadTo("Main");
    }

    private void SetDisplayPlayerStatus()
    {
        Text playerHeightText = GameObject.Find("PlayerHeightText").GetComponent<Text>();
        Text playerWeightText = GameObject.Find("PlayerWeightText").GetComponent<Text>();
        Text playerPowerText = GameObject.Find("PlayerPowerText").GetComponent<Text>();
        Text playerSpeedText = GameObject.Find("PlayerSpeedText").GetComponent<Text>();
        Text playerStaminaText = GameObject.Find("PlayerStaminaText").GetComponent<Text>();
        Text playerWaza0Text = GameObject.Find("PlayerWaza0Text").GetComponent<Text>();
        Text playerWaza1Text = GameObject.Find("PlayerWaza1Text").GetComponent<Text>();
        Text playerTokuiwazaText = GameObject.Find("PlayerTokuiwazaText").GetComponent<Text>();

        playerHeightText.text = GameData.instance.player.height.ToString();
        playerWeightText.text = GameData.instance.player.weight.ToString();
        playerPowerText.text = GameData.instance.player.powerString;
        playerSpeedText.text = GameData.instance.player.speedString;
        playerStaminaText.text = GameData.instance.player.staminaString;
        playerWaza0Text.text = GameData.instance.player.waza0String;
        playerWaza1Text.text = GameData.instance.player.waza1String;
        playerTokuiwazaText.text = GameData.instance.player.GetDisplayPlayerTokuiwaza();
    }
}
