using System.Data;
using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class ActorConfig
{
    private Dictionary<int, ActorData> m_ActorDic;
    public ActorConfig(DataTable actorTable)
    {
        this.ParseActorTable(actorTable);
    }

    private void ParseActorTable(DataTable actorTable)
    {
        this.m_ActorDic = new Dictionary<int, ActorData>();
        foreach (DataRow row in actorTable.Rows)
        {
            ActorData data = new ActorData()
            {
                ID = int.Parse(row["ID"].ToString()),

                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Icon = row["Icon"].ToString(),
                Texture = row["Texture"].ToString(),
                Model = true ? null : row["Model"].ToString(),
                Profession = row.IsNull("Profession") ? (ProfessionType?)null : (ProfessionType)Enum.Parse(typeof(ProfessionType), row["Profession"].ToString()),
                Nationality = row.IsNull("Nationality") ? (NationalityType?)null : (NationalityType)Enum.Parse(typeof(NationalityType), row["Nationality"].ToString()),
                Color = (ColorType)Enum.Parse(typeof(ColorType), row["Color"].ToString()),
                Sex = row.IsNull("Sex") ? (SexType?)null : (SexType)Enum.Parse(typeof(SexType), row["Sex"].ToString()),
                Quality = byte.Parse(row["Quality"].ToString()),
                LV = byte.Parse(row["LV"].ToString()),

                #region 1级属性
                L1MainAttribute = row.IsNull("L1MainAttribute") ? (AttributeType?)null : (AttributeType)Enum.Parse(typeof(AttributeType), row["L1MainAttribute"].ToString()),
                Power = int.Parse(row["Power"].ToString()),
                IQ = int.Parse(row["IQ"].ToString()),
                Agile = int.Parse(row["Agile"].ToString()),
                Physique = int.Parse(row["Physique"].ToString()),
                #endregion

                #region 2级属性
                HP = int.Parse(row["HP"].ToString()),
                AP = int.Parse(row["AP"].ToString()),
                PhysicsDEF = int.Parse(row["PhysicsDEF"].ToString()),
                MagicDEf = int.Parse(row["MagicDEf"].ToString()),
                Speed = int.Parse(row["Speed"].ToString()),
                Hit = float.Parse(row["Hit"].ToString()),
                Dodge = float.Parse(row["Dodge"].ToString()),
                Critical = float.Parse(row["Critical"].ToString()),
                OpposeCritical = float.Parse(row["OpposeCritical"].ToString()),
                CriticalDamge = float.Parse(row["CriticalDamge"].ToString()),
                CriticalDamgeCounteract = float.Parse(row["CriticalDamgeCounteract"].ToString()),
                Heal = float.Parse(row["Heal"].ToString()),
                BeHealed = float.Parse(row["BeHealed"].ToString()),
                Block = float.Parse(row["Block"].ToString()),
                Broken = float.Parse(row["Broken"].ToString()),
                SpecialAttribute = int.Parse(row["SpecialAttribute"].ToString()),
                #endregion
            };
            string[] weapons = row["EnableWeaponTypes"].ToString().Split(ConfigConst.SplitChar);
            data.EnableWeaponTypes = new WeaponType[weapons.Length];
            for (int i = 0; i < weapons.Length; i++)
            {
                data.EnableWeaponTypes[i] = (WeaponType)Enum.Parse(typeof(WeaponType), weapons[i]);
            }
            string[] skillStrs = row["Skills"].ToString().Split(ConfigConst.SplitChar);
            data.Skills = new int[skillStrs.Length];
            for (int i = 0; i < skillStrs.Length; i++)
            {
                data.Skills[i] = int.Parse(skillStrs[i]);
            }

            if (this.m_ActorDic.ContainsKey(data.ID))
            {
                Debug.LogError(actorTable.TableName + " 重复ID " + data.ID);
            }
            else
            {
                this.m_ActorDic.Add(data.ID, data);
            }
        }
    }

    public ActorData GetActorDataByID(int id)
    {
        if (this.m_ActorDic.ContainsKey(id))
        {
            return this.m_ActorDic[id];
        }
        Debug.Log(string.Format( "ID {0} not exist",id));
        return null;
    }
}
