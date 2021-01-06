import * as React from 'react';
import { Route, Router, Switch } from 'react-router-dom'
import { connect } from 'react-redux';
import history from 'utils/history'
import { initializeIcons } from '@uifabric/icons'

import { AccountModel, UISettings, ActionstepMatterInfo } from 'utils/wcaApiTypes';
import { AppState, JwtMatterInfo, IJWTInfo } from 'app.types';
import {
	getAccountInfo,
	setUIDefinitions,
	setJwtMatterInfo,
	getMatterInfo,
	actionstepOrgConnected,
	logAuthFailure,
	clearCommonState
} from 'redux/common/actions'
import * as CONSTANTS from 'redux/common/constants'
import { getUIDefinitions } from 'utils/commonInfo';

import HomePage from 'containers/home/index'
import RequestPolicy from 'containers/firsttitle/requestPolicy'
import CreatePexaWorkspace from 'containers/pexa/createWorkspace'
import SettlementCalculator from 'containers/calculators/settlement/settlementCalculator';
import MatterPage from 'containers/matter';
import GXPropertyInformation from 'containers/globalx/pages/gxPropertyInformation'

import ConnectToActionstep from 'components/connectToActionstep/connectToActionstep';
import NavHeader from 'components/layout/navHeader'
import Footer from 'components/layout/footer'
import InvalidAccess from 'components/common/invalidAccess'
import AuthFailed from 'components/common/authFailed'
import LoadingWidget from 'components/common/loadingWidget'

import Tools from 'utils/tools';

import "./App.css"

interface IMapStateToProps {
	jwtMatterInfo: JwtMatterInfo | undefined;
	matterInfo: ActionstepMatterInfo | undefined;
	accountInfo: AccountModel | undefined;
	uiDefinitions: UISettings | undefined;
	success: boolean;
	gotResponse: boolean;
	requestType: string;
	orgConnected: boolean | null;
}

interface IMapDispatchToProps {
	setJwtMatterInfo: (data: JwtMatterInfo) => void;
	getMatterInfo: (jwtMatterInfo: JwtMatterInfo) => void;
	getAccountInfo: (encodedJwt: string | null) => void;
	setUIDefinitions: (uiDefinitions: UISettings) => void;
	actionstepOrgConnected: () => void;
	logAuthFailure: (encodedJwt: string | null) => void;
	clearCommonState: () => void;
}

interface IAppProps {
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

type AppStates = {
	orgConnected: boolean | null,
	isValidAccess: boolean
	jwt: string | null;
	retriedAuth: boolean;
	authFailed: boolean | null;
}

export class App extends React.Component<AppProps, AppStates> {

	constructor(props: AppProps) {
		super(props);

		const urlParams = new URLSearchParams(window.location.search);
		const encodedJwt = urlParams.get('jwt');

		// Always refresh account info when the app is loaded, in case the cookie/persisted state info is stale.
		this.props.getAccountInfo(encodedJwt);

		initializeIcons();

		this.state = {
			jwt: encodedJwt,
			retriedAuth: false,
			authFailed: null,
			orgConnected: null,
			isValidAccess: true
		}
	}

	public async componentDidMount(): Promise<void> {
		if (this.props.uiDefinitions === undefined) {
			const uiDefinitions = await getUIDefinitions();
			this.props.setUIDefinitions(uiDefinitions);
		}

		this.initMatterInfo();
	}

	public initMatterInfo(): void {
		const jwt = require('jsonwebtoken');
		const urlParams = new URLSearchParams(window.location.search);

		let jwtMatterInfo: JwtMatterInfo | null = null;

		// The JWT will be set by Actionstep, so we can be confident enough that it'll always be lower case.
		// This isn't like actionstepOrg or matterId where a user/admin might be setting the URL params.
		const encodedJwt = urlParams.get('jwt');

		if (encodedJwt) {
			const jwtInfo: IJWTInfo = jwt.decode(encodedJwt) as IJWTInfo;

			jwtMatterInfo = {
				orgKey: jwtInfo.orgkey,
				matterId: jwtInfo.action_id,
				actionTypeId: jwtInfo.action_type_id,
				actionTypeName: jwtInfo.action_type_name,
				timezone: jwtInfo.timezone
			};

			this.props.setJwtMatterInfo(jwtMatterInfo);
		} else {
			if (this.props.jwtMatterInfo) {
				jwtMatterInfo = this.props.jwtMatterInfo;
			} else {

				//This is only for legacy access mode

				const queryString = require('query-string');

				const urlParams = queryString.parse(window.location.search);

				const matterInfo = Tools.ParseActionstepMatterInfo(urlParams);

				if (matterInfo) {
					jwtMatterInfo = {
						orgKey: matterInfo.orgKey!,
						matterId: matterInfo.matterId!,
						actionTypeId: 0,
						actionTypeName: "",
						timezone: ""
					};

					this.props.setJwtMatterInfo(jwtMatterInfo);
				}
			}
		}

		if (jwtMatterInfo != null) {
			this.props.getMatterInfo(jwtMatterInfo);
		} else {
			this.setState({
				isValidAccess: false
			})
		}
	}

	public navigateToLogin = (): void => {
		let insideIframe: boolean = false;

		try {
			if (window.self !== window.top)
				insideIframe = true;
			else
				insideIframe = false;
		} catch (e) {
			insideIframe = true;
		}

		if (insideIframe) {
			// The app is loaded inside the AS UI, so retry auth and log the error
			if (this.state.jwt) {
				if (this.state.retriedAuth) {
					// Something went wrong - need to log this incident
					this.props.logAuthFailure(this.state.jwt);

					this.setState({
						authFailed: true
					})

				} else {
					// Retry auth in case the jwt is provided
					this.props.getAccountInfo(this.state.jwt);

					this.setState({
						retriedAuth: true
					});

					return;
				}
			}
		} else {
			const returnUrlParam = 'returnurl=' + encodeURIComponent(window.location.href);
			const loginUrl = "/Identity/Account/Login?" + returnUrlParam;

			window.location.href = loginUrl;
		}
	}

	public componentDidUpdate(nextProps: AppProps): void {
		if (nextProps.gotResponse) {
			switch (nextProps.requestType) {
				case CONSTANTS.GET_ACCOUNT_INFO_SUCCESS:
				case CONSTANTS.GET_ACCOUNT_INFO_FAILED:
					this.props.clearCommonState();

					if (nextProps.success === true) {
						if (!(nextProps.accountInfo && nextProps.accountInfo.isLoggedIn === true)) {
							this.navigateToLogin();
						}
					}
					break;

				case CONSTANTS.GET_MATTER_INFO_SUCCESS:
				case CONSTANTS.GET_MATTER_INFO_FAILED:
					this.props.clearCommonState();
					break;

				default:
					break;
			}
		}
	}

	static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
		let nextState = {} as AppStates;

		if (!nextProps.orgConnected || (nextProps.orgConnected && nextProps.matterInfo !== undefined))
			nextState.orgConnected = nextProps.orgConnected;

		return nextState;
	}

	public connectedToActionstep = (): void => {
		this.props.actionstepOrgConnected();

		this.initMatterInfo();

		this.setState({
			orgConnected: null
		});
	}

	render() {
		const { accountInfo } = this.props;
		const { orgConnected, isValidAccess, authFailed } = this.state;

		if (accountInfo && accountInfo.isLoggedIn)
			return (
				<div className="white-bg" id="wrapper">

					<Router history={history}>

						<NavHeader />

						{!isValidAccess ?
							<InvalidAccess />
							:
							orgConnected === true ?
								<Switch>
									<Route path="/pexa/create-workspace" component={CreatePexaWorkspace} />
									<Route path="/calculators/settlement" component={SettlementCalculator} />
									<Route path="/firsttitle/request-policy" component={RequestPolicy} />
									<Route path="/matter" component={MatterPage} />
									<Route path="/globalx/property-information" component={GXPropertyInformation} />
									<Route path="/" exact component={HomePage} />
								</Switch>
								:
								orgConnected === null ?
									<LoadingWidget />
									:
									<ConnectToActionstep callback={() => this.connectedToActionstep()} />
						}

						<Footer />

					</Router>

				</div>
			);

		if (authFailed === true)
			return <AuthFailed />;

		return <LoadingWidget />;
	}
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
	return {
		jwtMatterInfo: state.common.jwtMatterInfo,
		matterInfo: state.common.matterInfo,
		accountInfo: state.common.accountInfo,
		uiDefinitions: state.common.uiDefinitions,
		success: state.common.success,
		gotResponse: state.common.gotResponse,
		requestType: state.common.requestType,
		orgConnected: state.common.orgConnected
	}
}

const mapDispatchToProps: IMapDispatchToProps = {
	getMatterInfo,
	setJwtMatterInfo,
	getAccountInfo,
	logAuthFailure,
	setUIDefinitions,
	actionstepOrgConnected,
	clearCommonState
}

export default connect(mapStateToProps, mapDispatchToProps)(App);