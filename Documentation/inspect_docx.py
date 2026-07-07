# -*- coding: utf-8 -*-
import zipfile
import re

path = r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\ПЗКурсовая.docx"
out = r"c:\Users\user\Desktop\PerfumeStore_NEW\PerfumeStoreCourseProject\Documentation\inspect_out.txt"
lines = []
with zipfile.ZipFile(path) as z:
    for name in sorted(z.namelist()):
        if "footer" in name or "header" in name:
            lines.append(f"FILE {name}")
    styles = z.read("word/styles.xml").decode("utf-8")
    for chunk in re.findall(r"<w:style w:type=\"paragraph\".*?</w:style>", styles, re.DOTALL):
        sid = re.search(r'w:styleId="([^"]+)"', chunk)
        sname = re.search(r'<w:name w:val="([^"]+)"', chunk)
        if sid and sname:
            lines.append(f"STYLE {sid.group(1)} -> {sname.group(1)}")
    for i in range(1, 6):
        fn = f"word/footer{i}.xml"
        if fn in z.namelist():
            xml = z.read(fn).decode("utf-8")
            lines.append(f"--- {fn} ---")
            lines.append(xml[:2500])
    # TOC field in document
    doc = z.read("word/document.xml").decode("utf-8")
    if "fldChar" in doc or "TOC" in doc:
        lines.append("Has TOC field")
    toc_matches = re.findall(r".{0,80}(fldSimple|instrText|TOC).{0,80}", doc)
    lines.append(f"TOC matches: {len(toc_matches)}")
    # page breaks
    pb = doc.count("w:br") + doc.count("w:lastRenderedPageBreak")
    lines.append(f"breaks approx: {pb}")

with open(out, "w", encoding="utf-8") as f:
    f.write("\n".join(lines))
print("ok")
