-- Script para verificar la estructura de la base de datos
-- Ejecutar este script para verificar que todas las tablas existan con la estructura correcta

-- Conectar a la base de datos inventory
\c inventory;

-- Verificar que todas las tablas existen
SELECT 'Verificando existencia de tablas...' as mensaje;

SELECT 
    table_name,
    CASE 
        WHEN table_name IN ('users', 'roles', 'products', 'orders', 'order_items', 'refresh_tokens')
        THEN '✅ Existe'
        ELSE '❌ No existe'
    END as status
FROM information_schema.tables 
WHERE table_schema = 'public' 
AND table_type = 'BASE TABLE'
ORDER BY table_name;

-- Verificar estructura de la tabla users
SELECT 'Verificando estructura de tabla users...' as mensaje;
SELECT column_name, data_type, is_nullable, column_default
FROM information_schema.columns 
WHERE table_name = 'users' 
ORDER BY ordinal_position;

-- Verificar estructura de la tabla order_items
SELECT 'Verificando estructura de tabla order_items...' as mensaje;
SELECT column_name, data_type, is_nullable, column_default
FROM information_schema.columns 
WHERE table_name = 'order_items' 
ORDER BY ordinal_position;

-- Verificar estructura de la tabla refresh_tokens
SELECT 'Verificando estructura de tabla refresh_tokens...' as mensaje;
SELECT column_name, data_type, is_nullable, column_default
FROM information_schema.columns 
WHERE table_name = 'refresh_tokens' 
ORDER BY ordinal_position;

-- Verificar datos de roles
SELECT 'Verificando datos de roles...' as mensaje;
SELECT id, name FROM roles ORDER BY id;

-- Verificar usuarios existentes
SELECT 'Verificando usuarios existentes...' as mensaje;
SELECT 
    u.id,
    u.username,
    u.role_id,
    r.name as role_name,
    u.is_active
FROM users u
LEFT JOIN roles r ON u.role_id = r.id
ORDER BY u.id;
