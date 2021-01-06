import { ConveyancingDataCheck } from '../client-app/src/containers/pexa/steps/conveyancingDataCheck';

const initialPexaWorkspaceCreationData = {
    "createPexaWorkspaceCommand": {
        "authenticatedUser": null,
        "pexaWorkspaceCreationRequest": {
            "landTitleDetails": {
                "landTitle": [
                    {
                        "landTitleReference": "61100",
                        "unregisteredLotReference": "123"
                    }
                ],
                "parentTitle": "Yes"
            },
            "requestLandTitleData": "No",
            "jurisdiction": "NSW",
            "role": "Incoming Proprietor",
            "subscriberReference": "trial181078920_23",
            "projectName": null,
            "financialSettlement": "Yes",
            "settlementDateValue": "2050-12-01T00:00:00",
            "settlementDateValueSpecified": true,
            "settlementDate": "2050-12-01T00:00:00",
            "settlementDateAndTimeValueSpecified": false,
            "participantSettlementAcceptanceStatus": "Accepted",
            "partyDetails": [
                {
                    "representingParty": "Yes",
                    "partyType": "Individual",
                    "partyRole": "Incoming Proprietor",
                    "fullName": {
                        "nameTitle": null,
                        "givenName": [
                            {
                                "value": "PEXA",
                                "order": "1"
                            },
                            {
                                "value": "Middle",
                                "order": "2"
                            }
                        ],
                        "familyName": "Test",
                        "familyNameOrder": null,
                        "nameSuffix": null,
                        "dateOfBirthValue": "1980-10-01T00:00:00",
                        "dateOfBirthValueSpecified": true,
                        "dateOfBirth": "1980-10-01T00:00:00"
                    },
                    "currentAddress": {
                        "streetAddress": {
                            "subDwellingUnitType": {
                                "unitTypeCode": null,
                                "unitNumber": null
                            },
                            "level": null,
                            "complexRoadSpecified": false,
                            "secondaryComplex": null,
                            "addressSiteName": null,
                            "road": {
                                "lotNumber": "",
                                "roadNumber": "27",
                                "roadName": "ST Bunney",
                                "roadTypeCode": "PLZA",
                                "roadSuffixCode": "IN"
                            },
                            "localityName": "CypressTestCityForInidividual",
                            "postcode": "1102",
                            "state": "NSW"
                        },
                        "correspondenceAddress": {
                            "postalDelivery": {
                                "postalDeliveryTypeCode": "",
                                "postalDeliveryNumber": "1102"
                            },
                            "road": {
                                "lotNumber": "",
                                "roadNumber": "27",
                                "roadName": "ST Bunney",
                                "roadTypeCode": "PLZA",
                                "roadSuffixCode": "IN"
                            },
                            "localityName": "CypressTestCityForInidividual",
                            "postcode": "1102",
                            "state": "NSW"
                        }
                    }
                },
                {
                    "representingParty": "Yes",
                    "partyType": "Organisation",
                    "partyRole": "Incoming Proprietor",
                    "business": {
                        "legalEntityName": "PEXA Test Company",
                        "businessName": "PEXA Test Company",
                        "administrationStatusSpecified": false,
                        "identificationSpecified": false
                    },
                    "currentAddress": {
                        "streetAddress": {
                            "subDwellingUnitType": {
                                "unitTypeCode": null,
                                "unitNumber": null
                            },
                            "level": null,
                            "complexRoadSpecified": false,
                            "secondaryComplex": null,
                            "addressSiteName": null,
                            "road": {
                                "lotNumber": "",
                                "roadNumber": "133",
                                "roadName": "MOORE SOMETHING building",
                                "roadTypeCode": "RES",
                                "roadSuffixCode": "N"
                            },
                            "localityName": "CypressTestCityForOrg",
                            "postcode": "1234",
                            "state": "NSW"
                        },
                        "correspondenceAddress": {
                            "postalDelivery": {
                                "postalDeliveryTypeCode": "",
                                "postalDeliveryNumber": "1234"
                            },
                            "road": {
                                "lotNumber": "",
                                "roadNumber": "133",
                                "roadName": "MOORE SOMETHING building",
                                "roadTypeCode": "RES",
                                "roadSuffixCode": "N"
                            },
                            "localityName": "CypressTestCityForOrg",
                            "postcode": "1234",
                            "state": "NSW"
                        }
                    }
                }
            ],
            "partyDetailsSpecified": true
        },
        "orgKey": "trial181078920",
        "matterId": 23
    },
    "pexaRoleSpecified": true,
    "actionstepData": {
        "id": 23,
        "actionType": "Conveyancing - NSW",
        "conveyancingType": 2,
        "conveyancingSubType": null,
        "fileReference": null,
        "name": "For PEXA Test",
        "created": {

        },
        "updated": {

        },
        "propertyDetails": {
            "titleReference": "61100",
            "lotNo": "123"
        },
        "propertyAddresses": [

        ],
        "buyers": [
            {
                "name": null,
                "identityType": 1,
                "title": null,
                "firstName": "PEXA",
                "middleName": null,
                "lastName": "Test",
                "suffix": null,
                "preferredName": null,
                "employer": null,
                "companyName": null,
                "occupation": null,
                "dateOfBirth": {
                    "calendar": {
                        "id": "ISO",
                        "name": "ISO",
                        "minYear": -9998,
                        "maxYear": 9999,
                        "eras": [
                            {
                                "name": "BCE"
                            },
                            {
                                "name": "CE"
                            }
                        ]
                    },
                    "year": 1980,
                    "month": 10,
                    "day": 1,
                    "dayOfWeek": 3,
                    "yearOfEra": 1980,
                    "era": {
                        "name": "CE"
                    },
                    "dayOfYear": 275
                },
                "isDeceased": false,
                "maritalStatus": 0,
                "gender": 0,
                "emailAddress": null,
                "addressLine1": "30/23-27 ST Bunney",
                "addressLine2": "Inner Plaza",
                "city": "CypressTestCityForInidividual",
                "country": null,
                "postCode": "1102",
                "stateProvince": "NSW",
                "phoneContacts": [

                ]
            },
            {
                "name": null,
                "identityType": 0,
                "title": null,
                "firstName": null,
                "middleName": null,
                "lastName": null,
                "suffix": null,
                "preferredName": null,
                "employer": null,
                "companyName": "PEXA Test Company",
                "occupation": null,
                "dateOfBirth": {
                    "calendar": {
                        "id": "ISO",
                        "name": "ISO",
                        "minYear": -9998,
                        "maxYear": 9999,
                        "eras": [
                            {
                                "name": "BCE"
                            },
                            {
                                "name": "CE"
                            }
                        ]
                    },
                    "year": 1,
                    "month": 1,
                    "day": 1,
                    "dayOfWeek": 1,
                    "yearOfEra": 1,
                    "era": {
                        "name": "CE"
                    },
                    "dayOfYear": 1
                },
                "isDeceased": false,
                "maritalStatus": 0,
                "gender": 0,
                "emailAddress": null,
                "addressLine1": "2/133 MOORE SOMETHING RESERVE",
                "addressLine2": "North building",
                "city": "CypressTestCityForOrg",
                "country": null,
                "postCode": "1234",
                "stateProvince": "NSW",
                "phoneContacts": [

                ]
            }
        ],
        "sellers": [

        ],
        "conveyancers": [
            {
                "name": null,
                "identityType": 1,
                "title": null,
                "firstName": "First02",
                "middleName": null,
                "lastName": "Last01",
                "suffix": null,
                "preferredName": null,
                "employer": null,
                "companyName": null,
                "occupation": null,
                "dateOfBirth": {
                    "calendar": {
                        "id": "ISO",
                        "name": "ISO",
                        "minYear": -9998,
                        "maxYear": 9999,
                        "eras": [
                            {
                                "name": "BCE"
                            },
                            {
                                "name": "CE"
                            }
                        ]
                    },
                    "year": 2001,
                    "month": 8,
                    "day": 15,
                    "dayOfWeek": 3,
                    "yearOfEra": 2001,
                    "era": {
                        "name": "CE"
                    },
                    "dayOfYear": 227
                },
                "isDeceased": false,
                "maritalStatus": 0,
                "gender": 0,
                "emailAddress": "testuser02@domain.com",
                "addressLine1": "Line02 1",
                "addressLine2": "Line02 2",
                "city": "City02",
                "country": null,
                "postCode": "1102",
                "stateProvince": "NSW",
                "phoneContacts": [

                ]
            }
        ],
        "clients": [

        ],
        "clientPrimaryContacts": [

        ],
        "agentsOffice": [

        ],
        "principalSolicitor": [

        ],
        "incomingBanks": [

        ],
        "purchasePrice": null,
        "fullDeposit": null,
        "initialDeposit": null,
        "balanceDeposit": null,
        "depositHolder": null,
        "depositBondExists": false,
        "originalBondGuaranteedHeldByAgent": false,
        "reduction": null,
        "specialConditions": [

        ],
        "settlementBookingTime": null,
        "settlementDate": {
            "calendar": {
                "id": "ISO",
                "name": "ISO",
                "minYear": -9998,
                "maxYear": 9999,
                "eras": [
                    {
                        "name": "BCE"
                    },
                    {
                        "name": "CE"
                    }
                ]
            },
            "year": 2050,
            "month": 12,
            "day": 1,
            "dayOfWeek": 4,
            "yearOfEra": 2050,
            "era": {
                "name": "CE"
            },
            "dayOfYear": 335
        },
        "contractLength": {
            "start": {

            },
            "hasStart": true,
            "end": {

            },
            "hasEnd": true,
            "duration": {
                "days": 0,
                "nanosecondOfDay": 0,
                "hours": 0,
                "minutes": 0,
                "seconds": 0,
                "milliseconds": 0,
                "subsecondTicks": 0,
                "subsecondNanoseconds": 0,
                "bclCompatibleTicks": 0,
                "totalDays": 0,
                "totalHours": 0,
                "totalMinutes": 0,
                "totalSeconds": 0,
                "totalMilliseconds": 0,
                "totalTicks": 0,
                "totalNanoseconds": 0
            }
        },
        "settlementLocation": null,
        "settlementVenue": null,
        "bookingReference": null,
        "loanAmount": null,
        "payoutFigure": null,
        "agentCommissionPayable": null
    }
};

let wrapper;
beforeEach(() => {
    const pexaWorkspaceCreationData = { ...initialPexaWorkspaceCreationData };
    wrapper = shallow(<ConveyancingDataCheck
        pexaWorkspaceCreationData={pexaWorkspaceCreationData} />);
});

describe('Initial state', () => {
    it('Create button disabled by default', () => {
        expect(wrapper.find("[data-cy='pexa_create_button']").prop('disabled')).toEqual(true);
    });

    /*
    /// Disabled this test for now due to the land title validation
    it('Set address types to enable Create button', () => {
        let state = wrapper.state();
        state.addressTypes = state.addressTypes.map(addressType => 0);
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_create_button']").prop('disabled')).toEqual(false);
    });
    */
});

describe("Transaction Details", () => {
    it('Financial Settlement', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.financialSettlement = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_financial_settlement']").prop('errorMessage')).toEqual(" ");
    });

    it('Jurisdiction', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.jurisdiction = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_jurisdictions']").prop('errorMessage')).toEqual(" ");
    });

    it('Request Land Title Data', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.requestLandTitleData = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_request_land_title_data']").prop('errorMessage')).toEqual(" ");
    });

    it('Role', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.role = null;
        state.pexaRoleSpecified = false;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_role']").prop('errorMessage')).toEqual(" ");
    });

    it('Subscriber Reference', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.subscriberReference = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_subscriber_reference']").prop('errorMessage')).toEqual(" ");
    });
})

describe("Party Details", () => {
    it('Party Type', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].partyType = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_party_type_0']").prop('errorMessage')).toEqual(" ");
    });

    it('Representing Party', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].representingParty = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_representing_party_0']").prop('errorMessage')).toEqual(" ");
    });

    it('Road Name', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].currentAddress.correspondenceAddress.road.roadName = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_road_name_0']").prop('errorMessage')).toEqual(" ");
    });

    it('Road Type Code', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].currentAddress.correspondenceAddress.road.roadTypeCode = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_road_type_code_0']").prop('errorMessage')).toEqual(" ");
    });

    it('City/Suburb', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].currentAddress.correspondenceAddress.localityName = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_city_suburb_0']").prop('errorMessage')).toEqual(" ");
    });

    it('State', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].currentAddress.correspondenceAddress.state = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_state_0']").prop('errorMessage')).toEqual(" ");
    });

    it('Post Code', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[0].currentAddress.correspondenceAddress.postcode = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_post_code_0']").prop('errorMessage')).toEqual(" ");
    });

    it('Business Name - only for organisations', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.partyDetails[1].business.businessName = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_business_name_1']").prop('errorMessage')).toEqual(" ");
    });

    it('Unit Type - only for Unit/Apartment & Correspondence addresses', () => {
        let state = wrapper.state();
        state.addressTypes = state.addressTypes.map(addressType => 1);  //Set the address types to Unit/Apartment
        state.workspaceCreationRequest.partyDetails[0].currentAddress.streetAddress.subDwellingUnitType.unitTypeCode = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_unit_type_0']").prop('errorMessage')).toEqual(" ");
    });

    it('Unit Number - only for Unit/Apartment & Correspondence addresses', () => {
        let state = wrapper.state();
        state.addressTypes = state.addressTypes.map(addressType => 1);  //Set the address types to Unit/Apartment
        state.workspaceCreationRequest.partyDetails[0].currentAddress.streetAddress.subDwellingUnitType.unitNumber = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_unit_number_0']").prop('errorMessage')).toEqual(" ");
    });
})

describe("Transaction Details", () => {
    it('Parent Title', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.landTitleDetails.parentTitle = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_parent_title']").prop('errorMessage')).toEqual(" ");
    });

    it('Land Title Reference', () => {
        let state = wrapper.state();
        state.workspaceCreationRequest.landTitleDetails.landTitle[0].landTitleReference = null;
        wrapper.setState(state);

        expect(wrapper.find("[data-cy='pexa_land_title_reference']").prop('errorMessage')).toEqual(" ");
    });
})