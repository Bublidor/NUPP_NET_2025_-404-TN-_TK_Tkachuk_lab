-- SQL скрипт для налаштування бази даних ZooShop в PostgreSQL
-- Виконайте цей скрипт як суперкористувач PostgreSQL

-- Видаляємо існуючу базу, якщо є (для чистого старту)
DROP DATABASE IF EXISTS "ZooShopDB";

-- Видаляємо існуючого користувача, якщо є
DROP USER IF EXISTS zooshop;

-- Створюємо нового користувача
CREATE USER zooshop WITH PASSWORD 'zooshop123';

-- Створюємо базу даних з власником zooshop
CREATE DATABASE "ZooShopDB" 
    WITH OWNER = zooshop
    ENCODING = 'UTF8'
    LC_COLLATE = 'uk_UA.UTF-8'
    LC_CTYPE = 'uk_UA.UTF-8'
    TEMPLATE = template0;

-- Підключаємося до бази даних
\c "ZooShopDB"

-- Надаємо всі права на схему public
GRANT ALL ON SCHEMA public TO zooshop;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO zooshop;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO zooshop;

-- Встановлюємо права за замовчуванням для майбутніх об'єктів
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO zooshop;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO zooshop;

-- Перевірка
SELECT current_database(), current_user;

