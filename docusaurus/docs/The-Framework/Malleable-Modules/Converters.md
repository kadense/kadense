---
title: Converters
sidebar_position: 2
---

Once you've got your Malleable models, it's likely that you're going to want to do more than just store the data you've received. So let's say that we've got a class, that contains the name of an author.

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: conversion-tutorial
  namespace: default
spec:
  classes:
    TutorialV1:
      properties:
        Author:
          type: string
    Author:
      properties:
        FirstName:
          type: string
        Surname:
          type: string
    TutorialV2:
      properties:
        Author:
          type: list
          subType: string
```

Now lets say that requirements have changed and they want to split out The first name and surname:

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: conversion-tutorial
  namespace: default
spec:
  classes:
    TutorialV1:
      properties:
        Author:
          type: string

    TutorialV2:
      properties:
        FirstName:
          type: string
        Surname:
          type: string
```

So now we've got the models, but obviously we need to update the V1 events to our V2 events. For our tutorial we'll keep it simple and simply assume that the first name and surname will be split by a space character. We'll create a test object for the system:

```json
{
    "Author" : "Joe Bloggs"
}
```

So that the system can handle this, we will create a **MalleableConverterModule**.

```yaml
apiVersion: kadense.io/v1
kind: MalleableConverterModule
metadata:
  name: converter
  namespace: default
spec:
  converters:
    FromTutorialV1ToTutorialV2:
        from:
            className: TutorialV1
            moduleName: conversion-tutorial
            moduleNamespace: default
        to:
            className: TutorialV2
            moduleName: conversion-tutorial
            moduleNamespace: default
        expressions:
            FirstName: Source.Author.Split(' ')[0]
            Surname: Source.Author.Split(' ')[1]
```

Now that Kadense knows how to handle the conversions we can process the changes through it:

```json
{
    "FirstName" : "Joe",
    "Surname" : "Bloggs"
}
```

Using dynamic expressions we can make more complicated models and transformations:

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: conversion-tutorial
  namespace: default
spec:
  classes:
    TutorialV1:
      properties:
        Author:
          type: string
    Contact:
      properties:
        FirstName:
          type: string
        Surname:
          type: string
    TutorialV2:
      properties:
        Authors:
          type: list
          subType: Contact
        
```

With a template


```yaml
apiVersion: kadense.io/v1
kind: MalleableConverterModule
metadata:
  name: converter
  namespace: default
spec:
  converters:
    FromTutorialV1ToTutorialV2:
        from:
            className: TutorialV1
            moduleName: conversion-tutorial
            moduleNamespace: default
        to:
            className: TutorialV2
            moduleName: conversion-tutorial
            moduleNamespace: default
        expressions:
            Authors: |
              new List<string>()
              {
                new Contact()
                {
                  FirstName: Source.Author.Split(' ')[0],
                  Surname: Source.Author.Split(' ')[1]    
                }
              }
            
```

Which will return:

```json
{
    "Authors" : [
        {
            "FirstName" : "Joe",
            "Surname" : "Bloggs"
        }
    ]
}
```


