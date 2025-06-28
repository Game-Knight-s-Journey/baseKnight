const express = require('express');
const app = express();
const port = 3000;

app.use(express.json()); // Để đọc dữ liệu JSON từ client

app.get('/', (req, res) => {
    res.send('Hello World!');
});

// API nhận điểm từ client
app.post('/api/score', (req, res) => {
    const { playerId, score } = req.body;
    if (!playerId || score === undefined) {
        return res.status(400).json({ message: 'Missing playerId or score' });
    }
    // Xử lý lưu điểm ở đây (ví dụ lưu vào biến hoặc database)
    res.json({ message: 'Score received', playerId, score });
});

app.listen(port, () => {
    console.log(`Server running at http://localhost:${port}`);
});
