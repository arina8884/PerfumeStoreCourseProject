# -*- coding: utf-8 -*-
"""Remove trailing periods from UI Subtitle strings in Razor views."""
import re
from pathlib import Path

views = Path(__file__).resolve().parents[1] / "Source" / "PerfumeStore.MVCC" / "PerfumeStore.MVCC" / "Views"
count = 0
for path in views.rglob("*.cshtml"):
    text = path.read_text(encoding="utf-8")
    original = text

    def strip_subtitle(m: re.Match) -> str:
        inner = m.group(1)
        if inner.endswith("."):
            return f'Subtitle = "{inner[:-1]}"'
        return m.group(0)

    text = re.sub(r'Subtitle = "([^"]+)"', strip_subtitle, text)

    def strip_short_desc(m: re.Match) -> str:
        inner = m.group(1)
        if len(inner) < 72 and inner.endswith("."):
            return f'Description = "{inner[:-1]}"'
        return m.group(0)

    text = re.sub(r'Description = "([^"]+)"', strip_short_desc, text)

    if text != original:
        path.write_text(text, encoding="utf-8")
        count += 1

print(f"Updated {count} files")
