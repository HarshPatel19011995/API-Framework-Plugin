function loadPage(page, clickedLink = null) {

    fetch("./pages/" + page + ".html")
        .then(response => {

            if (!response.ok) {
                throw new Error("Page not found");
            }

            return response.text();
        })
        .then(data => {

            document.getElementById("content-area").innerHTML = data;

            window.scrollTo(0, 0);

            setActiveMenu(clickedLink);

            // update URL hash
            window.location.hash = page;
        })
        .catch(() => {

            document.getElementById("content-area").innerHTML =
                "<h1>404 - Page Not Found</h1>";
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

function loadInitialPage() {

    const hash = window.location.hash.replace("#", "");

    const page = hash || "overview";

    const link = document.querySelector(`[data-page="${page}"]`);

    loadPage(page, link);
}

window.onload = function () {

    setupMenuLinks();

    loadInitialPage();
};
