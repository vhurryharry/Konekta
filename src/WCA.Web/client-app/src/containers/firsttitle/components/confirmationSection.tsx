import * as React from 'react'

interface IProps {
    heading: string,
    value: string
}

export default class ConfirmationSection extends React.Component<IProps> {
    public render(): JSX.Element {
        return (
            <div className="ms-Grid-row state-row first-title-question-wrapper">
                <div className="ms-Grid-col ms-sm12">
                    <span>
                        {this.props.heading}:
                    </span>
                </div>

                <div className="ms-Grid-col ms-sm12">
                    <h3>
                        {this.props.value}
                    </h3>
                </div>
            </div>
        )
    }
}