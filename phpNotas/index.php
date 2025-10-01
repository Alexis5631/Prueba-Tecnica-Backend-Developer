<?php
require_once __DIR__ . '/src/config/database.php';
require_once __DIR__ . '/src/controllers/StudentController.php';
require_once __DIR__ . '/src/controllers/TeacherController.php';
require_once __DIR__ . '/src/controllers/GradeController.php';
require_once __DIR__ . '/src/controllers/SubjectController.php';

header("Content-Type: application/json");

$database = new Database();
$db = $database->getConnection();

$uri = explode("/", trim(parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH), "/"));
$method = $_SERVER['REQUEST_METHOD'];

// Instancias de controladores
$studentController = new StudentController($db);
$teacherController = new TeacherController($db);
$gradeController   = new GradeController($db);
$subjectController = new SubjectController($db);

// RUTAS ---------------------

// Students
if ($uri[0] == "students") {
    if ($method == "POST") {
        $data = json_decode(file_get_contents("php://input"));
        $studentController->create($data);
    } elseif ($method == "GET") {
        $studentController->getAll();
    }
    
}

// Teachers
if ($uri[0] == "teachers") {
    if ($method == "POST") {
        $data = json_decode(file_get_contents("php://input"));
        $teacherController->create($data);
    } elseif ($method == "GET") {
        $teacherController->getAll();
    }
}

// Grades
if ($uri[0] == "grades") {
    if ($method == "POST") {
        $data = json_decode(file_get_contents("php://input"));
        $gradeController->create($data);
    } elseif ($method == "GET" && $uri[1] == "student") {
        $gradeController->getByStudent($uri[2]);
    } elseif ($method == "GET" && $uri[1] == "average") {
        $gradeController->getAverage($_GET['student_id']);
    } elseif ($method == "GET" && $uri[1] == "subject") {
        $gradeController->getBySubject($uri[2]);
    }

// Subjects
if ($uri[0] == "subjects") {
    if ($method == "POST") {
        $data = json_decode(file_get_contents("php://input"));
        $subjectController->create($data);
    } elseif ($method == "GET") {
        $subjectController->getAllWithTeachers();
    }
}
}


