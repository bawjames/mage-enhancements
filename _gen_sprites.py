#!/usr/bin/env python3
"""Generate the item sprite and mod icon using only the Python standard library.

Draws an ornate gold amulet ("reliquary") holding a faceted mana gem that fades
from arcane purple at the top to celestial cyan at the bottom, with a few star
sparkles for the celestial theme.
"""
import math
import struct
import zlib


def write_png(path, pixels):
    """pixels: 2D list (rows) of (r, g, b, a) tuples -> 8-bit RGBA PNG."""
    h = len(pixels)
    w = len(pixels[0])
    raw = bytearray()
    for row in pixels:
        raw.append(0)  # filter type 0 (None) per scanline
        for (r, g, b, a) in row:
            raw += bytes((int(r) & 255, int(g) & 255, int(b) & 255, int(a) & 255))

    def chunk(tag, data):
        c = struct.pack(">I", len(data)) + tag + data
        return c + struct.pack(">I", zlib.crc32(tag + data) & 0xFFFFFFFF)

    sig = b"\x89PNG\r\n\x1a\n"
    ihdr = struct.pack(">IIBBBBB", w, h, 8, 6, 0, 0, 0)  # 8-bit, color type 6 (RGBA)
    idat = zlib.compress(bytes(raw), 9)
    with open(path, "wb") as f:
        f.write(sig + chunk(b"IHDR", ihdr) + chunk(b"IDAT", idat) + chunk(b"IEND", b""))


def blank(w, h):
    return [[(0, 0, 0, 0) for _ in range(w)] for _ in range(h)]


def lerp(a, b, t):
    return tuple(int(round(a[i] + (b[i] - a[i]) * t)) for i in range(3))


def clampc(c):
    return tuple(max(0, min(255, v)) for v in c)


# Palette
PURPLE = (150, 86, 205)      # top of gem (arcane)
CYAN = (96, 214, 236)        # bottom of gem (celestial)
GEM_LIGHT = (60, 60, 60)     # facet lighten (added)
GEM_DARK = (-55, -55, -55)   # facet darken (added)
GOLD_HI = (255, 228, 140)
GOLD = (226, 178, 64)
GOLD_LO = (168, 116, 34)
OUTLINE = (38, 24, 58)
SPARK = (240, 255, 255)
SPARK_ARM = (160, 232, 248)


def draw_amulet(px, cx, cy, rx, ry, bail_cx, bail_cy, bail_or, bail_ir, sparkles):
    h = len(px)
    w = len(px[0])
    for y in range(h):
        for x in range(w):
            dx = x - cx
            dy = y - cy
            # Diamond / faceted-gem metric.
            val = abs(dx) / rx + abs(dy) / ry
            color = None
            if val <= 0.80:
                # Gem interior: vertical purple->cyan gradient.
                t = (y - (cy - ry)) / (2 * ry)
                t = max(0.0, min(1.0, t))
                base = lerp(PURPLE, CYAN, t)
                # Facet shading: lighten upper-left, darken lower-right.
                shade = (-dx - dy) * 6
                col = clampc((base[0] + shade, base[1] + shade, base[2] + shade))
                # Crisp facet seams along the diagonals of the cut.
                if abs(abs(dx) / rx - abs(dy) / ry) < 0.07:
                    col = clampc((col[0] - 35, col[1] - 35, col[2] - 35))
                # Bright vertical core highlight.
                if abs(dx) <= 1 and dy < 0:
                    col = clampc((col[0] + 45, col[1] + 45, col[2] + 55))
                color = (col[0], col[1], col[2], 255)
            elif val <= 1.00:
                # Gold frame, lit from the upper-left.
                lit = (-dx - dy)
                if lit > 3:
                    g = GOLD_HI
                elif lit < -3:
                    g = GOLD_LO
                else:
                    g = GOLD
                color = (g[0], g[1], g[2], 255)
            elif val <= 1.16:
                color = (OUTLINE[0], OUTLINE[1], OUTLINE[2], 255)

            # Bail (the loop the chain would pass through), drawn on top.
            bd = math.hypot(x - bail_cx, y - bail_cy)
            if bail_ir <= bd <= bail_or:
                lit = (bail_cy - y) + (bail_cx - x)
                g = GOLD_HI if lit > 1 else (GOLD if lit > -2 else GOLD_LO)
                color = (g[0], g[1], g[2], 255)
            elif bail_or < bd <= bail_or + 1.1 and y < cy - ry + 2:
                if color is None:
                    color = (OUTLINE[0], OUTLINE[1], OUTLINE[2], 255)

            if color is not None:
                px[y][x] = color

    # Star sparkles in otherwise-empty space.
    for (sx, sy, size) in sparkles:
        if 0 <= sy < h and 0 <= sx < w:
            px[sy][sx] = (SPARK[0], SPARK[1], SPARK[2], 255)
            for k in range(1, size + 1):
                for (ax, ay) in ((sx + k, sy), (sx - k, sy), (sx, sy + k), (sx, sy - k)):
                    if 0 <= ay < h and 0 <= ax < w and px[ay][ax][3] == 0:
                        fade = 255 - (k - 1) * 90
                        px[ay][ax] = (SPARK_ARM[0], SPARK_ARM[1], SPARK_ARM[2], max(60, fade))


# ---- Item sprite: 32x32 ----
item = blank(32, 32)
draw_amulet(item, cx=16, cy=19.0, rx=8.5, ry=10.5,
            bail_cx=16, bail_cy=6.0, bail_or=3.3, bail_ir=1.5,
            sparkles=[(5, 10, 1), (27, 12, 1), (25, 27, 1), (6, 26, 1)])
write_png("Content/Items/Accessories/SorcerersReliquary.png", item)

# ---- Mod icon: 80x80 with a soft dark backdrop ----
icon = blank(80, 80)
for y in range(80):
    for x in range(80):
        d = math.hypot(x - 40, y - 40)
        if d <= 39:
            # Radial indigo backdrop, lighter in the centre (a faint glow).
            t = d / 39
            bg = clampc(lerp((58, 40, 96), (20, 16, 42), t))
            a = 255 if d <= 37 else int(255 * (39 - d) / 2)
            icon[y][x] = (bg[0], bg[1], bg[2], max(0, a))
draw_amulet(icon, cx=40, cy=44.0, rx=20.0, ry=25.0,
            bail_cx=40, bail_cy=15.0, bail_or=7.5, bail_ir=3.4,
            sparkles=[(15, 22, 2), (64, 26, 2), (62, 62, 2), (16, 60, 2), (40, 10, 1)])
write_png("icon.png", icon)

print("wrote Content/Items/Accessories/SorcerersReliquary.png (32x32) and icon.png (80x80)")
