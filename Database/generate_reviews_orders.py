# -*- coding: utf-8 -*-
from pathlib import Path

reviews = [
    ("anna.koval@mail.by", "Sauvage Eau de Toilette", "Dior", 100, 5, "Беру уже третий флакон — на работе все спрашивают, что за аромат. Стойкость на коже около шести часов."),
    ("dmitry.sidorov@gmail.com", "N°5 Eau de Parfum", "Chanel", 50, 4, "Классика, которую сложно не узнать. На вечернее мероприятие села идеально, хотя для офиса многовато."),
    ("elena.moroz@mail.by", "Black Opium", "Yves Saint Laurent", 50, 5, "Ваниль с кофе звучит громко, но на коже раскрывается мягко. Зимой ношу почти каждый день."),
    ("igor.petrov@yandex.by", "Aventus", "Creed", 50, 5, "Дорого, но оправдано: ананас чувствуется с первых секунд, а шлейф держится до вечера."),
    ("maria.voytovich@mail.by", "Libre Eau de Parfum", "Yves Saint Laurent", 50, 4, "Лаванда здесь не лекарственная, а свежая. Коллеги отметили, что аромат запоминающийся."),
    ("olga.lebedeva@gmail.com", "Coco Mademoiselle", "Chanel", 50, 5, "Подарила себе на день рождения — ни разу не пожалела. Цитрус в начале, потом тёплый жасмин."),
    ("sergey.novik@mail.by", "Bleu de Chanel Eau de Parfum", "Chanel", 100, 4, "Универсальный вариант: и на свидание, и на деловую встречу. Флакон 100 мл хватает надолго."),
    ("natalya.zhukova@mail.by", "J'adore Eau de Parfum", "Dior", 50, 5, "Цветочный, но не приторный. Муж сказал, что пахну «как весной в саду» — для меня лучший комплимент."),
    ("pavel.kravets@gmail.com", "Acqua di Gio Profondo", "Giorgio Armani", 100, 4, "Морской акцент без мыла — редкость. Летом на замену тяжёлым духам."),
    ("tatiana.sokol@yandex.by", "Good Girl", "Carolina Herrera", 50, 3, "Красивый флакон, аромат сладковатый. На мне держится 3–4 часа, потом нужно обновить."),
    ("andrey.volkov@mail.by", "Eros", "Versace", 100, 4, "Мятная нота в начале бодрит, потом уходит в ваниль. Молодёжный, но не дешёвый по ощущению."),
    ("ksenia.orlova@gmail.com", "Baccarat Rouge 540", "Maison Francis Kurkdjian", 70, 5, "Шафран и кедр — сочетание необычное. На мне шлейф слышен даже на следующий день на одежде."),
    ("anna.koval@mail.by", "Gypsy Water", "Byredo", 50, 4, "Лёгкий, почти прозрачный. Подходит, когда не хочется «кричать» ароматом."),
    ("dmitry.sidorov@gmail.com", "Oud Wood", "Tom Ford", 50, 5, "Уд без больничного запаха — сбалансированный, с ванилью. Для особых случаев."),
    ("elena.moroz@mail.by", "Bright Crystal", "Versace", 90, 4, "Свежий, водянистый, для жаркого лета самое то. Расходуется быстро — наношу щедро."),
    ("igor.petrov@yandex.by", "1 Million", "Paco Rabanne", 100, 3, "Яркий, заметный, но на мне немного приторный к концу дня. Зато комплименты гарантированы."),
    ("maria.voytovich@mail.by", "Miss Dior Eau de Parfum", "Dior", 50, 5, "Роза и пион — романтично, но современно. Ношу с платьями и с джинсами одинаково хорошо."),
    ("olga.lebedeva@gmail.com", "La Nuit de L'Homme", "Yves Saint Laurent", 60, 4, "Подарила мужу — теперь сам просит докупить. Кардамон чувствуется, но не доминирует."),
    ("sergey.novik@mail.by", "Stronger With You", "Giorgio Armani", 100, 4, "Сладковатый, уютный, как вечер у камина. Зимой — мой основной."),
    ("natalya.zhukova@mail.by", "Chance Eau Tendre", "Chanel", 50, 5, "Нежный, почти невесомый. Для офиса идеален — никого не раздражает."),
    ("pavel.kravets@gmail.com", "Fahrenheit", "Dior", 100, 4, "Старый знакомый аромат, но качество стабильное. Кожа и фиалка — узнаваемый почерк Dior."),
    ("tatiana.sokol@yandex.by", "Donna Born In Roma", "Valentino", 50, 4, "Ваниль с жасмином — тёплый, женственный. Флакон красиво смотрится на полке."),
    ("andrey.volkov@mail.by", "Invictus", "Paco Rabanne", 100, 3, "Спортивный характер, на тренировку не пойдёт, но после душа — самое то."),
    ("ksenia.orlova@gmail.com", "Lost Cherry", "Tom Ford", 50, 5, "Вишня и миндаль — гурманский рай. Дорого, но эмоции стоят того."),
    ("anna.koval@mail.by", "English Pear & Freesia", "Jo Malone", 100, 4, "Лёгкий, освежающий. Сам по себе держится недолго, зато сочетается с другими ароматами."),
    ("dmitry.sidorov@gmail.com", "Sauvage Elixir", "Dior", 60, 5, "Концентрация выше EDT — разница ощутима с первого распыления. Шлейф плотный."),
    ("elena.moroz@mail.by", "Prada L'Homme", "Prada", 100, 4, "Чистый, мыльно-ирисовый, деловой. Под костюм садится безупречно."),
    ("igor.petrov@yandex.by", "Red Tobacco", "Mancera", 120, 4, "Табак и корица — на любителя. Мне зашло, жене показалось слишком тяжёлым."),
    ("maria.voytovich@mail.by", "Intense Cafe", "Montale", 100, 5, "Пахнет как кофе с розой в хорошей кондитерской. Зимой ношу постоянно."),
    ("olga.lebedeva@gmail.com", "Naxos", "Xerjoff", 100, 5, "Мёд и лаванда — неожиданный дуэт. Один из самых сложных ароматов в коллекции."),
    ("sergey.novik@mail.by", "Black Phantom", "Kilian", 50, 4, "Тёмный, гурманский, с ромом. На вечеринку — да, на работу — нет."),
    ("natalya.zhukova@mail.by", "Silver Mountain Water", "Creed", 50, 4, "Альпийская свежесть, без химозного «спорта». Летом заменил мне одеколон."),
    ("pavel.kravets@gmail.com", "Bal d'Afrique", "Byredo", 50, 5, "Солнечный, с лёгкой сладостью. Напоминает отпуск — в хорошем смысле."),
    ("tatiana.sokol@yandex.by", "Aqua Universalis", "Maison Francis Kurkdjian", 70, 4, "Универсальный, чистый. Когда не знаю, что надеть из парфюма — беру его."),
    ("andrey.volkov@mail.by", "Y Eau de Parfum", "Yves Saint Laurent", 60, 4, "Яблоко и шалфей — свежо, по-молодому. Жене тоже понравился, носим по очереди."),
    ("ksenia.orlova@gmail.com", "Sauvage Eau de Parfum", "Dior", 60, 5, "Плотнее EDT, шлейф заметнее. На свидание выбрала именно эту версию."),
    ("anna.koval@mail.by", "Coco Mademoiselle", "Chanel", 100, 4, "Большой флакон выгоднее — пользуюсь второй год, запах не надоел."),
    ("dmitry.sidorov@gmail.com", "Bleu de Chanel Eau de Parfum", "Chanel", 50, 5, "Купил после теста в магазине — на коже раскрывается иначе, чем на блоттере."),
    ("elena.moroz@mail.by", "J'adore Eau de Parfum", "Dior", 100, 5, "Мама носила J'adore, я продолжаю традицию. Качество стабильное."),
    ("igor.petrov@yandex.by", "Eros", "Versace", 50, 3, "Сладковатый, молодёжный. В 35 уже чувствую себя немного старше целевой аудитории."),
    ("maria.voytovich@mail.by", "Good Girl", "Carolina Herrera", 80, 4, "Формат 80 мл удобен в сумке. Аромат вечерний, днём не использую."),
    ("olga.lebedeva@gmail.com", "Black Opium", "Yves Saint Laurent", 30, 5, "Мини-формат для пробы — теперь закажу полный. Кофе и ваниль — моё."),
    ("sergey.novik@mail.by", "Aventus", "Creed", 100, 4, "Легенда оправдана, хотя цена кусается. 100 мл — инвестиция на полгода."),
    ("natalya.zhukova@mail.by", "Libre Eau de Parfum", "Yves Saint Laurent", 90, 4, "Формат 90 мл — хорошая стойкость. Лаванда не доминирует, баланс удачный."),
    ("pavel.kravets@gmail.com", "Oud Wood", "Tom Ford", 100, 5, "Уд мягкий, кремовый. Не пахну мечетью, как некоторые боятся — всё цивилизованно."),
    ("olga.lebedeva@gmail.com", "Shalimar", "Guerlain", 50, 5, "Классика Guerlain — ваниль и ирис на мне звучат благородно, без приторности."),
    ("sergey.novik@mail.by", "Wood Sage & Sea Salt", "Jo Malone", 100, 4, "Беру на лето: лёгкий, морской, не перегружает. С коллегами в лифте комфортно."),
    ("natalya.zhukova@mail.by", "Mon Guerlain", "Guerlain", 100, 4, "Лаванда здесь мягкая, почти пудровая. На свидание села лучше, чем ожидала."),
    ("andrey.volkov@mail.by", "Luna Rossa Carbon", "Prada", 100, 4, "Современный, чуть металлический старт — потом тёплый амбровый шлейф. На каждый день."),
]

orders = [
    ("anna.koval@mail.by", -45, 258, "г. Минск, ул. Немига, 12, кв. 45", "Карта", "Курьер", "Delivered", [("Sauvage Eau de Toilette", "Dior", 100, 1), ("English Pear & Freesia", "Jo Malone", 100, 1)]),
    ("dmitry.sidorov@gmail.com", -30, 328, "г. Минск, пр-т Победителей, 84", "Карта", "Самовывоз", "Delivered", [("N°5 Eau de Parfum", "Chanel", 50, 1), ("Bleu de Chanel Eau de Parfum", "Chanel", 50, 1)]),
    ("elena.moroz@mail.by", -14, 349, "г. Гродно, ул. Советская, 5", "Карта", "Почта", "OnTheWay", [("Aventus", "Creed", 50, 1)]),
    ("igor.petrov@yandex.by", -7, 199, "г. Брест, ул. Машерова, 17", "Наличные", "Курьер", "Assembling", [("N°5 Eau de Parfum", "Chanel", 35, 1)]),
    ("maria.voytovich@mail.by", -3, 129, "г. Минск, ул. Кальварийская, 22", "Карта", "Курьер", "New", [("Sauvage Eau de Toilette", "Dior", 60, 1)]),
    ("olga.lebedeva@gmail.com", -60, 648, "г. Витебск, пр-т Фрунзе, 3", "Карта", "Почта", "Delivered", [("Baccarat Rouge 540", "Maison Francis Kurkdjian", 70, 1), ("Lost Cherry", "Tom Ford", 50, 1)]),
    ("sergey.novik@mail.by", -21, 129, "г. Минск, ул. Притыцкого, 156", "Карта", "Самовывоз", "Delivered", [("Stronger With You", "Giorgio Armani", 100, 1)]),
    ("natalya.zhukova@mail.by", -10, 159, "г. Гомель, ул. Советская, 28", "Карта", "Курьер", "OnTheWay", [("Chance Eau Tendre", "Chanel", 50, 1)]),
    ("pavel.kravets@gmail.com", -5, 299, "г. Минск, ул. Независимости, 95", "Карта", "Курьер", "Assembling", [("Oud Wood", "Tom Ford", 50, 1)]),
    ("tatiana.sokol@yandex.by", -18, 199, "г. Могилёв, ул. Ленинская, 7", "Наличные", "Почта", "Delivered", [("Donna Born In Roma", "Valentino", 50, 1)]),
    ("andrey.volkov@mail.by", -2, 129, "г. Минск, ул. Тимирязева, 64", "Карта", "Курьер", "New", [("Invictus", "Paco Rabanne", 100, 1)]),
    ("ksenia.orlova@gmail.com", -35, 498, "г. Минск, ул. Якубовского, 33", "Карта", "Курьер", "Delivered", [("Gypsy Water", "Byredo", 50, 1), ("Bal d'Afrique", "Byredo", 50, 1)]),
    ("anna.koval@mail.by", -90, 199, "г. Минск, ул. Немига, 12, кв. 45", "Карта", "Почта", "Delivered", [("Miss Dior Eau de Parfum", "Dior", 50, 1)]),
    ("dmitry.sidorov@gmail.com", -1, 199, "г. Минск, пр-т Победителей, 84", "Карта", "Самовывоз", "Canceled", [("Sauvage Elixir", "Dior", 60, 1)]),
    ("elena.moroz@mail.by", -25, 129, "г. Гродно, ул. Советская, 5", "Наличные", "Курьер", "Delivered", [("Black Opium", "Yves Saint Laurent", 50, 1)]),
    ("igor.petrov@yandex.by", -12, 199, "г. Брест, ул. Машерова, 17", "Карта", "Почта", "OnTheWay", [("Red Tobacco", "Mancera", 120, 1)]),
    ("maria.voytovich@mail.by", -40, 199, "г. Минск, ул. Кальварийская, 22", "Карта", "Курьер", "Delivered", [("Intense Cafe", "Montale", 100, 1)]),
    ("olga.lebedeva@gmail.com", -8, 199, "г. Витебск, пр-т Фрунзе, 3", "Карта", "Самовывоз", "Assembling", [("Coco Mademoiselle", "Chanel", 50, 1)]),
    ("sergey.novik@mail.by", -55, 258, "г. Минск, ул. Притыцкого, 156", "Карта", "Почта", "Delivered", [("Fahrenheit", "Dior", 100, 1), ("Acqua di Gio Profondo", "Giorgio Armani", 100, 1)]),
    ("natalya.zhukova@mail.by", -4, 99, "г. Гомель, ул. Советская, 28", "Наличные", "Курьер", "New", [("Bright Crystal", "Versace", 90, 1)]),
    ("pavel.kravets@gmail.com", -28, 389, "г. Минск, ул. Независимости, 95", "Карта", "Курьер", "Delivered", [("Aventus", "Creed", 100, 1)]),
    ("tatiana.sokol@yandex.by", -15, 299, "г. Могилёв, ул. Ленинская, 7", "Карта", "Почта", "OnTheWay", [("Naxos", "Xerjoff", 100, 1)]),
]


def esc(s: str) -> str:
    return s.replace("'", "''")


lines = []
for i, (email, name, brand, vol, rating, comment) in enumerate(reviews):
    prefix = "INSERT INTO Reviews (UserId, ProductId, Rating, Comment)\nSELECT" if i == 0 else "INSERT INTO Reviews (UserId, ProductId, Rating, Comment)\nSELECT"
    lines.append(prefix)
    lines.append(
        f" u.Id, p.Id, {rating}, N'{esc(comment)}' "
        f"FROM Users u JOIN Products p ON p.Name = N'{esc(name)}' AND p.Brand = N'{esc(brand)}' AND p.Volume = {vol} "
        f"WHERE u.Email = N'{email}';"
    )
lines.append("GO")
lines.append("")

for email, days, total, addr, pay, deliv, status, items in orders:
    lines.append("INSERT INTO Orders (UserId, OrderDate, TotalAmount, DeliveryAddress, PaymentMethod, DeliveryMethod, Status)")
    lines.append(
        f"VALUES ((SELECT Id FROM Users WHERE Email=N'{email}'), DATEADD(day,{days},GETDATE()), {total}.00, "
        f"N'{esc(addr)}', N'{pay}', N'{deliv}', N'{status}');"
    )
    for name, brand, vol, qty in items:
        lines.append("INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)")
        lines.append(
            f"SELECT (SELECT MAX(Id) FROM Orders), p.Id, {qty}, p.Price FROM Products p "
            f"WHERE p.Name=N'{esc(name)}' AND p.Brand=N'{esc(brand)}' AND p.Volume={vol};"
        )
lines.append("GO")

fragment = "\n".join(lines)
main = Path(__file__).parent / "02_TestData.sql"
text = main.read_text(encoding="utf-8")
start = text.index("INSERT INTO Reviews (UserId, ProductId, Rating, Comment)")
end = text.index("INSERT INTO SupplyRequests")
main.write_text(text[:start] + fragment + "\n\n" + text[end:], encoding="utf-8")
print(f"Merged {len(reviews)} reviews, {len(orders)} orders")
