Cypress.Cookies.defaults({
    whitelist: ['.AspNetCore.Identity.Application', 'ai_session', 'ai_user']
})

describe("Testing InfoTrack Redirect Page", () => {

    it('Login to the Portal', () => {
        cy.login("fakeorgwithinfotrack", 9);
    })

    // it("Disconnect the actionstep org", () => {
    //     cy.restoreCookies();
    //     cy.get("[data-cy='disconnect-trial181078920']").click();
    //     cy.wait(2000);
    // })

    it("Navigate to Infotrack Redirect Page", () => {
        cy.visit("/wca/infotrack/redirect-with-matter-info?matterId=1&actionstepOrg=fakeorgwithinfotrack");

        cy.wait(3000);
        cy.get("body").should("have.descendants", "ai-dialog");
        cy.get(".modal-title").should("have.text", "We need your approval!");
    })
})