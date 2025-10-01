<?php

require_once __DIR__ . "/../models/Student.php";

class StudentController {
    private $db;

    public function __construct($db) {
        $this->db = $db;
    }

    // POST /students
    public function create($data) {
        $student = new Student($this->db);
        $student->name = $data->name ?? null;
        $student->registration = $data->registration ?? null;

        if ($student->create()) {
            http_response_code(201);
            echo json_encode(["message" => "Estudiante creado"]);
        } else {
            http_response_code(400);
            echo json_encode(["message" => "Error al crear estudiante"]);
        }
    }

    // GET /students
    public function getAll() {
        $student = new Student($this->db);
        $stmt = $student->readAll();
        $students = $stmt->fetchAll(PDO::FETCH_ASSOC);

        http_response_code(200);
        echo json_encode($students);
    }
}