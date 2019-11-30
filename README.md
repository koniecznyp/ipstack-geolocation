# ipstack-geolocation

This is a small application for retrieving location information based on an IP address or URL. Location data comes from the site [ipstack](https://ipstack.com/). To use their API you must create a free account and receive your own access key.

## Setup
The application is extremely easy to configure. All you have to do is fill in the access key in the configuration file.
```
"ipstack": {
  "address": "http://api.ipstack.com/",
  "accessKey": "ACCESS_KEY"
}
```
A ``Dockerfile`` file has been created for the application, which makes it easy to launch it using the ``docker``
```
docker build -t geolocation-api . 
docker run -d -p 5000:5000 --name=geo-api geolocation-api
```
## Usage
For simplicity of application creation, a [swagger](https://swagger.io/) has been added, which can be accessed by typing in the browser
```
http://localhost:5000/swagger
```
The application API takes one parameter, which is the ``address``, which is passed in ``body`` as ``JSON``. In the address field you can pass the IP address or URL.

### Example 1:

``POST`` Request:
```
{
  "address": "8.8.8.8"
}
```
``GET`` Response:
```
{
  "ip": "8.8.8.8",
  "city": "Mountain View",
  "latitude": 37.419158935546875,
  "longitude": -122.07540893554688
}
```
### Example 2:

``POST`` Request:
```
{
  "address": "www.ebay.com"
}
```
``GET`` Response:
```
{
  "ip": "184.27.29.127",
  "city": "San Jose",
  "latitude": 37.330528259277344,
  "longitude": -121.83822631835938
}
```
The ``ipstack`` service provides a lot more information but for simplicity the amount of data in the response has been limited. This can be easily changed by extending the ``DTO``(data transfer object) class with new properties without violating the internal logic of the application.

> To run ``End-to-End`` and ``Integration`` tests, enter the access key to the ``appsettings.json`` file in test projects
