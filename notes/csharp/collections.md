# C# Collections

Collections are objects used to store and manage groups of values.

C# provides different collection types for different situations. This document focuses on:

- Arrays
- Lists (`List<T>`)
- Dictionaries (`Dictionary<TKey, TValue>`)

## 1. Arrays

An **array** stores a fixed number of elements of the same type.

Once an array is created, its length cannot be changed.

```csharp
string[] students = new string[3];

students[0] = "Maya";
students[1] = "Leo";
students[2] = "Omar";

Console.WriteLine(students[0]); // Maya
Console.WriteLine(students.Length); // 3
```

You can also initialize an array with values directly:

```csharp
string[] students = { "Maya", "Leo", "Omar" };
```

Important points:

- Array indexes start at `0`.
- `Length` returns the number of elements in the array.
- Accessing an index outside the array's bounds causes an `IndexOutOfRangeException`.
- The array's size is fixed, but you can change the values stored in its elements.

## 2. Lists (`List<T>`)

A **List<T>** is a collection that can grow or shrink as elements are added or removed.

`T` is a generic type parameter. It represents the type of elements the list stores.

For example:

```csharp
List<string> students = new List<string>();
```

This creates a list that stores strings.

You can also initialize a list with values:

```csharp
List<string> students = new List<string>
{
    "Maya",
    "Leo",
    "Omar"
};
```

### 2.1 Common List operations

```csharp
List<string> students = new List<string>();

// Add an element to the end of the list.
students.Add("Maya");
students.Add("Leo");
students.Add("Omar");

// Get the number of elements.
int howMany = students.Count;

// Access an element by its index.
string first = students[0];

// Insert an element at a specific index.
students.Insert(1, "Dara");

// Remove the first matching element.
students.Remove("Leo");

// Remove an element by its index.
students.RemoveAt(0);
```

Common properties and methods:

| Member                | Purpose                                                       |
| --------------------- | ------------------------------------------------------------- |
| `Add(item)`           | Adds an element to the end                                    |
| `Insert(index, item)` | Inserts an element at a specific index                        |
| `Remove(item)`        | Removes the first matching element; returns `true` if removed |
| `RemoveAt(index)`     | Removes the element at the specified index                    |
| `Count`               | Property that returns the number of elements                  |
| `list[index]`         | Gets or sets the element at an index                          |
| `Clear()`             | Removes all elements                                          |
| `Contains(item)`      | Checks whether an element exists; returns `bool`              |

**Important:** `Count` is a property, not a method. Write `students.Count`, not `students.Count()`.

### 2.2 Iterating through a List

You can use `foreach` to go through every element in a list:

```csharp
List<string> students = new List<string>
{
    "Maya",
    "Leo",
    "Omar"
};

foreach (string student in students)
{
    Console.WriteLine(student);
}
```

The loop processes each element in the list, one at a time.

### 2.3 Lists of objects

A list can store objects just as it can store strings or integers.

For example, consider a `Customer` class:

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
```

We can create a list of customers:

```csharp
List<Customer> customers = new List<Customer>();

customers.Add(new Customer { Id = 1, Name = "Maya" });
customers.Add(new Customer { Id = 2, Name = "Leo" });
customers.Add(new Customer { Id = 3, Name = "Omar" });
```

Each element in the list is a `Customer` object.

You can access its properties using an index:

```csharp
Customer firstCustomer = customers[0];

Console.WriteLine(firstCustomer.Name); // Maya
```

### 2.4 Finding an object in a List

One way to find a customer by ID is to loop through the list:

```csharp
Customer? foundCustomer = null;
int customerId = 2;

foreach (Customer customer in customers)
{
    if (customer.Id == customerId)
    {
        foundCustomer = customer;
        break;
    }
}

if (foundCustomer is not null)
{
    Console.WriteLine(foundCustomer.Name);
}
else
{
    Console.WriteLine("Customer not found.");
}
```

The `break` statement stops the loop as soon as the matching customer is found.

This works, but it may be inefficient when you frequently need to find objects by their IDs in a large collection. A dictionary can be a better choice for that kind of lookup.

## 3. Dictionaries (`Dictionary<TKey, TValue>`)

A **Dictionary<TKey, TValue>** stores elements as key-value pairs.

- `TKey` represents the type of the key.
- `TValue` represents the type of the value.

For example:

```csharp
Dictionary<int, Customer> customersById =
    new Dictionary<int, Customer>();
```

In this dictionary:

- The key is an `int` representing the customer ID.
- The value is a `Customer` object.

Each key must be unique within the dictionary. Multiple keys can refer to equal values, but the same key cannot be stored more than once.

### 3.1 List vs. Dictionary

These collections are useful for different purposes:

| List                                                     | Dictionary                                              |
| -------------------------------------------------------- | ------------------------------------------------------- |
| Stores elements in a sequence                            | Stores key-value pairs                                  |
| Access elements by index                                 | Access values by key                                    |
| Useful when order and iteration matter                   | Useful when looking up values by a key                  |
| Finding an item by a property usually requires searching | Looking up an existing key is generally O(1) on average |

Think of it this way:

- **List:** What elements are in this collection, and in what sequence?
- **Dictionary:** What value is associated with this key?

If you frequently need to find a customer using their ID, a dictionary is often a suitable choice.

### 3.2 Adding and accessing dictionary elements

You can add key-value pairs using the indexer:

```csharp
Dictionary<int, Customer> customersById =
    new Dictionary<int, Customer>();

Customer Maya = new Customer { Id = 1, Name = "Maya" };
Customer Leo = new Customer { Id = 2, Name = "Leo" };
Customer Omar = new Customer { Id = 3, Name = "Omar" };

customersById[Maya.Id] = Maya;
customersById[Leo.Id] = Leo;
customersById[Omar.Id] = Omar;
```

The indexer behaves differently depending on whether the key already exists:

- If the key does not exist, a new key-value pair is added.
- If the key already exists, its value is replaced.

To retrieve a value using its key:

```csharp
Customer foundCustomer = customersById[2];

Console.WriteLine(foundCustomer.Name); // Leo
```

**Important:** If the specified key does not exist, accessing it with the indexer throws a `KeyNotFoundException`.

### 3.3 Safely retrieving a value: `TryGetValue`

Instead of accessing a key directly and risking an exception when it does not exist, use `TryGetValue`.

```csharp
if (customersById.TryGetValue(99, out Customer? customer))
{
    Console.WriteLine(customer.Name);
}
else
{
    Console.WriteLine("No customer with that ID was found.");
}
```

`TryGetValue`:

- Returns `true` if the key exists.
- Returns `false` if the key does not exist.
- If the key exists, assigns the associated value to the `out` variable.
- If the key does not exist, the `out` variable receives the default value for its type (which is `null` for `Customer`).

#### What does `out` mean?

`out` is a C# keyword used with method parameters. It allows a method to assign a value to a variable supplied by the caller.

In this example:

```csharp
customersById.TryGetValue(99, out Customer? customer);
```

The method uses `customer` as an output parameter. It assigns the found customer to that variable when the key exists.

This lets you check whether the key exists and retrieve its value in one operation.

### 3.4 Adding without replacing: `TryAdd`

If you use the dictionary indexer to add a key that already exists, the existing value is replaced.

If you want to add a key-value pair only when the key does not already exist, use `TryAdd`:

```csharp
bool added = customersById.TryAdd(
    1,
    new Customer { Id = 1, Name = "Maya Updated" }
);

Console.WriteLine(added);
```

- Returns `true` if the key-value pair was added.
- Returns `false` if the key already exists.
- Does not replace the existing value when the key is already present.

### 3.5 Iterating through a Dictionary

You can use `foreach` to iterate through the key-value pairs:

```csharp
foreach (KeyValuePair<int, Customer> pair in customersById)
{
    Console.WriteLine($"{pair.Key}: {pair.Value.Name}");
}
```

A `KeyValuePair<TKey, TValue>` represents one key-value pair in a dictionary.

- `pair.Key` gives you the key.
- `pair.Value` gives you the value.

You can also iterate through only the keys:

```csharp
foreach (int id in customersById.Keys)
{
    Console.WriteLine(id);
}
```

Or only the values:

```csharp
foreach (Customer customer in customersById.Values)
{
    Console.WriteLine(customer.Name);
}
```

**Important:** Do not rely on dictionary enumeration order. If your application needs a particular order, sort the data explicitly or use an ordered collection.

### 3.6 Sorting dictionary keys

You can copy the keys into a list and sort them:

```csharp
List<int> orderedIds = customersById.Keys.ToList();

orderedIds.Sort();

foreach (int id in orderedIds)
{
    Console.WriteLine(id);
}
```

This sorts the copied list of keys in ascending order. It does not change the dictionary itself.

`ToList()` is a LINQ extension method, so you need:

```csharp
using System.Linq;
```

In projects with implicit usings enabled, this namespace may already be imported.

## 4. How does a Dictionary work internally?

A dictionary uses a **hash table** to organize key-value pairs.

When you look up a key, the dictionary uses the key's hash code to help determine where its entry is stored.

A simplified explanation:

1. You provide a key.
2. The dictionary calculates the key's hash code.
3. It uses that information to locate the relevant entry.
4. It checks key equality to identify the correct key-value pair.

This makes dictionary lookups generally **O(1) on average**.

However, this is not a guarantee that every lookup takes exactly the same amount of time. Hash collisions can occur when different keys produce hash codes that lead to the same location. The dictionary handles collisions, but performance can be affected.

For comparison:

- Searching for a customer by ID in a list generally takes O(n) time.
- Looking up a customer by ID in a dictionary generally takes O(1) average time.

Here, `n` represents the number of elements.

## 5. Quick comparison

| Feature                     | Array                | List<T>                           | Dictionary<TKey, TValue> |
| --------------------------- | -------------------- | --------------------------------- | ------------------------ |
| Stores                      | Elements             | Elements                          | Key-value pairs          |
| Size                        | Fixed                | Can grow or shrink                | Can grow or shrink       |
| Access                      | Index                | Index                             | Key                      |
| Number of elements          | `Length`             | `Count`                           | `Count`                  |
| Typical lookup by index/key | O(1)                 | O(1) by index                     | O(1) average by key      |
| Useful for                  | Fixed-size sequences | General-purpose ordered sequences | Fast lookup by key       |

---

## Important reminders

- Arrays have a fixed length; lists can grow and shrink.
- `List<T>` and `Dictionary<TKey, TValue>` are generic collections.
- A list uses zero-based indexes.
- `Count` is a property on `List<T>` and `Dictionary<TKey, TValue>`.
- Dictionary keys must be unique.
- Dictionary indexer assignment adds a new key or replaces an existing value.
- `TryGetValue` safely checks for a key and retrieves its value.
- `TryAdd` adds an element only if its key does not already exist.
- Dictionary lookups are generally O(1) on average, not guaranteed O(1) in every case.
- Do not rely on dictionary enumeration order.
