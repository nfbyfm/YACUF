# YACUF

Yet another c-sharp utility-framework. Library with classes and functions often used in other applications.

## Utilities

Contains simple methods for checking / adding entries in a class-instance implementing the IEnumerable-Interface.


## Mail

Code based on based / copied from [David M Brooks code-project-solution](https://www.codeproject.com/Articles/17561/Programmatically-adding-attachments-to-emails-in-C).


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
