<?php
// src/controllers/SubjectController.php

require_once __DIR__ . "/../models/Subject.php";

class SubjectController {
    private $db;
    private $subject;

    public function __construct($db) {
        $this->db = $db;
        $this->subject = new Subject($this->db);
    }

    // POST /subjects - Crear materia
    public function create($data) {
        // Validar datos
        if (empty($data->name) || empty($data->teacher_id)) {
            http_response_code(400);
            echo json_encode(["message" => "Faltan datos: name y teacher_id son requeridos"]);
            return;
        }

        $this->subject->name = $data->name;
        $this->subject->teacher_id = $data->teacher_id;

        if ($this->subject->create()) {
            http_response_code(201);
            echo json_encode(["message" => "Materia creada correctamente"]);
        } else {
            http_response_code(400);
            echo json_encode(["message" => "Error al crear materia"]);
        }
    }

    // GET /subjects - Listar todas las materias con profesores
    public function getAll() {
        $stmt = $this->subject->readAll();
        $subjects = $stmt->fetchAll(PDO::FETCH_ASSOC);

        http_response_code(200);
        echo json_encode($subjects);
    }

}