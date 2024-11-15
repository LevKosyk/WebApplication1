const url = 'https://localhost:7037';


async function register() {
    const urlAuth = `${url}/api/APIUser/register`;
    try {
        const response = await fetch(urlAuth, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Origin': '*'
            },
            body: JSON.stringify({
                email: document.getElementById('exampleInputEmail1'),
                password: document.getElementById('emailHelp')
            })
        });
        const data = await response.json();
        return data.token;
    } catch (error) {
        console.error('Error fetching token:', error);
    }
}


document.getElementById('btn').addEventListener('onClick', register())

