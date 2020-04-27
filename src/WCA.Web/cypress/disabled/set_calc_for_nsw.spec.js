Cypress.Cookies.defaults({
    whitelist: ['.AspNetCore.Identity.Application', 'ai_session', 'ai_user']
})

describe("Testing Settlement Calculator for NSW", () => {

    it('Navigate to the Calculator', () => {
        cy.login("trial181078920", 18);

        cy.get("[data-cy='settlement-calculator']").click();

        cy.saveLocalStorage();
        cy.saveCookies();
    })

    it("Confirm modal", () => {
        cy.restoreLocalStorage();
        cy.restoreCookies();

        cy.wait(500);

        cy.get("body").find("[data-cy='modal-header']").its("length").then(length => {
            if (length == 2) {
                cy.get("[data-cy='use_new_button']").click();
            }
        })

        cy.get("[data-cy='delete_button']").click();
        cy.wait(500);
    })

    it("Correct the Contract Price & Deposit value", () => {
        cy.get("[data-cy='adjustment_info_0']").click();

        cy.get("[data-cy='contract_price_input']").clear().type("25000");
        cy.get("[data-cy='deposit_input']").clear().type("0");

        cy.get("[data-cy='modal_save_button']").click();
    })

    it("Release Fee", () => {
        cy.addAdjustment("Release Fee");

        cy.get("[data-cy='release_mortgages_input']").clear().type("50");

        cy.get("[data-cy='release_each_input']").clear().type("50");

        cy.get("[data-cy='modal_save_button']").click();

        cy.get("[data-cy='adjustment_result_1']").should("have.text", "2,500.00");
    })

    it("Water Usage", () => {
        cy.addAdjustment("Water Usage");

        cy.get("[data-cy='paid_to_date_input']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='March 2019']").click();
        cy.get(":nth-child(4) > :nth-child(5) > .day_350f6485").click();

        cy.get("[data-cy='paid_reading_amount_input']").clear().type("2011");

        cy.get("[data-cy='search_date_input']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='June 2019']").click();
        cy.get(":nth-child(1) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='search_reading_amount_input']").clear().type("3000");

        cy.get("[data-cy='tier1_charge_input']").clear().type("60");
        cy.get("[data-cy='tier1_kl_count_input']").clear().type("3");

        cy.get("[data-cy='tier2_charge_input']").clear().type("50");
        cy.get("[data-cy='tier2_kl_count_input']").clear().type("7");

        cy.get("[data-cy='balance_charge_input']").clear().type("24");
        cy.get("[data-cy='balance_fee_increase']").clear().type("5");

        cy.get("[data-cy='bulk_charge_input']").clear().type("31");
        cy.get("[data-cy='bulk_fee_increase']").clear().type("3");


        cy.get("[data-cy='cts_option_select']").click().then(() => {
            cy.wait(500);
            cy.contains("Shared Percentage").click();
            cy.get("[data-cy='percentage_input']").clear().type("15");
        })

        cy.get("[data-cy='modal_save_button']").click();

        cy.get("[data-cy='adjustment_result_2']").should("have.text", "12,057.93");
    })

    it("Emergency Services Levy", () => {
        cy.addAdjustment("Emergency Services Levy");

        cy.get("[data-cy='amount_input']").clear().type("5000");

        cy.get("[data-cy='from_date_select']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='April 2019']").click();
        cy.get(":nth-child(1) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='to_date_select']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='June 2019']").click();
        cy.get(":nth-child(4) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='status_select']").click();

        cy.get("[data-cy='modal_save_button']").click();

        cy.get("[data-cy='adjustment_result_3']").should("have.text", "7,564.10");
    })

    it("Penalty Interest", () => {
        cy.addAdjustment("Penalty Interest");

        cy.get("[data-cy='rate_input']").clear().type("500");

        cy.get("[data-cy='from_date_select']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='April 2019']").click();
        cy.get(":nth-child(1) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='to_date_select']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='June 2019']").click();
        cy.get(":nth-child(4) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='modal_save_button']").click();

        cy.get("[data-cy='adjustment_result_4']").should("have.text", "26,712.33");
    })

    it("Other Adjustment Fixed", () => {
        cy.addAdjustment("Other Adjustment Fixed");

        cy.get("[data-cy='description_input']").type("This is the description");
        cy.get("[data-cy='amount_input']").clear().type("1500");

        cy.get("[data-cy='modal_save_button']").click();

        cy.get("[data-cy='adjustment_result_5']").should("have.text", "1,500.00");
    })

    it("Other Adjustment Date", () => {
        cy.addAdjustment("Other Adjustment Date");

        cy.get("[data-cy='description_input']").clear().type("This is the description");
        cy.get("[data-cy='amount_input']").clear().type("2000");

        cy.get("[data-cy='from_date_select']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='April 2019']").click();
        cy.get(":nth-child(1) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='to_date_select']").click();

        cy.get(".ms-DatePicker-monthAndYear").click();
        cy.get("[aria-label='June 2019']").click();
        cy.get(":nth-child(4) > :nth-child(6) > .day_350f6485").click();

        cy.get("[data-cy='modal_save_button']").click();

        cy.get("[data-cy='adjustment_result_6']").should("have.text", "-1,025.64");
    })

    it("Add an additional requirement", () => {
        cy.get("[data-cy='add_additional_requirement']").click();
    })

    it("Additional Requirement", () => {
        cy.get("[data-cy='description_input']").type("This is the description");
        cy.get("[data-cy='amount_input']").clear().type("2500");

        cy.get("[data-cy='modal_save_button']").click();
    })

    it("Add a payee", () => {
        cy.get("[data-cy='add_payee']").click();
    })

    it("Payee", () => {
        cy.get("[data-cy='description_input']").type("This is the description");
        cy.get("[data-cy='amount_input']").clear().type("3000");

        cy.get("[data-cy='modal_save_button']").click();
    })

    it("Add a payee", () => {
        cy.get("[data-cy='add_payee']").click();
    })

    it("Payee", () => {
        cy.get("[data-cy='description_input']").type("This is the final payee.");
        cy.get(".ms-Grid > .ms-Checkbox > .ms-Checkbox-label").click();

        cy.get("[data-cy='modal_save_button']").click();
    })

    it("Add our requirement", () => {
        cy.get("[data-cy='add_our_requirement']").click();
    })

    it("Our Requirement", () => {
        cy.get("[data-cy='detail_input']").type("This is the description");

        cy.get("[data-cy='modal_save_button']").click();

    })

    it("Check the payee result", () => {
        cy.get("[data-cy='contract_debit_value']").should("have.text", "52,186.69");

        //// Temporarily disabled Valdas, can you help with this please :)
        //cy.get("[data-cy='contract_credit_value']").should("have.text", "22,122.04");

        cy.get("[data-cy='additional_debit_value']").should("have.text", "$54,686.69");
        cy.get("[data-cy='additional_credit_value']").should("have.text", "$22,122.04");

        cy.get("[data-cy='allocated_debit_value']").should("have.text", "$52,186.69");
        cy.get("[data-cy='allocated_credit_value']").should("have.text", "$52,186.69");

        cy.get("[data-cy='unallocated_value']").should("have.text", "(unallocated: $ 0.00)");
    })
})