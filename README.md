# Planetary-Visor
![](Images/teaser.png) 

## About
Planetary Visor is an immersive visualization tool that provides geologic and geographic context of the Martian terrain, by localizing it with spectroscopy data from the orbiting satellite. Through the use of virtual reality, we can immerse scientists in the data they study, while providing an intuitive sense of scale that is not possible with traditional tools. This technology is relevant to the ongoing operations of the Curiosity rover and will directly be able to represent the data collected in the Mars 2020 Perseverance rover mission.

A large polyhedron represents a single pixel from the CRISM instrument on the Mars Reconnaissance Orbiter (MRO), a satellite orbiting Mars. The shape of the pixel indicates the angle of incidence of  the MRO when it scanned the ground. In your right hand, a spectral plotter shows the spectral CRISM reading of the pixel. As you drag the pixel around with the right hand trigger button, you can relocate the pixel, dynamically plotting captured spectra of the terrain.

For more information, please refer to our [paper](https://ieeexplore.ieee.org/abstract/document/9417645) and [specifications sheet](https://docs.google.com/document/d/1kGCzEMR2xrff4Ix_1EuVwuAB_juHsJ6bS9WEtmvN8OQ/edit?usp=sharing).


## Downloads
a. [Quest/Quest2 .apk](https://drive.google.com/drive/u/0/folders/1saZhpeA-oVFzD2kpPOjG9ppVDmhHgpDY)

b. [Mac Desktop app](https://drive.google.com/drive/u/0/folders/17XL8BXiAt1yD2579Hp4jL3Yb1FRPufyY)

c. [Windows Desktop app](https://drive.google.com/drive/u/0/folders/17XL8BXiAt1yD2579Hp4jL3Yb1FRPufyY)
## Contents
This repository contains a Unity project (version 2019.4) with a cross-platform VR scene, as well as a desktop scene. 

To use Planetary Visor in VR, open the “Visor Main XR scene”. To use Visor in desktop mode, open the “Desktop Visor” scene. 
In either one, add the [data file](https://drive.google.com/file/d/1CCanj8WCWzXFJ99qGjfwHSd6wggRgGys/view?usp=sharing) to the Assets/Resources folder.
To add the accompanying [HiRISE mesh](https://drive.google.com/file/d/1cAhYsbwRMsdiet7UFBo99PIZeNEpnXHr/view?usp=sharing), drag it to the Assets/Resources/Images folder.
## Import your own datasets
To visualize your own processed data with Planetary Visor, navigate to Assets/Resources and simply replace the the ddr.bytes file with your own. Additionally, you can add your own reflectance data by swapping out the ssa file for your own.
## Terrain
The Marias Pass terrain in this repository was provided by Google Creative Lab's [Access Mars](https://github.com/googlecreativelab/access-mars).
## Contributors

### Team Leads 
[Lauren Gold](https://github.com/LaurenGold), PhD Student <br/>
[Alireza Bahremand](https://github.com/TheWiselyBearded/assign-git), PhD Student <br />
Kathryn Powell, Planetary Scientist, Consultant <br />
[Robert LiKamWa](https://github.com/roblkw-asu), Principal Investigator <br />

### Development Team 
Connor Richards  
Kyle Sese
Justin Hertzberg 
Alexander Gonzalez  
Zoe Purcell 
Hector Taylor    
Don Balanzat       
Olivia Wang   
Jacob Watson 

### Design Team 
Hannah Bartolomea    
Alice Bao  
