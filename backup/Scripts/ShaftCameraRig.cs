using UnityEngine;

/// <summary>
/// Follows the core up the shaft and nothing else: X is fixed, Y is damped, and both
/// ends are clamped so the floor never floats and the beacon always fits in frame on
/// the last section.
/// </summary>
public sealed class ShaftCameraRig : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _smoothTime = 0.22f;

    private float _minY;
    private float _maxY;
    private float _velocity;
    private float _shakeTimer;
    private float _shakeStrength;
    private float _restX;

    public void Configure(float floorY, float ceilingY)
    {
        if (this._camera == null)
        {
            return;
        }

        float halfHeight = this._camera.orthographicSize;
        this._minY = floorY + halfHeight;
        this._maxY = Mathf.Max(this._minY, ceilingY - halfHeight);
        this._restX = this.transform.position.x;
        this.transform.position = new Vector3(this._restX, this._minY, this.transform.position.z);
    }

    public void Shake(float strength, float seconds)
    {
        this._shakeStrength = strength;
        this._shakeTimer = seconds;
    }

    public void Follow(float targetY, float deltaTime)
    {
        Vector3 position = this.transform.position;
        float wanted = Mathf.Clamp(targetY, this._minY, this._maxY);
        float next = Mathf.SmoothDamp(position.y, wanted, ref this._velocity, this._smoothTime, Mathf.Infinity, deltaTime);

        float offsetX = 0f;
        float offsetY = 0f;
        if (this._shakeTimer > 0f)
        {
            this._shakeTimer = Mathf.Max(0f, this._shakeTimer - deltaTime);
            float decay = this._shakeStrength * (this._shakeTimer / 0.25f);
            offsetX = Mathf.Sin(Time.unscaledTime * 62f) * decay;
            offsetY = Mathf.Cos(Time.unscaledTime * 71f) * decay;
        }

        this.transform.position = new Vector3(this._restX + offsetX, next + offsetY, position.z);
    }
}
