USE master;
GO

-- Kreiranje baza podataka
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Culturio.Users')
BEGIN
    CREATE DATABASE [Culturio.Users];
END
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Culturio.CultureObjects')
BEGIN
    CREATE DATABASE [Culturio.CultureObjects];
END
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Culturio.Subscriptions')
BEGIN
    CREATE DATABASE [Culturio.Subscriptions];
END
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Culturio.Packages')
BEGIN
    CREATE DATABASE [Culturio.Packages];
END
GO