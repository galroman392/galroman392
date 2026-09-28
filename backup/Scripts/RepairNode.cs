using UnityEngine;

/// <summary>
/// Wall capsule that gives a shield pip back when the core locks beside it. This is the
/// only way integrity ever goes up, and it is earned by a player action - never by a timer.
/// </summary>
public sealed class RepairNode : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;

    private float _pulse;
    private float _baseSize = 1f;
    private bool _spent;

    /// <summary>Which floor of the shaft this node sits on.</summary>
    public int SectionIndex { get; private set; }

    public bool Spent
    {
        get { return this._spent; }
    }

    public void Configure(int sectionIndex, float diameter)
    {
        this.SectionIndex = sectionIndex;
        this._baseSize = diameter;
        if (this._body != null)
        {
            this._body.size = new Vector2(this._baseSize, this._baseSize);
            this._body.color = ShaftPalette.AccentPrimary;
        }
    }

    public void Consume()
    {
        this._spent = true;
        if (this._body != null)
        {
            this._body.color = ShaftPalette.Dormant;
            this._body.size = new Vector2(this._baseSize * 0.8f, this._baseSize * 0.8f);
        }
    }

    public void Animate(float deltaTime)
    {
        if (this._spent || this._body == null)
        {
            return;
        }

        this._pulse += deltaTime;
        float wave = 0.5f + (0.5f * Mathf.Sin(this._pulse * 3.8f));
        this._body.color = Color.Lerp(ShaftPalette.AccentPrimary, ShaftPalette.AccentRoyal, wave * 0.6f);
        float side = this._baseSize * (1f + (0.06f * wave));
        this._body.size = new Vector2(side, side);
    }
}
