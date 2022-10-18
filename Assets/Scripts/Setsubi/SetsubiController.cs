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
        this.UpdateBuhiValue();
    }

    private void UpdateBuhiValue()
    {
        moneyPanel.transform.Find("Value").GetComponent<Text>().text = GameData.instance.GetPlayerSchool().money.ToString();
    }

    public void BuySetsubi(string setsubiNo)
    {
        GameObject parent = GameObject.Find("Setsubi" + setsubiNo);
        int value = Int32.Parse(parent.transform.Find("Value").GetComponent<Text>().text);
        Debug.Log("Setsubi" + setsubiNo + " : " + value);

        if (value > GameData.instance.GetPlayerSchool().money)
        {
            Debug.Log("購入できません。");
        }
        else
        {
            GameData.instance.GetPlayerSchool().money -= value;
            this.UpdateBuhiValue();
            parent.transform.Find("Button").GetComponent<Button>().interactable = false;
            parent.transform.Find("Button").transform.Find("Text").GetComponent<Text>().text = "購入済み";
        }
    }
}
