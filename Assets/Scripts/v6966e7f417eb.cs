using System.Collections.Generic;
using UnityEngine;











public sealed class v6966e7f417eb : MonoBehaviour
{
    private const int v8d626427625d = 0;
    private const int vabbbcddd4dea = 1;
    private const int ve108c23f3c60 = 2;
    private const int v8728a6feefb1 = 3;
    private const int vdbd2975d2751 = 4;
    private const int v0c4e4a6ba044 = 5;

    private const int v44dc857d3b50 = 3;
    private const float ve2165fbc7562 = 0.18f;
    private const float v3f27bcf8e15a = 1.2f;
    private const float v2d970ab7ad54 = 0.16f;

    
    
    
    private const int v1fdd771ea28f = -19;
    private const int v320f4b54d58e = -18;
    private const int v1714d4505d8d = -16;
    private const int vc5126130f183 = -14;
    private const int v0f7dad341bb6 = -12;
    private const int vc788b28f015f = -10;
    private const int v27eb3626ca59 = -8;

    [SerializeField] private Camera _camera;
    [SerializeField] private v29b23e777026 _cameraRig;
    [SerializeField] private vb6b38774829e _input;
    [SerializeField] private vf4d8143b01bd _core;
    [SerializeField] private vb43312e1e369 _hud;
    [SerializeField] private v45f4b7c1454e _pops;
    [SerializeField] private Transform _shaftRoot;
    [SerializeField] private SpriteRenderer _backdrop;
    [SerializeField] private SpriteRenderer _beacon;
    [SerializeField] private GameObject _wallSegmentPrefab;
    [SerializeField] private GameObject _sectionMarkerPrefab;
    [SerializeField] private GameObject _runeGlyphPrefab;
    [SerializeField] private GameObject _socketPrefab;
    [SerializeField] private GameObject _repairNodePrefab;
    [SerializeField] private GameObject _barrierSweepPrefab;
    [SerializeField] private GameObject _barrierRotorPrefab;
    [SerializeField] private GameObject _barrierGatePrefab;

    private readonly List<v3b0744bb0222> _sockets = new List<v3b0744bb0222>();
    private readonly List<vbc8ac1625074> v7b306ecbd9b6 = new List<vbc8ac1625074>();
    private readonly List<vb5c1890f2bcc> v564e9362ba55 = new List<vb5c1890f2bcc>();

    private va19a859d9a14 v2411b59e4151;
    private vf5d557e7ec24 v15f9e7d40341;

    private float v2a306ef2a61d;
    private float vc65755d9d3c2;
    private float v3bf84419f291;
    private float vd5ded9dd412d;
    private float v9f548f872603;
    private float va9a25a6ba560;
    private float vb4576cfb5235;
    private float v519a49c02c2d;
    private float v478b0ceb985c;
    private float v54fb3721a9be;

    private int v1abd6dc7c758 = v8d626427625d;
    private int _routeIndex;
    private int ve0e73a586cf0;
    private int ve44488bc20ed = v44dc857d3b50;
    private float v2ceafd35bd89;
    private float vae76d3742b82;
    private float v5cf9661e09eb;
    private bool vbb1bd448d614;

    private v3b0744bb0222 v0f1c52be2360;
    private Vector3 v02ab20b93562;
    private Vector3 ve586e8a89b2a;
    private float v8b7bd5eecf7f;
    private int v4463326224c3;

    private void Awake()
    {
        this.v9cdfc8b69bcf();
        this._routeIndex = vff93c1a0dc29.vd54499223391();

        int attempt = vff93c1a0dc29.vf5880f526d6d();
        this.v2411b59e4151 = new va19a859d9a14(
            this.vd5ded9dd412d, this.v3bf84419f291, this.v9f548f872603, this.vb4576cfb5235, this.vc65755d9d3c2);
        this.v15f9e7d40341 = this.v2411b59e4151.Build(this._routeIndex, attempt);

        this.v74f3316fea64();
        this.v04ca26157973(this.v8465bb6263e8(0, 0), true);

        if (this._hud != null)
        {
            this._hud.vf0e82162ab83(this._routeIndex);
            this._hud.v794261e20b7d(0);
            this._hud.v6cefef1edf2a(this.ve44488bc20ed);
        }

        if (this._cameraRig != null)
        {
            this._cameraRig.v897ce3b95a97(this.v478b0ceb985c, (this.v15f9e7d40341.TopSection + 1.4f) * this.v3bf84419f291);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (this.vbb1bd448d614)
        {
            return;
        }

        if (this._input != null)
        {
            this._input.Tick(deltaTime);
        }

        this.v2ceafd35bd89 += deltaTime;
        this.vae76d3742b82 = Mathf.Max(0f, this.vae76d3742b82 - deltaTime);

        this.vd7e9df9b8a0d(deltaTime);

        switch (this.v1abd6dc7c758)
        {
            case v8d626427625d:
                this.v4695906ecc51();
                break;
            case vabbbcddd4dea:
                this.vaad41212fcaa(deltaTime);
                break;
            case ve108c23f3c60:
                this.v9af3838523d6(deltaTime);
                break;
            case v8728a6feefb1:
                this.v992167477662(deltaTime);
                break;
            case vdbd2975d2751:
                this.vea525b961dac(deltaTime, true);
                break;
            case v0c4e4a6ba044:
                this.vea525b961dac(deltaTime, false);
                break;
        }

        if (this._core != null)
        {
            this._core.Animate(deltaTime, this.vae76d3742b82 > 0f);
        }

        if (this._cameraRig != null && this._core != null)
        {
            this._cameraRig.v6cbeb845104c(this._core.transform.position.y, deltaTime);
        }
    }

    
    public void v256d77a60108()
    {
        if (this.v1abd6dc7c758 == vdbd2975d2751 || this.v1abd6dc7c758 == v0c4e4a6ba044 || this.vbb1bd448d614)
        {
            return;
        }

        this.vbb1bd448d614 = true;
        if (this._pops != null)
        {
            this._pops.v6bedca08252b(this.ve0e73a586cf0, this.ve44488bc20ed);
        }
    }

    public void v1f216549aa97()
    {
        this.vbb1bd448d614 = false;
        if (this._input != null)
        {
            
            this._input.Clear();
        }
    }

    

    private void v9cdfc8b69bcf()
    {
        float halfHeight = this._camera != null ? this._camera.orthographicSize : 5f;
        float aspect = this._camera != null ? this._camera.aspect : (9f / 19.5f);

        this.vc65755d9d3c2 = halfHeight * 2f;
        this.v2a306ef2a61d = this.vc65755d9d3c2 * aspect;

        this.v3bf84419f291 = this.vc65755d9d3c2 * 0.118f;
        this.vd5ded9dd412d = this.v2a306ef2a61d * 0.74f;
        this.v9f548f872603 = this.v2a306ef2a61d * 0.155f;
        this.va9a25a6ba560 = this.v2a306ef2a61d * 0.13f;
        this.vb4576cfb5235 = this.va9a25a6ba560 * 0.45f;
        this.v519a49c02c2d = this.v3bf84419f291 * 0.22f;
        
        
        
        this.v478b0ceb985c = -this.v3bf84419f291 * 1.2f;
        this.v54fb3721a9be = this.v2a306ef2a61d * 0.40f;
    }

    
    
    
    
    
    
    private float v9e04a2631141(int v720ad0d3a1a2, float v0c3d932e9fca)
    {
        float margin = (this.v519a49c02c2d + this.v9f548f872603) * 0.6f;
        float low = this.v089f78ef802b(v720ad0d3a1a2 - 1) + margin;
        float high = this.v089f78ef802b(v720ad0d3a1a2) - margin;
        if (high <= low)
        {
            return (this.v089f78ef802b(v720ad0d3a1a2 - 1) + this.v089f78ef802b(v720ad0d3a1a2)) * 0.5f;
        }

        return Mathf.Lerp(low, high, v0c3d932e9fca);
    }

    private float v089f78ef802b(int v720ad0d3a1a2)
    {
        v5dc5c793ebd9 vdfa971bdfc3e = this.v15f9e7d40341.v0ff36440fbbc[Mathf.Clamp(v720ad0d3a1a2, 0, this.v15f9e7d40341.TopSection)];
        return (v720ad0d3a1a2 + vdfa971bdfc3e.SocketHeightFraction) * this.v3bf84419f291;
    }

    private float v19cb49dade2e(int va2f2beaaaa9a)
    {
        float vda5fcc8756a9 = (this.vd5ded9dd412d * 0.5f) - this.vb4576cfb5235;
        return va2f2beaaaa9a == 0 ? -vda5fcc8756a9 : vda5fcc8756a9;
    }

    

    private void v74f3316fea64()
    {
        if (this._backdrop != null)
        {
            this._backdrop.size = new Vector2(this.v2a306ef2a61d, this.vc65755d9d3c2);
            this._backdrop.sortingOrder = v1fdd771ea28f;
        }

        int top = this.v15f9e7d40341.TopSection;

        
        
        this.v52e6ce33858d(-1);
        this.v52e6ce33858d(top + 1);

        for (int v720ad0d3a1a2 = 0; v720ad0d3a1a2 <= top; v720ad0d3a1a2++)
        {
            v5dc5c793ebd9 vdfa971bdfc3e = this.v15f9e7d40341.v0ff36440fbbc[v720ad0d3a1a2];

            this.v52e6ce33858d(v720ad0d3a1a2);
            this.v534fc1245677(v720ad0d3a1a2);
            this.v1985d35a9af7(v720ad0d3a1a2, vdfa971bdfc3e.RuneOffset);

            if (vdfa971bdfc3e.SocketLeft)
            {
                this.v76ccabfc9cc2(v720ad0d3a1a2, 0);
            }

            if (vdfa971bdfc3e.SocketRight)
            {
                this.v76ccabfc9cc2(v720ad0d3a1a2, 1);
            }

            if (vdfa971bdfc3e.RepairSide >= 0)
            {
                this.v1cd5a20b8807(v720ad0d3a1a2, vdfa971bdfc3e.RepairSide);
            }

            for (int i = 0; i < vdfa971bdfc3e.Barriers.Count; i++)
            {
                this.vfd6ec9bbefc7(v720ad0d3a1a2, vdfa971bdfc3e.Barriers[i]);
            }
        }

        if (this._beacon != null)
        {
            this._beacon.size = new Vector2(this.v54fb3721a9be, this.v54fb3721a9be);
            this._beacon.sortingOrder = vc788b28f015f;
            this._beacon.transform.position = new Vector3(0f, (top + 0.9f) * this.v3bf84419f291, 0f);
            this._beacon.color = v5e702f77a0c6.v44add4c81f28;
        }

        if (this._core != null)
        {
            this._core.v897ce3b95a97(this.v9f548f872603, this.v9f548f872603 * 1.45f, this.v3bf84419f291 * 0.9f);
        }
    }

    private void v52e6ce33858d(int v720ad0d3a1a2)
    {
        SpriteRenderer wall = this.v8643f02d4959(this._wallSegmentPrefab, v1714d4505d8d,
            new Vector3(0f, (v720ad0d3a1a2 + 0.5f) * this.v3bf84419f291, 0f));
        if (wall != null)
        {
            wall.size = new Vector2(this.vd5ded9dd412d, this.v3bf84419f291);
            wall.color = v720ad0d3a1a2 % 2 == 0 ? v5e702f77a0c6.va37c9d296cc7 : v5e702f77a0c6.vf971a5a3afc1;
        }
    }

    private void v534fc1245677(int v720ad0d3a1a2)
    {
        SpriteRenderer marker = this.v8643f02d4959(this._sectionMarkerPrefab, vc5126130f183,
            new Vector3(0f, v720ad0d3a1a2 * this.v3bf84419f291, 0f));
        if (marker != null)
        {
            marker.size = new Vector2(this.vd5ded9dd412d * 0.9f, this.vc65755d9d3c2 * 0.05f);
            marker.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v8cce7ccc9ec7, 0.55f);
        }
    }

    private void v1985d35a9af7(int v720ad0d3a1a2, float vda5fcc8756a9)
    {
        SpriteRenderer rune = this.v8643f02d4959(this._runeGlyphPrefab, v320f4b54d58e,
            new Vector3(vda5fcc8756a9 * this.vd5ded9dd412d * 0.34f, (v720ad0d3a1a2 + 0.5f) * this.v3bf84419f291, 0f));
        if (rune != null)
        {
            float va2f2beaaaa9a = this.v2a306ef2a61d * 0.20f;
            rune.size = new Vector2(va2f2beaaaa9a, va2f2beaaaa9a);
            rune.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v3893738f411c, 0.28f);
        }
    }

    private void v76ccabfc9cc2(int v720ad0d3a1a2, int va2f2beaaaa9a)
    {
        if (this._socketPrefab == null || this._shaftRoot == null)
        {
            return;
        }

        GameObject spawned = Instantiate(this._socketPrefab, this._shaftRoot);
        spawned.transform.position = new Vector3(this.v19cb49dade2e(va2f2beaaaa9a), this.v089f78ef802b(v720ad0d3a1a2), 0f);

        v3b0744bb0222 v0c1aa4a9ea15 = spawned.GetComponent<v3b0744bb0222>();
        if (v0c1aa4a9ea15 == null)
        {
            return;
        }

        v0c1aa4a9ea15.v897ce3b95a97(v720ad0d3a1a2, va2f2beaaaa9a, this.va9a25a6ba560);
        this._sockets.Add(v0c1aa4a9ea15);
    }

    private void v1cd5a20b8807(int v720ad0d3a1a2, int va2f2beaaaa9a)
    {
        if (this._repairNodePrefab == null || this._shaftRoot == null)
        {
            return;
        }

        GameObject spawned = Instantiate(this._repairNodePrefab, this._shaftRoot);
        spawned.transform.position = new Vector3(
            this.v19cb49dade2e(va2f2beaaaa9a) * 0.72f,
            this.v089f78ef802b(v720ad0d3a1a2) + (this.v3bf84419f291 * 0.26f),
            0f);

        vb5c1890f2bcc node = spawned.GetComponent<vb5c1890f2bcc>();
        if (node == null)
        {
            return;
        }

        node.v897ce3b95a97(v720ad0d3a1a2, this.v9f548f872603 * 0.62f);
        this.v564e9362ba55.Add(node);
    }

    private void vfd6ec9bbefc7(int v720ad0d3a1a2, v4434d66e509d vdfa971bdfc3e)
    {
        GameObject v32636e8a83b2 = this.vb9a1edc4a8da(vdfa971bdfc3e.Kind);
        if (v32636e8a83b2 == null || this._shaftRoot == null)
        {
            return;
        }

        GameObject spawned = Instantiate(v32636e8a83b2, this._shaftRoot);
        vbc8ac1625074 barrier = spawned.GetComponent<vbc8ac1625074>();
        if (barrier == null)
        {
            return;
        }

        barrier.v897ce3b95a97(vdfa971bdfc3e, this.vd5ded9dd412d, this.v519a49c02c2d,
            this.v9e04a2631141(v720ad0d3a1a2, vdfa971bdfc3e.HeightFraction));
        barrier.Advance(0f);
        this.v7b306ecbd9b6.Add(barrier);
    }

    private GameObject vb9a1edc4a8da(int v2cba01959503)
    {
        if (v2cba01959503 == v4434d66e509d.v30559dd3082e)
        {
            return this._barrierRotorPrefab;
        }

        return v2cba01959503 == v4434d66e509d.v2a418c016828 ? this._barrierGatePrefab : this._barrierSweepPrefab;
    }

    private SpriteRenderer v8643f02d4959(GameObject v32636e8a83b2, int sortingOrder, Vector3 position)
    {
        if (v32636e8a83b2 == null || this._shaftRoot == null)
        {
            return null;
        }

        GameObject spawned = Instantiate(v32636e8a83b2, this._shaftRoot);
        spawned.transform.position = position;

        SpriteRenderer vc16309758b26 = spawned.GetComponent<SpriteRenderer>();
        if (vc16309758b26 != null)
        {
            vc16309758b26.sortingOrder = Mathf.Clamp(sortingOrder, -19, -1);
        }

        return vc16309758b26;
    }

    

    private void vd7e9df9b8a0d(float deltaTime)
    {
        for (int i = 0; i < this.v7b306ecbd9b6.Count; i++)
        {
            this.v7b306ecbd9b6[i].Advance(this.v2ceafd35bd89);
        }

        for (int i = 0; i < this._sockets.Count; i++)
        {
            this._sockets[i].Animate(deltaTime, this._sockets[i] == this.v0f1c52be2360);
        }

        for (int i = 0; i < this.v564e9362ba55.Count; i++)
        {
            this.v564e9362ba55[i].Animate(deltaTime);
        }
    }

    private void v4695906ecc51()
    {
        if (this._input != null && this._input.WentDown)
        {
            this.v1abd6dc7c758 = vabbbcddd4dea;
            if (this._core != null)
            {
                this._core.vcdab30192bd3(false);
                this._core.v4a67f96115e7(true);
            }
        }
    }

    private void vaad41212fcaa(float deltaTime)
    {
        if (this._core == null)
        {
            return;
        }

        float speed = this.v2411b59e4151.RiseSpeed(this.ve0e73a586cf0);
        Vector3 position = this._core.transform.position;
        position.y += speed * deltaTime;
        this._core.transform.position = position;

        if (this.v1f4092dc687f(position))
        {
            this.vc8f341464389();
            return;
        }

        
        v3b0744bb0222 ceiling = this.v8465bb6263e8(this.v15f9e7d40341.TopSection, 0);
        if (ceiling != null && position.y >= ceiling.DockPoint.y)
        {
            this._core.v4a67f96115e7(false);
            this.v8c0b3439c71f(ceiling);
            return;
        }

        if (this._input == null || !this._input.WentUp)
        {
            return;
        }

        bool wasTap = this._input.HoldSeconds <= ve2165fbc7562;
        this._core.v4a67f96115e7(false);

        v3b0744bb0222 v2132923cb930 = wasTap ? this.ve9b850f24cea(position) : this.v5daac8dc8faa(position);
        if (v2132923cb930 != null)
        {
            this.v8c0b3439c71f(v2132923cb930);
        }
        else
        {
            this.v1abd6dc7c758 = ve108c23f3c60;
        }
    }

    private void v9af3838523d6(float deltaTime)
    {
        if (this._core == null)
        {
            return;
        }

        Vector3 position = this._core.transform.position;
        position.y -= this.v2411b59e4151.DriftSpeed * deltaTime;
        this._core.transform.position = position;

        if (position.y < this.v478b0ceb985c - this.v9f548f872603)
        {
            this.vd03fc7291d9d();
            return;
        }

        if (this.v1f4092dc687f(position))
        {
            this.vc8f341464389();
            return;
        }

        if (this._input != null && this._input.WentDown)
        {
            this.v1abd6dc7c758 = vabbbcddd4dea;
            this._core.v4a67f96115e7(true);
            return;
        }

        v3b0744bb0222 v2132923cb930 = this.v5daac8dc8faa(position);
        if (v2132923cb930 != null)
        {
            this.v8c0b3439c71f(v2132923cb930);
        }
    }

    private void v992167477662(float deltaTime)
    {
        if (this._core == null)
        {
            return;
        }

        this.v8b7bd5eecf7f = Mathf.Min(1f, this.v8b7bd5eecf7f + (deltaTime / v2d970ab7ad54));
        
        float eased = 1f - Mathf.Pow(1f - this.v8b7bd5eecf7f, 3f);
        float overshoot = 1f + (0.12f * Mathf.Sin(this.v8b7bd5eecf7f * Mathf.PI));
        this._core.transform.position = Vector3.LerpUnclamped(this.v02ab20b93562, this.ve586e8a89b2a, eased * overshoot);

        if (this.v8b7bd5eecf7f < 1f)
        {
            return;
        }

        this._core.transform.position = this.ve586e8a89b2a;
        this.v04ca26157973(this.v8465bb6263e8(this.v4463326224c3, this.vdf3d0191f85b(this.ve586e8a89b2a)), false);
    }

    private void vea525b961dac(float deltaTime, bool va5521bf2eda1)
    {
        this.v5cf9661e09eb = Mathf.Max(0f, this.v5cf9661e09eb - deltaTime);

        if (va5521bf2eda1 && this._beacon != null)
        {
            float wave = Mathf.Abs(Mathf.Sin(this.v2ceafd35bd89 * 6f));
            float va2f2beaaaa9a = this.v54fb3721a9be * (1f + (0.25f * wave));
            this._beacon.size = new Vector2(va2f2beaaaa9a, va2f2beaaaa9a);
            this._beacon.color = Color.Lerp(v5e702f77a0c6.v44add4c81f28, Color.white, wave);
        }
        else if (!va5521bf2eda1 && this._core != null)
        {
            this._core.vb7721f841dcc(deltaTime * 1.4f);
        }

        if (this.v5cf9661e09eb > 0f || this._pops == null)
        {
            return;
        }

        this.vbb1bd448d614 = true;
        if (va5521bf2eda1)
        {
            this._pops.v9cb9c904ce56(this._routeIndex, this.ve44488bc20ed);
        }
        else
        {
            this._pops.v02b0199b1792(this._routeIndex, this.ve0e73a586cf0);
        }
    }

    

    private void v8c0b3439c71f(v3b0744bb0222 v2132923cb930)
    {
        this.v02ab20b93562 = this._core.transform.position;
        this.ve586e8a89b2a = v2132923cb930.DockPoint;
        this.v4463326224c3 = v2132923cb930.SectionIndex;
        this.v8b7bd5eecf7f = 0f;
        this.v1abd6dc7c758 = v8728a6feefb1;
        v2132923cb930.Highlight();
    }

    private void v04ca26157973(v3b0744bb0222 v0c1aa4a9ea15, bool vce6c41ee2354)
    {
        if (v0c1aa4a9ea15 == null)
        {
            this.v1abd6dc7c758 = ve108c23f3c60;
            return;
        }

        this.v0f1c52be2360 = v0c1aa4a9ea15;
        this.v1abd6dc7c758 = v8d626427625d;
        this._core.transform.position = v0c1aa4a9ea15.DockPoint;
        this._core.v4a67f96115e7(false);
        this._core.vcdab30192bd3(true);

        if (!vce6c41ee2354)
        {
            this._core.v5e5daba24910();
        }

        if (v0c1aa4a9ea15.SectionIndex > this.ve0e73a586cf0)
        {
            this.ve0e73a586cf0 = v0c1aa4a9ea15.SectionIndex;
            vff93c1a0dc29.v6fe279d232b4(this._routeIndex, this.ve0e73a586cf0);
        }

        if (this._hud != null)
        {
            this._hud.v794261e20b7d(this.ve0e73a586cf0);
            this._hud.vf0e82162ab83(this._routeIndex);
        }

        this.vdec7ef38e8a5(v0c1aa4a9ea15.SectionIndex);

        if (v0c1aa4a9ea15.SectionIndex >= this.v15f9e7d40341.TopSection)
        {
            this.ve8b35c59beaf();
        }
    }

    private void vdec7ef38e8a5(int v720ad0d3a1a2)
    {
        for (int i = 0; i < this.v564e9362ba55.Count; i++)
        {
            vb5c1890f2bcc node = this.v564e9362ba55[i];
            if (node.Spent || node.SectionIndex != v720ad0d3a1a2 || this.ve44488bc20ed >= v44dc857d3b50)
            {
                continue;
            }

            node.va66e86929f1d();
            this.ve44488bc20ed = Mathf.Min(v44dc857d3b50, this.ve44488bc20ed + 1);
            if (this._hud != null)
            {
                this._hud.v6cefef1edf2a(this.ve44488bc20ed);
            }

            return;
        }
    }

    private bool v1f4092dc687f(Vector3 vbd4453254a7e)
    {
        if (this.vae76d3742b82 > 0f)
        {
            return false;
        }

        float coreHalf = this.v9f548f872603 * 0.42f;
        for (int i = 0; i < this.v7b306ecbd9b6.Count; i++)
        {
            vbc8ac1625074 barrier = this.v7b306ecbd9b6[i];
            Vector3 barrierPosition = barrier.transform.position;
            Vector2 half = barrier.HalfExtents;

            if (Mathf.Abs(barrierPosition.y - vbd4453254a7e.y) > half.y + coreHalf)
            {
                continue;
            }

            if (Mathf.Abs(barrierPosition.x - vbd4453254a7e.x) <= half.x + coreHalf)
            {
                return true;
            }
        }

        return false;
    }

    private void vc8f341464389()
    {
        this.ve44488bc20ed = Mathf.Max(0, this.ve44488bc20ed - 1);
        this.vae76d3742b82 = v3f27bcf8e15a;

        if (this._hud != null)
        {
            this._hud.v6cefef1edf2a(this.ve44488bc20ed);
        }

        if (this._core != null)
        {
            this._core.vc1be5bfc2b0a();
            this._core.v4a67f96115e7(false);
        }

        if (this._cameraRig != null)
        {
            this._cameraRig.vc715b17bc3cb(this.v2a306ef2a61d * 0.026f, 0.25f);
        }

        if (this.ve44488bc20ed <= 0)
        {
            this.vd03fc7291d9d();
            return;
        }

        
        int fallback = Mathf.Max(0, this.ve0e73a586cf0 - 1);
        v3b0744bb0222 v0c1aa4a9ea15 = this.v8465bb6263e8(fallback, 0);
        if (v0c1aa4a9ea15 == null)
        {
            v0c1aa4a9ea15 = this.v8465bb6263e8(fallback, 1);
        }

        if (v0c1aa4a9ea15 == null)
        {
            this.v1abd6dc7c758 = ve108c23f3c60;
            return;
        }

        this.ve0e73a586cf0 = fallback;
        if (this._hud != null)
        {
            this._hud.v794261e20b7d(this.ve0e73a586cf0);
        }

        this.v8c0b3439c71f(v0c1aa4a9ea15);
    }

    private void ve8b35c59beaf()
    {
        if (this.v1abd6dc7c758 == vdbd2975d2751)
        {
            return;
        }

        this.v1abd6dc7c758 = vdbd2975d2751;
        this.v5cf9661e09eb = 0.9f;
        vff93c1a0dc29.v6fe279d232b4(this._routeIndex, this.v15f9e7d40341.TopSection);
    }

    private void vd03fc7291d9d()
    {
        if (this.v1abd6dc7c758 == v0c4e4a6ba044)
        {
            return;
        }

        this.v1abd6dc7c758 = v0c4e4a6ba044;
        this.v5cf9661e09eb = 0.9f;
        if (this._core != null)
        {
            this._core.v4a67f96115e7(false);
            this._core.vcdab30192bd3(false);
            this._core.vc1be5bfc2b0a();
        }
    }

    

    private int vdf3d0191f85b(Vector3 position)
    {
        return position.x < 0f ? 0 : 1;
    }

    private v3b0744bb0222 v8465bb6263e8(int v720ad0d3a1a2, int va2f2beaaaa9a)
    {
        for (int i = 0; i < this._sockets.Count; i++)
        {
            v3b0744bb0222 v0c1aa4a9ea15 = this._sockets[i];
            if (v0c1aa4a9ea15.SectionIndex == v720ad0d3a1a2 && v0c1aa4a9ea15.Side == va2f2beaaaa9a)
            {
                return v0c1aa4a9ea15;
            }
        }

        return null;
    }

    
    private v3b0744bb0222 v5daac8dc8faa(Vector3 position)
    {
        float range = this.v3bf84419f291 * 0.47f;
        v3b0744bb0222 best = null;
        float bestScore = float.MaxValue;

        for (int i = 0; i < this._sockets.Count; i++)
        {
            v3b0744bb0222 v0c1aa4a9ea15 = this._sockets[i];
            Vector3 dock = v0c1aa4a9ea15.DockPoint;
            float dy = Mathf.Abs(dock.y - position.y);
            if (dy > range)
            {
                continue;
            }

            float score = dy + (Mathf.Abs(dock.x - position.x) * 0.25f);
            if (score < bestScore)
            {
                bestScore = score;
                best = v0c1aa4a9ea15;
            }
        }

        return best;
    }

    
    
    
    
    
    private v3b0744bb0222 ve9b850f24cea(Vector3 position)
    {
        int v2132923cb930 = Mathf.Min(this.v15f9e7d40341.TopSection, this.ve0e73a586cf0 + 1);
        v3b0744bb0222 left = this.v8465bb6263e8(v2132923cb930, 0);
        v3b0744bb0222 right = this.v8465bb6263e8(v2132923cb930, 1);

        if (left == null)
        {
            return right;
        }

        if (right == null)
        {
            return left;
        }

        return Mathf.Abs(left.DockPoint.x - position.x) <= Mathf.Abs(right.DockPoint.x - position.x) ? left : right;
    }
}
