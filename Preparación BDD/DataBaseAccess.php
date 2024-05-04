<?php
$servername = "your_server";
$username = "your_username";
$password = "your_password";
$dbname = "Stray_Bullet_DB";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Get the posted data
$postdata = file_get_contents("php://input");
$request = json_decode($postdata);

$registerUser = $conn->real_escape_string($request->user);
$registerPass = $conn->real_escape_string($request->pass);

// Check if user already exists
$check = $conn->prepare("SELECT username FROM users WHERE username=?");
$check->bind_param("s", $registerUser);
$check->execute();
$check->store_result();

if ($check->num_rows > 0) {
    echo "User already exists";
} else {
    // Prepare and bind
    $stmt = $conn->prepare("INSERT INTO users (username, password) VALUES (?, ?)");
    $stmt->bind_param("ss", $registerUser, $registerPass);

    // Set parameters and execute
    if ($stmt->execute()) {
        echo "Registration successful";
    } else {
        echo "Registration failed";
    }

    $stmt->close();
}

$check->close();
$conn->close();
?>
