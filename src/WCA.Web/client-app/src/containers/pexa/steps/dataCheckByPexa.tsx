import * as React from 'react';
import { connect } from 'react-redux';

import { DefaultButton } from 'office-ui-fabric-react';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react';

import { WorkspaceCreationRequest, ErrorViewModel } from 'utils/wcaApiTypes';
import { AppState } from 'app.types';

import { SubwayNavNodeState } from 'components/SubwayNav';
import { setPexaWorkspaceCreation } from 'containers/pexa/redux/actions';
import { WorkspaceCreationRequestWithMatterInfo } from 'containers/pexa/redux/actionTypes';

interface IMapStateToProps {
    pexaFormData: WorkspaceCreationRequestWithMatterInfo | undefined;
    success: boolean;
    error: ErrorViewModel | undefined;
}

interface IMapDispatchToProps {
    setPexaWorkspaceCreation: (data: WorkspaceCreationRequest) => void;
}

interface IAppProps {
    onChangeStep: (newState: SubwayNavNodeState, newStep: number) => void;
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

export class DataCheckByPexa extends React.Component<AppProps> {

    private goBackToDataInput(): void {
        const { pexaFormData } = this.props;
        if (pexaFormData) {
            this.props.setPexaWorkspaceCreation(pexaFormData.workspaceCreationRequest);
        }

        this.props.onChangeStep(SubwayNavNodeState.Current, 1);
    }

    render() {
        const { success, error } = this.props;

        return (
            <>

                {!success && error && error.errorList &&
                    <div>
                        <h3>
                            {error.errorList.length > 1 ? "PEXA returned the following errors:" : "PEXA returned the following error:"}
                        </h3>
                        <br />

                        {error.errorList.map((item, index) => (
                            <MessageBar messageBarType={MessageBarType.error} isMultiline={false} key={index}>
                                {item}
                            </MessageBar>
                        ))}
                    </div>
                }

                <br />

                <DefaultButton
                    className="button"
                    data-automation-id="go_back_button"
                    data-cy="go_back_button"
                    text="Go Back"
                    onClick={() => this.goBackToDataInput()}
                    allowDisabledFocus={true}
                />

            </>
        );
    }

}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        pexaFormData: state.pexa.pexaFormData,
        success: state.pexa.success,
        error: state.pexa.error
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    setPexaWorkspaceCreation
}

export default connect(mapStateToProps, mapDispatchToProps)(DataCheckByPexa);