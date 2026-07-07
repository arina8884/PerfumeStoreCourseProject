# -*- coding: utf-8 -*-
"""Пересборка содержания в правильном порядке."""
import re
import shutil
from pathlib import Path

from docx import Document
from docx.enum.text import WD_TAB_ALIGNMENT
from docx.shared import Cm, Pt

DOC = Path(
    r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая.docx"
)
OUT = DOC.with_name("ПЗКурсовая_ГОТОВО.docx")

APPENDIX_RE = re.compile(r"^ПРИЛОЖЕНИЕ\s+[А-ЯA-Z]$")


def is_heading(p, text: str) -> bool:
    return p.style.name == "Заголовок ПЗ 1" and bool(text)


def compute_pages(doc: Document) -> dict[int, int]:
    pages: dict[int, int] = {}
    page = 1
    for i, p in enumerate(doc.paragraphs):
        if i > 0 and ("lastRenderedPageBreak" in p._element.xml or 'w:type="page"' in p._element.xml):
            page += 1
        pages[i] = page
    return pages


def collect_entries(doc: Document, pages: dict[int, int], start_idx: int) -> list[tuple[str, int, int]]:
    entries: list[tuple[str, int, int]] = []
    for i, p in enumerate(doc.paragraphs):
        if i < start_idx:
            continue
        text = p.text.strip()
        if not text or "\t" in text:
            continue
        if is_heading(p, text):
            if text.lower() == "содержание":
                continue
            level = 0
            if re.match(r"^\d+\.\d+", text):
                level = 2
            elif re.match(r"^\d+\s", text):
                level = 1
            elif text.lower() in ("введение", "заключение"):
                level = 0
            else:
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


def rebuild_toc(doc: Document):
    content_idx = intro_idx = None
    for i, p in enumerate(doc.paragraphs):
        t = p.text.strip()
        if t.upper() == "СОДЕРЖАНИЕ":
            content_idx = i
        if intro_idx is None and i > (content_idx or 0) and is_heading(p, t) and t.lower().startswith("введение"):
            intro_idx = i

    if content_idx is None or intro_idx is None:
        print("skip: content", content_idx, "intro", intro_idx)
        return

    # удалить всё между СОДЕРЖАНИЕ и Введение
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

    # найти введение снова
    intro_p = None
    for p in doc.paragraphs:
        if is_heading(p, p.text.strip()) and p.text.strip().lower().startswith("введение"):
            intro_p = p
            break
    if intro_p is None:
        return

    tab_pos = Cm(16.5)
    for title, page, level in reversed(entries):
        prefix = "  " * level
        np = intro_p.insert_paragraph_before(f"{prefix}{title}\t{page}")
        np.style = doc.styles["Normal"]
        np.paragraph_format.tab_stops.add_tab_stop(tab_pos, WD_TAB_ALIGNMENT.RIGHT)
        np.paragraph_format.space_after = Pt(0)


def main():
    shutil.copy2(DOC, OUT)
    doc = Document(OUT)
    rebuild_toc(doc)
    doc.save(OUT)
    try:
        shutil.copy2(OUT, DOC)
        print("OK", DOC)
    except PermissionError:
        print("OK", OUT)


if __name__ == "__main__":
    main()
