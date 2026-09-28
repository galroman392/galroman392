/// <summary>A whole generated shaft: section 0 is the start socket, section 18 the beacon.</summary>
public sealed class ShaftLayout
{
    public ShaftLayout(int sections)
    {
        this.Sections = new SectionPlan[sections + 1];
        for (int i = 0; i <= sections; i++)
        {
            this.Sections[i] = new SectionPlan();
        }
    }

    public int Seed;

    public int RouteIndex;

    /// <summary>Indexed 0..SECTIONS; index 0 is the launch socket.</summary>
    public SectionPlan[] Sections;

    public int TopSection
    {
        get { return this.Sections.Length - 1; }
    }
}
