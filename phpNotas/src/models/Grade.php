<?php
class Grade {
    private $conn;
    private $table = "grades";

    public $id;
    public $student_id;
    public $subject_id;
    public $grade;
    public $created_at;

    public function __construct($db) {
        $this->conn = $db;
    }

    // Registrar nota
    public function create() {
        $query = "INSERT INTO " . $this->table . " (student_id, subject_id, grade) 
                  VALUES (:student_id, :subject_id, :grade)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":student_id", $this->student_id);
        $stmt->bindParam(":subject_id", $this->subject_id);
        $stmt->bindParam(":grade", $this->grade);
        return $stmt->execute();
    }

    // Listar todas las notas de un estudiante
    public function getByStudent($student_id) {
        $query = "SELECT g.id, g.grade, g.created_at, s.name as subject 
                  FROM " . $this->table . " g
                  JOIN subjects s ON g.subject_id = s.id
                  WHERE g.student_id = :student_id";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":student_id", $student_id);
        $stmt->execute();
        return $stmt;
    }

    // Obtener promedio de un estudiante
    public function getAverageByStudent($student_id) {
        $query = "SELECT AVG(grade) as average 
                  FROM " . $this->table . " 
                  WHERE student_id = :student_id";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":student_id", $student_id);
        $stmt->execute();
        return $stmt->fetch(PDO::FETCH_ASSOC);
    }

    // Obtener notas de todos los estudiantes en una materia
    public function getBySubject($subject_id) {
        $query = "SELECT g.id, g.grade, g.created_at, st.name as student 
                  FROM " . $this->table . " g
                  JOIN students st ON g.student_id = st.id
                  WHERE g.subject_id = :subject_id";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(":subject_id", $subject_id);
        $stmt->execute();
        return $stmt;
    }
}
