
const closeBtns = document.querySelectorAll(".custom-close-btn")

if (closeBtns) {
    closeBtns.forEach(btn => btn.addEventListener("click", (e) => {
        btn.parentElement.remove();
    }))
}