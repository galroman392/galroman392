using UnityEngine;






public sealed class vbc8ac1625074 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;

    private v4434d66e509d v124868a88d7f;
    private float v7d77af6d3a2d;
    private float v494ed514d2aa;
    private float vdb30c7e213f9;
    private float vb227a0eb7488;

    public void v897ce3b95a97(v4434d66e509d vdfa971bdfc3e, float v2e9d1f07493b, float v8260e328bd07, float v25111896e527)
    {
        this.v124868a88d7f = vdfa971bdfc3e;
        this.v494ed514d2aa = v2e9d1f07493b * vdfa971bdfc3e.WidthFraction;
        this.v7d77af6d3a2d = Mathf.Max(0.0001f, v2e9d1f07493b - this.v494ed514d2aa);
        this.vb227a0eb7488 = v25111896e527;

        
        
        this.vdb30c7e213f9 = vdfa971bdfc3e.Kind == v4434d66e509d.v30559dd3082e ? this.v494ed514d2aa : v8260e328bd07;

        if (this._body == null)
        {
            return;
        }

        this._body.size = new Vector2(this.v494ed514d2aa, this.vdb30c7e213f9);
        this._body.color = vdfa971bdfc3e.Kind == v4434d66e509d.v6224c3d1d228 ? v5e702f77a0c6.vc46f649702f0 : v5e702f77a0c6.v3893738f411c;
    }

    
    public Vector2 HalfExtents
    {
        get { return new Vector2(this.v494ed514d2aa * 0.5f, this.vdb30c7e213f9 * 0.5f); }
    }

    public void Advance(float v0e675bdee7fa)
    {
        if (this.v124868a88d7f == null)
        {
            return;
        }

        
        
        float distance = (v0e675bdee7fa * this.v124868a88d7f.Speed) + (this.v124868a88d7f.Phase * this.v7d77af6d3a2d * 2f);
        float vda5fcc8756a9 = Mathf.PingPong(distance, this.v7d77af6d3a2d) - (this.v7d77af6d3a2d * 0.5f);
        this.transform.localPosition = new Vector3(vda5fcc8756a9 * this.v124868a88d7f.Direction, this.vb227a0eb7488, 0f);

        if (this._body == null)
        {
            return;
        }

        if (this.v124868a88d7f.Kind == v4434d66e509d.v30559dd3082e)
        {
            this._body.transform.localRotation = Quaternion.Euler(0f, 0f, (v0e675bdee7fa * 180f) % 360f);
        }
        else if (this.v124868a88d7f.Kind == v4434d66e509d.v2a418c016828)
        {
            float wave = 0.5f + (0.5f * Mathf.Sin(v0e675bdee7fa * 4f));
            this._body.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v3893738f411c, 0.55f + (0.45f * wave));
        }
    }
}
