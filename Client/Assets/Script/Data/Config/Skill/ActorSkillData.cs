public class ActorSkillData
{
    public int ID;//技能每一阶的ID,最终填到角色表里
    public string Name;
    public string Description;
    public string Icon;
    public SkillType SkillType;
    public byte Order;//阶数[0,9]
    public int[] SkillIDs;//技能ID
}
