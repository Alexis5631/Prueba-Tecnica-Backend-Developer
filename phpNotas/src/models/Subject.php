<?php

class Subject {
    private $conn;
    private $table = "subjects";

    public $id;
    public $name;
    public $teacher_id;

    public function __construct($db) {
        $this->conn = $db;
    }

    // Crear materia
    public function create() {
        $query = "INSERT INTO " . $this->table . " (name, teacher_id) 
                  VALUES (:name, :teacher_id)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":name", $this->name);
        $stmt->bindParam(":teacher_id", $this->teacher_id);
        return $stmt->execute();
    }

    // Listar todas las materias con nombre del profesor
    public function readAll() {
        $query = "SELECT s.id, s.name, s.teacher_id, t.name as teacher_name 
                  FROM " . $this->table . " s
                  LEFT JOIN teachers t ON s.teacher_id = t.id";
        $stmt = $this->conn->prepare($query);
        $stmt->execute();
        return $stmt;
    }

}