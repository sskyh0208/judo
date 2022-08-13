using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchManager
{
}

public class Tournament
{
    public Schedule eventObj;
    public string placeId;
    public string region;
    public string place;
    public string city;
    public string targetRangeId;

    public List<School> joinSchoolList;
    public List<PlayerManager> joinMember60List;
    public List<PlayerManager> joinMember66List;
    public List<PlayerManager> joinMember73List;
    public List<PlayerManager> joinMember81List;
    public List<PlayerManager> joinMember90List;
    public List<PlayerManager> joinMember100List;
    public List<PlayerManager> joinMemberOver100List;

    public Tournament (Schedule eventObj, string placeId)
    {
        this.eventObj = eventObj;
        this.placeId = placeId;
        this.region = placeId.Substring(0, 2);
        this.place = placeId.Substring(2, 2);
        this.city = placeId.Substring(2, 4);
        this.targetRangeId = GenerateEventAddTargetRangeId();
        this.joinSchoolList = new List<School>();
        this.joinMember60List = new List<PlayerManager>();
        this.joinMember66List = new List<PlayerManager>();
        this.joinMember73List = new List<PlayerManager>();
        this.joinMember81List = new List<PlayerManager>();
        this.joinMember90List = new List<PlayerManager>();
        this.joinMember100List = new List<PlayerManager>();
        this.joinMemberOver100List = new List<PlayerManager>();
    }

    public void SetTournamentDetail()
    {
        // 団体戦を含むかどうかかどうか
        if (eventObj.eventType == "all" || eventObj.eventType == "school")
        {SetJoinScools();}
        // 個人戦を含むかどうか
        if (eventObj.eventType == "all" || eventObj.eventType == "member")
        {SetJoinMembers();}
    }

    private string GenerateEventAddTargetRangeId()
    {
        switch (eventObj.filterType)
        {
            default:
            case "countryRank":
                return "00";
            case "regionRank":
                return region;
            case "placeRank":
                return place;
            case "cityRank":
                return city;
        }
    }

    private void SetJoinScools()
    {
        List<string> schoolIdList = new List<string>();
        Dictionary<int, string> targetRanking = null;
        YearRanking thisYearRanking = GameData.instance.rankingManager.GetYearRanking(GameData.instance.storyDate.Year);
        targetRanking = thisYearRanking.GetRanking(targetRangeId, eventObj.filterType).school;
        for(int i = 1; i < targetRanking.Count + 1; i++)
        {
            schoolIdList.Add(targetRanking[i]);
            if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
        }
        joinSchoolList = GameData.instance.schoolManager.GetApplyJoinEventMembersNumSchools(schoolIdList);
    }

    private void SetJoinMembers()
    {
        Dictionary<int, string> targetRanking = null;
        YearRanking thisYearRanking = GameData.instance.rankingManager.GetYearRanking(GameData.instance.storyDate.Year);
        // 他校取得範囲決め
        switch (eventObj.filterType)
        {
            // ランキングに存在する条件の場合取得
            default:
            case "countryRank":
                targetRanking = thisYearRanking.GetCountryRanking("0").members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetCountryRanking("0").members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCountryRanking("0").membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                break;

            case "regionRank":
                targetRanking = thisYearRanking.GetRegionRanking(region).members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetRegionRanking(region).members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetRegionRanking(region).membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                break;

            case "placeRank":
                targetRanking = thisYearRanking.GetPlaceRanking(place).members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetPlaceRanking(place).members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetPlaceRanking(place).membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                break;
            case "cityRank":
                targetRanking = thisYearRanking.GetCityRanking(city).members60;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember60List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                
                targetRanking = thisYearRanking.GetCityRanking(city).members66;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember66List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members73;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember73List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members81;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember81List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members90;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember90List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).members100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMember100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }

                targetRanking = thisYearRanking.GetCityRanking(city).membersOver100;
                for(int i = 1; i < targetRanking.Count + 1; i++)
                {
                    joinMemberOver100List.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
                    if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
                }
                break;
        }
    }

    // 総合力が高い順に学校を並べ替え
    public void SortSchoolTotalStatus()
    {
        List<School> tmpSchoolList = new List<School>();
        foreach (School school in joinSchoolList)
        {
            school.SetSchoolTotalStatus();
            tmpSchoolList.Add(school);
        }
        joinSchoolList = tmpSchoolList.OrderByDescending(school => school.totalStatus).ToList();
    }

    public List<PlayerManager> GetSortMemberTotalStatus(List<PlayerManager> targetList)
    {
        List<PlayerManager> tmpMemberList = new List<PlayerManager>();
        foreach (PlayerManager member in targetList)
        {
            member.SetTotalStatus();
            tmpMemberList.Add(member);
        }
        return tmpMemberList.OrderByDescending(member => member.totalStatus).ToList();
    }

    public List<List<T>> GenerateEventLeaderBoad<T>(List<T> targetList)
    {
        List<List<T>> leaderBoad = new List<List<T>>();
        List<List<T>> leaderBoadA = new List<List<T>>();
        List<List<T>> leaderBoadB = new List<List<T>>();
        List<List<T>> tmpLeaderBoadA = new List<List<T>>();
        List<List<T>> tmpLeaderBoadB = new List<List<T>>();

        int notSeedNum = 8;

        List<T> seedList = new List<T>();
        if (targetList.Count >= 128)
        {
            notSeedNum = 128;
        }
        else if (targetList.Count >= 64)
        {
            notSeedNum = 64;
        }
        else if (targetList.Count >= 32)
        {
            notSeedNum = 32;
        }
        else if (targetList.Count >= 16)
        {
            notSeedNum = 16;
        }

        int notSeedCount = targetList.Count % notSeedNum;

        T firstTarget = targetList[0];
        targetList.RemoveAt(0);
        T secondTarget = targetList[0];
        targetList.RemoveAt(0);

        // 残りをシャッフル
        for (int i = 0; i < targetList.Count; i++)
        {
            T tmp = targetList[i];
            int randomIndex = UnityEngine.Random.Range(i, targetList.Count);
            targetList[i] = targetList[randomIndex];
            targetList[randomIndex] = tmp;
        }

        targetList.Insert(0, firstTarget);
        targetList.Insert(0, secondTarget);

        // 子要素初期化
        List<T> matchList = new List<T>();
        // 組み合わせ


        for (int i = 0; i < notSeedCount * 2; i++)
        {
            matchList.Add(targetList[targetList.Count - 1]);
            if (matchList.Count == 2)
            {
                tmpLeaderBoadA.Add(matchList);
                matchList = new List<T>();
            }
            targetList.RemoveAt(targetList.Count - 1);
        }

        foreach (T target in targetList)
        {
            tmpLeaderBoadB.Add(new List<T>(){target});
        }

        for (int i = 0; i < notSeedNum; i++)
        {
            if (i < tmpLeaderBoadB.Count)
            {
                if (i % 2 == 0)
                {
                    leaderBoadB.Add(tmpLeaderBoadB[i]);
                }
                else
                {
                    leaderBoadA.Add(tmpLeaderBoadB[i]);
                }
            }
            if (i < tmpLeaderBoadA.Count)
            {
                if (i % 2 == 0)
                {
                    leaderBoadB.Add(tmpLeaderBoadA[i]);
                }
                else
                {
                    leaderBoadA.Add(tmpLeaderBoadA[i]);
                }
            }
        }

        if (leaderBoadA.Count > 0)
        {
            foreach (List<T> match in leaderBoadA)
            {
                leaderBoad.Add(match);
            }
        }
        if (leaderBoadB.Count > 0)
        {
            List<T> tmp = leaderBoadB[0];
            leaderBoadB[0] = leaderBoadB[leaderBoadB.Count - 1];
            leaderBoadB[leaderBoadB.Count - 1] = tmp;
            foreach (List<T> match in leaderBoadB)
            {
                leaderBoad.Add(match);
            }
        }


        return leaderBoad;
    }

    public List<School> DoMatchAllSchool(List<List<School>> targetBoad)
    {
        List<School> result = new List<School>();
        List<List<School>> matchList = new List<List<School>>(targetBoad);
        while (true)
        {
            List<School> winnerList = new List<School>();
            foreach (List<School> targetMatch in matchList)
            {
                if (targetMatch.Count == 1)
                {
                    winnerList.Add(targetMatch[0]);
                    {Debug.Log(string.Format("不戦勝: {0}", targetMatch[0].name));}
                    continue;
                }
                else
                {
                    // 試合をする
                    SchoolMatch schoolMatch = new SchoolMatch("s", targetMatch[0], targetMatch[1]);
                    winnerList.Add(schoolMatch.winner);
                    result.Add(schoolMatch.loser);
                    Debug.Log(string.Format("赤 {0}: {1} | {2} - {5} | 白 {3}: {4}", schoolMatch.red.name, schoolMatch.red.regularMemberStatus, schoolMatch.redWinCount, schoolMatch.white.name, schoolMatch.white.regularMemberStatus, schoolMatch.whiteWinCount));
                }
            }
            if (winnerList.Count == 1)
            {   
                // 優勝決まり
                result.Add(winnerList[0]);
                break;
            }

            List<School> nextMatch = new List<School>();
            matchList = new List<List<School>>();
            foreach (School winnerSchool in winnerList)
            {
                nextMatch.Add(winnerSchool);
                if (nextMatch.Count == 2)
                {
                    matchList.Add(nextMatch);
                    nextMatch = new List<School>();
                }
            }
        }
        result.Reverse();
        return result;
    }

    public List<PlayerManager> DoMatchAllMember(List<List<PlayerManager>> targetBoad)
    {
        List<PlayerManager> result = new List<PlayerManager>();
        List<List<PlayerManager>> matchList = new List<List<PlayerManager>>(targetBoad);
        int count = 1;
        int chidCount = 1;
        while (true)
        {
            Debug.Log(count + "回戦");
            List<PlayerManager> winnerList = new List<PlayerManager>();
            foreach (List<PlayerManager> targetMatch in matchList)
            {
                // 組み合わせが奇数の場合、第一試合をシード試合とする
                if (targetMatch.Count == 1)
                {
                    winnerList.Add(targetMatch[0]);
                    chidCount ++;
                    {Debug.Log(string.Format("不戦勝: {0} {1}", GameData.instance.schoolManager.GetSchool(targetMatch[0].schoolId).name, targetMatch[0].nameKaki));}
                    continue;
                }
                PlayerManager winner = null;
                PlayerManager loser = null;
                // 試合をする
                MemberMatch match = new MemberMatch("s", targetMatch[0], targetMatch[1], false);
                if (match.winnerFlag == 1)
                {
                    winner = match.red;
                    loser = match.white;
                }
                else
                {
                    winner = match.white;
                    loser = match.red;
                }
                winnerList.Add(winner);
                result.Add(loser);
                chidCount ++;
                Debug.Log(
                    string.Format("【赤】{0}: {1} -【白】{2}: {3}\n{4} {5} {6} {7}",
                        GameData.instance.schoolManager.GetSchool(targetMatch[0].schoolId).name,
                        targetMatch[0].nameKaki,
                        GameData.instance.schoolManager.GetSchool(targetMatch[1].schoolId).name,
                        targetMatch[1].nameKaki,
                        match.ChecMatchDetail().Values.ToList()[0],
                        match.ChecMatchDetail().Values.ToList()[1],
                        match.ChecMatchDetail().Values.ToList()[2],
                        match.ChecMatchDetail().Values.ToList()[3]
                    )
                );
            }
            if (winnerList.Count == 1)
            {   
                // 優勝決まり
                result.Add(winnerList[0]);
                break;
            }

            List<PlayerManager> nextMatch = new List<PlayerManager>();
            matchList = new List<List<PlayerManager>>();
            foreach (PlayerManager winnerMember in winnerList)
            {
                nextMatch.Add(winnerMember);
                if (nextMatch.Count == 2)
                {
                    matchList.Add(nextMatch);
                    nextMatch = new List<PlayerManager>();
                }
            }
            count ++ ;
        }
        result.Reverse();
        return result;
    }

    // 勝ったほうを0番に入れた配列を作成
    public List<School> DoMatchSchool(List<School> targets)
    {
        // 対戦相手がいない場合はそのまま返す
        if (targets.Count == 1)
        {
            return targets;
        }
        List<School> result = new List<School>();
        // ここで試合をする

        for (int i = 0; i < 5; i++)
        {
            List<PlayerManager> matchMember = new List<PlayerManager>(){ targets[0].regularMembers[i],  targets[1].regularMembers[i]};
            List<PlayerManager> resultMember = DoMatchMember(matchMember);
        }
        // test
        if(targets[0].totalStatus <= targets[1].totalStatus)
        {targets.Reverse(); result = targets;}
        result = targets;
        return result;
    }

    // 勝ったほうを0番に入れた配列を作成
    public List<PlayerManager> DoMatchMember(List<PlayerManager> targets)
    {
        if (targets.Count == 1)
        {
            return targets;
        }
        // 対戦相手がいない場合はそのまま返す
        List<PlayerManager> result = new List<PlayerManager>();
        // ここで試合をする
        // test
        if(targets[0].totalStatus <= targets[1].totalStatus)
        {targets.Reverse(); result = targets;}
        result = targets;
        return result;
    }

}
public class SchoolMatch
{
    string id;
    public School red;
    public School white;
    public List<MemberMatch> memberMatchList;
    public School winner;
    public School loser;
    public int redWinCount;
    public int whiteWinCount;

    public SchoolMatch (string id, School red, School white)
    {
        this.id = id;
        this.red = red;
        this.white = white;
        this.memberMatchList = new List<MemberMatch>();
        this.winner = null;
        this.loser = null;
        this.redWinCount = 0;
        this.whiteWinCount = 0;

        Fight();
    }

    private bool Fight()
    {
        int redSchoolPoint = 0;
        int whiteSchoolPoint = 0;
        bool isDraw = true;
        for (int i = 0; i < 5; i++)
        {
            string matchId = id + i.ToString("d2");
            // 勝敗同数かつ同点かつ大将の場合終わるまで
            if (redWinCount == whiteWinCount && redSchoolPoint == whiteSchoolPoint && i == 4) {
                isDraw = false;
            }
            // Debug.Log(string.Format("{0} vs {1}", red.regularMembers[i].nameKaki, white.regularMembers[i].nameKaki));
            MemberMatch match = new MemberMatch(matchId, red.regularMembers[i], white.regularMembers[i], isDraw);
            if(match.winnerFlag == 1)
            {   
                if(match.redFusensho > 0 || match.redIppon > 0)
                {
                    redSchoolPoint += 10;
                }
                if(match.redWazaari > 0)
                {
                    redSchoolPoint += 1;
                }
                redWinCount ++;
            }
            if(match.winnerFlag == 2)
            {
                if(match.whiteFusensho > 0 || match.whiteIppon > 0)
                {
                    whiteSchoolPoint += 10;
                }
                if(match.whiteWazaari > 0)
                {
                    whiteSchoolPoint += 1;
                }
                whiteWinCount ++;
            }
            memberMatchList.Add(match);

            Debug.Log(string.Format("{0} {1} {2} {3} {4}", i, match.ChecMatchDetail().Values.ToList()[0], match.ChecMatchDetail().Values.ToList()[1], match.ChecMatchDetail().Values.ToList()[2], match.ChecMatchDetail().Values.ToList()[3]));
        }

        if (redWinCount > whiteWinCount)
        {
            winner = red;
            loser = white;
        }
        else
        {
            winner = white;
            loser = red;
        }
        return true;
    }
}

public class MemberMatch
{
    public string id;
    public PlayerManager red;
    public PlayerManager white;
    public PlayerManager winner;
    public PlayerManager loser;

    // 0: 引き分け 1: redの勝ち 2: whiteの勝ち
    public int winnerFlag;
    public float defaultMatchTime = 180.0f;
    public float endMatchTime = 0.0f;
    public float timeSpeed = 1.0f;
    public int redFusensho;
    public int redIppon;
    public int redWazaari;
    public int redYuko;
    public int whiteFusensho;
    public int whiteIppon;
    public int whiteWazaari;
    public int whiteYuko;
    public int enchoCount;

    public Abillity winnerAbillity;


    public MemberMatch(string id, PlayerManager red, PlayerManager white, bool isDraw)
    {
        this.id = id;
        this.red = red;
        this.white = white;
        this.winner = null;
        this.loser = null;
        this.enchoCount = 0;
        bool endFight = false;
        while (!endFight)
        {
            this.redFusensho = 0;
            this.redIppon = 0;
            this.redWazaari = 0;
            this.redYuko = 0;
            
            this.whiteFusensho = 0;
            this.whiteIppon = 0;
            this.whiteWazaari = 0;
            this.whiteYuko = 0;

            this.winnerFlag = 0;
            this.winnerAbillity = null;
            endFight = Fight(isDraw);
            if(!endFight){this.enchoCount ++;};
        }
    }

    private bool Fight(bool isDraw)
    {
        System.Random r = new System.Random();
        if (red == null)
        {
            this.whiteFusensho ++;
            this.winnerFlag = 2;
            return true;
        }
        else if (white == null)
        {
            this.redFusensho ++;
            this.winnerFlag = 1;
            return true;
        }
        bool end = false;
        
        Abillity lastAffectiveRedAbillity = null;
        Abillity lastAffectiveWhiteAbillity = null;
        float matchTime = defaultMatchTime;
        while (!end)
        {
            Abillity redBaseAbility = red.abillities[r.Next(red.abillities.Count - 3, red.abillities.Count)];
            Abillity whiteBaseAbility = white.abillities[r.Next(red.abillities.Count - 3, white.abillities.Count)];
            Abillity redTurnAbility = red.abillities[r.Next(0, red.abillities.Count - 3)];
            Abillity whiteTurnAbility = white.abillities[r.Next(0, white.abillities.Count - 3)];

            int redAbilityStatusNum = (int)Math.Floor((double)(redBaseAbility.status + redTurnAbility.status) / 1000);
            int whiteAbilityStatusNum = (int)Math.Floor((double)(whiteBaseAbility.status + whiteTurnAbility.status) / 1000);


            float diffValue = redAbilityStatusNum - whiteAbilityStatusNum;

            switch (diffValue)
            {
                case >= 5:
                    // 赤一本
                    this.redIppon ++;
                    lastAffectiveRedAbillity = redTurnAbility;
                    break;
                case >= 3:
                    // 赤技あり
                    if (redTurnAbility.groupId != "6" && redTurnAbility.groupId != "7")
                    {
                        if (this.redWazaari > 0) {
                            this.redIppon ++;
                        }
                        else
                        {
                            this.redWazaari ++;
                        }
                        lastAffectiveRedAbillity = redTurnAbility;
                    }
                    break;
                case >= 2:
                    // 赤有効
                    if (redTurnAbility.groupId != "6" && redTurnAbility.groupId != "7")
                    {
                        this.redYuko ++;
                        lastAffectiveRedAbillity = redTurnAbility;
                    }
                    break;
                case <= -5:
                    // 白一本
                    this.whiteIppon ++;
                    lastAffectiveWhiteAbillity = whiteTurnAbility;
                    break;
                case <= -3:
                    // 白技あり
                    if (whiteTurnAbility.groupId != "6" && whiteTurnAbility.groupId != "7")
                    {
                        if (this.whiteWazaari > 0) {
                            this.whiteIppon ++;
                        }
                        else
                        {
                            this.whiteWazaari ++;
                        }
                        lastAffectiveWhiteAbillity = whiteTurnAbility;
                    }
                    break;
                case <= -2:
                    // 白有効
                    if (whiteTurnAbility.groupId != "6" && whiteTurnAbility.groupId != "7")
                    {
                        this.whiteYuko ++;
                        lastAffectiveWhiteAbillity = whiteTurnAbility;
                    }
                    break;
                default:
                    // 何もなし
                    break;
            }

            // 一回判定したらランダムで秒数追加
            endMatchTime += r.Next(3, 11);

            if (this.redIppon > 0 || this.whiteIppon > 0)
            {
                end = true;
            }
            endMatchTime += timeSpeed;
            if (endMatchTime >= defaultMatchTime)
            {
                endMatchTime = defaultMatchTime;
                end = true;
            }
            // ここでwait
        }

        // 有効ポイント比較
        if (this.redYuko > this.whiteYuko)
        {
            this.winnerFlag = 1;
            this.winnerAbillity = lastAffectiveRedAbillity;
            this.winner = red;
            this.loser = white;
        }
        else if (this.redYuko < this.whiteYuko)
        {
            this.winnerFlag = 2;
            this.winnerAbillity = lastAffectiveWhiteAbillity;
            this.winner = white;
            this.loser = red;
        }


        // 技ありポイント比較
        if (this.redWazaari > this.whiteWazaari)
        {
            this.winnerFlag = 1;
            this.winnerAbillity = lastAffectiveRedAbillity;
            this.winner = red;
            this.loser = white;
        }
        else if (this.redWazaari < this.whiteWazaari)
        {
            this.winnerFlag = 2;
            this.winnerAbillity = lastAffectiveWhiteAbillity;
            this.winner = white;
            this.loser = red;
        }
        
        // 一本ポイント比較
        if (this.redIppon > this.whiteIppon)
        {
            this.winnerFlag = 1;
            this.winnerAbillity = lastAffectiveRedAbillity;
            this.winner = red;
            this.loser = white;
        }
        else if (this.redIppon < this.whiteIppon)
        {
            this.winnerFlag = 2;
            this.winnerAbillity = lastAffectiveWhiteAbillity;
            this.winner = white;
            this.loser = red;
        }
        
        if (winnerFlag == 0 && !isDraw)
        {
            // 引き分け負荷は延長戦
            return false;
        }
        return true;
    }

    public Dictionary<string, string> ChecMatchDetail()
    {
        Dictionary<string, string> matchDetail = new Dictionary<string, string>();
        string winnerSchoolName = "";
        string pattern = "引き分け";
        int position = 0;
        string name = "";
        string time = "";
        string point = "";
        string waza = "";
        if (this.winnerFlag == 1)
        {
            pattern = "赤: {0} {1}年 {2}";
            position = this.winner.positionId;
            name = this.winner.nameKaki;
            time = this.endMatchTime.ToString();
            waza = this.winnerAbillity.name;
            winnerSchoolName = GameData.instance.schoolManager.GetSchool(this.winner.schoolId).name;
            if (this.redYuko > 0)
            {
                point = "有効";
            }
            if (this.redWazaari > 0)
            {
                point = "技あり";
            }
            if (this.redIppon > 0)
            {
                point = "一本";
            }
            if (this.redFusensho > 0)
            {
                point = "不戦勝";
                time = "";
                waza = "";
            }
        }
        else if (this.winnerFlag == 2)
        {
            pattern = "白: {0} {1}年 {2}";
            position = this.winner.positionId;
            name = this.winner.nameKaki;
            time = this.endMatchTime.ToString();
            waza = this.winnerAbillity.name;
            winnerSchoolName = GameData.instance.schoolManager.GetSchool(this.winner.schoolId).name;
            if (this.whiteYuko > 0)
            {
                point = "有効";
            }
            if (this.whiteWazaari > 0)
            {
                point = "技あり";
            }
            if (this.whiteIppon > 0)
            {
                point = "一本";
            }
            if (this.whiteFusensho > 0)
            {
                point = "不戦勝";
                time = "";
                waza = "";
            }
        }

        matchDetail["result"] = string.Format(pattern, winnerSchoolName, position, name);
        matchDetail["time"] = time;
        matchDetail["point"] = point;
        matchDetail["waza"] = waza;

        return matchDetail;
    }
}