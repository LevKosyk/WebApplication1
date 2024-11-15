const url = 'https://localhost:7037';

async function getToken() {
    return localStorage.getItem('authToken');
}

async function loadProductsInBasket() {
    const token = await getToken();
    if (!token) {
        console.error("No token found, redirecting to login...");
        window.location.href = '/html/login.html'; 
        return;
    }

    try {
        const response = await fetch(`${url}/api/apiorder/getProducts`, {
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
        <li class="list-group-item d-flex justify-content-between align-items-start">
          <div class="ms-2 me-auto">
            <div class="fw-bold">${item.name}</div>
            ${item.description}
          </div>
          <span class="badge text-bg-primary rounded-pill">${item.price}</span>
        </li>`;
        });

        const container = document.getElementById('contener_products');
        if (container) {
            container.innerHTML = res;
        } else {
            console.error("Container for products not found");
        }
    } catch (error) {
        console.error('Error fetching products:', error);
    }
}

function onClickHandele() {
    window.location.href = '/html/Success.html'; 
}


loadProductsInBasket();
