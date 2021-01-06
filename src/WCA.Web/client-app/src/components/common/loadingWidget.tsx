import * as React from 'react'

type AppState = {
    message: string,
}

type AppProps = {
    message?: string,
}

export default class LoadingWidget extends React.Component<AppProps, AppState> {
    constructor(props: AppProps) {
        super(props);

        this.state = {
            message: props.message ? props.message : 'Loading...'
        }
    }

    public render(): JSX.Element {
        return (
            <div className="loading-widget">
                <img src="/images/Konekta_loading.svg" alt="Logo" height="150" /> {this.state.message}
            </div>
        )
    }
}