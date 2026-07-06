"""
Download product images from Openverse (Creative Commons) and save as WebP.
Falls back to same-brand image or generated bottle placeholder.
"""
import io
import json
import re
import shutil
import time
import urllib.parse
import urllib.request
from collections import defaultdict
from pathlib import Path

try:
    from PIL import Image, ImageDraw, ImageFont, ImageFilter
except ImportError:
    import subprocess
    subprocess.check_call(["pip", "install", "Pillow", "-q"])
    from PIL import Image, ImageDraw, ImageFont, ImageFilter

ROOT = Path(r"C:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject")
SQL_PATH = ROOT / "Database" / "02_TestData.sql"
OUT_DIR = ROOT / "Source" / "PerfumeStore.MVCC" / "PerfumeStore.MVCC" / "wwwroot" / "images" / "products"
HERO_DIR = ROOT / "Source" / "PerfumeStore.MVCC" / "PerfumeStore.MVCC" / "wwwroot" / "images" / "hero"

USER_AGENT = "PerfumeStoreCourseProject/1.0 (educational demo)"

BRAND_COLORS = {
    "Dior": (28, 32, 48),
    "Chanel": (0, 0, 0),
    "Creed": (18, 48, 72),
    "Paco Rabanne": (180, 140, 40),
    "Tom Ford": (45, 38, 35),
    "Versace": (180, 160, 50),
    "Giorgio Armani": (55, 55, 60),
    "Yves Saint Laurent": (0, 0, 0),
    "Dolce & Gabbana": (120, 30, 35),
    "Guerlain": (90, 20, 35),
    "Byredo": (240, 240, 238),
    "Carolina Herrera": (20, 20, 25),
    "Jo Malone": (200, 195, 185),
    "Kilian": (15, 15, 15),
    "Maison Francis Kurkdjian": (210, 195, 175),
    "Mancera": (120, 25, 30),
    "Montale": (85, 55, 35),
    "Prada": (0, 0, 0),
    "Valentino": (140, 20, 45),
    "Xerjoff": (160, 130, 70),
}


def parse_products():
    data = SQL_PATH.read_text(encoding="utf-8")
    pattern = re.compile(
        r"N'([^']+)', N'([^']+)', (\d+), (\d+), N'[^']*', \d+, N'/images/products/([^']+)'"
    )
    products = []
    for m in pattern.finditer(data):
        name, brand, price, volume, image = m.groups()
        products.append({
            "name": name,
            "brand": brand,
            "price": int(price),
            "volume": int(volume),
            "image": image,
        })
    return products


def http_get(url, timeout=25):
    req = urllib.request.Request(url, headers={"User-Agent": USER_AGENT})
    with urllib.request.urlopen(req, timeout=timeout) as resp:
        return resp.read()


def search_openverse(query):
    q = urllib.parse.quote(query)
    url = f"https://api.openverse.engineering/v1/images/?q={q}&page_size=5&license_type=commercial,modification"
    try:
        raw = http_get(url)
        data = json.loads(raw.decode("utf-8"))
        for item in data.get("results", []):
            img_url = item.get("url")
            if img_url and img_url.startswith("http"):
                return img_url
    except Exception as exc:
        print(f"  Openverse search failed for '{query}': {exc}")
    return None


def download_image(url):
    try:
        return http_get(url)
    except Exception as exc:
        print(f"  Download failed {url}: {exc}")
        return None


def save_as_webp(raw_bytes, dest: Path, size=800):
    img = Image.open(io.BytesIO(raw_bytes))
    if img.mode not in ("RGB", "RGBA"):
        img = img.convert("RGBA")
    bg = Image.new("RGB", img.size, (250, 247, 244))
    if img.mode == "RGBA":
        bg.paste(img, mask=img.split()[3])
        img = bg
    else:
        img = img.convert("RGB")

    img.thumbnail((size, size), Image.Resampling.LANCZOS)
    canvas = Image.new("RGB", (size, size), (250, 247, 244))
    ox = (size - img.width) // 2
    oy = (size - img.height) // 2
    canvas.paste(img, (ox, oy))
    canvas.save(dest, "WEBP", quality=88, method=6)


def brand_key_from_filename(filename):
    # e.g. dior-sauvage-eau-de-toilette-60.webp -> dior
    return filename.split("-")[0]


def generate_bottle(brand, name, volume, dest: Path, size=800):
    color = BRAND_COLORS.get(brand, (50, 48, 46))
    img = Image.new("RGB", (size, size), (248, 245, 242))
    draw = ImageDraw.Draw(img)

    # soft vignette
    for i in range(40):
        alpha = int(8 + i * 0.5)
        draw.ellipse((80 - i, 120 - i, size - 80 + i, size - 60 + i), outline=(230, 225, 220))

    cx = size // 2
    # bottle body
    body_top = 200
    body_bottom = 620
    body_w = 140
    draw.rounded_rectangle(
        (cx - body_w // 2, body_top, cx + body_w // 2, body_bottom),
        radius=28,
        fill=tuple(min(c + 35, 255) for c in color),
        outline=tuple(max(c - 20, 0) for c in color),
        width=3,
    )
    # neck
    draw.rectangle((cx - 28, 150, cx + 28, body_top), fill=tuple(min(c + 20, 255) for c in color))
    draw.rectangle((cx - 38, 120, cx + 38, 155), fill=tuple(max(c - 10, 0) for c in color))
    # cap
    draw.rounded_rectangle((cx - 42, 70, cx + 42, 125), radius=8, fill=(210, 205, 200))

    # reflection
    draw.rounded_rectangle(
        (cx - body_w // 2 + 18, body_top + 30, cx - body_w // 2 + 38, body_bottom - 80),
        radius=12,
        fill=(255, 255, 255, 40) if hasattr(Image, "Resampling") else (255, 255, 255),
    )

    try:
        font_b = ImageFont.truetype("arial.ttf", 22)
        font_s = ImageFont.truetype("arial.ttf", 16)
    except OSError:
        font_b = ImageFont.load_default()
        font_s = font_b

    brand_text = brand[:28]
    name_text = (name[:36] + "…") if len(name) > 36 else name
    vol_text = f"{volume} мл"

    draw.text((cx, 660), brand_text, fill=(60, 58, 55), anchor="mm", font=font_b)
    draw.text((cx, 690), name_text, fill=(120, 115, 110), anchor="mm", font=font_s)
    draw.text((cx, 720), vol_text, fill=(150, 145, 140), anchor="mm", font=font_s)

    img.save(dest, "WEBP", quality=90, method=6)


def build_search_query(brand, name):
    return f"{brand} {name} perfume bottle"


def main():
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    HERO_DIR.mkdir(parents=True, exist_ok=True)

    products = parse_products()
    by_file = {p["image"]: p for p in products}
    brand_cache = {}

    print(f"Processing {len(by_file)} product images...")

    for i, (filename, product) in enumerate(sorted(by_file.items()), 1):
        dest = OUT_DIR / filename
        if dest.exists() and dest.stat().st_size > 5000:
            print(f"[{i}/{len(by_file)}] skip existing {filename}")
            brand_cache.setdefault(product["brand"], dest)
            continue

        query = build_search_query(product["brand"], product["name"])
        print(f"[{i}/{len(by_file)}] {filename} <- {query}")

        img_url = search_openverse(query)
        if not img_url:
            img_url = search_openverse(f"{product['brand']} perfume")

        success = False
        if img_url:
            raw = download_image(img_url)
            if raw:
                try:
                    save_as_webp(raw, dest)
                    success = True
                    print(f"  saved from {img_url[:80]}...")
                except Exception as exc:
                    print(f"  convert failed: {exc}")

        if not success and product["brand"] in brand_cache:
            shutil.copy2(brand_cache[product["brand"]], dest)
            print(f"  copied from same brand cache")
            success = True

        if not success:
            generate_bottle(product["brand"], product["name"], product["volume"], dest)
            print("  generated placeholder bottle")

        brand_cache.setdefault(product["brand"], dest)
        time.sleep(0.35)

    # Hero image
    hero_dest = HERO_DIR / "hero-main.webp"
    if not hero_dest.exists() or hero_dest.stat().st_size < 5000:
        print("Downloading hero image...")
        hero_url = search_openverse("luxury perfume boutique interior")
        if not hero_url:
            hero_url = search_openverse("perfume store shelf")
        if hero_url:
            raw = download_image(hero_url)
            if raw:
                save_as_webp(raw, hero_dest, size=1200)
                print("Hero saved")
        if not hero_dest.exists():
            # wide boutique gradient
            w, h = 1400, 900
            img = Image.new("RGB", (w, h), (35, 32, 30))
            draw = ImageDraw.Draw(img)
            for x in range(w):
                t = x / w
                r = int(35 + t * 40)
                g = int(32 + t * 30)
                b = int(30 + t * 25)
                draw.line([(x, 0), (x, h)], fill=(r, g, b))
            draw.rounded_rectangle((w // 2 - 200, 200, w // 2 + 200, 700), radius=40, fill=(60, 55, 50))
            img.save(hero_dest, "WEBP", quality=90)
            print("Hero generated")

    count = len(list(OUT_DIR.glob("*.webp")))
    print(f"Done. {count} webp files in products folder.")


if __name__ == "__main__":
    main()
