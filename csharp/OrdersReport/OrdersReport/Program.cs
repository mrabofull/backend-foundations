//Build a console app OrdersReport : the same users and orders as C# objects in lists.
//Use LINQ to reproduced 4 of Day 3 queries. Check that the answers match SQL.

//Practice with Users(Id, Name, City) and
//Orders(Id, UserId, Amount, CreatedAt)

using OrdersReport;

internal class Program
{
    static void Main(string[] args)
    {
        List<User> users = new List<User>
        {
            new User(1, "Mohamed", "Jatt"),
            new User(2, "Ahmad", "Baqa"),
            new User(3, "Ali", "Haifa"),
            new User(4, "Omar", "Nazareth"),
            new User(5, "Yousef", "Tel Aviv"),
            new User(6, "Khaled", "Jerusalem"),
            new User(7, "Sara", "Netanya"),
            new User(8, "Noor", "Akko")
        };

        List<Order> orders = new List<Order>()
        {
            new Order(1, 1, 150m),
            new Order(2, 1, 200m),
            new Order(3, 1, 75m),
            new Order(4, 1, 320m),
            new Order(5, 2, 500m),
            new Order(6, 2, 120m),
            new Order(7, 2, 90m),
            new Order(8, 3, 250m),
            new Order(9, 3, 180m),
            new Order(10, 3, 400m),
            new Order(11, 3, 60m),
            new Order(12, 4, 100m),
            new Order(13, 4, 350m),
            new Order(14, 5, 700m),
            new Order(15, 5, 220m),
            new Order(16, 5, 130m),
            new Order(17, 6, 450m),
            new Order(18, 6, 80m),
            new Order(19, 6, 290m),
            new Order(20, 6, 160m)
        };

        //1. Users with no orders

        // Solution 1
        var usersWithNoOrdersMethod1 = users.GroupJoin(
            orders,
            u => u.getId(),
            o => o.getUserId(),
            (u, userOrders) => new // Creates a result for each user and their matching orders.
            {
                UserId = u.getId(),
                UserName = u.getName(),
                Orders = userOrders,
                OrderCount = userOrders.Count() // if there is no orders an empty collection will be returned thats why the value will be 0 and not null
            })
            .Where(u => u.OrderCount == 0); // or we can do u => !u.Orders.Any()

        Console.WriteLine("Users with no orders solution 1:");
        foreach (var user in usersWithNoOrdersMethod1)
        {
            Console.WriteLine($"User Id: {user.UserId}, Name: {user.UserName}");
        }

        // Solution 2:
        var usersWithNoOrdersMethod2 = users.LeftJoin(
            orders,
            u => u.getId(),
            o => o.getUserId(),
            (u, o) => new
            {
                UserId = u.getId(),
                UserName = u.getName(),
                Amount = o?.getAmount() ?? 0m,
                // ?. checks whether o is null.
                // ?? 0m provides 0 if the result is null
                HasNoOrder = o == null
            }
            ).Where(u => u.HasNoOrder);

        Console.WriteLine("\nUsers with no orders solution 2:");

        foreach (var user in usersWithNoOrdersMethod2)
        {
            Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}");
        }

        // Solution 3 similar to NOT EXISTS subquery in SQL
        Console.WriteLine("\nUsers with no orders solution 3:");
        var usersWithNoOrdersMethod3 = users.Where(u => !orders.Any(o => o.getUserId() == u.getId()));

        foreach (var user in usersWithNoOrdersMethod3)
        {
            Console.WriteLine(user);
        }

        //2. Top 3 users by total amount
        Console.WriteLine("\nTop 3 users by total amount:");
        var topUsersByTotalAmount = users.GroupJoin(
            orders,
            u => u.getId(),
            o => o.getUserId(),
            (u, userOrders) => new
            {
                UserId = u.getId(),
                UserName = u.getName(),
                TotalAmount = userOrders.Sum(o => o.getAmount()) // LINQ's Sum() on an empty collection of decimal values returns 0m
            }
            )
            .OrderByDescending(u => u.TotalAmount)
            .Take(3);
            
        foreach (var user in topUsersByTotalAmount) { Console.WriteLine(user); }

        //3. Number of orders per city
        Console.WriteLine("\nNumber of orders per city:");

        var numOfOrdersPerCity = users.Join(
            orders,
            u => u.getId(),
            o => o.getUserId(),
            (u, o) => new // result selector
            {
                City = u.getCity(),
                OrderId = o.getId()
            }
            )
            .GroupBy(x => x.City)
            .Select(g => new
            {
                City = g.Key, // gives the city name
                NumberOfOrders = g.Count()
            })
            .OrderByDescending(x => x.NumberOfOrders);


        foreach (var user in numOfOrdersPerCity) { Console.WriteLine(user); }

        //4. Average order amount per user, only for users with at least 2 orders
        Console.WriteLine("\nAverage order amount per user, only for users with at least 2 orders:");
        var avgOrderAmountPerUser = users.GroupJoin( // Group Join Stores and Products (Left Join + group by)
            orders,
            u => u.getId(), // from every User take the primary key
            o => o.getUserId(), // from every Order take the foreign key
            (u, o) => new
            {
                UserId = u.getId(),
                UserName = u.getName(),
                OrderCount = o.Count(),
                AverageAmount = o.DefaultIfEmpty().Average(o => o?.getAmount() ?? 0m)
                // Unlike Sum(), Average() cannot return 0 because there is no value to calculate an average from
                // DefaultIfEmpty() checks whether a collection is empty.
                // If the collection has elements → returns the original elements.
                // If the collection is empty → returns a collection containing one default value.
                // For a user without orders, it inserts one default Order value, which is null because Order is a reference type.
                // List<User> null, List<string> null, List<bool> false, List<double> 0.0, List<decimal> 0m, List<int> 0
                // o?.getAmount() ?? 0m, Returns 0m for that null order.
                // also I can use ternary operator (shorted if else) instead of o?.getAmount() ?? 0m and write it like: o != null ? o.getAmount() : 0m
                // ?.	Null-conditional: safely accesses a member if the object isn't null
                // ??	Null-coalescing: provides a fallback if the result is null
                // ? :	Conditional (ternary): chooses between two values based on a condition
            })
            .Where(a => a.OrderCount >= 2)
            .OrderByDescending(or => or.AverageAmount);

        foreach (var user in avgOrderAmountPerUser) { Console.WriteLine(user); }
    }
}