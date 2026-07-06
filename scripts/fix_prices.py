"""Fix product prices in 02_TestData.sql so larger volumes cost more."""
import re
from collections import defaultdict
from pathlib import Path

SQL_PATH = Path(r"C:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Database\02_TestData.sql")

pattern = re.compile(
    r"(INSERT INTO Products \(CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive\)\s*"
    r"SELECT c\.Id, N'([^']+)', N'([^']+)', )(\d+)(, (\d+), N'[^']*', \d+, N'/images/products/[^']+', 1 FROM Categories c WHERE c\.Name = N'[^']+';)"
)

text = SQL_PATH.read_text(encoding="utf-8")
matches = list(pattern.finditer(text))

entries = []
for idx, m in enumerate(matches):
    name, brand, price, rest, volume = m.group(2), m.group(3), int(m.group(4)), m.group(5), int(m.group(6))
    entries.append({
        "idx": idx,
        "name": name,
        "brand": brand,
        "price": price,
        "volume": volume,
        "match": m,
    })

groups = defaultdict(list)
for e in entries:
    groups[(e["brand"], e["name"])].append(e)

for key, items in groups.items():
    items.sort(key=lambda x: x["volume"])
    for i in range(1, len(items)):
        prev, cur = items[i - 1], items[i]
        if cur["price"] <= prev["price"]:
            per_ml = prev["price"] / prev["volume"]
            bump = max(15, int((cur["volume"] - prev["volume"]) * per_ml * 0.82))
            cur["price"] = prev["price"] + bump

# Rebuild SQL by replacing prices in reverse order to preserve positions
new_text = text
for e in sorted(entries, key=lambda x: x["match"].start(), reverse=True):
    m = e["match"]
    old_fragment = m.group(0)
    new_fragment = (
        f"INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)\n"
        f"SELECT c.Id, N'{e['name']}', N'{e['brand']}', {e['price']}, {e['volume']},"
    )
    # replace only price in the matched line - safer approach
    prefix = m.group(1)
    suffix = m.group(5)
    replacement = f"{prefix}{e['price']}{suffix}"
    new_text = new_text[: m.start()] + replacement + new_text[m.end() :]

SQL_PATH.write_text(new_text, encoding="utf-8")
print("Prices updated in 02_TestData.sql")
