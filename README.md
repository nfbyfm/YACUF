# YACUF

Yet another c-sharp utility-framework. Library with classes and functions often used in other applications.

## Utilities

### Enumerable extensions
Contains simple methods for checking / adding entries in a class-instance implementing the IEnumerable-Interface.

### File utilites

Constains methods for saving / loading serializable objects to xml-files.

### Crypto utilities

Methods for saving and loading serializable objects as encrypted binary-files.

## Mail

Code based on / copied from [David M Brooks code-project-solution](https://www.codeproject.com/Articles/17561/Programmatically-adding-attachments-to-emails-in-C).


```csharp
MailAPI mail = new MailAPI();

mail.AddAttachment("c:\\temp\\file1.txt");
mail.AddRecipientTo("person1@somewhere.com");
mail.SendMailPopup("testing", "body text");
```




<!--
## How to use the Library

The main entry-point is ...

-->
