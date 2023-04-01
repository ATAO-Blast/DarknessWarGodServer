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
}

