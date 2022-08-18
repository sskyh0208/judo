using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RenshuController : MonoBehaviour
{

    private GameObject trainingMenuPanel;
    private GameObject trainingMenuParent;
    School targetSchool;
    private void Start() {
        trainingMenuPanel = GameObject.Find("TrainingMenuPanel");
        targetSchool = GameData.instance.schoolManager.GetSchool(GameData.instance.player.schoolId);
        trainingMenuParent = GameObject.Find("TrainingMenuParent");

        if(targetSchool.trainingMenu.Count > 0)
        {
            SetTrainingMenuInput();
        }
    }
    public void SetTrainingMenu()
    {
        List<Tuple<RectTransform, Image, int>> barTrainingList = new List<Tuple<RectTransform, Image, int>>();
        List<Tuple<string, int>> trainingMenuTuple = new List<Tuple<string, int>>();
        int totalTrainingMinutes = 0;
        foreach (Transform child in trainingMenuParent.transform)
        {   
            int trainingMinutes = Int32.Parse(child.transform.Find("Input").GetComponent<InputField>().text);
            if(trainingMinutes == 0){continue;}
            string trainingName = child.transform.Find("Label").GetComponent<Text>().text;
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
            switch (trainingMenu.Item1)
            {
                default:
                case "ランニング":
                    setField = trainingMenuParent.transform.Find("Running").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "ダッシュ":
                    setField = trainingMenuParent.transform.Find("Dash").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "階段ダッシュ":
                    setField = trainingMenuParent.transform.Find("KaidanDash").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "自重トレ":
                    setField = trainingMenuParent.transform.Find("SelfWeight").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "マシントレ":
                    setField = trainingMenuParent.transform.Find("MachineWeight").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(手)":
                    setField = trainingMenuParent.transform.Find("UchikomiTe").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(腰)":
                    setField = trainingMenuParent.transform.Find("UchikomiKoshi").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(足)":
                    setField = trainingMenuParent.transform.Find("UchikomiAshi").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(捨)":
                    setField = trainingMenuParent.transform.Find("UchikomiSute").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "乱取り(立)":
                    setField = trainingMenuParent.transform.Find("RandoriTachi").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(抑)":
                    setField = trainingMenuParent.transform.Find("UchikomiOsae").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(締)":
                    setField = trainingMenuParent.transform.Find("UchikomiShime").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "打ち込み(関)":
                    setField = trainingMenuParent.transform.Find("UchikomiKan").transform.Find("Input").GetComponent<InputField>();
                    break;
                case "乱取り(寝)":
                    setField = trainingMenuParent.transform.Find("RandoriNe").transform.Find("Input").GetComponent<InputField>();
                    break;
            }
            Debug.Log(trainingMenu.Item1 + " : " + trainingMenu.Item2.ToString());
            setField.text = trainingMenu.Item2.ToString();
        }
    }
}
