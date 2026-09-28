using UnityEngine;





public sealed class vb5c1890f2bcc : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;

    private float vf449f21e16d9;
    private float v53b6ccf5361a = 1f;
    private bool v23884e4d91f6;

    
    public int SectionIndex { get; private set; }

    public bool Spent
    {
        get { return this.v23884e4d91f6; }
    }

    public void v897ce3b95a97(int v93e5fd9cd831, float ve562b1fb8a8c)
    {
        this.SectionIndex = v93e5fd9cd831;
        this.v53b6ccf5361a = ve562b1fb8a8c;
        if (this._body != null)
        {
            this._body.size = new Vector2(this.v53b6ccf5361a, this.v53b6ccf5361a);
            this._body.color = v5e702f77a0c6.v8cce7ccc9ec7;
        }
    }

    public void va66e86929f1d()
    {
        this.v23884e4d91f6 = true;
        if (this._body != null)
        {
            this._body.color = v5e702f77a0c6.ve698a8efbd72;
            this._body.size = new Vector2(this.v53b6ccf5361a * 0.8f, this.v53b6ccf5361a * 0.8f);
        }
    }

    public void Animate(float deltaTime)
    {
        if (this.v23884e4d91f6 || this._body == null)
        {
            return;
        }

        this.vf449f21e16d9 += deltaTime;
        float wave = 0.5f + (0.5f * Mathf.Sin(this.vf449f21e16d9 * 3.8f));
        this._body.color = Color.Lerp(v5e702f77a0c6.v8cce7ccc9ec7, v5e702f77a0c6.v44add4c81f28, wave * 0.6f);
        float va2f2beaaaa9a = this.v53b6ccf5361a * (1f + (0.06f * wave));
        this._body.size = new Vector2(va2f2beaaaa9a, va2f2beaaaa9a);
    }
}
