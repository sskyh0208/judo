using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetsubiController : MonoBehaviour
{
    private GameObject moneyPanel;

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
        foreach (Setsubi setsubi in setsubiList)
        {
            GameObject obj = GameObject.Find("Setsubi" + setsubi.no);
            obj.transform.Find("Label").GetComponent<Text>().text = setsubi.name;
            obj.transform.Find("Value").GetComponent<Text>().text = setsubi.value.ToString();
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
            this.ChangeStatusBought(setsubi.no);
        }
    }

    public void BuySetsubi(string setsubiNo)
    {
        GameObject parent = GameObject.Find("Setsubi" + setsubiNo);
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
            this.ChangeStatusBought(setsubiNo);
        }
    }

    // 購入済み設備のボタンのグレーアウトおよび表示変更
    private　void ChangeStatusBought(string setsubiNo)
    {
        GameObject button = GameObject.Find("Setsubi" + setsubiNo).transform.Find("Button").gameObject;
        button.GetComponent<Button>().interactable = false;
        button.transform.Find("Text").GetComponent<Text>().text = "購入済み";
    }
}
