using UnityEngine;










public sealed class vf4d8143b01bd : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _shieldRing;
    [SerializeField] private SpriteRenderer _thrustBeam;
    [SerializeField] private SpriteRenderer _sparks;

    private float v47ae5494b327 = 1f;
    private float v3aa0bd16b400 = 1f;
    private float vc0e3872157e6 = 1f;
    private float vf449f21e16d9;
    private float v85138ccc3c4c;
    private float v0eedcade9e4e = 0.32f;
    private Color vdcf52a47aaf6 = v5e702f77a0c6.v44add4c81f28;
    private bool v813d82d08247;
    private bool v6182022e9291;
    private float vb757f8a0caa5;

    
    public void v897ce3b95a97(float va715527ad2c7, float v0a1a397fa4f1, float v3e75ebf25b79)
    {
        this.v47ae5494b327 = va715527ad2c7;
        this.v3aa0bd16b400 = v0a1a397fa4f1;
        this.vc0e3872157e6 = v3e75ebf25b79;

        vd910987707de(this._body, this.v47ae5494b327);
        vd910987707de(this._shieldRing, this.v3aa0bd16b400);
        vd910987707de(this._sparks, this.v47ae5494b327 * 0.9f);

        if (this._sparks != null)
        {
            this._sparks.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v44add4c81f28, 0f);
        }

        if (this._thrustBeam != null)
        {
            this._thrustBeam.size = new Vector2(this.v47ae5494b327 * 0.55f, this.vc0e3872157e6);
            this._thrustBeam.transform.localPosition = new Vector3(0f, -((this.vc0e3872157e6 * 0.5f) + (va715527ad2c7 * 0.35f)), 0f);
            this._thrustBeam.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v8cce7ccc9ec7, 0f);
        }

        this.vcdab30192bd3(true);
        this.v4a67f96115e7(false);
    }

    public void vcdab30192bd3(bool up)
    {
        this.v813d82d08247 = up;
        if (this._shieldRing != null)
        {
            this._shieldRing.enabled = up;
        }
    }

    public void v4a67f96115e7(bool on)
    {
        this.v6182022e9291 = on;
    }

    
    public void vc1be5bfc2b0a()
    {
        this.vdcf52a47aaf6 = v5e702f77a0c6.vc46f649702f0;
        this.v0eedcade9e4e = 0.32f;
        this.v85138ccc3c4c = this.v0eedcade9e4e;
        this.vb757f8a0caa5 = 0.40f;
    }

    
    public void v5e5daba24910()
    {
        this.vdcf52a47aaf6 = v5e702f77a0c6.v44add4c81f28;
        this.v0eedcade9e4e = 0.22f;
        this.v85138ccc3c4c = this.v0eedcade9e4e;
        this.vb757f8a0caa5 = 0.24f;
    }

    
    public void Animate(float deltaTime, bool v7db1846a84cf)
    {
        this.vf449f21e16d9 += deltaTime;

        if (this._shieldRing != null && this.v813d82d08247)
        {
            float wave = 0.5f + (0.5f * Mathf.Sin(this.vf449f21e16d9 * 3.4f));
            this._shieldRing.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v8cce7ccc9ec7, 0.5f + (0.35f * wave));
            vd910987707de(this._shieldRing, this.v3aa0bd16b400 * (1f + (0.04f * wave)));
        }

        if (this._thrustBeam != null)
        {
            float v2132923cb930 = this.v6182022e9291 ? 0.85f : 0f;
            float v875a2b2d2dc9 = Mathf.MoveTowards(this._thrustBeam.color.a, v2132923cb930, deltaTime * 4.5f);
            this._thrustBeam.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v8cce7ccc9ec7, v875a2b2d2dc9);
            float jitter = 1f + (0.06f * Mathf.Sin(this.vf449f21e16d9 * 18f));
            this._thrustBeam.size = new Vector2(this.v47ae5494b327 * 0.55f, this.vc0e3872157e6 * jitter);
        }

        if (this._body != null)
        {
            Color v6afc3d41b866 = v5e702f77a0c6.v5bccfad8f382;
            if (this.v85138ccc3c4c > 0f)
            {
                this.v85138ccc3c4c = Mathf.Max(0f, this.v85138ccc3c4c - deltaTime);
                v6afc3d41b866 = Color.Lerp(v6afc3d41b866, this.vdcf52a47aaf6, Mathf.Clamp01(this.v85138ccc3c4c / this.v0eedcade9e4e));
            }

            if (v7db1846a84cf)
            {
                v6afc3d41b866 = v5e702f77a0c6.v35ea66314f38(v6afc3d41b866, 0.45f + (0.55f * Mathf.Abs(Mathf.Sin(this.vf449f21e16d9 * 12f))));
            }

            this._body.color = v6afc3d41b866;
            vd910987707de(this._body, this.v47ae5494b327 * (1f + (0.03f * Mathf.Sin(this.vf449f21e16d9 * 2.2f))));
        }

        if (this._sparks == null)
        {
            return;
        }

        if (this.vb757f8a0caa5 > 0f)
        {
            this.vb757f8a0caa5 = Mathf.Max(0f, this.vb757f8a0caa5 - deltaTime);
            float life = Mathf.Clamp01(this.vb757f8a0caa5 / 0.40f);
            this._sparks.color = v5e702f77a0c6.v35ea66314f38(this.vdcf52a47aaf6, life);
            vd910987707de(this._sparks, this.v47ae5494b327 * (0.9f + (1.3f * (1f - life))));
        }
        else if (this._sparks.color.a > 0f)
        {
            this._sparks.color = v5e702f77a0c6.v35ea66314f38(this.vdcf52a47aaf6, 0f);
        }
    }

    
    public void vb7721f841dcc(float vc7bca8bc1c21)
    {
        if (this._body != null)
        {
            this._body.color = Color.Lerp(this._body.color, v5e702f77a0c6.ve698a8efbd72, Mathf.Clamp01(vc7bca8bc1c21));
        }
    }

    private static void vd910987707de(SpriteRenderer vc16309758b26, float va2f2beaaaa9a)
    {
        if (vc16309758b26 != null)
        {
            vc16309758b26.size = new Vector2(va2f2beaaaa9a, va2f2beaaaa9a);
        }
    }
}
