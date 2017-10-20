---
layout: default
---

The **IConverter<T>** interface is used to convert your class types into the content type and vice versa. The **IConverter<T>** interface consists of the three methods listed below.

``` csharp
string ContentType {get;}
T ConvertToObject(string content);
string ConvertToContent(T type);
```

There are two implementations of the interface that are provided with the library currently for JSON (**JsonContentConverter<T>**) and XML (**XmlContentConverter<T>**) conversion. If there are other implementations that you need feel free to implement the interface or submit a PR to the repository.