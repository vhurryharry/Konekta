import * as React from 'react'
import { connect } from 'react-redux';

import { AppState, JwtMatterInfo, ReduxData, ReduxStatus } from 'app.types';
import {
    ActionstepMatterInfo,
    RequestPropertyInformationFromActionstepResponse
} from 'utils/wcaApiTypes';

import { getPropertyInformation } from 'containers/globalx/redux/actions'

import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar';
import ActionstepToGlobalX from 'containers/globalx/pages/components/actionstepToGlobalX'

import "containers/globalx/globalx.css"

interface IMapStateToProps {
    jwtMatterInfo: JwtMatterInfo | undefined;
    propertyInformation: ReduxData<RequestPropertyInformationFromActionstepResponse> | undefined;
}

interface IMapDispatchToProps {
    getPropertyInformation: (matterInfo: ActionstepMatterInfo, entryPoint: string, embed: boolean) => void;
}

type AppProps = IMapStateToProps & IMapDispatchToProps;

type AppStates = {
    dataLoaded: boolean,
    gxUri: string,
    gxVersion: string | null,
    gxMatter: string | null,    // GlobalX Matter
    errorMessage: string | null
}

interface ICdmIntegrationReadyMessage {
    data: {
        action: string,
        message: string
    },
    sourceOrigin: string,
    targetOrigin: string,
    sourceWindowName: string,
    targetWindowName: string
}

interface ICdmIntegrationMessage {
    data: {
        action: string,
        cdm: string | null,
        version: string | null
    }
}

export class GXPropertyInformation extends React.Component<AppProps, AppStates> {
    iframeRef: HTMLIFrameElement | null = null;

    constructor(props: AppProps) {
        super(props);

        this.state = {
            dataLoaded: false,
            gxUri: "",
            gxVersion: null,
            gxMatter: null,
            errorMessage: null
        }

        window.addEventListener("message", this.onReceiveMessage)
    }

    componentDidMount() {
        const { jwtMatterInfo } = this.props;
        let matterInfo: ActionstepMatterInfo | null = null;

        if (jwtMatterInfo) {
            matterInfo = new ActionstepMatterInfo({
                orgKey: jwtMatterInfo.orgKey,
                matterId: jwtMatterInfo.matterId
            });
        }

        if (matterInfo === null) {
            this.setState({
                errorMessage: "No matter selected",
                gxUri: "/",
                dataLoaded: true
            });

            return;
        }

        const urlParams = new URLSearchParams(window.location.search);
        const entryPoint = urlParams.get('entryPoint') ?? '';
        const embed = urlParams.get('embed') === 'true' ? true : false;

        this.props.getPropertyInformation(matterInfo, entryPoint, embed);
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (nextProps.propertyInformation) {
            if (nextProps.propertyInformation.status === ReduxStatus.Success) {
                nextState.dataLoaded = true;
                nextState.gxUri = nextProps.propertyInformation.data!.gxUri!;
                nextState.gxMatter = nextProps.propertyInformation.data!.matter!;
                nextState.gxVersion = nextProps.propertyInformation.data!.version!;
            } else if (nextProps.propertyInformation.status === ReduxStatus.Failed) {
                nextState.dataLoaded = true;
                nextState.gxUri = "/";
                nextState.gxMatter = null;
                nextState.gxVersion = null;
                nextState.errorMessage = nextProps.propertyInformation.error!.message!;
            }
        }

        return nextState;
    }

    render(): JSX.Element {
        const { dataLoaded, gxUri, errorMessage } = this.state;

        return (
            <div className="gx-wrapper">
                {!dataLoaded ?
                    <ActionstepToGlobalX />
                    :
                    errorMessage ?
                        <MessageBar messageBarType={MessageBarType.error}>
                            {errorMessage}
                        </MessageBar>
                        :
                        <iframe src={gxUri}
                            ref={ref => this.iframeRef = ref}
                            onLoad={() => this.onLoadIframe()}
                            className="gx-iframe"
                            title="Property Information"
                        />
                }
            </div>
        )
    }

    onReceiveMessage = (event: MessageEvent) => {
        if (event.origin.includes("globalx.com.au")) {
            console.log("Message from GlobalX: ", event);

            try {
                const message: ICdmIntegrationReadyMessage = JSON.parse(event.data);

                if (message && message.data && message.data.action === "CdmIntegrationReady") {
                    if (this.iframeRef && this.iframeRef.contentWindow) {

                        const { gxMatter, gxVersion } = this.state;

                        const cdmMessage: ICdmIntegrationMessage = {
                            data: {
                                "action": "CdmIntegration",     // notify GlobalX to perform search integration
                                "cdm": gxMatter,                // search data (matter) for integration with GlobalX
                                "version": gxVersion            // search data version
                            }
                        };

                        this.iframeRef.contentWindow.postMessage(JSON.stringify(cdmMessage), '*');
                    }
                }
            } catch (e) {
                console.log('Konekta: Error populating form data', e);
            }
        } else { }
    }

    onLoadIframe = () => {
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        jwtMatterInfo: state.common.jwtMatterInfo,
        propertyInformation: state.globalx.propertyInformation
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    getPropertyInformation
}

export default connect(mapStateToProps, mapDispatchToProps)(GXPropertyInformation);