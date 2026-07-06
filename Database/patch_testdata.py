# -*- coding: utf-8 -*-
import re
from pathlib import Path

path = Path(__file__).parent / "02_TestData.sql"
content = path.read_text(encoding="utf-8")
tiers = [39, 59, 79, 99, 129, 159, 199, 249, 299, 389]


def nearest(price: float) -> int:
    return min(tiers, key=lambda t: abs(t - price))


def round_price(match: re.Match) -> str:
    brand = match.group(1)
    price = float(match.group(2))
    volume = match.group(3)
    return f", N'{brand}', {nearest(price)}, {volume},"


content = re.sub(
    r", N'([^']+)',\s*(\d+(?:\.\d+)?),\s*(\d+),",
    round_price,
    content,
)

content = content.replace("N\ufffd5", "N\u00b05")
content = re.sub(
    r"p\.Name = N'N.5 Eau de Parfum'",
    "p.Name = N'N\u00b05 Eau de Parfum'",
    content,
)

niche = [
    "Creed",
    "Byredo",
    "Maison Francis Kurkdjian",
    "Mancera",
    "Montale",
    "Xerjoff",
    "Kilian",
    "Tom Ford",
]
men = [
    "Sauvage",
    "Fahrenheit",
    "Bleu de Chanel",
    "Acqua di Gio",
    "Stronger With You",
    "Eros",
    "La Nuit de L",
    "Y Eau de Parfum",
    "1 Million",
    "Invictus",
]
women = [
    "J'adore",
    "Miss Dior",
    "N\u00b05",
    "Coco Mademoiselle",
    "Chance Eau Tendre",
    "Black Opium",
    "Libre Eau de Parfum",
    "Good Girl",
    "Bright Crystal",
    "Donna Born In Roma",
]

lines = []
for line in content.splitlines():
    if "SELECT c.Id, N" in line and "FROM Categories c WHERE" in line:
        cat = None
        for brand in niche:
            if f"N'{brand}'" in line:
                cat = "Нишевая парфюмерия"
                break
        if not cat:
            for name in men:
                if f"N'{name}" in line:
                    cat = "Мужские"
                    break
        if not cat:
            for name in women:
                if f"N'{name}" in line:
                    cat = "Женские"
                    break
        if cat:
            line = re.sub(
                r"WHERE c\.Name = N'[^']+'",
                f"WHERE c.Name = N'{cat}'",
                line,
            )
    lines.append(line)

path.write_text("\n".join(lines) + "\n", encoding="utf-8")
sample = [l for l in lines if "Sauvage Eau de Toilette" in l and "60," in l][0]
print(sample[:140])
