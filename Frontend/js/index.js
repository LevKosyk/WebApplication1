async function loadProducts() {
    const token = localStorage.getItem('authToken'); 
    if (!token) {
        console.error('No token found, redirecting to login...');
        window.location.href = '/html/login.html';
        return;
    }

    try {
        const response = await fetch(`${url}/api/apiproduct/`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}` 
            }
        });

        if (!response.ok) {
            throw new Error("Failed to fetch products");
        }

        const data = await response.json();
        let res = '';
        data.forEach(item => {
            res += `
    <div class="card" style="width: 18rem;">
        <img src="${item.image}" class="card-img-top" alt="...">
        <div class="card-body">
            <h5 id="titleId" class="card-title">${item.name}</h5>
            <p id="descriptionId" class="card-text">${item.description}</p>
            <p id="priceId" class="card-text">${item.price}</p>
            <a href="#" class="btn btn-primary" data-id="${item.id}" onClick="addToBasket(event)">Buy</a>
        </div>
    </div>`;
        });
        document.getElementById('contener_products').innerHTML = res;
    } catch (error) {
        console.error('Error fetching products:', error);
    }
}
async function addToBasket(event) {
    event.preventDefault();
    const id = event.target.getAttribute('data-id'); 
    const token = localStorage.getItem('authToken'); 

    if (!token) {
        window.location.href = '/html/login.html';
        return;
    }

    try {
        const response = await fetch(`${url}/api/apiorder/addProduct/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}` 
            }
        });

        if (!response.ok) {
            throw new Error("Failed to add product to basket.");
        }

        window.location.href = '/html/basket.html';
    } catch (error) {
        console.error('Error adding to basket:', error);
    }
}


function logout() {
    localStorage.removeItem('authToken'); 
    window.location.href = '/html/login.html'; 
}
