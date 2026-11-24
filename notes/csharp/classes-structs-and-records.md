# C# Classes, Structs, and Records

A practical reference for understanding the differences between `class`, `struct`, `record class`, and `record struct` in C#.

## Table of contents

1. [The big picture](#the-big-picture)
2. [Classes](#1-classes)
3. [Structs](#2-structs)
4. [Records](#3-records)
5. [Record structs](#4-record-structs)
6. [Equality: `==`, `Equals()`, and `ReferenceEquals()`](#5-equality--equals-and-referenceequals)
7. [Assignment and copying](#6-assignment-and-copying)
8. [Choosing the right type](#7-choosing-the-right-type)
9. [Common misconceptions](#8-common-misconceptions)

---

## The big picture

C# gives you two main categories of types:

- **Reference types**: variables hold a reference to an object. Assigning one variable to another copies the reference, so both variables can refer to the same object.
- **Value types**: variables hold their own value. Assigning one variable to another copies that value.

| Type                      | Category       | Equality by default                                           | Inheritance                                                           |
| ------------------------- | -------------- | ------------------------------------------------------------- | --------------------------------------------------------------------- |
| `class`                   | Reference type | Reference equality                                            | Can inherit from one class and implement multiple interfaces          |
| `struct`                  | Value type     | Value equality through `Equals()`; no generated `==` operator | Cannot inherit from another class or struct; can implement interfaces |
| `record class` / `record` | Reference type | Value-based equality                                          | Record classes can inherit from other record classes                  |
| `record struct`           | Value type     | Value-based equality, including generated `==`                | Cannot inherit; can implement interfaces                              |

> **Important:** `record` by itself declares a reference-type record (`record class`). Use `record struct` when you specifically want a value-type record.

---

## 1. Classes

A **class** is a reference type that defines the data and behavior an object can have.

A class can contain:

- Fields
- Properties
- Methods
- Constructors
- Events
- Operators
- Nested types

### Example

```csharp
public class BankAccount
{
    public string Owner { get; set; }
    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal initialBalance)
    {
        Owner = owner;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        Balance += amount;
    }
}
```

Create and use an object:

```csharp
var account = new BankAccount("Maya", 500m);

account.Deposit(150m);

Console.WriteLine(account.Balance); // 650
```

### Main characteristics

1. **Reference type** — a variable stores a reference to an object, not the object data itself.
2. **Mutable or immutable** — classes are not automatically mutable. Their properties and fields determine whether their state can change.
3. **Inheritance** — a class can inherit from one base class.
4. **Interfaces** — a class can implement multiple interfaces.
5. **Behavior and state** — classes are useful for objects that combine data with operations.
6. **Identity** — two different objects can contain identical data but still represent different objects.

### When should you use a class?

Use a class when:

- The object has behavior or complex state.
- Its state needs to change over time.
- You need inheritance or polymorphism.
- Multiple parts of your program should refer to the same object.
- The object's identity matters independently of its property values.
- The object is relatively large and copying all its data would be undesirable.

Examples: `BankAccount`, `ShoppingCart`, `UserSession`, `OrderService`.

---

## 2. Structs

A **struct** is a value type. Unlike a class, assigning a struct variable to another variable copies the struct's data.

Structs can contain fields, properties, methods, and constructors. They are not limited to holding data, although they are generally best suited to small value-like types.

### Example

```csharp
public struct Temperature
{
    public double Celsius { get; set; }

    public Temperature(double celsius)
    {
        Celsius = celsius;
    }

    public double ToFahrenheit()
    {
        return (Celsius * 9 / 5) + 32;
    }
}
```

### Main characteristics

1. **Value type** — assignment copies the value.
2. **Not automatically immutable** — a normal struct can have settable properties and its values can change.
3. **No class inheritance** — a struct cannot inherit from another class or struct.
4. **Interfaces** — a struct can implement one or more interfaces.
5. **Value equality through `Equals()`** — the default implementation compares values, but a normal struct does not automatically get a `==` operator.
6. **Copy behavior** — changing one copy does not change another copy's own fields.
7. **Boxing** — a struct can be boxed when converted to `object` or an implemented interface in a way that requires boxing.

### When should you use a struct?

Consider a struct when:

- It represents one value or a small group of closely related values.
- Value semantics make sense: a copy should be independent.
- It is small and inexpensive to copy.
- It is preferably immutable.
- You do not need class inheritance.

Examples: `Point`, `Color`, `Temperature`, `DateTime` (a .NET value type).

**Size note:** Keep structs small. Microsoft's guidance commonly describes small structs as roughly 16 bytes or less, but this is a guideline, not a strict language rule. Do not choose a struct based only on a byte-size threshold.

### Prefer immutable structs when possible

A mutable struct can be confusing because it is copied when assigned or passed by value. A `readonly struct` makes the entire struct immutable after construction:

```csharp
public readonly struct ScreenSize
{
    public int Width { get; }
    public int Height { get; }

    public ScreenSize(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
```

---

## 3. Records

A **record** is designed for types whose main purpose is to hold data.

By default, `record` creates a **reference type** (`record class`). Records provide compiler-generated features, including:

- Value-based equality.
- A useful generated `ToString()` implementation.
- A `with` expression for creating a copy with selected changes.
- Concise positional syntax.

### Positional syntax

```csharp
public record ProductDto(
    int Id,
    string Name,
    decimal Price
);
```

The positional parameters create properties and a constructor.

Use it like this:

```csharp
var product = new ProductDto(10, "Keyboard", 249.90m);

Console.WriteLine(product.Name); // Keyboard
Console.WriteLine(product);
// ProductDto { Id = 10, Name = Keyboard, Price = 249.90 }
```

The generated properties in a positional record class use `init`, so they can be assigned during initialization but not normally changed afterward.

### Non-positional syntax

You can also write a record with an explicit body:

```csharp
public record ShippingAddress
{
    public string City { get; init; }
    public string Street { get; init; }

    public ShippingAddress(string city, string street)
    {
        City = city;
        Street = street;
    }
}
```

### Main characteristics

1. **Reference type by default** — `record` is equivalent to `record class`.
2. **Value-based equality** — two records with the same compared data can be equal even if they are separate objects.
3. **Convenient immutability** — positional record properties are `init`-only by default, but a record is not guaranteed to be immutable.
4. **`with` expressions** — create a copy with selected properties changed.
5. **Generated `ToString()`** — displays the record type and its public property values.
6. **Inheritance** — record classes can inherit from other record classes. A record class cannot inherit from an ordinary class, and an ordinary class cannot inherit from a record class.

### When should you use a record?

Use a record when:

- The main purpose of the type is to represent data.
- Two instances with the same data should be considered equal.
- You want convenient immutable-style data models.
- You want concise DTOs (Data Transfer Objects).
- You want a readable generated `ToString()`.

Examples: API request/response DTOs, search filters, configuration data, and messages passed between application layers.

### `with` expression

A `with` expression creates a new record based on an existing one and applies the changes you specify.

```csharp
var original = new ProductDto(10, "Keyboard", 249.90m);

var discounted = original with { Price = 199.90m };

Console.WriteLine(original.Price);   // 249.90
Console.WriteLine(discounted.Price); // 199.90
```

The original record is not changed.

> **Important:** `with` makes a shallow copy. If a record contains a reference to a mutable object (for example, a list), the original and copied records can still refer to the same list.

---

## 4. Record structs

A **record struct** combines the value-type behavior of a struct with the data-focused features of a record.

```csharp
public record struct MapCoordinate(double Latitude, double Longitude);
```

### Main characteristics

1. **Value type** — assigning it to another variable copies its data.
2. **Value-based equality** — the compiler generates equality members, including `==` and `!=`.
3. **Generated `ToString()`** — displays the type name and its property values.
4. **`with` expressions** — can create a copy with selected properties changed.
5. **Mutable by default** — positional properties of a normal `record struct` are settable.
6. **No class inheritance** — it cannot inherit from another class or struct, but it can implement interfaces.

### Example

```csharp
var first = new MapCoordinate(32.5, 35.0);
var second = first;

second.Longitude = 34.8;

Console.WriteLine(first.Longitude);  // 35
Console.WriteLine(second.Longitude); // 34.8

Console.WriteLine(first == second);  // False
```

`second` is an independent copy, so changing it does not change `first`.

### Immutable record structs

Use `readonly record struct` when you want the record struct's data to be immutable:

```csharp
public readonly record struct MapCoordinate(
    double Latitude,
    double Longitude
);
```

### When should you use a record struct?

Use one when:

- The data is small and value-like.
- Copies should be independent.
- You want value-based equality and `with` expressions.
- You want an immutable value type.

---

## 5. Equality: `==`, `Equals()`, and `ReferenceEquals()`

These comparisons are important because classes, structs, and records do not all use the same default equality behavior.

### A. Class: reference equality

A normal class uses reference equality by default. Two separate objects are not equal just because their properties contain the same values.

```csharp
public class Movie
{
    public string Title { get; set; }
    public int Year { get; set; }

    public Movie(string title, int year)
    {
        Title = title;
        Year = year;
    }
}

var movie1 = new Movie("Arrival", 2016);
var movie2 = new Movie("Arrival", 2016);

Console.WriteLine(movie1 == movie2);       // False
Console.WriteLine(movie1.Equals(movie2));  // False

var movie3 = movie1;

Console.WriteLine(movie1 == movie3);       // True
Console.WriteLine(ReferenceEquals(movie1, movie3)); // True
```

`movie1` and `movie2` are separate objects. `movie1` and `movie3` refer to the same object.

### B. Struct: value equality with `Equals()`

A normal struct's default `Equals()` compares its values.

```csharp
public struct BookDimensions
{
    public double Width { get; set; }
    public double Height { get; set; }
}

var dimensions1 = new BookDimensions { Width = 15, Height = 22 };
var dimensions2 = new BookDimensions { Width = 15, Height = 22 };

Console.WriteLine(dimensions1.Equals(dimensions2)); // True
```

**Important:** A normal struct does not automatically have a `==` operator. The following does not compile unless you define that operator yourself:

```csharp
// Console.WriteLine(dimensions1 == dimensions2);
```

### C. Record: value-based equality

A record compares its data rather than requiring both variables to refer to the same object.

```csharp
public record FlightTicket(string Destination, string Seat);

var ticket1 = new FlightTicket("Tokyo", "12A");
var ticket2 = new FlightTicket("Tokyo", "12A");

Console.WriteLine(ticket1 == ticket2);      // True
Console.WriteLine(ticket1.Equals(ticket2)); // True
Console.WriteLine(ReferenceEquals(ticket1, ticket2)); // False
```

The records contain the same data, so they are equal, but they are two different objects.

### Quick comparison

| Type          | `==` by default              | `Equals()` by default |
| ------------- | ---------------------------- | --------------------- |
| Normal class  | Reference equality           | Reference equality    |
| Normal struct | Not available unless defined | Value equality        |
| Record class  | Value-based equality         | Value-based equality  |
| Record struct | Value-based equality         | Value-based equality  |

> Equality can be customized by overriding `Equals()` and defining operators. The table describes the default behavior.

---

## 6. Assignment and copying

This is one of the most important differences to understand.

### A. Assigning a class copies the reference

```csharp
public class Playlist
{
    public string Name { get; set; }

    public Playlist(string name)
    {
        Name = name;
    }
}

var playlist1 = new Playlist("Workout");
var playlist2 = playlist1;

playlist2.Name = "Road Trip";

Console.WriteLine(playlist1.Name); // Road Trip
Console.WriteLine(playlist2.Name); // Road Trip
```

Both variables refer to the same object. Assigning `playlist1` to `playlist2` does not create a second `Playlist`.

### B. Assigning a struct copies the value

```csharp
public struct TimeRange
{
    public int StartHour { get; set; }
    public int EndHour { get; set; }
}

var range1 = new TimeRange { StartHour = 9, EndHour = 12 };
var range2 = range1;

range2.EndHour = 15;

Console.WriteLine(range1.EndHour); // 12
Console.WriteLine(range2.EndHour); // 15
```

`range2` receives its own copy. Changing it does not change `range1`.

### C. Assigning a record class copies the reference

A record class is still a reference type. The `record` keyword changes equality and adds other features; it does not change reference-type assignment behavior.

```csharp
public record PlayerProfile(string Username, int Level);

var profile1 = new PlayerProfile("Nova", 8);
var profile2 = profile1;

Console.WriteLine(ReferenceEquals(profile1, profile2)); // True
Console.WriteLine(profile1 == profile2);                 // True
```

Both variables refer to the same record object.

### D. Assigning a record struct copies the value

```csharp
public record struct AudioSettings(int Volume, bool IsMuted);

var settings1 = new AudioSettings(70, false);
var settings2 = settings1;

settings2.Volume = 30;

Console.WriteLine(settings1.Volume); // 70
Console.WriteLine(settings2.Volume); // 30
```

Each variable has its own copy.

---

## 7. Choosing the right type

| Situation                                          | A good starting choice                        | Why                                              |
| -------------------------------------------------- | --------------------------------------------- | ------------------------------------------------ |
| An object has identity and changing state          | `class`                                       | Reference semantics and behavior                 |
| You need inheritance or polymorphism               | `class`                                       | Class inheritance supports this                  |
| You mainly store data and want value equality      | `record class`                                | Built-in equality and convenient syntax          |
| You need a small value that copies independently   | `struct`                                      | Value semantics                                  |
| You need a small value, value equality, and `with` | `record struct`                               | Combines struct and record features              |
| You need immutable small value data                | `readonly struct` or `readonly record struct` | Prevents changes to the value after construction |

### Practical examples

- `CustomerAccount` — usually a **class**: it has identity, behavior, and changing state.
- `CreateOrderRequest` — often a **record class**: it represents data sent to an API.
- `Temperature` — often a **readonly struct**: it represents a small value.
- `GeoCoordinate` — often a **readonly record struct**: it is small, value-like, and benefits from generated equality.

These are guidelines, not strict rules. The behavior your application needs should determine the choice.

---

## 8. Common misconceptions

1. **"Classes are always mutable."**  
   No. A class can be designed to be immutable, for example by using read-only properties and constructor initialization.

2. **"Structs are always immutable."**  
   No. A normal struct can have settable properties. Use `readonly struct` when you need immutability.

3. **"Records are always immutable."**  
   No. Positional record-class properties are `init`-only by default, but records can have mutable members. A normal `record struct` is mutable by default.

4. **"Records are value types."**  
   Not necessarily. `record` means `record class` unless you explicitly write `record struct`.

5. **"A struct is always stored on the stack and a class is always stored on the heap."**  
   This is an oversimplification. Storage depends on context. For example, a struct can be a field inside a class object, and boxing can place a copy of a struct in a heap object. Focus first on value versus reference semantics.

6. **"Two objects with the same properties are always equal."**  
   No. Two normal class instances are not equal by default just because their properties match. Records provide value-based equality by default.

7. **"A `with` expression changes the original record."**  
   No. It creates a copy and applies the specified changes to that copy.

---

## Microsoft documentation

- [Classes](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes)
- [Structs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/structs)
- [Records](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records)
- [Choose between tuples, records, structs, and classes](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/tutorials/choosing-types)
- [Equality comparisons](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/expressions/equality)
