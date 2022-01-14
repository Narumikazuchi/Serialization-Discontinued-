![Logo](../master/logo.png)

# Utility Library
This library originally contained all classes that I used in my projects more than once. I made them universal instead of being hardcoded for the project in question and added them to the library in order to reuse them whenever needed. With time the library grew and not all aspects were needed in every project. That's when I decided to split them into organized individual pieces and also publish them on github as well as nuget.org.

# Serialization
This project provides a simple interface for a user to serialize data. The interface(s) can be implemented by calsses and structs and then serialized using any serializer build on this architecture.
  
## Implementation
### Serialization
[![NuGet](https://img.shields.io/nuget/v/Narumikazuchi.Serialization.svg)](https://www.nuget.org/packages/Narumikazuchi.Serialization)  
The installation can be simply done via installing the nuget package or by downloading the latest release here from github and referencing it in your project. You can then start to implement your own serializer and/or deserializer through the use of the provided interfaces. For a detailed explanation of the interfaces and classes in this library look in the [wiki](https://github.com/Narumikazuchi/Serialization/wiki).
