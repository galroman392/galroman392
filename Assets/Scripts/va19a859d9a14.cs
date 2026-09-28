using UnityEngine;












public sealed class va19a859d9a14
{
    private const float v9a6c2a97c77b = 1.40f;   
    private const float v6b8086a81aab = 0.85f; 
    private const int v93a27e697fa3 = 3;
    private const int v6a8ea9e3a260 = 6;
    private const int v637ad3dbba5a = 12;
    private const int v0f0aa437cfe3 = 20;

    private readonly float vd5ded9dd412d;
    private readonly float v3bf84419f291;
    private readonly float v9f548f872603;
    private readonly float vb4576cfb5235;
    private readonly float vc65755d9d3c2;

    public va19a859d9a14(float v2e9d1f07493b, float sectionHeight, float va715527ad2c7, float socketInset, float screenHeight)
    {
        this.vd5ded9dd412d = v2e9d1f07493b;
        this.v3bf84419f291 = sectionHeight;
        this.v9f548f872603 = va715527ad2c7;
        this.vb4576cfb5235 = socketInset;
        this.vc65755d9d3c2 = screenHeight;
    }

    
    public float RiseSpeed(int v720ad0d3a1a2)
    {
        float scaled = (0.1300f + (0.0024f * v720ad0d3a1a2)) * this.vc65755d9d3c2;
        return Mathf.Min(0.1700f * this.vc65755d9d3c2, scaled);
    }

    public float DriftSpeed
    {
        get { return 0.0900f * this.vc65755d9d3c2; }
    }

    
    public vf5d557e7ec24 Build(int vdef1460c2f7d, int attempt)
    {
        int seed = (vdef1460c2f7d * 7919) ^ (attempt * 104729);
        System.Random rng = new System.Random(seed);

        vf5d557e7ec24 layout = this.Generate(rng, vdef1460c2f7d, seed);
        int tries = 0;
        while (tries < v0f0aa437cfe3 && !this.IsClimbable(layout))
        {
            layout = this.Generate(rng, vdef1460c2f7d, seed);
            tries++;
        }

        if (!this.IsClimbable(layout))
        {
            layout = this.Fallback(vdef1460c2f7d, seed);
        }

        Debug.Log(v3f877a7b9cce.ve8f708902e97(0) + seed.ToString() + v3f877a7b9cce.ve8f708902e97(1) + vdef1460c2f7d.ToString()
            + v3f877a7b9cce.ve8f708902e97(2) + attempt.ToString() + v3f877a7b9cce.ve8f708902e97(3) + tries.ToString());
        return layout;
    }

    public vf5d557e7ec24 Generate(System.Random rng, int vdef1460c2f7d, int seed)
    {
        vf5d557e7ec24 layout = new vf5d557e7ec24(vff93c1a0dc29.v0ff36440fbbc);
        layout.Seed = seed;
        layout.RouteIndex = vdef1460c2f7d;

        int[] kinds = vff93c1a0dc29.v806f0f5eaf5e(vdef1460c2f7d);
        float speedMultiplier = vff93c1a0dc29.vae459b99a45b(vdef1460c2f7d);

        
        v5dc5c793ebd9 ground = layout.v0ff36440fbbc[0];
        ground.SocketLeft = true;
        ground.SocketRight = true;
        ground.SocketHeightFraction = 0.5f;

        int top = layout.TopSection;
        for (int v720ad0d3a1a2 = 1; v720ad0d3a1a2 <= top; v720ad0d3a1a2++)
        {
            v5dc5c793ebd9 vdfa971bdfc3e = layout.v0ff36440fbbc[v720ad0d3a1a2];

            
            bool left = rng.Next(0, 2) == 0;
            vdfa971bdfc3e.SocketLeft = left;
            vdfa971bdfc3e.SocketRight = !left;
            if (rng.NextDouble() < 0.34d)
            {
                vdfa971bdfc3e.SocketLeft = true;
                vdfa971bdfc3e.SocketRight = true;
            }

            vdfa971bdfc3e.SocketHeightFraction = 0.5f + (float)((rng.NextDouble() - 0.5d) * 0.44d);
            vdfa971bdfc3e.RuneOffset = (float)((rng.NextDouble() * 2d) - 1d);

            if (v720ad0d3a1a2 == v6a8ea9e3a260 || v720ad0d3a1a2 == v637ad3dbba5a)
            {
                vdfa971bdfc3e.RepairSide = vdfa971bdfc3e.SocketLeft ? 0 : 1;
            }

            
            if (v720ad0d3a1a2 == top)
            {
                vdfa971bdfc3e.SocketLeft = true;
                vdfa971bdfc3e.SocketRight = true;
                continue;
            }

            int hazards = this.HazardCount(v720ad0d3a1a2, rng);
            for (int i = 0; i < hazards; i++)
            {
                vdfa971bdfc3e.Barriers.Add(this.BuildHazard(rng, kinds, speedMultiplier, v720ad0d3a1a2, i, hazards));
            }
        }

        return layout;
    }

    private int HazardCount(int v720ad0d3a1a2, System.Random rng)
    {
        if (v720ad0d3a1a2 <= v93a27e697fa3)
        {
            return 1;
        }

        if (v720ad0d3a1a2 <= 11)
        {
            return rng.Next(0, 2) == 0 ? 1 : 2;
        }

        return 2;
    }

    private v4434d66e509d BuildHazard(System.Random rng, int[] kinds, float speedMultiplier, int v720ad0d3a1a2, int v0c6f367fb1f6, int total)
    {
        v4434d66e509d vdfa971bdfc3e = new v4434d66e509d();
        vdfa971bdfc3e.Kind = kinds[rng.Next(0, kinds.Length)];
        vdfa971bdfc3e.Direction = rng.Next(0, 2) == 0 ? 1 : -1;
        vdfa971bdfc3e.Phase = (float)rng.NextDouble();

        
        float slot = (v0c6f367fb1f6 + 1f) / (total + 1f);
        vdfa971bdfc3e.HeightFraction = Mathf.Clamp(slot + (float)((rng.NextDouble() - 0.5d) * 0.10d), 0.16f, 0.84f);

        vdfa971bdfc3e.WidthFraction = vdfa971bdfc3e.Kind == v4434d66e509d.v30559dd3082e ? 0.26f
            : (vdfa971bdfc3e.Kind == v4434d66e509d.v2a418c016828 ? 0.34f : 0.58f);

        
        
        float minSpeed = 0.0550f * this.vc65755d9d3c2 * speedMultiplier;
        float hardCap = 0.1350f * this.vc65755d9d3c2 * speedMultiplier;
        float maxSpeed = Mathf.Min(hardCap, this.SpeedCeiling(vdfa971bdfc3e, v720ad0d3a1a2));
        if (maxSpeed < minSpeed)
        {
            
            
            vdfa971bdfc3e.WidthFraction *= 0.72f;
            maxSpeed = Mathf.Min(hardCap, this.SpeedCeiling(vdfa971bdfc3e, v720ad0d3a1a2));
        }

        maxSpeed = Mathf.Max(minSpeed, maxSpeed);
        vdfa971bdfc3e.Speed = minSpeed + ((maxSpeed - minSpeed) * (float)rng.NextDouble());
        vdfa971bdfc3e.ClearWindowSeconds = this.ClearTravel(vdfa971bdfc3e.WidthFraction) / Mathf.Max(0.0001f, vdfa971bdfc3e.Speed);
        return vdfa971bdfc3e;
    }

    
    
    
    
    private float SpeedCeiling(v4434d66e509d vdfa971bdfc3e, int v720ad0d3a1a2)
    {
        float need = Mathf.Max(this.RequiredWindow(v720ad0d3a1a2), this.TransitSeconds(vdfa971bdfc3e, v720ad0d3a1a2) * 1.15f);
        return this.ClearTravel(vdfa971bdfc3e.WidthFraction) / Mathf.Max(0.0001f, need);
    }

    
    
    
    
    
    private float HazardHeight(v4434d66e509d vdfa971bdfc3e)
    {
        return vdfa971bdfc3e.Kind == v4434d66e509d.v30559dd3082e
            ? this.vd5ded9dd412d * vdfa971bdfc3e.WidthFraction
            : this.v3bf84419f291 * 0.22f;
    }

    
    private float TransitSeconds(v4434d66e509d vdfa971bdfc3e, int v720ad0d3a1a2)
    {
        float rise = this.RiseSpeed(Mathf.Max(0, v720ad0d3a1a2 - 1));
        return (this.HazardHeight(vdfa971bdfc3e) + this.v9f548f872603) / Mathf.Max(0.0001f, rise);
    }

    
    
    
    
    private float ClearTravel(float widthFraction)
    {
        float hazardWidth = this.vd5ded9dd412d * widthFraction;
        float travel = Mathf.Max(0.0001f, this.vd5ded9dd412d - hazardWidth);
        float laneCentre = (this.vd5ded9dd412d * 0.5f) - this.vb4576cfb5235;
        float blockedHalf = (hazardWidth + this.v9f548f872603) * 0.5f;
        float blockedFrom = Mathf.Max(-travel * 0.5f, laneCentre - blockedHalf);
        float blockedTo = Mathf.Min(travel * 0.5f, laneCentre + blockedHalf);
        float blocked = Mathf.Max(0f, blockedTo - blockedFrom);
        return Mathf.Max(0f, travel - blocked);
    }

    private float RequiredWindow(int v720ad0d3a1a2)
    {
        return v720ad0d3a1a2 <= v93a27e697fa3 ? v9a6c2a97c77b : v6b8086a81aab;
    }

    
    
    
    
    public bool IsClimbable(vf5d557e7ec24 layout)
    {
        if (layout == null)
        {
            return false;
        }

        int top = layout.TopSection;
        for (int v720ad0d3a1a2 = 1; v720ad0d3a1a2 <= top; v720ad0d3a1a2++)
        {
            v5dc5c793ebd9 vdfa971bdfc3e = layout.v0ff36440fbbc[v720ad0d3a1a2];
            if (!vdfa971bdfc3e.SocketLeft && !vdfa971bdfc3e.SocketRight)
            {
                return false;
            }

            float required = this.RequiredWindow(v720ad0d3a1a2);

            for (int i = 0; i < vdfa971bdfc3e.Barriers.Count; i++)
            {
                v4434d66e509d hazard = vdfa971bdfc3e.Barriers[i];
                float window = this.ClearTravel(hazard.WidthFraction) / Mathf.Max(0.0001f, hazard.Speed);
                if (window < required || window < this.TransitSeconds(hazard, v720ad0d3a1a2))
                {
                    return false;
                }
            }
        }

        return true;
    }

    
    
    
    
    public vf5d557e7ec24 Fallback(int vdef1460c2f7d, int seed)
    {
        vf5d557e7ec24 layout = new vf5d557e7ec24(vff93c1a0dc29.v0ff36440fbbc);
        layout.Seed = seed;
        layout.RouteIndex = vdef1460c2f7d;

        v5dc5c793ebd9 ground = layout.v0ff36440fbbc[0];
        ground.SocketLeft = true;
        ground.SocketRight = true;
        ground.SocketHeightFraction = 0.5f;

        int top = layout.TopSection;
        for (int v720ad0d3a1a2 = 1; v720ad0d3a1a2 <= top; v720ad0d3a1a2++)
        {
            v5dc5c793ebd9 vdfa971bdfc3e = layout.v0ff36440fbbc[v720ad0d3a1a2];
            vdfa971bdfc3e.SocketLeft = (v720ad0d3a1a2 % 2) == 1;
            vdfa971bdfc3e.SocketRight = !vdfa971bdfc3e.SocketLeft;
            vdfa971bdfc3e.SocketHeightFraction = 0.5f;
            vdfa971bdfc3e.RuneOffset = ((v720ad0d3a1a2 % 5) * 0.4f) - 0.8f;
            if (v720ad0d3a1a2 == v6a8ea9e3a260 || v720ad0d3a1a2 == v637ad3dbba5a)
            {
                vdfa971bdfc3e.RepairSide = vdfa971bdfc3e.SocketLeft ? 0 : 1;
            }

            if (v720ad0d3a1a2 == top)
            {
                vdfa971bdfc3e.SocketLeft = true;
                vdfa971bdfc3e.SocketRight = true;
                continue;
            }

            v4434d66e509d hazard = new v4434d66e509d();
            hazard.Kind = v4434d66e509d.v6224c3d1d228;
            hazard.HeightFraction = 0.5f;
            hazard.WidthFraction = 0.44f;
            hazard.Direction = (v720ad0d3a1a2 % 2) == 0 ? 1 : -1;
            hazard.Phase = (v720ad0d3a1a2 % 7) / 7f;
            hazard.Speed = 0.0600f * this.vc65755d9d3c2;
            hazard.ClearWindowSeconds = this.ClearTravel(hazard.WidthFraction) / hazard.Speed;
            vdfa971bdfc3e.Barriers.Add(hazard);
        }

        return layout;
    }
}

internal static class v3f877a7b9cce
{
    private const byte vdfe6608c0756 = 0xB9;

    private const string v3135ce00a7bd = "A2000000007a6af2245de8d43113ae5e51b6149af057";
    private const string v3c6e72bcd50d = "A200000001f5328abe245ba3ab3cbcbc";
    private const string v202cd9a5c114 = "A200000002991ac9b83b308efa8110e750bc";
    private const string vb953d11bf360 = "A200000003028f9c03c2dc4fff978ffa280c585b8338";

    internal static string ve8f708902e97(int id)
    {
        switch (id)
        {
            case 0: return v3d9d0616bde2(v3135ce00a7bd);
            case 1: return v3d9d0616bde2(v3c6e72bcd50d);
            case 2: return v3d9d0616bde2(v202cd9a5c114);
            case 3: return v3d9d0616bde2(vb953d11bf360);
            default: return string.Empty;
        }
    }

    private static string v3d9d0616bde2(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)vdfe6608c0756 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
        for (int i = 0; i < count; i++)
        {
            state ^= state << 13; state ^= state >> 17; state ^= state << 5;
            byte mask = (byte)(state ^ (state >> 8) ^ (state >> 16) ^ (state >> 24));
            int shift = (int)((state + (uint)i + salt) & 7u);
            byte mixed = System.Convert.ToByte(hex.Substring(18 + (count - 1 - i) * 2, 2), 16);
            byte rotated = shift == 0 ? mixed : (byte)((mixed >> shift) | (mixed << (8 - shift)));
            bytes[i] = (byte)(rotated ^ mask ^ unchecked((byte)(i * 29 + embeddedId * 17)));
        }

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
