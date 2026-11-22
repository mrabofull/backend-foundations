CREATE DATABASE Practice;

USE Practice;

CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(100) NOT NULL,
	City NVARCHAR(100)
);

-- can be done: PRIMARY KEY (Id) instead of putting PRIMARY KEY in the same line
-- if more than one column CONSTRAINT PK_Person PRIMARY KEY (Id, Name)


CREATE TABLE Orders
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	UserId INT FOREIGN KEY REFERENCES Users(Id),
	Amount INT,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
);

-- FOREIGN KEY (UserId) REFERENCES Users(Id)
-- DATETIMEOFFSET Date + time + UTC offset

INSERT INTO Users ([Name], City)
VALUES
    (N'Mohamed', N'Jatt'),
    (N'Ahmad', N'Baqa'),
    (N'Ali', N'Haifa'),
    (N'Omar', N'Nazareth'),
    (N'Yousef', N'Tel Aviv'),
    (N'Khaled', N'Jerusalem'),
    (N'Sara', N'Netanya'),
    (N'Noor', N'Akko');

INSERT INTO Orders (UserId, Amount)
VALUES
    (1, 150),
    (1, 200),
    (1, 75),
    (1, 320),

    (2, 500),
    (2, 120),
    (2, 90),

    (3, 250),
    (3, 180),
    (3, 400),
    (3, 60),

    (4, 100),
    (4, 350),

    (5, 700),
    (5, 220),
    (5, 130),

    (6, 450),
    (6, 80),
    (6, 290),
    (6, 160);

SELECT [Name], COALESCE(Sum(Amount),0) as OrdersAmount
From Users Left Join Orders 
    On Users.Id = Orders.UserId
Group By [Name]
Order By OrdersAmount DESC;

-- or ISNULL(Sum(Amount),0)
-- The COALESCE function returns the first of its arguments that is not null.