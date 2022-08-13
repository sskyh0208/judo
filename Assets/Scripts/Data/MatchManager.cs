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
    public string tournamentId;
    public string placeId;
    public string region;
    public string place;
    public string city;
    public string targetRangeId;

    public List<List<School>> teamMatchLeaderBoad;
    public List<List<PlayerManager>> weightClass60LeaderBoad;
    public List<List<PlayerManager>> weightClass66LeaderBoad;
    public List<List<PlayerManager>> weightClass73LeaderBoad;
    public List<List<PlayerManager>> weightClass81LeaderBoad;
    public List<List<PlayerManager>> weightClass90LeaderBoad;
    public List<List<PlayerManager>> weightClass100LeaderBoad;
    public List<List<PlayerManager>> weightClassOver100LeaderBoad;

    public Tournament (Schedule eventObj, string placeId)
    {
        this.eventObj = eventObj;
        this.tournamentId = 
        this.placeId = placeId;
        this.region = placeId.Substring(0, 2);
        this.place = placeId.Substring(2, 2);
        this.city = placeId.Substring(2, 4);
        this.targetRangeId = GenerateEventAddTargetRangeId();
        this.teamMatchLeaderBoad = new List<List<School>>();
        this.weightClass60LeaderBoad = new List<List<PlayerManager>>();
        this.weightClass66LeaderBoad = new List<List<PlayerManager>>();
        this.weightClass73LeaderBoad = new List<List<PlayerManager>>();
        this.weightClass81LeaderBoad = new List<List<PlayerManager>>();
        this.weightClass90LeaderBoad = new List<List<PlayerManager>>();
        this.weightClass100LeaderBoad = new List<List<PlayerManager>>();
        this.weightClassOver100LeaderBoad = new List<List<PlayerManager>>();
    }

    public bool CheckTeamMatch()
    {
        // 団体戦を含むかどうかかどうか
        if (eventObj.eventType == "all" || eventObj.eventType == "school")
        {
            return true;
        }
        return false;
    }

    public bool CheckIndividualMatch()
    {
        // // 個人戦を含むかどうか
        if (eventObj.eventType == "all" || eventObj.eventType == "member")
        {
            return true;
        }
        return false;
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

    private List<List<School>> GetTeamMatchList()
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

        List<School>JoinSchoolList = new List<School>(GameData.instance.schoolManager.GetApplyJoinEventMembersNumSchools(schoolIdList));

        // 全員参加可能な場合強い順位並べ替え
        if (eventObj.filterValue == 0)
        {
            JoinSchoolList = new List<School>(SortSchoolTotalStatus(JoinSchoolList));
        }

        return GenerateEventLeaderBoad(JoinSchoolList);
    }

    private List<List<PlayerManager>> GetIndividualMatchList(string weightClass)
    {
        YearRanking thisYearRanking = GameData.instance.rankingManager.GetYearRanking(GameData.instance.storyDate.Year);
        Ranking ranking = thisYearRanking.GetRanking(targetRangeId, eventObj.filterType);
        List<PlayerManager> targetMembers = new List<PlayerManager>();
        Dictionary<int, string> targetRanking = null; 
        switch (weightClass)
        {
            case "60":
                targetRanking = ranking.members60;
                break;
            case "66":
                targetRanking = ranking.members66;
                break;
            case "73":
                targetRanking = ranking.members73;
                break;
            case "81":
                targetRanking = ranking.members81;
                break;
            case "90":
                targetRanking = ranking.members90;
                break;
            case "100":
                targetRanking = ranking.members100;
                break;
            case "Over100":
                targetRanking = ranking.membersOver100;
                break;
        }
        for(int i = 1; i < targetRanking.Count + 1; i++)
        {
            targetMembers.Add(GameData.instance.schoolManager.GetSchool(targetRanking[i].Substring(4, 9)).GetMember(targetRanking[i]));
            if(i >= eventObj.filterValue && eventObj.filterValue != 0) {break;}
        }

        // 全員参加可能は強い順に並べ替え
        if (eventObj.filterValue == 0)
        {
            targetMembers = SortMemberTotalStatus(targetMembers);
        }

        return GenerateEventLeaderBoad<PlayerManager>(targetMembers);
    }

    // 総合力が高い順に学校を並べ替え
    public List<School> SortSchoolTotalStatus(List<School> target)
    {
        List<School> tmpSchoolList = new List<School>();
        foreach (School school in target)
        {
            school.SetSchoolTotalStatus();
            tmpSchoolList.Add(school);
        }
        return tmpSchoolList.OrderByDescending(school => school.totalStatus).ToList();
    }

    public List<PlayerManager> SortMemberTotalStatus(List<PlayerManager> targetList)
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
        Debug.Log("参加数: " + targetList.Count);
        List<List<T>> leaderBoad = new List<List<T>>();
        List<List<T>> leaderBoadA = new List<List<T>>();
        List<List<T>> leaderBoadB = new List<List<T>>();
        List<List<T>> tmpLeaderBoadA = new List<List<T>>();
        List<List<T>> tmpLeaderBoadB = new List<List<T>>();

        int notSeedNum = 8;

        List<T> seedList = new List<T>();
        if (targetList.Count >= 256)
        {
            notSeedNum = 256;
        }
        else if (targetList.Count >= 128)
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

        targetList.Reverse();
        for (int i = 0; i < notSeedCount * 2; i++)
        {
            matchList.Add(targetList[0]);
            if (matchList.Count == 2)
            {
                tmpLeaderBoadA.Add(matchList);
                matchList = new List<T>();
            }
            targetList.RemoveAt(0);
        }

        targetList.Reverse();
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

        Debug.Log("初回組み合わせ数: " + leaderBoad.Count);
        return leaderBoad;
    }

    public List<School> DoMatchAllSchool()
    {
        List<School> result = new List<School>();
        List<List<School>> matchList = GetTeamMatchList();
        while (true)
        {
            List<School> winnerList = new List<School>();
            foreach (List<School> targetMatch in matchList)
            {
                // 第一試合が1組をシード試合とする
                if (targetMatch.Count == 1)
                {
                    winnerList.Add(targetMatch[0]);
                    // {Debug.Log(string.Format("不戦勝: {0}", targetMatch[0].name));}
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

    public List<PlayerManager> DoIndividualMatch(string weightClass)
    {
        List<PlayerManager> result = new List<PlayerManager>();
        List<List<PlayerManager>> matchList = GetIndividualMatchList(weightClass);

        int count = 1;
        int chidCount = 1;
        while (true)
        {
            Debug.Log(count + "回戦");
            List<PlayerManager> winnerList = new List<PlayerManager>();
            foreach (List<PlayerManager> targetMatch in matchList)
            {
                MemberMatch match = null;
                // 第一試合が1組をシード試合とする
                if (targetMatch.Count == 1)
                {
                    targetMatch.Add(null);
                }
                PlayerManager winner = null;
                PlayerManager loser = null;
                // 試合をする
                match = new MemberMatch("s", targetMatch[0], targetMatch[1], false);
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
                Dictionary<string, string> matchDetail = match.ChecMatchDetail();
                if(loser != null)
                {
                    result.Add(loser);
                    Debug.Log(
                        string.Format(
                            "{0} {1} vs {2} {3}\n{4}",
                            GameData.instance.schoolManager.GetSchool(winner.schoolId).name, winner.nameKaki,
                            GameData.instance.schoolManager.GetSchool(loser.schoolId).name, loser.nameKaki,
                            matchDetail["formatString"]
                        )
                    );
                }
                else
                {
                    Debug.Log(matchDetail["formatString"]);
                }
                chidCount ++;
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

            Dictionary<string, string> matchDetail = match.ChecMatchDetail();
            Debug.Log(
                string.Format(
                    "{0} {1}",
                    i,
                    matchDetail["formatString"]
                )
            );
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
        if (white == null)
        {
            this.winner = red;
            endFight = true;
            this.redFusensho ++;
            this.winnerFlag = 1;
        }
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

            // 一回判定したらランダムで秒数追加
            endMatchTime += r.Next(2, 5);
            endMatchTime += timeSpeed;
            switch (diffValue)
            {
                case >= 5:
                    // 赤一本
                    this.redIppon ++;
                    lastAffectiveRedAbillity = redTurnAbility;
                    if (redTurnAbility.groupId == "5")
                    {
                        // 寝技の場合時間調整
                        endMatchTime += 30;
                    }
                    break;
                case >= 3:
                    // 赤技あり
                    if (redTurnAbility.groupId != "6" || redTurnAbility.groupId != "7")
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
                    if (redTurnAbility.groupId == "5")
                    {
                        endMatchTime += 25;
                    }
                    break;
                case >= 2:
                    // 赤有効
                    if (redTurnAbility.groupId != "6" && redTurnAbility.groupId != "7")
                    {
                        this.redYuko ++;
                        lastAffectiveRedAbillity = redTurnAbility;
                    }
                    if (redTurnAbility.groupId == "5")
                    {
                        endMatchTime += 20;
                    }
                    break;
                case <= -5:
                    // 白一本
                    this.whiteIppon ++;
                    lastAffectiveWhiteAbillity = whiteTurnAbility;
                    if (whiteTurnAbility.groupId == "5")
                    {
                        // 寝技の場合時間調整
                        endMatchTime += 30;
                    }
                    break;
                case <= -3:
                    // 白技あり
                    if  (whiteTurnAbility.groupId != "6" && whiteTurnAbility.groupId != "7")
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
                    if (whiteTurnAbility.groupId == "5")
                    {
                        endMatchTime += 25;
                    }
                    break;
                case <= -2:
                    // 白有効
                    if (whiteTurnAbility.groupId != "6" && whiteTurnAbility.groupId != "7")
                    {
                        this.whiteYuko ++;
                        lastAffectiveWhiteAbillity = whiteTurnAbility;
                    }
                    if (whiteTurnAbility.groupId == "5")
                    {
                        endMatchTime += 20;
                    }
                    break;
                default:
                    // 何もなし
                    break;
            }


            if (this.redIppon > 0 || this.whiteIppon > 0)
            {
                end = true;
                // 短いと不自然なので調整
                if (endMatchTime < 6)
                {
                    endMatchTime += 10;
                }
            }
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
        string winnerSide = "";
        string winnerSchool = "";
        string winnerName = "";
        string winnerPosition = "";
        string time = "";
        string point = "引き分け";
        string waza = "";
        string formatString = string.Format("{0}", point);
        if (this.winnerFlag == 1)
        {
            winnerSide = "赤";
            winnerSchool = GameData.instance.schoolManager.GetSchool(winner.schoolId).name;
            winnerName = winner.nameKaki;
            winnerPosition = winner.positionId.ToString();
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
            else
            {
                time = this.endMatchTime.ToString();
                waza = this.winnerAbillity.name;
            }
            formatString = string.Format(
                "{0} {1} {2}年 {3} {4}秒 {5} {6}",
                winnerSide,
                winnerSchool,
                winnerPosition,
                winnerName,
                time,
                point,
                waza
            );
        }
        else if (this.winnerFlag == 2)
        {
            winnerSide = "白";
            winnerSchool = GameData.instance.schoolManager.GetSchool(winner.schoolId).name;
            winnerName = winner.nameKaki;
            winnerPosition = winner.positionId.ToString();
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
            else
            {
                time = this.endMatchTime.ToString();
                waza = this.winnerAbillity.name;
            }
            formatString = string.Format(
                "{0} {1} {2}年 {3} {4}秒 {5} {6}",
                winnerSide,
                winnerSchool,
                winnerPosition,
                winnerName,
                time,
                point,
                waza
            );
        }

        matchDetail["winnerSide"] = winnerSide;
        matchDetail["winnerSchool"] = winnerSchool;
        matchDetail["winnerName"] = winnerName;
        matchDetail["winnerPosition"] = winnerPosition;
        matchDetail["time"] = time;
        matchDetail["point"] = point;
        matchDetail["waza"] = waza;
        matchDetail["formatString"] = formatString;

        return matchDetail;
    }
}