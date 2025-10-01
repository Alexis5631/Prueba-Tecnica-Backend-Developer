<?php
class Student {
    private $conn;
    private $table = "students";

    public $id;
    public $name;
    public $registration;

    public function __construct($db) {
        $this->conn = $db;
    }

    // Crear estudiante
    public function create() {
        $query = "INSERT INTO " . $this->table . " (name, registration) VALUES (:name, :registration)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":name", $this->name);
        $stmt->bindParam(":registration", $this->registration);
        return $stmt->execute();
    }

    // Listar todos los estudiantes
    public function readAll() {
        $query = "SELECT * FROM " . $this->table;
        $stmt = $this->conn->prepare($query);
        $stmt->execute();
        return $stmt;
    }
}
