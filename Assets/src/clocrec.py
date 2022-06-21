from datetime import datetime
from pathlib import Path
from os import getcwd as CD
from time import sleep
import time
import pymongo
import rpc
client = pymongo.MongoClient(
    "mongodb+srv://l9-bot:sladanasanic@hnl-bot-data.d2lnd.mongodb.net/?retryWrites=true&w=majority")
db = client["hnl_cxx_db"]["hnl_cxx_collection"]
print("Demo for python-discord-rpc")
client_id = '985117362153992302'
# Your application's client ID as a string. (This isn't a real client ID)
rpc_obj = rpc.DiscordIpcClient.for_platform(
    client_id)  # Send the client ID to the rpc module
print("RPC connection successful.")
tm = time.time()
while (True):

    lines = 0
    dictionary = {}

    cd = CD()

    print(cd)
    for path in Path(cd).rglob('*.cs'):
        conv = str(path.absolute()).replace(CD(), "")[1:].replace("\\", "/")
        l = len([lin for lin in open(conv, "r").readlines() if not lin.isspace()])
        dictionary[conv] = l
        lines += l

    addl = len([lin for lin in open("../provinces/provinces.json",
               "r", encoding="utf-8").readlines() if not lin.isspace()])
    dictionary["../provinces/provinces.json"] = addl
    lines += addl

    dictionary["overall"] = lines
    print(dictionary)

    db.update_one({"name": "dt"}, {"$set": {"data": dictionary}})
    d0 = datetime(2022, 6, 1)
    d1 = datetime.now()
    delta = d1 - d0
    days = delta.days
    activity = {
        "state": f"{lines} linija koda u {days} dana",
        "timestamps": {
            "start": tm
        },
        "assets": {
            "large_image": "logo",
            "large_text": "Balkanski AOC",
            "small_image": "logo",
            "small_text": "Balkanski AOC"
        },
        "instance": True
        }
    try:
        rpc_obj.set_activity(activity)
    except OSError:
        print(activity)
    sleep(5)
