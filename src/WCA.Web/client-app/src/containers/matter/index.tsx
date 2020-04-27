
import * as React from 'react'
import { connect } from 'react-redux';

import { AppState, JwtMatterInfo } from 'app.types';

import { getRequest } from '../../utils/request'

import "./index.css"
import { Card } from '@uifabric/react-cards';
import { Link, Text, Image } from 'office-ui-fabric-react';
import { TooltipHost } from 'office-ui-fabric-react/lib/Tooltip';
import { getId } from 'office-ui-fabric-react/lib/Utilities';
import { Link as ReactLink } from 'react-router-dom';
import { IIntegration } from 'utils/wcaApiTypes';
import LoadingWidget from 'components/common/loadingWidget';

interface IMapStateToProps {
    jwtMatterInfo: JwtMatterInfo | undefined;
}

interface IMapDispatchToProps { }

interface IAppProps { }

type AppProps = IAppProps & IMapDispatchToProps & IMapStateToProps;

type AppStates = {
    matterId: number;
    actionstepOrg: string;
    integrations: IIntegration[];
    integrationsRetrieved: boolean;
}

export class MatterPage extends React.Component<AppProps, AppStates> {
    private _hostId: string = getId('tooltipHost');

    constructor(props: AppProps) {
        super(props);

        const jwtMatterInfo: JwtMatterInfo = props.jwtMatterInfo!;

        this.state = {
            matterId: jwtMatterInfo.matterId,
            actionstepOrg: jwtMatterInfo.orgKey,
            integrations: [],
            integrationsRetrieved: false,
        }
    }

    public async componentDidMount(): Promise<void> {
        const integrations = (await getRequest(`/api/integrations/integration-links/${this.state.actionstepOrg}/${this.state.matterId}`)) as IIntegration[];

        this.setState({
            integrations: integrations,
            integrationsRetrieved: true,
        });
    }

    render(): JSX.Element {
        const { integrations, integrationsRetrieved } = this.state;

        return (
            <div className="matter-page">
                <div className="ms-Grid-row parter-cards-wrapper">
                    {!integrationsRetrieved ?
                        <LoadingWidget message="Retrieving your integrations..." />
                        :
                        (integrations.length < 1) ?
                            <div>No integrations were found for your account.</div>
                            :
                            integrations.map(integration => (
                                <div className="ms-Grid-col ms-sm4" key={integration.title}>
                                    <Card className="partner-card">
                                        <Card.Item className="partner-logo">
                                            <Link href={integration.logo?.href ?? '#'} target="_blank">
                                                <Image src={integration.logo?.src} alt={integration.logo?.alt} title={integration.title} width={integration.logo?.width} />
                                            </Link>
                                        </Card.Item>
                                        <Card.Section className="partner-integrations">
                                            {integration.links && integration.links.length > 0 ?
                                                integration.links?.map(link => (
                                                        <TooltipHost
                                                            content={link.toolTip}
                                                            id={this._hostId}
                                                            calloutProps={{ gapSpace: 0 }}
                                                            styles={{ root: { display: 'inline-block', textAlign: 'center' } }}
                                                            key={link.title}>
                                                            {link.disabled ?
                                                                <Link href="#" disabled aria-describedby={this._hostId}><Text variant="medium">{link.title}{MatterPage.getBetaTag(link.isBeta)}</Text></Link>
                                                                :
                                                                link.isReactLink ?
                                                                    <ReactLink to={this.parseHref(link.href)} target={MatterPage.parseTarget(link.openInNewWindow)} aria-describedby={this._hostId}>
                                                                        <Text variant="medium">{link.title}{MatterPage.getBetaTag(link.isBeta)}</Text>
                                                                    </ReactLink>
                                                                    :
                                                                    <Link href={this.parseHref(link.href)} target={MatterPage.parseTarget(link.openInNewWindow)} aria-describedby={this._hostId}>
                                                                        <Text variant="medium">{link.title}{MatterPage.getBetaTag(link.isBeta)}</Text>
                                                                    </Link>
                                                            }
                                                        </TooltipHost>
                                                    ))
                                                :
                                                integration.comingSoon ?
                                                    <Link href="#" disabled aria-describedby={this._hostId}><Text variant="medium">Coming Soon</Text></Link>
                                                    :
                                                    <Link href="#" disabled aria-describedby={this._hostId}><Text variant="medium">Not currently available</Text></Link>
                                            }
                                        </Card.Section>
                                    </Card>
                                </div>
                            ))
                    }
                </div>
            </div>
        )
    }

    private parseHref(href: string | undefined): string {
        return href?.replace('{actionstepOrg}', this.state.actionstepOrg)?.replace('{matterId}', this.state.matterId.toString()) ?? '#';
    }

    private static parseTarget(openInNewWindow: boolean) {
        return openInNewWindow ? '_blank' : '_self';
    }

    private static getBetaTag(isBeta: boolean) {
        return <sup>{isBeta ? 'beta' : ''}</sup> ?? '';
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        jwtMatterInfo: state.common.jwtMatterInfo
    }
}

export default connect(mapStateToProps)(MatterPage);