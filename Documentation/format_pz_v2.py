# -*- coding: utf-8 -*-
"""Финальное оформление ПЗ по методичке (без зависимости от Word COM)."""
from __future__ import annotations

import re
import shutil
from datetime import datetime
from pathlib import Path

from docx import Document
from docx.enum.text import WD_ALIGN_PARAGRAPH, WD_LINE_SPACING, WD_TAB_ALIGNMENT
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Pt, RGBColor

DOC_DIR = Path(
    r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation"
)
SRC = DOC_DIR / "ПЗКурсовая_ГОТОВО.docx"
OUT = DOC_DIR / "ПЗКурсовая_ГОТОВО.docx"
REPORT = DOC_DIR / "ПЗКурсовая_format_report.txt"

FONT_BODY = "Times New Roman"
FONT_CODE = "Consolas"
SIZE_BODY = Pt(14)
SIZE_CODE = Pt(11)
SIZE_TABLE = Pt(12)
INDENT = Cm(1.25)
INDENT_TWIP = "709"  # 1.25 см
SPACE_HEADING_AFTER = Pt(28)
SPACE_SUBHEADING_BEFORE = Pt(14)
TITLE_END = 49
APPENDIX_CODE_START = 526

SECTION_RE = re.compile(r"^(\d+(?:\.\d+)*)\s+(.+)$")
FIGURE_RE = re.compile(r"^Рисунок\s+[\dА-Яа-яA-Z\.]+", re.IGNORECASE)
TABLE_CAPTION_RE = re.compile(r"^Таблица\s+[\dА-Яа-яA-Z\.]+", re.IGNORECASE)
APPENDIX_RE = re.compile(r"^ПРИЛОЖЕНИЕ\s+[А-ЯA-Z]$")
LIST_RE = re.compile(r"^[\u2013\u2212\-–]\s")
INLINE_LIST_RE = re.compile(
    r"^(система|возможность|каталог|карточка|оформление|личный|панель|административн|экспорт|адаптивн|широкий|удобн|подробн|наличие|современн|регулярн|отсутств|недоступн)",
    re.IGNORECASE,
)

MANUAL: list[str] = []


def twip_indent():
    return str(int(Cm(1.25).twips))


def set_run_font(run, name=FONT_BODY, size=SIZE_BODY, bold=False, italic=False):
    run.font.name = name
    run._element.rPr.rFonts.set(qn("w:eastAsia"), name)
    run.font.size = size
    run.font.bold = bold
    run.font.italic = italic
    run.font.underline = False
    run.font.color.rgb = RGBColor(0, 0, 0)
    rpr = run._element.get_or_add_rPr()
    for tag in ("w:b", "w:bCs"):
        el = rpr.find(qn(tag))
        if bold:
            if el is None:
                rpr.append(OxmlElement(tag))
        else:
            if el is not None:
                rpr.remove(el)


def set_para_indent(p, first=INDENT_TWIP, align=None, space_before=None, space_after=None, keep=False):
    pPr = p._element.get_or_add_pPr()
    ind = pPr.find(qn("w:ind"))
    if ind is None:
        ind = OxmlElement("w:ind")
        pPr.append(ind)
    if first is None:
        if qn("w:firstLine") in ind.attrib:
            del ind.attrib[qn("w:firstLine")]
    else:
        ind.set(qn("w:firstLine"), first)
    if align is not None:
        p.alignment = align
    pf = p.paragraph_format
    pf.line_spacing_rule = WD_LINE_SPACING.SINGLE
    pf.line_spacing = 1.0
    pf.space_before = space_before
    pf.space_after = space_after
    pf.widow_control = True
    pf.keep_with_next = keep
    # запрет висячих строк
    widow = pPr.find(qn("w:widowControl"))
    if widow is None:
        pPr.append(OxmlElement("w:widowControl"))
    keep_next = pPr.find(qn("w:keepNext"))
    if keep:
        if keep_next is None:
            pPr.append(OxmlElement("w:keepNext"))
    elif keep_next is not None:
        pPr.remove(keep_next)


def normalize_text(text: str) -> str:
    t = text.replace("\u00a0", " ")
    t = re.sub(r"^[ \t]+", "", t)
    if t.startswith("Рисунок") or t.startswith("Таблица"):
        t = re.sub(r"\s[\u2014\-—]\s", " \u2013 ", t, count=1)
    return t


def is_heading(p, text: str) -> bool:
    return p.style.name == "Заголовок ПЗ 1" and bool(text)


def is_code(idx, p, text: str) -> bool:
    if p.style.name == "HTML Preformatted":
        return True
    if idx < APPENDIX_CODE_START:
        return False
    if any(r.font.name and "consolas" in r.font.name.lower() for r in p.runs if r.text.strip()):
        return True
    s = text.strip()
    return bool(
        s.startswith("//")
        or s.endswith(".cs")
        or "=>" in s
        or re.search(r"\b(class|namespace|public|await|using)\b", s)
    )


def format_all_runs(p, **kwargs):
    if not p.runs:
        p.add_run(p.text)
    for run in p.runs:
        set_run_font(run, **kwargs)


def paragraph_page_number(p, default=1) -> int:
    # оценка по lastRenderedPageBreak в документе
    return default


def compute_pages(doc: Document) -> dict[int, int]:
    pages: dict[int, int] = {}
    page = 1
    for i, p in enumerate(doc.paragraphs):
        xml = p._element.xml
        if i > 0 and ("lastRenderedPageBreak" in xml or 'w:type="page"' in xml):
            page += 1
        pages[i] = page
    return pages


def collect_toc_entries(doc: Document, pages: dict[int, int]) -> list[tuple[str, int, int]]:
    entries: list[tuple[str, int, int]] = []
    started = False
    for i, p in enumerate(doc.paragraphs):
        if i <= TITLE_END:
            continue
        text = p.text.strip()
        if not text or "\t" in text:
            continue
        if is_heading(p, text) and text.lower().startswith("введение"):
            started = True
        if not started:
            continue
        if is_heading(p, text) or text.lower() == "введение":
            level = 0
            if re.match(r"^\d+\.\d+", text):
                level = 2
            elif re.match(r"^\d+\s", text):
                level = 1
            elif text.lower() in ("введение", "заключение"):
                level = 0
            else:
                level = 2
            entries.append((text, pages.get(i, 1), level))
        if text.lower() == "список использованных источников":
            entries.append((text, pages.get(i, 1), 0))
        if APPENDIX_RE.match(text):
            title = ""
            for j in range(i + 1, min(i + 5, len(doc.paragraphs))):
                t2 = doc.paragraphs[j].text.strip()
                if t2 and t2 not in ("(обязательное)", "(рекомендуемое)", "(справочное)"):
                    title = t2
                    break
            label = f"{text} {title}".strip()
            entries.append((label, pages.get(i, 1), 0))
    return entries


def insert_toc(doc: Document, pages: dict[int, int]):
    content_idx = intro_idx = None
    for i, p in enumerate(doc.paragraphs):
        t = p.text.strip()
        if t.upper() == "СОДЕРЖАНИЕ":
            content_idx = i
        if (
            intro_idx is None
            and content_idx is not None
            and i > content_idx
            and is_heading(p, t)
            and t.lower().startswith("введение")
        ):
            intro_idx = i

    if content_idx is None or intro_idx is None:
        MANUAL.append("Не удалось построить содержание (нет СОДЕРЖАНИЕ или Введение).")
        return

    body = doc.element.body
    content_el = doc.paragraphs[content_idx]._element
    intro_el = doc.paragraphs[intro_idx]._element
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
    intro_idx2 = None
    for i, p in enumerate(doc.paragraphs):
        t = p.text.strip()
        if is_heading(p, t) and t.lower().startswith("введение"):
            intro_idx2 = i
            break
    if intro_idx2 is None:
        return

    entries = collect_toc_entries(doc, pages)
    # отфильтровать только записи начиная с Введения
    intro_title = doc.paragraphs[intro_idx2].text.strip()
    if entries and entries[0][0] != intro_title:
        for k, e in enumerate(entries):
            if e[0] == intro_title:
                entries = entries[k:]
                break

    intro_p = doc.paragraphs[intro_idx2]
    tab_pos = Cm(16.5)
    for title, page, level in entries:
        if title.lower() == "содержание":
            continue
        prefix = "  " * level if level else ""
        np = intro_p.insert_paragraph_before(f"{prefix}{title}\t{page}")
        np.style = doc.styles["Normal"]
        set_para_indent(np, first=None, align=WD_ALIGN_PARAGRAPH.LEFT, space_after=Pt(0))
        format_all_runs(np, bold=False)
        np.paragraph_format.tab_stops.add_tab_stop(tab_pos, WD_TAB_ALIGNMENT.RIGHT)


def add_page_number_footer(doc: Document):
    for section in doc.sections:
        section.different_first_page_header_footer = True
        footer = section.footer
        p = footer.paragraphs[0] if footer.paragraphs else footer.add_paragraph()
        for child in list(p._element):
            p._element.remove(child)
        p.alignment = WD_ALIGN_PARAGRAPH.RIGHT
        run = p.add_run()
        set_run_font(run, size=SIZE_BODY, bold=False)
        fld_begin = OxmlElement("w:fldChar")
        fld_begin.set(qn("w:fldCharType"), "begin")
        instr = OxmlElement("w:instrText")
        instr.set(qn("xml:space"), "preserve")
        instr.text = " PAGE "
        fld_sep = OxmlElement("w:fldChar")
        fld_sep.set(qn("w:fldCharType"), "separate")
        fld_end = OxmlElement("w:fldChar")
        fld_end.set(qn("w:fldCharType"), "end")
        run._r.extend([fld_begin, instr, fld_sep, fld_end])


def process_document(doc: Document):
    pages = compute_pages(doc)
    prev_heading = False

    for idx, p in enumerate(doc.paragraphs):
        text = normalize_text(p.text)
        if text != p.text:
            p.text = text
        stripped = text.strip()

        if idx <= TITLE_END:
            if stripped:
                format_all_runs(p, bold=False)
            continue
        if not stripped:
            continue

        if stripped.upper() == "СОДЕРЖАНИЕ":
            p.style = doc.styles["Normal"]
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.CENTER, space_after=SPACE_HEADING_AFTER)
            format_all_runs(p, bold=True)
            continue

        if stripped.lower() == "список использованных источников":
            p.style = doc.styles["Normal"]
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.CENTER, space_after=SPACE_HEADING_AFTER)
            format_all_runs(p, bold=True)
            continue

        if APPENDIX_RE.match(stripped):
            p.style = doc.styles["Normal"]
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.CENTER, space_after=Pt(14), keep=True)
            format_all_runs(p, bold=True)
            continue

        if stripped in ("(обязательное)", "(рекомендуемое)", "(справочное)"):
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.CENTER)
            format_all_runs(p, bold=False)
            continue

        if FIGURE_RE.match(stripped):
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.CENTER, space_before=Pt(14), space_after=Pt(14))
            format_all_runs(p, bold=False)
            continue

        if TABLE_CAPTION_RE.match(stripped):
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.LEFT, space_before=Pt(14), space_after=Pt(0))
            format_all_runs(p, bold=False)
            continue

        if is_code(idx, p, stripped):
            p.style = doc.styles["Normal"]
            set_para_indent(p, first=None, align=WD_ALIGN_PARAGRAPH.LEFT)
            format_all_runs(p, name=FONT_CODE, size=SIZE_CODE, bold=False)
            continue

        if is_heading(p, stripped):
            space_before = SPACE_SUBHEADING_BEFORE if prev_heading else None
            set_para_indent(
                p,
                first=twip_indent(),
                align=WD_ALIGN_PARAGRAPH.JUSTIFY,
                space_before=space_before,
                space_after=SPACE_HEADING_AFTER,
                keep=True,
            )
            format_all_runs(p, bold=True)
            prev_heading = True
            continue

        if LIST_RE.match(stripped) or (
            INLINE_LIST_RE.match(stripped)
            and stripped.endswith((";", "."))
        ):
            if LIST_RE.match(stripped):
                p.text = re.sub(r"^[\u2013\u2212\-–]\s*", "\u2013 ", stripped)
            elif INLINE_LIST_RE.match(stripped) and not stripped.startswith("\u2013"):
                end = stripped if stripped.endswith((";", ".")) else stripped + ";"
                p.text = "\u2013 " + end
            set_para_indent(p, first=twip_indent(), align=WD_ALIGN_PARAGRAPH.JUSTIFY)
            format_all_runs(p, bold=False)
            prev_heading = False
            continue

        set_para_indent(p, first=twip_indent(), align=WD_ALIGN_PARAGRAPH.JUSTIFY)
        format_all_runs(p, bold=False)
        prev_heading = False

    # источники — нумерация
    in_sources = False
    n = 0
    for p in doc.paragraphs:
        t = p.text.strip()
        if t.lower() == "список использованных источников":
            in_sources = True
            continue
        if in_sources:
            if APPENDIX_RE.match(t):
                break
            if not t:
                continue
            if re.match(r"^\d+\s", t):
                continue
            n += 1
            p.text = f"{n} {t}"
            set_para_indent(p, first=twip_indent(), align=WD_ALIGN_PARAGRAPH.JUSTIFY)
            format_all_runs(p, bold=False)

    for table in doc.tables:
        for row in table.rows:
            for cell in row.cells:
                for p in cell.paragraphs:
                    t = p.text.strip()
                    if not t:
                        continue
                    set_para_indent(p, first=None if row is table.rows[0] else "360", align=WD_ALIGN_PARAGRAPH.CENTER)
                    format_all_runs(p, size=SIZE_TABLE, bold=row is table.rows[0])


def main():
    if not SRC.exists():
        raise FileNotFoundError(SRC)

    backup = DOC_DIR / f"ПЗКурсовая_backup_{datetime.now().strftime('%Y%m%d_%H%M%S')}.docx"
    shutil.copy2(SRC, backup)

    doc = Document(SRC)
    for sec in doc.sections:
        sec.page_height = Cm(29.7)
        sec.page_width = Cm(21.0)
        sec.left_margin = Cm(3.0)
        sec.right_margin = Cm(1.5)
        sec.top_margin = Cm(2.0)
        sec.bottom_margin = Cm(2.0)

    process_document(doc)
    pages = compute_pages(doc)
    insert_toc(doc, pages)
    add_page_number_footer(doc)

    doc.save(OUT)

    # попытка перезаписать исходник
    try:
        shutil.copy2(OUT, SRC)
        saved_as = str(SRC)
    except PermissionError:
        saved_as = str(OUT)
        MANUAL.append(
            f"Исходный файл {SRC.name} открыт в Word. Используйте {OUT.name} "
            f"или закройте файл и скопируйте вручную."
        )

    report = [
        f"Исходник: {SRC}",
        f"Резервная копия: {backup}",
        f"Сохранено как: {saved_as}",
        f"Время: {datetime.now().isoformat(timespec='seconds')}",
        "",
        "Выполнено:",
        "- поля 30/15/20/20 мм",
        "- Times New Roman 14, одинарный интервал, абзац 1.25 см",
        "- заголовки: полужирные, по ширине, интервалы 14/28 пт",
        "- подписи рисунков по центру, таблиц — слева",
        "- списки с дефисами",
        "- код в приложении: Consolas 11",
        "- нумерация источников",
        "- содержание с номерами страниц (оценка по разметке Word)",
        "- номера страниц в колонтитуле справа",
    ]
    if MANUAL:
        report.append("")
        report.append("Требует ручной проверки:")
        report.extend(f"- {m}" for m in MANUAL)
        report.append("- Откройте документ в Word: ПКМ по содержанию/полям → «Обновить поле» для точных номеров страниц.")

    REPORT.write_text("\n".join(report), encoding="utf-8")
    print("\n".join(report))


if __name__ == "__main__":
    main()
