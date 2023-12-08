const basketDiv = document.querySelector(".basket-block");
const addBtns = document.querySelectorAll(".add-basket-btn")

addBtns.forEach(btn => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        const endpoint = btn.getAttribute("href");
        fetch(endpoint)
            .then(res => res.text())
            .then(data => {
                console.log(data);
                basketDiv.innerHTML = data;
            })
    })
})

