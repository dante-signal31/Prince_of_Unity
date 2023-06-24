#!/usr/bin/python3
import pathlib

from jinja2 import Template

content_to_input = "output/manual.tex"
latex_template = "template/complete_manual_template.tex"
populated_latex = "output/PoU_manual.tex"

if __name__ == '__main__':
    content = pathlib.Path(content_to_input).read_text()
    template = pathlib.Path(latex_template).read_text()
    with open(file=populated_latex, mode="w") as complete_manual:
        complete_manual.write(Template(template).render(contents=content))
