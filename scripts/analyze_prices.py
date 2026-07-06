import re
from collections import defaultdict

sql_path = r"C:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Database\02_TestData.sql"
data = open(sql_path, encoding="utf-8").read()

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

groups = defaultdict(list)
for p in products:
    key = (p["brand"], p["name"])
    groups[key].append(p)

issues = []
for key, items in sorted(groups.items()):
    items = sorted(items, key=lambda x: x["volume"])
    if len(items) < 2:
        continue
    for i in range(len(items) - 1):
        a, b = items[i], items[i + 1]
        if b["price"] <= a["price"]:
            issues.append((key, a, b))

print(f"Total products: {len(products)}")
print(f"Groups with volume variants: {sum(1 for g in groups.values() if len(g) > 1)}")
print(f"Price issues: {len(issues)}")
for key, a, b in issues[:30]:
    print(f"  {key[0]} | {key[1]} | {a['volume']}ml={a['price']} -> {b['volume']}ml={b['price']}")
