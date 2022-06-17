a = [4, 5, 6, 5, 4]

def carnet():
    k = a[:]
    for i in range(len(k)):
        k[i] = k[i - 1 if i > 0 else len(k) - 1]
    return k
k = carnet()
print(k)