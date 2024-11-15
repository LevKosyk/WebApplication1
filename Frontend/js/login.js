async function login() {
    const urlAuth = `${url}/api/APIUser/auth`;
    try {
        const response = await fetch(urlAuth, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Origin': '*'
            },
            body: JSON.stringify({
                email: document.getElementById('exampleInputEmail1').value,
                password: document.getElementById('exampleInputPassword1').value
            })
        });

        if (!response.ok) {
            throw new Error("Failed to authenticate.");
        }

        const data = await response.json();
        localStorage.setItem('authToken', data.token); 
        return data.token;
    } catch (error) {
        console.error('Error fetching token:', error);
    }
}
