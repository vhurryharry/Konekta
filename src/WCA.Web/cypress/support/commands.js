// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

var COOKIE_MEMORY = {};

Cypress.Commands.add("login", (actionstepOrg, matterId) => {
    cy.readFile('./appsettings.json').then(settings => {
        const username = settings.TestUserCredential.Username;
        const password = settings.TestUserCredential.Password;

        cy.visit("/Identity/Account/LoginWithPassword?returnurl=/wca?actionsteporg=" + actionstepOrg + "%26matterid=" + matterId);
        cy.get("[data-cy='Input_Email']").type(username, { delay: 0 });
        cy.get("[data-cy='Input_Password']").type(password, { delay: 0 });

        cy.get("[data-cy='Login_Button']").click();
    });
})

Cypress.Commands.add("addAdjustment", (adjustmentName) => {
    cy.get("[data-cy='add_adjustment_button']").click();

    if (adjustmentName != "Water Usage") {
        cy.get("[data-cy='adjustment_type_select']").click().then(() => {
            cy.wait(500);
            cy.contains(adjustmentName).click();
        })
    }

    cy.get("[data-cy='modal_save_button']").click();
})

Cypress.Commands.add("saveCookies", () => {
    cy.getCookies()
        .then(cookies => {
            cookies.forEach(cookie => {
                if (cookie.name != ".AspNetCore.Identity.Application" && cookie.name != "ai_session" && cookie.name != "ai_user")
                    COOKIE_MEMORY[cookie.name] = cookie.value;
            })
        })
})

Cypress.Commands.add("restoreCookies", () => {
    Object.keys(COOKIE_MEMORY).forEach(key => {
        cy.setCookie(key, COOKIE_MEMORY[key]);
    })
})

let LOCAL_STORAGE_MEMORY = {};

Cypress.Commands.add("saveLocalStorage", () => {
    Object.keys(localStorage).forEach(key => {
        LOCAL_STORAGE_MEMORY[key] = localStorage[key];
    });
});

Cypress.Commands.add("restoreLocalStorage", () => {
    Object.keys(LOCAL_STORAGE_MEMORY).forEach(key => {
        localStorage.setItem(key, LOCAL_STORAGE_MEMORY[key]);
    });
});

Cypress.Commands.add("disableAnimations", () => {
    cy.document().then((doc) => {
        // work with document element
        const disableAnimationsCss = `* {
            /*CSS transitions*/
            -o-transition-property: none !important;
            -moz-transition-property: none !important;
            -ms-transition-property: none !important;
            -webkit-transition-property: none !important;
            transition-property: none !important;
            /*CSS transforms*/
            -o-transform: none !important;
            -moz-transform: none !important;
            -ms-transform: none !important;
            -webkit-transform: none !important;
            transform: none !important;
            /*CSS animations*/
            -webkit-animation: none !important;
            -moz-animation: none !important;
            -o-animation: none !important;
            -ms-animation: none !important;
            animation: none !important;
    }`;


        let styleElement = doc.createElement('style');
        styleElement.innerHTML = disableAnimationsCss;
        doc.body.appendChild(styleElement);
    })
})
