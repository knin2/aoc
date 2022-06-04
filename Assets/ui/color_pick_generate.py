from PIL import Image
import colorsys


def hsv2rgb(h, s, v):
    return tuple(round(i * 255) for i in colorsys.hsv_to_rgb(h, s, v))


def run(column: bool):
    if column:
        img = Image.new("RGB", (32, 360))

        for x in range(img.width):
            for y in range(img.height):
                color = hsv2rgb(y / 360, 1, 1)
                img.putpixel((x, y), color)
        img.save("hsv.png")
        return
    img = Image.new("RGB", (360, 32))
    for x in range(img.width):
        for y in range(img.height):
            color = hsv2rgb(0, 0, x / img.width)
            img.putpixel((x, y), color)
    img.save("rect.png")


run(False)
