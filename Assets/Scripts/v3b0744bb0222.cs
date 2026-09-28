using UnityEngine;





public sealed class v3b0744bb0222 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _flash;

    private float v85138ccc3c4c;
    private float v53b6ccf5361a = 1f;

    
    public int SectionIndex { get; private set; }

    
    public int Side { get; private set; }

    public Vector3 DockPoint
    {
        get { return this.transform.position; }
    }

    public void v897ce3b95a97(int v93e5fd9cd831, int va2f2beaaaa9a, float v52f812ebd685)
    {
        this.SectionIndex = v93e5fd9cd831;
        this.Side = va2f2beaaaa9a;
        this.v53b6ccf5361a = v52f812ebd685;

        if (this._body != null)
        {
            this._body.size = new Vector2(this.v53b6ccf5361a, this.v53b6ccf5361a);
            
            this._body.flipX = va2f2beaaaa9a == 0;
            this._body.color = v5e702f77a0c6.v5bccfad8f382;
        }

        if (this._flash != null)
        {
            this._flash.size = new Vector2(this.v53b6ccf5361a * 1.3f, this.v53b6ccf5361a * 1.3f);
            this._flash.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v44add4c81f28, 0f);
        }
    }

    public void Highlight()
    {
        this.v85138ccc3c4c = 0.35f;
    }

    public void Animate(float deltaTime, bool v1d5ede252e2a)
    {
        if (this._body != null)
        {
            this._body.color = v1d5ede252e2a ? v5e702f77a0c6.v44add4c81f28 : v5e702f77a0c6.v5bccfad8f382;
        }

        if (this._flash == null)
        {
            return;
        }

        if (this.v85138ccc3c4c > 0f)
        {
            this.v85138ccc3c4c = Mathf.Max(0f, this.v85138ccc3c4c - deltaTime);
            float life = Mathf.Clamp01(this.v85138ccc3c4c / 0.35f);
            float va2f2beaaaa9a = this.v53b6ccf5361a * (1.3f + (0.8f * (1f - life)));
            this._flash.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v44add4c81f28, life * 0.9f);
            this._flash.size = new Vector2(va2f2beaaaa9a, va2f2beaaaa9a);
        }
        else if (this._flash.color.a > 0f)
        {
            this._flash.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v44add4c81f28, 0f);
        }
    }
}
