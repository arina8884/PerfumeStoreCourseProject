# -*- coding: utf-8 -*-
"""Generate IMAGE_MANIFEST.md from PerfumeStore products."""
import subprocess
import re
from pathlib import Path

BRAND_SOURCES = {
    "Byredo": "https://www.byredo.com/eu_en/",
    "Carolina Herrera": "https://www.carolinaherrera.com/ww/en/fragrances",
    "Chanel": "https://www.chanel.com/us/fragrance/",
    "Creed": "https://www.creedboutique.com/",
    "Dior": "https://www.dior.com/en_be/fragrance",
    "Dolce & Gabbana": "https://www.dolcegabbana.com/en/fragrances/",
    "Giorgio Armani": "https://www.giorgioarmanibeauty.com/us/en/fragrances/",
    "Guerlain": "https://www.guerlain.com/us/en-us/fragrance",
    "Jo Malone": "https://www.jomalone.com/",
    "Kilian": "https://www.bykilian.com/",
    "Maison Francis Kurkdjian": "https://www.franciskurkdjian.com/int-en/",
    "Mancera": "https://www.mancera.com/",
    "Montale": "https://www.montale.com/",
    "Paco Rabanne": "https://www.pacorabanne.com/fragrance/",
    "Prada": "https://www.prada.com/ww/en/beauty/fragrances.html",
    "Tom Ford": "https://www.tomfordbeauty.com/fragrance/",
    "Valentino": "https://www.valentino.com/en-us/beauty/fragrances",
    "Versace": "https://www.versace.com/international/en/fragrances/",
    "Xerjoff": "https://www.xerjoff.com/",
    "Yves Saint Laurent": "https://www.yslbeauty.com/fragrance/",
}

FALLBACK = "https://www.sephora.com/shop/perfume"

result = subprocess.run(
    [
        "sqlcmd",
        "-S", "WIN-D5AGENI8RMR\\SQLEXPRESS",
        "-d", "PerfumeStore",
        "-E",
        "-Q",
        "SET NOCOUNT ON; SELECT Name, Brand, Volume, ImageUrl FROM Products ORDER BY Brand, Name, Volume;",
        "-W", "-s", "|", "-h", "-1",
    ],
    capture_output=True,
    text=True,
    encoding="utf-8",
    errors="replace",
)

lines = []
for raw in result.stdout.splitlines():
    raw = raw.strip()
    if not raw or raw.startswith("(") or "|" not in raw:
        continue
    parts = [p.strip() for p in raw.split("|")]
    if len(parts) < 4:
        continue
    name, brand, volume, image_url = parts[0], parts[1], parts[2], parts[3]
    if not image_url.startswith("/images/products/"):
        continue
    filename = image_url.rsplit("/", 1)[-1]
    title = f"{brand} {name} {volume} мл"
    source = BRAND_SOURCES.get(brand, FALLBACK)
    lines.append((title, filename, image_url, source))

out = Path(__file__).parent / "IMAGE_MANIFEST.md"
buf = [
    "# IMAGE_MANIFEST — ручная загрузка изображений товаров",
    "",
    "Положите каждый файл в `Source/PerfumeStore.MVCC/PerfumeStore.MVCC/wwwroot/images/products/`.",
    "",
    "Формат: **WEBP**, рекомендуемый размер **800×800 px**, фон нейтральный или прозрачный.",
    "",
    "Не скачивать автоматически. Используйте официальные пресс-материалы бренда или каталог ритейлера.",
    "",
    f"**Всего товаров:** {len(lines)}",
    "",
    "| № | Товар | Файл | ImageUrl | Источник |",
    "|---|-------|------|----------|----------|",
]
for i, (title, filename, url, source) in enumerate(lines, 1):
    safe_title = title.replace("|", "\\|")
    buf.append(f"| {i} | {safe_title} | `{filename}` | `{url}` | {source} |")

buf.extend([
    "",
    "---",
    "",
    "## Hero главной страницы",
    "",
    "| Файл | Путь | ImageUrl | Источник |",
    "|------|------|----------|----------|",
    "| `hero-main.webp` | `wwwroot/images/hero/hero-main.webp` | `/images/hero/hero-main.webp` | https://unsplash.com/s/photos/perfume-boutique (для ручного подбора) или собственное фото |",
    "",
    "## Статические ассеты интерфейса",
    "",
    "| Файл | Назначение |",
    "|------|------------|",
    "| `logo.svg` | Wordmark в шапке и подвале |",
    "| `favicon.svg` | Иконка вкладки |",
    "| `no-image.png` | Placeholder до загрузки фото товара |",
    "| `hero-placeholder.svg` | Заглушка Hero, если `hero-main.webp` отсутствует |",
])

out.write_text("\n".join(buf) + "\n", encoding="utf-8")
print(f"Wrote {len(lines)} products to {out}")
