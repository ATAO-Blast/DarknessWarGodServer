using PEProtocol;

public class TaskSys : SingletonPattern<TaskSys>
{
    private CacheSvc _cacheSvc;
    private CfgSvc _cfgSvc;
    public void Init()
    {
        _cacheSvc = CacheSvc.Instance;
        _cfgSvc = CfgSvc.Instance;
        PECommon.Log("TaskSys Init Done");
    }
    public void ReqTakeTaskReward(MsgPack msgPack)
    {
        ReqTakeTaskReward reqData = msgPack.msg.reqTakeTaskReward;

        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspTaskReward
        };

        PlayerData pd = _cacheSvc.GetPalyerDataBySession(msgPack.session);
        TaskRewardCfg trc = _cfgSvc.GetTaskRewardCfg(reqData.id);
        TaskRewardData trd = CalcTaskRewardData(pd,reqData.id);

        if (trd.prgs == trc.count && !trd.taked)
        {
            pd.coin += trc.coin;
            PECommon.CalcExp(pd,trc.exp);
            trd.taked = true;

            CalcTaskArr(pd, trd);

            if (!_cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspTaskReward = new RspTaskReward
                {
                    cion = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp,
                    taskArr = pd.taskArr,
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        msgPack.session.SendMsg(msg);
    }

    public TaskRewardData CalcTaskRewardData(PlayerData pd,int id)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            var rid = int.Parse(taskInfo[0]);
            if (rid == id)
            {
                trd = new TaskRewardData()
                {
                    ID = rid,
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1"),
                };
                break;
            }
        }
        return trd;
    }
    public void CalcTaskArr(PlayerData pd, TaskRewardData trd)
    {
        string result = trd.ID + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
        int index = -1;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskinfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskinfo[0]) == trd.ID)
            {
                index = i; break;
            }
        }
        pd.taskArr[index] = result;
    }
    public void CalcTaskPrgs(PlayerData pd, int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);
        TaskRewardCfg trc = _cfgSvc.GetTaskRewardCfg(tid);

        if (trd.prgs < trc.count)
        {
            trd.prgs += 1;

            //更新任务进度
            CalcTaskArr(pd, trd);

            ServerSession session = _cacheSvc.GetOnlineServerSession(pd.id);
            if (session != null)
            {
                session.SendMsg(new GameMsg
                {
                    cmd = (int)CMD.PshTaskPrgs,
                    pshTaskPrgs = new PshTaskPrgs
                    {
                        taskArr = pd.taskArr,
                    }
                });
            }
        }
    }
    public PshTaskPrgs GetTaskPrgs(PlayerData pd, int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);
        TaskRewardCfg trc = _cfgSvc.GetTaskRewardCfg(tid);

        if (trd.prgs < trc.count)
        {
            trd.prgs += 1;

            //更新任务进度
            CalcTaskArr(pd, trd);

            return new PshTaskPrgs
            {
                taskArr = pd.taskArr
            };
        }
        else return null;
    }
}

