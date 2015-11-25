using System;
using System.Collections.Generic;
using System.Data;

public sealed class SkillConfig
{
    private Dictionary<int, ActorSkillData> m_ActorSkillDic;
    private Dictionary<int, ActiveSkillData> m_ActiveSkillDic;
    private Dictionary<int, PassiveSkillData> m_PassiveSkillDic;
    private Dictionary<int, TriggerSkillData> m_TriggerSkillDic;
    private Dictionary<int, BuffData> m_BuffDic;

    public SkillConfig(DataTable actorSkillTable, DataTable activeSkillTable, DataTable passiveSkillTable, DataTable triggerSkillTable, DataTable buffTable)
    {
        this.ParseActorSkillTable(actorSkillTable);
        //2015.10.27:需要填表
        return;
        this.ParseActiveSkillTable(activeSkillTable);
        this.ParsePassiveSkillTable(passiveSkillTable);
        this.ParseTriggerSkillTable(triggerSkillTable);
        this.ParseBuffTable(buffTable);
    }

    private void ParseActorSkillTable(DataTable actorSkillTable)
    {
        this.m_ActorSkillDic = new Dictionary<int, ActorSkillData>();
        foreach (DataRow row in actorSkillTable.Rows)
        {
            ActorSkillData data = new ActorSkillData()
            {
                ID = int.Parse(row["ID"].ToString()),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Order = byte.Parse(row["Order"].ToString())
            };
            string[] strArr = row["SkillIDs"].ToString().Split(ConfigConst.SplitChar);
            data.SkillIDs = new int[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                data.SkillIDs[i] = int.Parse(strArr[i]);
            }
            this.m_ActorSkillDic.Add(data.ID, data);
        }

    }

    private void ParseActiveSkillTable(DataTable activeSkillTable)
    {
        this.m_ActiveSkillDic = new Dictionary<int, ActiveSkillData>();
        foreach (DataRow row in activeSkillTable.Rows)
        {
            ActiveSkillData data = new ActiveSkillData();
            this.ParseSkillDataBase(row, data);
            data.SelectionTarget = (SelectionTargetType)Enum.Parse(typeof(SelectionTargetType), row["SelectionTarget"].ToString());
            if (data.SelectionTarget == SelectionTargetType.Attribute
                || data.SelectionTarget == SelectionTargetType.MinAttribute
                || data.SelectionTarget == SelectionTargetType.MaxAttribute)
            {
                data.SelectionTargetRefrenceAttribute = row.IsNull("SelectionTargetRefrenceAttribute") ? (AttributeType?)null : (AttributeType)Enum.Parse(typeof(AttributeType), row["SelectionTargetRefrenceAttribute"].ToString());
            }
            data.SkillRange = (SkillRangeType)Enum.Parse(typeof(SkillRangeType), row["SkillRange"].ToString());
            string[] strArr = row["RangeValue"].ToString().Split(ConfigConst.SplitChar);
            data.RangeValue = new byte[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                data.RangeValue[i] = byte.Parse(strArr[i]);
            }
            data.CD = byte.Parse(row["CD"].ToString());
            data.CostRateOrValue = (RateOrValueType)Enum.Parse(typeof(RateOrValueType), row["CostRateOrValue"].ToString());
            data.CostValue = float.Parse(row["CostValue"].ToString());
            string[] buffs = row["TriggerBuffs"].ToString().Split(ConfigConst.SplitChar);
            data.TriggerBuffs = new int[buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                data.TriggerBuffs[i] = int.Parse(buffs[i]);
            }
            data.DamageType = (DamageType)Enum.Parse(typeof(DamageType), row["DamageType"].ToString());
            data.Fluctuation = float.Parse(row["Fluctuation"].ToString());
            data.DamageRate = row.IsNull("DamageRate") ? (float?)null : float.Parse(row["DamageRate"].ToString());
            data.RestoreHPeSource = row.IsNull("RestoreHPeSource") ? (bool?)null : byte.Parse(row["RestoreHPeSource"].ToString()) == 1;
            this.m_ActiveSkillDic.Add(data.ID, data);
        }
    }

    private void ParsePassiveSkillTable(DataTable passiveSkillTable)
    {
        this.m_PassiveSkillDic = new Dictionary<int, PassiveSkillData>();
        foreach (DataRow row in passiveSkillTable.Rows)
        {
            PassiveSkillData data = new PassiveSkillData();
            this.ParseSkillDataBase(row, data);
            this.m_PassiveSkillDic.Add(data.ID, data);
        }
    }

    private void ParseTriggerSkillTable(DataTable triggerSkillTable)
    {
        this.m_TriggerSkillDic = new Dictionary<int, TriggerSkillData>();
        foreach (DataRow row in triggerSkillTable.Rows)
        {
            TriggerSkillData data = new TriggerSkillData();
            this.ParseSkillDataBase(row, data);
            data.TriggerType = (TriggerType)Enum.Parse(typeof(TriggerType), row["TriggerType"].ToString());
            data.TriggerValue = byte.Parse(row["TriggerValue"].ToString());
            data.TriggerCondition = (TriggerConditionType)Enum.Parse(typeof(TriggerConditionType), row["TriggerCondition"].ToString());
            data.CompareTarget = (ActorType)Enum.Parse(typeof(ActorType), row["CompareTarget"].ToString());
            data.CompareRateOrVale = row.IsNull("CompareRateOrVale") ? (RateOrValueType?)null : (RateOrValueType)Enum.Parse(typeof(RateOrValueType), row["CompareRateOrVale"].ToString());
            data.CompareValue = row.IsNull("CompareValue") ? (float?)null : float.Parse(row["CompareRateOrVale"].ToString());
            data.TriggerSkill = row.IsNull("TriggerSkill") ? (int?)null : int.Parse(row["TriggerSkill"].ToString());
            data.TriggerBuff = row.IsNull("TriggerBuff") ? (int?)null : int.Parse(row["TriggerBuff"].ToString());
            this.m_TriggerSkillDic.Add(data.ID, data);
        }
    }

    private void ParseBuffTable(DataTable buffTable)
    {
        this.m_BuffDic = new Dictionary<int, BuffData>();
        foreach (DataRow row in buffTable.Rows)
        {
            BuffData data = new BuffData();
            this.ParseSkillDataBase(row, data);
            data.BuffType = (BuffType)Enum.Parse(typeof(BuffType), row["BuffType"].ToString());
            this.m_BuffDic.Add(data.ID, data);
        }
    }

    private void ParseSkillDataBase(DataRow row, SkillDataBase data)
    {
        data.ID = int.Parse(row["ID"].ToString());
        data.Name = row["Name"].ToString();
        data.Description = row["Description"].ToString();
        data.Icon = row["Icon"].ToString();
        data.EffectTarget = row.IsNull("EffectTarget") ? (ActorType?)null : (ActorType)Enum.Parse(typeof(ActorType), row["EffectTarget"].ToString());
        data.ChangeAttribute = row.IsNull("ChangeAttribute") ? (AttributeType?)null : (AttributeType)Enum.Parse(typeof(AttributeType), row["ChangeAttribute"].ToString());
        data.ChangeRateOrVale = row.IsNull("ChangeRateOrVale") ? (RateOrValueType?)null : (RateOrValueType)Enum.Parse(typeof(RateOrValueType), row["ChangeRateOrVale"].ToString());
        data.ChangeValue = row.IsNull("ChangeValue") ? (float?)null : float.Parse(row["ChangeValue"].ToString());
        data.ShieldType = row.IsNull("ShieldType") ? (ShieldType?)null : (ShieldType)Enum.Parse(typeof(ActorType), row["ShieldType"].ToString());
    }

    public ActorSkillData GetActorSkillDataByID(int id)
    {
        if (this.m_ActorSkillDic.ContainsKey(id))
        {
            return this.m_ActorSkillDic[id];
        }
        return null;
    }

    public SkillDataBase GetSkillDataBaseByID(int id)
    {
        if (this.m_ActiveSkillDic.ContainsKey(id))
        {
            return this.m_ActiveSkillDic[id];
        }
        if (this.m_PassiveSkillDic.ContainsKey(id))
        {
            return this.m_PassiveSkillDic[id];
        }
        if (this.m_TriggerSkillDic.ContainsKey(id))
        {
            return this.m_TriggerSkillDic[id];
        }
        if (this.m_BuffDic.ContainsKey(id))
        {
            return this.m_BuffDic[id];
        }
        return null;
    }

}
