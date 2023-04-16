using PENet;
using PEProtocol;
using System.Collections.Generic;

public class PowerSys:SingletonPattern<PowerSys>
{
    CacheSvc _cacheSvc;
    TimerSvc _timerSvc;
    public void Init()
    {
        _cacheSvc = CacheSvc.Instance;
        _timerSvc = TimerSvc.Instance;

        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddInterval, PETimeUnit.Minute, 0);
        PECommon.Log("PowerSys Inited");
    }
    private void CalcPowerAdd(int tid)
    {
        //计算体力增长
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.PshPower
        };
        msg.pshPower = new PshPower();//提前new，这是一个群发的消息，这个消息只需要读取同一个引用即可
        //获取所有在线玩家获得实时的体力增长推送数据
        Dictionary<ServerSession,PlayerData> onlineDic = _cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic)
        {
            PlayerData pd = item.Value;
            ServerSession session = item.Key;

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if(pd.power>=powerMax)
            {
                continue;//体力最大时不用刷新时间戳
            }
            else
            {
                pd.power += PECommon.PowerAddNum;
                pd.time = _timerSvc.GetNowTime();
                if(pd.power >= powerMax)
                {
                    pd.power = powerMax;
                }
            }

            if(!_cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.pshPower.power = pd.power;
                session.SendMsg(msg);
            }
        }
    }
}

