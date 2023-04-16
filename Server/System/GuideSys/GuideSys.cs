using PEProtocol;
public class GuideSys : SingletonPattern<GuideSys>
{
    private CacheSvc _cacheSvc;
    private CfgSvc _cfgSvc;

    public void Init()
    {
        PECommon.Log("GuideSys Init Done");
        _cacheSvc = CacheSvc.Instance;
        _cfgSvc = CfgSvc.Instance;
    }

    public void ReqGuide(MsgPack msgPack)
    {
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspGuide
        };

        ReqGuide reqGuideData = msgPack.msg.reqGuide;

        PlayerData pd = _cacheSvc.GetPalyerDataBySession(msgPack.session);
        AutoGuideCfg autoGuideCfg = _cfgSvc.GetAutoGuideCfg(reqGuideData.guideid);
        //更新GuideID：
        if(pd.guideid == reqGuideData.guideid)//防止玩家和服务器数据不一致
        {
            //检测是否为"智者点拨"任务
            if (pd.guideid == 1001)
            {
                TaskSys.Instance.CalcTaskPrgs(pd, 1);//第二个参数表示"智者点拨"任务的ID
            }
            pd.guideid += 1;
            //更新玩家数据
            pd.coin += autoGuideCfg.coin;
            PECommon.CalcExp(pd, autoGuideCfg.exp);

            if (!_cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspGuide = new RspGuide()
                {
                    coin = pd.coin,
                    exp = pd.exp,
                    guideid = pd.guideid,
                    lv = pd.lv,
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        msgPack.session.SendMsg(msg);
    }

}

