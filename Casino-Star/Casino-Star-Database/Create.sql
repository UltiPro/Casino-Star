CREATE DATABASE [CasinoStar]
GO
USE [CasinoStar]
GO
CREATE TABLE [Users]
(
    Id INT NOT NULL IDENTITY(1,1),
    Login VARCHAR(15) NOT NULL UNIQUE,
    Password VARCHAR(320) NOT NULL,
    Email VARCHAR(320) NOT NULL UNIQUE,
    Active BIT NOT NULL DEFAULT 0,
    Banned BIT NOT NULL DEFAULT 0,
    Admin BIT NOT NULL DEFAULT 0,
    Money INT NOT NULL DEFAULT 0,
    Token VARCHAR(512),
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
  (
    [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TABLE [Roulette]
(
    Id INT NOT NULL IDENTITY(1,1),
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    WinnerId INT NOT NULL,
    WinMoney INT NOT NULL,
    Decision VARCHAR(32) NOT NULL,
    WinDecision VARCHAR(32) NOT NULL,
    CONSTRAINT [PK_Roulette] PRIMARY KEY CLUSTERED
  (
    [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TABLE [CoinFlip]
(
    Id INT NOT NULL IDENTITY(1,1),
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    WinnerId INT NOT NULL,
    WinMoney INT NOT NULL,
    Decision VARCHAR(32) NOT NULL,
    DecisionCounter INT NOT NULL,
    CONSTRAINT [PK_Blackjack] PRIMARY KEY CLUSTERED
  (
    [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
ALTER TABLE [Roulette] WITH CHECK ADD CONSTRAINT [Roulette_fk0] FOREIGN KEY ([WinnerId]) REFERENCES [Users]([Id])
GO
ALTER TABLE [CoinFlip] WITH CHECK ADD CONSTRAINT [CoinFlip_fk0] FOREIGN KEY ([WinnerId]) REFERENCES [Users]([Id]);
GO
CREATE PROCEDURE InsertUser
    (
    @login VARCHAR(15),
    @password VARCHAR(320),
    @email VARCHAR(320)
)
AS
BEGIN
    INSERT INTO Users
        (Login,Password,Email)
    Values
        (@login, @password, @email)
END
GO
CREATE PROCEDURE Users_CheckEmail
    (
    @email VARCHAR(320)
)
AS
BEGIN
    SELECT Email
    FROM Users
    WHERE Email = @email
End
GO
CREATE PROCEDURE Users_CheckLogin
    (
    @login VARCHAR(15)
)
AS
BEGIN
    SELECT Login
    FROM Users
    WHERE Login = @login
END
GO
CREATE PROCEDURE GetUser
    (
    @login VARCHAR(15)
)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE Login = @login
END
GO
CREATE PROCEDURE VerifyUser
    (
    @id INT,
    @token VARCHAR(512)
)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE Id = @id AND token = @token
END
GO
CREATE PROCEDURE SetToken
    (
    @id INT,
    @token varchar(512)
)
AS
BEGIN
    UPDATE Users SET token = @token WHERE id = @id
END
GO
CREATE PROCEDURE RemoveUser
    (
    @id INT
)
AS
BEGIN
    DELETE FROM Users WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateActive
    (
    @id INT
)
AS
BEGIN
    UPDATE Users SET Active = IIF(Active = 1,0,1) WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateBanned
    (
    @id INT
)
AS
BEGIN
    UPDATE Users SET Banned = IIF(Banned = 1,0,1) WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateAdmin
    (
    @id INT
)
AS
BEGIN
    UPDATE Users SET Admin = IIF(Admin = 1,0,1) WHERE Id = @id
END
GO
CREATE PROCEDURE UpdatePassword
    (
    @id INT,
    @password VARCHAR(320)
)
AS
BEGIN
    UPDATE Users SET Password = @password WHERE Id = @id
END
GO
CREATE PROCEDURE GetPassword
    (
    @id INT
)
AS
BEGIN
    SELECT Password
    FROM Users
    WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateEmail
    (
    @id INT,
    @email VARCHAR(320)
)
AS
BEGIN
    UPDATE Users SET Email = @email WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateMoney
    (
    @id INT,
    @value INT
)
AS
BEGIN
    UPDATE Users SET Money += @value WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateMoneyNewSet
    (
    @id INT,
    @value INT
)
AS
BEGIN
    UPDATE Users SET Money = @value WHERE Id = @id
END
GO
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT *
    FROM Users
END
GO
CREATE PROCEDURE InsertRouletteHistory
    (
    @id INT,
    @money INT,
    @decision VARCHAR(32),
    @decisionWin VARCHAR(32)
)
AS
BEGIN
    INSERT INTO Roulette
        (WinnerId,WinMoney,Decision,WinDecision)
    Values
        (@id, @money, @decision, @decisionWin)
END
GO
CREATE PROCEDURE InsertCoinFlipHistory
    (
    @id INT,
    @money INT,
    @decision VARCHAR(32),
    @decisionCounter INT
)
AS
BEGIN
    INSERT INTO CoinFlip
        (WinnerId,WinMoney,Decision,DecisionCounter)
    Values
        (@id, @money, @decision, @decisionCounter)
END
GO
CREATE PROCEDURE GetCoinFlipHistory
    (
    @id INT,
    @count INT
)
AS
BEGIN
    SELECT TOP (@count)
        *
    FROM CoinFlip
    WHERE WinnerId = @id
    ORDER BY Id DESC
END
GO
CREATE PROCEDURE GetRouletteHistory
    (
    @id INT,
    @count INT
)
AS
BEGIN
    SELECT TOP (@count)
        *
    FROM Roulette
    WHERE WinnerId = @id
    ORDER BY Id DESC
END