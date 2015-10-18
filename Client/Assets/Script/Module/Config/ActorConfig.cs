using System.Data;
using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class ActorConfig
{
    private Dictionary<int, ActorData> m_ActorDic;
    public ActorConfig(DataTable table)
    {
        this.m_ActorDic = new Dictionary<int, ActorData>();
        foreach (DataRow row in table.Rows)
        {
            ActorData data = new ActorData();
            data.ID = int.Parse(row["ID"].ToString());
            data.Name = row["Name"].ToString();
            /*
            data.Description = row["Description"].ToString();
            data.Icon = row["Icon"].ToString();
            data.Texture = row["Texture"].ToString();
            data.Model = row["Model"].ToString();
            if (!row.IsNull("Profession"))
            {
                data.Profession = (ProfessionType)Enum.Parse(typeof(ProfessionType), row["Profession"].ToString());
            }
            if (!row.IsNull("Nationality"))
            {
                data.Nationality = (NationalityType)Enum.Parse(typeof(NationalityType), row["Nationality"].ToString());
            }
            data.Color = (ColorType)Enum.Parse(typeof(ColorType), row["Color"].ToString());
            if (!row.IsNull("Sex"))
            {
                data.Sex = (SexType)Enum.Parse(typeof(SexType), row["Sex"].ToString());
            }
            data.Quality = byte.Parse(row["Quality"].ToString());
            data.LV = byte.Parse(row["LV"].ToString());


            #region 1级属性
            if (!row.IsNull("L1MainAttribute"))
            {
                data.L1MainAttribute = (AttributeType)Enum.Parse(typeof(AttributeType), row["L1MainAttribute"].ToString());
            }
            data.Power = int.Parse(row["Power"].ToString());
            data.IQ = int.Parse(row["IQ"].ToString());
            data.Agile = int.Parse(row["Agile"].ToString());
            data.Physique = int.Parse(row["Physique"].ToString());
            #endregion

            #region 2级属性
            data.HP = int.Parse(row["HP"].ToString());
            data.AP = int.Parse(row["AP"].ToString());
            data.PhysicsDEF = int.Parse(row["PhysicsDEF"].ToString());
            data.MagicDEf = int.Parse(row["MagicDEf"].ToString());
            data.Speed = int.Parse(row["Speed"].ToString());
            data.Hit = float.Parse(row["Hit"].ToString());
            data.Dodge = float.Parse(row["Dodge"].ToString());
            data.Critical = float.Parse(row["Critical"].ToString());
            data.OpposeCritical = float.Parse(row["OpposeCritical"].ToString());
            data.CriticalDamge = float.Parse(row["CriticalDamge"].ToString());
            data.CriticalDamgeCounteract = float.Parse(row["CriticalDamgeCounteract"].ToString());
            data.Heal = float.Parse(row["Heal"].ToString());
            data.BeHealed = float.Parse(row["BeHealed"].ToString());
            data.Block = float.Parse(row["Block"].ToString());
            data.Broken = float.Parse(row["Broken"].ToString());
            data.SpecialAttribute = int.Parse(row["SpecialAttribute"].ToString());
            if (!row.IsNull("ShieldType"))
            {
                data.ShieldType = (ShieldType)Enum.Parse(typeof(ShieldType), row["ShieldType"].ToString());
            }
            #endregion
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
            */
            if (this.m_ActorDic.ContainsKey(data.ID))
            {
                Debug.LogError(table.TableName + " 重复ID " + data.ID);
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
