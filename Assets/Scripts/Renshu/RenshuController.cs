using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RenshuController : MonoBehaviour
{

    private GameObject trainingMenuPanel;
    private void Start() {
        trainingMenuPanel = GameObject.Find("TrainingMenuPanel");
    }
    public void SetTrainingMenu()
    {
        School targetSchool = GameData.instance.schoolManager.getPlayerSchool(GameData.instance.player);

        List<Tuple<RectTransform, Image, int>> barTrainingList = new List<Tuple<RectTransform, Image, int>>();
        List<Tuple<string, int>> trainingMenuTuple = new List<Tuple<string, int>>();
        int totalTrainingMinutes = 0;
        foreach (Transform child in GameObject.Find("TrainingMenuParent").transform)
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
}
