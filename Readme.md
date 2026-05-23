\# Hypah.ToolBox

A collection of utility classes for C# to simplify common tasks.



\## Features



\### Logging

A simple and extensible logging framework.



\*\*Basic Usage\*\*

To start logging, add a logger receiver and then call the static `Log` methods.



```csharp

using Hypah.Logging;

using Hypah.Logging.Loggers;



// Convienent method to add loggers

Log.AddConsoleLogger();

Log.AddFileLogger("logs/log.txt");



// Adds log files per day in this directory

Log.AddLogFiles("logs/");



// Add a custom logger

Log.Receivers.Add(new ConsoleLogger());



// Add a file logger that creates a new log file every day

Log.Receivers.Add(new FileLogger("logs/log.txt") { AutoCreate = true });



// Log messages

Log.Info("This is an informational message.");

Log.Warning("This is a warning message.");

Log.Error("This is an error message.");

```



\*\*Component Loggers\*\*

Use `ComponentLogger` to add a component name to all logs from a specific class.



```csharp

using Hypah.Logging;



public class MyService

{

&#x20;   private readonly ComponentLogger \_logger = new ComponentLogger("MyService");



&#x20;   public void DoWork()

&#x20;   {

&#x20;       \_logger.Info("Doing some work.");

&#x20;       // Log output will be: \[MyService] Doing some work.

&#x20;   }

}

```



\### File Path Manipulation

The `FilePath` class provides a more robust way to handle file paths. It overloads the `/` operator to combine paths.



```csharp

using Hypah.Utility.Files;



var root = new FilePath("C:/Users/Default");

var documents = root / "Documents";

var myFile = documents / "MyFile.txt";



Console.WriteLine(myFile.ToString()); // C:\\Users\\Default\\Documents\\MyFile.txt

Console.WriteLine(myFile.Parent);     // C:\\Users\\Default\\Documents

Console.WriteLine(myFile.Exists);     // Checks if the file exists

```



\### Serialization



\#### XML Serialization

A generic XML serializer for any class.



```csharp

using Hypah.Utility.Files.Serialization;



public class MyData

{

&#x20;   public string Name { get; set; }

&#x20;   public int Value { get; set; }

}



var serializer = new GenericXmlSerializer<MyData>();

var data = new MyData { Name = "Test", Value = 123 };



serializer.SerializeXml(data, "data.xml");



var deserializedData = serializer.DeserializeXml("data.xml");

```



\#### CSV Serialization

A generic CSV serializer for lists of objects. The header is generated from the public properties of the class.



```csharp

using Hypah.Utility.Files.Serialization;

using System.Collections.Generic;



public class CsvData

{

&#x20;   public int Id { get; set; }

&#x20;   public string Name { get; set; }

}



var serializer = new CsvSerializer<CsvData>();

var data = new List<CsvData>

{

&#x20;   new CsvData { Id = 1, Name = "Alice" },

&#x20;   new CsvData { Id = 2, Name = "Bob" }

};



serializer.Write("data.csv", data);



var readData = serializer.Read("data.csv");

```



\#### Binary Serialization

A serializer for converting objects to a byte array and back.



\*\*Note:\*\* When deserializing classes, they must have a parameterless constructor.



```csharp

using Hypah.Utility.Files.Serialization;



var serializer = new BinarySerializer();

var data = "Hello, World!";



byte\[] serializedData = serializer.Serialize(data);

string deserializedData = serializer.Deserialize<string>(serializedData);



// Also works with complex objects

var myObject = new MyData { Name = "Binary", Value = 42 };

byte\[] serializedObject = serializer.Serialize(myObject);

MyData deserializedObject = serializer.Deserialize<MyData>(serializedObject);



```



\### OS Execution



\#### Running Processes

A fluent API to start and configure processes.



```csharp

using Hypah.Utility.Files;

using Hypah.Utility.Os.Execution;



var process = Execute.Process("cmd.exe")

&#x20;   .WithArguments("/c", "echo Hello, World!")

&#x20;   .WithWorkingDirectory("C:/")

&#x20;   .SuppressWindow()

&#x20;   .Start();

&#x20;   

process?.WaitForExit();

```



\#### Opening Files and Folders in Explorer

Windows-only helpers to interact with `explorer.exe`.



```csharp

using Hypah.Utility.Files;

using Hypah.Utility.Os.Execution;



// Open explorer with a file selected

Explorer.OpenFileLocation(new FilePath("C:/path/to/file.txt"));



// Open a folder in explorer

Explorer.OpenFolder(new FilePath("C:/path/to/folder"));

```



\#### Terminating Processes

Kill processes by name.



```csharp

using Hypah.Utility.Os.Execution;



// Kill all notepad processes

int killedCount = Terminate.Kill("notepad");

```



\### Performance Benchmarking

A simple tool to benchmark an `Action`.



```csharp

using System;

using Hypah.Utility.Performance;



Benchmark.Run(() => 

{

&#x20;   // The code to benchmark

&#x20;   for (int i = 0; i < 1000; i++)

&#x20;   {

&#x20;       // some work

&#x20;   }

}, iterations: 100, numberOfBenchmarks: 5);

```



\### Primitive Parsers

These helpers use `CultureInfo.InvariantCulture` for consistent parsing and string conversion of floating-point numbers.



```csharp

using Hypah.Utility.Primitives;



float f = Float.Parse("123.45");

double d = Double.Parse("123.45");

decimal m = Decimal.Parse("123.45");



string s = Float.ToString(f);

```



\### Reflection

A wrapper around `System.Type` to simplify common reflection tasks.



```csharp

using Hypah.Utility.Reflection;



var reflectionType = new ReflectionType(typeof(MyData));



// Get all public properties and fields

var members = reflectionType.GetMembers(MemberAccess.OnlyPublic);



// Create an instance

var instance = reflectionType.Create<MyData>();

```



\### String Extensions

A set of useful extension methods for `string`.



\- `Between(start, end)`: Gets the text between two strings.

\- `IndexOfReverse(needle, startIndex)`: Finds the last index of a substring.

\- `ToTitleCase()`: Converts the string to title case.

\- `Capitalize()`: Capitalizes the first character.

\- `IsCapitalized()`: Checks if the first character is capitalized.

\- `IsLowerCase()`: Checks if the entire string is lower case.

\- `IsEmpty()`: Alias for `string.IsNullOrEmpty()`.

\- `IsWhiteSpace()`: Alias for `string.IsNullOrWhiteSpace()`.

\- `Left(count)`: Returns the leftmost characters.

\- `Right(count)`: Returns the rightmost characters.

\- `Repeat(count)`: Repeats the string.

\- `OnlyDigits()`: Removes all non-digit characters.

\- `TitleCaseToSpaced()`: Converts "TitleCase" to "Title Case".

\- `SplitTrim(separator, trimChars)`: Splits a string and trims the results.

```csharp

using Hypah.Utility.Text;

