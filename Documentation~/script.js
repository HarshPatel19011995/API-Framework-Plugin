function loadPage(page, clickedLink = null) {
    fetch("pages/" + page + ".html")
        .then(response => response.text())
        .then(data => {
            document.getElementById("content-area").innerHTML = data;
            window.scrollTo(0, 0);
            setActiveMenu(clickedLink);
        })
        .catch(() => {
            document.getElementById("content-area").innerHTML =
                "<h1>Page Not Found</h1>";
        });
}

function setActiveMenu(activeLink) {
    const links = document.querySelectorAll(".sidebar a");
    links.forEach(link => link.classList.remove("active"));

    if (activeLink) {
        activeLink.classList.add("active");
    }
}

function toggleTheme() {
    document.body.classList.toggle("dark");
}

function setupMenuLinks() {
    const links = document.querySelectorAll(".sidebar a");

    links.forEach(link => {
        link.addEventListener("click", function (e) {
            e.preventDefault();
            const page = this.getAttribute("data-page");
            loadPage(page, this);
        });
    });
}

window.onload = function () {
    setupMenuLinks();

    const defaultLink = document.querySelector('[data-page="overview"]');
    loadPage("overview", defaultLink);
};