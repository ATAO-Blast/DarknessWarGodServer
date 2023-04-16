namespace PEProtocol
{
    [System.Serializable]
    public class GameMsg : PENet.PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTaskReward rspTaskReward;

        public PshTaskPrgs pshTaskPrgs;
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
        public int crystal;

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

        /// <summary>
        /// 数组的index表示pos、数组的值表示starlv
        /// </summary>
        public int[] strongArr;

        public long time;
        public string[] taskArr;
        public int fuben;
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
    #region 强化相关
    [System.Serializable]
    public class ReqStrong
    {
        public int pos;
    }
    [System.Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion
    #region 聊天相关
    [System.Serializable]
    public class SndChat
    {
        public string chat;
    }
    [System.Serializable]
    public class PshChat
    {
        public string name;
        public string chat;
    }
    #endregion
    #region 资源交易相关
    [System.Serializable]
    public class ReqBuy
    {
        public int buytype;
        public int cost;
    }
    [System.Serializable]
    public class RspBuy
    {
        public int buytype;
        public int diamond;
        public int coin;
        public int power;
    }

    [System.Serializable]
    public class PshPower
    {
        public int power;
    }
    #endregion
    #region 任务奖励相关
    [System.Serializable]
    public class ReqTakeTaskReward
    {
        public int id;
    }
    [System.Serializable]
    public class RspTaskReward
    {
        public int cion;
        public int lv;
        public int exp;
        public string[] taskArr;
    }
    [System.Serializable]
    public class PshTaskPrgs
    {
        public string[] taskArr;
    }
    #endregion
    public enum ErrorCode
    {
        None = 0,//没有错误
        AcctIsOnline = 1,//账号已经上线
        WrongPass = 2,//密码错误
        NameIsExist = 3,//名字已经存在
        UpdateDBError = 4,//更新数据库出错
        ServerDataError = 5,//服务器与客户端数据不一致，客户端开挂，强制下线

        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,

        ClientDataError//客户端数据异常
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        //主城相关 200
        ReqGuide = 201,
        RspGuide = 202,

        ReqStrong = 203,
        RspStrong = 204,

        SndChat = 205,
        PshChat = 206,

        ReqBuy = 207,
        RspBuy = 208,

        PshPower = 209,

        ReqTakeTaskReward = 210,
        RspTaskReward = 211,

        PshTaskPrgs = 212
    }
    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
