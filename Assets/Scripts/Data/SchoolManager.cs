using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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


    public List<School> GetRegionAllSchool(string regionId)
    {
        return GetTargetTypeAllSchool(0, 2, regionId);
    }

    public List<School> GetPlaceAllSchool(string placeId)
    {
        return GetTargetTypeAllSchool(2, 2, "00" + placeId);
    }

    public List<School> GetCityAllSchool(string placeId, string cityId)
    {
        return GetTargetTypeAllSchool(2, 4, "00" + placeId + cityId);
    }
    public List<School> GetSameRegionAllSchool(string schoolId)
    {
        return GetTargetTypeAllSchool(0, 2, schoolId);
    }

    public List<School> GetSamePlaceAllSchool(string schoolId)
    {
        return GetTargetTypeAllSchool(2, 2, schoolId);
    }

    public List<School> GetSameCityAllSchool(string schoolId)
    {
        return GetTargetTypeAllSchool(2, 4, schoolId);
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

    // 元となる学校Idから市までの地域IDをすべて取得するGetSamePlaceIdList
    public List<string> GetSamePlaceIdList(string baseId)
    {
        string checkId = baseId.Substring(0, 4);
        HashSet<string> targetList = new HashSet<string>();
        foreach (School school in schoolList.Values.ToList())
        {
            if (Regex.IsMatch(school.id, "^" + checkId))
            {
                targetList.Add(school.id.Substring(0, 6));
            }
        }
        return targetList.ToList();
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
    public int regularMemberStatus;

    // 顧問
    public PlayerManager supervisor;

    // 部員
    public Dictionary<string, PlayerManager> members;
    public Dictionary<int, PlayerManager> regularMembers;

    public int trainingLimitMinutes;
    public List<Tuple<string, int>> trainingMenu;
    public List<Tuple<string, int>> trainingMenuResult;

    public School(string id, string placeId, string name, int schoolRank)
    {
        this.id = id;
        this.placeId = placeId;
        this.name = name;
        this.schoolRank = schoolRank;
        this.totalStatus = 0;
        this.supervisor = null;
        this.members = new Dictionary<string, PlayerManager>();
        this.regularMembers = new Dictionary<int, PlayerManager>();
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
                membersMaxWeight = r.Next(2, 4) * 0.1f;
                break;
            case 2:
                membersMaxWeight = r.Next(3, 4) * 0.1f;
                break;
            case 3:
                membersMaxWeight = r.Next(4, 5) * 0.1f;
                break;
            case 4:
                membersMaxWeight = r.Next(5, 6) * 0.1f;
                break;
            case 5:
                membersMaxWeight = r.Next(7, 8) * 0.1f;
                break;
            case 6:
                membersMaxWeight = r.Next(8, 9) * 0.1f;
                break;
            case 7:
                membersMaxWeight = r.Next(9, 10) * 0.1f;
                break;
            case 8:
                membersMaxWeight = r.Next(10, 11) * 0.1f;
                break;
            case 9:
                membersMaxWeight = r.Next(11, 12) * 0.1f;
                break;
            case 10:
                membersMaxWeight = r.Next(12, 13) * 0.1f;
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

    // レギュラーの総合力取得
    private void SetSchoolRegularMemberStatus()
    {
        regularMemberStatus = 0;
        foreach (PlayerManager member in regularMembers.Values)
        {
            foreach(Abillity abillity in member.abillities)
            {
                regularMemberStatus += abillity.status;
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

    public void SetRegularMember()
    {
        int count = 4;
        foreach (PlayerManager member in members.Values.OrderByDescending(member => member.totalStatus).ToList())
        {
            regularMembers[count] = member;
            if (count == 0)
            {
                break;
            }
            count --;
        }
        SetSchoolRegularMemberStatus();

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
        mainExp = (int)(mainExp * supervisor.GetAbillity("902").GetUpdateExpSenseCoef());
        // 設備の効果係数
        this.trainingMenuResult.Add(new Tuple<string, int>("902", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
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
        mainExp = (int)(mainExp * supervisor.GetAbillity("901").GetUpdateExpSenseCoef());
        this.trainingMenuResult.Add(new Tuple<string, int>("901", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
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
        mainExp = (int)(mainExp * supervisor.GetAbillity("902").GetUpdateExpSenseCoef());
        this.trainingMenuResult.Add(new Tuple<string, int>("902", mainExp * minutes));
        secondExp = (int)(secondExp * supervisor.GetAbillity("901").GetUpdateExpSenseCoef());
        this.trainingMenuResult.Add(new Tuple<string, int>("901", secondExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(thirdExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
    }

    // 自重トレ
    public void SelfWeight(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 監督の指導力係数
        mainExp = (int)(mainExp * supervisor.GetAbillity("900").GetUpdateExpSenseCoef());
        this.trainingMenuResult.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
    }
    // マシントレ
    public void MachineWeight(int minutes)
    {
        // スタミナUP
        int mainExp = 150;
        int secondExp = 10;
        // 設備の効果係数
        // 監督の指導力係数
        mainExp = (int)(mainExp * supervisor.GetAbillity("900").GetUpdateExpSenseCoef());
        this.trainingMenuResult.Add(new Tuple<string, int>("900", mainExp * minutes));
        // 技すべて
        foreach (string wazaTypeId in new List<string>(){"0", "1"})
        {
            foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType(wazaTypeId))
            {
                int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
                this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
            }
        }
    }
    // 打ち込み(手)
    public void UchikomiTe(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 手技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("0"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 打ち込み(腰)
    public void UchikomiKoshi(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 腰技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("1"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 打ち込み(足)
    public void UchikomiAshi(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 足技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("3"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 打ち込み(捨)
    public void UchikomiSute(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 捨身技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("2"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 乱取り(立)
    public void RandoriTachi(int minutes)
    {
        // スタミナUP
        int mainExp = 60;
        int secondExp = 40;
        // 設備の効果係数
        // 立技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("0"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 打ち込み(抑)
    public void UchikomiOsae(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 抑技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("5"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 打ち込み(締)
    public void UchikomiShime(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 締技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("6"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 打ち込み(関)
    public void UchikomiKan(int minutes)
    {
        // スタミナUP
        int mainExp = 100;
        int secondExp = 20;
        // 設備の効果係数
        // 関節技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithGroup("7"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
    // 乱取り(寝)
    public void RandoriNe(int minutes)
    {
        // スタミナUP
        int mainExp = 60;
        int secondExp = 40;
        // 設備の効果係数
        // 捨身技すべて
        foreach (string wazaId in GameData.instance.abillityManager.GetWazaIdArrayWithType("1"))
        {
            // 監督の指導力係数
            int exp = (int)(mainExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }

        foreach (string wazaId in new List<string>(){"900", "901", "902"})
        {
            int exp = (int)(secondExp * supervisor.GetAbillity(wazaId).GetUpdateExpSenseCoef());
            this.trainingMenuResult.Add(new Tuple<string, int>(wazaId, exp * minutes));
        }
    }
}
