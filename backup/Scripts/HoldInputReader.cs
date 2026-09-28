using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The whole control scheme: one finger, held down. Reads the new Input System only
/// (the project ships with activeInputHandler = Input System, so legacy UnityEngine.Input
/// would throw at runtime), and ignores presses that land in the top strip so the
/// BACK / PAUSE buttons are never also read as "rise".
/// </summary>
public sealed class HoldInputReader : MonoBehaviour
{
    [SerializeField] private float _topDeadZoneFraction = 0.13f;

    private bool _wasHeld;

    /// <summary>A finger is down inside the play area right now.</summary>
    public bool IsHeld { get; private set; }

    /// <summary>The finger went down this tick.</summary>
    public bool WentDown { get; private set; }

    /// <summary>The finger came up this tick.</summary>
    public bool WentUp { get; private set; }

    /// <summary>How long the finger that just came up (or is still down) has been held.</summary>
    public float HoldSeconds { get; private set; }

    public void Tick(float deltaTime)
    {
        bool held = this.ReadHeld();

        this.WentDown = held && !this._wasHeld;
        this.WentUp = !held && this._wasHeld;

        if (this.WentDown)
        {
            this.HoldSeconds = 0f;
        }
        else if (held)
        {
            this.HoldSeconds += deltaTime;
        }

        this.IsHeld = held;
        this._wasHeld = held;
    }

    /// <summary>Drop any in-flight press, e.g. after unpausing.</summary>
    public void Clear()
    {
        this.IsHeld = false;
        this.WentDown = false;
        this.WentUp = false;
        this.HoldSeconds = 0f;
        this._wasHeld = this.ReadHeld();
    }

    private bool ReadHeld()
    {
        Pointer pointer = Pointer.current;
        if (pointer == null || !pointer.press.isPressed)
        {
            return false;
        }

        Vector2 position = pointer.position.ReadValue();
        float ceiling = Screen.height * (1f - Mathf.Clamp01(this._topDeadZoneFraction));
        return position.y <= ceiling;
    }
}
