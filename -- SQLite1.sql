-- SQLite
CREATE TABLE users(
id INTEGER PRIMARY KEY,
email TEXT NOT NULL, 
password TEXT NOT NULL 
);

INSERT INTO users (id, email, password) VALUES (1, "user", "1234")

SELECT * FROM users



CREATE TABLE Orders (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    PaymentType TEXT NOT NULL,
    DeliveryType TEXT NOT NULL,
    Comments TEXT,
    FOREIGN KEY (UserId) REFERENCES users(Id)
);

CREATE TABLE OrderItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);


CREATE TABLE WishlistItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- Уникальный идентификатор
    UserId TEXT NOT NULL,                   -- Идентификатор пользователя
    ProductId INTEGER NOT NULL,             -- Идентификатор продукта
    ProductName TEXT NOT NULL,              -- Название продукта
    ImageUrl TEXT NOT NULL,                 -- URL изображения продукта
    UnitPrice DECIMAL(10, 2) NOT NULL,     -- Цена продукта (с двумя десятичными знаками)
    DateAdded DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Дата добавления
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id), -- Внешний ключ на таблицу пользователей
    FOREIGN KEY (ProductId) REFERENCES Products(Id)   -- Внешний ключ на таблицу продуктов
);



DROP TABLE WishlistItems;

CREATE TABLE CartItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT,
    ProductId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    DateAdded DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
