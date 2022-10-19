using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetsubiController : MonoBehaviour
{

    [SerializeField] private  EventSystem eventSystem;
    private GameObject moneyPanel;

    public GameObject setsubiPrefab;

    void Start()
    {
        moneyPanel = GameObject.Find("MoneyPanel");
        this.SetSetsubiDisplay();
        this.UpdateBuhiValue();
        this.SetDisplayBuySetsubi();
    }

    private void SetSetsubiDisplay()
    {
        List<Setsubi> setsubiList = GameData.instance.setsubiManager.GetAllSetsubi();
        GameObject content = GameObject.Find("Viewport").transform.Find("Content").gameObject;
        foreach (Setsubi setsubi in setsubiList)
        {
            GameObject setsubiObj = Instantiate(setsubiPrefab, content.transform);
            setsubiObj.name = "Setsubi" + setsubi.no;
            GameObject button = setsubiObj.transform.Find("Button").gameObject;
            button.GetComponent<Button>().onClick.AddListener(this.BuySetsubi);
            setsubiObj.gameObject.AddComponent<EventTrigger>();
            EventTrigger trigger = setsubiObj.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => {Debug.Log(setsubiObj.name);});
            trigger.triggers.Add(entry);
            setsubiObj.transform.Find("Label").GetComponent<Text>().text = setsubi.name;
            setsubiObj.transform.Find("Value").GetComponent<Text>().text = setsubi.value.ToString();
        }
    }

    private void UpdateBuhiValue()
    {
        moneyPanel.transform.Find("Value").GetComponent<Text>().text = GameData.instance.GetPlayerSchool().money.ToString();
    }

    private void SetDisplayBuySetsubi()
    {
        foreach (Setsubi setsubi in GameData.instance.GetPlayerSchool().setsubiList)
        {
            // 購入済みの設備の設定
            this.ChangeStatusBought(GameObject.Find("Setsubi" + setsubi.no).transform.Find("Button").gameObject);
        }
    }

    public void BuySetsubi()
    {
        GameObject button = eventSystem.currentSelectedGameObject.gameObject;
        Debug.Log(button.name);
        GameObject parent = button.transform.parent.gameObject;
        string setsubiNo = parent.name.Replace("Setsubi", "");
        int value = Int32.Parse(parent.transform.Find("Value").GetComponent<Text>().text);

        if (value > GameData.instance.GetPlayerSchool().money)
        {
            Debug.Log("購入できません。");
        }
        else
        {
            GameData.instance.GetPlayerSchool().money -= value;
            GameData.instance.GetPlayerSchool().setsubiList.Add(GameData.instance.setsubiManager.GetSetsubi(setsubiNo));
            this.UpdateBuhiValue();
            this.ChangeStatusBought(button);
        }
    }

    // 購入済み設備のボタンのグレーアウトおよび表示変更
    private　void ChangeStatusBought(GameObject button)
    {
        button.GetComponent<Button>().interactable = false;
        button.transform.Find("Text").GetComponent<Text>().text = "購入済み";
    }
}
