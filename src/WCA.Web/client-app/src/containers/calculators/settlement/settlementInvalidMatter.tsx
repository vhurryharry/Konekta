
import * as React from 'react'

import { MessageBar, MessageBarType, IMessageBarStyles } from 'office-ui-fabric-react';
import { Link } from 'office-ui-fabric-react/lib/Link'

interface IAppProps { }

type AppProps = IAppProps;

const messageBarStyles: IMessageBarStyles = {
    root: {
        margin: "10px"
    }
}

export default class SettlementInvalidMatter extends React.Component<AppProps> {

    public render(): JSX.Element {

        return (
            <div className="wrapper wrapper-content animated fadeInRight">
                <div className="ms-Grid-row">
                    <div className="ms-Grid-col ms-sm9">

                        <MessageBar messageBarType={MessageBarType.error} styles={messageBarStyles}>
                            <h4>No Matter Selected</h4>
                            Cannot load the
                            <strong> Settlement Calculator </strong>
                            because no matter is selected.
                        </MessageBar>

                        <MessageBar messageBarType={MessageBarType.info} styles={messageBarStyles}>
                            <h3>Help! What do I do now?</h3>

                            <p>
                                Please click the integrations tab, then click Access Integrations
                            </p>

                            <div className="text-center">
                                <img src="/images/actionstep-integrations-tab.png" width="90%"
                                    alt={"Actionstep - Konekta Integrations Tab"} />
                            </div>

                            <p>
                                This <strong> Integrations </strong> tab also provides you access to a <strong> new </strong>
                                method to order InfoTrack searches.
                            </p>
                        </MessageBar>

                        <MessageBar messageBarType={MessageBarType.warning} styles={messageBarStyles}>
                            <h4>But I don't have the Integrations tab</h4>
                            Please
                            <Link href={"https://support.konekta.com.au/support/tickets/new"} target="_blank" rel="noopener noreferrer"> log a support ticket </Link>
                            and our team will set this up for you.
                        </MessageBar>

                    </div>

                    <div className="ms-Grid-col ms-sm3">

                        <MessageBar messageBarType={MessageBarType.info} styles={messageBarStyles}>
                            <h4><em>New</em> &nbsp;InfoTrack Integration Available Now</h4>

                            <p>
                                This new integration automatically prepopulates InfoTrack with
                                <strong> over 90 fields </strong> from your matter data, saving double-entry and eliminating typos.
                            </p>
                            <p>
                                <Link href={"https://support.konekta.com.au/solution/articles/6000200120-ordering-infotrack-searches-via-the-workcloud-conveyancing-app"}>
                                    Learn more
                                </Link>
                            </p>
                            <p>
                                <Link href={"https://support.konekta.com.au/solution/articles/6000217510-add-your-infotrack-username-and-password"}>
                                    Set up now
                                </Link>
                            </p>

                            <div className="text-center">
                                <Link href="https://www.infotrack.com.au/">
                                    <img src="/images/InfoTrackLogo_216x80.png" alt="InfoTrack Logo" width="170px" />
                                </Link>
                            </div>
                        </MessageBar>

                        <MessageBar messageBarType={MessageBarType.info} styles={messageBarStyles}>
                            <h4>PEXA Coming Soon</h4>
                            <p>
                                Soon you will be able to create a PEXA workspace using the
                                information already stored in your Actionstep matter.
                            </p>
                            <p>
                                You can
                                <Link href={"https://support.konekta.com.au/support/solutions/articles/6000222358-pexa-integration-coming-soon"}>
                                    &nbsp;learn more about the coming PEXA Integration with Konekta Conveyancing here.
                                </Link>
                            </p>

                            <div className="text-center">
                                <Link href="https://www.pexa.com.au/">
                                    <img src="/images/pexa-logo.svg" alt="PEXA Logo" width="170px" />
                                </Link>
                                <p className="caption">PEXA Integration Coming Soon</p>
                            </div>
                        </MessageBar>
                    </div>
                </div>
            </div>
        );
    }
}

//max-width: 180px; width:100%