using UnityEngine;

/// <summary>
/// One hazard, replaying the plan the generator wrote for it. It does not tick itself:
/// the director advances it with the run clock, so pausing stops every hazard at once
/// without touching Time.timeScale.
/// </summary>
public sealed class BarrierSweeper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;

    private BarrierPlan _plan;
    private float _travel;
    private float _width;
    private float _height;
    private float _baseY;

    public void Configure(BarrierPlan plan, float shaftWidth, float hazardHeight, float baseY)
    {
        this._plan = plan;
        this._width = shaftWidth * plan.WidthFraction;
        this._travel = Mathf.Max(0.0001f, shaftWidth - this._width);
        this._baseY = baseY;

        // The rotor is a round object, so it keeps a square footprint; the bar and the
        // shutter are 9-sliced strips that are meant to stretch across the shaft.
        this._height = plan.Kind == BarrierPlan.KindRotor ? this._width : hazardHeight;

        if (this._body == null)
        {
            return;
        }

        this._body.size = new Vector2(this._width, this._height);
        this._body.color = plan.Kind == BarrierPlan.KindSweep ? ShaftPalette.Danger : ShaftPalette.AccentArcane;
    }

    /// <summary>Half the hazard footprint, used by the director for the overlap test.</summary>
    public Vector2 HalfExtents
    {
        get { return new Vector2(this._width * 0.5f, this._height * 0.5f); }
    }

    public void Advance(float runTime)
    {
        if (this._plan == null)
        {
            return;
        }

        // Ping-pong across the shaft: Mathf.PingPong gives the turn-around for free, and
        // the phase offset means two hazards on the same route never march in lockstep.
        float distance = (runTime * this._plan.Speed) + (this._plan.Phase * this._travel * 2f);
        float offset = Mathf.PingPong(distance, this._travel) - (this._travel * 0.5f);
        this.transform.localPosition = new Vector3(offset * this._plan.Direction, this._baseY, 0f);

        if (this._body == null)
        {
            return;
        }

        if (this._plan.Kind == BarrierPlan.KindRotor)
        {
            this._body.transform.localRotation = Quaternion.Euler(0f, 0f, (runTime * 180f) % 360f);
        }
        else if (this._plan.Kind == BarrierPlan.KindGate)
        {
            float wave = 0.5f + (0.5f * Mathf.Sin(runTime * 4f));
            this._body.color = ShaftPalette.WithAlpha(ShaftPalette.AccentArcane, 0.55f + (0.45f * wave));
        }
    }
}
