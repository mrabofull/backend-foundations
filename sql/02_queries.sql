USE practice

-- 1: Users with no orders
	-- Solution 1:
SELECT [Name], COALESCE(Sum(Amount),0) as OrdersAmount
FROM users LEFT JOIN orders ON users.id = orders.userid
GROUP BY [Name]
HAVING SUM(Amount) IS NULL;

	-- Solution 2:
SELECT users.id, [name], city
FROM users LEFT JOIN orders ON users.id = orders.userid
WHERE orders.Id IS NULL

    -- Solution 3:
SELECT *
FROM users
WHERE NOT EXISTS (
	SELECT 1
	FROM orders
	WHERE orders.UserId = users.Id
);

-- 2: Top 3 users by total amount
SELECT TOP 3 [Name], COALESCE(SUM(amount),0) AS TotalAmount
FROM users LEFT JOIN orders ON users.id = orders.UserId
GROUP BY [Name]
ORDER BY [TotalAmount] DESC
-- LIMIT 3;
-- LIMIT is used in databases like MySQL and PostgreSQL, but SQL Server uses TOP instead
-- Database	                  Syntax
-- SQL Server	OFFSET 3 ROWS FETCH NEXT 3 ROWS ONLY
-- MySQL	    LIMIT 3 OFFSET 3
-- PostgreSQL	LIMIT 3 OFFSET 3

-- 3: Number of orders per city
SELECT city, COALESCE(COUNT(orders.id), 0) AS NumberOfOrders
FROM users LEFT JOIN orders ON users.id = orders.UserId
GROUP BY city
ORDER BY NumberOfOrders DESC;

-- 4: Average order amount per user, only for users with at least 2 orders (HAVING)
SELECT [name], COALESCE(AVG(orders.amount), 0) AS AverageOrdersAmount
FROM users LEFT JOIN orders ON users.id = orders.userId
GROUP BY [name]
HAVING SUM(orders.id) >= 2;
-- It can be done without COALESCE because we check if users have at least 2 orders

-- 5: Each user's single largest order
	-- Solution 1:
SELECT users.id AS UserId, [Name], COALESCE(MAX(Amount), 0) AS LargestOrder
FROM users LEFT JOIN orders ON users.id = orders.userid
GROUP BY users.id, [Name]
ORDER BY LargestOrder DESC;

	-- Solution 2: 
SELECT users.id AS UserId, [Name], MAX(amount) AS LargestOrder
FROM users LEFT JOIN orders ON users.id = orders.UserId
GROUP BY users.id, [Name]
HAVING COUNT(orders.id) > 0
ORDER BY LargestOrder DESC;

-- 6: Orders from the last 30 days
SELECT *
FROM orders 
where DATEDIFF(day, CreatedAt, GETUTCDATE()) BETWEEN 0 AND 30
-- GETUTCDATE() to get current utc date
-- SELECT DATEDIFF(year, '2017/08/25', '2011/08/25') AS DateDiff; Return the difference between two date values, in years:

-- 7: Users whose total is above the average user total (subquery)
SELECT *
FROM users
WHERE (SELECT SUM(amount)
	FROM orders 
	WHERE orders.UserId = users.Id) > (
	SELECT AVG(UserTotal)
	FROM (SELECT SUM(amount) AS UserTotal
		FROM orders
		GROUP BY UserId) AS UserTotals);

-- 8: Number of orders per month
SELECT YEAR(createdat) OrderYear, MONTH(createdat) AS OrderMonth, Count(*) AS NumberOfOrders
FROM orders
GROUP BY YEAR(createdat), MONTH(createdat)
ORDER BY OrderYear, OrderMonth;