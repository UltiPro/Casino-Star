USE [CasinoStar]
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
    @token varchar(512)
)
AS
BEGIN
    SELECT * FROM Users WHERE Id = @id AND token = @token
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
    @id INT,
    @set BIT
)
AS
BEGIN
    UPDATE Users SET Active = @set WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateBanned
    (
    @id INT,
    @set BIT
)
AS
BEGIN
    UPDATE Users SET Banned = @set WHERE Id = @id
END
GO
CREATE PROCEDURE UpdateAdmin
    (
    @id INT,
    @set BIT
)
AS
BEGIN
    UPDATE Users SET Admin = @set WHERE Id = @id
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
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT *
    FROM Users
END