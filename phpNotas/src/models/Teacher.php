<?php
class Teacher {
    private $conn;
    private $table = "teachers";

    public $id;
    public $name;

    public function __construct($db) {
        $this->conn = $db;
    }

    // Crear profesor
    public function create() {
        $query = "INSERT INTO " . $this->table . " (name) VALUES (:name)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":name", $this->name);
        return $stmt->execute();
    }

    // Listar todos los profesores
    public function readAll() {
        $query = "SELECT * FROM " . $this->table;
        $stmt = $this->conn->prepare($query);
        $stmt->execute();
        return $stmt;
    }
}
