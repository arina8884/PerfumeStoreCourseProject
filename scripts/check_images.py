import re
from pathlib import Path

sql = Path(r"C:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Database\02_TestData.sql").read_text(encoding="utf-8")
out = Path(r"C:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Source\PerfumeStore.MVCC\PerfumeStore.MVCC\wwwroot\images\products")

db = set(re.findall(r"/images/products/([^']+)", sql))
disk = {p.name for p in out.glob("*.webp")}
missing = sorted(db - disk)
extra = sorted(disk - db)
print("DB:", len(db), "Disk:", len(disk))
print("Missing:", missing)
print("Extra:", extra)
