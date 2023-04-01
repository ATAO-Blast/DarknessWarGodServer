namespace PEProtocol
{
    [System.Serializable]
    public class GameMsg : PENet.PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;
    }
    #region 登录相关
    [System.Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }
    [System.Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;
        /// <summary>
        /// 穿透比率
        /// </summary>
        public int pierce;
        /// <summary>
        /// 暴击概率
        /// </summary>
        public int critical;

        public int guideid;
        //To Add

    }

    [System.Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [System.Serializable]
    public class ReqRename
    {
        public string name;
    }

    [System.Serializable]
    public class RspRename
    {
        public string name;
    }
    #endregion

    #region 引导相关
    [System.Serializable]
    public class ReqGuide
    {
        public int guideid;
    }
    [System.Serializable]
    public class RspGuide
    {
        public int guideid;
        public int coin;//具体奖励由服务端发送，客户端不进行关键奖励的计算
        public int lv;
        public int exp;
    }
    #endregion
    public enum ErrorCode
    {
        None = 0,//没有错误
        AcctIsOnline = 1,//账号已经上线
        WrongPass = 2,//密码错误
        NameIsExist = 3,//名字已经存在
        UpdateDBError = 4,//更新数据库出错
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104
    }
    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
