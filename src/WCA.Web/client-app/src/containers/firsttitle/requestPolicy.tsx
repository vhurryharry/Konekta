import * as React from 'react';
import { connect } from 'react-redux';

import QuestionnaireComponent from 'containers/firsttitle/components/questionnaireComponent';
import ConfirmationComponent from 'containers/firsttitle/components/confirmationComponent';
import PolicySuccess from 'containers/firsttitle/components/policySuccess';

import {
    getFirstTitlePolicyRequestFromActionstep,
    sendDataToFirstTitle
} from 'containers/firsttitle/redux/actions'

import { AppState, JwtMatterInfo, ReduxData, ReduxStatus } from 'app.types';

import {
    ActionstepMatterInfo,
    FirstTitlePolicyRequestFromActionstepResponse,
    SendFirstTitlePolicyRequestResponse,
    SendFirstTitlePolicyRequestQuery,
    RequestPolicyOptions,
    FTActionstepMatter,
    RiskInformation
} from 'utils/wcaApiTypes';

import Tools from 'utils/tools';
import LoadingWidget from 'components/common/loadingWidget';
import FirstTitleAuth from 'containers/firsttitle/auth/firstTitleAuth';

import './requestPolicy.css';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react';
import { PrimaryButton } from 'office-ui-fabric-react/lib/Button';

interface IMapDispatchToProps {
    getFirstTitlePolicyRequestFromActionstep: (params: ActionstepMatterInfo) => void;
    sendDataToFirstTitle: (params: FirstTitlePolicyRequestFromActionstepResponse) => void;
}

interface IMapStateToProps {
    firstTitlePolicyRequestFromActionstepResponse: ReduxData<FirstTitlePolicyRequestFromActionstepResponse> | undefined
    isValidCredentials: ReduxData<boolean> | undefined,
    sendFirstTitlePolicyRequestResponse: ReduxData<SendFirstTitlePolicyRequestResponse> | undefined,
    jwtMatterInfo: JwtMatterInfo | undefined,
    firstTitleConnected: boolean | null
}

interface IAppProps { }

type AppProps = IMapStateToProps & IMapDispatchToProps & IAppProps;

enum RequestPolicySteps {
    NotStarted = -1,
    Questionnaire = 0,
    Confirmation = 1,
    Requested = 2,
    Success = 3,
    Failed = 4,
    FirstTitleNotConnected = 5
}

interface AppStates {
    requestPolicyOptions: RequestPolicyOptions | undefined,
    actionstepMatter: FTActionstepMatter | undefined,
    sendFirstTitlePolicyRequestResponse: SendFirstTitlePolicyRequestResponse | undefined,
    currentStep: RequestPolicySteps,
    firstTitleConnected: boolean,
    matterInfo: ActionstepMatterInfo | null,
    error: string | undefined
}

class RequestPolicy extends React.Component<AppProps, AppStates> {
    constructor(props: AppProps) {
        super(props);

        this.state = {
            actionstepMatter: undefined,
            currentStep: RequestPolicySteps.NotStarted,
            firstTitleConnected: false,
            requestPolicyOptions: undefined,
            sendFirstTitlePolicyRequestResponse: undefined,
            matterInfo: null,
            error: undefined
        };
    }

    public componentDidMount(): void {
        this.requestMatterInformation();
    }

    public reviewForm = () => {
        this.setState({
            currentStep: RequestPolicySteps.Confirmation
        })
    };

    /** Place order function */
    public placeOrder = () => {
        const { requestPolicyOptions, actionstepMatter, matterInfo } = this.state;

        let params: SendFirstTitlePolicyRequestQuery = new SendFirstTitlePolicyRequestQuery({
            actionstepMatter,
            requestPolicyOptions,
            matterId: matterInfo!.matterId,
            actionstepOrg: matterInfo!.orgKey
        });

        this.props.sendDataToFirstTitle(params);

        this.setState({
            currentStep: RequestPolicySteps.Requested
        });
    }

    /**
     * Requests the Matter information from Actionstep API
     */
    public requestMatterInformation = () => {
        const { jwtMatterInfo } = this.props;
        let matterInfo: ActionstepMatterInfo | null = null;

        this.setState({
            firstTitleConnected: true,
            currentStep: RequestPolicySteps.NotStarted
        });

        if (jwtMatterInfo === undefined) {

            const queryString = require('query-string');

            const urlParams = queryString.parse(window.location.search);

            matterInfo = Tools.ParseActionstepMatterInfo(urlParams);

        } else {
            matterInfo = new ActionstepMatterInfo({
                orgKey: jwtMatterInfo.orgKey,
                matterId: jwtMatterInfo.matterId
            });
        }

        if (matterInfo == null) {
            return;
        }

        this.setState({
            matterInfo
        });

        /** Sending the request for information about matter information **/
        this.props.getFirstTitlePolicyRequestFromActionstep(matterInfo);
    };

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (prevState.firstTitleConnected) {
            if (prevState.currentStep === RequestPolicySteps.NotStarted && nextProps.firstTitlePolicyRequestFromActionstepResponse) {
                if (nextProps.firstTitlePolicyRequestFromActionstepResponse.status === ReduxStatus.Success) {
                    nextState.currentStep = RequestPolicySteps.Questionnaire;

                    nextState.actionstepMatter = FTActionstepMatter.fromJS(nextProps.firstTitlePolicyRequestFromActionstepResponse.data!.actionstepData);
                    nextState.requestPolicyOptions = RequestPolicyOptions.fromJS(nextProps.firstTitlePolicyRequestFromActionstepResponse.data!.requestPolicyOptions);

                    nextState.requestPolicyOptions.riskInformation = nextState.requestPolicyOptions.riskInformation || new RiskInformation();

                } else if (nextProps.firstTitlePolicyRequestFromActionstepResponse.status === ReduxStatus.Failed) {
                    nextState.currentStep = RequestPolicySteps.Failed;
                    nextState.error = nextProps.firstTitlePolicyRequestFromActionstepResponse.error!.message;

                    if (!nextProps.firstTitleConnected) {
                        nextState.currentStep = RequestPolicySteps.FirstTitleNotConnected;

                        nextState.firstTitleConnected = false;
                    }
                }
            }

            if (prevState.currentStep === RequestPolicySteps.Requested && nextProps.sendFirstTitlePolicyRequestResponse) {
                if (nextProps.sendFirstTitlePolicyRequestResponse.status === ReduxStatus.Success) {
                    nextState.currentStep = RequestPolicySteps.Success;
                    nextState.sendFirstTitlePolicyRequestResponse = nextProps.sendFirstTitlePolicyRequestResponse.data!;
                } else if (nextProps.sendFirstTitlePolicyRequestResponse.status === ReduxStatus.Failed) {
                    nextState.currentStep = RequestPolicySteps.Failed;
                    nextState.error = nextProps.sendFirstTitlePolicyRequestResponse.error!.message;

                    if (!nextProps.firstTitleConnected) {
                        nextState.currentStep = RequestPolicySteps.FirstTitleNotConnected;

                        nextState.firstTitleConnected = false;
                    }
                }
            }
        }

        return nextState;
    }

    backToForm = () => {
        this.setState({
            currentStep: RequestPolicySteps.Questionnaire
        })
    };

    setKnownRiskPolicyProperty = (propName: string, propVal: any | undefined) => {
        let newRequestPolicyOptions: RequestPolicyOptions = RequestPolicyOptions.fromJS({ ...this.state.requestPolicyOptions });

        (newRequestPolicyOptions.riskInformation! as any)[propName] = propVal;

        this.setState({
            requestPolicyOptions: newRequestPolicyOptions
        });
    };

    setRequestPolicyOptions = (propName: string, propVal: boolean) => {
        let newRequestPolicyOptions = new RequestPolicyOptions(this.state.requestPolicyOptions);

        (newRequestPolicyOptions as any)[propName] = propVal;

        this.setState({
            requestPolicyOptions: newRequestPolicyOptions
        });
    };

    setMatterInformation = (keyPath: string, newValue: any) => {
        let newActionstepMatter = this.state.actionstepMatter;

        if (typeof newValue === "string" && newValue === "") newValue = null;

        Tools.assign(newActionstepMatter, keyPath, newValue);

        this.setState({
            actionstepMatter: newActionstepMatter
        });
    }

    render(): JSX.Element {
        const { currentStep, actionstepMatter, requestPolicyOptions, sendFirstTitlePolicyRequestResponse, error } = this.state;

        if (currentStep === RequestPolicySteps.NotStarted ||
            currentStep === RequestPolicySteps.Requested) {
            return (
                <LoadingWidget />
            );
        }

        return (
            <div className="wrapper vertical-container wrapper-content animated fadeIn" dir="ltr">
                <div className="firsttitle-page">
                    {currentStep === RequestPolicySteps.FirstTitleNotConnected &&
                        <FirstTitleAuth onConnection={this.requestMatterInformation} />
                    }

                    {currentStep === RequestPolicySteps.Questionnaire &&
                        <QuestionnaireComponent
                            requestPolicyOptions={requestPolicyOptions!}
                            actionstepMatter={actionstepMatter!}
                            reviewForm={this.reviewForm}
                            placeOrder={this.placeOrder}
                            setKnownRiskPolicyProperty={this.setKnownRiskPolicyProperty}
                            setRequestPolicyOptions={this.setRequestPolicyOptions}
                            setMatterInformation={this.setMatterInformation}
                        />
                    }

                    {currentStep === RequestPolicySteps.Confirmation &&
                        <ConfirmationComponent
                            requestPolicyOptions={requestPolicyOptions!}
                            actionstepMatter={actionstepMatter!}
                            backOnClick={this.backToForm}
                            placeOrder={this.placeOrder}
                        />
                    }

                    {currentStep === RequestPolicySteps.Success &&
                        <PolicySuccess
                            sendFirstTitlePolicyRequestResponse={sendFirstTitlePolicyRequestResponse!}
                        />
                    }

                    {currentStep === RequestPolicySteps.Failed &&
                        <>
                            <MessageBar messageBarType={MessageBarType.error}>
                                {error ? error : "Unexpected error occured!"}
                            </MessageBar>
                            <br />
                            <PrimaryButton
                                className="button"
                                text="Back"
                                onClick={() => this.backToForm()}
                                allowDisabledFocus={true}
                            />
                        </>
                    }
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state: AppState) => {
    return {
        firstTitlePolicyRequestFromActionstepResponse: state.firstTitle.firstTitlePolicyRequestFromActionstepResponse,
        isValidCredentials: state.firstTitle.isValidCredentials,
        sendFirstTitlePolicyRequestResponse: state.firstTitle.sendFirstTitlePolicyRequestResponse,
        jwtMatterInfo: state.common.jwtMatterInfo,
        firstTitleConnected: state.common.firstTitleConnected
    }
}

const mapDispatchToProps = {
    getFirstTitlePolicyRequestFromActionstep,
    sendDataToFirstTitle
}

export default connect(mapStateToProps, mapDispatchToProps)(RequestPolicy);