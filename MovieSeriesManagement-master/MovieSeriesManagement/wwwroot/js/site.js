// Funcionalidad JavaScript para el sitio
document.addEventListener("DOMContentLoaded", () => {
    // Inicializar tooltips de Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    tooltipTriggerList.forEach((tooltipTriggerEl) => {
        new bootstrap.Tooltip(tooltipTriggerEl)
    })

    // Inicializar popovers de Bootstrap
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    popoverTriggerList.forEach((popoverTriggerEl) => {
        new bootstrap.Popover(popoverTriggerEl)
    })

    // Navbar background change on scroll
    window.addEventListener("scroll", () => {
        const navbar = document.querySelector(".navbar")
        if (window.scrollY > 50) {
            navbar.classList.add("scrolled")
        } else {
            navbar.classList.remove("scrolled")
        }
    })

    // Slider functionality for content rows
    const sliders = document.querySelectorAll(".slider-container")

    sliders.forEach((slider) => {
        // Add scroll buttons to each slider if not already added
        if (!slider.parentElement.querySelector(".slider-controls")) {
            const sliderParent = slider.parentElement
            const controlsDiv = document.createElement("div")
            controlsDiv.className = "slider-controls"

            const leftControl = document.createElement("div")
            leftControl.className = "slider-control slider-control-left"
            leftControl.innerHTML = '<i class="bi bi-chevron-left"></i>'

            const rightControl = document.createElement("div")
            rightControl.className = "slider-control slider-control-right"
            rightControl.innerHTML = '<i class="bi bi-chevron-right"></i>'

            controlsDiv.appendChild(leftControl)
            controlsDiv.appendChild(rightControl)
            sliderParent.appendChild(controlsDiv)

            // Scroll functionality
            leftControl.addEventListener("click", () => {
                slider.scrollBy({ left: -500, behavior: "smooth" })
            })

            rightControl.addEventListener("click", () => {
                slider.scrollBy({ left: 500, behavior: "smooth" })
            })
        }
    })
})

