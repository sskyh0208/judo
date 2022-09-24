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
    public int baseRank;
    public int fame;

    public int schoolRank;
    public int totalStatus;
    public int regularMemberStatus;

    // 顧問
    public PlayerManager supervisor;

    // 部員
    public Dictionary<string, PlayerManager> members;
    public Dictionary<int, PlayerManager> regularMembers;

    public int trainingLimitMinutes;
    

    public School(string id, string placeId, string name, int schoolRank)
    {
        this.id = id;
        this.placeId = placeId;
        this.name = name;
        this.baseRank = schoolRank;
        this.fame = 0;
        this.schoolRank = 0;
        this.totalStatus = 0;
        this.supervisor = null;
        this.members = new Dictionary<string, PlayerManager>();
        this.regularMembers = new Dictionary<int, PlayerManager>();
        this.trainingLimitMinutes = 120;
        SetSchoolRank();
    }

    public void SetSchoolRank()
    {
        if (this.baseRank > this.fame)
        {this.schoolRank = this.baseRank;}
        else
        {this.schoolRank = this.fame;}
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
                membersMaxWeight = r.Next(2, 6) * 0.1f;
                break;
            case 2:
                membersMaxWeight = r.Next(2, 7) * 0.1f;
                break;
            case 3:
                membersMaxWeight = r.Next(2, 8) * 0.1f;
                break;
            case 4:
                membersMaxWeight = r.Next(2, 9) * 0.1f;
                break;
            case 5:
                membersMaxWeight = r.Next(5, 12) * 0.1f;
                break;
            case 6:
                membersMaxWeight = r.Next(5, 13) * 0.1f;
                break;
            case 7:
                membersMaxWeight = r.Next(5, 14) * 0.1f;
                break;
            case 8:
                membersMaxWeight = r.Next(5, 15) * 0.1f;
                break;
            case 9:
                membersMaxWeight = r.Next(5, 16) * 0.1f;
                break;
            case 10:
                membersMaxWeight = r.Next(5, 17) * 0.1f;
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
            member.GetTrainingExp(GameData.instance.trainingManager.GetAllTrainingMenuResult(member.trainingMenu));
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
        List<PlayerManager> regularList = members.Values.OrderByDescending(member => member.totalStatus).ToList().GetRange(0, 4);
        foreach (PlayerManager member in regularList.OrderBy(member => member.weight).ToList())
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
}
