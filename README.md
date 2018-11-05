# GameStoreMID

GameStoreMID video game e-commerce [ASP.Net Core](https://www.asp.net/core/overview/aspnet-vnext)  MVC application

## Key Features

  - Admin interface - REST based system, see your most viewed games, customers order locations and how the ML algorithm connected the products.
  - Recommendation system powered by machine learning
  - Integrated with Facebook, Twitter, GoogleMaps and IGDB
  - Beautiful and mobile-friendly interface
  - Review system: rate and review every product, every vote counts!
  - Order and cart system - order your favorite games(except for the actual payment)

## Tech

GameStoreMID uses a number of open source projects to work properly:

* [ASP.Net Core](https://www.asp.net/core/overview/aspnet-vnext) - is a free and open-source web framework
* [IGBD](https://api.igdb.com/) - our source for images, descreption, videos of games.
* [3d.js](https://d3js.org/) - great javascript library for graphing stuff
* [Twitter Bootstrap](http://twitter.github.com/bootstrap/) - great UI boilerplate for modern web apps
* [ColoSHOP](https://colorlib.com/etc/coloshop/index.html) - beautiful free e-commerce template.
* [jQuery](http://jquery.com) - is a JavaScript library designed to simplify the client-side scripting of HTML.

## Installation

GameStoreMID requires [ASP.Net](https://www.asp.net/core/overview/aspnet-vnext) and [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017) to run.

 - open ./GameStoreMid/appsetting.json and make sure to replace all "Your Key" with the relevant API keys.
 - open the sln file on the main folder with Visual Studio, and type the following command in the NuGet console

```sh
$ Update-Database
```
This will update your local SQL Server 30 game loaded from IGDB, give them random prices and create admin user.
```
user: mid@mid.com
password: Qwe123!
```
## Pictures
![Landing Page](https://raw.githubusercontent.com/dotanme/GameStoreMid/master/readmepics//main.jpg)
![Best Sellers](https://raw.githubusercontent.com/dotanme/GameStoreMid/master/readmepics/bestsellers.JPG)

![Product Page](https://raw.githubusercontent.com/dotanme/GameStoreMid/master/readmepics//products.jpg)

![Similar graph](https://raw.githubusercontent.com/dotanme/GameStoreMid/master/readmepics//similargraph.jpg)

![Most Viewed Graph](https://raw.githubusercontent.com/dotanme/GameStoreMid/master/readmepics//mostviewed.jpg)
![Products Management](https://raw.githubusercontent.com/dotanme/GameStoreMid/master/readmepics//prodcutspanel.jpg)




#### This project was made for our Web Application Course and was not made for commercial use :)

#### Made By Matan Avitan, Ilan Lidovski & Dotan Menachem
