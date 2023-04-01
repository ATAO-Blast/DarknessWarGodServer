using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}
public class NetSvc : SingletonPattern<NetSvc>
{
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();
    public static readonly string obj = "lock";
    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);

        PECommon.Log("NetSvc Init Done");//PELog会输出时间，并且给出日志级别
    }
    public void AddMsgQue(ServerSession session, GameMsg msg)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session,msg));
        }
    }
    public void Update()//也可以使用多线程Update，到涉及到修改数据的时候再加锁，这里简化了
    {
        if(msgPackQue.Count > 0)
        {
            //PECommon.Log("PackCount:"+msgPackQue.Count);
            lock (obj)
            {
                MsgPack msgPack = msgPackQue.Dequeue();
                HandOutMsg(msgPack);
            }
        }
    }
    private void HandOutMsg(MsgPack msgPack)
    {
        switch((CMD)msgPack.msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(msgPack);
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(msgPack);
                break;
        }
    }
}

