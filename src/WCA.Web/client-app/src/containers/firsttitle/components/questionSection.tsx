import * as React from 'react'
import { ChoiceGroup } from 'office-ui-fabric-react/lib/ChoiceGroup';

interface IProps {
    heading: string,
    keyStr: string,
    initialValue?: boolean,
    setRequestPolicyProperty: (key: string, val: boolean) => void
}

interface IState {
    selectedValue: string | undefined
}

export default class QuestionSection extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);

        this.state = {
            selectedValue: props.initialValue !== undefined ? (props.initialValue ? "Yes" : "No") : undefined
        }
    }

    public onChangeValue = (newVal: string | undefined) => {
        this.setState({
            selectedValue: newVal
        })

        this.props.setRequestPolicyProperty(this.props.keyStr, newVal === "Yes")
    }

    public render(): JSX.Element {
        const { selectedValue } = this.state;

        return (
            <div className="ms-Grid-row state-row first-title-question-wrapper">
                <div className="ms-Grid-col ms-sm12"> <span>{this.props.heading}</span> </div>
                <div className="ms-Grid-col ms-sm12">
                    <ChoiceGroup
                        selectedKey={selectedValue}
                        className="defaultChoiceGroup"
                        options={[
                            {
                                key: "Yes",
                                text: "Yes"
                            },
                            {
                                key: "No",
                                text: "No"
                            }
                        ]}
                        onChange={(ev, a) => this.onChangeValue(a ? a.key : undefined)} />
                </div>
            </div>
        )
    }
}