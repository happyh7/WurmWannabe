<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $page = $_POST['page'];
    $content = $_POST['content'];

    // Spara innehållet till den valda sidan
    file_put_contents($page, $content);

    // Omdirigera tillbaka till dashboard
    header('Location: dashboard.html');
    exit;
}
?> 