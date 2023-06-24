# Consolidate ervey markdwon subdocument in one single document.
multimarkdown -t mmd -o output/manual.mmd template/manual.mmd
# Convert markdown document to latex content.
pandoc --listings -f markdown_mmd -o output/manual.tex output/manual.mmd 
# Insert content into latext template to generate a complete manual.
./populate_tex_template.py $1
# Convert latex document to PDF.
pdflatex -shell-escape -output-directory pdf output/PoU_manual.tex 
# You need to run pdflatex twice to generate a proper table of contents.
# Odd but true: https://tex.stackexchange.com/questions/314796/why-is-there-no-content-under-the-contents-section-generated-with-tableofconten
pdflatex -shell-escape -output-directory pdf output/PoU_manual.tex 

