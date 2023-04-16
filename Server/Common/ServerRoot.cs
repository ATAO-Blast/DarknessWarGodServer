public class ServerRoot : SingletonPattern<ServerRoot>
{
    public void Init()
    {
        //数据库层
        DBMgr.Instance.Init();
        //服务层
        CfgSvc.Instance.Init();
        CacheSvc.Instance.Init();
        NetSvc.Instance.Init();
        TimerSvc.Instance.Init();
        //业务系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
    }
    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int sessionId = 0;
    public int GetSessionID()
    {
        if(sessionId == int.MinValue) { sessionId = 0; }
        return sessionId += 1;
    }
}

