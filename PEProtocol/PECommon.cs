using PENet;
using PEProtocol;

public enum LogType
{
    Log = 0,
    Warning = 1,
    Error = 2,
    Info = 3
}
public class PECommon
{
    public static void Log(string message = "", LogType logType = LogType.Log)
    {
        LogLevel lv = (LogLevel)logType;
        PETool.LogMsg(message, lv);
    }
    public static int GetFightByProps(PlayerData playerData)
    {
        return playerData.lv*100+playerData.ad+playerData.ap+playerData.addef+playerData.apdef;
    }
    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10 )* 150 + 150;
    }
    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }
    /// <summary>
    /// 增加体力值的间隔，单位：分钟
    /// </summary>
    public const int PowerAddInterval = 5;
    /// <summary>
    /// 增加多少体力值
    /// </summary>
    public const int PowerAddNum = 2;

    public static void CalcExp(PlayerData playerData, int addExp)
    {
        int curLv = playerData.lv;
        int curExp = playerData.exp;
        int addRestExp = addExp;
        while (true)
        {
            int upNeedExp = GetExpUpValByLv(curLv) - curExp;
            if (addRestExp >= upNeedExp)
            {
                curLv += 1;
                curExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                playerData.lv = curLv;
                playerData.exp = curExp + addRestExp;
                break;
            }
        }
    }
}

