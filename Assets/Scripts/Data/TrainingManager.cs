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

    public Dictionary<string, int> GetTrainingMenuTemplateDictionary(int limitMInutes, string type)
    {
        Dictionary<string, int> template = new Dictionary<string, int>();
        switch (type)
        {
            default:
            case "balance":
                switch (limitMInutes)
                {
                    default:
                    case 120:
                        template["RandoriTachi"] = 21;
                        template["UchikomiTe"] = 3;
                        template["NagekomiTe"] = 3;
                        template["UchikomiKoshi"] = 3;
                        template["NagekomiKoshi"] = 3;
                        template["UchikomiAshi"] = 3;
                        template["NagekomiAshi"] = 3;
                        template["RandoriNe"] = 12;
                        template["UchikomiOsae"] = 3;
                        template["UchikomiShime"] = 3;
                        template["UchikomiKansetsu"] = 3;
                        template["Running"] = 30;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                    case 180:
                        template["RandoriTachi"] = 31;
                        template["UchikomiTe"] = 5;
                        template["NagekomiTe"] = 5;
                        template["UchikomiKoshi"] = 5;
                        template["NagekomiKoshi"] = 5;
                        template["UchikomiAshi"] = 5;
                        template["NagekomiAshi"] = 5;
                        template["RandoriNe"] = 21;
                        template["UchikomiOsae"] = 6;
                        template["UchikomiShime"] = 6;
                        template["UchikomiKansetsu"] = 6;
                        template["Running"] = 35;
                        template["Dash"] = 15;
                        template["SelfWeight"] = 30;
                        break;
                    case 240:
                        template["RandoriTachi"] = 40;
                        template["UchikomiTe"] = 7;
                        template["NagekomiTe"] = 7;
                        template["UchikomiKoshi"] = 7;
                        template["NagekomiKoshi"] = 7;
                        template["UchikomiAshi"] = 7;
                        template["NagekomiAshi"] = 7;
                        template["RandoriNe"] = 33;
                        template["UchikomiOsae"] = 9;
                        template["UchikomiShime"] = 9;
                        template["UchikomiKansetsu"] = 9;
                        template["Running"] = 40;
                        template["Dash"] = 20;
                        template["SelfWeight"] = 40;
                        break;
                }
                break;
            case "tachiwaza":
                switch (limitMInutes)
                {
                    default:
                    case 120:
                        template["RandoriTachi"] = 35;
                        template["UchikomiTe"] = 5;
                        template["NagekomiTe"] = 5;
                        template["UchikomiKoshi"] = 5;
                        template["NagekomiKoshi"] = 5;
                        template["UchikomiAshi"] = 5;
                        template["NagekomiAshi"] = 5;
                        template["RandoriNe"] = 6;
                        template["UchikomiOsae"] = 3;
                        template["UchikomiShime"] = 3;
                        template["UchikomiKansetsu"] = 3;
                        template["Running"] = 10;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                    case 180:
                        template["RandoriTachi"] = 65;
                        template["UchikomiTe"] = 10;
                        template["NagekomiTe"] = 10;
                        template["UchikomiKoshi"] = 10;
                        template["NagekomiKoshi"] = 10;
                        template["UchikomiAshi"] = 10;
                        template["NagekomiAshi"] = 10;
                        template["RandoriNe"] = 6;
                        template["UchikomiOsae"] = 3;
                        template["UchikomiShime"] = 3;
                        template["UchikomiKansetsu"] = 3;
                        template["Running"] = 10;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                    case 240:
                        template["RandoriTachi"] = 95;
                        template["UchikomiTe"] = 15;
                        template["NagekomiTe"] = 15;
                        template["UchikomiKoshi"] = 15;
                        template["NagekomiKoshi"] = 15;
                        template["UchikomiAshi"] = 15;
                        template["NagekomiAshi"] = 15;
                        template["RandoriNe"] = 6;
                        template["UchikomiOsae"] = 3;
                        template["UchikomiShime"] = 3;
                        template["UchikomiKansetsu"] = 3;
                        template["Running"] = 10;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                }
                break;
            case "newaza":
                switch (limitMInutes)
                {
                    default:
                    case 120:
                        template["RandoriTachi"] = 6;
                        template["UchikomiTe"] = 3;
                        template["UchikomiKoshi"] = 3;
                        template["UchikomiAshi"] = 3;
                        template["RandoriNe"] = 35;
                        template["UchikomiOsae"] = 10;
                        template["UchikomiShime"] = 10;
                        template["UchikomiKansetsu"] = 10;
                        template["Running"] = 10;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                    case 180:
                        template["RandoriTachi"] = 6;
                        template["UchikomiTe"] = 3;
                        template["UchikomiKoshi"] = 3;
                        template["UchikomiAshi"] = 3;
                        template["RandoriNe"] = 95;
                        template["UchikomiOsae"] = 10;
                        template["UchikomiShime"] = 10;
                        template["UchikomiKansetsu"] = 10;
                        template["Running"] = 10;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                    case 240:
                        template["RandoriTachi"] = 6;
                        template["UchikomiTe"] = 3;
                        template["UchikomiKoshi"] = 3;
                        template["UchikomiAshi"] = 3;
                        template["RandoriNe"] = 155;
                        template["UchikomiOsae"] = 10;
                        template["UchikomiShime"] = 10;
                        template["UchikomiKansetsu"] = 10;
                        template["Running"] = 10;
                        template["Dash"] = 10;
                        template["SelfWeight"] = 20;
                        break;
                }
                break;
            case "kiso":
                switch (limitMInutes)
                {
                    default:
                    case 120:
                        template["RandoriTachi"] = 15;
                        template["RandoriNe"] = 15;
                        template["Running"] = 30;
                        template["Dash"] = 30;
                        template["SelfWeight"] = 30;
                        break;
                    case 180:
                        template["RandoriTachi"] = 15;
                        template["RandoriNe"] = 15;
                        template["Running"] = 50;
                        template["Dash"] = 50;
                        template["SelfWeight"] = 50;
                        break;
                    case 240:
                        template["RandoriTachi"] = 15;
                        template["RandoriNe"] = 15;
                        template["Running"] = 70;
                        template["Dash"] = 70;
                        template["SelfWeight"] = 70;
                        break;
                }
                break;
        }
        return template;
    }
}
