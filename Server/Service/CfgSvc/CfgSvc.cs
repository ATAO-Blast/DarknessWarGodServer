//using MySql.Data;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class CfgSvc : SingletonPattern<CfgSvc>
{
    public void Init()
    {
        InitGuideCfg();
        InitStrongCfg();
        InitTaskRewardCfg();
        PECommon.Log("CfgSvc Init Done.");
    }

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideCfgDataDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg()
    {
        var guideCfgAsset = File.ReadAllText(@"E:\UnityProjects\Plane_DarknessWarGodLearning\Assets\Resources\ResCfgs\guide.xml");
        if (guideCfgAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(guideCfgAsset);

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement ele = xmlNodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                AutoGuideCfg autoGuideCfg = new AutoGuideCfg { ID = ID };

                foreach (XmlElement element in ele.ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "coin":
                            autoGuideCfg.coin = int.Parse(element.InnerText);
                            break;
                        case "exp":
                            autoGuideCfg.exp = int.Parse(element.InnerText);
                            break;
                    }
                }
                guideCfgDataDic.Add(ID, autoGuideCfg);
            }
            PECommon.Log("GuideCfg Init Done");
        }
    }

    public AutoGuideCfg GetAutoGuideCfg(int ID)
    {
        AutoGuideCfg autoGuideCfg = null;
        if (guideCfgDataDic.TryGetValue(ID, out autoGuideCfg))
        {
            return autoGuideCfg;
        };
        return null;
    }
    #endregion

    #region 强化系统配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongCfgDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg()
    {
        var strongCfgAsset = File.ReadAllText(@"E:\UnityProjects\Plane_DarknessWarGodLearning\Assets\Resources\ResCfgs\strong.xml");
        if (strongCfgAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strongCfgAsset);

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement ele = xmlNodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                StrongCfg strongCfg = new StrongCfg { ID = ID };

                foreach (XmlElement element in ele.ChildNodes)
                {
                    int val = int.Parse(element.InnerText);
                    switch (element.Name)
                    {
                        case "pos":
                            strongCfg.pos = val;
                            break;
                        case "starlv":
                            strongCfg.starlv = val;
                            break;
                        case "addhp":
                            strongCfg.addhp = val;
                            break;
                        case "addhurt":
                            strongCfg.addhurt = val;
                            break;
                        case "adddef":
                            strongCfg.adddef = val;
                            break;
                        case "minlv":
                            strongCfg.minlv = val;
                            break;
                        case "coin":
                            strongCfg.coin = val;
                            break;
                        case "crystal":
                            strongCfg.crystal = val;
                            break;

                    }
                }
                Dictionary<int, StrongCfg> dic = null;
                if (strongCfgDic.TryGetValue(strongCfg.pos, out dic))
                {
                    dic.Add(strongCfg.starlv, strongCfg);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(strongCfg.starlv, strongCfg);

                    strongCfgDic.Add(strongCfg.pos, dic);
                }
            }
            PECommon.Log("StrongCfg Init Done");
        }

    }

    public StrongCfg GetStrongCfg(int pos, int starlv)
    {
        StrongCfg strongCfg = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongCfgDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(starlv))
            {
                strongCfg = dic[starlv];
            }
        }
        return strongCfg;
    }
    #endregion
    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardDataDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg()
    {
        var guideCfgAsset = File.ReadAllText(@"E:\UnityProjects\Plane_DarknessWarGodLearning\Assets\Resources\ResCfgs\taskreward.xml");
        if (guideCfgAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(guideCfgAsset);

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement ele = xmlNodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                TaskRewardCfg taskRewardCfg = new TaskRewardCfg { ID = ID };

                foreach (XmlElement element in ele.ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "coin":
                            taskRewardCfg.coin = int.Parse(element.InnerText);
                            break;
                        case "exp":
                            taskRewardCfg.exp = int.Parse(element.InnerText);
                            break;
                        case "count":
                            taskRewardCfg.count = int.Parse(element.InnerText);
                            break;
                    }
                }
                taskRewardDataDic.Add(ID, taskRewardCfg);
            }
            PECommon.Log("TaskRewardCfg Init Done");
        }
    }

    public TaskRewardCfg GetTaskRewardCfg(int ID)
    {
        TaskRewardCfg taskRewardCfg = null;
        if (taskRewardDataDic.TryGetValue(ID, out taskRewardCfg))
        {
            return taskRewardCfg;
        };
        return null;
    }
    #endregion
}

