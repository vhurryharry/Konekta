import * as React from 'react'

import { GroupedList, IGroup, IGroupHeaderProps } from 'office-ui-fabric-react/lib/GroupedList'
import { Link, ILinkStyles } from 'office-ui-fabric-react/lib/Link'
import { Icon } from 'office-ui-fabric-react/lib/Icon'

import {
    LandTitleReferenceVerificationResponseTypeWarningsWorkspace,
    LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport,
    PexaRole
} from 'utils/wcaApiTypes'

import ValidateLandTitleWorkspace from 'containers/pexa/steps/components/validateLandTitleWorkspace';

interface AppProps {
    warnings?: LandTitleReferenceVerificationResponseTypeWarningsWorkspace[] | undefined;
    landTitleReferenceReport?: LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport | undefined;
    landTitleReference: string;
    subscriberRole: PexaRole
}

interface AppStates {
    collapsedStates: boolean[];
}

export default class ValidateLandTitle extends React.Component<AppProps, AppStates> {
    constructor(props: AppProps) {
        super(props);

        this.state = {
            collapsedStates: props.warnings ? new Array<boolean>(props.warnings.length).fill(true) : []
        }
    }

    render(): JSX.Element {
        const { warnings, landTitleReferenceReport, landTitleReference } = this.props;
        const { collapsedStates } = this.state;

        let groups: IGroup[] = [];
        if (warnings) {
            groups = warnings.map((warning, index) => {
                return {
                    count: 1,
                    startIndex: index,
                    key: `${index}`,
                    name: `${warning.workspaceId}`,
                    isCollapsed: collapsedStates[index]
                };
            })
        }

        return (
            <div className="validate-land-title-result">
                <h4>
                    Land Title Reference: {landTitleReference}
                </h4>

                {landTitleReferenceReport &&
                    <h4>
                        Property Address: {landTitleReferenceReport.propertyDetails}
                    </h4>
                }

                {warnings && <GroupedList
                    items={warnings}
                    onRenderCell={this._onRenderWorkspace}
                    groupProps={{
                        onRenderHeader: this._onRenderHeader
                    }}
                    groups={groups}
                />}
            </div>
        )
    }

    private _onRenderWorkspace = (nestingDepth: number | undefined, item: LandTitleReferenceVerificationResponseTypeWarningsWorkspace, itemIndex: number | undefined): JSX.Element => {
        const { subscriberRole } = this.props;

        return (
            <ValidateLandTitleWorkspace workspace={item} subscriberRole={subscriberRole} />
        );
    }

    private _onRenderHeader = (props: IGroupHeaderProps | undefined): JSX.Element => {
        const toggleCollapse = (): void => {
            props!.onToggleCollapse!(props!.group!);

            let collapsedStates = [...this.state.collapsedStates];
            collapsedStates[props!.group!.startIndex] = !collapsedStates[props!.group!.startIndex];

            this.setState({
                collapsedStates
            });
        }

        const linkStyle: ILinkStyles = {
            root: {
                color: 'black',
                fontWeight: '600'
            }
        };

        return (
            <b>
                <Link onClick={toggleCollapse} styles={linkStyle}>
                    <Icon iconName={props!.group!.isCollapsed ? "CaretHollow" : "CaretSolid"} /> {props!.group!.name}
                </Link>
            </b>
        )
    }
}