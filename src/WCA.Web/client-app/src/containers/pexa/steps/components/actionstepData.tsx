import * as React from 'react'

import { MessageBar, IMessageBarStyles } from 'office-ui-fabric-react/lib/MessageBar';
import { TooltipHost, TooltipDelay, DirectionalHint } from 'office-ui-fabric-react/lib/Tooltip';

export default class ActionstepData extends React.Component {
    render(): JSX.Element {
        const messageBarStyle: IMessageBarStyles = {
            root: {
                background: 'rgba(113, 175, 229, 0.2)'
            }
        };

        return (
            <TooltipHost
                tooltipProps={{
                    onRenderContent: () => {
                        return (
                            <div>
                                We've tried our best to fill in the road details from the address stored in Actionstep's <b>Line 1</b> and <b>Line 2</b> fields.
                                Because the fields don't match up with what PEXA expects, please double check that the road information is correct.
                                <br /><br />
                                For your reference, this box shows the address as it is stored in Actionstep.
                            </div>
                        );
                    }
                }}
                delay={TooltipDelay.zero}
                directionalHint={DirectionalHint.topAutoEdge}
            >
                <MessageBar styles={messageBarStyle}>
                    {this.props.children}
                </MessageBar>
            </TooltipHost>
        )
    }
}