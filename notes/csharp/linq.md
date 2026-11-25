# LINQ (Language Integrated Query)

## 1. What is LINQ?

**LINQ** stands for **Language Integrated Query**. It is a set of C# features that lets you query data using a consistent syntax.

You can use LINQ to:

- Filter data.
- Select or transform data.
- Sort data.
- Group data.
- Calculate values such as totals, averages, minimums, and maximums.
- Check whether elements match a condition.

LINQ can work with different data sources, including in-memory collections, databases, and XML. The available operations and how they are executed depend on the data source and its LINQ provider.

A **LINQ provider** is the component that connects LINQ queries to a particular data source. For example, Entity Framework Core provides a LINQ provider that translates supported LINQ expressions into database queries.

## 2. The examples used in these notes

The examples below use a collection of games. Assume this class and collection are available:

```csharp
public class Game
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public decimal Price { get; set; }
    public double Rating { get; set; }
}

List<Game> games = new List<Game>
{
    new Game { Title = "Skybound", Genre = "Adventure", ReleaseYear = 2021, Price = 59.99m, Rating = 8.7 },
    new Game { Title = "Iron Kingdom", Genre = "RPG", ReleaseYear = 2019, Price = 79.99m, Rating = 9.1 },
    new Game { Title = "Deep Space", Genre = "Adventure", ReleaseYear = 2023, Price = 49.99m, Rating = 8.4 },
    new Game { Title = "Pixel Racer", Genre = "Racing", ReleaseYear = 2018, Price = 39.99m, Rating = 7.8 },
    new Game { Title = "Mystic Lands", Genre = "RPG", ReleaseYear = 2022, Price = 69.99m, Rating = 9.3 }
};
```

The `m` suffix indicates that a numeric literal is a `decimal`, which matches the type of the `Price` property.

## 3. Lambda expressions and LINQ

A **lambda expression** is a concise way to write an anonymous function (a function without a declared name).

LINQ method syntax commonly uses lambda expressions to describe what should happen to each element in a collection.

Example:

```csharp
var adventureGames = games.Where(game => game.Genre == "Adventure");
```

In this expression:

- `game` is the parameter representing the current element being examined.
- `=>` is the lambda operator.
- `game.Genre == "Adventure"` is the condition.
- `Where` keeps the elements for which the condition is `true`.

The parameter name is up to you. These expressions do the same thing:

```csharp
games.Where(game => game.Genre == "Adventure");
games.Where(g => g.Genre == "Adventure");
```

`g` is simply a shorter parameter name; it has no special meaning.

Another example:

```csharp
var gameTitles = games.Select(game => game.Title);
```

Here, the lambda expression tells `Select` to return the `Title` property from each game.

**Important:** Lambda expressions are not part of LINQ itself. They are a C# language feature that is frequently used with LINQ, especially with method syntax.

## 4. LINQ syntax styles

There are two common ways to write LINQ queries:

- **Method syntax** (also called fluent syntax).
- **Query syntax** (which resembles SQL).

### 4.1 Method syntax

Method syntax uses methods such as `Where`, `Select`, and `OrderBy`. It commonly uses lambda expressions.

```csharp
var adventureGames = games
    .Where(game => game.Genre == "Adventure")
    .OrderBy(game => game.Title);

foreach (var game in adventureGames)
{
    Console.WriteLine(game.Title);
}
```

### 4.2 Query syntax

Query syntax uses keywords such as `from`, `where`, `orderby`, and `select`.

```csharp
var adventureGames =
    from game in games
    where game.Genre == "Adventure"
    orderby game.Title
    select game;

foreach (var game in adventureGames)
{
    Console.WriteLine(game.Title);
}
```

Both examples produce the same result.

**Note:** Query syntax is translated by the compiler into method calls. Not every LINQ operation has a direct query-syntax keyword, so method syntax is often used for operations such as `Count`, `Any`, `Sum`, and `Average`.

## 5. Filtering elements: `Where`

`Where` filters a sequence and returns the elements that satisfy a condition.

```csharp
var rpgGames = games.Where(game => game.Genre == "RPG");

foreach (var game in rpgGames)
{
    Console.WriteLine(game.Title);
}
```

You can also combine conditions:

```csharp
var affordableAdventureGames = games.Where(game =>
    game.Genre == "Adventure" && game.Price <= 60m);
```

- `&&` means AND: both conditions must be true.
- `||` means OR: at least one condition must be true.

## 6. Selecting or transforming elements: `Select`

`Select` transforms each element into a new value. It does not have to return the original object.

For example, to get only the game titles:

```csharp
IEnumerable<string> gameTitles = games.Select(game => game.Title);

foreach (string title in gameTitles)
{
    Console.WriteLine(title);
}
```

You can also create a formatted string for each game:

```csharp
var gameDescriptions = games.Select(game =>
    $"{game.Title} - {game.Genre} - {game.Price:C}");
```

### `Select` versus `Where`

- `Where` decides **which elements to keep**.
- `Select` decides **what value to produce from each element**.

For example:

```csharp
var adventureGameTitles = games
    .Where(game => game.Genre == "Adventure")
    .Select(game => game.Title);
```

This first filters the games to Adventure games, then selects their titles.

### `IEnumerable<T>` and `ToList()`

When you call `Select` on an `IEnumerable<T>` source, the result is generally an `IEnumerable<TResult>`.

For example:

```csharp
IEnumerable<string> titles = games.Select(game => game.Title);
```

If you specifically need a `List<string>`, use `ToList()`:

```csharp
List<string> titles = games
    .Select(game => game.Title)
    .ToList();
```

`ToList()` executes the query and stores its results in a new list.

## 7. Counting elements: `Count`

`Count()` returns the number of elements in a sequence.

```csharp
int totalGames = games.Count();

Console.WriteLine($"Total games: {totalGames}");
```

You can also provide a condition to count only matching elements:

```csharp
int adventureGameCount = games.Count(game => game.Genre == "Adventure");

Console.WriteLine($"Adventure games: {adventureGameCount}");
```

## 8. Checking conditions: `Any`

`Any()` returns a `bool` (`true` or `false`).

Without a condition, it checks whether the sequence contains at least one element:

```csharp
bool hasGames = games.Any();
```

With a condition, it checks whether at least one element satisfies that condition:

```csharp
bool hasRecentGames = games.Any(game => game.ReleaseYear > 2020);

Console.WriteLine($"Are there games released after 2020? {hasRecentGames}");
```

## 9. Sorting: `OrderBy` and `OrderByDescending`

`OrderBy` sorts elements in ascending order.

```csharp
var gamesByYear = games.OrderBy(game => game.ReleaseYear);

foreach (var game in gamesByYear)
{
    Console.WriteLine($"{game.Title} - {game.ReleaseYear}");
}
```

`OrderByDescending` sorts elements in descending order.

```csharp
var gamesByRating = games.OrderByDescending(game => game.Rating);

foreach (var game in gamesByRating)
{
    Console.WriteLine($"{game.Title} - {game.Rating}");
}
```

You can sort by more than one property using `ThenBy` and `ThenByDescending`:

```csharp
var sortedGames = games
    .OrderBy(game => game.Genre)
    .ThenByDescending(game => game.Rating);
```

This sorts by genre first. Games with the same genre are then sorted by rating from highest to lowest.

## 10. Aggregation: `Sum`, `Average`, `Min`, and `Max`

**Aggregation** means combining values from a sequence to produce a single result.

Common aggregation methods include:

### `Sum`

Calculates the total of numeric values.

```csharp
decimal totalPrice = games.Sum(game => game.Price);

Console.WriteLine($"Total price: {totalPrice}");
```

The result type depends on the numeric type being summed. For example:

- Summing `int` values returns an `int`.
- Summing `double` values returns a `double`.
- Summing `decimal` values returns a `decimal`.

Since `Price` is a `decimal`, `Sum(game => game.Price)` returns a `decimal`.

### `Average`

Calculates the average of numeric values.

```csharp
double averageRating = games.Average(game => game.Rating);

Console.WriteLine($"Average rating: {averageRating}");
```

The return type depends on the overload used. For example, averaging `int` values returns a `double`, while averaging `decimal` values returns a `decimal`.

### `Min`

Returns the minimum value.

```csharp
decimal lowestPrice = games.Min(game => game.Price);

Console.WriteLine($"Lowest price: {lowestPrice}");
```

### `Max`

Returns the maximum value.

```csharp
double highestRating = games.Max(game => game.Rating);

Console.WriteLine($"Highest rating: {highestRating}");
```

You can combine `Max` with `First` to find the game with the highest rating:

```csharp
double highestRating = games.Max(game => game.Rating);

Game bestGame = games.First(game => game.Rating == highestRating);

Console.WriteLine($"Best game: {bestGame.Title} - {bestGame.Rating}");
```

If multiple games share the highest rating, `First` returns the first matching game in the sequence.

## 11. Grouping: `GroupBy`

`GroupBy` groups elements according to a key.

For example, group games by genre:

```csharp
var gamesByGenre = games.GroupBy(game => game.Genre);

foreach (var group in gamesByGenre)
{
    Console.WriteLine($"Genre: {group.Key}");

    foreach (var game in group)
    {
        Console.WriteLine(game.Title);
    }
}
```

- `group.Key` contains the value used to create the group (in this example, the genre).
- Each `group` contains the games that belong to that genre.

## 12. Removing duplicates: `Distinct`

`Distinct()` returns distinct elements, removing duplicates according to the equality rules of the element type.

To get the unique genres:

```csharp
var uniqueGenres = games
    .Select(game => game.Genre)
    .Distinct();

foreach (string genre in uniqueGenres)
{
    Console.WriteLine(genre);
}
```

Here, `Select` gets the genre from each game, and `Distinct` removes duplicate genre strings.

## 13. Pagination: `Skip` and `Take`

`Skip(n)` skips the first `n` elements.

`Take(n)` takes up to the next `n` elements.

These methods are often combined to implement pagination.

For example, if each page contains two games:

```csharp
int pageSize = 2;
int pageNumber = 2; // Page numbers start at 1.

var pageGames = games
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize);

foreach (var game in pageGames)
{
    Console.WriteLine(game.Title);
}
```

For page 2:

- `Skip(2)` skips the first two games.
- `Take(2)` returns the next two games.

In SQL terms, this is conceptually similar to using `OFFSET` and `FETCH` (or other database-specific pagination syntax).

## 14. Finding elements: `First` and `FirstOrDefault`

`First()` returns the first element in a sequence, or the first element that matches a condition.

```csharp
Game firstGame = games.First();

Game firstRpgGame = games.First(game => game.Genre == "RPG");
```

If the sequence is empty, or no element matches the condition, `First()` throws an `InvalidOperationException`.

`FirstOrDefault()` behaves similarly, but returns the default value if no element is found. For a reference type such as `Game`, that default value is `null`.

```csharp
Game? strategyGame = games.FirstOrDefault(game => game.Genre == "Strategy");

if (strategyGame is not null)
{
    Console.WriteLine(strategyGame.Title);
}
else
{
    Console.WriteLine("No strategy game was found.");
}
```

Use `First` when an element is expected to exist. Use `FirstOrDefault` when no match is a normal possibility and you want to handle that case.

## 15. Combining LINQ methods

You can chain multiple LINQ methods to build a query. Each method performs one part of the operation.

```csharp
var affordableAdventureGames = games
    .Where(game => game.Genre == "Adventure" && game.Price <= 60m)
    .OrderByDescending(game => game.Rating)
    .Select(game => $"{game.Title} - {game.Price:C} - Rating: {game.Rating}");

foreach (string description in affordableAdventureGames)
{
    Console.WriteLine(description);
}
```

The query:

1. Filters for Adventure games that cost 60 or less.
2. Sorts the matching games by rating, from highest to lowest.
3. Converts each matching game into a formatted string.

## 16. The `let` keyword in query syntax

The `let` keyword creates a named intermediate value inside a query expression. This can make a query easier to read when you need to reuse or refer to a calculated value.

```csharp
var gameLabels =
    from game in games
    let priceCategory = game.Price < 50m ? "Budget" : "Regular"
    where game.Rating >= 8.5
    orderby game.Price
    select $"{game.Title}: {priceCategory}";

foreach (string label in gameLabels)
{
    Console.WriteLine(label);
}
```

Here, `priceCategory` is calculated for each game and then used in the rest of the query.

## 17. Deferred execution

Many LINQ queries that return `IEnumerable<T>` use **deferred execution**. This means the query is not necessarily executed when it is created. It is executed when the results are enumerated, for example with `foreach`, or when a method such as `ToList()` forces the results to be produced.

```csharp
var affordableGames = games.Where(game => game.Price < 60m);

// The query is created above. It has not necessarily been executed yet.

foreach (var game in affordableGames)
{
    Console.WriteLine(game.Title);
}
```

Each time a deferred query is enumerated, it may run again against the source sequence.

By contrast, `ToList()` materializes the results at that point:

```csharp
List<Game> affordableGamesList = games
    .Where(game => game.Price < 60m)
    .ToList();
```

**Important:** Not every LINQ method is deferred. Methods such as `Count`, `Any`, `Sum`, `Average`, `First`, and `ToList` execute the query to produce a result.

## 18. Quick reference

| Method                | Purpose                                      | Typical return value              |
| --------------------- | -------------------------------------------- | --------------------------------- |
| `Where()`             | Filters elements by a condition              | `IEnumerable<T>`                  |
| `Select()`            | Transforms each element                      | `IEnumerable<TResult>`            |
| `OrderBy()`           | Sorts in ascending order                     | `IOrderedEnumerable<T>`           |
| `OrderByDescending()` | Sorts in descending order                    | `IOrderedEnumerable<T>`           |
| `ThenBy()`            | Adds an ascending secondary sort             | `IOrderedEnumerable<T>`           |
| `GroupBy()`           | Groups elements by a key                     | `IEnumerable<IGrouping<TKey, T>>` |
| `Count()`             | Counts elements                              | `int`                             |
| `Any()`               | Checks whether any element exists or matches | `bool`                            |
| `Sum()`               | Calculates a numeric total                   | Depends on numeric type           |
| `Average()`           | Calculates a numeric average                 | Depends on overload               |
| `Min()` / `Max()`     | Finds a minimum or maximum                   | Depends on selected type          |
| `Distinct()`          | Removes duplicate values                     | `IEnumerable<T>`                  |
| `Skip()`              | Skips a number of elements                   | `IEnumerable<T>`                  |
| `Take()`              | Takes up to a number of elements             | `IEnumerable<T>`                  |
| `First()`             | Gets the first element or match              | `T`                               |
| `FirstOrDefault()`    | Gets the first element or default value      | `T` or its default                |

---

## Important reminders

- LINQ stands for **Language Integrated Query**.
- Method syntax and query syntax are two ways to write LINQ queries.
- Lambda expressions are C# language features commonly used with LINQ.
- `Where` filters; `Select` transforms.
- `IEnumerable<T>` represents a sequence that can be enumerated.
- `ToList()` executes and materializes a query into a list.
- Many LINQ queries use deferred execution.
- Aggregation methods such as `Sum`, `Average`, `Min`, and `Max` produce summary values.
