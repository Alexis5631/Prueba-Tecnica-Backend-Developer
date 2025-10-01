<?php
require_once __DIR__ . "/../models/Grade.php";

class GradeController {
    private $db;

    public function __construct($db) {
        $this->db = $db;
    }

    // POST /grades
    public function create($data) {
        $grade = new Grade($this->db);
        $grade->student_id = $data->student_id ?? null;
        $grade->subject_id = $data->subject_id ?? null;
        $grade->grade = $data->grade ?? null;

        if ($grade->create()) {
            http_response_code(201);
            echo json_encode(["message" => "Nota registrada"]);
        } else {
            http_response_code(400);
            echo json_encode(["message" => "Error al registrar nota"]);
        }
    }

    // GET /grades/student/{id}
    public function getByStudent($student_id) {
        $grade = new Grade($this->db);
        $stmt = $grade->getByStudent($student_id);
        $grades = $stmt->fetchAll(PDO::FETCH_ASSOC);

        http_response_code(200);
        echo json_encode($grades);
    }

    // GET /grades/average?student_id={id}
    public function getAverage($student_id) {
        $grade = new Grade($this->db);
        $result = $grade->getAverageByStudent($student_id);

        http_response_code(200);
        echo json_encode($result);
    }

    // GET /grades/subject/{id}
    public function getBySubject($subject_id) {
        $grade = new Grade($this->db);
        $stmt = $grade->getBySubject($subject_id);
        $grades = $stmt->fetchAll(PDO::FETCH_ASSOC);

        http_response_code(200);
        echo json_encode($grades);
    }
}
