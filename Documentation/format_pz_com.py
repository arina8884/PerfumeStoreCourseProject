# -*- coding: utf-8 -*-
"""Второй проход: заголовки, содержание, колонтитулы через MS Word."""
import re
import time
from pathlib import Path

import win32com.client

DOC = Path(
    r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая_оформлено.docx"
)
REPORT = DOC.with_name("ПЗКурсовая_com_pass_report.txt")

WD_ALIGN_JUSTIFY = 3
WD_ALIGN_CENTER = 1
WD_ALIGN_LEFT = 0
WD_LINE_SPACE_SINGLE = 0
WD_ACTIVE_END_PAGE_NUMBER = 7
WD_COLLAPSE_END = 0


def cm_to_points(word, cm):
    return word.CentimetersToPoints(cm)


def format_heading_style(doc, word):
    for name in ("Заголовок ПЗ 1", "Heading 1"):
        try:
            st = doc.Styles(name)
            st.Font.Name = "Times New Roman"
            st.Font.Size = 14
            st.Font.Bold = True
            st.Font.Color = 0
            pf = st.ParagraphFormat
            pf.Alignment = WD_ALIGN_JUSTIFY
            pf.FirstLineIndent = cm_to_points(word, 1.25)
            pf.LineSpacingRule = WD_LINE_SPACE_SINGLE
            pf.SpaceAfter = 28
            pf.SpaceBefore = 0
            pf.WidowControl = True
            pf.KeepWithNext = True
        except Exception:
            pass


def format_normal_style(doc, word):
    try:
        st = doc.Styles("Normal")
        st.Font.Name = "Times New Roman"
        st.Font.Size = 14
        st.Font.Bold = False
        pf = st.ParagraphFormat
        pf.Alignment = WD_ALIGN_JUSTIFY
        pf.FirstLineIndent = cm_to_points(word, 1.25)
        pf.LineSpacingRule = WD_LINE_SPACE_SINGLE
        pf.SpaceAfter = 0
        pf.WidowControl = True
    except Exception:
        pass


def apply_heading_paragraphs(doc):
    for p in doc.Paragraphs:
        try:
            st = p.Style.NameLocal
        except Exception:
            continue
        text = p.Range.Text.strip().replace("\r", "").replace("\x07", "")
        if not text:
            continue
        if st == "Заголовок ПЗ 1":
            p.Range.Font.Name = "Times New Roman"
            p.Range.Font.Size = 14
            p.Range.Font.Bold = True
            p.Range.Font.Italic = False
            p.Format.Alignment = WD_ALIGN_JUSTIFY
            p.Format.FirstLineIndent = cm_to_points(doc.Application, 1.25)
            p.Format.SpaceAfter = 28
            p.Format.KeepWithNext = True
            p.Format.WidowControl = True
        if text.upper() == "СОДЕРЖАНИЕ":
            p.Range.Font.Bold = True
            p.Format.Alignment = WD_ALIGN_CENTER
            p.Format.FirstLineIndent = 0
            p.Format.SpaceAfter = 28
        if text.lower() == "список использованных источников":
            p.Range.Font.Bold = True
            p.Format.Alignment = WD_ALIGN_CENTER
            p.Format.FirstLineIndent = 0
            p.Format.SpaceAfter = 28
        if re.match(r"^ПРИЛОЖЕНИЕ\s+[А-ЯA-Z]$", text):
            p.Range.Font.Bold = True
            p.Format.Alignment = WD_ALIGN_CENTER
            p.Format.FirstLineIndent = 0
        if text.startswith("Рисунок"):
            p.Format.Alignment = WD_ALIGN_CENTER
            p.Format.FirstLineIndent = 0
            p.Range.Font.Bold = False
        if re.match(r"^Таблица\s+[\dА-ЯA-Z\.]+", text):
            p.Format.Alignment = WD_ALIGN_LEFT
            p.Format.FirstLineIndent = 0


def collect_headings(doc):
    items = []
    for p in doc.Paragraphs:
        text = p.Range.Text.strip().replace("\r", "").replace("\x07", "")
        if not text:
            continue
        try:
            st = p.Style.NameLocal
        except Exception:
            st = ""
        if st == "Заголовок ПЗ 1":
            page = p.Range.Information(WD_ACTIVE_END_PAGE_NUMBER)
            items.append((text, int(page)))
        if text.upper() in ("ВВЕДЕНИЕ",) and st != "Заголовок ПЗ 1":
            page = p.Range.Information(WD_ACTIVE_END_PAGE_NUMBER)
            items.append((text, int(page)))
    return items


def build_toc(doc, word):
    """Заполнить содержание вручную по заголовкам."""
    content_para = None
    for p in doc.Paragraphs:
        if p.Range.Text.strip().upper() == "СОДЕРЖАНИЕ":
            content_para = p
            break
    if content_para is None:
        return "СОДЕРЖАНИЕ не найдено"

    headings = collect_headings(doc)
    if not headings:
        return "Заголовки не найдены"

    # удалить пустые абзацы после СОДЕРЖАНИЕ (до Введения)
    start = content_para.Range.End
    for p in doc.Paragraphs:
        t = p.Range.Text.strip().replace("\r", "").replace("\x07", "")
        if p.Range.Start <= content_para.Range.End:
            continue
        if t.lower().startswith("введение") or re.match(r"^\d+\s", t):
            end = p.Range.Start
            if end > start:
                doc.Range(start, end).Delete()
            break

    rng = doc.Range(content_para.Range.End, content_para.Range.End)
    rng.InsertAfter("\r")

    lines = []
    for title, page in headings:
        title = title.strip()
        indent = ""
        m = re.match(r"^(\d+)\.(\d+)\s", title)
        m2 = re.match(r"^(\d+)\s", title)
        if m:
            indent = "  "
        elif m2 and "." not in title.split()[0]:
            indent = ""
        # Введение, Заключение — без отступа
        if title.lower() in ("введение", "заключение", "список использованных источников"):
            indent = ""
        elif re.match(r"^\d+\.\d+", title):
            indent = "  "
        elif re.match(r"^\d+\s", title):
            indent = ""
        elif title[0].isupper() and not re.match(r"^\d", title):
            indent = "  "

        # название + точки-заполнитель + номер страницы (пробелы по методичке)
        line = f"{indent}{title}\t{page}"
        lines.append(line)

    # приложения
    for p in doc.Paragraphs:
        t = p.Range.Text.strip().replace("\r", "").replace("\x07", "")
        if re.match(r"^ПРИЛОЖЕНИЕ\s+[А-ЯA-Z]$", t):
            page = int(p.Range.Information(WD_ACTIVE_END_PAGE_NUMBER))
            # заголовок приложения на следующем абзаце
            title2 = ""
            idx = p.Range.Paragraphs(1).Range.Start
            for p2 in doc.Paragraphs:
                if p2.Range.Start > p.Range.End:
                    t2 = p2.Range.Text.strip().replace("\r", "").replace("\x07", "")
                    if t2 and t2 not in ("(обязательное)", "(рекомендуемое)", "(справочное)"):
                        title2 = t2
                        break
                    if t2.startswith("(обязательное)"):
                        continue
            label = f"{t} {title2}".strip()
            lines.append(f"{label}\t{page}")

    toc_text = "\r".join(lines) + "\r"
    insert_rng = doc.Range(content_para.Range.End, content_para.Range.End)
    insert_rng.InsertAfter(toc_text)

    # оформить строки содержания
    toc_range = doc.Range(content_para.Range.End, content_para.Range.End + len(toc_text))
    toc_range.Font.Name = "Times New Roman"
    toc_range.Font.Size = 14
    toc_range.Font.Bold = False
    toc_range.ParagraphFormat.Alignment = WD_ALIGN_LEFT
    toc_range.ParagraphFormat.FirstLineIndent = 0
    toc_range.ParagraphFormat.LineSpacingRule = WD_LINE_SPACE_SINGLE

    # табуляция: номер страницы справа
    content_para.Range.Select()
    word.Selection.ParagraphFormat.TabStops.Add(
        Position=word.CentimetersToPoints(16.5),
        Alignment=2,  # right
    )

    return f"Добавлено строк содержания: {len(lines)}"


def setup_page_numbers(doc):
    for sec in doc.Sections:
        footer = sec.Footers(1)
        footer.Range.Text = ""
        footer.PageNumbers.Add(PageNumberAlignment=2, FirstPage=False)  # right
        footer.Range.Font.Name = "Times New Roman"
        footer.Range.Font.Size = 14
        sec.PageSetup.DifferentFirstPageHeaderFooter = True


def main():
    word = win32com.client.Dispatch("Word.Application")
    word.Visible = False
    word.DisplayAlerts = 0
    doc = word.Documents.Open(FileName=str(DOC), ReadOnly=False)
    if not hasattr(doc, "Paragraphs"):
        raise RuntimeError(f"Не удалось открыть документ: {DOC}")

    format_heading_style(doc, word)
    format_normal_style(doc, word)
    apply_heading_paragraphs(doc)

    toc_msg = build_toc(doc, word)
    setup_page_numbers(doc)

    doc.Fields.Update()
    time.sleep(1)
    doc.Save()
    doc.Close(False)
    word.Quit()

    report = [
        f"Файл: {DOC}",
        toc_msg,
        "Выполнено: стили заголовков, ручное содержание, номера страниц.",
    ]
    REPORT.write_text("\n".join(report), encoding="utf-8")
    print("\n".join(report))


if __name__ == "__main__":
    main()
