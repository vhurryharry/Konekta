
import * as React from 'react'
import { connect } from 'react-redux';
import { withRouter, RouteComponentProps } from 'react-router';

import { Link } from 'office-ui-fabric-react/lib/Link'

import "./footer.css"
import { AppState } from 'app.types';
import { UISettings } from 'utils/wcaApiTypes';

interface IMapStateToProps {
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

const footerDisabledUrls = ['/globalx/property-information'];

export class Footer extends React.Component<AppProps> {
    render(): JSX.Element | null {
        const { uiDefinitions, location } = this.props;

        if (footerDisabledUrls.indexOf(location.pathname) >= 0)
            return null;

        const yearFrom = uiDefinitions ? uiDefinitions.yearWcaIncorporated : 2017;
        const yearTo = new Date().getUTCFullYear();

        return (
            <div className="konekta-footer">
                <div className="konekta-footer-text">
                    <p>
                        &copy; <Link href="https://www.konekta.com.au">Konekta</Link>&nbsp;
                        {yearFrom}-{yearTo}&nbsp;
                        <Link href="https://support.konekta.com.au/support/solutions/articles/6000229543-terms-conditions" target="_blank">
                            Terms &amp; Conditions
                        </Link>
                    </p>
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state: AppState) => {
    return {
        uiDefinitions: state.common.uiDefinitions
    }
}

export default withRouter(connect(mapStateToProps)(Footer));