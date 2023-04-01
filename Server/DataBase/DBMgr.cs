using MySql.Data.MySqlClient;
using PEProtocol;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;

public class DBMgr : SingletonPattern<DBMgr>
{
    private MySqlConnection conn = null;
    public void Init()
    {
        conn = new MySqlConnection("server=localhost;User Id = root;password =;Database = darkgod;Charset = utf8");
        conn.Open();
        PECommon.Log("DBMgr Init Done");

    }

    public PlayerData QueryPlayerData(string acct, string pass)
    {
        PlayerData playerData = null;
        MySqlDataReader mySqlDataReader = null;
        bool isNew = true;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select*from account where acct=@account", conn);
            cmd.Parameters.AddWithValue("account", acct);
            mySqlDataReader = cmd.ExecuteReader();

            if (mySqlDataReader.Read()) //注意如果这里使用if(mySqlDataReader.Read()),只能查找到第一条数据
            {
                isNew = false;
                string _pass = mySqlDataReader.GetString("pass");
                if (_pass.Equals(pass))
                {
                    playerData = new PlayerData()
                    {
                        id = mySqlDataReader.GetInt32("id"),
                        name = mySqlDataReader.GetString("name"),
                        lv = mySqlDataReader.GetInt32("level"),
                        exp = mySqlDataReader.GetInt32("exp"),
                        power = mySqlDataReader.GetInt32("power"),
                        coin = mySqlDataReader.GetInt32("coin"),
                        diamond = mySqlDataReader.GetInt32("diamond"),

                        hp = mySqlDataReader.GetInt32("hp"),
                        ad = mySqlDataReader.GetInt32("ad"),
                        ap = mySqlDataReader.GetInt32("ap"),
                        addef = mySqlDataReader.GetInt32("addef"),
                        apdef = mySqlDataReader.GetInt32("apdef"),
                        dodge = mySqlDataReader.GetInt32("dodge"),
                        pierce = mySqlDataReader.GetInt32("pierce"),
                        critical = mySqlDataReader.GetInt32("critical"),

                        guideid = mySqlDataReader.GetInt32("guideid")
                        //To add
                    };
                }
            }
        }
        catch (Exception ex)
        {
            PECommon.Log ("Query PalyerData By Acct&Pass Error: " + ex.Message,LogType.Error);
        }
        finally
        {
            if(mySqlDataReader != null) { mySqlDataReader.Close(); }//DataReader需要先关闭，否则无法对数据库发送其他cmd
            if (isNew)
            {
                //不存在账号数据，创建新的默认账号数据，并返回
                playerData = new PlayerData()
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin  = 5000,
                    diamond = 500,

                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,

                    guideid = 1001
                    //To add
                };
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }
    /// <summary>
    /// 向数据库插入新账号
    /// </summary>
    /// <param name="acct">账号</param>
    /// <param name="pass">密码</param>
    /// <param name="playerData">用户数据</param>
    /// <returns>返回主键</returns>
    private int InsertNewAcctData(string acct,string pass,PlayerData playerData)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into account set acct=@acct,pass=@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond," +
                "hp=@hp,ad=@ad,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical," +
                "guideid=@guideid",
                conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name",playerData.name);
            cmd.Parameters.AddWithValue("level",playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);

            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);
            //To add
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception ex)
        {
            PECommon.Log("Insert PalyerData Error: " + ex.Message, LogType.Error);
        }
        return id;
    }

    public bool QueryNameData(string name)
    {
        bool isExist = false;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select*from account where name=@name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                isExist = true;
            }
        }
        catch (Exception ex)
        {
            PECommon.Log("Query Name Error: " + ex.Message, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        return isExist;
    }
    public bool UpdatePalyerData(int id,PlayerData playerData)
    {
        bool upDated = true;
        try
        {
            MySqlCommand cmd = new MySqlCommand(
                "update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond," +
                "hp=@hp,ad=@ad,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical," +
                "guideid=@guideid"+
                " where id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);

            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);

            //To add
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            PECommon.Log("Update PlayerData Error: " + ex.Message, LogType.Error);
        }
        return upDated;
    }
}

