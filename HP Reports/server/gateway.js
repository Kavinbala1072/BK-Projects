const express = require('express');
const sql = require('mssql');
const cors = require('cors');
const fs = require('fs');

const app = express();
app.use(express.json());
app.use(cors());

const loginConfig = JSON.parse(fs.readFileSync('./config/login.json', 'utf8'));

app.post('/api/login', (req, res) => {
    const { username, password } = req.body;
    if (username === loginConfig.username && password === loginConfig.password) {
        res.json({ success: true, token: "mock-jwt-token" });
    } else {
        res.status(401).json({ error: "Invalid credentials" });
    }
});

app.post('/api/query', async (req, res) => {
    const { connectionString, query, params } = req.body;
    try {
        let pool = await sql.connect(connectionString);
        let request = pool.request();
        
        if (params) {
            params.forEach(p => request.input(p.name, p.value));
        }
        
        let result = await request.query(query);
        res.json(result.recordset);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

app.listen(5000, () => console.log('SQL Gateway running on port 5000'));