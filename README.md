<div id="top"></div>

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/AlphaRalph/IoTSensorBase">
    <img src="images/screenshot_1.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">IOT Sensor Base</h3>

  <p align="center">
    A project at FH-Wels.
    <br />
    <a href="https://github.com/AlphaRalph/IoTSensorBase"><strong>Explore the docs »</strong></a>
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>      
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
	    <li><a href="#HardwareSelection">Selection of suitable hardware</a></li>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#discussion">Discussion</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

[![IoT SensorBase][product-screenshot]]

The aim is to develop a sensor platform, which is independent of the measured variables, records data and sends it to a cloud service.
In cloud-services this data is analysed and visualised.

Project scope:
* Familiarisation with current IoT technologies
* Creation of a universal software architecture
* Realisation of a Showcase, e.g."Environmental measurement"

Fields of activity:
* Softwaredevelopment/programming with .NET, Python
* Database and SQL
* Cloud Services
* Sensors
* Communication ( WIFI,LTE,etc...)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- Getting Started -->
## Getting Started

### HardwareSelection

Since a Raspberry was already provided by the FH-Wels, there was no need to look for an alternative here.

The selection of sensors and LTE modul was done by Andrej Pervan

<p align="right">(<a href="#top">back to top</a>)</p>

### Prerequisites

MongoDB 5.0 under Raspberry Pi OS (64-bit) [https://andyfelong.com/2021/08/mongodb-4-4-under-raspberry-pi-os-64-bit-raspbian64/]

To run the C# .NET Code built on Visual Studio 2017 we used MONO:
   ```sh
   sudo apt install mono-complete
   ```

### Installation

How to install IOTSensorBase and get KEY for AWS-Cloude

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- discussion -->
## Discussion

### How to read data from sensor

### How to transfert data from sensor to gateway

For the purpose of data security, measured values must be persistently buffered.
This works easiest in a lean database.
At the beginning of the discussion, we wanted to use MariaDB.
It can be used with simple SQL statements. 
Advantages of this database are 
* Easy installation on Raspberry
* Easy to use in C# code as there are many well-documented application examples.

However, it is an SQL database, which does not bring any advantage with the simplicity of our data.
We do not need any relations between sets of data.
For this reason, we went looking for a NO-SQL database.
In combination with Raspberry-Pi we decided to use a MONGO-DB.
Only in the course of the project did we become aware of some difficulties.
* Old Mongo DBs no longer work with the current C# drivers.
* new Mongo DBs cannot be easily installed on the latest Raspberry OS versions

Translated with www.DeepL.com/Translator (free version)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- Roadmap -->
## Roadmap

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[contributors-url]: https://github.com/alphaRalph/IoTSensorBase/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[forks-url]: https://github.com/alphaRalph/IoTSensorBase/network/members
[stars-shield]: https://img.shields.io/github/stars/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[stars-url]: https://github.com/alphaRalph/IoTSensorBase/stargazers
[issues-shield]: https://img.shields.io/github/issues/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[issues-url]: https://github.com/alphaRalph/IoTSensorBase/issues
[license-shield]: https://img.shields.io/github/license/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[license-url]: https://github.com/alphaRalph/IoTSensorBase/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/alphaRalph
[product-screenshot]: images/screenshot_1.png