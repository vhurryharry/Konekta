Cypress.Cookies.defaults({
    whitelist: ['.AspNetCore.Identity.Application', 'ai_session', 'ai_user']
})

describe("Testing Actionstep Connection Prompt on HomePage", () => {

    it('Login to the portal with non-existant ActionstepOrg', () => {
        cy.login("fakeorg", 1);
    })

    it("Check that Actionstep Connection Modal is shown", () => {
        cy.get("body").should("have.descendants", "ai-dialog");
        cy.get(".modal-title").should("have.text", "We need your approval!");
    })
})