using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SchoolManager
{
    public School[] schoolArray;

    public School getPlayerSchool(PlayerManager player)
    {
        string placeId = player.placeId;
        string schoolId = player.schoolId;

        School target = new School();
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId && school.id == schoolId)
            {
                target = school;
            }
        }
        return target;
    }

    public School getSchool(string placeId, string schoolId)
    {
        School target = new School();
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId && school.id == schoolId)
            {
                target = school;
            }
        }
        return target;
    }

    public List<School> getPlaceAllSchool(string placeId)
    {
        List<School> targetPlaceSchools = new List<School>();
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId)
            {
                targetPlaceSchools.Add(school);
            }
        }
        return targetPlaceSchools;
    }

    // 監督を設定する
    public void setSuperVoisor(string placeId, string schoolId, PlayerManager supervisor)
    {
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId && school.id == schoolId)
            {
                school.supervisor = supervisor;
                break;
            }
        }
    }
}

[Serializable]
public class School
{
    public string id;
    public string placeId;
    public string name;
    public int rank;

    // 顧問
    public PlayerManager supervisor;

    // 部員
    public List<PlayerManager> members;

    public int trainingLimitMinutes = 120;
    public List<Tuple<string, int>> trainingMenu;

    public int GenerateThisYearLimitMembersNum()
    {
        float membersDefaultNum = 10.0f;
        float membersMaxWeight;
        System.Random r = new System.Random();
        switch (rank)
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

    public List<PlayerManager> GetSortDescMembers()
    {
        return members.OrderByDescending(member => member.positionId).ToList();
    }

    public void DoneTraining()
    {
        foreach (PlayerManager member in members)
        {
            member.GetTrainingExp(trainingMenu);
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
    }

    public void SetTrainingMenu(string trainingName, int minutes)
    {
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
        this.trainingMenu.Add(new Tuple<string, int>("902", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
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
        this.trainingMenu.Add(new Tuple<string, int>("901", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
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
        this.trainingMenu.Add(new Tuple<string, int>("902", mainExp * minutes));
        this.trainingMenu.Add(new Tuple<string, int>("901", secondExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, thirdExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, thirdExp * minutes));
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
        this.trainingMenu.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
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
        this.trainingMenu.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
        // 寝技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            this.trainingMenu.Add(new Tuple<string, int>(wazaId, secondExp * minutes));
        }
    }
}
