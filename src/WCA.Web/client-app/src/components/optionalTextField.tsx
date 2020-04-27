import * as React from 'react'
import { TextField, IconButton } from 'office-ui-fabric-react';

interface IAppProps {
    className?: string;
    canRemove?: boolean;
    value?: string;
    onChange: (event: React.FormEvent, newValue: string | undefined) => void;
    onRemove?: () => void;
}

type AppProps = IAppProps;

export default class OptionalTextField extends React.Component<AppProps> {
    render() {
        const { className, canRemove, value, onChange, onRemove } = this.props;

        return (
            <div className={className + " ms-Grid-row"}>
                {!canRemove ?
                    <TextField
                        className="ms-Grid-col ms-sm12"
                        placeholder="Another given name..."
                        value={value}
                        onChange={onChange}
                    />
                    :
                    <>
                        <TextField className="ms-Grid-col ms-sm11"
                            value={value}
                            onChange={onChange}
                        />
                        <IconButton iconProps={{ iconName: 'Cancel' }}
                            className="ms-Grid-col ms-sm1"
                            onClick={onRemove}
                        />
                    </>
                }
            </div>
        )
    }
}