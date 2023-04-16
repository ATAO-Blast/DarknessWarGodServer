using PENet;
using PEProtocol;
using System.Collections.Generic;

public class ChatSys:SingletonPattern<ChatSys>
{
    private CacheSvc _cacheSvc;
    public void Init()
    {
        _cacheSvc = CacheSvc.Instance;
        PECommon.Log("StrongSys Init Done");
    }

    public void SendChat(MsgPack msgPack)
    {
        SndChat data = msgPack.msg.sndChat;
        PlayerData pd = _cacheSvc.GetPalyerDataBySession(msgPack.session);
        TaskSys.Instance.CalcTaskPrgs(pd, 6);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshChat,

            pshChat = new PshChat
            {
                name = pd.name,
                chat = data.chat,
            }
        };
        //广播所有在线客户端
        List<ServerSession> serverSessions = _cacheSvc.GetOnlineServerSessions();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        serverSessions.ForEach(session =>
        {
            session.SendMsg(bytes);
        });
    }
}

