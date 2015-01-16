from pyExcelerator import *
import sys
import shutil
import os
import glob
import struct

def writeInt(f, data):
    f.write(struct.pack('<L', data))

def exportXLS(xlsFile, binOut, srcOut, scriptPath):

	stringIdsTableRow = {}
	languagesColumn = {}
	stringIds = []
	languages = []
	parsedXLS = parse_xls(xlsFile, 'cp1251')
	
	index = 0
	for sheet_name, values in parsedXLS:
		sheetName = sheet_name.encode('cp866', 'backslashreplace').upper()
		languageNameRow = -1
		for row_idx, col_idx in sorted(values.keys()):
			#print "Values " + str(row_idx) + ","+ str(col_idx) 
			if values[(row_idx, col_idx)].lower() == "id" and languageNameRow == -1:
				languageNameRow = row_idx
			if row_idx == languageNameRow and col_idx != 0:
				if not values[(row_idx, col_idx)] in languages:
					languages.append(values[(row_idx, col_idx)])
				languagesColumn[values[(row_idx, col_idx)]] = col_idx
			if col_idx == 0 and row_idx != languageNameRow:
				stringIds.append(sheetName + "_" + values[(row_idx, col_idx)])
				stringIdsTableRow[sheetName + "_" + values[(row_idx, col_idx)]] = [index, row_idx]
		index = index + 1

	try:
		os.mkdir(srcOut)
	except:
		pass
	try:
		os.mkdir(binOut)
	except:
		pass


	outHeader = open(os.path.join(srcOut, "Strings.cs"), 'w')

	#outHeader.write("#ifndef __STRINGPACKDEF_H__\n")
	#outHeader.write("#define __STRINGPACKDEF_H__\n")
	#outHeader.write("\n")
	outHeader.write("public enum LANG\n{\n")
	for language in languages:
		outHeader.write("\t" + language + ",\n")
	outHeader.write("\tCOUNT\n")
	outHeader.write("}\n")

	#outHeader.write("public string[] stringPackFiles = new string[(int)LANG_COUNT]{\n")
	#for language in languages:
	#	outHeader.write("\t\"strings/" + language + "\",\n")
	#outHeader.write("};\n\n")

	#outHeader.write("const char g_langList[LANG_COUNT][5] =\n{\n")
	#for language in languages:
	#	outHeader.write("\t\"" + language.lower() + "\",\n")
	#outHeader.write("};\n\n")

#	outHeader.write("const char g_langDList[LANG_COUNT][10] =\n{\n")
#	for language in languages:
#		outHeader.write("\t\"" + parsedXLS[0][1][(languageNameRow + 1, languagesColumn[language])] + "\",\n")
#	outHeader.write("};\n\n")


	outHeader.write("public enum MotinStrings\n{\n")
	for sId in stringIds:
		outHeader.write("\t" + sId + ",\n")
	outHeader.write("\tSTRING_COUNT\n")
	outHeader.write("}\n\n")
	
	#outHeader.write("const char stringId[STRING_COUNT][64] =\n{\n")
	#for sId in stringIds:
	#	outHeader.write("\t\"" + sId + "\",\n")
	#outHeader.write("};\n\n")

	outHeader.write("\n")
	#outHeader.write("#endif\n")
	#outHeader.write("\n")
	outHeader.close()


	for language in languagesColumn.keys():
		outFile = open(os.path.join(binOut, language + ".bytes"), 'wb')
		writeInt(outFile, len(stringIdsTableRow.keys()))
		for sId in stringIds:
			sheet = int(stringIdsTableRow[sId][0])
			row	= int(stringIdsTableRow[sId][1])
			col	= languagesColumn[language]
			
			try:
				stringa = parsedXLS[sheet][1][(row, col)]
			except:
				writeInt(outFile, 0)
				print "Warning: no string found for " + sId + " in " + language
				continue
			
			if "\\n" in stringa:
				stringa = stringa.replace("\\n",'\n')
			encodedStr = stringa.encode("UTF-8")
			#print(stringa)
			#print(len(encodedStr))
			writeInt(outFile, len(encodedStr))
			outFile.write(encodedStr)
			
	print "Done!"
				

if __name__ == '__main__':
	me, args = sys.argv[0], sys.argv[1:]
	scriptPath = os.path.abspath(os.path.dirname(me))
	if (len(args) == 3):
		xlsFile = args[0]
		binOut  = args[1]
		srcOut  = args[2]
		exportXLS(xlsFile, binOut, srcOut, scriptPath)






