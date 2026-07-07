# -*- coding: utf-8 -*-
"""
Приведение ПЗКурсовая.docx к требованиям методички «ОФОРМЛЕНИЕ КП и ДП 2026».
Меняется только оформление, текст сохраняется.
"""
from __future__ import annotations

import re
import shutil
from copy import deepcopy
from datetime import datetime
from pathlib import Path

from docx import Document
from docx.enum.text import WD_ALIGN_PARAGRAPH, WD_LINE_SPACING
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Pt, RGBColor

SRC = Path(
    r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая.docx"
)
OUT = SRC.with_name("ПЗКурсовая_оформлено.docx")
BACKUP = SRC.with_name(
    f"ПЗКурсовая_backup_{datetime.now().strftime('%Y%m%d_%H%M%S')}.docx"
)
REPORT = SRC.with_name("ПЗКурсовая_format_report.txt")

# Границы особых зон (по структуре документа)
TITLE_PAGE_START = 0
TITLE_PAGE_END = 49
APPENDIX_CODE_START = 526  # начало кода в приложении А

FONT_BODY = "Times New Roman"
FONT_CODE = "Consolas"
SIZE_BODY = Pt(14)
SIZE_CODE = Pt(11)
SIZE_TABLE = Pt(12)
INDENT = Cm(1.25)
SPACE_HEADING_AFTER = Pt(28)
SPACE_SUBHEADING_BEFORE = Pt(14)

SECTION_RE = re.compile(r"^(\d+(?:\.\d+)*)\s+(.+)$")
FIGURE_RE = re.compile(r"^Рисунок\s+[\dА-Яа-яA-Z\.]+", re.IGNORECASE)
TABLE_CAPTION_RE = re.compile(r"^Таблица\s+[\dА-Яа-яA-Z\.]+", re.IGNORECASE)
APPENDIX_RE = re.compile(r"^ПРИЛОЖЕНИЕ\s+[А-ЯA-Z]$")
LIST_RE = re.compile(r"^[\u2013\u2014\u2212\-–—]\s")
INLINE_LIST_RE = re.compile(
    r"^(система|возможность|каталог|карточка|оформление|личный|панель|административн|экспорт|адаптивн|широкий|удобн|подробн|наличие|современн|регулярн|отсутств|недоступн)",
    re.IGNORECASE,
)

SPECIAL_HEADINGS = {
    "введение",
    "заключение",
    "список использованных источников",
    "анализ предметной области",
    "постановка задачи",
    "реферат",
}

MANUAL_ISSUES: list[str] = []


def set_run_font(run, name=FONT_BODY, size=SIZE_BODY, bold=None, italic=False, underline=False, color=None):
    run.font.name = name
    run._element.rPr.rFonts.set(qn("w:eastAsia"), name)
    run.font.size = size
    if bold is not None:
        run.font.bold = bold
    run.font.italic = italic
    run.font.underline = underline
    if color is not None:
        run.font.color.rgb = color
    else:
        run.font.color.rgb = RGBColor(0, 0, 0)


def format_runs(p, **kwargs):
    for run in p.runs:
        set_run_font(run, **kwargs)


def set_paragraph_format(
    p,
    align=None,
    indent=INDENT,
    line_spacing=1.0,
    space_before=None,
    space_after=Pt(0),
    keep_with_next=False,
    widow_control=True,
):
    pf = p.paragraph_format
    if align is not None:
        p.alignment = align
    pf.first_line_indent = indent
    pf.line_spacing_rule = WD_LINE_SPACING.SINGLE
    pf.line_spacing = line_spacing
    pf.space_before = space_before
    pf.space_after = space_after
    pf.widow_control = widow_control
    pf.keep_with_next = keep_with_next


def normalize_text(text: str) -> str:
    t = text.replace("\u00a0", " ")
    t = re.sub(r"^[ \t]+", "", t)
    # em-dash / hyphen в подписях к рисункам и таблицам
    if t.startswith("Рисунок") or t.startswith("Таблица"):
        t = re.sub(r"\s[\u2014\u2013\-—]\s", " \u2013 ", t, count=1)
    return t


def is_heading_paragraph(p, text: str) -> bool:
    if p.style.name == "Заголовок ПЗ 1":
        return bool(text.strip())
    return False


def is_figure_caption(text: str) -> bool:
    return bool(FIGURE_RE.match(text.strip()))


def is_table_caption(text: str) -> bool:
    return bool(TABLE_CAPTION_RE.match(text.strip()))


def is_appendix_heading(text: str) -> bool:
    return bool(APPENDIX_RE.match(text.strip()))


def is_content_heading(text: str) -> bool:
    return text.strip().upper() == "СОДЕРЖАНИЕ"


def is_sources_heading(text: str) -> bool:
    return text.strip().lower() == "список использованных источников"


def is_code_paragraph(idx: int, p, text: str) -> bool:
    if p.style.name == "HTML Preformatted":
        return True
    if idx >= APPENDIX_CODE_START:
        if any(r.font.name and "consolas" in r.font.name.lower() for r in p.runs if r.text.strip()):
            return True
        stripped = text.strip()
        if stripped.startswith("//") or stripped.endswith(".cs") or "=>" in stripped:
            return True
        if re.search(r"\b(class|namespace|public|private|await|using)\b", stripped):
            return True
    return False


def is_list_paragraph(text: str) -> bool:
    return bool(LIST_RE.match(text.strip()))


def needs_list_dash(text: str) -> bool:
    stripped = text.strip()
    if not stripped or is_list_paragraph(stripped):
        return False
    if stripped.endswith(";") or stripped.endswith("."):
        return bool(INLINE_LIST_RE.match(stripped))
    return False


def add_dash_to_list_item(text: str) -> str:
    stripped = text.strip()
    if is_list_paragraph(stripped):
        return re.sub(r"^[\u2013\u2014\u2212\-–—]\s*", "\u2013 ", stripped)
    if needs_list_dash(stripped):
        if not stripped.endswith(";") and not stripped.endswith("."):
            stripped += ";"
        return "\u2013 " + stripped
    return stripped


def set_sections(doc: Document):
    for sec in doc.sections:
        sec.page_height = Cm(29.7)
        sec.page_width = Cm(21.0)
        sec.left_margin = Cm(3.0)
        sec.right_margin = Cm(1.5)
        sec.top_margin = Cm(2.0)
        sec.bottom_margin = Cm(2.0)


def update_heading_style(doc: Document):
    try:
        style = doc.styles["Заголовок ПЗ 1"]
    except KeyError:
        return
    style.font.name = FONT_BODY
    style.font.size = SIZE_BODY
    style.font.bold = True
    style.font.color.rgb = RGBColor(0, 0, 0)
    pf = style.paragraph_format
    pf.first_line_indent = INDENT
    pf.line_spacing_rule = WD_LINE_SPACING.SINGLE
    pf.line_spacing = 1.0
    pf.space_after = SPACE_HEADING_AFTER
    pf.widow_control = True
    pf.keep_with_next = True
    pf.alignment = WD_ALIGN_PARAGRAPH.JUSTIFY


def format_tables(doc: Document):
    for table in doc.tables:
        try:
            table.autofit = False
        except Exception:
            pass
        for row in table.rows:
            for cell in row.cells:
                for p in cell.paragraphs:
                    txt = p.text.strip()
                    if not txt:
                        continue
                    # заголовки таблиц — по центру, без абзацного отступа
                    if p == cell.paragraphs[0] and row == table.rows[0]:
                        set_paragraph_format(p, align=WD_ALIGN_PARAGRAPH.CENTER, indent=Cm(0))
                        format_runs(p, bold=True, italic=False)
                    else:
                        set_paragraph_format(
                            p,
                            align=WD_ALIGN_PARAGRAPH.JUSTIFY if len(txt) > 20 else WD_ALIGN_PARAGRAPH.CENTER,
                            indent=Cm(0.5) if len(txt) > 20 else Cm(0),
                        )
                        format_runs(p, size=SIZE_TABLE, bold=False, italic=False)
                    for run in p.runs:
                        if run.font.size is None or run.font.size > SIZE_BODY:
                            run.font.size = SIZE_TABLE


def number_sources(doc: Document, start_idx: int, end_idx: int) -> int:
    n = 0
    for i in range(start_idx, end_idx):
        p = doc.paragraphs[i]
        text = p.text.strip()
        if not text:
            continue
        if is_sources_heading(text) or is_appendix_heading(text):
            break
        if re.match(r"^\d+\s", text):
            continue
        n += 1
        new_text = f"{n} {text}"
        if p.text != new_text:
            p.text = new_text
        set_paragraph_format(p, align=WD_ALIGN_PARAGRAPH.JUSTIFY, indent=INDENT)
        format_runs(p, bold=False, italic=False)
    return n


def process_paragraphs(doc: Document):
    stats = {
        "body": 0,
        "heading": 0,
        "figure": 0,
        "table_cap": 0,
        "list": 0,
        "code": 0,
        "title": 0,
        "special": 0,
    }
    prev_was_heading = False
    prev_heading_level = 0

    for idx, p in enumerate(doc.paragraphs):
        raw = p.text
        text = normalize_text(raw)
        if text != raw:
            p.text = text
            raw = text

        stripped = text.strip()

        # Титульные и удостоверяющий лист — не трогаем сильно
        if TITLE_PAGE_START <= idx <= TITLE_PAGE_END:
            if stripped:
                format_runs(p, bold=None, italic=False)
            stats["title"] += 1
            continue

        if not stripped:
            p.paragraph_format.space_after = Pt(0)
            continue

        if is_content_heading(stripped):
            p.style = doc.styles["Normal"]
            set_paragraph_format(
                p,
                align=WD_ALIGN_PARAGRAPH.CENTER,
                indent=Cm(0),
                space_after=SPACE_HEADING_AFTER,
            )
            format_runs(p, bold=True)
            stats["special"] += 1
            prev_was_heading = True
            continue

        if is_sources_heading(stripped):
            p.style = doc.styles["Normal"]
            set_paragraph_format(
                p,
                align=WD_ALIGN_PARAGRAPH.CENTER,
                indent=Cm(0),
                space_after=SPACE_HEADING_AFTER,
            )
            format_runs(p, bold=True)
            stats["special"] += 1
            prev_was_heading = True
            continue

        if is_appendix_heading(stripped):
            p.style = doc.styles["Normal"]
            set_paragraph_format(
                p,
                align=WD_ALIGN_PARAGRAPH.CENTER,
                indent=Cm(0),
                space_after=Pt(14),
                keep_with_next=True,
            )
            format_runs(p, bold=True)
            stats["special"] += 1
            prev_was_heading = True
            continue

        if stripped in ("(обязательное)", "(рекомендуемое)", "(справочное)"):
            set_paragraph_format(p, align=WD_ALIGN_PARAGRAPH.CENTER, indent=Cm(0))
            format_runs(p, bold=False)
            stats["special"] += 1
            continue

        if is_figure_caption(stripped):
            set_paragraph_format(
                p,
                align=WD_ALIGN_PARAGRAPH.CENTER,
                indent=Cm(0),
                space_before=Pt(14),
                space_after=Pt(14),
            )
            format_runs(p, bold=False, italic=False)
            stats["figure"] += 1
            prev_was_heading = False
            continue

        if is_table_caption(stripped):
            set_paragraph_format(
                p,
                align=WD_ALIGN_PARAGRAPH.LEFT,
                indent=Cm(0),
                space_before=Pt(14),
                space_after=Pt(0),
            )
            format_runs(p, bold=False, italic=False)
            stats["table_cap"] += 1
            prev_was_heading = False
            continue

        if is_code_paragraph(idx, p, stripped):
            p.style = doc.styles["Normal"]
            set_paragraph_format(p, align=WD_ALIGN_PARAGRAPH.LEFT, indent=Cm(0), space_after=Pt(0))
            format_runs(p, name=FONT_CODE, size=SIZE_CODE, bold=False, italic=False)
            stats["code"] += 1
            prev_was_heading = False
            continue

        if is_heading_paragraph(p, stripped):
            clean = stripped.strip()
            if clean.lower() in SPECIAL_HEADINGS:
                display = clean[:1].upper() + clean[1:] if clean else clean
            else:
                display = clean
            if p.text != display:
                p.text = display

            m = SECTION_RE.match(display)
            level = display.count(".") + 1 if m and "." in m.group(1) else 1
            space_before = SPACE_SUBHEADING_BEFORE if prev_was_heading and level > 1 else None

            set_paragraph_format(
                p,
                align=WD_ALIGN_PARAGRAPH.JUSTIFY,
                indent=INDENT,
                space_before=space_before,
                space_after=SPACE_HEADING_AFTER,
                keep_with_next=True,
            )
            format_runs(p, bold=True, italic=False)
            stats["heading"] += 1
            prev_was_heading = True
            prev_heading_level = level
            continue

        if is_list_paragraph(stripped):
            new_text = add_dash_to_list_item(stripped)
            if new_text != stripped:
                p.text = new_text
            set_paragraph_format(p, align=WD_ALIGN_PARAGRAPH.JUSTIFY, indent=INDENT)
            format_runs(p, bold=False, italic=False)
            stats["list"] += 1
            prev_was_heading = False
            continue

        if needs_list_dash(stripped):
            new_text = add_dash_to_list_item(stripped)
            if new_text != stripped:
                p.text = new_text
            set_paragraph_format(p, align=WD_ALIGN_PARAGRAPH.JUSTIFY, indent=INDENT)
            format_runs(p, bold=False, italic=False)
            stats["list"] += 1
            prev_was_heading = False
            continue

        # Основной текст
        set_paragraph_format(
            p,
            align=WD_ALIGN_PARAGRAPH.JUSTIFY,
            indent=INDENT,
            space_after=Pt(0),
        )
        format_runs(p, bold=False, italic=False, underline=False)
        stats["body"] += 1
        prev_was_heading = False

    return stats


def update_toc_with_word(path: Path) -> bool:
    try:
        import win32com.client  # type: ignore

        word = win32com.client.Dispatch("Word.Application")
        word.Visible = False
        word.DisplayAlerts = 0
        doc = word.Documents.Open(str(path))
        # обновить оглавление
        for toc in doc.TablesOfContents:
            toc.Update()
        # обновить все поля (номера страниц)
        doc.Fields.Update()
        # убедиться в нижнем правом колонтитуле
        for sec in doc.Sections:
            footer = sec.Footers(1)  # wdHeaderFooterPrimary
            footer.Range.Font.Name = "Times New Roman"
            footer.Range.Font.Size = 14
        doc.Save()
        doc.Close(False)
        word.Quit()
        return True
    except Exception as exc:
        MANUAL_ISSUES.append(
            f"Не удалось автоматически обновить поля Word (содержание/номера страниц): {exc}. "
            "Откройте документ в Word → выделите содержание → ПКМ → «Обновить поле»."
        )
        return False


def validate_document(path: Path) -> list[str]:
    issues = []
    doc = Document(path)
    for i, p in enumerate(doc.paragraphs):
        t = p.text.strip()
        if not t or i <= TITLE_PAGE_END:
            continue
        if is_code_paragraph(i, p, t):
            continue
        for r in p.runs:
            if r.font.italic:
                issues.append(f"п.{i}: курсив в основном тексте")
            if r.font.name and "consolas" not in (r.font.name or "").lower():
                if r.font.name not in (FONT_BODY, None) and not is_figure_caption(t):
                    if "Times" not in (r.font.name or ""):
                        issues.append(f"п.{i}: шрифт {r.font.name}")
        if t.startswith("•"):
            issues.append(f"п.{i}: маркер • вместо дефиса")
        if is_figure_caption(t) and p.alignment != WD_ALIGN_PARAGRAPH.CENTER:
            issues.append(f"п.{i}: подпись рисунка не по центру")
        if " — " in t and (t.startswith("Рисунок") or t.startswith("Таблица")):
            issues.append(f"п.{i}: длинное тире в подписи")
    return issues[:50]


def main():
    if not SRC.exists():
        raise FileNotFoundError(SRC)

    shutil.copy2(SRC, BACKUP)
    doc = Document(SRC)

    set_sections(doc)
    update_heading_style(doc)
    stats = process_paragraphs(doc)

    # Нумерация источников
    sources_start = None
    for i, p in enumerate(doc.paragraphs):
        if is_sources_heading(p.text):
            sources_start = i + 1
            break
    sources_count = 0
    if sources_start:
        sources_count = number_sources(doc, sources_start, len(doc.paragraphs))

    format_tables(doc)
    doc.save(OUT)

    toc_updated = update_toc_with_word(OUT)
    validation_issues = validate_document(OUT)

    report_lines = [
        f"Исходный файл: {SRC}",
        f"Результат: {OUT}",
        f"Резервная копия: {BACKUP}",
        f"Обработано: {datetime.now().isoformat(timespec='seconds')}",
        "",
        "Статистика:",
        *[f"  {k}: {v}" for k, v in stats.items()],
        f"  sources_numbered: {sources_count}",
        f"  toc_updated_via_word: {toc_updated}",
        "",
        "Проверка после форматирования:",
    ]
    if validation_issues:
        report_lines.append("  Остались замечания:")
        report_lines.extend(f"    - {x}" for x in validation_issues)
    else:
        report_lines.append("  Критичных расхождений не обнаружено.")

    if MANUAL_ISSUES:
        report_lines.append("")
        report_lines.append("Требует ручной проверки в Word:")
        report_lines.extend(f"  - {x}" for x in MANUAL_ISSUES)

    report_lines.extend(
        [
            "",
            "Рекомендация: откройте документ в Word и визуально проверьте",
            "содержание, переносы заголовков и номера страниц.",
        ]
    )
    REPORT.write_text("\n".join(report_lines), encoding="utf-8")
    print("\n".join(report_lines))


if __name__ == "__main__":
    main()
