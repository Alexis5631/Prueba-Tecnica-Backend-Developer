# 📚 Sistema de Notas (PHP + MySQL/Postgres)

Una API sencilla para gestionar **estudiantes, profesores, materias y calificaciones**, desarrollada en PHP usando PDO y MySQL

---

## ⚙️ Requerimientos

- PHP >= 7.4  
- MySQL o PostgreSQL  
- Extensión PDO habilitada  

---

## 🛠️ Instalación

1. **Crear la base de datos**:  

   ```bash
   mysql -u root -p notas_db < sql/tables.sql

2. **Configurar credenciales en**:

```bash
src/config/database.php
```

3. **Levantar el servidor**:

```bash
php -S localhost:8000
```

4. **Probar los endpoints usando Swagger**

```bash
http://localhost:8000/swagger/index.html
```