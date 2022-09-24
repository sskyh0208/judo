using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager
{
    public List<Tuple<string, int>> GetAllTrainingMenuResult(List<Tuple<string, int>> trainingMenuList)
    {
        List<Tuple<string, int>> allTrainingMenuResult = new List<Tuple<string, int>>();
        foreach (Tuple<string, int> trainingMenu in trainingMenuList)
        {
            foreach (Tuple<string, int> trainingMenuResult in GetTrainingMenuResult(trainingMenu))
            {
                allTrainingMenuResult.Add(trainingMenuResult);
            }
        }
        return allTrainingMenuResult;
    }
    public List<Tuple<string, int>> GetTrainingMenuResult(Tuple<string, int> trainingMenu)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        switch (trainingMenu.Item1)
        {
            default:
            case "Running":
                trainingMenuResult = Running(trainingMenu.Item2);
                break;
            case "Dash":
                trainingMenuResult = Dash(trainingMenu.Item2);
                break;
            case "KaidanDash":
                trainingMenuResult = KaidanDash(trainingMenu.Item2);
                break;
            case "SelfWeight":
                trainingMenuResult = SelfWeight(trainingMenu.Item2);
                break;
            case "MachineWeight":
                trainingMenuResult = MachineWeight(trainingMenu.Item2);
                break;
        }
        return trainingMenuResult;
    }

    // ランニング
    public List<Tuple<string, int>> Running(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 監督の指導力係数
        mainExp = (int)(mainExp * GameData.instance.player.GetAbillity("902").GetUpdateExpSenseCoef());
        // 設備の効果係数
        trainingMenuResult.Add(new Tuple<string, int>("902", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        return trainingMenuResult;
    }

    // ダッシュ
    public List<Tuple<string, int>> Dash(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 監督の指導力係数
        // 設備の効果係数
        mainExp = (int)(mainExp * GameData.instance.player.GetAbillity("901").GetUpdateExpSenseCoef());
        trainingMenuResult.Add(new Tuple<string, int>("901", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        return trainingMenuResult;
    }

    // ダッシュ
    public List<Tuple<string, int>> KaidanDash(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        // スタミナUP
        int mainExp = 70;
        int secondExp = 40;
        int thirdExp = 20;
        // 監督の指導力係数
        // 設備の効果係数
        mainExp = (int)(mainExp * GameData.instance.player.GetAbillity("902").GetUpdateExpSenseCoef());
        trainingMenuResult.Add(new Tuple<string, int>("902", mainExp * minutes));
        secondExp = (int)(secondExp * GameData.instance.player.GetAbillity("901").GetUpdateExpSenseCoef());
        trainingMenuResult.Add(new Tuple<string, int>("901", secondExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(thirdExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        return trainingMenuResult;
    }

    // 自重トレ
    public List<Tuple<string, int>> SelfWeight(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 監督の指導力係数
        mainExp = (int)(mainExp * GameData.instance.player.GetAbillity("900").GetUpdateExpSenseCoef());
        trainingMenuResult.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        return trainingMenuResult;
    }
    // マシントレ
    public List<Tuple<string, int>> MachineWeight(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        // スタミナUP
        int mainExp = 150;
        int secondExp = 10;
        // 設備の効果係数
        // 監督の指導力係数
        mainExp = (int)(mainExp * GameData.instance.player.GetAbillity("900").GetUpdateExpSenseCoef());
        trainingMenuResult.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        return trainingMenuResult;
    }
    // 打ち込み(手)
    public List<Tuple<string, int>> UchikomiTe(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 手技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("0"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 打ち込み(腰)
    public List<Tuple<string, int>> UchikomiKoshi(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 腰技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("1"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 打ち込み(足)
    public List<Tuple<string, int>> UchikomiAshi(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 足技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("3"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 打ち込み(捨)
    public List<Tuple<string, int>> UchikomiSute(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 捨身技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("2"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 乱取り(立)
    public List<Tuple<string, int>> RandoriTachi(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 60;
        int secondExp = 40;
        // 設備の効果係数
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 打ち込み(抑)
    public List<Tuple<string, int>> UchikomiOsae(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 抑技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("5"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 打ち込み(締)
    public List<Tuple<string, int>> UchikomiShime(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 締技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("6"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 打ち込み(関)
    public List<Tuple<string, int>> UchikomiKan(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 関節技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("7"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
    // 乱取り(寝)
    public List<Tuple<string, int>> RandoriNe(int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        int mainExp = 60;
        int secondExp = 40;
        // 設備の効果係数
        // 捨身技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }

        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * GameData.instance.player.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
      }
        return trainingMenuResult;
    }
}
