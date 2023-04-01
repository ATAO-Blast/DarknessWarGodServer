using PENet;
using PEProtocol;
public class ServerSession : PESession<GameMsg>
{
    public int sessionID = 0;
    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("SessionID: " + sessionID + " Client Connected");
    }
    protected override void OnReciveMsg(GameMsg msg)//多线程
    {
        PECommon.Log("SessionID: " + sessionID + "RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(this,msg);
    }
    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflieData(this);
        PECommon.Log("SessionID: " + sessionID + "Client DisConnected");
    }
}

