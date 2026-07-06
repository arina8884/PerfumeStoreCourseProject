# Изображения товаров

Все фотографии парфюмерии хранятся **только** в этой папке.

## Правила

| Параметр | Значение |
|----------|----------|
| Поле в БД | `Products.ImageUrl` |
| Формат пути | `/images/products/имя-файла.webp` |
| Допустимые форматы | `.webp`, `.jpg`, `.jpeg`, `.png` |
| Заглушка | `/images/products/no-image.png` (при отсутствии файла или пустом `ImageUrl`) |

## Пример

| Товар | ImageUrl в БД | Файл на диске |
|-------|---------------|---------------|
| Dior Sauvage EDT 100 мл | `/images/products/dior-sauvage-edt.webp` | `wwwroot/images/products/dior-sauvage-edt.webp` |

## Добавление фото

1. Скачайте изображение вручную (без автозагрузки из интернета в репозиторий).
2. Сохраните файл в эту папку с именем из `Database/IMAGE_MANIFEST.md`.
3. Убедитесь, что `Products.ImageUrl` совпадает с путём `/images/products/<имя-файла>`.

Полный список файлов для скачивания — в `Database/IMAGE_MANIFEST.md`.
