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

    // ?????????????????????
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

    // ??????????????????Id????????????????????????ID????????????????????????GetSamePlaceIdList
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

    // ??????????????????????????????????????????
    public void UpgradeScoolRank()
    {
        foreach (string schoolId in schoolList.Keys)
        {
            List<int> gradeLine = new List<int>();
            switch (schoolList[schoolId].schoolRank)
            {
                default:
                case 1:
                    gradeLine = new List<int>{0, 8};
                    break;
                case 2:
                    gradeLine = new List<int>{8, 16};
                    break;
                case 3:
                    gradeLine = new List<int>{16, 32};
                    break;
                case 4:
                    gradeLine = new List<int>{32, 64};
                    break;
                case 5:
                    gradeLine = new List<int>{64, 128};
                    break;
                case 6:
                    gradeLine = new List<int>{128, 256};
                    break;
                case 7:
                    gradeLine = new List<int>{256, 512};
                    break;
                case 8:
                    gradeLine = new List<int>{512, 1024};
                    break;
                case 9:
                    gradeLine = new List<int>{1024, 2048};
                    break;
                case 10:
                    gradeLine = new List<int>{2048, 9999};
                    break;
            }

            // ????????????????????????????????????
            if (schoolList[schoolId].fame < gradeLine[0])
            {
                schoolList[schoolId].schoolRank -= 1;
            }
            else if (schoolList[schoolId].fame >= gradeLine[1])
            {
                schoolList[schoolId].schoolRank += 1;
            }
            schoolList[schoolId].fame = 0;
        }
    }

    // 
    public void PayTheClubActivityFee()
    {
        foreach (string schoolId in schoolList.Keys)
        {  
            schoolList[schoolId].money += (schoolList[schoolId].schoolRank * 10) + schoolList[schoolId].fame;
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
    public int baseRank;
    public int fame;
    public int money;

    public int schoolRank;
    public int totalStatus;
    public int regularMemberStatus;

    // ??????
    public PlayerManager supervisor;

    // ??????
    public Dictionary<string, PlayerManager> members;
    public Dictionary<int, PlayerManager> regularMembers;

    public int trainingLimitMinutes;
    public Dictionary<string, int> trainingMenu1;
    public Dictionary<string, int> trainingMenu2;
    public Dictionary<string, int> trainingMenu3;
    public Dictionary<string, int> trainingMenu4;
    public Dictionary<string, int> trainingMenu5;
    
    public List<Setsubi> setsubiList;

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
        this.trainingMenu1 = new Dictionary<string, int>();
        this.trainingMenu2 = new Dictionary<string, int>();
        this.trainingMenu3 = new Dictionary<string, int>();
        this.trainingMenu4 = new Dictionary<string, int>();
        this.trainingMenu5 = new Dictionary<string, int>();
        this.setsubiList = GameData.instance.setsubiManager.GetAllSetsubi();
        this.money = 50;
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

    // ?????????????????????????????????
    public List<PlayerManager> GetSortDescMembers()
    {
        return members.Values.OrderByDescending(member => member.positionId).ToList();
    }

    // ID??????????????????
    public PlayerManager GetMember(string memberId)
    {
        {
            return members[memberId];
        }
    }

    // ????????????????????????
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

    // ?????????????????????????????????
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
            member.GetTrainingExp(GameData.instance.trainingManager.GetTrainingMenuResult(member.trainingMenu.menuList));
        }
    }

    public void DoneGraduationMembers()
    {
        // ??????????????????
        List<string> memberIds = members.Keys.ToList();
        foreach (string memberId in memberIds)
        {

            if (members[memberId].positionId == 3)
            {
                members.Remove(key: memberId);
            }
        }
        // ??????????????????
        memberIds = members.Keys.ToList();
        foreach (string memberId in memberIds)
        {
            members[memberId].positionId ++;
        }
    }

    public void SetRegularMember()
    {
        int count = 4;
        List<PlayerManager> regularList = members.Values.OrderByDescending(member => member.totalStatus).ToList().GetRange(0, 5);
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

    public Dictionary<string, int> GetTrainingMenu(int setMenuNum)
    {
        switch (setMenuNum)
        {
            default:
            case 1:
                return this.trainingMenu1;
            case 2:
                return this.trainingMenu2;
            case 3:
                return this.trainingMenu3;
            case 4:
                return this.trainingMenu4;
            case 5:
                return this.trainingMenu5;
        }
    }

    public void SetTrainingMenu(int setMenuNum, Dictionary<string, int>trainingMenu)
    {
        switch (setMenuNum)
        {
            default:
            case 1:
                this.trainingMenu1 = trainingMenu;
                break;
            case 2:
                this.trainingMenu2 = trainingMenu;
                break;
            case 3:
                this.trainingMenu3 = trainingMenu;
                break;
            case 4:
                this.trainingMenu4 = trainingMenu;
                break;
            case 5:
                this.trainingMenu5 = trainingMenu;
                break;
        }
    }
}
