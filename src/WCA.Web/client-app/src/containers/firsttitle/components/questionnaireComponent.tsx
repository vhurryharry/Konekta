import * as React from 'react'
import QuestionSection from './questionSection';

import { PrimaryButton, IButtonStyles } from 'office-ui-fabric-react/lib/Button';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';
import { MessageBar, MessageBarType, DatePicker } from 'office-ui-fabric-react';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Dropdown } from 'office-ui-fabric-react/lib/Dropdown';

import { NameTitles, PropertyTitleTypes, TitleInfoTypes } from './constants'

import { RequestPolicyOptions, FTActionstepMatter, IdentityType, FTParty, FTTitle, TitleInfoType } from 'utils/wcaApiTypes';
import FTAddressInput from 'containers/firsttitle/components/fTAddressInput';
import { _onFormatDate } from 'utils/dataFormatter';

interface IProps {
    requestPolicyOptions: RequestPolicyOptions;
    actionstepMatter: FTActionstepMatter;
    reviewForm: () => void;
    placeOrder: () => void;
    setKnownRiskPolicyProperty: (key: string, val: any) => void;
    setRequestPolicyOptions: (key: string, val: boolean) => void;
    setMatterInformation: (keyPath: string, newValue: any) => void;
}

const buttonStyles: IButtonStyles = {
    root: {
        backgroundColor: '#88c03f'
    },
    rootHovered: {
        backgroundColor: '#3e3e3e'
    }
}

export default class QuestionnaireComponent extends React.Component<IProps, {}> {
    constructor(props: IProps) {
        super(props);

        this.state = {};
    }

    private checkValidation(): boolean {
        const { requestPolicyOptions, actionstepMatter } = this.props;

        if (!this.validateRequestOptions(requestPolicyOptions)) {
            return false;
        }

        if (!actionstepMatter.title || !this.validateLandTitle(actionstepMatter.title)) {
            return false;
        }

        if (!actionstepMatter.sourceProperty || !this.validateAddress(actionstepMatter.sourceProperty)) {
            return false;
        }

        if (!actionstepMatter.buyers) {
            return false;
        }

        for (let i = 0; i < actionstepMatter.buyers.length; i++) {
            if (!this.validateParty(actionstepMatter.buyers[i])) {
                return false;
            }
        }

        return true;
    }

    private validateLandTitle(landTitle: FTTitle) {
        if (landTitle.titleInfoType === TitleInfoType.Reference) {
            if (!landTitle.titleReference) {
                return false;
            }
        } else {
            if (!landTitle.titleVolume || !landTitle.titleFolio) {
                return false;
            }
        }

        return true;
    }

    private validateRequestOptions(requestPolicyOptions: RequestPolicyOptions) {
        if (!requestPolicyOptions.requestKnownRiskPolicies) return true;

        const { riskInformation } = this.props.requestPolicyOptions;

        let countYes = 0;
        for (let key in riskInformation) {
            const value = (riskInformation as any)[key];

            if (value === undefined) {
                return false;
            }

            if (value === true) {
                countYes++;
            }
        }

        if (countYes === 0 || countYes > 3) {
            return false;
        }

        return true;
    }

    private validateParty(party: FTParty): boolean {
        if (!this.validateAddress(party)) {
            return false;
        }

        if (party.identityType === IdentityType.Company) {
            if (!party.companyName) {
                return false;
            }

            if (!party.abn && !party.acn && !party.abrn) {
                return false;
            }
        } else {
            if (!party.firstName || !party.lastName || !party.title || !party.emailAddress) {
                return false;
            }
        }
        return true;
    }

    private validateAddress(party: FTParty): boolean {
        if (!party.city || !party.stateProvince || !party.postCode || !party.country || !party.streetType) {
            return false;
        }

        return true;
    }

    private isValid(value: any): boolean {
        if (value == null)
            return false;

        if (Object.prototype.toString.call(value) === '[object Date]') {
            return !isNaN(value.getTime());
        }

        if (typeof value == "string") {
            if (value === "") return false;
        }

        return true;
    }

    public render(): JSX.Element {
        const { actionstepMatter, requestPolicyOptions,
            setKnownRiskPolicyProperty, setRequestPolicyOptions, setMatterInformation } = this.props;
        const { riskInformation } = requestPolicyOptions;
        const canProceed = this.checkValidation();
        return (
            <>
                <h2>REQUEST POLICY OPTIONS</h2>

                <div className="ft-section">
                    <div className="ft-section-body">
                        <Checkbox
                            className="ft-data-field"
                            label="Request Policy Document"
                            checked={requestPolicyOptions.requestPolicyDocument}
                            onChange={(ev, checked) => setRequestPolicyOptions("requestPolicyDocument", checked === true)}
                        />

                        <Checkbox
                            className="ft-data-field"
                            label="Request Policies for Vacant Land"
                            checked={requestPolicyOptions.requestPoliciesForVacantLand}
                            onChange={(ev, checked) => setRequestPolicyOptions("requestPoliciesForVacantLand", checked === true)}
                        />

                        <Checkbox
                            label="Request Known Risk Policies"
                            checked={requestPolicyOptions.requestKnownRiskPolicies}
                            onChange={(ev, checked) => setRequestPolicyOptions("requestKnownRiskPolicies", checked === true)}
                        />

                        <fieldset
                            disabled={!requestPolicyOptions.requestKnownRiskPolicies}
                            style={!requestPolicyOptions.requestKnownRiskPolicies ? { pointerEvents: "none", opacity: "0.4" } : {}}
                            className="known-risk-policies"
                        >

                            <MessageBar messageBarType={MessageBarType.info}>
                                You can only select 1-3 known risks.
                            </MessageBar>

                            <QuestionSection
                                heading={"Unapproved Works: "}
                                keyStr={"unapprovedWorks"}
                                initialValue={riskInformation!.unapprovedWorks}
                                setRequestPolicyProperty={setKnownRiskPolicyProperty}
                            />
                            <QuestionSection
                                heading={"Encroaching Structures: "}
                                keyStr={"encroachingStructures"}
                                initialValue={riskInformation!.encroachingStructures}
                                setRequestPolicyProperty={setKnownRiskPolicyProperty}
                            />
                            <QuestionSection
                                heading={"Structure Over Sewerage and Drainage: "}
                                keyStr={"structureOverSewerageAndDrainage"}
                                initialValue={riskInformation!.structureOverSewerageAndDrainage}
                                setRequestPolicyProperty={setKnownRiskPolicyProperty}
                            />
                            <QuestionSection
                                heading={"Sewer and Drainage Connection Without an Easement: "}
                                keyStr={"sewerAndDrainageConnectionWithoutAnEasement"}
                                initialValue={riskInformation!.sewerAndDrainageConnectionWithoutAnEasement}
                                setRequestPolicyProperty={setKnownRiskPolicyProperty}
                            />
                            <QuestionSection
                                heading={"Incomplete or Inaccurate Sewer and Drainage Diagram: "}
                                keyStr={"incompleteOrInaccurateSewerAndDrainageDiagram"}
                                initialValue={riskInformation!.incompleteOrInaccurateSewerAndDrainageDiagram}
                                setRequestPolicyProperty={setKnownRiskPolicyProperty}
                            />

                            <TextField className="ft-data-field"
                                label="Known Risk"
                                value={riskInformation!.knownRisk || ""}
                                errorMessage={this.isValid(riskInformation!.knownRisk) ? "" : " "}
                                onChange={(ev, newValue) => setKnownRiskPolicyProperty(`knownRisk`, newValue)}
                            />
                        </fieldset>
                    </div>
                </div>

                <div className="divider" />

                <h2>REQUEST POLICY FORM</h2>

                <div className="ft-section">
                    <div className="ft-section-body ms-Grid-row">
                        <Dropdown
                            className="ms-Grid-col ms-sm12 ms-md6"
                            label="Is this property Strata or Community Title?"
                            options={PropertyTitleTypes}
                            placeholder="Please select..."
                            selectedKey={actionstepMatter.propertyTitleType}
                            errorMessage={this.isValid(actionstepMatter.propertyTitleType) ? "" : " "}
                            onChange={(event, item) => setMatterInformation(`propertyTitleType`, item!.key.toString())}
                        />

                        <DatePicker
                            label="Settlement Date"
                            className="ms-Grid-col ms-sm12 ms-md6"
                            showMonthPickerAsOverlay={true}
                            allowTextInput={true}
                            value={new Date(actionstepMatter.settlementDate!)}
                            placeholder="Please select..."
                            formatDate={_onFormatDate}
                            onSelectDate={(newDate) => setMatterInformation("settlementDate", newDate)}
                        />
                    </div>
                </div>

                <div className="ft-section">
                    <div className="ft-section-body ms-Grid-row">
                        <div className="ms-Grid-col ms-sm12">
                            <h3>Application / Financial Segment / Asset / Real Estate / Location</h3>
                        </div>

                        <Dropdown
                            className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                            label="Land Title Type"
                            options={TitleInfoTypes}
                            placeholder="Please select..."
                            selectedKey={actionstepMatter.title!.titleInfoType}
                            errorMessage={this.isValid(actionstepMatter.title!.titleInfoType) ? "" : " "}
                            onChange={(event, item) => setMatterInformation(`title.titleInfoType`, item!.key.toString())}
                        />

                        <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                            label="Title Reference"
                            disabled={actionstepMatter.title!.titleInfoType !== "Reference"}
                            value={actionstepMatter.title!.titleReference || ""}
                            errorMessage={(actionstepMatter.title!.titleInfoType !== "Reference" || this.isValid(actionstepMatter.title!.titleReference)) ? "" : " "}
                            onChange={(ev, newValue) => setMatterInformation(`title.titleReference`, newValue)}
                        />

                        <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                            label="Title Volumn"
                            disabled={actionstepMatter.title!.titleInfoType !== "VolumeFolio"}
                            value={actionstepMatter.title!.titleVolume || ""}
                            errorMessage={(actionstepMatter.title!.titleInfoType !== "VolumeFolio" || this.isValid(actionstepMatter.title!.titleVolume)) ? "" : " "}
                            onChange={(ev, newValue) => setMatterInformation(`title.titleVolume`, newValue)}
                        />

                        <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                            label="Title Folio"
                            disabled={actionstepMatter.title!.titleInfoType !== "VolumeFolio"}
                            value={actionstepMatter.title!.titleFolio || ""}
                            errorMessage={(actionstepMatter.title!.titleInfoType !== "VolumeFolio" || this.isValid(actionstepMatter.title!.titleFolio)) ? "" : " "}
                            onChange={(ev, newValue) => setMatterInformation(`title.titleFolio`, newValue)}
                        />

                        <TextField className="ms-Grid-col ms-sm12"
                            label="Legal Description"
                            value={actionstepMatter.title!.legalDescription || ""}
                            errorMessage={this.isValid(actionstepMatter.title!.legalDescription) ? "" : " "}
                            onChange={(ev, newValue) => setMatterInformation(`title.legalDescription`, newValue)}
                        />

                        <FTAddressInput
                            prefix="source_property_"
                            party={actionstepMatter.sourceProperty!}
                            isValid={this.isValid}
                            setMatterInformation={(keyPath, newValue) => setMatterInformation("sourceProperty." + keyPath, newValue)}
                        />
                    </div>
                </div>

                <div className="ft-section">
                    <div className="ft-section-body ms-Grid-row">
                        <div className="ms-Grid-col ms-sm12">
                            <h3>Related Party Segments</h3>
                        </div>

                        {actionstepMatter.buyers && actionstepMatter.buyers.map((buyer, index) => {
                            return (
                                <div className="ft-party" key={index}>
                                    {buyer.identityType === IdentityType.Individual &&
                                        <>
                                            <h3 className="ms-Grid-col ms-sm12">
                                                Individual : {buyer.firstName} {buyer.lastName}
                                            </h3>

                                            <Dropdown className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                                                label="Name Title"
                                                id={`buyers_${index}_name_title`}
                                                options={NameTitles}
                                                placeholder="Please select..."
                                                selectedKey={buyer.title}
                                                errorMessage={this.isValid(buyer.title) ? "" : " "}
                                                onChange={(event, item) => setMatterInformation(`buyers.${index}.title`, item!.key.toString())}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                                                label="First Name"
                                                id={`buyers_${index}_first_name`}
                                                value={buyer.firstName || ""}
                                                errorMessage={this.isValid(buyer.firstName) ? "" : " "}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.firstName`, newValue)}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                                                label="Last Name"
                                                id={`buyers_${index}_last_name`}
                                                value={buyer.lastName || ""}
                                                errorMessage={this.isValid(buyer.lastName) ? "" : " "}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.lastName`, newValue)}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                                                label="Email Address"
                                                id={`buyers_${index}_email_address`}
                                                value={buyer.emailAddress || ""}
                                                errorMessage={this.isValid(buyer.emailAddress) ? "" : " "}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.emailAddress`, newValue)}
                                            />
                                        </>
                                    }

                                    {buyer.identityType === IdentityType.Company &&
                                        <>
                                            <h3 className="ms-Grid-col ms-sm12">
                                                Company : {buyer.companyName}
                                            </h3>

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6"
                                                label="Company Name"
                                                id={`buyers_${index}_company_name`}
                                                value={buyer.companyName || ""}
                                                errorMessage={this.isValid(buyer.companyName) ? "" : " "}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.companyName`, newValue)}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6"
                                                label="Email Address"
                                                id={`buyers_${index}_email_address`}
                                                value={buyer.emailAddress || ""}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.emailAddress`, newValue)}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg4"
                                                label="ABN"
                                                id={`buyers_${index}_abn`}
                                                value={buyer.abn || ""}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.abn`, newValue)}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg4"
                                                label="ACN"
                                                id={`buyers_${index}_acn`}
                                                value={buyer.acn || ""}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.acn`, newValue)}
                                            />

                                            <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg4"
                                                label="ABRN"
                                                id={`buyers_${index}_abrn`}
                                                value={buyer.abrn || ""}
                                                onChange={(ev, newValue) => setMatterInformation(`buyers.${index}.abrn`, newValue)}
                                            />

                                            <div className="ms-Grid-col ms-sm12">
                                                You have to provide one of these numbers.
                                            </div>
                                        </>
                                    }

                                    <FTAddressInput
                                        prefix={`buyers_${index}_`}
                                        party={buyer}
                                        isValid={this.isValid}
                                        setMatterInformation={(keyPath, newValue) => setMatterInformation(`buyers.${index}.${keyPath}`, newValue)}
                                    />
                                </div>
                            )
                        })}
                    </div>
                </div>

                {/* <div className="first-title-button-section">
                    <PrimaryButton
                        styles={buttonStyles}
                        className="button"
                        data-cy="review_button"
                        text="Review"
                        onClick={this.props.reviewForm}
                        allowDisabledFocus={true}
                        disabled={!canProceed}
                    />
                </div> */}

                <div className="first-title-button-section">
                    <PrimaryButton
                        styles={buttonStyles}
                        className="button"
                        data-cy="submit_button"
                        text="Place Order"
                        onClick={this.props.placeOrder}
                        allowDisabledFocus={true}
                        disabled={!canProceed}
                    />
                </div>
            </>
        )
    }
}