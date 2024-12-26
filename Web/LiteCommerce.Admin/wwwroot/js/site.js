// The initial idea was to create a service named LayoutSettingsService to handle the logic instead of writing it directly in the component.
// However, since these settings need to modify the attributes of the HTML tags in the index.html file from a Blazor component,
// it was necessary to use an intermediate JavaScript file that the component could invoke.

window.externalLibs = {
    showModal: function (modalId) {
        const modal = document.getElementById(modalId);
        const bootstrapModal = new bootstrap.Modal(modal);
        bootstrapModal.show();
    },
    hideModal: function (modalId) {
        const modal = document.getElementById(modalId);
        const bootstrapModal = bootstrap.Modal.getInstance(modal);
        bootstrapModal.hide();
    }
}

// Data & default operations
window.appInterop = {
    toggleSidebarMenu: function () {
        document.querySelectorAll(".sidebartoggler").forEach((el) => {
            el.checked = true;
        });

        const mainWrapper = document.getElementById("main-wrapper");
        if (mainWrapper) {
            mainWrapper.classList.toggle("show-sidebar");
        }

        document.querySelectorAll(".sidebarmenu").forEach((el) => {
            el.classList.toggle("close");
        });

        const dataTheme = document.body.getAttribute("data-sidebartype");
        if (dataTheme === "full") {
            document.body.setAttribute("data-sidebartype", "mini-sidebar");
        } else {
            document.body.setAttribute("data-sidebartype", "full");
        }
    }
};

// Layout related functions
window.layoutInterop = {

    // Theme mode Dark or Light
    setThemeMode: function (theme, darkDisplay, lightDisplay, sunDisplay, moonDisplay) {
        // Set theme attribute
        document.documentElement.setAttribute("data-bs-theme", theme);

        // Handle logo display
        document.querySelectorAll(`.${darkDisplay}`)
            .forEach(el => el.style.display = "none");
        document.querySelectorAll(`.${lightDisplay}`)
            .forEach(el => el.style.display = "flex");

        // Handle icon display    
        document.querySelectorAll(`.${sunDisplay}`)
            .forEach(el => el.style.display = "none");
        document.querySelectorAll(`.${moonDisplay}`)
            .forEach(el => el.style.display = "flex");

        // Set radio button if exists
        const themeLayoutElement = document.getElementById(`${theme}-layout`);
        if (themeLayoutElement) {
            themeLayoutElement.checked = true;
        }
    },

    // Theme Direction RTL LTR click
    setLayoutDirection: function (direction) {
        document.documentElement.setAttribute("dir", direction);

        if (direction === "rtl") {
            const offcanvasEnd = document.querySelector(".offcanvas-end");
            if (offcanvasEnd) {
                offcanvasEnd.classList.toggle("offcanvas-start");
                offcanvasEnd.classList.remove("offcanvas-end");
            }
        } else {
            const offcanvasStart = document.querySelector(".offcanvas-start");
            if (offcanvasStart) {
                offcanvasStart.classList.toggle("offcanvas-end");
                offcanvasStart.classList.remove("offcanvas-start");
            }
        }
    },

    // Theme Layout Color
    setColorTheme: function (colorTheme) {
        document.documentElement.setAttribute("data-color-theme", colorTheme);
    },

    // Theme Layout Vertical or Horizontal
    setLayoutType: function (layoutType) {
        if (layoutType === "vertical") {
            document.documentElement.setAttribute("data-layout", "vertical");
        } else {
            document.documentElement.setAttribute("data-layout", "horizontal");
        }
    },

    // Theme Layout Box or Full
    setContainerOption: function (containerOption) {
        const containerFluid = document.querySelectorAll(".container-fluid");

        if (containerOption == "boxed") {
            containerFluid.forEach(element => element.classList.remove("mw-100"));
            document.documentElement.setAttribute("data-boxed-layout", "boxed");
        } else {
            containerFluid.forEach(element => element.classList.add("mw-100"));
            document.documentElement.setAttribute("data-boxed-layout", "full");
        }
    },

    // Theme Sidebar Full or Collapse
    setSidebarType: function (sidebarType) {
        if (sidebarType === "full") {
            document.body.setAttribute("data-sidebartype", "full");
        } else {
            document.body.setAttribute("data-sidebartype", "mini-sidebar");
        }
    },

    manageSidebarType: function (sidebarType) {
        switch (sidebarType) {
            case "full":
                var fullSidebarElement = document.querySelector("#full-sidebar");
                if (fullSidebarElement) {
                    fullSidebarElement.checked = true;
                }
                this.setSidebarType("full");

                const setSidebarType = () => {
                    const width =
                        window.innerWidth > 0 ? window.innerWidth : screen.width;
                    if (width < 1300) {
                        this.setSidebarType("mini-sidebar");
                    } else {
                        this.setSidebarType("full");
                    }
                };
                window.addEventListener("DOMContentLoaded", setSidebarType);
                window.addEventListener("resize", setSidebarType);
                break;
            case "mini-sidebar":
                var miniSidebarElement = document.querySelector("#mini-sidebar");
                if (miniSidebarElement) {
                    miniSidebarElement.checked = true;
                }
                this.setSidebarType("mini-sidebar");
                break;
            default:
                break;
        }
    },

    // Theme card with Border or Shadow
    setCardLayout: function (cardLayout) {
        if (cardLayout === "border") {
            document.documentElement.setAttribute("data-card", "border");
        } else {
            document.documentElement.setAttribute("data-card", "shadow");
        }
    }
};
