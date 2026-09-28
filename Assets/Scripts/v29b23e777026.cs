using UnityEngine;






public sealed class v29b23e777026 : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _smoothTime = 0.22f;

    private float vd307ec38d82b;
    private float v9e9af8fe7fb7;
    private float v117473f5a9de;
    private float v7e066ebdbafe;
    private float ve69c9eae54e1;
    private float v36e4d46e1252;

    public void v897ce3b95a97(float v7d29113c98a4, float v89ebcefff625)
    {
        if (this._camera == null)
        {
            return;
        }

        float halfHeight = this._camera.orthographicSize;
        this.vd307ec38d82b = v7d29113c98a4 + halfHeight;
        this.v9e9af8fe7fb7 = Mathf.Max(this.vd307ec38d82b, v89ebcefff625 - halfHeight);
        this.v36e4d46e1252 = this.transform.position.x;
        this.transform.position = new Vector3(this.v36e4d46e1252, this.vd307ec38d82b, this.transform.position.z);
    }

    public void vc715b17bc3cb(float vbe2880a7cb7e, float vbcddb745a600)
    {
        this.ve69c9eae54e1 = vbe2880a7cb7e;
        this.v7e066ebdbafe = vbcddb745a600;
    }

    public void v6cbeb845104c(float v2a1250fd42d1, float deltaTime)
    {
        Vector3 position = this.transform.position;
        float wanted = Mathf.Clamp(v2a1250fd42d1, this.vd307ec38d82b, this.v9e9af8fe7fb7);
        float next = Mathf.SmoothDamp(position.y, wanted, ref this.v117473f5a9de, this._smoothTime, Mathf.Infinity, deltaTime);

        float offsetX = 0f;
        float offsetY = 0f;
        if (this.v7e066ebdbafe > 0f)
        {
            this.v7e066ebdbafe = Mathf.Max(0f, this.v7e066ebdbafe - deltaTime);
            float decay = this.ve69c9eae54e1 * (this.v7e066ebdbafe / 0.25f);
            offsetX = Mathf.Sin(Time.unscaledTime * 62f) * decay;
            offsetY = Mathf.Cos(Time.unscaledTime * 71f) * decay;
        }

        this.transform.position = new Vector3(this.v36e4d46e1252 + offsetX, next + offsetY, position.z);
    }
}
