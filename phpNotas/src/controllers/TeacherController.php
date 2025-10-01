<?php

require_once __DIR__ . '/../models/Teacher.php';

class TeacherController {
    private $db;
    private $teacher;

    public function __construct($db) {
        $this->db = $db;
        $this->teacher = new Teacher($this->db);
    }

    // POST /teachers
    public function create($data) {
        $this->teacher->name = $data->name ?? null;

        if ($this->teacher->create()) {
            http_response_code(201);
            echo json_encode(["message" => "Profesor creado correctamente"]);
        } else {
            http_response_code(503);
            echo json_encode(["message" => "No se pudo crear el profesor"]);
        }
    }

    // GET /teachers
    public function getAll() {
        $stmt = $this->teacher->readAll();
        $teachers = $stmt->fetchAll(PDO::FETCH_ASSOC);
        
        http_response_code(200);
        echo json_encode($teachers);
    }
}