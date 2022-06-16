from pathlib import Path
from os import getcwd as CD
from time import sleep
import pymongo
from discord import Client, Intents, User
client = pymongo.MongoClient("mongodb+srv://l9-bot:sladanasanic@hnl-bot-data.d2lnd.mongodb.net/?retryWrites=true&w=majority")
db = client["hnl_cxx_db"]["hnl_cxx_collection"]

while (True):

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

    db.update_one({"name": "dt"}, {"$set": {"data": dictionary}})

    sleep(5)