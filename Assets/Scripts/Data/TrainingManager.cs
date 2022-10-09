using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager
{
    public List<Tuple<string, int>> GetTrainingMenuResult(Dictionary<string, int> trainingMenu)
    {
        List<Tuple<string, int>> allTrainingMenuResult = new List<Tuple<string, int>>();
        foreach (KeyValuePair<string, int> training in trainingMenu)
        {
            foreach (Tuple<string, int> trainingMenuResult in this.ExecuteTraining(this.GetTraining(training.Key), GameData.instance.player, training.Value))
            {
                allTrainingMenuResult.Add(trainingMenuResult);
            }
        }
        return allTrainingMenuResult;
    }

    public List<Tuple<string, int>> ExecuteTraining(Training training, PlayerManager supervisor, int minutes)
    {
        List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();
        foreach (string wazaId in training.firstExpWazaIdList)
        {
            int exp = (int)(training.firstExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in training.secondExpWazaIdList)
        {
            int exp = (int)(training.secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in training.thirdExpWazaIdList)
        {
            int exp = (int)(training.thirdExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }


        foreach (string typeId in training.firstExpWazaTypeList)
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(typeId))
            {
                int exp = (int)(training.firstExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        foreach (string typeId in training.secondExpWazaTypeList)
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(typeId))
            {
                int exp = (int)(training.secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        foreach (string typeId in training.thirdExpWazaTypeList)
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(typeId))
            {
                int exp = (int)(training.thirdExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }

        foreach (string group in training.firstExpWazaGroupList)
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup(group))
            {
                int exp = (int)(training.firstExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        foreach (string group in training.secondExpWazaGroupList)
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup(group))
            {
                int exp = (int)(training.secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
        foreach (string group in training.thirdExpWazaGroupList)
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup(group))
            {
                int exp = (int)(training.thirdExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }

        return trainingMenuResult;
    }

    public Training GetTraining(string trainingName)
    {
        switch (trainingName)
        {
            default:
            case "Running":
                return new Running();
            case "Dash":
                return new Dash();
            case "KaidanDash":
                return new KaidanDash();
            case "SelfWeight":
                return new SelfWeight();
            case "MachineWeight":
                return new MachineWeight();
            case "UchikomiTe":
                return new UchikomiTe();
            case "IdoUchikomiTe":
                return new IdoUchikomiTe();
            case "SpeedUchikomiTe":
                return new SpeedUchikomiTe();
            case "SanninUchikomiTe":
                return new SanninUchikomiTe();
            case "NagekomiTe":
                return new NagekomiTe();
            case "UchikomiAshi":
                return new UchikomiAshi();
            case "IdoUchikomiAshi":
                return new IdoUchikomiAshi();
            case "SpeedUchikomiAshi":
                return new SpeedUchikomiAshi();
            case "SanninUchikomiAshi":
                return new SanninUchikomiAshi();
            case "NagekomiAshi":
                return new NagekomiAshi();
            case "UchikomiKoshi":
                return new UchikomiKoshi();
            case "IdoUchikomiKoshi":
                return new IdoUchikomiKoshi();
            case "SpeedUchikomiKoshi":
                return new SpeedUchikomiKoshi();
            case "SanninUchikomiKoshi":
                return new SanninUchikomiKoshi();
            case "NagekomiKoshi":
                return new NagekomiKoshi();
            case "UchikomiOsae":
                return new UchikomiOsae();
            case "GrapplingOsae":
                return new GrapplingOsae();
            case "UchikomiShime":
                return new UchikomiShime();
            case "GrapplingShime":
                return new GrapplingShime();
            case "UchikomiKansetsu":
                return new UchikomiKansetsu();
            case "GrapplingKansetsu":
                return new GrapplingKansetsu();
            case "RandoriTachi":
                return new RandoriTachi();
            case "RandoriNe":
                return new RandoriNe();
        }
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

public class TrainingMenu
{
    public int trainingMenuNum;
    public Dictionary<string, int> menuList;

    public TrainingMenu(int trainingMenuNum = 0, Dictionary<string, int> menuList = null)
    {
        this.trainingMenuNum = trainingMenuNum;
        if (menuList is null)
        {
            this.menuList = new Dictionary<string, int>();
        }
        else
        {
            this.menuList = menuList;
        }
    }
}

public class Training
{

    public string colorCode = "#808080";
    public int firstExp = 0;
    public int secondExp = 0;
    public int thirdExp = 0;

    // 技IDで指定する場合追加
    public List<string> firstExpWazaIdList = new List<string>();
    // 技タイプで指定する場合追加
    public List<string> firstExpWazaTypeList = new List<string>();
    // 技グループで指定する場合追加
    public List<string> firstExpWazaGroupList = new List<string>();
    public List<string> secondExpWazaIdList = new List<string>();
    public List<string> secondExpWazaTypeList = new List<string>();
    public List<string> secondExpWazaGroupList = new List<string>();
    public List<string> thirdExpWazaIdList = new List<string>();
    public List<string> thirdExpWazaTypeList = new List<string>();
    public List<string> thirdExpWazaGroupList = new List<string>();

}

class Running: Training
{
    public Running()
    {
        this.colorCode = "#CD5C5C";
        this.firstExp = 100;
        this.secondExp = 20;

        this.firstExpWazaIdList = new List<string>(){"902"};
        this.secondExpWazaTypeList = new List<string>(){"0", "1"};
    }
}

class Dash: Training
{
    public Dash()
    {
        this.colorCode = "#CD5C5C";
        this.firstExp = 100;
        this.secondExp = 20;

        this.firstExpWazaIdList = new List<string>(){"901"};
        this.secondExpWazaTypeList = new List<string>(){"0", "1"};
    }
}

class KaidanDash: Training
{
    public KaidanDash()
    {
        this.colorCode = "#CD5C5C";
        this.firstExp = 70;
        this.secondExp = 40;
        this.thirdExp = 20;
        
        this.firstExpWazaIdList = new List<string>(){"902"};
        this.secondExpWazaIdList = new List<string>(){"901"};
        this.thirdExpWazaTypeList = new List<string>(){"0", "1"};
    }
}

class SelfWeight: Training
{
    public SelfWeight()
    {
        this.colorCode = "#CD5C5C";
        this.firstExp = 100;
        this.secondExp = 20;

        this.firstExpWazaIdList = new List<string>(){"900"};
        this.secondExpWazaTypeList = new List<string>(){"0", "1"};
    }
}

class MachineWeight: Training
{
    public MachineWeight()
    {
        this.colorCode = "#CD5C5C";
        this.firstExp = 150;
        this.secondExp = 20;

        this.firstExpWazaIdList = new List<string>(){"900"};
        this.secondExpWazaTypeList = new List<string>(){"0", "1"};
    }
}

class Circuit: Training
{
    public Circuit()
    {
        this.colorCode = "#CD5C5C";
        this.firstExp = 70;
        this.secondExp = 30;

        this.firstExpWazaIdList = new List<string>(){"900", "901", "902"};
        this.secondExpWazaTypeList = new List<string>(){"0", "1"};
    }
}

class UchikomiTe: Training
{
    public UchikomiTe()
    {
        this.colorCode = "#4169E1";
        this.firstExp = 90;
        this.secondExp = 10;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class IdoUchikomiTe: Training
{
    public IdoUchikomiTe()
    {
        this.colorCode = "#4169E1";
        this.firstExp = 130;
        this.secondExp = 20;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class SpeedUchikomiTe: Training
{
    public SpeedUchikomiTe()
    {
        this.colorCode = "#4169E1";
        this.firstExp = 120;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"901", "902"};
    }
}

class SanninUchikomiTe: Training
{
    public SanninUchikomiTe()
    {
        this.colorCode = "#4169E1";
        this.firstExp = 120;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "902"};
    }
}

class NagekomiTe: Training
{
    public NagekomiTe()
    {
        this.colorCode = "#4169E1";
        this.firstExp = 150;
        this.secondExp = 10;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class UchikomiKoshi: Training
{
    public UchikomiKoshi()
    {
        this.colorCode = "#DAA520";
        this.firstExp = 100;
        this.secondExp = 20;

        this.firstExpWazaGroupList = new List<string>(){"1"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class IdoUchikomiKoshi: Training
{
    public IdoUchikomiKoshi()
    {
        this.colorCode = "#DAA520";
        this.firstExp = 140;
        this.secondExp = 20;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class SpeedUchikomiKoshi: Training
{
    public SpeedUchikomiKoshi()
    {
        this.colorCode = "#DAA520";
        this.firstExp = 130;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"901", "902"};
    }
}

class SanninUchikomiKoshi: Training
{
    public SanninUchikomiKoshi()
    {
        this.colorCode = "#DAA520";
        this.firstExp = 130;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "902"};
    }
}

class NagekomiKoshi: Training
{
    public NagekomiKoshi()
    {
        this.colorCode = "#DAA520";
        this.firstExp = 150;
        this.secondExp = 10;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class UchikomiAshi: Training
{
    public UchikomiAshi()
    {
        this.colorCode = "#6B8E23";
        this.firstExp = 100;
        this.secondExp = 20;

        this.firstExpWazaGroupList = new List<string>(){"2"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class IdoUchikomiAshi: Training
{
    public IdoUchikomiAshi()
    {
        this.colorCode = "#6B8E23";
        this.firstExp = 140;
        this.secondExp = 20;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class SpeedUchikomiAshi: Training
{
    public SpeedUchikomiAshi()
    {
        this.colorCode = "#6B8E23";
        this.firstExp = 130;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"901", "902"};
    }
}

class SanninUchikomiAshi: Training
{
    public SanninUchikomiAshi()
    {
        this.colorCode = "#6B8E23";
        this.firstExp = 130;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "902"};
    }
}

class NagekomiAshi: Training
{
    public NagekomiAshi()
    {
        this.colorCode = "#6B8E23";
        this.firstExp = 150;
        this.secondExp = 10;

        this.firstExpWazaGroupList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class RandoriTachi: Training
{
    public RandoriTachi()
    {
        this.colorCode = "#B1063A";
        this.firstExp = 70;
        this.secondExp = 40;

        this.firstExpWazaTypeList = new List<string>(){"0"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class UchikomiOsae: Training
{
    public UchikomiOsae()
    {
        this.colorCode = "#800080";
        this.firstExp = 80;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"5"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class GrapplingOsae: Training
{
    public GrapplingOsae()
    {
        this.colorCode = "#800080";
        this.firstExp = 120;
        this.secondExp = 40;

        this.firstExpWazaGroupList = new List<string>(){"5"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class UchikomiShime: Training
{
    public UchikomiShime()
    {
        this.colorCode = "#800080";
        this.firstExp = 100;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"6"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class GrapplingShime: Training
{
    public GrapplingShime()
    {
        this.colorCode = "#800080";
        this.firstExp = 120;
        this.secondExp = 40;

        this.firstExpWazaGroupList = new List<string>(){"5"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class UchikomiKansetsu: Training
{
    public UchikomiKansetsu()
    {
        this.colorCode = "#800080";
        this.firstExp = 100;
        this.secondExp = 30;

        this.firstExpWazaGroupList = new List<string>(){"7"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class GrapplingKansetsu: Training
{
    public GrapplingKansetsu()
    {
        this.colorCode = "#800080";
        this.firstExp = 120;
        this.secondExp = 40;

        this.firstExpWazaGroupList = new List<string>(){"5"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}

class RandoriNe: Training
{
    public RandoriNe()
    {
        this.colorCode = "#745399";
        this.firstExp = 70;
        this.secondExp = 50;

        this.firstExpWazaTypeList = new List<string>(){"1"};
        this.secondExpWazaIdList = new List<string>(){"900", "901", "902"};
    }
}