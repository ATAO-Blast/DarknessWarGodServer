public class BaseData<T> where T : BaseData<T>
{
    public int ID;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int coin;
    public int exp;
}
public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}
public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public int count;
    public int exp;
    public int coin;

}
public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}