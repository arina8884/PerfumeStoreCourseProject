USE PerfumeStore;
GO

-- PerfumeStore: реалистичные данные для демонстрации и защиты проекта
-- Запускать ПОСЛЕ Database/01_CreateDatabase.sql

INSERT INTO Categories (Name, Description) VALUES (N'Женские', N'Парфюмерия для женщин — от лёгких цветочных до насыщенных восточных композиций');
INSERT INTO Categories (Name, Description) VALUES (N'Мужские', N'Мужская парфюмерия: свежие, древесные, пряные и фужерные ароматы');
INSERT INTO Categories (Name, Description) VALUES (N'Унисекс', N'Универсальные композиции, которые подходят любому стилю и настроению');
INSERT INTO Categories (Name, Description) VALUES (N'Нишевая парфюмерия', N'Авторские и редкие ароматы от Creed, Byredo, Xerjoff и других нишевых домов');
INSERT INTO Categories (Name, Description) VALUES (N'Цветочные', N'Ароматы с доминирующими нотами розы, жасмина, пиона и фрезии');
INSERT INTO Categories (Name, Description) VALUES (N'Восточные', N'Пряные, чувственные ароматы с амброй, мускусом и ванилью');
INSERT INTO Categories (Name, Description) VALUES (N'Древесные', N'Благородные шлейфы с сандалом, кедром, пачули и ветивером');
INSERT INTO Categories (Name, Description) VALUES (N'Цитрусовые', N'Освежающие композиции на основе бергамота, лимона и мандарина');
INSERT INTO Categories (Name, Description) VALUES (N'Фужерные', N'Классические композиции с лавандой, дубовым мхом и благородными древесными аккордами');
INSERT INTO Categories (Name, Description) VALUES (N'Свежие', N'Лёгкие водные и зелёные ароматы для ежедневного использования');
INSERT INTO Categories (Name, Description) VALUES (N'Пряные', N'Ароматы с акцентом на специи, перец, кардамон и тёплые пряности');
INSERT INTO Categories (Name, Description) VALUES (N'Гурманские', N'Сладкие и уютные ароматы с нотами ванили, карамели и какао');
GO

INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Администратор Системы', N'admin@perfume.by', N'admin123', N'+375291000001', N'Admin');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Контент Менеджер', N'content@perfume.by', N'content123', N'+375291000002', N'ContentManager');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Менеджер Заказов', N'manager@perfume.by', N'manager123', N'+375291000003', N'OrderManager');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Анна Коваль', N'anna.koval@mail.by', N'client123', N'+375291112233', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Дмитрий Сидоров', N'dmitry.sidorov@gmail.com', N'client123', N'+375292223344', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Елена Мороз', N'elena.moroz@mail.by', N'client123', N'+375293334455', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Игорь Петров', N'igor.petrov@yandex.by', N'client123', N'+375294445566', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Мария Войтович', N'maria.voytovich@mail.by', N'client123', N'+375295556677', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Ольга Лебедева', N'olga.lebedeva@gmail.com', N'client123', N'+375296667788', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Сергей Новик', N'sergey.novik@mail.by', N'client123', N'+375297778899', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Наталья Жукова', N'natalya.zhukova@mail.by', N'client123', N'+375298889900', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Павел Кравец', N'pavel.kravets@gmail.com', N'client123', N'+375299990011', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Татьяна Сокол', N'tatiana.sokol@yandex.by', N'client123', N'+375291101212', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Андрей Волков', N'andrey.volkov@mail.by', N'client123', N'+375292202323', N'Client');
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES (N'Ксения Орлова', N'ksenia.orlova@gmail.com', N'client123', N'+375293303434', N'Client');
GO

INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Luxe Fragrance Distribution', N'orders@luxefrag.by', N'+375 (17) 300-01-01');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Parfum Elite Belarus', N'supply@parfumelite.by', N'+375 (17) 300-01-02');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'EuroScent Logistics', N'logistics@euroscent.eu', N'+48 22 123 45 67');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Minsk Beauty Supply', N'info@minskbeauty.by', N'+375 (17) 300-01-03');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Chanel Group Belarus', N'b2b@chanel.by', N'+375 (17) 300-01-04');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Dior Prestige Supply', N'wholesale@dior.by', N'+375 (17) 300-01-05');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Niche Aromas Import', N'import@nichearomas.by', N'+375 (17) 300-01-06');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Global Perfume Hub', N'hub@globalperfume.com', N'+33 1 45 67 89 00');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Baltic Fragrance Trade', N'trade@balticfrag.lt', N'+370 6 123 45 67');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Premium Scent Warehouse', N'warehouse@premiumscent.by', N'+375 (17) 300-01-07');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Guerlain Official BY', N'partners@guerlain.by', N'+375 (17) 320-44-10');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Kilian Paris Belarus', N'orders@kilian.by', N'+375 (17) 320-55-20');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Jo Malone London BY', N'b2b@jomalone.by', N'+375 (17) 320-66-30');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Prada Beauty Supply', N'wholesale@prada-beauty.by', N'+375 (17) 320-77-40');
INSERT INTO Suppliers (Name, Email, Phone) VALUES (N'Versace Fragrance Minsk', N'sales@versace-frag.by', N'+375 (17) 320-88-50');
GO

INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Sauvage Eau de Toilette', N'Dior', 129, 60, N'Свежий и пряный мужской аромат с нотами бергамота и амброксана.', 53, N'/images/products/dior-sauvage-eau-de-toilette-60.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Sauvage Eau de Toilette', N'Dior', 169, 100, N'Свежий и пряный мужской аромат с нотами бергамота и амброксана.', 53, N'/images/products/dior-sauvage-eau-de-toilette-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Sauvage Eau de Toilette', N'Dior', 199, 150, N'Свежий и пряный мужской аромат с нотами бергамота и амброксана.', 57, N'/images/products/dior-sauvage-eau-de-toilette-150.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Sauvage Eau de Parfum', N'Dior', 129, 60, N'Более насыщенная версия культового Sauvage с глубоким шлейфом.', 48, N'/images/products/dior-sauvage-eau-de-parfum-60.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Sauvage Eau de Parfum', N'Dior', 159, 100, N'Более насыщенная версия культового Sauvage с глубоким шлейфом.', 13, N'/images/products/dior-sauvage-eau-de-parfum-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'J''adore Eau de Parfum', N'Dior', 159, 50, N'Изысканный цветочный букет иланг-иланга, дамасской розы и жасмина.', 40, N'/images/products/dior-jadore-eau-de-parfum-50.webp', 1 FROM Categories c WHERE c.Name = N'Цветочные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'J''adore Eau de Parfum', N'Dior', 229, 100, N'Изысканный цветочный букет иланг-иланга, дамасской розы и жасмина.', 22, N'/images/products/dior-jadore-eau-de-parfum-100.webp', 1 FROM Categories c WHERE c.Name = N'Цветочные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Miss Dior Eau de Parfum', N'Dior', 159, 50, N'Романтичный аромат с нотами розы, пиона и ириса.', 35, N'/images/products/dior-miss-dior-eau-de-parfum-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Miss Dior Eau de Parfum', N'Dior', 229, 100, N'Романтичный аромат с нотами розы, пиона и ириса.', 21, N'/images/products/dior-miss-dior-eau-de-parfum-100.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Fahrenheit', N'Dior', 99, 50, N'Легендарный мужской аромат с нотами кожи, фиалки и мускуса.', 45, N'/images/products/dior-fahrenheit-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Fahrenheit', N'Dior', 129, 100, N'Легендарный мужской аромат с нотами кожи, фиалки и мускуса.', 51, N'/images/products/dior-fahrenheit-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'N°5 Eau de Parfum', N'Chanel', 199, 35, N'Культовый цветочный альдегидный аромат, символ элегантности.', 35, N'/images/products/chanel-n-5-eau-de-parfum-35.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'N°5 Eau de Parfum', N'Chanel', 219, 50, N'Культовый цветочный альдегидный аромат, символ элегантности.', 34, N'/images/products/chanel-n-5-eau-de-parfum-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'N°5 Eau de Parfum', N'Chanel', 249, 100, N'Культовый цветочный альдегидный аромат, символ элегантности.', 44, N'/images/products/chanel-n-5-eau-de-parfum-100.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Coco Mademoiselle', N'Chanel', 159, 50, N'Современный восточно-цветочный аромат с апельсином и жасмином.', 51, N'/images/products/chanel-coco-mademoiselle-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Coco Mademoiselle', N'Chanel', 199, 100, N'Современный восточно-цветочный аромат с апельсином и жасмином.', 43, N'/images/products/chanel-coco-mademoiselle-100.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bleu de Chanel Eau de Parfum', N'Chanel', 159, 50, N'Ароматический древесный шедевр с цитрусом и ладаном.', 25, N'/images/products/chanel-bleu-de-chanel-eau-de-parfum-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bleu de Chanel Eau de Parfum', N'Chanel', 199, 100, N'Ароматический древесный шедевр с цитрусом и ладаном.', 35, N'/images/products/chanel-bleu-de-chanel-eau-de-parfum-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bleu de Chanel Eau de Parfum', N'Chanel', 249, 150, N'Ароматический древесный шедевр с цитрусом и ладаном.', 27, N'/images/products/chanel-bleu-de-chanel-eau-de-parfum-150.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Chance Eau Tendre', N'Chanel', 129, 50, N'Нежный цветочно-фруктовый аромат с грейпфрутом и жасмином.', 10, N'/images/products/chanel-chance-eau-tendre-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Chance Eau Tendre', N'Chanel', 159, 100, N'Нежный цветочно-фруктовый аромат с грейпфрутом и жасмином.', 24, N'/images/products/chanel-chance-eau-tendre-100.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Black Opium', N'Yves Saint Laurent', 129, 30, N'Соблазнительный кофейно-ванильный аромат для вечера.', 26, N'/images/products/yves-saint-laurent-black-opium-30.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Black Opium', N'Yves Saint Laurent', 169, 50, N'Соблазнительный кофейно-ванильный аромат для вечера.', 48, N'/images/products/yves-saint-laurent-black-opium-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Black Opium', N'Yves Saint Laurent', 219, 90, N'Соблазнительный кофейно-ванильный аромат для вечера.', 41, N'/images/products/yves-saint-laurent-black-opium-90.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Libre Eau de Parfum', N'Yves Saint Laurent', 129, 50, N'Фужерно-цветочный аромат с лавандой и апельсиновым цветом.', 43, N'/images/products/yves-saint-laurent-libre-eau-de-parfum-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Libre Eau de Parfum', N'Yves Saint Laurent', 159, 90, N'Фужерно-цветочный аромат с лавандой и апельсиновым цветом.', 31, N'/images/products/yves-saint-laurent-libre-eau-de-parfum-90.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'La Nuit de L''Homme', N'Yves Saint Laurent', 129, 60, N'Пряный мужской аромат с кардамоном и кедром.', 49, N'/images/products/yves-saint-laurent-la-nuit-de-lhomme-60.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'La Nuit de L''Homme', N'Yves Saint Laurent', 129, 100, N'Пряный мужской аромат с кардамоном и кедром.', 23, N'/images/products/yves-saint-laurent-la-nuit-de-lhomme-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Y Eau de Parfum', N'Yves Saint Laurent', 129, 60, N'Свежий ароматический фужер с яблоком и шалфеем.', 57, N'/images/products/yves-saint-laurent-y-eau-de-parfum-60.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Y Eau de Parfum', N'Yves Saint Laurent', 159, 100, N'Свежий ароматический фужер с яблоком и шалфеем.', 34, N'/images/products/yves-saint-laurent-y-eau-de-parfum-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Acqua di Gio Profondo', N'Giorgio Armani', 129, 50, N'Морской ароматический фужер с нотами морских водорослей.', 21, N'/images/products/giorgio-armani-acqua-di-gio-profondo-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Acqua di Gio Profondo', N'Giorgio Armani', 179, 100, N'Морской ароматический фужер с нотами морских водорослей.', 51, N'/images/products/giorgio-armani-acqua-di-gio-profondo-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Acqua di Gio Profondo', N'Giorgio Armani', 199, 125, N'Морской ароматический фужер с нотами морских водорослей.', 27, N'/images/products/giorgio-armani-acqua-di-gio-profondo-125.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Armani Code Eau de Parfum', N'Giorgio Armani', 99, 50, N'Элегантный восточный аромат с тонкой бобовой ванилью.', 45, N'/images/products/giorgio-armani-armani-code-eau-de-parfum-50.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Armani Code Eau de Parfum', N'Giorgio Armani', 129, 75, N'Элегантный восточный аромат с тонкой бобовой ванилью.', 51, N'/images/products/giorgio-armani-armani-code-eau-de-parfum-75.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Armani Code Eau de Parfum', N'Giorgio Armani', 169, 125, N'Элегантный восточный аромат с тонкой бобовой ванилью.', 38, N'/images/products/giorgio-armani-armani-code-eau-de-parfum-125.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Stronger With You', N'Giorgio Armani', 99, 50, N'Тёплый гурманский аромат с каштаном и ванилью.', 16, N'/images/products/giorgio-armani-stronger-with-you-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Stronger With You', N'Giorgio Armani', 129, 100, N'Тёплый гурманский аромат с каштаном и ванилью.', 45, N'/images/products/giorgio-armani-stronger-with-you-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Si Passione', N'Giorgio Armani', 129, 50, N'Страстный цветочный аромат с нотами черной смородины и розы.', 10, N'/images/products/giorgio-armani-si-passione-50.webp', 1 FROM Categories c WHERE c.Name = N'Цветочные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Si Passione', N'Giorgio Armani', 179, 100, N'Страстный цветочный аромат с нотами черной смородины и розы.', 50, N'/images/products/giorgio-armani-si-passione-100.webp', 1 FROM Categories c WHERE c.Name = N'Цветочные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Eros Eau de Toilette', N'Versace', 99, 50, N'Энергичный мятно-ванильный аромат для уверенного образа.', 18, N'/images/products/versace-eros-eau-de-toilette-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Eros Eau de Toilette', N'Versace', 139, 100, N'Энергичный мятно-ванильный аромат для уверенного образа.', 12, N'/images/products/versace-eros-eau-de-toilette-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Eros Eau de Toilette', N'Versace', 169, 150, N'Энергичный мятно-ванильный аромат для уверенного образа.', 62, N'/images/products/versace-eros-eau-de-toilette-150.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Dylan Blue Pour Homme', N'Versace', 99, 50, N'Ароматический фужер с бергамотом и амброксаном.', 10, N'/images/products/versace-dylan-blue-pour-homme-50.webp', 1 FROM Categories c WHERE c.Name = N'Фужерные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Dylan Blue Pour Homme', N'Versace', 139, 100, N'Ароматический фужер с бергамотом и амброксаном.', 64, N'/images/products/versace-dylan-blue-pour-homme-100.webp', 1 FROM Categories c WHERE c.Name = N'Фужерные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bright Crystal', N'Versace', 79, 50, N'Сияющий цветочно-фруктовый аромат с гранатом и пионом.', 34, N'/images/products/versace-bright-crystal-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bright Crystal', N'Versace', 99, 90, N'Сияющий цветочно-фруктовый аромат с гранатом и пионом.', 37, N'/images/products/versace-bright-crystal-90.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'1 Million', N'Paco Rabanne', 99, 50, N'Провокационный кожано-пряный аромат с мятой и кожей.', 32, N'/images/products/paco-rabanne-1-million-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'1 Million', N'Paco Rabanne', 139, 100, N'Провокационный кожано-пряный аромат с мятой и кожей.', 60, N'/images/products/paco-rabanne-1-million-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'1 Million', N'Paco Rabanne', 169, 150, N'Провокационный кожано-пряный аромат с мятой и кожей.', 27, N'/images/products/paco-rabanne-1-million-150.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Lady Million', N'Paco Rabanne', 99, 50, N'Гламурный цветочный аромат с малиной и мёдом.', 20, N'/images/products/paco-rabanne-lady-million-50.webp', 1 FROM Categories c WHERE c.Name = N'Цветочные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Lady Million', N'Paco Rabanne', 119, 80, N'Гламурный цветочный аромат с малиной и мёдом.', 35, N'/images/products/paco-rabanne-lady-million-80.webp', 1 FROM Categories c WHERE c.Name = N'Цветочные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Invictus', N'Paco Rabanne', 99, 50, N'Спортивный свежий аромат с грейпфрутом и дубовым мхом.', 41, N'/images/products/paco-rabanne-invictus-50.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Invictus', N'Paco Rabanne', 139, 100, N'Спортивный свежий аромат с грейпфрутом и дубовым мхом.', 20, N'/images/products/paco-rabanne-invictus-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Invictus', N'Paco Rabanne', 169, 150, N'Спортивный свежий аромат с грейпфрутом и дубовым мхом.', 47, N'/images/products/paco-rabanne-invictus-150.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Good Girl', N'Carolina Herrera', 129, 50, N'Контрастный аромат с миндалём, кофе и туберозой.', 24, N'/images/products/carolina-herrera-good-girl-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Good Girl', N'Carolina Herrera', 159, 80, N'Контрастный аромат с миндалём, кофе и туберозой.', 58, N'/images/products/carolina-herrera-good-girl-80.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bad Boy', N'Carolina Herrera', 99, 50, N'Пряный мужской аромат с белым перцем и какао.', 45, N'/images/products/carolina-herrera-bad-boy-50.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bad Boy', N'Carolina Herrera', 129, 100, N'Пряный мужской аромат с белым перцем и какао.', 62, N'/images/products/carolina-herrera-bad-boy-100.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Light Blue', N'Dolce & Gabbana', 79, 50, N'Средиземноморский цитрусовый аромат с яблоком и кедром.', 41, N'/images/products/dolce-and-gabbana-light-blue-50.webp', 1 FROM Categories c WHERE c.Name = N'Цитрусовые';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Light Blue', N'Dolce & Gabbana', 99, 100, N'Средиземноморский цитрусовый аромат с яблоком и кедром.', 27, N'/images/products/dolce-and-gabbana-light-blue-100.webp', 1 FROM Categories c WHERE c.Name = N'Цитрусовые';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Light Blue', N'Dolce & Gabbana', 115, 125, N'Средиземноморский цитрусовый аромат с яблоком и кедром.', 62, N'/images/products/dolce-and-gabbana-light-blue-125.webp', 1 FROM Categories c WHERE c.Name = N'Цитрусовые';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'The One', N'Dolce & Gabbana', 99, 50, N'Тёплый восточный аромат с табаком и амброй.', 46, N'/images/products/dolce-and-gabbana-the-one-50.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'The One', N'Dolce & Gabbana', 149, 100, N'Тёплый восточный аромат с табаком и амброй.', 41, N'/images/products/dolce-and-gabbana-the-one-100.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'The One', N'Dolce & Gabbana', 179, 150, N'Тёплый восточный аромат с табаком и амброй.', 26, N'/images/products/dolce-and-gabbana-the-one-150.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Black Orchid', N'Tom Ford', 199, 50, N'Роскошный восточный аромат с чёрной орхидеей и пачули.', 50, N'/images/products/tom-ford-black-orchid-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Black Orchid', N'Tom Ford', 249, 100, N'Роскошный восточный аромат с чёрной орхидеей и пачули.', 63, N'/images/products/tom-ford-black-orchid-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Oud Wood', N'Tom Ford', 299, 50, N'Изысканный удово-древесный аромат с розовым деревом.', 37, N'/images/products/tom-ford-oud-wood-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Lost Cherry', N'Tom Ford', 299, 50, N'Соблазнительный вишнёво-миндальный гурманский аромат.', 38, N'/images/products/tom-ford-lost-cherry-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Aventus', N'Creed', 389, 50, N'Легендарный фруктово-древесный аромат с ананасом и берёзой.', 51, N'/images/products/creed-aventus-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Aventus', N'Creed', 589, 100, N'Легендарный фруктово-древесный аромат с ананасом и берёзой.', 8, N'/images/products/creed-aventus-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Silver Mountain Water', N'Creed', 299, 50, N'Альпийский свежий аромат с зелёным чаем и мускусом.', 20, N'/images/products/creed-silver-mountain-water-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Silver Mountain Water', N'Creed', 389, 100, N'Альпийский свежий аромат с зелёным чаем и мускусом.', 61, N'/images/products/creed-silver-mountain-water-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Gypsy Water', N'Byredo', 249, 50, N'Древесно-ароматический аромат с можжевельником и ванилью.', 44, N'/images/products/byredo-gypsy-water-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Gypsy Water', N'Byredo', 299, 100, N'Древесно-ароматический аромат с можжевельником и ванилью.', 51, N'/images/products/byredo-gypsy-water-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bal d''Afrique', N'Byredo', 249, 50, N'Солнечный цитрусово-цветочный аромат с африканскими нотами.', 60, N'/images/products/byredo-bal-dafrique-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Bal d''Afrique', N'Byredo', 299, 100, N'Солнечный цитрусово-цветочный аромат с африканскими нотами.', 20, N'/images/products/byredo-bal-dafrique-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Baccarat Rouge 540', N'Maison Francis Kurkdjian', 389, 70, N'Культовый амброво-цветочный аромат с шафраном и кедром.', 34, N'/images/products/maison-francis-kurkdjian-baccarat-rouge-540-70.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Aqua Universalis', N'Maison Francis Kurkdjian', 199, 70, N'Универсальный свежий аромат с цитрусами и белыми цветами.', 53, N'/images/products/maison-francis-kurkdjian-aqua-universalis-70.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Aqua Universalis', N'Maison Francis Kurkdjian', 249, 200, N'Универсальный свежий аромат с цитрусами и белыми цветами.', 38, N'/images/products/maison-francis-kurkdjian-aqua-universalis-200.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Red Tobacco', N'Mancera', 199, 120, N'Насыщенный табачно-восточный аромат с корицей и удом.', 41, N'/images/products/mancera-red-tobacco-120.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Intense Cafe', N'Montale', 159, 100, N'Гурманский кофейно-розовый аромат с ванилью.', 46, N'/images/products/montale-intense-cafe-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Naxos', N'Xerjoff', 299, 100, N'Итальянский гурманский аромат с лавандой и мёдом.', 37, N'/images/products/xerjoff-naxos-100.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Black Phantom', N'Kilian', 299, 50, N'Тёмный гурманский аромат с ромом и кофе.', 24, N'/images/products/kilian-black-phantom-50.webp', 1 FROM Categories c WHERE c.Name = N'Нишевая парфюмерия';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'English Pear & Freesia', N'Jo Malone', 159, 100, N'Сочная груша с белой фрезией — лёгкий британский cologne для каждого дня.', 38, N'/images/products/jo-malone-english-pear-and-freesia-100.webp', 1 FROM Categories c WHERE c.Name = N'Цитрусовые';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Wood Sage & Sea Salt', N'Jo Malone', 129, 100, N'Морской бриз и шалфей — свежий унисекс-аромат без тяжёлого шлейфа.', 44, N'/images/products/jo-malone-wood-sage-and-sea-salt-100.webp', 1 FROM Categories c WHERE c.Name = N'Свежие';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Prada L''Homme', N'Prada', 199, 100, N'Чистый ирисово-амбровый аромат с нотами кардамона и пачули.', 31, N'/images/products/prada-lhomme-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Luna Rossa Carbon', N'Prada', 159, 100, N'Современный древесно-амбровый аромат с лавандой и металлическим акцентом.', 27, N'/images/products/prada-luna-rossa-carbon-100.webp', 1 FROM Categories c WHERE c.Name = N'Мужские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Donna Born In Roma', N'Valentino', 159, 50, N'Ванильная жасминовая композиция в стильном флаконе с шипами.', 33, N'/images/products/valentino-donna-born-in-roma-50.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Donna Born In Roma', N'Valentino', 199, 100, N'Ванильная жасминовая композиция в стильном флаконе с шипами.', 19, N'/images/products/valentino-donna-born-in-roma-100.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Shalimar', N'Guerlain', 129, 50, N'Легендарный восточный аромат с бергамотом, ирисом и ванилью.', 42, N'/images/products/guerlain-shalimar-50.webp', 1 FROM Categories c WHERE c.Name = N'Восточные';
INSERT INTO Products (CategoryId, Name, Brand, Price, Volume, Description, StockQuantity, ImageUrl, IsActive)
SELECT c.Id, N'Mon Guerlain', N'Guerlain', 159, 100, N'Современный лавандово-ванильный аромат с нотами туберозы.', 36, N'/images/products/guerlain-mon-guerlain-100.webp', 1 FROM Categories c WHERE c.Name = N'Женские';
GO

INSERT INTO Cart (UserId) SELECT Id FROM Users WHERE Email IN (N'anna.koval@mail.by', N'dmitry.sidorov@gmail.com', N'elena.moroz@mail.by');
GO
INSERT INTO CartItems (CartId, ProductId, Quantity)
SELECT c.Id, p.Id, 1 FROM Cart c JOIN Users u ON u.Id = c.UserId JOIN Products p ON p.Name = N'Sauvage Eau de Toilette' AND p.Brand = N'Dior' AND p.Volume = 100 WHERE u.Email = N'anna.koval@mail.by';
INSERT INTO CartItems (CartId, ProductId, Quantity)
SELECT c.Id, p.Id, 2 FROM Cart c JOIN Users u ON u.Id = c.UserId JOIN Products p ON p.Name = N'N°5 Eau de Parfum' AND p.Brand = N'Chanel' AND p.Volume = 50 WHERE u.Email = N'dmitry.sidorov@gmail.com';
GO

INSERT INTO Favorites (UserId, ProductId) SELECT u.Id, p.Id FROM Users u CROSS JOIN (SELECT TOP 3 Id FROM Products ORDER BY Id) p WHERE u.Email = N'anna.koval@mail.by';
INSERT INTO Favorites (UserId, ProductId) SELECT u.Id, p.Id FROM Users u CROSS JOIN (SELECT TOP 2 Id FROM Products ORDER BY Id DESC) p WHERE u.Email = N'maria.voytovich@mail.by';
GO

INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Беру уже третий флакон — на работе все спрашивают, что за аромат. Стойкость на коже около шести часов.' FROM Users u JOIN Products p ON p.Name = N'Sauvage Eau de Toilette' AND p.Brand = N'Dior' AND p.Volume = 100 WHERE u.Email = N'anna.koval@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Классика, которую сложно не узнать. На вечернее мероприятие села идеально, хотя для офиса многовато.' FROM Users u JOIN Products p ON p.Name = N'N°5 Eau de Parfum' AND p.Brand = N'Chanel' AND p.Volume = 50 WHERE u.Email = N'dmitry.sidorov@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Ваниль с кофе звучит громко, но на коже раскрывается мягко. Зимой ношу почти каждый день.' FROM Users u JOIN Products p ON p.Name = N'Black Opium' AND p.Brand = N'Yves Saint Laurent' AND p.Volume = 50 WHERE u.Email = N'elena.moroz@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Дорого, но оправдано: ананас чувствуется с первых секунд, а шлейф держится до вечера.' FROM Users u JOIN Products p ON p.Name = N'Aventus' AND p.Brand = N'Creed' AND p.Volume = 50 WHERE u.Email = N'igor.petrov@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Лаванда здесь не лекарственная, а свежая. Коллеги отметили, что аромат запоминающийся.' FROM Users u JOIN Products p ON p.Name = N'Libre Eau de Parfum' AND p.Brand = N'Yves Saint Laurent' AND p.Volume = 50 WHERE u.Email = N'maria.voytovich@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Подарила себе на день рождения — ни разу не пожалела. Цитрус в начале, потом тёплый жасмин.' FROM Users u JOIN Products p ON p.Name = N'Coco Mademoiselle' AND p.Brand = N'Chanel' AND p.Volume = 50 WHERE u.Email = N'olga.lebedeva@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Универсальный вариант: и на свидание, и на деловую встречу. Флакон 100 мл хватает надолго.' FROM Users u JOIN Products p ON p.Name = N'Bleu de Chanel Eau de Parfum' AND p.Brand = N'Chanel' AND p.Volume = 100 WHERE u.Email = N'sergey.novik@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Цветочный, но не приторный. Муж сказал, что пахну «как весной в саду» — для меня лучший комплимент.' FROM Users u JOIN Products p ON p.Name = N'J''adore Eau de Parfum' AND p.Brand = N'Dior' AND p.Volume = 50 WHERE u.Email = N'natalya.zhukova@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Морской акцент без мыла — редкость. Летом на замену тяжёлым духам.' FROM Users u JOIN Products p ON p.Name = N'Acqua di Gio Profondo' AND p.Brand = N'Giorgio Armani' AND p.Volume = 100 WHERE u.Email = N'pavel.kravets@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 3, N'Красивый флакон, аромат сладковатый. На мне держится 3–4 часа, потом нужно обновить.' FROM Users u JOIN Products p ON p.Name = N'Good Girl' AND p.Brand = N'Carolina Herrera' AND p.Volume = 50 WHERE u.Email = N'tatiana.sokol@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Мятная нота в начале бодрит, потом уходит в ваниль. Молодёжный, но не дешёвый по ощущению.' FROM Users u JOIN Products p ON p.Name = N'Eros' AND p.Brand = N'Versace' AND p.Volume = 100 WHERE u.Email = N'andrey.volkov@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Шафран и кедр — сочетание необычное. На мне шлейф слышен даже на следующий день на одежде.' FROM Users u JOIN Products p ON p.Name = N'Baccarat Rouge 540' AND p.Brand = N'Maison Francis Kurkdjian' AND p.Volume = 70 WHERE u.Email = N'ksenia.orlova@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Лёгкий, почти прозрачный. Подходит, когда не хочется «кричать» ароматом.' FROM Users u JOIN Products p ON p.Name = N'Gypsy Water' AND p.Brand = N'Byredo' AND p.Volume = 50 WHERE u.Email = N'anna.koval@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Уд без больничного запаха — сбалансированный, с ванилью. Для особых случаев.' FROM Users u JOIN Products p ON p.Name = N'Oud Wood' AND p.Brand = N'Tom Ford' AND p.Volume = 50 WHERE u.Email = N'dmitry.sidorov@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Свежий, водянистый, для жаркого лета самое то. Расходуется быстро — наношу щедро.' FROM Users u JOIN Products p ON p.Name = N'Bright Crystal' AND p.Brand = N'Versace' AND p.Volume = 90 WHERE u.Email = N'elena.moroz@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 3, N'Яркий, заметный, но на мне немного приторный к концу дня. Зато комплименты гарантированы.' FROM Users u JOIN Products p ON p.Name = N'1 Million' AND p.Brand = N'Paco Rabanne' AND p.Volume = 100 WHERE u.Email = N'igor.petrov@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Роза и пион — романтично, но современно. Ношу с платьями и с джинсами одинаково хорошо.' FROM Users u JOIN Products p ON p.Name = N'Miss Dior Eau de Parfum' AND p.Brand = N'Dior' AND p.Volume = 50 WHERE u.Email = N'maria.voytovich@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Подарила мужу — теперь сам просит докупить. Кардамон чувствуется, но не доминирует.' FROM Users u JOIN Products p ON p.Name = N'La Nuit de L''Homme' AND p.Brand = N'Yves Saint Laurent' AND p.Volume = 60 WHERE u.Email = N'olga.lebedeva@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Сладковатый, уютный, как вечер у камина. Зимой — мой основной.' FROM Users u JOIN Products p ON p.Name = N'Stronger With You' AND p.Brand = N'Giorgio Armani' AND p.Volume = 100 WHERE u.Email = N'sergey.novik@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Нежный, почти невесомый. Для офиса идеален — никого не раздражает.' FROM Users u JOIN Products p ON p.Name = N'Chance Eau Tendre' AND p.Brand = N'Chanel' AND p.Volume = 50 WHERE u.Email = N'natalya.zhukova@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Старый знакомый аромат, но качество стабильное. Кожа и фиалка — узнаваемый почерк Dior.' FROM Users u JOIN Products p ON p.Name = N'Fahrenheit' AND p.Brand = N'Dior' AND p.Volume = 100 WHERE u.Email = N'pavel.kravets@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Ваниль с жасмином — тёплый, женственный. Флакон красиво смотрится на полке.' FROM Users u JOIN Products p ON p.Name = N'Donna Born In Roma' AND p.Brand = N'Valentino' AND p.Volume = 50 WHERE u.Email = N'tatiana.sokol@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 3, N'Спортивный характер, на тренировку не пойдёт, но после душа — самое то.' FROM Users u JOIN Products p ON p.Name = N'Invictus' AND p.Brand = N'Paco Rabanne' AND p.Volume = 100 WHERE u.Email = N'andrey.volkov@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Вишня и миндаль — гурманский рай. Дорого, но эмоции стоят того.' FROM Users u JOIN Products p ON p.Name = N'Lost Cherry' AND p.Brand = N'Tom Ford' AND p.Volume = 50 WHERE u.Email = N'ksenia.orlova@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Лёгкий, освежающий. Сам по себе держится недолго, зато сочетается с другими ароматами.' FROM Users u JOIN Products p ON p.Name = N'English Pear & Freesia' AND p.Brand = N'Jo Malone' AND p.Volume = 100 WHERE u.Email = N'anna.koval@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Концентрация выше EDT — разница ощутима с первого распыления. Шлейф плотный.' FROM Users u JOIN Products p ON p.Name = N'Sauvage Elixir' AND p.Brand = N'Dior' AND p.Volume = 60 WHERE u.Email = N'dmitry.sidorov@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Чистый, мыльно-ирисовый, деловой. Под костюм садится безупречно.' FROM Users u JOIN Products p ON p.Name = N'Prada L''Homme' AND p.Brand = N'Prada' AND p.Volume = 100 WHERE u.Email = N'elena.moroz@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Табак и корица — на любителя. Мне зашло, жене показалось слишком тяжёлым.' FROM Users u JOIN Products p ON p.Name = N'Red Tobacco' AND p.Brand = N'Mancera' AND p.Volume = 120 WHERE u.Email = N'igor.petrov@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Пахнет как кофе с розой в хорошей кондитерской. Зимой ношу постоянно.' FROM Users u JOIN Products p ON p.Name = N'Intense Cafe' AND p.Brand = N'Montale' AND p.Volume = 100 WHERE u.Email = N'maria.voytovich@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Мёд и лаванда — неожиданный дуэт. Один из самых сложных ароматов в коллекции.' FROM Users u JOIN Products p ON p.Name = N'Naxos' AND p.Brand = N'Xerjoff' AND p.Volume = 100 WHERE u.Email = N'olga.lebedeva@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Тёмный, гурманский, с ромом. На вечеринку — да, на работу — нет.' FROM Users u JOIN Products p ON p.Name = N'Black Phantom' AND p.Brand = N'Kilian' AND p.Volume = 50 WHERE u.Email = N'sergey.novik@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Альпийская свежесть, без химозного «спорта». Летом заменил мне одеколон.' FROM Users u JOIN Products p ON p.Name = N'Silver Mountain Water' AND p.Brand = N'Creed' AND p.Volume = 50 WHERE u.Email = N'natalya.zhukova@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Солнечный, с лёгкой сладостью. Напоминает отпуск — в хорошем смысле.' FROM Users u JOIN Products p ON p.Name = N'Bal d''Afrique' AND p.Brand = N'Byredo' AND p.Volume = 50 WHERE u.Email = N'pavel.kravets@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Универсальный, чистый. Когда не знаю, что надеть из парфюма — беру его.' FROM Users u JOIN Products p ON p.Name = N'Aqua Universalis' AND p.Brand = N'Maison Francis Kurkdjian' AND p.Volume = 70 WHERE u.Email = N'tatiana.sokol@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Яблоко и шалфей — свежо, по-молодому. Жене тоже понравился, носим по очереди.' FROM Users u JOIN Products p ON p.Name = N'Y Eau de Parfum' AND p.Brand = N'Yves Saint Laurent' AND p.Volume = 60 WHERE u.Email = N'andrey.volkov@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Плотнее EDT, шлейф заметнее. На свидание выбрала именно эту версию.' FROM Users u JOIN Products p ON p.Name = N'Sauvage Eau de Parfum' AND p.Brand = N'Dior' AND p.Volume = 60 WHERE u.Email = N'ksenia.orlova@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Большой флакон выгоднее — пользуюсь второй год, запах не надоел.' FROM Users u JOIN Products p ON p.Name = N'Coco Mademoiselle' AND p.Brand = N'Chanel' AND p.Volume = 100 WHERE u.Email = N'anna.koval@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Купил после теста в магазине — на коже раскрывается иначе, чем на блоттере.' FROM Users u JOIN Products p ON p.Name = N'Bleu de Chanel Eau de Parfum' AND p.Brand = N'Chanel' AND p.Volume = 50 WHERE u.Email = N'dmitry.sidorov@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Мама носила J''adore, я продолжаю традицию. Качество стабильное.' FROM Users u JOIN Products p ON p.Name = N'J''adore Eau de Parfum' AND p.Brand = N'Dior' AND p.Volume = 100 WHERE u.Email = N'elena.moroz@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 3, N'Сладковатый, молодёжный. В 35 уже чувствую себя немного старше целевой аудитории.' FROM Users u JOIN Products p ON p.Name = N'Eros' AND p.Brand = N'Versace' AND p.Volume = 50 WHERE u.Email = N'igor.petrov@yandex.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Формат 80 мл удобен в сумке. Аромат вечерний, днём не использую.' FROM Users u JOIN Products p ON p.Name = N'Good Girl' AND p.Brand = N'Carolina Herrera' AND p.Volume = 80 WHERE u.Email = N'maria.voytovich@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Мини-формат для пробы — теперь закажу полный. Кофе и ваниль — моё.' FROM Users u JOIN Products p ON p.Name = N'Black Opium' AND p.Brand = N'Yves Saint Laurent' AND p.Volume = 30 WHERE u.Email = N'olga.lebedeva@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Легенда оправдана, хотя цена кусается. 100 мл — инвестиция на полгода.' FROM Users u JOIN Products p ON p.Name = N'Aventus' AND p.Brand = N'Creed' AND p.Volume = 100 WHERE u.Email = N'sergey.novik@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Формат 90 мл — хорошая стойкость. Лаванда не доминирует, баланс удачный.' FROM Users u JOIN Products p ON p.Name = N'Libre Eau de Parfum' AND p.Brand = N'Yves Saint Laurent' AND p.Volume = 90 WHERE u.Email = N'natalya.zhukova@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Уд мягкий, кремовый. Не пахну мечетью, как некоторые боятся — всё цивилизованно.' FROM Users u JOIN Products p ON p.Name = N'Oud Wood' AND p.Brand = N'Tom Ford' AND p.Volume = 100 WHERE u.Email = N'pavel.kravets@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 5, N'Классика Guerlain — ваниль и ирис на мне звучат благородно, без приторности.' FROM Users u JOIN Products p ON p.Name = N'Shalimar' AND p.Brand = N'Guerlain' AND p.Volume = 50 WHERE u.Email = N'olga.lebedeva@gmail.com';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Беру на лето: лёгкий, морской, не перегружает. С коллегами в лифте комфортно.' FROM Users u JOIN Products p ON p.Name = N'Wood Sage & Sea Salt' AND p.Brand = N'Jo Malone' AND p.Volume = 100 WHERE u.Email = N'sergey.novik@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Лаванда здесь мягкая, почти пудровая. На свидание села лучше, чем ожидала.' FROM Users u JOIN Products p ON p.Name = N'Mon Guerlain' AND p.Brand = N'Guerlain' AND p.Volume = 100 WHERE u.Email = N'natalya.zhukova@mail.by';
INSERT INTO Reviews (UserId, ProductId, Rating, Comment)
SELECT
 u.Id, p.Id, 4, N'Современный, чуть металлический старт — потом тёплый амбровый шлейф. На каждый день.' FROM Users u JOIN Products p ON p.Name = N'Luna Rossa Carbon' AND p.Brand = N'Prada' AND p.Volume = 100 WHERE u.Email = N'andrey.volkov@mail.by';
GO

INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'anna.koval@mail.by'), DATEADD(day,-45,GETDATE()), 258.00, N'г. Минск, ул. Немига, 12, кв. 45', N'Карта', N'Курьер', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Sauvage Eau de Toilette' AND p.Brand=N'Dior' AND p.Volume=100;
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'English Pear & Freesia' AND p.Brand=N'Jo Malone' AND p.Volume=100;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'dmitry.sidorov@gmail.com'), DATEADD(day,-30,GETDATE()), 328.00, N'г. Минск, пр-т Победителей, 84', N'Карта', N'Самовывоз', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'N°5 Eau de Parfum' AND p.Brand=N'Chanel' AND p.Volume=50;
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Bleu de Chanel Eau de Parfum' AND p.Brand=N'Chanel' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'elena.moroz@mail.by'), DATEADD(day,-14,GETDATE()), 349.00, N'г. Гродно, ул. Советская, 5', N'Карта', N'Почта', N'OnTheWay');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Aventus' AND p.Brand=N'Creed' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'igor.petrov@yandex.by'), DATEADD(day,-7,GETDATE()), 199.00, N'г. Брест, ул. Машерова, 17', N'Наличные', N'Курьер', N'Assembling');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'N°5 Eau de Parfum' AND p.Brand=N'Chanel' AND p.Volume=35;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'maria.voytovich@mail.by'), DATEADD(day,-3,GETDATE()), 129.00, N'г. Минск, ул. Кальварийская, 22', N'Карта', N'Курьер', N'New');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Sauvage Eau de Toilette' AND p.Brand=N'Dior' AND p.Volume=60;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'olga.lebedeva@gmail.com'), DATEADD(day,-60,GETDATE()), 648.00, N'г. Витебск, пр-т Фрунзе, 3', N'Карта', N'Почта', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Baccarat Rouge 540' AND p.Brand=N'Maison Francis Kurkdjian' AND p.Volume=70;
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Lost Cherry' AND p.Brand=N'Tom Ford' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'sergey.novik@mail.by'), DATEADD(day,-21,GETDATE()), 129.00, N'г. Минск, ул. Притыцкого, 156', N'Карта', N'Самовывоз', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Stronger With You' AND p.Brand=N'Giorgio Armani' AND p.Volume=100;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'natalya.zhukova@mail.by'), DATEADD(day,-10,GETDATE()), 159.00, N'г. Гомель, ул. Советская, 28', N'Карта', N'Курьер', N'OnTheWay');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Chance Eau Tendre' AND p.Brand=N'Chanel' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'pavel.kravets@gmail.com'), DATEADD(day,-5,GETDATE()), 299.00, N'г. Минск, ул. Независимости, 95', N'Карта', N'Курьер', N'Assembling');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Oud Wood' AND p.Brand=N'Tom Ford' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'tatiana.sokol@yandex.by'), DATEADD(day,-18,GETDATE()), 199.00, N'г. Могилёв, ул. Ленинская, 7', N'Наличные', N'Почта', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Donna Born In Roma' AND p.Brand=N'Valentino' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'andrey.volkov@mail.by'), DATEADD(day,-2,GETDATE()), 129.00, N'г. Минск, ул. Тимирязева, 64', N'Карта', N'Курьер', N'New');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Invictus' AND p.Brand=N'Paco Rabanne' AND p.Volume=100;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'ksenia.orlova@gmail.com'), DATEADD(day,-35,GETDATE()), 498.00, N'г. Минск, ул. Якубовского, 33', N'Карта', N'Курьер', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Gypsy Water' AND p.Brand=N'Byredo' AND p.Volume=50;
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Bal d''Afrique' AND p.Brand=N'Byredo' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'anna.koval@mail.by'), DATEADD(day,-90,GETDATE()), 199.00, N'г. Минск, ул. Немига, 12, кв. 45', N'Карта', N'Почта', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Miss Dior Eau de Parfum' AND p.Brand=N'Dior' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'dmitry.sidorov@gmail.com'), DATEADD(day,-1,GETDATE()), 199.00, N'г. Минск, пр-т Победителей, 84', N'Карта', N'Самовывоз', N'Canceled');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Sauvage Elixir' AND p.Brand=N'Dior' AND p.Volume=60;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'elena.moroz@mail.by'), DATEADD(day,-25,GETDATE()), 129.00, N'г. Гродно, ул. Советская, 5', N'Наличные', N'Курьер', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Black Opium' AND p.Brand=N'Yves Saint Laurent' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'igor.petrov@yandex.by'), DATEADD(day,-12,GETDATE()), 199.00, N'г. Брест, ул. Машерова, 17', N'Карта', N'Почта', N'OnTheWay');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Red Tobacco' AND p.Brand=N'Mancera' AND p.Volume=120;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'maria.voytovich@mail.by'), DATEADD(day,-40,GETDATE()), 199.00, N'г. Минск, ул. Кальварийская, 22', N'Карта', N'Курьер', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Intense Cafe' AND p.Brand=N'Montale' AND p.Volume=100;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'olga.lebedeva@gmail.com'), DATEADD(day,-8,GETDATE()), 199.00, N'г. Витебск, пр-т Фрунзе, 3', N'Карта', N'Самовывоз', N'Assembling');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Coco Mademoiselle' AND p.Brand=N'Chanel' AND p.Volume=50;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'sergey.novik@mail.by'), DATEADD(day,-55,GETDATE()), 258.00, N'г. Минск, ул. Притыцкого, 156', N'Карта', N'Почта', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Fahrenheit' AND p.Brand=N'Dior' AND p.Volume=100;
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Acqua di Gio Profondo' AND p.Brand=N'Giorgio Armani' AND p.Volume=100;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'natalya.zhukova@mail.by'), DATEADD(day,-4,GETDATE()), 99.00, N'г. Гомель, ул. Советская, 28', N'Наличные', N'Курьер', N'New');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Bright Crystal' AND p.Brand=N'Versace' AND p.Volume=90;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'pavel.kravets@gmail.com'), DATEADD(day,-28,GETDATE()), 389.00, N'г. Минск, ул. Независимости, 95', N'Карта', N'Курьер', N'Delivered');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Aventus' AND p.Brand=N'Creed' AND p.Volume=100;
INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)
VALUES ((SELECT Id FROM Users WHERE Email=N'tatiana.sokol@yandex.by'), DATEADD(day,-15,GETDATE()), 299.00, N'г. Могилёв, ул. Ленинская, 7', N'Карта', N'Почта', N'OnTheWay');
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
SELECT (SELECT MAX(Id) FROM Orders), p.Id, 1, p.Price FROM Products p WHERE p.Name=N'Naxos' AND p.Brand=N'Xerjoff' AND p.Volume=100;
GO

INSERT INTO SupplyRequests (SupplierId, CreatedByUserId, RequestDate, Status, Comment)
SELECT s.Id, u.Id, DATEADD(day,-7,GETDATE()), N'Completed', N'Пополнение ходовых позиций Dior и Chanel' FROM Suppliers s CROSS JOIN Users u WHERE s.Name=N'Luxe Fragrance Distribution' AND u.Email=N'manager@perfume.by';
INSERT INTO SupplyRequests (SupplierId, CreatedByUserId, RequestDate, Status, Comment)
SELECT s.Id, u.Id, DATEADD(day,-2,GETDATE()), N'Pending', N'Заявка на нишевые бренды' FROM Suppliers s CROSS JOIN Users u WHERE s.Name=N'Niche Aromas Import' AND u.Email=N'manager@perfume.by';
GO
INSERT INTO SupplyRequestItems (SupplyRequestId, ProductId, Quantity)
SELECT sr.Id, p.Id, 20 FROM SupplyRequests sr JOIN Products p ON p.Brand IN (N'Dior',N'Chanel') WHERE sr.Status=N'Completed';
GO

