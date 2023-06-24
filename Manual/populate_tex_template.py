#!/usr/bin/python3

# This script needs manual version to be passed as an argument.
# So, to pass 1.0.0 version do: 
#     ./populate_tex_template.py 1.0.0

import pathlib
import sys

from jinja2 import Template, TemplateSyntaxError

content_to_input = "output/manual.tex"
latex_template = "template/complete_manual_template.tex"
populated_latex = "output/PoU_manual.tex"

if __name__ == '__main__':
    content = pathlib.Path(content_to_input).read_text()
    template = pathlib.Path(latex_template).read_text()
    with open(file=populated_latex, mode="w") as complete_manual:
        try:
            manual_template = Template(template)
            complete_manual_content = manual_template.render(contents=content, version=sys.argv[1])
        except TemplateSyntaxError as e:
            print(
                "Jinja2 template syntax error during render of line {}:{} error: {}".
                format(e.name, e.lineno, e.message))
            raise e
        complete_manual.write(complete_manual_content)
