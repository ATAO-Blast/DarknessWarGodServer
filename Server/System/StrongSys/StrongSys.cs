using PEProtocol;
public class StrongSys : SingletonPattern<StrongSys>
{
    private CacheSvc _cacheSvc;
    private CfgSvc _cfgSvc;

    public void Init()
    {
        _cfgSvc = CfgSvc.Instance;
        _cacheSvc = CacheSvc.Instance;
        PECommon.Log("StrongSys Init Done");
    }

    public void ReqStrong(MsgPack msgPack)
    {
        ReqStrong reqStrongData = msgPack.msg.reqStrong;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspStrong,
        };

        PlayerData pd = _cacheSvc.GetPalyerDataBySession(msgPack.session);

        int curStarLv = pd.strongArr[reqStrongData.pos];
        StrongCfg nextSd = _cfgSvc.GetStrongCfg(reqStrongData.pos, curStarLv+1);
        //条件判断
        if(pd.lv<nextSd.minlv)
        {
            msg.err = (int)ErrorCode.LackLevel;
        }
        else if(pd.coin<nextSd.coin)
        {
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if(pd.crystal<nextSd.crystal)
        {
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else
        {
            if (nextSd != null)
            {
                TaskSys.Instance.CalcTaskPrgs(pd, 3);
                //资源扣除
                pd.coin -= nextSd.coin;
                pd.crystal -= nextSd.crystal;
                pd.strongArr[reqStrongData.pos] += 1;
                //增加属性
                pd.hp += nextSd.addhp;
                pd.ad += nextSd.addhurt;
                pd.ap += nextSd.addhurt;
                pd.addef += nextSd.adddef;
                pd.apdef += nextSd.adddef;
                
                //更新数据库
                if (!_cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.rspStrong = new RspStrong()
                    {
                        coin = pd.coin,
                        crystal = pd.crystal,
                        hp = pd.hp,
                        ad = pd.ad,
                        ap = pd.ap,
                        addef = pd.addef,
                        apdef = pd.apdef,
                        strongArr = pd.strongArr
                    };
                }
            }
        }
        msgPack.session.SendMsg(msg);
    }
}

