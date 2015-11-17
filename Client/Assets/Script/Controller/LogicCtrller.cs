public sealed class LogicCtrller
    {
    public static LogicCtrller Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new LogicCtrller();
            }
            return m_Instance;
        }
    }
    private static  LogicCtrller m_Instance;

    public ActorModule Actor { get; private set; }
    public LevelModule Level { get; private set; }

    public LogicCtrller()
    {
        this.Actor = new ActorModule();
        this.Level = new LevelModule();
    }
}
