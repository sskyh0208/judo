using System;
using System.Collections.Generic;

public class MatchManager
{
}
public class SchoolMatch
{
    string id;
    School red;
    School white;
    public List<ResultMatch> resultMatchList;
    public School winner;
    public School loser;

    public SchoolMatch (string id, School red, School white)
    {
        this.id = id;
        this.red = red;
        this.white = white;
        this.resultMatchList = new List<ResultMatch>();
        this.winner = null;
        this.loser = null;

        Fight();
    }

    private void Fight()
    {
        List<School> resultSchools = new List<School>();
        
        int redSchoolPoint = 0;
        int whiteSchoolPoint = 0;
        int redWinCount = 0;
        int whiteWinCount = 0;
        for (int i = 0; i < 5; i++)
        {
            string matchId = id + i.ToString("d2");
            MemberMatch match = new MemberMatch(matchId, red.regularMembers[i], white.regularMembers[i]);
            resultMatchList.Add(match.resultMatch);
            if(match.resultMatch.winner.schoolId == red.id)
            {   
                if(match.resultMatch.tyepeId == "04")
                {
                    redSchoolPoint += Int16.Parse("00");
                }
                else
                {
                    redSchoolPoint += Int16.Parse(match.resultMatch.tyepeId);
                }
                redWinCount ++;
            }
            else
            {
                if(match.resultMatch.tyepeId == "04")
                {
                    whiteSchoolPoint += Int16.Parse("00");
                }
                else
                {
                    whiteSchoolPoint += Int16.Parse(match.resultMatch.tyepeId);
                }
                whiteWinCount ++;
            }
        }

        if (redSchoolPoint == whiteSchoolPoint)
        {
            // 同点はどちらかが勝つまで大将同士でやりあう
            while (true)
            {
                // 延長戦IDにする
                string matchId = id + 5.ToString("d2");
                MemberMatch match = new MemberMatch(matchId, red.regularMembers[4], white.regularMembers[4]);
                if (match.resultMatch.tyepeId == "03")
                {
                    continue;
                }
                if(match.resultMatch.winner.schoolId == red.id)
                {
                    redWinCount ++;
                }
                else
                {
                    whiteWinCount ++;
                }
                resultMatchList.Add(match.resultMatch);
                break;
            }
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
    }
}

public class MemberMatch
{
    string id;
    PlayerManager red;
    PlayerManager white;
    public ResultMatch resultMatch;
    // float defaultMatchTime = 180.0f;
    float endMatchTime = 0.0f;
    List<string> redIppon;
    List<string> redWazaari;
    List<string> redYuko;
    List<string> whiteIppon;
    List<string> whiteWazaari;
    List<string> whiteYuko;

    public MemberMatch(string id, PlayerManager red, PlayerManager white)
    {
        this.id = id;
        this.red = red;
        this.redIppon = new List<string>();
        this.redWazaari = new List<string>();
        this.redYuko = new List<string>();
        
        this.white = white;
        this.whiteIppon = new List<string>();
        this.whiteWazaari = new List<string>();
        this.whiteYuko = new List<string>();
        Fight();
    }

    private void Fight()
    {
        if (red == null)
        {
            resultMatch = new ResultMatch(id, white, null, "04", "", endMatchTime);
        }
        else if (white == null)
        {
            resultMatch = new ResultMatch(id, red, null, "04", "", endMatchTime);
        }
        // string typeId = "03";
        string typeId = "00";
        string wazaId = "01";
        PlayerManager winner = red;
        PlayerManager loser = white;

        if (redIppon.Count > whiteIppon.Count)
        // 赤の一本勝ち
        {
            typeId = "00";
            wazaId = redIppon[0];
        }
        else if (redIppon.Count < whiteIppon.Count)
        // 白の一本勝ち
        {
            typeId = "00";
            wazaId = whiteIppon[0];
            winner = white;
            loser = red;
        }
        else
        {
            if (redWazaari.Count > whiteWazaari.Count)
            // 赤の技有勝ち
            {
                typeId = "01";
                wazaId = redWazaari[0];
            }
            else if (redWazaari.Count < whiteWazaari.Count)
            // 白の技有勝ち
            {
                typeId = "01";
                wazaId = whiteWazaari[0];
                winner = white;
                loser = red;
            }
            else
            {
                if (redYuko.Count > whiteYuko.Count)
                // 赤の有効勝ち
                {
                    if (redWazaari.Count > 0)
                    {
                        typeId = "01";
                        wazaId = redWazaari[0];
                    }
                    else
                    {
                        typeId = "02";
                        wazaId = redYuko[redYuko.Count - 1];
                    }
                }
                else if (redYuko.Count < whiteYuko.Count)
                {
                    winner = white;
                    loser = red;
                    if (whiteWazaari.Count > 0)
                    {
                        typeId = "01";
                        wazaId = whiteWazaari[0];
                    }
                    else
                    {
                        typeId = "02";
                        wazaId = whiteYuko[whiteYuko.Count - 1];
                    }
                }
            }
        }
        resultMatch = new ResultMatch(id, winner, loser, typeId, wazaId, endMatchTime);
    }
}

public class ResultMatch
{
    public string matchId;
    public PlayerManager winner;
    public PlayerManager loser;
    
    // 00:一本 01:技有 02:有効 03:引き分け 04:不戦勝
    public string tyepeId;
    public string wazaId;
    public float time;

    
    public ResultMatch(string matchId, PlayerManager winner, PlayerManager loser, string tyepeId, string wazaId, float time)
    {
        this.matchId = matchId;
        this.winner = winner;
        this.loser = loser;
        this.tyepeId = tyepeId;
        this.wazaId = wazaId;
        this.time = time;
    }
}