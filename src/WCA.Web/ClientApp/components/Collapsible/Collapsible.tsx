import * as React from 'react'
import './Collapsible.css'
import { initializeIcons } from 'office-ui-fabric-react/lib/Icons';
import { Icon } from 'office-ui-fabric-react/lib/Icon';

export interface ICollapsibleProps {
    /**
     * Title for item header
     */
    headerTitle?: string;

    /**
     * Optional function to render header title
     */
    renderHeaderTitle?: (props: ICollapsibleProps) => JSX.Element;

    /**
     * Optional custom style for the header
     */
    customHeaderStyle?: React.CSSProperties;

    /**
     * Optional custom icons
     */
    customExpandIcon?: string;
    customCollapseIcon?: string;

    /**
     * Optional parameter to expand the list by default
     */
    expanded?: boolean;
}


export class Collapsible extends React.Component<ICollapsibleProps> {

    state = {
        showContent: this.props.expanded ? true : false
    }

    public constructor(props) {
        super(props);

        initializeIcons();
    }

    public render(): JSX.Element {
        const { showContent } = this.state;

        return (
            <div className="collapsible">
                {this._renderHeader()}

                {showContent &&
                    <div className="ms-Grid-row animated fadeIn collapsible-content">
                        {this.props.children}
                    </div>
                }
            </div>
        )
    }

    private _renderHeader = (): JSX.Element => {
        const { showContent } = this.state;
        const { headerTitle, renderHeaderTitle, customHeaderStyle, customExpandIcon, customCollapseIcon } = this.props;
        const expandIcon = customExpandIcon ? customExpandIcon : "ChevronDown";
        const collapseIcon = customCollapseIcon ? customCollapseIcon : "ChevronUp";

        return (
            <button className="btn btn-primary collapsible-header" onClick={() => this._toggleContent()} style={customHeaderStyle}>
                {headerTitle ? headerTitle : renderHeaderTitle(this.props)}

                <Icon iconName={showContent ? collapseIcon : expandIcon} className="collapsible-header-icon" />
            </button>
        );
    }

    private _toggleContent = (): void => {
        this.setState({
            showContent: !this.state.showContent
        })
    }
}