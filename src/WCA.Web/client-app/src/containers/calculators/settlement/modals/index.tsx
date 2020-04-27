import * as React from 'react'
import { DefaultButton, PrimaryButton, IconButton } from 'office-ui-fabric-react/lib/Button';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar';

import AddAdjustment from './addAdjustment'
import MatterDetails from './matterDetails'
import ContractPrice from './contractPrice'
import ReleaseFee from './releaseFee';
import WaterUsage from './waterUsage';
import AdjustmentTemplate from './adjustmentTemplate';
import PenaltyInterest from './penaltyInterest';
import OtherAdjustment from './otherAdjustment';
import PayeeDetails from './payeeDetails';
import OtherAdjustmentDate from './otherAdjustmentDate';
import OurRequirements from './ourRequirements';
import AdditionalRequirements from './additionalRequirements';
import Fee from './fee';

import { ModalIDs } from 'containers/calculators/settlement/common';
import {
    ActionstepMatter,
    MatterDetails as MatterDetailsModel
} from 'utils/wcaApiTypes';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    modalID: ModalIDs;
    matterDetails: MatterDetailsModel;
    balanceFunds: number;
    index: number;
    actionstepData: ActionstepMatter;
    updatedState: any;
    dataInputError: string;

    closeModal: () => void;
    saveModal: () => void;
    deleteData: (index: number, modalID: ModalIDs) => void;
    updateValue: (newValue: any, whichValue: string, needRefresh?: boolean) => void;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class Modals extends React.Component<AppProps, AppStates> {

    private modalContent(): JSX.Element {
        const { modalID, actionstepData } = this.props;

        switch (modalID) {
            case ModalIDs.matterDetails:
                return (
                    <MatterDetails
                        updateValue={this.props.updateValue}
                        matterDetails={this.props.matterDetails}
                        actionstepData={actionstepData}
                    />
                );

            case ModalIDs.addAdjustment:
                return (
                    <AddAdjustment
                        updateValue={this.props.updateValue}
                    />
                );

            case ModalIDs.contractPrice:
                return (
                    <ContractPrice
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.releaseFee:
                return (
                    <ReleaseFee
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.waterUsage:
                return (
                    <WaterUsage
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.councilRates:
            case ModalIDs.waterAccessFee:
            case ModalIDs.sewerageAccessFee:
            case ModalIDs.administrationFund:
            case ModalIDs.sinkingFund:
            case ModalIDs.insurance:
            case ModalIDs.strataLevies:
            case ModalIDs.waterDrainageFee:
            case ModalIDs.parksCharge:
            case ModalIDs.waterServiceCharge:
            case ModalIDs.sewerageUsage:
            case ModalIDs.ownersCorporationFees:
            case ModalIDs.maintenanceFund:
            case ModalIDs.landTax:
            case ModalIDs.rent:
            case ModalIDs.councilRatesChargesLevies:
            case ModalIDs.waterRatesChargesLevies:
            case ModalIDs.sewerageServiceCharge:
            case ModalIDs.ownersAdministrationFundFee:
            case ModalIDs.ownersMaintenanceFundFee:
            case ModalIDs.ownersSinkingFundFee:
            case ModalIDs.ownersInsurance:
            case ModalIDs.waterSewerageRates:
            case ModalIDs.emergencyServicesLevy:
            case ModalIDs.waterAndSewerageRates:
                return (
                    <AdjustmentTemplate
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.penaltyInterest:
                return (
                    <PenaltyInterest
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.otherAdjustment:
                return (
                    <OtherAdjustment
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.otherAdjustmentDate:
                return (
                    <OtherAdjustmentDate
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.fee:
                return (
                    <Fee
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.additionalRequirements:
                return (
                    <AdditionalRequirements
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            case ModalIDs.payeeDetails:
                return (
                    <PayeeDetails
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                        balanceFunds={this.props.balanceFunds}
                    />
                );

            case ModalIDs.ourRequirements:
                return (
                    <OurRequirements
                        updateValue={this.props.updateValue}
                        updatedState={this.props.updatedState}
                    />
                );

            default:
                break;
        }

        return (
            <div></div>
        )
    }

    public render(): JSX.Element {
        let { index, modalID, dataInputError } = this.props;

        return (
            <div>
                <div className="modal-header">
                    <span className="modal-title">{modalID}</span>

                    <IconButton
                        className="modal-close-button"
                        onClick={() => this.props.closeModal()}
                        iconProps={{
                            iconName: 'ChromeClose',
                            style: {
                                fontSize: 12,
                                fontWeight: 500
                            }
                        }}
                    />
                </div>

                {dataInputError !== "" &&
                    <MessageBar
                        messageBarType={MessageBarType.severeWarning}
                    >
                        {dataInputError}
                    </MessageBar>
                }

                {this.modalContent()}

                <div className="modal-footer">

                    {index >= 0 && (modalID !== ModalIDs.contractPrice) &&
                        <DefaultButton
                            className="button red-button"
                            data-automation-id="delete_button"
                            data-cy="modal_delete_button"
                            text="Delete"
                            onClick={() => this.props.deleteData(index, modalID)}
                            allowDisabledFocus={true}
                        />
                    }

                    <DefaultButton
                        className="button gray-button"
                        data-automation-id="cancel_button"
                        data-cy="cancel_button"
                        text="Cancel"
                        onClick={() => this.props.closeModal()}
                        allowDisabledFocus={true}
                    />

                    <PrimaryButton
                        className="button"
                        data-automation-id="email_button"
                        text="OK"
                        data-cy="modal_save_button"
                        onClick={() => this.props.saveModal()}
                        allowDisabledFocus={true}
                    />
                </div>
            </div>
        )
    }
}