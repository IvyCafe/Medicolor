# Medicolor

**Medicolor** is a console tool to convert images color for simulation of color vision deficiency.

| Normal | Protanomaly | Deuteranomaly | Tritanomaly |
| -- | -- | -- | -- |
| ![sample-image](docs/assets/sample1-0.png) | ![sample-image](docs/assets/sample1-1.png) | ![sample-image](docs/assets/sample1-2.png) | ![sample-image](docs/assets/sample1-3.png) |

You can run Medicolor on the major desktop platforms (Linux, macOS, Windows).

<!--
Arch:
- Linux (x64, arm64)
- macOS (x64, arm64)
- Windows (x86, x64, arm64)
-->

<!--
## Installation

(To Be Write)
-->

## Usage

```sh
medicolor <image-path>

# e.g.
# medicolor ./sample.png

# then, select color-type in your console
# (output file name is "./output.png")
```

```sh
medicolor <image-path> <output-color-type>
# <output-color-type>: (1 = Protanomaly, 2 = Deuteranomaly, 3 = Tritanomaly)

# e.g.
# medicolor ./sample 2
```

## Acknowledgments

Super Thanks:

- [Paper: A Physiologically-based Model for Simulation of Color Vision Deficiency](https://www.inf.ufrgs.br/~oliveira/pubs_files/CVD_Simulation/CVD_Simulation.html)
