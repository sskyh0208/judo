using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SchoolManager
{
    public SchoolMst[] schoolMst;
    public Dictionary<string, School> schoolList;

    public void SetAllSchool()
    {
        schoolList = new Dictionary<string, School>();
        foreach (SchoolMst data in schoolMst)
        {
            schoolList.Add(data.id, new School(data.id, data.placeId, data.name, data.schoolRank));
        }
    }

    public School GetSchool(string schoolId)
    {
        return schoolList[schoolId];
    }

    public List<School> GetSchools(List<string> schoolIdList)
    {   
        List<School> result = new List<School>();
        foreach (string schoolId in schoolIdList)
        {
            result.Add(schoolList[schoolId]);
        }
        return result;
    }

    // 部員数5名以上を有する学校のみ取得
    public List<School> GetApplyJoinEventMembersNumSchools(List<string> schoolIdList)
    {   
        List<School> result = new List<School>();
        foreach (string schoolId in schoolIdList)
        {
            if (schoolList[schoolId].members.Count >= 5)
            {
                result.Add(schoolList[schoolId]);
            }
        }
        return result;
    }

    public List<School> GetRegionAllSchool(string schoolId)
    {
        return GetTargetTypeAllSchool(0, 2, schoolId);
    }
    public List<School> GetPlaceAllSchool(string schoolId)
    {
        return GetTargetTypeAllSchool(2, 2, schoolId);
    }

    public List<School> GetCityAllSchool(string schoolId)
    {
        return GetTargetTypeAllSchool(4, 2, schoolId);
    }

    private List<School> GetTargetTypeAllSchool(int start, int length, string targetId)
    {
        targetId = targetId.Substring(start, length);
        List<School> targetSchools = new List<School>();
        foreach (string key in schoolList.Keys)
        {
            if (key.Substring(start, length) == targetId)
            {
                targetSchools.Add(GetSchool(key));
            }
        }
        return targetSchools;
    }

    // 監督を設定する
    public void SetSuperVoisor(string schoolId, PlayerManager supervisor)
    {
        schoolList[schoolId].supervisor = supervisor;
    }

    public PlayerManager GetSchoolMember(string memberId)
    {
        return GetSchool(memberId.Substring(4, 9)).GetMember(memberId);
    }

    public void DoneGuradiationAllSchools()
    {
        foreach (string schoolId in schoolList.Keys)
        {
            schoolList[schoolId].DoneGraduationMembers();
        }
    }
}

[Serializable]
public class SchoolMst
{
    public string id;
    public string placeId;
    public string name;
    public int schoolRank;
}

public class School
{
    public string id;
    public string placeId;
    public string name;
    public int schoolRank;

    public int totalStatus;

    // 顧問
    public PlayerManager supervisor;

    // 部員
    public Dictionary<string, PlayerManager> members;

    public int trainingLimitMinutes;
    public List<Tuple<string, int>> trainingMenu = new List<Tuple<string, int>>();
    public List<Tuple<string, int>> trainingMenuResult = new List<Tuple<string, int>>();

    public School(string id, string placeId, string name, int schoolRank)
    {
        this.id = id;
        this.placeId = placeId;
        this.name = name;
        this.schoolRank = schoolRank;
        this.totalStatus = 0;
        this.supervisor = null;
        this.members = new Dictionary<string, PlayerManager>();
        this.trainingLimitMinutes = 120;
        this.trainingMenu = new List<Tuple<string, int>>();
        this.trainingMenuResult = new List<Tuple<string, int>>();
    }

    public int GenerateThisYearLimitMembersNum()
    {
        float membersDefaultNum = 10.0f;
        float membersMaxWeight;
        System.Random r = new System.Random();
        switch (schoolRank)
        {
            default:
            case 1:
                membersMaxWeight = r.Next(0, 3) * 0.1f;
                break;
            case 2:
                membersMaxWeight = r.Next(3, 5) * 0.1f;
                break;
            case 3:
                membersMaxWeight = r.Next(5, 7) * 0.1f;
                break;
            case 4:
                membersMaxWeight = r.Next(7, 9) * 0.1f;
                break;
            case 5:
                membersMaxWeight = r.Next(9, 11) * 0.1f;
                break;
            case 6:
                membersMaxWeight = r.Next(11, 13) * 0.1f;
                break;

        }
        return (int)(membersDefaultNum * membersMaxWeight);
    }

    // 学年の降順で部員を取得
    public List<PlayerManager> GetSortDescMembers()
    {
        return members.Values.OrderByDescending(member => member.positionId).ToList();
    }

    // IDで部員を取得
    public PlayerManager GetMember(string memberId)
    {
        {
            return members[memberId];
        }
    }

    // 学校の総合力取得
    public void SetSchoolTotalStatus()
    {
        totalStatus = 0;
        foreach (PlayerManager member in members.Values)
        {
            foreach(Abillity abillity in member.abillities)
            {
                totalStatus += abillity.status;
            }
        }
    }

    public void DoneTraining()
    {
        foreach (PlayerManager member in members.Values)
        {
            member.GetTrainingExp(trainingMenuResult);
        }
    }

    public void DoneGraduationMembers()
    {
        // 部員から削除
        List<string> memberIds = members.Keys.ToList();
        foreach (string memberId in memberIds)
        {

            if (members[memberId].positionId == 3)
            {
                members.Remove(key: memberId);
            }
        }
        // １学年上げる
        memberIds = members.Keys.ToList();
        foreach (string memberId in memberIds)
        {
            members[memberId].positionId ++;
        }
    }

    public bool CheckTrainingLimitMinutes(int minutes)
    {
        return minutes > this.trainingLimitMinutes;
    }

    /********************************************* 以下トレーニングメニュー *********************************************/
    public void ClearTrainingMenu()
    {
        this.trainingMenu = new List<Tuple<string, int>>();
        this.trainingMenuResult = new List<Tuple<string, int>>();
    }

    public void SetTrainingMenu(string trainingName, int minutes)
    {
        this.trainingMenu.Add(new Tuple<string, int>(trainingName, minutes));
        switch (trainingName)
        {
            default:
            case "ランニング":
                Running(minutes);
                break;
            case "ダッシュ":
                Dash(minutes);
                break;
            case "階段ダッシュ":
                KaidanDash(minutes);
                break;
            case "自重トレ":
                SelfWeight(minutes);
                break;
            case "マシントレ":
                MachineWeight(minutes);
                break;
        }
    }

    // ランニング
    public void Running(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 監督の指導力係数
        // 設備の効果係数
        this.trainingMenuResult.Add(new Tuple<string, int>("902", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
    }

    // ダッシュ
    public void Dash(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 監督の指導力係数
        // 設備の効果係数
        this.trainingMenuResult.Add(new Tuple<string, int>("901", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
    }

    // ダッシュ
    public void KaidanDash(int minutes)
    {
        // スタミナUP
        int mainExp = 70;
        int secondExp = 40;
        int thirdExp = 20;
        // 監督の指導力係数
        // 設備の効果係数
        this.trainingMenuResult.Add(new Tuple<string, int>("902", mainExp * minutes));
        this.trainingMenuResult.Add(new Tuple<string, int>("901", secondExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, thirdExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, thirdExp * minutes));
        }
    }

    // 自重トレ
    public void SelfWeight(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 監督の指導力係数
        // 設備の効果係数
        this.trainingMenuResult.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
    }
    // マシントレ
    public void MachineWeight(int minutes)
    {
        // スタミナUP
        int mainExp = 150;
        int secondExp = 10;
        // 監督の指導力係数
        // 設備の効果係数
        this.trainingMenuResult.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
    }
}
