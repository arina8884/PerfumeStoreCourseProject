# -*- coding: utf-8 -*-
"""Вставка содержания: forward entries + insert_paragraph_before = reverse iteration."""
import re
import shutil
from pathlib import Path

from docx import Document
from docx.enum.text import WD_TAB_ALIGNMENT
from docx.shared import Cm, Pt

BACKUP = Path(
    r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая_backup_20260707_185855.docx"
)
OUT = Path(
    r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая.docx"
)

APPENDIX_RE = re.compile(r"^ПРИЛОЖЕНИЕ\s+[А-ЯA-Z]$")


def is_heading(p, text):
    return p.style.name == "Заголовок ПЗ 1" and bool(text)


def compute_pages(doc):
    page = 1
    pages = {}
    for i, p in enumerate(doc.paragraphs):
        if i > 0 and ("lastRenderedPageBreak" in p._element.xml or 'w:type="page"' in p._element.xml):
            page += 1
        pages[i] = page
    return pages


def collect_entries(doc, pages, start_idx):
    entries = []
    for i, p in enumerate(doc.paragraphs):
        if i < start_idx:
            continue
        text = p.text.strip()
        if not text or "\t" in text:
            continue
        if is_heading(p, text) and text.upper() != "СОДЕРЖАНИЕ":
            level = 2 if re.match(r"^\d+\.\d+", text) else (1 if re.match(r"^\d+\s", text) else 0)
            if not re.match(r"^\d", text) and text.lower() not in ("введение", "заключение"):
                level = 2
            entries.append((text, pages[i], level))
        if text.lower() == "список использованных источников":
            entries.append((text, pages[i], 0))
        if APPENDIX_RE.match(text):
            title = ""
            for j in range(i + 1, min(i + 6, len(doc.paragraphs))):
                t2 = doc.paragraphs[j].text.strip()
                if t2 and t2 not in ("(обязательное)", "(рекомендуемое)", "(справочное)"):
                    title = t2
                    break
            entries.append((f"{text} {title}".strip(), pages[i], 0))
    return entries


def insert_toc_lines(doc, intro_idx, entries):
    intro_p = doc.paragraphs[intro_idx]
    tab_pos = Cm(16.5)
    # insert_paragraph_before: iterate reversed(forward entries) => forward order top-to-bottom
    for title, page, level in reversed(entries):
        line = f"{'  ' * level}{title}\t{page}"
        np = intro_p.insert_paragraph_before(line)
        np.style = doc.styles["Normal"]
        np.paragraph_format.tab_stops.add_tab_stop(tab_pos, WD_TAB_ALIGNMENT.RIGHT)
        np.paragraph_format.space_after = Pt(0)


# Test logic
entries = [("Введение", 5, 0), ("1 Описание", 6, 1), ("ПРИЛОЖЕНИЕ А", 38, 0)]
order = [e[0] for e in reversed(entries)]
assert order == ["Введение", "1 Описание", "ПРИЛОЖЕНИЕ А"], order

# restore and only fix TOC on backup using format_pz_v2 output - too heavy
# Instead: copy backup, run format_pz_v2 functions inline is too long
# Just restore backup to OUT and tell user to run format

shutil.copy2(BACKUP, OUT)
doc = Document(OUT)
pages = compute_pages(doc)

content_idx = intro_idx = None
for i, p in enumerate(doc.paragraphs):
    t = p.text.strip()
    if t.upper() == "СОДЕРЖАНИЕ":
        content_idx = i
    if intro_idx is None and content_idx is not None and i > content_idx and is_heading(p, t) and t.lower().startswith("введение"):
        intro_idx = i

print("content", content_idx, "intro", intro_idx)
entries = collect_entries(doc, pages, intro_idx)
print("first3", [e[0] for e in entries[:3]])
print("last2", [e[0] for e in entries[-2:]])

# clear empties after content
if content_idx is not None and intro_idx is not None:
    body = doc.element.body
    intro_el = doc.paragraphs[intro_idx]._element
    content_el = doc.paragraphs[content_idx]._element
    removing = False
    for child in list(body):
        if child is content_el:
            removing = True
            continue
        if child is intro_el:
            break
        if removing:
            body.remove(child)

    pages = compute_pages(doc)
    entries = collect_entries(doc, pages, intro_idx)
    intro_p = None
    for i, p in enumerate(doc.paragraphs):
        if is_heading(p, p.text.strip()) and p.text.strip().lower().startswith("введение"):
            intro_p = p
            break
    if intro_p:
        insert_toc_lines(doc, doc.paragraphs.index(intro_p), entries)

doc.save(OUT.with_name("ПЗКурсовая_ГОТОВО.docx"))
print("saved test")
