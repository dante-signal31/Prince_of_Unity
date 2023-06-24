# Consolidate ervey markdwon subdocument in one single document.
multimarkdown -t mmd -o output/manual.mmd template/manual.mmd
# Convert markdown document to latex content.
pandoc --listings -f markdown_mmd -o output/manual.tex output/manual.mmd 
# Insert content into latext template to generate a complete manual.
./populate_tex_template.py
# Convert latex document to PDF.
pdflatex -shell-escape -output-directory pdf output/PoU_manual.tex 

