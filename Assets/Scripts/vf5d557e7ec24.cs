
public sealed class vf5d557e7ec24
{
    public vf5d557e7ec24(int sections)
    {
        this.v0ff36440fbbc = new v5dc5c793ebd9[sections + 1];
        for (int i = 0; i <= sections; i++)
        {
            this.v0ff36440fbbc[i] = new v5dc5c793ebd9();
        }
    }

    public int Seed;

    public int RouteIndex;

    
    public v5dc5c793ebd9[] v0ff36440fbbc;

    public int TopSection
    {
        get { return this.v0ff36440fbbc.Length - 1; }
    }
}
