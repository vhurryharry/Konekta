import * as React from 'react'

import { FeatureFlag } from '../utils/wcaApiTypes';


interface IComponentProps {
    enabledFlags: FeatureFlag[] | undefined;
    flagToCheck: FeatureFlag;
    childrenIfDisabled: React.ReactNode | undefined;
}

interface IComponentState {
    actualChildren: React.ReactNode;
}

type ComponentProps = IComponentProps;
type ComponentState = IComponentState;

export default class FeatureFlagged extends React.Component<ComponentProps, ComponentState> {

    constructor(props: IComponentProps) {
        super(props);

        this.state = {
            actualChildren: this.props.enabledFlags && this.props.enabledFlags.indexOf(this.props.flagToCheck) > -1
                ? this.props.children
                : this.props.childrenIfDisabled
        }
    }

    public render(): JSX.Element {
        return <>{this.state.actualChildren}</>;
    }
}