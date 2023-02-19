# Raytracer for course NPRG004 Photorealistic graphics
author: Hong Son Ngo

## Usage
- build project in .NET 6/7
- run executable in terminal with command `rt004.exe <output image filename>`
  - if filename is ommited the image is saved under `out.pfm`
  - `rt004.exe` needs to be accompanied with config file `config.txt`
    - parameters are in format `parameterName=value`
    - recognized parameters: `width`, `height`

## Checkpoint 1
Checkpoint 1 project can be downloaded [here](https://github.com/ngohongs/nprg004/tree/be68e02708bdbadad413f0902dcecb7b3a8b21c3/src/rt004)
#### Step 1
- git repository created in [here](https://github.com/ngohongs/nprg004)
- documentation in [here](https://github.com/ngohongs/nprg004/README.md)
- R/O permission added to user [@pepcape](https://github.com/pepcape/)
#### Step 2
- added optional parameter for output image filename
- config file parse implemented using regex parse of `config.txt` file
- logging/debugging system postponed
#### Step 3
- output image showing a period of a sine wave on blue/green gradient background  
