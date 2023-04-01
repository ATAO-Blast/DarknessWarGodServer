using PEProtocol;

public class LoginSys : SingletonPattern<LoginSys>
{
    private CacheSvc _cacheSvc;
    public void Init()
    {
        PECommon.Log("LoginSys Init Done");
        _cacheSvc = CacheSvc.Instance;
    }
    public void ReqLogin(MsgPack msgPack)
    {
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspLogin
        };
        ReqLogin data = msgPack.msg.reqLogin;
        //判断当前账号是否已经上线
        //已经上线：返回错误信息，禁止重复登录
        if(_cacheSvc.IsAcctOnLine(data.acct))
        {
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
        //未上线：
            PlayerData _playerData = _cacheSvc.GetPlayerData(data.acct, data.pass);
        //账号是否存在
            //密码错误
            if(_playerData == null)
            {
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                msg.rspLogin = new RspLogin
                {
                    playerData = _playerData
                };
                //缓存账号数据
                _cacheSvc.AcctOnline(data.acct, msgPack.session, _playerData);
            }

        }

        //回应客户端
        msgPack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack msgPack)
    {
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspRename
        };

        ReqRename reqRenameData = msgPack.msg.reqRename;
        //判断此名字是否已经存在
        if (_cacheSvc.IsNameExist(reqRenameData.name))
        {
            //存在：返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            //不存在：更新缓存以及数据库，再返回给客户端
            var playerData = _cacheSvc.GetPalyerDataBySession(msgPack.session);
            playerData.name = reqRenameData.name;
            if (!_cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = reqRenameData.name,
                };
            }
        }
        msgPack.session.SendMsg(msg);
    }
    public void ClearOfflieData(ServerSession session)
    {
        _cacheSvc.AcctOffLine(session);
    }
}

