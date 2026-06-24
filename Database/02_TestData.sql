USE PerfumeStore;
GO

DELETE FROM SupplyRequestItems;
DELETE FROM SupplyRequests;
DELETE FROM Suppliers;
DELETE FROM Reviews;
DELETE FROM Favorites;
DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM CartItems;
DELETE FROM Cart;
DELETE FROM Products;
DELETE FROM Categories;
DELETE FROM Users;
GO


INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES
(N'Александр Администратор', 'admin@perfume.by', 'admin123', '+375291112233', 'Admin'),
(N'Мария Менеджер Заказов', 'manager@perfume.by', 'manager123', '+375292223344', 'OrderManager'),
(N'Константин Контент', 'content@perfume.by', 'content123', '+375293334455', 'ContentManager'),
(N'Иван Клиент', 'client123@mail.by', 'client123', '+375294445566', 'Client'),
(N'Елена Покупатель', 'client2@mail.by', 'client123', '+375295556677', 'Client');
GO


INSERT INTO Categories (Name, Description) VALUES
(N'Цветочные', N'Ароматы с доминирующими нотами роз, жасмина, пионов и других цветов.'),
(N'Цитрусовые', N'Освежающие композиции на основе бергамота, лимона, мандарина и грейпфрута.'),
(N'Древесные', N'Благородные шлейфы с нотами сандала, кедра, пачули и ветивера.'),
(N'Восточные', N'Пряные, чувственные и теплые ароматы с амброй, мускусом и ванилью.'),
(N'Унисекс', N'Композиции, идеально подходящие как для мужчин, так и для женщин.');
GO


INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive) VALUES
((SELECT Id FROM Categories WHERE Name = N'Цитрусовые'), 'Sauvage', 'Dior', 12500.00, 100, N'Свежий и мужественный аромат с нотами калабрийского бергамота.', 45, '/images/dior-sauvage.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Древесные'), 'Bleu de Chanel', 'Chanel', 14200.00, 50, N'Древесно-пряный аромат для мужчин с глубоким шлейфом кедра.', 30, '/images/bleu-chanel.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Восточные'), 'Noir', 'Tom Ford', 16800.00, 100, N'Пряный восточный аромат с нотами черного перца и ванили.', 15, '/images/tf-noir.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Восточные'), 'Eros', 'Versace', 8900.00, 100, N'Яркий мужской аромат с мятой, зеленым яблоком и бобами тонка.', 60, '/images/versace-eros.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Древесные'), 'Code', 'Armani', 9500.00, 75, N'Элегантный древесно-восточный парфюм с базой из кожи.', 20, '/images/armani-code.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Цветочные'), 'J''adore', 'Dior', 13400.00, 50, N'Легендарный женский цветочный букет: иланг-иланг и жасмин.', 25, '/images/dior-jadore.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Цветочные'), 'Light Blue', 'Dolce & Gabbana', 7800.00, 100, N'Жизнерадостный, свежий цветочно-фруктовый аромат.', 50, '/images/dg-lightblue.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Унисекс'), 'Lost Cherry', 'Tom Ford', 28000.00, 50, N'Насыщенный унисекс-аромат спелой вишни и горького миндаля.', 2, '/images/tf-cherry.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Унисекс'), 'Baccarat Rouge 540', 'Maison Francis Kurkdjian', 32000.00, 70, N'Изысканный древесно-амбровый аромат с нотами шафрана.', 12, '/images/mfk-baccarat.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Древесные'), 'Terre d''Hermes', 'Hermes', 10500.00, 100, N'Минеральный древесный аромат с горьким грейпфрутом и кедром.', 35, '/images/hermes-terre.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Цитрусовые'), 'Aqua Allegoria Mandarine Basilic', 'Guerlain', 8200.00, 75, N'Искрящийся и легкий летний аромат мандарина и базилика.', 40, '/images/guerlain-mandarine.jpg', 1),
((SELECT Id FROM Categories WHERE Name = N'Восточные'), 'Black Opium', 'Yves Saint Laurent', 11800.00, 50, N'Пряный гурманский аромат с нотами кофе. Скрыт от клиентов.', 0, '/images/ysl-opium.jpg', 0);
GO


INSERT INTO Cart (UserId) VALUES 
((SELECT Id FROM Users WHERE Email = 'client123@mail.by')),
((SELECT Id FROM Users WHERE Email = 'client2@mail.by'));

INSERT INTO CartItems (CartId, ProductId, Quantity) VALUES
((SELECT Id FROM Cart WHERE UserId = (SELECT Id FROM Users WHERE Email = 'client123@mail.by')), 
 (SELECT Id FROM Products WHERE Name = 'Sauvage' AND Brand = 'Dior'), 2),
((SELECT Id FROM Cart WHERE UserId = (SELECT Id FROM Users WHERE Email = 'client2@mail.by')), 
 (SELECT Id FROM Products WHERE Name = 'Lost Cherry' AND Brand = 'Tom Ford'), 1);
GO


DECLARE @OrderId1 INT;
DECLARE @OrderId2 INT;
DECLARE @OrderId3 INT;

INSERT INTO Orders (UserId, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status) VALUES
((SELECT Id FROM Users WHERE Email = 'client123@mail.by'), 14200.00, N'г. Минск, ул. Сурганова, д. 15, кв. 42', N'Картой онлайн', N'Курьер', 'New');

SET @OrderId1 = SCOPE_IDENTITY();

INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES
(@OrderId1, (SELECT Id FROM Products WHERE Name = 'Bleu de Chanel'), 1, 14200.00);


INSERT INTO Orders (UserId, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status) VALUES
((SELECT Id FROM Users WHERE Email = 'client2@mail.by'), 20200.00, N'г. Гродно, ул. Советская, д. 5, кв. 14', N'При получении', N'Самовывоз из Европочты', 'Assembling');

SET @OrderId2 = SCOPE_IDENTITY();

INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES
(@OrderId2, (SELECT Id FROM Products WHERE Name = 'Code'), 1, 9500.00),
(@OrderId2, (SELECT Id FROM Products WHERE Name = 'Terre d''Hermes'), 1, 10500.00);


INSERT INTO Orders (UserId, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status) VALUES
((SELECT Id FROM Users WHERE Email = 'client123@mail.by'), 8900.00, N'г. Минск, ул. Сурганова, д. 15, кв. 42', N'Картой онлайн', N'Курьер', 'Delivered');

SET @OrderId3 = SCOPE_IDENTITY();

INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES
(@OrderId3, (SELECT Id FROM Products WHERE Name = 'Eros'), 1, 8900.00);
GO

INSERT INTO Favorites (UserId, ProductId) VALUES
((SELECT Id FROM Users WHERE Email = 'client123@mail.by'), (SELECT Id FROM Products WHERE Name = 'Noir' AND Brand = 'Tom Ford')),
((SELECT Id FROM Users WHERE Email = 'client123@mail.by'), (SELECT Id FROM Products WHERE Name = 'Baccarat Rouge 540')),
((SELECT Id FROM Users WHERE Email = 'client2@mail.by'), (SELECT Id FROM Products WHERE Name = 'J''adore' AND Brand = 'Dior'));
GO


INSERT INTO Reviews (UserId, ProductId, Rating, Comment) VALUES
((SELECT Id FROM Users WHERE Email = 'client123@mail.by'), (SELECT Id FROM Products WHERE Name = 'Sauvage'), 5, N'Потрясающий стойкий аромат! Пользуюсь каждый день, шлейф невероятный'),
((SELECT Id FROM Users WHERE Email = 'client2@mail.by'), (SELECT Id FROM Products WHERE Name = 'Lost Cherry'), 4, N'Очень класный аромат вишневой косточки,дочери-подростку понравилось '),
((SELECT Id FROM Users WHERE Email = 'client123@mail.by'), (SELECT Id FROM Products WHERE Name = 'Eros'), 5, N'Классика от Версаче. Сладкий, как раз то что искала');
GO

INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'ООО Каприз-Парфюм', 'opt@caprice.by', '+375172223344');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'ИООО БелПарфюмТрейд', 'supply@perfumetrade.by', '+375173334455');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'ОДО ЭлитКосметик Групп', 'info@elite-cosmetic.by', '+375174445566');
GO

DECLARE @ManagerId INT = (SELECT Id FROM Users WHERE Email = 'manager@perfume.by');
DECLARE @RequestId1 INT;
DECLARE @RequestId2 INT;


INSERT INTO SupplyRequests (SupplierId, CreatedByUserId, Status, Comment) 
VALUES ((SELECT Id FROM Suppliers WHERE Name = N'ООО Каприз-Парфюм'), @ManagerId, 'Pending', N'Срочное пополнение запасов Dior и Chanel перед праздниками');

SET @RequestId1 = SCOPE_IDENTITY();

INSERT INTO SupplyRequestItems (SupplyRequestId, ProductId, Quantity) 
VALUES (@RequestId1, (SELECT Id FROM Products WHERE Name = 'Sauvage'), 20);

INSERT INTO SupplyRequestItems (SupplyRequestId, ProductId, Quantity) 
VALUES (@RequestId1, (SELECT Id FROM Products WHERE Name = 'Bleu de Chanel'), 15);


INSERT INTO SupplyRequests (SupplierId, CreatedByUserId, Status, Comment) 
VALUES ((SELECT Id FROM Suppliers WHERE Name = N'ИООО БелПарфюмТрейд'), @ManagerId, 'Completed', N'Плановая закупка селективного парфюма. Товар успешно принят на склад');

SET @RequestId2 = SCOPE_IDENTITY();

INSERT INTO SupplyRequestItems (SupplyRequestId, ProductId, Quantity) 
VALUES (@RequestId2, (SELECT Id FROM Products WHERE Name = 'Lost Cherry'), 10);

INSERT INTO SupplyRequestItems (SupplyRequestId, ProductId, Quantity) 
VALUES (@RequestId2, (SELECT Id FROM Products WHERE Name = 'Noir'), 5);
GO


