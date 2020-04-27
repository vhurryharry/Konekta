Cypress.Cookies.defaults({
    whitelist: ['.AspNetCore.Identity.Application', 'ai_session', 'ai_user']
})

describe("Testing Actionstep Connection Prompt in Settlement Calculator", () => {

    it('Login', () => {
        cy.login("fakeorg", 1);
    })

    it('Navigate to the Settlement Calculator', () => {
        cy.visit("/wca/calculators/settlement?matterId=1&actionstepOrg=fakeorg");
    })

    it("Check the Actionstep Connection Modal", () => {
        cy.wait(3000);
        cy.get("body").should("have.descendants", "[data-cy='connect-to-actionstep-box']");
        cy.get(".ibox-title").should("have.text", "We need your approval");
    })
})