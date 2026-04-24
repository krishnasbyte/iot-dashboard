// Dynamic API URL - works with localhost OR IP address
const API_URL = `${window.location.protocol}//${window.location.hostname}:5215`;

console.log(`Dashboard connecting to API at: ${API_URL}`);

let chart;
let previousData = null;

function initChart() {
    const ctx = document.getElementById('sensorChart').getContext('2d');
    chart = new Chart(ctx, {
        type: 'line',
        data: { labels: [], datasets: [] },
        options: { responsive: true, maintainAspectRatio: true }
    });
}

async function fetchData() {
    try {
        const response = await fetch(`${API_URL}/api/sensor/memory`);
        if (!response.ok) throw new Error('API not responding');
        const data = await response.json();
        
        if (data && data.length > 0) {
            const latest = data[data.length - 1];
            document.getElementById('temperature').innerText = latest.temperature.toFixed(1);
            document.getElementById('humidity').innerText = latest.humidity.toFixed(1);
            document.getElementById('pressure').innerText = latest.pressure.toFixed(1);
            document.getElementById('status').innerHTML = '🟢 Live';
            document.getElementById('status').className = 'status online';
            document.getElementById('lastUpdate').innerHTML = `Last update: ${new Date().toLocaleTimeString()}`;
            
            const tbody = document.getElementById('tableBody');
            tbody.innerHTML = '';
            data.slice(-10).reverse().forEach(r => {
                const row = tbody.insertRow();
                row.insertCell(0).innerText = new Date(r.timestamp).toLocaleTimeString();
                row.insertCell(1).innerText = r.temperature.toFixed(1);
                row.insertCell(2).innerText = r.humidity.toFixed(1);
                row.insertCell(3).innerText = r.pressure.toFixed(1);
            });
            
            if (chart) {
                const chartData = data.slice(-20);
                chart.data.labels = chartData.map(d => new Date(d.timestamp).toLocaleTimeString());
                chart.data.datasets = [
                    { label: 'Temperature (°C)', data: chartData.map(d => d.temperature), borderColor: '#ff6b6b', tension: 0.4 },
                    { label: 'Humidity (%)', data: chartData.map(d => d.humidity), borderColor: '#4ecdc4', tension: 0.4 }
                ];
                chart.update();
            }
        }
    } catch (error) {
        console.error('Fetch error:', error);
        document.getElementById('status').innerHTML = '🔴 Offline';
        document.getElementById('status').className = 'status offline';
    }
}

initChart();
fetchData();
setInterval(fetchData, 5000);
