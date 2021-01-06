import * as React from 'react';

import {
    SubwayNav,
    SubwayNavNodeState,
    ISubwayNavNodeProps,
    setSubwayState
} from 'components/SubwayNav'

import RetrieveFromActionstep from 'containers/pexa/steps/retrieveFromActionstep';
import ConveyancingDataCheck from 'containers/pexa/steps/conveyancingDataCheck';
import SendToPexa from 'containers/pexa/steps/sendToPexa';
import DataCheckByPexa from 'containers/pexa/steps/dataCheckByPexa';
import WorkspaceCreated from 'containers/pexa/steps/workspaceCreated';

import "./createWorkspace.css"

interface IAppProps {
}

type AppProps = IAppProps;

export default class CreatePexaWorkspace extends React.Component<AppProps, any> {
    steps: ISubwayNavNodeProps[] = [];

    constructor(props: AppProps) {
        super(props);

        this.steps = [
            {
                id: '0',
                label: 'Retrieve from Actionstep',
                state: SubwayNavNodeState.Current,
                onClickStep: this._handleClickStep,
                disabled: true
            },
            {
                id: '1',
                label: 'Conveyancing Data Check',
                state: SubwayNavNodeState.NotStarted,
                onClickStep: this._handleClickStep,
                disabled: true
            },
            {
                id: '2',
                label: 'Send to PEXA',
                state: SubwayNavNodeState.NotStarted,
                onClickStep: this._handleClickStep,
                disabled: true
            },
            {
                id: '3',
                label: 'Data Check by PEXA',
                state: SubwayNavNodeState.NotStarted,
                onClickStep: this._handleClickStep,
                disabled: true
            },
            {
                id: '4',
                label: 'Workspace Created',
                state: SubwayNavNodeState.NotStarted,
                onClickStep: this._handleClickStep,
                disabled: true
            }
        ];

        this.state = {
            steps: this.steps,
            currentStepId: 0
        };
    }

    public componentDidMount(): void {
    }

    private _handleClickStep = (step: ISubwayNavNodeProps): void => {
        this.setState({ ...setSubwayState(step, this.state.steps, this.state.currentStepId) });
    }

    private _onChangeStep = (newState: SubwayNavNodeState, newStep: number = -1): void => {
        let newSteps = [...this.state.steps];
        let currentStepId = this.state.currentStepId;

        if (newStep >= 0) {
            currentStepId = newStep;
        }
        for (let i = 0; i < currentStepId; i++) {
            newSteps[i].state = SubwayNavNodeState.Completed;
        }
        for (let i = currentStepId + 1; i < newSteps.length; i++) {
            newSteps[i].state = SubwayNavNodeState.NotStarted;
        }

        newSteps[currentStepId].state = newState;
        if (newState === SubwayNavNodeState.Completed && currentStepId < newSteps.length - 1) {
            currentStepId++;
            newSteps[currentStepId].state = SubwayNavNodeState.Current;
        }

        this.setState({
            steps: newSteps,
            currentStepId: currentStepId
        })

        window.scrollTo(0, 0);
    }

    render() {
        const { currentStepId, steps } = this.state;

        return (
            <div>
                <div className="wrapper vertical-container wrapper-content animated fadeIn" dir="ltr">

                    <div className="ms-Grid-row create-pexa-workspace">
                        <div className="create-pexa-step ms-Grid-col ms-lg3">
                            <SubwayNav steps={steps} />
                        </div>

                        <div className="create-pexa-content ms-Grid-col ms-lg9">
                            {currentStepId === 0 && <RetrieveFromActionstep onChangeStep={this._onChangeStep} />}

                            {currentStepId === 1 &&
                                <ConveyancingDataCheck onChangeStep={this._onChangeStep} />
                            }

                            {currentStepId === 2 && <SendToPexa onChangeStep={this._onChangeStep} />}

                            {currentStepId === 3 && <DataCheckByPexa onChangeStep={this._onChangeStep} />}

                            {currentStepId === 4 &&
                                <WorkspaceCreated onChangeStep={this._onChangeStep} />
                            }
                        </div>
                    </div>

                </div>
            </div>
        );
    }
}
