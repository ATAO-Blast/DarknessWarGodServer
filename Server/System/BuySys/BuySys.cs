using PENet;
using PEProtocol;
public class BuySys : SingletonPattern<BuySys>
{
    CacheSvc _cacheSvc;
    public void Init()
    {
        _cacheSvc = CacheSvc.Instance;
        PECommon.Log("BuySys Inited");
    }
    public void ReqBuy(MsgPack msgPack)
    {
        ReqBuy data = msgPack.msg.reqBuy;
        PlayerData pd = _cacheSvc.GetPalyerDataBySession(msgPack.session);
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspBuy,
            
        };
        if (pd.diamond < data.cost)
        {
            msg.err = (int)ErrorCode.LackDiamond;
        }
        else
        {
            pd.diamond -= data.cost;
            PshTaskPrgs pshTaskPrgs = null;
            switch (data.buytype)
            {
                case 0:
                    pd.power += 100;//购买体力无视体力限制
                    pshTaskPrgs = TaskSys.Instance.GetTaskPrgs(pd, 4);
                    break;
                case 1:
                    pd.coin += 1000;
                    pshTaskPrgs = TaskSys.Instance.GetTaskPrgs(pd,5);
                    break;
            }
            if(_cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspBuy = new RspBuy
                {
                    buytype = data.buytype,
                    diamond = pd.diamond,
                    coin = pd.coin,
                    power = pd.power,
                };

                //并包处理
                msg.pshTaskPrgs = pshTaskPrgs;
            }
        }
        msgPack.session.SendMsg(msg);
    }
}

