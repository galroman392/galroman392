using UnityEngine;
using UnityEngine.InputSystem;







public sealed class vb6b38774829e : MonoBehaviour
{
    [SerializeField] private float _topDeadZoneFraction = 0.13f;

    private bool v6ad63ee0de8f;

    
    public bool IsHeld { get; private set; }

    
    public bool WentDown { get; private set; }

    
    public bool WentUp { get; private set; }

    
    public float HoldSeconds { get; private set; }

    public void Tick(float deltaTime)
    {
        bool held = this.v893528d06704();

        this.WentDown = held && !this.v6ad63ee0de8f;
        this.WentUp = !held && this.v6ad63ee0de8f;

        if (this.WentDown)
        {
            this.HoldSeconds = 0f;
        }
        else if (held)
        {
            this.HoldSeconds += deltaTime;
        }

        this.IsHeld = held;
        this.v6ad63ee0de8f = held;
    }

    
    public void Clear()
    {
        this.IsHeld = false;
        this.WentDown = false;
        this.WentUp = false;
        this.HoldSeconds = 0f;
        this.v6ad63ee0de8f = this.v893528d06704();
    }

    private bool v893528d06704()
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
