using UnityEngine;

/// <summary>
/// A wall bracket the core can snap into. While the core sits here it is untouchable,
/// which is what makes a run with no input last forever instead of dying to a timer.
/// </summary>
public sealed class SupportSocket : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _flash;

    private float _flashTimer;
    private float _baseSize = 1f;

    /// <summary>Which floor of the shaft this socket belongs to.</summary>
    public int SectionIndex { get; private set; }

    /// <summary>0 = left wall, 1 = right wall.</summary>
    public int Side { get; private set; }

    public Vector3 DockPoint
    {
        get { return this.transform.position; }
    }

    public void Configure(int sectionIndex, int side, float socketWidth)
    {
        this.SectionIndex = sectionIndex;
        this.Side = side;
        this._baseSize = socketWidth;

        if (this._body != null)
        {
            this._body.size = new Vector2(this._baseSize, this._baseSize);
            // The art is drawn opening to the right; the left-wall copy is a deliberate mirror.
            this._body.flipX = side == 0;
            this._body.color = ShaftPalette.TextPrimary;
        }

        if (this._flash != null)
        {
            this._flash.size = new Vector2(this._baseSize * 1.3f, this._baseSize * 1.3f);
            this._flash.color = ShaftPalette.WithAlpha(ShaftPalette.AccentRoyal, 0f);
        }
    }

    public void Highlight()
    {
        this._flashTimer = 0.35f;
    }

    public void Animate(float deltaTime, bool occupied)
    {
        if (this._body != null)
        {
            this._body.color = occupied ? ShaftPalette.AccentRoyal : ShaftPalette.TextPrimary;
        }

        if (this._flash == null)
        {
            return;
        }

        if (this._flashTimer > 0f)
        {
            this._flashTimer = Mathf.Max(0f, this._flashTimer - deltaTime);
            float life = Mathf.Clamp01(this._flashTimer / 0.35f);
            float side = this._baseSize * (1.3f + (0.8f * (1f - life)));
            this._flash.color = ShaftPalette.WithAlpha(ShaftPalette.AccentRoyal, life * 0.9f);
            this._flash.size = new Vector2(side, side);
        }
        else if (this._flash.color.a > 0f)
        {
            this._flash.color = ShaftPalette.WithAlpha(ShaftPalette.AccentRoyal, 0f);
        }
    }
}
