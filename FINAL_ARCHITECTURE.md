# FINAL ARCHITECTURE — PerfumeStore

## 1. Статус Документа

Этот документ фиксирует итоговую архитектуру MVP интернет-магазина парфюмерии `PerfumeStore` перед началом разработки.

Приоритет источников истины:

1. Финальная спецификация пользователя.
2. SQL-структура БД: `Database/01_CreateDatabase.sql`.
3. Тестовые данные: `Database/02_TestData.sql`.
4. ERD-диаграммы Chen, Barker, Martin из `Documentation/erd диаграммы`.
5. Figma-макеты из `Documentation/фигма макеты`.
6. Текущий код `Source`.

Разработка кода не выполнялась. Документ создан как результат аудита репозитория.

## 2. Технологии

Использовать:

- ASP.NET Core MVC.
- Entity Framework Core.
- SQL Server.
- Razor Views.
- Bootstrap 5.
- Service Layer Pattern.
- Cookie Authentication.
- Claims.
- Role Authorization.

Не использовать:

- React.
- Angular.
- Vue.
- Blazor.
- Minimal API.
- Microservices.
- ASP.NET Identity.
- IdentityUser.
- AspNetUsers.
- AspNetRoles.

Проект должен оставаться классическим монолитным ASP.NET Core MVC-приложением.
Авторизация должна использовать существующую таблицу `Users`, cookie, claims и роли `Client`, `OrderManager`, `ContentManager`, `Admin`.
Repository Pattern допускается только при реальной необходимости; если достаточно `DbContext` и `Services`, отдельный repository-слой не создавать.

## 3. Аудит Репозитория

### 3.1. Database

Найдены и проанализированы:

- `Database/01_CreateDatabase.sql` — структура БД `PerfumeStore`.
- `Database/02_TestData.sql` — тестовые данные.

Существующие таблицы:

- `Users`
- `Categories`
- `Products`
- `Cart`
- `CartItems`
- `Orders`
- `OrderItems`
- `Favorites`
- `Reviews`
- `Suppliers`
- `SupplyRequests`
- `SupplyRequestItems`

Запрещено без отдельного согласования:

- создавать новые таблицы;
- изменять существующие таблицы;
- добавлять новые связи;
- менять типы полей;
- расширять SQL-схему под функции, которых нет в БД.

### 3.2. Test Data

`Database/02_TestData.sql` содержит:

- пользователей всех основных ролей:
  - `admin@perfume.by` — `Admin`;
  - `manager@perfume.by` — `OrderManager`;
  - `content@perfume.by` — `ContentManager`;
  - `client123@mail.by` — `Client`;
  - `client2@mail.by` — `Client`;
- категории:
  - `Цветочные`;
  - `Цитрусовые`;
  - `Древесные`;
  - `Восточные`;
  - `Унисекс`;
- товары Dior, Chanel, Tom Ford, Versace, Armani, Dolce & Gabbana, Maison Francis Kurkdjian, Hermes, Guerlain, Yves Saint Laurent;
- активные и архивные товары через `Products.IsActive`;
- корзины и позиции корзин;
- заказы со статусами `New`, `Assembling`, `Delivered`;
- избранное;
- отзывы;
- поставщиков;
- заявки поставщикам со статусами `Pending`, `Completed`.

Замечание по безопасности: тестовые пароли в `02_TestData.sql` записаны как простые строки (`admin123`, `manager123`, `client123`). Для учебного seed-скрипта это допустимо только как тестовые данные; в приложении поле `Users.PasswordHash` должно использовать хеш пароля.

### 3.3. Diagrams

Проанализированы ERD-диаграммы:

- `нотация питера-чена.png`;
- `нотация баркера.png`;
- `нотация мартина.png`;
- `erd Пиляй.docx` присутствует как DOCX-материал.

Вывод:

- Chen описывает концептуальное ядро: пользователь, заказ, товар, категория, отзыв.
- Barker добавляет более табличную модель: пользователь, корзина, позиция корзины, заказ, позиция заказа, товар, категория.
- Martin ближе к SQL, так как содержит статус заказа и связи в Crow's Foot-нотации.
- SQL-схема шире диаграмм: содержит `Favorites`, `Suppliers`, `SupplyRequests`, `SupplyRequestItems`.

### 3.4. Documentation

В `Documentation` присутствуют:

- DOCX-постановка задачи;
- DOCX-анализ требований;
- DOCX-инструменты разработки;
- DOCX-описание ERD;
- ERD PNG;
- Figma PNG-макеты по ролям.

DOCX-файлы являются проектной документацией, но бизнес-правила для разработки фиксируются этим `FINAL_ARCHITECTURE.md` и финальной спецификацией пользователя.

### 3.5. Source

Текущий `Source` — стартовый ASP.NET Core MVC-проект:

- `Program.cs` содержит `AddControllersWithViews`, `UseStaticFiles`, `UseRouting`, `UseAuthorization`.
- `UseAuthentication` отсутствует.
- EF Core не подключен.
- `DbContext` отсутствует.
- доменные модели отсутствуют.
- сервисы и репозитории отсутствуют.
- контроллер есть только шаблонный `HomeController`.
- views представлены стандартными Razor-шаблонами `Home`, `Shared`, `_Layout`.
- Bootstrap, jQuery и jQuery Validation присутствуют в `wwwroot/lib`.

## 4. Роли И Права

### 4.1. Guest

`Guest` не является пользователем системы и не хранится в БД.

Может:

- просматривать главную страницу;
- просматривать каталог;
- просматривать категории;
- просматривать карточки товаров;
- пользоваться поиском;
- пользоваться фильтрацией.

Не может:

- добавлять товары в корзину;
- оформлять заказ;
- добавлять товары в избранное;
- оставлять отзывы;
- просматривать личный кабинет;
- просматривать историю заказов.

Страницы входа и регистрации являются публичными страницами. После регистрации пользователь получает роль `Client`.

### 4.2. Client

Может:

- входить в систему;
- редактировать профиль;
- просматривать каталог;
- просматривать товары;
- использовать корзину;
- оформлять заказ;
- выбирать доставку;
- выбирать оплату;
- просматривать историю заказов;
- отслеживать заказы;
- добавлять товары в избранное;
- удалять товары из избранного;
- оставлять отзывы.

Не может:

- управлять товарами;
- управлять категориями;
- управлять заказами других пользователей;
- работать со складом;
- работать с поставщиками;
- управлять пользователями.

### 4.3. OrderManager

Работает только с заказами.

Может:

- просматривать список заказов;
- просматривать детали заказа;
- изменять статус заказа;
- отменять заказ;
- просматривать историю заказов.

Запрещено:

- изменять товары;
- изменять категории;
- изменять склад;
- изменять поставщиков;
- управлять пользователями;
- изменять настройки системы.

### 4.4. ContentManager

Работает только с каталогом.

Может:

- создавать товары;
- редактировать товары;
- архивировать товары через `Products.IsActive`;
- восстанавливать товары через `Products.IsActive`;
- управлять категориями;
- загружать изображения товаров через `Products.ImageUrl`.

Запрещено:

- работать с заказами;
- менять статусы заказов;
- работать со складом;
- работать с поставщиками;
- управлять пользователями;
- изменять настройки системы.

### 4.5. Admin

Имеет полный доступ в рамках существующей БД.

Может:

- управлять пользователями;
- управлять ролями пользователей;
- просматривать сотрудников;
- управлять складскими остатками;
- управлять поставщиками;
- создавать и обрабатывать заявки поставщикам;
- просматривать аналитику;
- просматривать отчеты.

Важно:

- сотрудники — это записи `Users` с ролями `Admin`, `OrderManager`, `ContentManager`;
- отдельную таблицу сотрудников не создавать;
- отдельный `Employee` entity не создавать;
- отдельный `EmployeeManagementController` не создавать.

## 5. Правила Использования БД

### 5.1. Склад

Склад реализуется только через:

- `Products.StockQuantity`.

Не создавать:

- таблицы склада;
- таблицы складских движений;
- отдельные сущности остатков.

### 5.2. Поставщики

Поставщики реализуются только через:

- `Suppliers`;
- `SupplyRequests`;
- `SupplyRequestItems`.

Не создавать дополнительные сущности поставщиков.

### 5.3. Заказы

Использовать только статусы из SQL:

- `New`
- `Confirmed`
- `Assembling`
- `InDelivery`
- `OnTheWay`
- `Delivered`
- `Canceled`

Другие статусы запрещены.

Историю изменения статусов не реализовывать отдельной таблицей.

### 5.4. Изображения

Изображение товара реализуется только через:

- `Products.ImageUrl`.

Не создавать:

- галерею изображений;
- таблицу изображений;
- несколько изображений на товар.

### 5.5. Архив Товаров

Архив реализуется только через:

- `Products.IsActive`.

Не создавать отдельную таблицу архива.

### 5.6. Оплата И Доставка

Использовать только:

- `Orders.PaymentMethod`;
- `Orders.DeliveryMethod`.

Не создавать:

- таблицы настроек оплаты;
- таблицы настроек доставки;
- отдельные сущности payment/delivery settings.

### 5.7. Аналитика И Отчеты

Никаких таблиц аналитики не создавать.

Отчеты строить на основе:

- `Orders`;
- `OrderItems`;
- `Products`;
- `Users`;
- `Suppliers`;
- `SupplyRequests`.

## 6. Список Экранов MVP

### 6.1. Public / Guest

Реализовать:

- Главная.
- Каталог.
- Категории.
- Поиск товаров.
- Фильтрация товаров.
- Карточка товара.
- Вход.
- Регистрация.

Ограничение:

- вход и регистрация являются публичными страницами, но не правами роли `Guest`;
- гостевая корзина из макетов вне MVP, потому что Guest не может добавлять товары в корзину.

### 6.2. Client

Реализовать:

- Главная в авторизованном состоянии.
- Каталог.
- Карточка товара.
- Корзина.
- Оформление заказа.
- Выбор доставки.
- Выбор оплаты.
- Результат оформления/оплаты.
- Личный кабинет.
- Профиль.
- Редактирование профиля.
- История заказов.
- Детали заказа.
- Отслеживание заказа.
- Избранное.
- Отзывы.

Вне MVP:

- уведомления, так как нет таблицы уведомлений.

### 6.3. OrderManager

Реализовать:

- Панель заказов.
- Очередь заказов.
- Детали заказа.
- Управление статусом.
- Отмена заказа.
- История заказов.

Не реализовывать для OrderManager:

- управление товарами;
- редактор товара;
- категории;
- архив товаров;
- скидки;
- загрузку изображений;
- предпросмотр товара.

### 6.4. ContentManager

Реализовать:

- Панель контент-менеджера.
- Управление товарами.
- Создание товара.
- Редактирование товара.
- Предпросмотр товара.
- Архив товаров.
- Восстановление товара.
- Категории.
- Загрузка изображения товара.

Не реализовывать для ContentManager:

- очередь заказов;
- детали заказа;
- управление статусом;
- отмену заказа;
- поставщиков;
- склад.

Вне MVP:

- скидки, так как нет таблиц скидок/промокодов.

### 6.5. Admin

Реализовать:

- Админ-панель.
- Пользователи.
- Роли пользователей.
- Сотрудники как фильтр `Users` по ролям `Admin`, `OrderManager`, `ContentManager`.
- Склад через `Products.StockQuantity`.
- Корректировка остатков.
- Поставщики.
- Заявки поставщикам.
- Детали заявки поставщику.
- Аналитика.
- Отчеты.

Не реализовывать как полноценный CRUD:

- настройки оплаты;
- настройки доставки.

Причина:

- для них нет отдельных таблиц;
- в MVP использовать только значения `Orders.PaymentMethod` и `Orders.DeliveryMethod` внутри заказов и отчетов.

## 7. Контроллеры MVP

### 7.1. Public

- `HomeController`
- `CatalogController`
- `ProductController`
- `AccountController`

### 7.2. Client

- `CartController`
- `CheckoutController`
- `ClientProfileController`
- `ClientOrdersController`
- `FavoritesController`
- `ReviewsController`

### 7.3. OrderManager

- `OrderManagerController`
- `OrderManagementController`

### 7.4. ContentManager

- `ContentManagerController`
- `ProductManagementController`
- `CategoryManagementController`

### 7.5. Admin

- `AdminController`
- `UserManagementController`
- `WarehouseController`
- `SupplierController`
- `SupplyRequestController`
- `ReportsController`

### 7.6. Контроллеры, Которые Не Создавать

- `EmployeeManagementController`
- `DiscountController`
- `PromoCodeController`
- `NotificationController`
- `PaymentSettingsController`
- `DeliverySettingsController`
- отдельный API-only controller для Minimal API

## 8. Entity Framework Models

Создавать только модели, соответствующие существующим таблицам:

- `User`
- `Category`
- `Product`
- `Cart`
- `CartItem`
- `Order`
- `OrderItem`
- `Favorite`
- `Review`
- `Supplier`
- `SupplyRequest`
- `SupplyRequestItem`

Не создавать:

- `Employee`
- `Discount`
- `PromoCode`
- `Coupon`
- `Notification`
- `OrderStatusHistory`
- `ProductImage`
- `Warehouse`
- `StockMovement`
- `PaymentSetting`
- `DeliverySetting`
- `Analytics`

## 9. ViewModels MVP

### 9.1. Auth / Public

- `LoginViewModel`
- `RegisterViewModel`
- `ProductCatalogViewModel`
- `ProductDetailsViewModel`
- `ProductFilterViewModel`
- `CategoryListViewModel`

### 9.2. Client

- `CartViewModel`
- `CartItemViewModel`
- `CheckoutViewModel`
- `DeliverySelectionViewModel`
- `PaymentSelectionViewModel`
- `OrderResultViewModel`
- `ClientDashboardViewModel`
- `ClientProfileViewModel`
- `EditProfileViewModel`
- `ClientOrderListViewModel`
- `ClientOrderDetailsViewModel`
- `OrderTrackingViewModel`
- `FavoriteListViewModel`
- `ReviewCreateViewModel`
- `ReviewListViewModel`

### 9.3. OrderManager

- `OrderManagerDashboardViewModel`
- `OrderQueueViewModel`
- `ManagerOrderDetailsViewModel`
- `OrderStatusUpdateViewModel`
- `OrderCancelViewModel`
- `ManagerOrderHistoryViewModel`

### 9.4. ContentManager

- `ContentManagerDashboardViewModel`
- `ProductManagementListViewModel`
- `ProductEditViewModel`
- `ProductPreviewViewModel`
- `ArchivedProductsViewModel`
- `CategoryEditViewModel`
- `ImageUploadViewModel`

### 9.5. Admin

- `AdminDashboardViewModel`
- `UserListViewModel`
- `UserEditRoleViewModel`
- `StaffListViewModel`
- `WarehouseViewModel`
- `StockAdjustmentViewModel`
- `SupplierListViewModel`
- `SupplierEditViewModel`
- `SupplyRequestListViewModel`
- `SupplyRequestDetailsViewModel`
- `SupplyRequestEditViewModel`
- `ReportsViewModel`

## 10. Services MVP

- `AuthService`
- `UserService`
- `CatalogService`
- `ProductService`
- `CategoryService`
- `CartService`
- `CheckoutService`
- `OrderService`
- `OrderStatusService`
- `FavoriteService`
- `ReviewService`
- `InventoryService`
- `SupplierService`
- `SupplyRequestService`
- `ImageUploadService`
- `ReportService`

Не создавать:

- `EmployeeService`
- `DiscountService`
- `PromoCodeService`
- `NotificationService`
- `PaymentSettingsService`
- `DeliverySettingsService`
- `OrderStatusHistoryService`
- `AnalyticsService`

## 11. Repositories MVP

Repository-слой не является обязательным для MVP.

Основной доступ к данным на первом этапе:

- `PerfumeStoreDbContext`;
- сервисы из Service Layer.

Репозитории допускаются только если на следующих этапах появится реальная необходимость отделить сложные запросы от сервисов.

Не создавать repository-слой формально, если он только дублирует `DbContext`.

## 12. Действия Контроллеров

### 12.1. `HomeController`

- `Index`

### 12.2. `CatalogController`

- `Index`
- `ByCategory`
- `Search`
- `Filter`

### 12.3. `ProductController`

- `Details`

### 12.4. `AccountController`

- `Login`
- `Register`
- `Logout`

### 12.5. `CartController`

- `Index`
- `Add`
- `UpdateQuantity`
- `Remove`
- `Clear`

Доступ: только `Client`.

### 12.6. `CheckoutController`

- `Index`
- `Delivery`
- `Payment`
- `Confirm`
- `Result`

Доступ: только `Client`.

### 12.7. `ClientProfileController`

- `Index`
- `Edit`

Доступ: только `Client`.

### 12.8. `ClientOrdersController`

- `Index`
- `Details`
- `Tracking`

Доступ: только `Client`, только свои заказы.

### 12.9. `FavoritesController`

- `Index`
- `Add`
- `Remove`

Доступ: только `Client`.

### 12.10. `ReviewsController`

- `Index`
- `Create`
- `Edit`

Доступ: только `Client`.

### 12.11. `OrderManagerController`

- `Index`

Доступ: `OrderManager`, `Admin`.

### 12.12. `OrderManagementController`

- `Queue`
- `Details`
- `UpdateStatus`
- `Cancel`
- `History`

Доступ: `OrderManager`, `Admin`.

### 12.13. `ContentManagerController`

- `Index`

Доступ: `ContentManager`, `Admin`.

### 12.14. `ProductManagementController`

- `Index`
- `Create`
- `Edit`
- `Archive`
- `Activate`
- `Preview`
- `UploadImage`

Доступ: `ContentManager`, `Admin`.

### 12.15. `CategoryManagementController`

- `Index`
- `Create`
- `Edit`

Доступ: `ContentManager`, `Admin`.

### 12.16. `AdminController`

- `Index`

Доступ: только `Admin`.

### 12.17. `UserManagementController`

- `Index`
- `Details`
- `EditRole`
- `Staff`

Доступ: только `Admin`.

`Staff` должен показывать пользователей с ролями:

- `Admin`;
- `OrderManager`;
- `ContentManager`.

### 12.18. `WarehouseController`

- `Index`
- `Adjust`

Доступ: только `Admin`.

Работает через `Products.StockQuantity`.

### 12.19. `SupplierController`

- `Index`
- `Details`
- `Create`
- `Edit`

Доступ: только `Admin`.

### 12.20. `SupplyRequestController`

- `Index`
- `Details`
- `Create`
- `Approve`
- `Reject`
- `Complete`

Доступ: только `Admin`.

### 12.21. `ReportsController`

- `Index`
- `Sales`
- `Warehouse`
- `Suppliers`

Доступ: только `Admin`.

## 13. Карта Маршрутов

### 13.1. Public

- `/`
- `/catalog`
- `/catalog/category/{id}`
- `/catalog/search`
- `/catalog/filter`
- `/product/{id}`
- `/account/login`
- `/account/register`
- `/account/logout`

### 13.2. Client

- `/cart`
- `/cart/add`
- `/cart/update`
- `/cart/remove`
- `/checkout`
- `/checkout/delivery`
- `/checkout/payment`
- `/checkout/confirm`
- `/checkout/result`
- `/client`
- `/client/profile`
- `/client/profile/edit`
- `/client/orders`
- `/client/orders/{id}`
- `/client/orders/{id}/tracking`
- `/client/favorites`
- `/client/reviews`

### 13.3. OrderManager

- `/order-manager`
- `/order-manager/orders`
- `/order-manager/orders/queue`
- `/order-manager/orders/{id}`
- `/order-manager/orders/{id}/status`
- `/order-manager/orders/{id}/cancel`
- `/order-manager/orders/history`

### 13.4. ContentManager

- `/content-manager`
- `/content-manager/products`
- `/content-manager/products/create`
- `/content-manager/products/{id}/edit`
- `/content-manager/products/{id}/preview`
- `/content-manager/products/{id}/archive`
- `/content-manager/products/{id}/activate`
- `/content-manager/products/upload-image`
- `/content-manager/categories`
- `/content-manager/categories/create`
- `/content-manager/categories/{id}/edit`

### 13.5. Admin

- `/admin`
- `/admin/users`
- `/admin/users/{id}`
- `/admin/users/{id}/role`
- `/admin/staff`
- `/admin/warehouse`
- `/admin/warehouse/adjust`
- `/admin/suppliers`
- `/admin/suppliers/{id}`
- `/admin/suppliers/create`
- `/admin/suppliers/{id}/edit`
- `/admin/supply-requests`
- `/admin/supply-requests/{id}`
- `/admin/supply-requests/create`
- `/admin/reports`
- `/admin/reports/sales`
- `/admin/reports/warehouse`
- `/admin/reports/suppliers`
- `/admin/analytics`

## 14. Соответствие БД И Макетов

### 14.1. Полностью Соответствует MVP

- Каталог товаров: `Products`, `Categories`.
- Карточка товара: `Products`.
- Категории: `Categories`.
- Корзина клиента: `Cart`, `CartItems`.
- Оформление заказа: `Orders`, `OrderItems`, `Cart`, `CartItems`.
- Доставка: `Orders.DeliveryMethod`.
- Оплата: `Orders.PaymentMethod`.
- История заказов клиента: `Orders`, `OrderItems`.
- Отслеживание заказа: `Orders.Status`.
- Избранное: `Favorites`.
- Отзывы: `Reviews`.
- Управление заказами: `Orders`, `OrderItems`.
- Управление товарами: `Products`.
- Архив товаров: `Products.IsActive`.
- Управление категориями: `Categories`.
- Загрузка изображения: `Products.ImageUrl`.
- Пользователи: `Users`.
- Сотрудники: `Users.Role`.
- Склад: `Products.StockQuantity`.
- Поставщики: `Suppliers`.
- Заявки поставщикам: `SupplyRequests`, `SupplyRequestItems`.
- Отчеты: `Orders`, `OrderItems`, `Products`, `Users`, `Suppliers`, `SupplyRequests`.

### 14.2. Требует Упрощения По БД

- Макеты настроек оплаты и доставки: реализовать не как отдельные настройки, а как справочную/отчетную информацию по фактически используемым `Orders.PaymentMethod` и `Orders.DeliveryMethod`, либо исключить из MVP.
- Макет сотрудников: реализовать через `Users` с ролями сотрудников.
- Макет склада: реализовать через `Products.StockQuantity`, без складских таблиц.
- Макет загрузки изображений: сохранять один путь в `Products.ImageUrl`.

### 14.3. Вне MVP

- Промокоды.
- Скидки.
- Купоны.
- Уведомления.
- Чат.
- Бонусная система.
- История изменения статусов.
- Wishlist вне `Favorites`.
- Несколько изображений товара.
- Отдельная система сотрудников.
- Отдельная система склада.
- Отдельные таблицы настроек.
- Таблицы аналитики.

## 15. Список Функций Вне MVP

Не реализовывать в первой версии:

- промокоды;
- скидки;
- купоны;
- уведомления;
- чат;
- бонусную систему;
- историю изменения статусов заказа;
- отдельную таблицу wishlist;
- несколько изображений товара;
- отдельную систему сотрудников;
- отдельную систему склада;
- отдельные таблицы настроек оплаты;
- отдельные таблицы настроек доставки;
- отдельные таблицы аналитики;
- review moderation;
- payment transaction history;
- saved payment cards;
- courier tracking;
- map tracking;
- stock movement history.

## 16. Потенциальные Проблемы Перед Разработкой

- Текущий `Source` является стартовым MVC-шаблоном без EF Core, `DbContext`, доменных моделей, сервисов и репозиториев.
- В `Program.cs` есть `UseAuthorization`, но отсутствует `UseAuthentication`.
- `02_TestData.sql` содержит пароли в виде простых строк; приложение должно работать с `PasswordHash`.
- Макеты содержат функции, которых нет в БД; такие функции должны быть исключены из MVP.
- Макеты OrderManager и ContentManager смешивают зоны ответственности; приоритет имеет финальная спецификация.
- Guest-макеты включают корзину, но Guest по спецификации не может использовать корзину.
- Admin-макеты настроек оплаты/доставки не могут быть реализованы как отдельный CRUD без изменения БД.
- В SQL нет истории статусов заказа; использовать только текущее поле `Orders.Status`.
- В SQL нет отдельной таблицы изображений; использовать только `Products.ImageUrl`.
- В SQL нет отдельных сотрудников; использовать `Users.Role`.
- В SQL нет таблиц скидок и уведомлений; соответствующие Figma-экраны вне MVP.
- При удалении пользователя SQL каскадно удаляет его заказы, что потенциально опасно для аудита.
- При удалении категории с товарами, участвующими в заказах, возможны конфликты ссылочной целостности.
- Нет явных индексов на внешние ключи.
- Нет миграций EF Core; схема задана SQL-скриптом.

## 17. Итоговая Архитектурная Граница MVP

MVP должен реализовать только функциональность, которая укладывается в существующие 12 таблиц БД.

Основные модули MVP:

- публичная витрина;
- авторизация;
- клиентская корзина;
- оформление заказа;
- личный кабинет клиента;
- избранное;
- отзывы;
- управление заказами для `OrderManager`;
- управление каталогом для `ContentManager`;
- управление пользователями, складом, поставщиками, заявками и отчетами для `Admin`.

Любое расширение БД, новая сущность или новый бизнес-процесс требуют отдельного согласования до реализации.

