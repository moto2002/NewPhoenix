public class GridData
{
    public byte XGrid { get; set; }
    public byte ZGrid { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is GridData)
        {
            GridData other =(GridData) obj ;
            return ( this.XGrid == other.XGrid) && (this.ZGrid == other.ZGrid);
        }
        return false;
    }
}
