import * as React from 'react'
import { PrimaryButton, IButtonStyles } from 'office-ui-fabric-react/lib/Button';

// Components
import ConfirmationSection from './confirmationSection';

import './style.css';
import { IIconProps } from 'office-ui-fabric-react';
import { RequestPolicyOptions, FTActionstepMatter } from 'utils/wcaApiTypes';

interface IProps {
    backOnClick?: () => void;
    placeOrder: () => void;
    actionstepMatter: FTActionstepMatter;
    requestPolicyOptions: RequestPolicyOptions;
}

const buttonStyles: IButtonStyles = {
    root: {
        backgroundColor: '#88c03f'
    },
    rootHovered: {
        backgroundColor: '#3e3e3e'
    }
}

interface IState { }

const editIcon: IIconProps = { iconName: 'edit' };

export default class ConfirmationComponent extends React.Component<IProps, IState> {
    render() {
        const { requestPolicyOptions } = this.props;
        const { riskInformation } = requestPolicyOptions;

        return (
            <>
                <h2>
                    CONFIRMATION
                </h2>

                <div className="section">
                    <div className="section-body">
                        <ConfirmationSection
                            heading={"Request Policy Document"}
                            value={requestPolicyOptions.requestPolicyDocument ? 'Yes' : 'No'}
                        />
                    </div>
                </div>

                <div className="section">
                    <div className="section-body">

                        <ConfirmationSection
                            heading={"Request Known Risk Policies"}
                            value={requestPolicyOptions.requestKnownRiskPolicies ? 'Yes' : 'No'}
                        />

                        {requestPolicyOptions.requestKnownRiskPolicies &&
                            <div className="known-risk-policies">
                                <ConfirmationSection
                                    heading={"Unapproved Works"}
                                    value={riskInformation!.unapprovedWorks ? 'Yes' : 'No'}
                                />

                                <ConfirmationSection
                                    heading={"Encroaching Structures"}
                                    value={riskInformation!.encroachingStructures ? 'Yes' : 'No'}
                                />

                                <ConfirmationSection
                                    heading={"Structures over Sewage and Drainage"}
                                    value={riskInformation!.structureOverSewerageAndDrainage ? 'Yes' : 'No'}
                                />

                                <ConfirmationSection
                                    heading={"Sewer and Drainage Connection Without an Easement"}
                                    value={riskInformation!.sewerAndDrainageConnectionWithoutAnEasement ? 'Yes' : 'No'}
                                />

                                <ConfirmationSection
                                    heading={"Incomplete or Inaccurate Sewer and Drainage Diagram"}
                                    value={riskInformation!.incompleteOrInaccurateSewerAndDrainageDiagram ? 'Yes' : 'No'}
                                />
                            </div>
                        }
                    </div>
                </div>

                <div className="section">
                    <div className="section-body">
                        <ConfirmationSection
                            heading={"Request Policies for Vacant Land"}
                            value={requestPolicyOptions.requestPoliciesForVacantLand ? 'Yes' : 'No'}
                        />
                    </div>
                </div>

                <div className="first-title-button-section">
                    <PrimaryButton
                        styles={buttonStyles}
                        className="button"
                        data-cy="back_button"
                        iconProps={editIcon}
                        text="Edit"
                        onClick={this.props.backOnClick}
                        allowDisabledFocus={true}
                    />
                    <PrimaryButton
                        styles={buttonStyles}
                        className="button confirm_btn"
                        data-cy="placeotder_btn"
                        text="Place order"
                        onClick={this.props.placeOrder}
                        allowDisabledFocus={true}
                    />
                </div>
            </>
        )
    }
}