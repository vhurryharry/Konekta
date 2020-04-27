import * as React from 'react'
import { AppInfoService } from '../services/app-info-service';

interface IAppProps {
    appInfoService: AppInfoService;
}

type AppProps = IAppProps;

export default class SettlementInvalidMatter extends React.Component<AppProps> {
    public render(): JSX.Element {
        const { appName, supportUrl } = this.props.appInfoService;

        return (
            <div className="wrapper wrapper-content animated fadeInRight">
                <div className="row">
                    <div className="col-lg-9">
                        <div className="panel panel-danger">
                            <div className="panel-heading">
                                <i className="fa fa-exclamation"></i> <span className="m-l-xs">No Matter Selected</span>
                            </div>
                            <div className="panel-body p-v-xs">
                                <p className="text-danger">
                                    Cannot load the
                                    <strong> Settlement Calculator </strong>
                                    because no matter is selected.
                                </p>
                            </div>
                        </div>

                        <div className="ibox">
                            <div className="ibox-title">
                                <h2>Help! What do I do now?</h2>
                            </div>
                            <div className="ibox-content">
                                <p>
                                    Please click the integrations tab, then click Access Integrations
                                </p>

                                <div className="text-center">
                                    <img src="/images/actionstep-integrations-tab.png" width="90%"
                                        alt={"Actionstep - " + appName + " Integrations Tab"} />
                                </div>

                                <p>
                                    This <strong> Integrations </strong> tab also provides you access to a <strong> new </strong>
                                    method to order InfoTrack searches.
                                </p>

                            </div>
                        </div>

                        <div className="panel panel-warning">
                            <div className="panel-heading">
                                <i className="fa fa-question"></i> <span className="m-l-xs">But I don't have the Integrations tab</span>
                            </div>
                            <div className="panel-body p-v-xs">
                                <p>
                                    Please
                                    <a href={supportUrl + "/support/tickets/new"} target="_blank"> log a support ticket </a>
                                    and our team will set this up for you.
                                </p>
                            </div>
                        </div>

                    </div>

                    <div className="col-lg-3">

                        <div className="panel panel-info">
                            <div className="panel-heading">
                                <i className="fa fa-info"></i>
                                <span className="m-l-xs"><em>New</em> &nbsp;InfoTrack Integration Available Now</span>
                            </div>
                            <div className="ibox-content">
                                <p>
                                    This new integration automatically prepopulates InfoTrack with
                                    <strong> over 90 fields </strong> from your matter data, saving double-entry and eliminating                                                                                                                                    typos.
                                </p>
                                <p>
                                    <a href={supportUrl + "/solution/articles/6000200120-ordering-infotrack-searches-via-the-workcloud-conveyancing-app"}>
                                        Learn more
                                    </a>
                                </p>
                                <p>
                                    <a href={supportUrl + "/solution/articles/6000217510-add-your-infotrack-username-and-password"}>
                                        Set up now
                                    </a>
                                </p>

                                <div className="text-center">
                                    <a href="">
                                        <img src="/images/InfoTrackLogo_216x80.png" alt="InfoTrack Logo" />
                                    </a>
                                </div>

                            </div>
                        </div>

                        <div className="panel panel-info">
                            <div className="panel-heading">
                                <i className="fa fa-info"></i> <span className="m-l-xs">PEXA Coming Soon</span>
                            </div>
                            <div className="panel-body">
                                <p>
                                    Soon you will be able to create a PEXA workspace using the
                                    information already stored in your Actionstep matter.
                                </p>
                                <p>
                                    You can
                                    <a href={supportUrl + "/support/solutions/articles/6000222358-pexa-integration-coming-soon"}>
                                        &nbsp;learn more about the coming PEXA Integration with {appName} Conveyancing here.
                                    </a>
                                </p>

                                <div className="text-center">
                                    <a href="https://www.pexa.com.au/">
                                        <img src="/images/pexa-logo.svg" alt="PEXA Logo" />
                                    </a>
                                    <p className="caption">PEXA Integration Coming Soon</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

//max-width: 180px; width:100%