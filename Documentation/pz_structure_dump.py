# -*- coding: utf-8 -*-
from docx import Document

PZ_PATH = r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая.docx"
OUT = r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\pz_structure.txt"

doc = Document(PZ_PATH)
lines = []

# paragraphs 0-120
for i in range(0, 130):
    if i >= len(doc.paragraphs):
        break
    p = doc.paragraphs[i]
    t = p.text
    if t.strip() or i < 55:
        r = p.runs[0] if p.runs else None
        bold = any(run.bold for run in p.runs if run.text.strip()) if p.runs else None
        lines.append(f"{i:4d}|{p.style.name:18s}|b={bold}| {repr(t[:100])}")

lines.append("\n--- Sources area ---")
for i in range(505, 525):
    if i < len(doc.paragraphs):
        p = doc.paragraphs[i]
        lines.append(f"{i:4d}|{p.style.name}| {p.text[:120]}")

lines.append("\n--- Appendix A start ---")
for i in range(520, 545):
    if i < len(doc.paragraphs):
        p = doc.paragraphs[i]
        if p.text.strip() or i < 530:
            lines.append(f"{i:4d}|{p.style.name}| {p.text[:100]}")

with open(OUT, "w", encoding="utf-8") as f:
    f.write("\n".join(lines))
print("done")
