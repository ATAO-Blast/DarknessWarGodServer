﻿using PEProtocol;
using System.Collections.Generic;

public class CacheSvc:SingletonPattern<CacheSvc>
{
    private Dictionary<string,ServerSession> onLineAcctDic = new Dictionary<string,ServerSession>();
    private Dictionary<ServerSession,PlayerData> onLineSessionDic = new Dictionary<ServerSession,PlayerData>();
    private DBMgr dbMgr = null;
    public void Init()
    {
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done");
    }
    public bool IsAcctOnLine(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }
    public void AcctOnline(string acct, ServerSession session,PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }
    public PlayerData GetPlayerData(string acct,string pass)
    {
        return dbMgr.QueryPlayerData(acct, pass); 
    }
    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }
    public PlayerData GetPalyerDataBySession(ServerSession session)
    {
        PlayerData playerData = null;
        if(onLineSessionDic.TryGetValue(session, out playerData))
        {
            return playerData;
        }
        return playerData;
    }
    public List<ServerSession> GetOnlineServerSessions()
    {
        List<ServerSession> lst = new List<ServerSession>();
        foreach(var item in onLineSessionDic)
        {
            lst.Add(item.Key);
        }
        return lst;
    }
    public ServerSession GetOnlineServerSession(int id)
    {
        ServerSession session = null;
        foreach (var item in onLineSessionDic)
        {
            if (item.Value.id == id)
            {
                session = item.Key;
                break;
            }
        }
        return session;
    }
    public bool UpdatePlayerData(int id,PlayerData playerData)
    {
        // To do
        return dbMgr.UpdatePalyerData(id,playerData);
    }
    public Dictionary<ServerSession, PlayerData> GetOnlineCache()
    {
        return onLineSessionDic;
    }

    public void AcctOffLine(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if (item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }
        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("Offline Result: SessionID: " + session.sessionID +" "+ succ);
    }
}

