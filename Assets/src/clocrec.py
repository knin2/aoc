from pathlib import Path
from os import getcwd as CD

lines = 0
dictionary = {}

cd = CD()

print(cd)
for path in Path(cd).rglob('*.cs'):
    conv = str(path.absolute()).replace(CD(), "")[1:].replace("\\", "/")
    l = len( [lin for lin in open(conv, "r").readlines() if not lin.isspace()] )
    dictionary[conv] = l
    lines += l

dictionary["overall"] = lines

print(dictionary)