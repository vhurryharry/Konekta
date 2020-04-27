import * as React from 'react'
import { connect } from 'react-redux';
import { withRouter, RouteComponentProps } from 'react-router';

import { AppState } from 'app.types';
import { AccountModel, UISettings } from 'utils/wcaApiTypes';

import { Icon } from 'office-ui-fabric-react/lib/Icon'
import { Link } from 'office-ui-fabric-react/lib/Link'

import "./navHeader.css"
import { Label, Breadcrumb, IBreadcrumbItem } from 'office-ui-fabric-react';
import breadcrumbItems, { breadcrumbStyle, breadcrumbHeaderStyle, onRenderItem } from './breadcrumbItems';

interface IMapStateToProps {
    accountInfo: AccountModel | undefined;
    uiDefinitions: UISettings | undefined;
}

interface IMapDispatchToProps {
}

interface IAppProps {
}

// Your component own properties
type PropsType = RouteComponentProps<IAppProps> & {
}

type AppProps = PropsType & IMapStateToProps & IMapDispatchToProps;

export class NavHeader extends React.Component<AppProps> {

    private insideIframe: boolean;

    constructor(props: AppProps) {
        super(props);

        try {
            if (window.self !== window.top)
                this.insideIframe = true;
            else
                this.insideIframe = false;
        } catch (e) {
            this.insideIframe = true;
        }
    }

    public render(): JSX.Element {
        const { accountInfo, uiDefinitions, location } = this.props;
        const userName = accountInfo ? (accountInfo.firstName + " " + accountInfo.lastName) : null;

        const insideIframe = this.insideIframe;
        const actionstepUrl = uiDefinitions ? uiDefinitions.backToActionstepURL : "";

        if (accountInfo && accountInfo.isLoggedIn) {
            return (
                <div className="ms-Grid" dir="ltr">
                    {!insideIframe &&
                        <div className="ms-Grid-row" id="top-frameless-nav">
                            <div className="ms-Grid-col ms-lg2">
                                <Link href={actionstepUrl}> Back to Actionstep</Link>
                            </div>

                            <div className="ms-Grid-col ms-lg10 align-right">
                                <Link href="#">{userName}</Link>

                                <Link href="/Identity/Account/Logout">Log out</Link>
                            </div>
                        </div>
                    }

                    <div className="ms-Grid-row" id="page-header">
                        <div className="ms-Grid-col ms-lg8">
                            <Label styles={breadcrumbHeaderStyle}>Konekta Integrations</Label>
                            <Breadcrumb
                                items={breadcrumbItems(location.pathname)}
                                styles={breadcrumbStyle}
                                maxDisplayedItems={5}
                                className="ms-Grid-col ms-sm10"
                                onRenderItem={(item: IBreadcrumbItem | undefined) => onRenderItem(item)}
                            />
                        </div>

                        <div className="ms-Grid-col ms-lg4" id="top-nav-links">
                            <div>
                                <Link href="https://support.konekta.com.au" target="_blank">
                                    <Icon iconName="Help" aria-hidden="true" /> Help
                                </Link>
                                <Link href="/wca/integrations" target="_blank">
                                    <Icon iconName="Settings" aria-hidden="true" /> Settings
                                </Link>
                            </div>
                            <Link href="https://www.konekta.com.au" target="_blank">
                                <img src="/images/Konekta_powered by.svg" alt="Powered by Konekta" height="40" />
                            </Link>
                        </div>
                    </div>
                </div>
            );
        }

        return (
            <div></div>
        );
    }
}

const mapStateToProps = (state: AppState) => {
    return {
        accountInfo: state.common.accountInfo,
        uiDefinitions: state.common.uiDefinitions
    }
}

export default withRouter(connect(mapStateToProps)(NavHeader));